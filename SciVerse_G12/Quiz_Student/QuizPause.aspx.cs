using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizPause : System.Web.UI.Page
    {
        // ---------------------------
        // Querystring: ?quizId=123
        // ---------------------------
        private int QuizId
        {
            get { return int.TryParse(Request.QueryString["quizId"], out var q) ? q : 0; }
        }

        // ---------------------------
        // Session-backed integers (NO Dictionary)
        // These should be updated by your actual quiz runner page.
        // ---------------------------
        private int CurrentIndex   // 1-based question index for progress/counter
        {
            get { return (Session["Q_IDX"] is int v) ? v : 1; }
            set { Session["Q_IDX"] = value; }
        }

        private int AnsweredCount  // how many answered so far
        {
            get { return (Session["Q_ANSWERED"] is int v) ? v : 0; }
            set { Session["Q_ANSWERED"] = value; }
        }

        private int CorrectCount   // how many correct so far
        {
            get { return (Session["Q_CORRECT"] is int v) ? v : 0; }
            set { Session["Q_CORRECT"] = value; }
        }

        private int TotalQuestions // total in this quiz (cached here for display)
        {
            get { return (Session["Q_TOTAL"] is int v) ? v : 0; }
            set { Session["Q_TOTAL"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (QuizId <= 0)
            {
                // No quiz selected
                ResetUiNoQuiz();
                return;
            }

            // Load total from DB (or keep the one already in Session if you set it elsewhere)
            TotalQuestions = GetTotalQuestionsFromDb(QuizId);

            if (TotalQuestions <= 0)
            {
                // No questions
                CurrentIndex = 1;
                litCounter.Text = "0 / 0";
                capsuleFill.Style["width"] = "0%";
                phScore.Visible = false;
                phNoQuestions.Visible = true;
                return;
            }

            // Clamp CurrentIndex
            CurrentIndex = Math.Max(1, Math.Min(CurrentIndex, TotalQuestions));

            // Counter like "1 / 20"
            litCounter.Text = $"{CurrentIndex} / {TotalQuestions}";

            // Progress: empty at Q1, full at last question
            double pct = (TotalQuestions > 1)
                ? (CurrentIndex - 1) * 100.0 / (TotalQuestions - 1)
                : (CurrentIndex >= 1 ? 100 : 0);
            capsuleFill.Style["width"] = pct.ToString("0.#") + "%";

            // Score only when all answered
            bool finished = (AnsweredCount >= TotalQuestions);
            if (finished)
            {
                int percent = (int)Math.Round(CorrectCount * 100.0 / TotalQuestions);
                litScore.Text = $"{CorrectCount} / {TotalQuestions} ({percent}%)";
                phScore.Visible = true;
            }
            else
            {
                phScore.Visible = false;
            }

            phNoQuestions.Visible = false; // we do have questions
        }

        // Buttons
        protected void btnResume_Click(object sender, EventArgs e)
        {
            // Navigate back to your quiz runner page
            Response.Redirect($"~/Quiz_Student/Quiz.aspx?quizId={QuizId}", false);
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            // Save partial progress if needed then go back to list/dashboard
            Response.Redirect("~/Quiz_Student/MyQuizzes.aspx", false);
        }

        // Helpers
        private int GetTotalQuestionsFromDb(int quizId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            const string sql = @"SELECT COUNT(*) FROM dbo.tblQuestion WHERE QuizID = @id;";

            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", quizId);
                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        private void ResetUiNoQuiz()
        {
            TotalQuestions = 0;
            AnsweredCount = 0;
            CorrectCount = 0;
            CurrentIndex = 1;

            litCounter.Text = "0 / 0";
            capsuleFill.Style["width"] = "0%";
            phScore.Visible = false;
            phNoQuestions.Visible = true;
        }
    }
}