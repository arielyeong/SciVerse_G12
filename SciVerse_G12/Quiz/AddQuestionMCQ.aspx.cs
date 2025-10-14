using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SciVerse_G12.Quiz
{
    public partial class AddQuestionMCQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate the dropdown once
                dropdownType.Items.Clear();
                dropdownType.Items.Add(new System.Web.UI.WebControls.ListItem("--- select ---", ""));
                dropdownType.Items.Add(new System.Web.UI.WebControls.ListItem("Multiple Choice (MCQ)", "MCQ"));
                dropdownType.Items.Add(new System.Web.UI.WebControls.ListItem("True / False", "TF"));
                dropdownType.Items.Add(new System.Web.UI.WebControls.ListItem("Fill in the blank", "FILL"));
            }

            // Always re-apply visibility after postbacks
            TogglePanels();
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

        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            // 1) Validate quizId
            var quizIdStr = Request.QueryString["quizId"];
            if (!int.TryParse(quizIdStr, out var quizId))
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Missing or invalid quiz id.";
                return;
            }

            // 2) Gather form values (make sure these IDs exist in your .aspx)
            string questionText = (txtQuestion.Text ?? "").Trim();     // textbox for the question
            string qType = dropdownType.SelectedValue;                 // "MCQ" | "TF" | "FILL"
            int marks = int.TryParse(txtMarks.Text, out var m) ? m : 0;
            string feedback = (txtFeedback.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(questionText) || string.IsNullOrWhiteSpace(qType))
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please enter a question and choose a question type.";
                return;
            }

            // 3) Insert the question
            int newQuestionId;
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO [tblQuestion]
                        (QuizID, QuestionText, QuestionType, Marks, Feedback)
                    OUTPUT INSERTED.QuestionID
                    VALUES
                        (@QuizID, @QuestionText, @QuestionType, @Marks, @Feedback);
                ", con))
                {
                    cmd.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizId;
                    cmd.Parameters.Add("@QuestionText", SqlDbType.NVarChar).Value = questionText;
                    cmd.Parameters.Add("@QuestionType", SqlDbType.NVarChar, 50).Value = qType;
                    cmd.Parameters.Add("@Marks", SqlDbType.Int).Value = marks;
                    cmd.Parameters.Add("@Feedback", SqlDbType.NVarChar).Value =
                        (object)feedback ?? DBNull.Value;

                    con.Open();
                    newQuestionId = (int)cmd.ExecuteScalar();
                }

                // 4) (Optional) If you have tables for options/answers, save them here.
                // Example IDs used below must match your .aspx controls:
                //
                // if (qType == "MCQ")
                // {
                //     string optA = txtOptA.Text.Trim();
                //     string optB = txtOptB.Text.Trim();
                //     string optC = txtOptC.Text.Trim();
                //     string optD = txtOptD.Text.Trim();
                //     string correct = rblMcqCorrect.SelectedValue; // "A"|"B"|"C"|"D"
                //     // INSERT into your tblQuestionOption / tblAnswer tables…
                // }
                // else if (qType == "TF")
                // {
                //     string correctTF = rblTF.SelectedValue; // "True"|"False"
                //     // INSERT answer row…
                // }
                // else if (qType == "FILL")
                // {
                //     string fill = txtFillAnswer.Text.Trim();
                //     // INSERT answer row…
                // }

                // 5) Success → go back to the quiz editor (or stay on same page to add another)
                Response.Redirect("EditQuizPage.aspx?quizId=" + quizId, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Error saving question: " + ex.Message;
            }
        
    }
    }
}