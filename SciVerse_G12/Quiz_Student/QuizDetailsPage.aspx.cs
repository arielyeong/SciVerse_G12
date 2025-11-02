using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizDetailsPage : System.Web.UI.Page
    {
        private string ConnStr => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

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
            // Enforce login
            if (Session["RID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (IsPostBack) return;

            if (!int.TryParse(Request.QueryString["quizId"], out int quizId) || quizId <= 0)
            {
                ShowError("Quiz not found");
                return;
            }

            LoadQuiz(quizId, CurrentRid);
        }

        private void ShowError(string msg)
        {
            titleLiteral.Text = Server.HtmlEncode(msg);
            btnStart.Enabled = false;
        }

        private void LoadQuiz(int quizId, int rid)
        {
            const string sqlQuiz = @"
                SELECT Title, Description, TimeLimit, AttemptLimit
                FROM dbo.tblQuiz
                WHERE QuizID = @QuizID;";

            const string sqlQCount = @"SELECT COUNT(*) FROM dbo.tblQuestion WHERE QuizID = @QuizID;";

            const string sqlAttemptsUsed = @"
                SELECT COUNT(*)
                FROM dbo.tblQuizAttempt
                WHERE QuizID = @QuizID AND RID = @RID;";

            using (var con = new SqlConnection(ConnStr))
            {
                con.Open();

                string title = "Untitled";
                string desc = "";
                int timeLimitMinutes = 0;
                int attemptLimit = 0;

                // Quiz header
                using (var cmd = new SqlCommand(sqlQuiz, con))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    using (var r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!r.Read())
                        {
                            ShowError("Quiz not found");
                            return;
                        }

                        title = r["Title"]?.ToString() ?? "Untitled";
                        desc = r["Description"]?.ToString() ?? "";
                        timeLimitMinutes = r["TimeLimit"] == DBNull.Value ? 0 : Convert.ToInt32(r["TimeLimit"]);
                        attemptLimit = r["AttemptLimit"] == DBNull.Value ? 0 : Convert.ToInt32(r["AttemptLimit"]);
                    }
                }

                // Question count
                int qCount;
                using (var cmd = new SqlCommand(sqlQCount, con))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    qCount = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Attempts used by this RID
                int used = 0;
                if (attemptLimit > 0 && rid > 0)
                {
                    using (var cmd = new SqlCommand(sqlAttemptsUsed, con))
                    {
                        cmd.Parameters.AddWithValue("@QuizID", quizId);
                        cmd.Parameters.AddWithValue("@RID", rid);
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                int remaining = (attemptLimit > 0) ? Math.Max(attemptLimit - used, 0) : 999;

                // Bind UI
                titleLiteral.Text = Server.HtmlEncode(title);
                litDescription.Text = Server.HtmlEncode(desc);
                lblTimeLimit.Text = timeLimitMinutes > 0 ? $"{timeLimitMinutes} min" : "No time limit";
                lblQuestions.Text = $"{qCount} Questions";
                lblAttempts.Text = remaining == 999 ? "Unlimited" : $"{remaining} / {attemptLimit}";

                // Timed/Untimed radio logic
                if (timeLimitMinutes == 0)
                {
                    rbTimed.Visible = false;
                    rbUntimed.Visible = false;
                    rbTimed.Checked = false;
                    rbUntimed.Checked = true;
                    ViewState["ForceUntimed"] = true;
                }
                else
                {
                    rbTimed.Visible = true;
                    rbUntimed.Visible = true;
                    if (!rbTimed.Checked && !rbUntimed.Checked) rbTimed.Checked = true;
                    ViewState["ForceUntimed"] = false;
                }

                // Keep state
                ViewState["QuizId"] = quizId;
                ViewState["TimeLimitMin"] = timeLimitMinutes;
                ViewState["AttemptLimit"] = attemptLimit;
                ViewState["AttemptsUsed"] = used;
                ViewState["RID"] = rid;
            }
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            // Login guard again for safety (e.g., session expired between views)
            if (Session["RID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            int quizId = (int)(ViewState["QuizId"] ?? 0);
            if (quizId == 0) return;

            int attemptLimit = (int)(ViewState["AttemptLimit"] ?? 0);
            int used = (int)(ViewState["AttemptsUsed"] ?? 0);
            if (attemptLimit > 0 && used >= attemptLimit)
            {
                ShowError("You have reached the maximum number of attempts for this quiz.");
                btnStart.Enabled = false;
                return;
            }

            bool forceUntimed = (bool?)ViewState["ForceUntimed"] ?? false;
            bool isTimedChoice = rbTimed.Visible && rbTimed.Checked;
            bool isTimed = !forceUntimed && isTimedChoice;

            int timeLimitMin = (int)(ViewState["TimeLimitMin"] ?? 0);
            int totalSec = isTimed ? (timeLimitMin * 60) : 0;

            // Build target URL (RID is NOT sent via query; Session carries identity)
            string url = ResolveUrl(
                isTimed
                ? $"~/Quiz_Student/Quiz.aspx?QuizID={quizId}&mode=timed&sec={totalSec}"
                : $"~/Quiz_Student/Quiz.aspx?QuizID={quizId}&mode=untimed"
            );

            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}