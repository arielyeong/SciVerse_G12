using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizSummary : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        // Read RID strictly from Session (set during login)
        private int CurrentRid
        {
            get
            {
                int rid = 0;
                if (Session["RID"] != null)
                    int.TryParse(Session["RID"].ToString(), out rid);
                return rid;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce login (all requests)
            if (Session["RID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Optionally hide site nav on this page
            var nav = Master?.FindControl("NavLinks");
            if (nav != null) nav.Visible = false;

            if (IsPostBack) return;

            // Preferred path: summary from AttemptID
            if (int.TryParse(Request.QueryString["attemptId"], out int attemptId) && attemptId > 0)
            {
                hidAttemptId.Value = attemptId.ToString();
                LoadFromAttempt(attemptId, CurrentRid);   // secure load by RID
                return;
            }

            // Fallback: summary from querystring pieces
            int quizId = GetQSInt("quizId");
            int correct = GetQSInt("correct");
            int incorrect = GetQSInt("incorrect");
            int seconds = GetQSInt("seconds");
            int total = GetQSInt("total");
            if (total <= 0) total = correct + incorrect;

            hidQuizId.Value = quizId.ToString();

            litScoreNum.Text = correct.ToString();
            litScoreDen.Text = total.ToString();
            litCorrect.Text = correct.ToString();
            litIncorrect.Text = incorrect.ToString();
            litTime.Text = FormatTime(seconds);
        }

        private void LoadFromAttempt(int attemptId, int rid)
        {
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
                SELECT
                    a.QuizID,
                    (SELECT COUNT(*) FROM dbo.tblQuestion WHERE QuizID = a.QuizID) AS TotalQ,
                    a.Duration AS TotalSeconds,
                    SUM(CASE WHEN r.Score > 0 THEN 1 ELSE 0 END) AS Correct,
                    SUM(CASE WHEN r.Score = 0 THEN 1 ELSE 0 END) AS Incorrect
                FROM dbo.tblQuizAttempt a
                LEFT JOIN dbo.tblQuizResult r ON r.AttemptID = a.AttemptID
                WHERE a.AttemptID = @A AND a.RID = @RID   -- enforce ownership
                GROUP BY a.QuizID, a.Duration;", con))
            {
                cmd.Parameters.Add("@A", SqlDbType.Int).Value = attemptId;
                cmd.Parameters.Add("@RID", SqlDbType.Int).Value = rid;
                con.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        int quizId = rd.GetInt32(0);
                        int totalQ = rd.IsDBNull(1) ? 0 : rd.GetInt32(1);
                        int seconds = rd.IsDBNull(2) ? 0 : rd.GetInt32(2);
                        int correct = rd.IsDBNull(3) ? 0 : rd.GetInt32(3);
                        int incorrect = rd.IsDBNull(4) ? 0 : rd.GetInt32(4);

                        hidQuizId.Value = quizId.ToString();
                        litScoreNum.Text = correct.ToString();
                        litScoreDen.Text = totalQ.ToString();
                        litCorrect.Text = correct.ToString();
                        litIncorrect.Text = incorrect.ToString();
                        litTime.Text = FormatTime(seconds);
                    }
                    else
                    {
                        // Attempt not found or not owned by this RID → safe defaults
                        litScoreNum.Text = "0";
                        litScoreDen.Text = "0";
                        litCorrect.Text = "0";
                        litIncorrect.Text = "0";
                        litTime.Text = "00:00";
                    }
                }
            }
        }

        private static int GetQSInt(string key)
        {
            int.TryParse(System.Web.HttpContext.Current.Request.QueryString[key], out int v);
            return v;
        }

        private static string FormatTime(int totalSeconds)
        {
            if (totalSeconds <= 0) return "00:00";
            var ts = TimeSpan.FromSeconds(totalSeconds);
            return (ts.TotalHours >= 1)
                ? $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}"
                : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        protected void btnShowAnswers_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hidAttemptId.Value))
                Response.Redirect($"~/Quiz_Student/AnswersReview.aspx?attemptId={hidAttemptId.Value}", false);
            else
                Response.Redirect($"~/Quiz_Student/AnswersReview.aspx?quizId={hidQuizId.Value}", false);
        }

        protected void btnFlashcards_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hidAttemptId.Value))
                Response.Redirect($"~/Flashcard/ViewFlashcard.aspx?attemptId={hidAttemptId.Value}", false);
            else
                Response.Redirect($"~/Flashcard/ViewFlashcard.aspx?quizId={hidQuizId.Value}", false);
        }

        protected void btnStartAgain_Click(object sender, EventArgs e)
        {
            // Your dashboard typically doesn’t take quizId; if you prefer to jump to details page, use:
            // Response.Redirect($"QuizDetailsPage.aspx?quizId={hidQuizId.Value}", false);
            Response.Redirect($"~/Quiz_Student/QuizDashboardPageStudent.aspx?quizId={hidQuizId.Value}", false);
        }
    }
}