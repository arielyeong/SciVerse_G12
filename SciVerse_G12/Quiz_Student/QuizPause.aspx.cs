using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizPause : System.Web.UI.Page
    {
        private string ConnStr => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        // Logged-in user (strictly from Session)
        private int CurrentRid
        {
            get
            {
                int rid = 0;
                if (Session["RID"] != null) int.TryParse(Session["RID"].ToString(), out rid);
                return rid;
            }
        }

        // Paused state (from Session, set by Quiz.aspx)
        private int AttemptId => Convert.ToInt32(Session["Paused_AttemptId"] ?? 0);
        private int QuizId => Convert.ToInt32(Session["Paused_QuizId"] ?? 0);
        private int Index => Convert.ToInt32(Session["Paused_Idx"] ?? 0);     // 0-based
        private int TotalQ => Convert.ToInt32(Session["Paused_TotalQ"] ?? 0);
        private int RemainSec => Convert.ToInt32(Session["Paused_Remain"] ?? 0);
        private int TotalSec => Convert.ToInt32(Session["Paused_TotalSec"] ?? 0);
        private string Mode => (Session["Paused_Mode"] as string) ?? "untimed";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce login for all requests
            if (Session["RID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (IsPostBack) return;

            // Basic presence checks
            if (AttemptId == 0 || QuizId == 0 || TotalQ <= 0)
            {
                Response.Redirect("~/Quiz_Student/QuizDashboardPageStudent.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Security: ensure this Attempt belongs to the current RID
            if (!DoesAttemptBelongToRid(AttemptId, CurrentRid))
            {
                // If not owned, bounce to dashboard
                Response.Redirect("~/Quiz_Student/QuizDashboardPageStudent.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Populate UI
            litOn.Text = (Index + 1).ToString();
            litTotal.Text = TotalQ.ToString();
            litCounter.Text = $"{Index + 1} / {TotalQ}";

            double pct = (TotalQ > 0) ? Math.Min(100.0, ((Index + 1) * 100.0) / TotalQ) : 0;
            capsuleFill.Style["width"] = pct.ToString("0.#") + "%";

            // Optional: show remaining time if timed
            if (RemainSec > 0 && TotalSec > 0)
            {
                // If you have a literal for time, e.g., litRemain
                // litRemain.Text = FormatTime(RemainSec) + " remaining";
            }
        }

        private bool DoesAttemptBelongToRid(int attemptId, int rid)
        {
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.tblQuizAttempt WHERE AttemptID=@A AND RID=@R", con))
            {
                cmd.Parameters.AddWithValue("@A", attemptId);
                cmd.Parameters.AddWithValue("@R", rid);
                con.Open();
                var count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        protected void btnResume_Click(object sender, EventArgs e)
        {
            // Re-check login & ownership (in case of session race)
            if (Session["RID"] == null || !DoesAttemptBelongToRid(AttemptId, CurrentRid))
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Mark attempt back to InProgress
            try
            {
                using (var con = new SqlConnection(ConnStr))
                using (var cmd = new SqlCommand("UPDATE dbo.tblQuizAttempt SET Status='InProgress' WHERE AttemptID=@A", con))
                {
                    cmd.Parameters.AddWithValue("@A", AttemptId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* swallow and continue */ }

            // IMPORTANT: Do NOT clear paused session here.
            // Quiz.aspx relies on these Paused_* values on first load to restore timer & index.
            Response.Redirect($"~/Quiz_Student/Quiz.aspx?QuizID={QuizId}&mode={Mode}", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            // Optionally: keep attempt as 'Paused' (already set by Quiz.aspx), then return to dashboard
            Response.Redirect("~/Quiz_Student/QuizDashboardPageStudent.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Optional helper if you want to display remaining time
        private static string FormatTime(int totalSeconds)
        {
            if (totalSeconds <= 0) return "00:00";
            var ts = TimeSpan.FromSeconds(totalSeconds);
            return (ts.TotalHours >= 1)
                ? $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}"
                : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }
}
