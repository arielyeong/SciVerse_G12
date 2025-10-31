using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Student
{
    public partial class AnswersReview : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // Get the attemptId from the URL
            if (!int.TryParse(Request.QueryString["attemptId"], out int attemptId))
            {
                litEmpty.Text = "<div>No attempt found.</div>";
                return;
            }

            hidAttemptId.Value = attemptId.ToString();

            LoadHeader(attemptId);   // show top summary
            LoadQuestions(attemptId); // show each question
        }

        // ================= HEADER SUMMARY =================
        private void LoadHeader(int attemptId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sql = @"
                SELECT 
                    a.QuizID, q.Title, a.AttemptDate, a.Duration,
                    SUM(CAST(r.Score AS INT)) AS TotalScore,
                    SUM(CAST(qs.Marks AS INT)) AS TotalMarks
                FROM tblQuizAttempt a
                JOIN tblQuiz q ON q.QuizID = a.QuizID
                JOIN tblQuizResult r ON r.AttemptID = a.AttemptID
                JOIN tblQuestion qs ON qs.QuestionID = r.Question
                WHERE a.AttemptID = @AttemptID
                GROUP BY a.QuizID, q.Title, a.AttemptDate, a.Duration";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@AttemptID", attemptId);

                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    litHeader.Text = "Quiz Results - " + rd["Title"].ToString();
                    litDate.Text = Convert.ToDateTime(rd["AttemptDate"]).ToString("dd/MM/yyyy");
                    litTimeTaken.Text = FormatTime(Convert.ToInt32(rd["Duration"]));
                    litOverallScore.Text = rd["TotalScore"].ToString() + " / " + rd["TotalMarks"].ToString();
                }
                else
                {
                    litHeader.Text = "Quiz Results";
                    litDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    litTimeTaken.Text = "0:00";
                    litOverallScore.Text = "0 / 0";
                }
                rd.Close();
            }
        }

        // ================= QUESTION DETAILS =================
        private void LoadQuestions(int attemptId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sql = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY q.QuestionID)     AS QuestionNo,
                q.QuestionText,
                q.Explanation,
                q.Marks                                       AS ScorePossible,
                r.Answer                                      AS UserAnswer,
                r.Score                                       AS ScoreAwarded,
                q.QuestionType,
                o.OptionText                                   AS CorrectAnswer
            FROM tblQuizResult r
            JOIN tblQuestion q ON q.QuestionID = r.Question
            LEFT JOIN tblOptions o ON o.QuestionID = q.QuestionID AND o.IsCorrect = 1
            WHERE r.AttemptID = @AttemptID
            ORDER BY q.QuestionID;";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@AttemptID", attemptId);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    litEmpty.Text = "<div>No answers found for this attempt.</div>";
                }

                repQuestions.DataSource = dt;
                repQuestions.DataBind();
            }
        }

        // ================= REPEATER EVENT =================
        protected void repQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var row = (DataRowView)e.Item.DataItem;

            // Use the correct alias from your SELECT
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


        // ================= TIME FORMATTER =================
        private string FormatTime(int totalSeconds)
        {
            int m = totalSeconds / 60;
            int s = totalSeconds % 60;
            return $"{m:D2}:{s:D2}";
        }
    }
}