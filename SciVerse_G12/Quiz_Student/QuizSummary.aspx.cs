using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizSummary : System.Web.UI.Page
    {
        // Use the same name you use elsewhere in your project (you used "ConnectionString" previously)
        private readonly string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide right-side nav links during quiz
            var navLinks = Master.FindControl("NavLinks");
            if (navLinks != null)
                navLinks.Visible = false;

            if (IsPostBack) return;

            if (int.TryParse(Request.QueryString["attemptId"], out int attemptId))
            {
                hidAttemptId.Value = attemptId.ToString();
                LoadFromAttempt(attemptId);
            }
            else
            {
                int quizId = GetQSInt("quizId");
                hidQuizId.Value = quizId.ToString();

                int correct = GetQSInt("correct");
                int incorrect = GetQSInt("incorrect");
                int seconds = GetQSInt("seconds");

                litCorrect.Text = correct.ToString();
                litIncorrect.Text = incorrect.ToString();
                litTime.Text = FormatTime(seconds);
                litQuizTitle.Text = GetQuizTitle(quizId);
            }
        }

        private void LoadFromAttempt(int attemptId)
        {
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
        SELECT 
            a.QuizID,
            q.Title,
            SUM(CASE WHEN r.Score > 0 THEN 1 ELSE 0 END)    AS Correct,
            SUM(CASE WHEN r.Score = 0 THEN 1 ELSE 0 END)    AS Incorrect,
            a.Duration                                      AS TotalSeconds
        FROM dbo.tblQuizAttempt a
        LEFT JOIN dbo.tblQuiz q       ON q.QuizID     = a.QuizID
        LEFT JOIN dbo.tblQuizResult r ON r.AttemptID  = a.AttemptID
        WHERE a.AttemptID = @AttemptID
        GROUP BY a.QuizID, q.Title, a.Duration;", con))
            {
                cmd.Parameters.Add("@AttemptID", SqlDbType.Int).Value = attemptId;
                con.Open();

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        int quizId = r.GetInt32(0);
                        hidQuizId.Value = quizId.ToString();

                        litQuizTitle.Text = r.IsDBNull(1) ? "Quiz Summary" : r.GetString(1);
                        int correct = r.IsDBNull(2) ? 0 : r.GetInt32(2);
                        int incorrect = r.IsDBNull(3) ? 0 : r.GetInt32(3);
                        int seconds = r.IsDBNull(4) ? 0 : r.GetInt32(4);

                        litCorrect.Text = correct.ToString();
                        litIncorrect.Text = incorrect.ToString();
                        litTime.Text = FormatTime(seconds);
                    }
                    else
                    {
                        // Attempt not found – show safe defaults
                        litQuizTitle.Text = "Quiz Summary";
                        litCorrect.Text = "0";
                        litIncorrect.Text = "0";
                        litTime.Text = "0s";
                    }
                }
            }
        }

        private string GetQuizTitle(int quizId)
        {
            if (quizId <= 0) return "Quiz Summary";
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand("SELECT Title FROM dbo.tblQuiz WHERE QuizID=@id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = quizId;
                con.Open();
                var title = cmd.ExecuteScalar() as string;
                return string.IsNullOrWhiteSpace(title) ? "Quiz Summary" : title;
            }
        }

        private static int GetQSInt(string key)
        {
            int.TryParse(System.Web.HttpContext.Current.Request.QueryString[key], out int v);
            return v;
        }

        private static string FormatTime(int totalSeconds)
        {
            if (totalSeconds < 60) return $"{totalSeconds}s";
            var ts = TimeSpan.FromSeconds(totalSeconds);
            return ts.Hours > 0 ? $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}"
                                : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        protected void btnShowAnswers_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hidAttemptId.Value))
                Response.Redirect($"AnswersReview.aspx?attemptId={hidAttemptId.Value}");
            else
                Response.Redirect($"AnswersReview.aspx?quizId={hidQuizId.Value}");
        }

        protected void btnFlashcards_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hidAttemptId.Value))
                Response.Redirect($"ViewFlashcard.aspx?attemptId={hidAttemptId.Value}");
            else
                Response.Redirect($"ViewFlashcard.aspx?quizId={hidQuizId.Value}");
        }

        protected void btnStartAgain_Click(object sender, EventArgs e)
        {
            Response.Redirect($"QuizDetailsPage.aspx?quizId={hidQuizId.Value}");
        }
    }
}