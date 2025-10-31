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
    public partial class QuizResultPage : System.Web.UI.Page
    {
        private readonly string cs =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // quizId comes from the dashboard “View Result” button
            if (!int.TryParse(Request.QueryString["quizId"], out int quizId) || quizId <= 0)
            {
                litTitle.Text = "Results";
                litEmpty.Text = "<div class='empty'>Quiz not found.</div>";
                return;
            }

            // If you don't have auth wired yet, you can hardcode for testing (RID=1).
            int rid = 0;
            if (Session["RID"] != null) int.TryParse(Session["RID"].ToString(), out rid);
            if (rid == 0) rid = 1; // TEMP fallback for testing

            LoadHeader(quizId);
            LoadAttempts(quizId, rid);
        }

        private void LoadHeader(int quizId)
        {
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand("SELECT Title FROM tblQuiz WHERE QuizID=@QID", con))
            {
                cmd.Parameters.AddWithValue("@QID", quizId);
                con.Open();
                var o = cmd.ExecuteScalar();
                litTitle.Text = (o == null || o == DBNull.Value) ? "Results" : (o.ToString() + " - Results");
            }
        }

        private void LoadAttempts(int quizId, int rid)
        {
            using (var con = new SqlConnection(cs))
            {
                // r.Score and q.Marks should be INT; if not, we CAST safely.
                string sql = @"
                    WITH Scored AS (
                      SELECT 
                        a.AttemptID,
                        a.AttemptDate,
                        -- sum earned and total per attempt
                        SUM(COALESCE(CAST(r.Score AS INT),0)) AS Earned,
                        SUM(COALESCE(CAST(q.Marks AS INT),0)) AS Total
                      FROM tblQuizAttempt a
                      JOIN tblQuizResult r ON r.AttemptID = a.AttemptID
                      JOIN tblQuestion   q ON q.QuestionID = r.Question
                      WHERE a.QuizID = @QuizID AND a.RID = @RID
                      GROUP BY a.AttemptID, a.AttemptDate
                    )
                    SELECT 
                      ROW_NUMBER() OVER (ORDER BY AttemptDate, AttemptID) AS AttemptNo,
                      AttemptID,
                      AttemptDate,
                      Earned,
                      Total
                    FROM Scored
                    ORDER BY AttemptDate, AttemptID;";

                var da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@QuizID", quizId);
                da.SelectCommand.Parameters.AddWithValue("@RID", rid);

                var dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    phTable.Visible = false;
                    litEmpty.Text = "<div class='empty'>No attempts yet.</div>";
                    return;
                }

                // add a friendly "ScoreText" column for binding
                dt.Columns.Add("ScoreText", typeof(string));
                foreach (DataRow r in dt.Rows)
                {
                    int earned = Convert.ToInt32(r["Earned"]);
                    int total = Convert.ToInt32(r["Total"]);
                    r["ScoreText"] = $"{earned}/{total}";
                }

                phTable.Visible = true;
                repAttempts.DataSource = dt;
                repAttempts.DataBind();
            }
        }

        protected void repAttempts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "view")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int attemptId))
                {
                    Response.Redirect($"~/Quiz_Student/AnswersReview.aspx?attemptId={attemptId}");
                }
            }
        }
    }
}