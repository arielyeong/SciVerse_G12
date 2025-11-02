using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizAttemptHistory : Page
    {
        private readonly string _cs =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private int QuizIdFilter
        {
            get
            {
                int.TryParse(Request.QueryString["quizId"], out var qid);
                return qid;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Session["RID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            hidQuizId.Value = QuizIdFilter.ToString();

            // Adjust header if we're filtering by one quiz
            if (QuizIdFilter > 0)
            {
                var title = GetQuizTitle(QuizIdFilter);
                litHeader.Text = "Quiz Attempt History — " + title;
                litSub.Text = "Showing your attempts for this quiz only.";
                litEmptyMsg.Text = "No attempts found for this quiz yet.";
            }

            BindAttempts(Convert.ToInt32(Session["RID"]), QuizIdFilter);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Quiz_Student/QuizDashboardPageStudent.aspx");
            
        }

        private void BindAttempts(int rid, int quizIdFilter)
        {
            using (var con = new SqlConnection(_cs))
            using (var da = new SqlDataAdapter(@"
                /* Aggregate scores per attempt */
                WITH S AS (
                  SELECT 
                      a.AttemptID,
                      a.RID,
                      a.QuizID,
                      a.AttemptDate,
                      q.Title AS QuizTitle,
                      SUM(CAST(r.Score AS int))      AS Score,
                      SUM(CAST(qq.Marks AS int))     AS TotalMarks
                  FROM dbo.tblQuizAttempt a
                  JOIN dbo.tblQuiz q        ON q.QuizID      = a.QuizID
                  JOIN dbo.tblQuizResult r  ON r.AttemptID   = a.AttemptID
                  JOIN dbo.tblQuestion qq   ON qq.QuestionID = r.Question
                  WHERE a.RID = @RID
                    AND (@QuizID IS NULL OR a.QuizID = @QuizID)
                  GROUP BY a.AttemptID, a.RID, a.QuizID, a.AttemptDate, q.Title
                )
                SELECT 
                  ROW_NUMBER() OVER (ORDER BY AttemptDate DESC) AS RowNo,
                  AttemptID,
                  QuizTitle,
                  AttemptDate,
                  /* integer percent; change 100.0 for decimal */
                  (Score * 100) / NULLIF(TotalMarks,0) AS [Percent]
                FROM S
                ORDER BY AttemptDate DESC;", con))
            {
                da.SelectCommand.Parameters.Add("@RID", SqlDbType.Int).Value = rid;

                // pass NULL when no quiz filter is applied
                if (quizIdFilter > 0)
                    da.SelectCommand.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizIdFilter;
                else
                    da.SelectCommand.Parameters.Add("@QuizID", SqlDbType.Int).Value = DBNull.Value;

                var dt = new DataTable();
                da.Fill(dt);

                phEmpty.Visible = dt.Rows.Count == 0;
                repAttempts.DataSource = dt;
                repAttempts.DataBind();
            }
        }

        private string GetQuizTitle(int quizId)
        {
            using (var con = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT Title FROM dbo.tblQuiz WHERE QuizID=@Q", con))
            {
                cmd.Parameters.Add("@Q", SqlDbType.Int).Value = quizId;
                con.Open();
                var o = cmd.ExecuteScalar();
                return o == null ? "Selected Quiz" : o.ToString();
            }
        }

        protected string GetPctClass(object pctObj)
        {
            int p = 0;
            if (pctObj != DBNull.Value) int.TryParse(pctObj.ToString(), out p);
            if (p >= 80) return "pct-green";
            if (p >= 50) return "pct-amber";
            return "pct-red";
        }
    }
}
