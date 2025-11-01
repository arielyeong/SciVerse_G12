using Microsoft.Ajax.Utilities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Admin
{
    public partial class NewQuestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // --- Session check (RID required) ---
            if (Session["RID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // --- quizId required ---
            var quizIdStr = Request.QueryString["quizId"];
            if (string.IsNullOrEmpty(quizIdStr))
            {
                Response.Redirect("~/Quiz_Admin/ViewQuizList.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            hiddenQuizID.Value = quizIdStr;
            linkBack.NavigateUrl = ResolveUrl("~/Quiz_Admin/EditQuizPage.aspx?quizId=" + quizIdStr);

            if (!IsPostBack)
            {
                // Populate type dropdown
                dropdownType.Items.Clear();
                dropdownType.Items.Add(new ListItem("--- select ---", ""));
                dropdownType.Items.Add(new ListItem("Multiple Choice (MCQ)", "MCQ"));
                dropdownType.Items.Add(new ListItem("True / False", "TrueFalse"));
                dropdownType.Items.Add(new ListItem("Fill in the blank", "FillInBlanks"));

                // Load for edit (only once)
                if (int.TryParse(Request.QueryString["questionId"], out var qid) && qid > 0)
                {
                    LoadQuestionForEdit(qid);
                    ViewState["EditingQuestionId"] = qid;
                }

                TogglePanelsAndValidators();
            }
            else
            {
                // Keep UI/validators in sync after postback
                TogglePanelsAndValidators();
            }
        }

        // Toggle visible panels and enable only relevant validators
        private void TogglePanelsAndValidators()
        {
            var val = dropdownType.SelectedValue ?? "";

            panelMCQ.Visible = (val == "MCQ");
            panelTF.Visible = (val == "TrueFalse");
            panelFill.Visible = (val == "FillInBlanks");

            bool isMcq = (val == "MCQ");
            bool isTf = (val == "TrueFalse");
            bool isFiB = (val == "FillInBlanks");

            // These validator IDs come from your .aspx
            rfvA.Enabled = isMcq;
            rfvB.Enabled = isMcq;
            rfvC.Enabled = isMcq;
            rfvD.Enabled = isMcq;
            cvMcqCorrect.Enabled = isMcq;

            rfvTF.Enabled = isTf;
            rfvFill.Enabled = isFiB;
            // Always-on validators
            rfvExplanation.Enabled = true;
        }

        protected void dropdownType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TogglePanelsAndValidators();
        }

        // FIRST-LOAD ONLY: prefill fields when editing
        private void LoadQuestionForEdit(int questionId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // Question fields
                using (var cmd = new SqlCommand(@"
                    SELECT QuestionText, QuestionType, Marks, Explanation
                    FROM dbo.tblQuestion
                    WHERE QuestionID=@id;", con))
                {
                    cmd.Parameters.AddWithValue("@id", questionId);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            txtQuestion.Text = Convert.ToString(r["QuestionText"]);
                            dropdownType.SelectedValue = Convert.ToString(r["QuestionType"]);
                            txtMarks.Text = Convert.ToString(r["Marks"]);
                            txtExplanation.Text = Convert.ToString(r["Explanation"]);
                        }
                    }
                }

                // Options
                using (var cmdOpt = new SqlCommand(@"
                    SELECT OptionText, IsCorrect
                    FROM dbo.tblOptions
                    WHERE QuestionID=@id
                    ORDER BY OptionID;", con))
                {
                    cmdOpt.Parameters.AddWithValue("@id", questionId);
                    using (var rr = cmdOpt.ExecuteReader())
                    {
                        if (dropdownType.SelectedValue == "MCQ")
                        {
                            string[] texts = new string[4];
                            bool[] flags = new bool[4];
                            int i = 0;
                            while (rr.Read() && i < 4)
                            {
                                texts[i] = Convert.ToString(rr["OptionText"]);
                                flags[i] = Convert.ToBoolean(rr["IsCorrect"]);
                                i++;
                            }
                            txtChoiceA.Text = i > 0 ? texts[0] : "";
                            txtChoiceB.Text = i > 1 ? texts[1] : "";
                            txtChoiceC.Text = i > 2 ? texts[2] : "";
                            txtChoiceD.Text = i > 3 ? texts[3] : "";

                            radiobtnChoice1.Checked = (flags.Length > 0 && flags[0]);
                            radiobtnChoice2.Checked = (flags.Length > 1 && flags[1]);
                            radiobtnChoice3.Checked = (flags.Length > 2 && flags[2]);
                            radiobtnChoice4.Checked = (flags.Length > 3 && flags[3]);
                        }
                        else if (dropdownType.SelectedValue == "TrueFalse")
                        {
                            bool isTrue = false, isFalse = false;
                            while (rr.Read())
                            {
                                var text = Convert.ToString(rr["OptionText"]);
                                var ok = Convert.ToBoolean(rr["IsCorrect"]);
                                if (text.Equals("True", StringComparison.OrdinalIgnoreCase)) isTrue = ok;
                                if (text.Equals("False", StringComparison.OrdinalIgnoreCase)) isFalse = ok;
                            }
                            radiobtnTF.ClearSelection();
                            if (isTrue) radiobtnTF.SelectedValue = "True";
                            if (isFalse) radiobtnTF.SelectedValue = "False";
                        }
                        else if (dropdownType.SelectedValue == "FillInBlanks")
                        {
                            if (rr.Read())
                                txtFillInBlank.Text = Convert.ToString(rr["OptionText"]);
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Stop if any validator in group "Q" failed
            Page.Validate("Q");
            if (!Page.IsValid)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please correct the errors above.";
                return;
            }

            // parse quiz id
            if (!int.TryParse(hiddenQuizID.Value, out var quizId) || quizId <= 0)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Missing or invalid quiz id.";
                return;
            }

            // gather fields
            string qText = (txtQuestion.Text ?? "").Trim();
            string qType = dropdownType.SelectedValue; // "MCQ" | "TrueFalse" | "FillInBlanks"
            int marks = int.TryParse((txtMarks.Text ?? "").Trim(), out var m) ? m : 0;
            string explanation = (txtExplanation.Text ?? "").Trim();

            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (var con = new SqlConnection(cs))
                {
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        int questionId;
                        bool isEditing = ViewState["EditingQuestionId"] != null;

                        if (isEditing)
                        {
                            questionId = (int)ViewState["EditingQuestionId"];

                            // Update question row
                            using (var up = new SqlCommand(@"
                                UPDATE dbo.tblQuestion
                                SET QuestionText=@qt, QuestionType=@ty, Marks=@mk, Explanation=@ex
                                WHERE QuestionID=@id;", con, tx))
                            {
                                up.Parameters.AddWithValue("@qt", qText);
                                up.Parameters.AddWithValue("@ty", qType);
                                up.Parameters.AddWithValue("@mk", marks);
                                up.Parameters.AddWithValue("@ex", (object)explanation ?? DBNull.Value);
                                up.Parameters.AddWithValue("@id", questionId);
                                up.ExecuteNonQuery();
                            }

                            // Delete old options; we'll re-insert below
                            using (var del = new SqlCommand("DELETE FROM dbo.tblOptions WHERE QuestionID=@id;", con, tx))
                            {
                                del.Parameters.AddWithValue("@id", questionId);
                                del.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Insert question
                            using (var ins = new SqlCommand(@"
                                INSERT INTO dbo.tblQuestion
                                  (QuizID, QuestionText, QuestionType, Marks, Explanation)
                                OUTPUT INSERTED.QuestionID
                                VALUES (@qid, @qt, @ty, @mk, @ex);", con, tx))
                            {
                                ins.Parameters.AddWithValue("@qid", quizId);
                                ins.Parameters.AddWithValue("@qt", qText);
                                ins.Parameters.AddWithValue("@ty", qType);
                                ins.Parameters.AddWithValue("@mk", marks);
                                ins.Parameters.AddWithValue("@ex", (object)explanation ?? DBNull.Value);
                                questionId = (int)ins.ExecuteScalar();
                            }
                        }

                        // Re-insert options for BOTH create & edit
                        if (qType == "MCQ")
                        {
                            string a = (txtChoiceA.Text ?? "").Trim();
                            string b = (txtChoiceB.Text ?? "").Trim();
                            string c = (txtChoiceC.Text ?? "").Trim();
                            string d = (txtChoiceD.Text ?? "").Trim();
                            string correct = radiobtnChoice1.Checked ? "A"
                                         : radiobtnChoice2.Checked ? "B"
                                         : radiobtnChoice3.Checked ? "C"
                                         : radiobtnChoice4.Checked ? "D" : "";

                            // server-side guard (mirrors validators)
                            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b) ||
                                string.IsNullOrWhiteSpace(c) || string.IsNullOrWhiteSpace(d))
                                throw new ApplicationException("Please fill all four MCQ options (A, B, C, D).");
                            if (string.IsNullOrEmpty(correct))
                                throw new ApplicationException("Please select the correct MCQ option.");

                            InsertOption(con, tx, questionId, a, correct == "A");
                            InsertOption(con, tx, questionId, b, correct == "B");
                            InsertOption(con, tx, questionId, c, correct == "C");
                            InsertOption(con, tx, questionId, d, correct == "D");
                        }
                        else if (qType == "TrueFalse")
                        {
                            var chosen = (radiobtnTF.SelectedValue ?? "").Trim(); // "True" | "False"
                            if (string.IsNullOrEmpty(chosen))
                                throw new ApplicationException("Please choose True or False.");

                            InsertOption(con, tx, questionId, "True", chosen.Equals("True", StringComparison.OrdinalIgnoreCase));
                            InsertOption(con, tx, questionId, "False", chosen.Equals("False", StringComparison.OrdinalIgnoreCase));
                        }
                        else if (qType == "FillInBlanks")
                        {
                            string fillAns = (txtFillInBlank.Text ?? "").Trim();
                            if (string.IsNullOrWhiteSpace(fillAns))
                                throw new ApplicationException("Please enter the fill-in answer.");

                            InsertOption(con, tx, questionId, fillAns, true);
                        }

                        tx.Commit();
                    }
                }

                // success
                lblMessage.CssClass = "text-success";
                lblMessage.Text = "Saved.";
                Response.Redirect($"~/Quiz_Admin/EditQuizPage.aspx?quizId={quizId}", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Error saving question: " + ex.Message;
            }
        }

        private void InsertOption(SqlConnection con, SqlTransaction tx, int questionId, string text, bool isCorrect)
        {
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.tblOptions (QuestionID, OptionText, IsCorrect)
                VALUES (@qid, @txt, @ok);", con, tx))
            {
                cmd.Parameters.AddWithValue("@qid", questionId);
                cmd.Parameters.AddWithValue("@txt", text);
                cmd.Parameters.AddWithValue("@ok", isCorrect);
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            var quizId = hiddenQuizID.Value;
            Response.Redirect($"~/Quiz_Admin/EditQuizPage.aspx?quizId={quizId}", false);
        }
    }
}