using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz
{
    public partial class NewQuestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Require a quizId in the URL, e.g. .../NewQuestion.aspx?quizId=12
            var quizId = Request.QueryString["quizId"];
            if (string.IsNullOrEmpty(quizId))
            {
                Response.Redirect("~/Quiz_Admin/ViewQuizList.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Keep it handy
            hiddenQuizID.Value = quizId;
            linkBack.NavigateUrl = ResolveUrl("~/Quiz_Admin/EditQuizPage.aspx?quizId=" + quizId);

            if (!IsPostBack)
            {
                // Populate the dropdown once
                dropdownType.Items.Clear();
                dropdownType.Items.Add(new ListItem("--- select ---", ""));
                dropdownType.Items.Add(new ListItem("Multiple Choice (MCQ)", "MCQ"));
                dropdownType.Items.Add(new ListItem("True / False", "TF"));
                dropdownType.Items.Add(new ListItem("Fill in the blank", "FILL"));
            }

            // if editing
            int qid;
            if (int.TryParse(Request.QueryString["questionId"], out qid))
            {
                LoadQuestionForEdit(qid);
            }
        
        // Always re-apply visibility after postbacks
        TogglePanels();
        }

        private void LoadQuestionForEdit(int questionId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // question
                using (var cmd = new SqlCommand(@"
            SELECT QuestionText, QuestionType, Marks, Feedback
            FROM dbo.tblQuestion WHERE QuestionID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", questionId);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            txtQuestion.Text = r["QuestionText"].ToString();
                            dropdownType.SelectedValue = r["QuestionType"].ToString();
                            txtMarks.Text = Convert.ToString(r["Marks"]);
                            txtFeedback.Text = Convert.ToString(r["Feedback"]);
                        }
                    }
                }

                // options
                using (var cmdOpt = new SqlCommand(@"
            SELECT OptionText, IsCorrect
            FROM dbo.tblOption WHERE QuestionID=@id
            ORDER BY OptionID", con))
                {
                    cmdOpt.Parameters.AddWithValue("@id", questionId);
                    using (var r = cmdOpt.ExecuteReader())
                    {
                        if (dropdownType.SelectedValue == "MCQ")
                        {
                            string[] texts = new string[4];
                            bool[] flags = new bool[4];
                            int i = 0;
                            while (r.Read() && i < 4)
                            {
                                texts[i] = r["OptionText"].ToString();
                                flags[i] = Convert.ToBoolean(r["IsCorrect"]);
                                i++;
                            }
                            txtChoiceA.Text = texts.Length > 0 ? texts[0] : "";
                            txtChoiceB.Text = texts.Length > 1 ? texts[1] : "";
                            txtChoiceC.Text = texts.Length > 2 ? texts[2] : "";
                            txtChoiceD.Text = texts.Length > 3 ? texts[3] : "";

                            radiobtnChoice1.Checked = flags.Length > 0 && flags[0];
                            radiobtnChoice2.Checked = flags.Length > 1 && flags[1];
                            radiobtnChoice3.Checked = flags.Length > 2 && flags[2];
                            radiobtnChoice4.Checked = flags.Length > 3 && flags[3];
                        }
                        else if (dropdownType.SelectedValue == "TF")
                        {
                            bool isTrue = false, isFalse = false;
                            while (r.Read())
                            {
                                var text = r["OptionText"].ToString();
                                var correct = Convert.ToBoolean(r["IsCorrect"]);
                                if (text.Equals("True", StringComparison.OrdinalIgnoreCase)) isTrue = correct;
                                if (text.Equals("False", StringComparison.OrdinalIgnoreCase)) isFalse = correct;
                            }
                            radiobtnTF.ClearSelection();
                            if (isTrue) radiobtnTF.SelectedValue = "True";
                            if (isFalse) radiobtnTF.SelectedValue = "False";
                        }
                        else if (dropdownType.SelectedValue == "FILL")
                        {
                            if (r.Read())
                                txtFillInBlank.Text = r["OptionText"].ToString();
                        }
                    }
                }
            }

            // remember the editing id (a hidden field or ViewState)
            ViewState["EditingQuestionId"] = questionId;
        }
        protected void dropdownType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }

        private void TogglePanels()
        {
            var val = dropdownType.SelectedValue ?? "";
            panelMCQ.Visible = val == "MCQ";
            panelTF.Visible = val == "TF";
            panelFill.Visible = val == "FILL";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.CssClass = "text-danger";
            lblMessage.Text = "";

            if (!int.TryParse(hiddenQuizID.Value, out var quizId))
            {
                lblMessage.Text = "Missing or invalid quiz id.";
                return;
            }

            string questionText = (txtQuestion.Text ?? "").Trim();
            string questionType = dropdownType.SelectedValue;
            int marks = int.TryParse((txtMarks.Text ?? "").Trim(), out var mark) ? mark : 0;
            string feedback = (txtFeedback.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(questionText))
            { lblMessage.Text = "Please enter the question text."; return; }
            if (string.IsNullOrWhiteSpace(questionType))
            { lblMessage.Text = "Please choose a question type."; return; }

            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (var con = new SqlConnection(cs))
                {
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        int questionId;

                        // Are we editing?
                        if (ViewState["EditingQuestionId"] != null)
                        {
                            questionId = (int)ViewState["EditingQuestionId"];

                            // Update question
                            using (var up = new SqlCommand(@"
                        UPDATE dbo.tblQuestion
                        SET QuestionText=@qt, QuestionType=@ty, Marks=@mk, Feedback=@fb
                        WHERE QuestionID=@id;", con, tx))
                            {
                                up.Parameters.AddWithValue("@qt", questionText);
                                up.Parameters.AddWithValue("@ty", questionType);
                                up.Parameters.AddWithValue("@mk", marks);
                                up.Parameters.AddWithValue("@fb", (object)feedback ?? DBNull.Value);
                                up.Parameters.AddWithValue("@id", questionId);
                                up.ExecuteNonQuery();
                            }

                            // wipe old options
                            using (var del = new SqlCommand("DELETE FROM dbo.tblOption WHERE QuestionID=@id", con, tx))
                            {
                                del.Parameters.AddWithValue("@id", questionId);
                                del.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Insert new question
                            using (var ins = new SqlCommand(@"
                        INSERT INTO dbo.tblQuestion
                          (QuizID, QuestionText, QuestionType, Marks, Feedback)
                        OUTPUT INSERTED.QuestionID
                        VALUES (@QuizID, @qt, @ty, @mk, @fb);", con, tx))
                            {
                                ins.Parameters.AddWithValue("@QuizID", quizId);
                                ins.Parameters.AddWithValue("@qt", questionText);
                                ins.Parameters.AddWithValue("@ty", questionType);
                                ins.Parameters.AddWithValue("@mk", marks);
                                ins.Parameters.AddWithValue("@fb", (object)feedback ?? DBNull.Value);
                                questionId = (int)ins.ExecuteScalar();
                            }
                        }

                        // re-insert options based on type
                        if (questionType == "MCQ")
                        {
                            string a = (txtChoiceA.Text ?? "").Trim();
                            string b = (txtChoiceB.Text ?? "").Trim();
                            string c = (txtChoiceC.Text ?? "").Trim();
                            string d = (txtChoiceD.Text ?? "").Trim();
                            string correct = radiobtnChoice1.Checked ? "A"
                                         : radiobtnChoice2.Checked ? "B"
                                         : radiobtnChoice3.Checked ? "C"
                                         : radiobtnChoice4.Checked ? "D" : "";

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
                        else if (questionType == "TF")
                        {
                            var chosen = (radiobtnTF.SelectedValue ?? "").Trim(); // True | False
                            if (string.IsNullOrEmpty(chosen))
                                throw new ApplicationException("Please choose True or False.");

                            InsertOption(con, tx, questionId, "True", chosen.Equals("True", StringComparison.OrdinalIgnoreCase));
                            InsertOption(con, tx, questionId, "False", chosen.Equals("False", StringComparison.OrdinalIgnoreCase));
                        }
                        else if (questionType == "FILL")
                        {
                            string fillAns = (txtFillInBlank.Text ?? "").Trim();
                            if (string.IsNullOrWhiteSpace(fillAns))
                                throw new ApplicationException("Please enter the fill-in answer.");
                            InsertOption(con, tx, questionId, fillAns, true);
                        }

                        tx.Commit();
                    }
                }

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
        INSERT INTO dbo.tblOption (QuestionID, OptionText, IsCorrect)
        VALUES (@qid, @txt, @ok);", con, tx))
            {
                cmd.Parameters.AddWithValue("@qid", questionId);
                cmd.Parameters.AddWithValue("@txt", text);
                cmd.Parameters.AddWithValue("@ok", isCorrect);
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertOption(SqlConnection con, SqlTransaction tx, int questionId, string text, bool isCorrect, string matchKey = null)
        {
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.tblOption (QuestionID, OptionText, IsCorrect, MatchKey)
                VALUES (@QuestionID, @OptionText, @IsCorrect, @MatchKey);
            ", con, tx))
            {
                cmd.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionId;
                cmd.Parameters.Add("@OptionText", SqlDbType.NVarChar, 400).Value = text;
                cmd.Parameters.Add("@IsCorrect", SqlDbType.Bit).Value = isCorrect;
                cmd.Parameters.Add("@MatchKey", SqlDbType.NVarChar, 100).Value = (object)matchKey ?? DBNull.Value;
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {

        }
    }
}
