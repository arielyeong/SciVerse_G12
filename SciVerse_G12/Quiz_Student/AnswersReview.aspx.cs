using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Student
{
    public partial class AnswersReview : System.Web.UI.Page
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        // Logged-in RID from Session
        private int CurrentRid
        {
            get
            {
                int rid = 0;
                if (Session["RID"] != null) int.TryParse(Session["RID"].ToString(), out rid);
                return rid;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce login
            if (Session["RID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (IsPostBack) return;

            if (!int.TryParse(Request.QueryString["attemptId"], out int attemptId) || attemptId <= 0)
            {
                litEmpty.Text = "<div class='empty'>No attempt found.</div>";
                return;
            }

            // Security: ensure attempt belongs to this RID
            if (!AttemptBelongsToRid(attemptId, CurrentRid))
            {
                litEmpty.Text = "<div class='empty'>This attempt is not available.</div>";
                return;
            }

            hidAttemptId.Value = attemptId.ToString();

            LoadHeader(attemptId, CurrentRid);
            LoadQuestions(attemptId, CurrentRid);
        }

        private bool AttemptBelongsToRid(int attemptId, int rid)
        {
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM dbo.tblQuizAttempt WHERE AttemptID=@A AND RID=@R", con))
            {
                cmd.Parameters.AddWithValue("@A", attemptId);
                cmd.Parameters.AddWithValue("@R", rid);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // ===== Header Summary =====
        private void LoadHeader(int attemptId, int rid)
        {
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                SELECT 
                    a.QuizID, q.Title, a.AttemptDate, a.Duration,
                    SUM(CAST(r.Score AS INT)) AS TotalScore,
                    SUM(CAST(qs.Marks AS INT)) AS TotalMarks
                FROM dbo.tblQuizAttempt a
                JOIN dbo.tblQuiz q   ON q.QuizID = a.QuizID
                JOIN dbo.tblQuizResult r ON r.AttemptID = a.AttemptID
                JOIN dbo.tblQuestion qs ON qs.QuestionID = r.Question
                WHERE a.AttemptID = @AttemptID
                  AND a.RID = @RID                    -- enforce ownership
                GROUP BY a.QuizID, q.Title, a.AttemptDate, a.Duration;", con))
            {
                cmd.Parameters.AddWithValue("@AttemptID", attemptId);
                cmd.Parameters.AddWithValue("@RID", rid);
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        litHeader.Text = "Quiz Results - " + rd["Title"];
                        litDate.Text = Convert.ToDateTime(rd["AttemptDate"]).ToString("dd/MM/yyyy");
                        litTimeTaken.Text = FormatTime(Convert.ToInt32(rd["Duration"]));
                        litOverallScore.Text = $"{rd["TotalScore"]} / {rd["TotalMarks"]}";
                    }
                    else
                    {
                        litHeader.Text = "Quiz Results";
                        litDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        litTimeTaken.Text = "00:00";
                        litOverallScore.Text = "0 / 0";
                    }
                }
            }
        }

        // ===== Load All Questions =====
        private void LoadQuestions(int attemptId, int rid)
        {
            using (var con = new SqlConnection(cs))
            using (var da = new SqlDataAdapter(@"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY q.QuestionID) AS QuestionNo,
                    q.QuestionText,
                    q.Explanation,
                    q.Marks AS ScorePossible,
                    r.Answer AS UserAnswer,
                    r.Score AS ScoreAwarded,
                    q.QuestionType,
                    o.OptionText AS CorrectAnswer
                FROM dbo.tblQuizResult r
                JOIN dbo.tblQuizAttempt a ON a.AttemptID = r.AttemptID   -- join to check RID
                JOIN dbo.tblQuestion q ON q.QuestionID = r.Question
                LEFT JOIN dbo.tblOptions o ON o.QuestionID = q.QuestionID AND o.IsCorrect = 1
                WHERE r.AttemptID = @AttemptID
                  AND a.RID = @RID                                      -- enforce ownership
                ORDER BY q.QuestionID;", con))
            {
                da.SelectCommand.Parameters.AddWithValue("@AttemptID", attemptId);
                da.SelectCommand.Parameters.AddWithValue("@RID", rid);

                var dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    litEmpty.Text = "<div class='empty'>No answers found for this attempt.</div>";
                }

                repQuestions.DataSource = dt;
                repQuestions.DataBind();
            }
        }

        // ===== Row UI Binding =====
        protected void repQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var row = (DataRowView)e.Item.DataItem;
            int score = (row["ScoreAwarded"] == DBNull.Value) ? 0 : Convert.ToInt32(row["ScoreAwarded"]);

            var bubble = (HtmlGenericControl)e.Item.FindControl("statusBubble");
            if (bubble != null)
            {
                if (score > 0)
                {
                    bubble.InnerText = "✓";
                    bubble.Attributes["class"] = "status ok";
                }
                else
                {
                    bubble.InnerText = "✕";
                    bubble.Attributes["class"] = "status bad";
                }
            }
        }

        // ===== Format Time =====
        private string FormatTime(int totalSeconds)
        {
            if (totalSeconds <= 0) return "00:00";
            var ts = TimeSpan.FromSeconds(totalSeconds);
            return (ts.TotalHours >= 1)
                ? $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}"
                : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }
}
