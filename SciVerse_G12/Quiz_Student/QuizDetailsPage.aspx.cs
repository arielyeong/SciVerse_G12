//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace SciVerse_G12.Quiz_Student
//{
//    public partial class QuizDetailsPage : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (IsPostBack) return;

//            int quizId;
//            if (!int.TryParse(Request.QueryString["quizId"], out quizId) || quizId <= 0)
//            {
//                ShowError("Quiz not found");
//                return;
//            }

//            // If you enable login later:
//            // int userId = Session["RID"] == null ? 0 : Convert.ToInt32(Session["RID"]);
//            // if (userId == 0) Response.Redirect("~/Account/Login.aspx");
//            int userId = '5';
//            LoadQuiz(quizId , userId);
//        }

//        private void ShowError(string msg)
//        {
//            titleLiteral.Text = Server.HtmlEncode(msg);
//            btnStart.Enabled = false;
//        }

//        private void LoadQuiz(int quizId , int userId)
//        {
//            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

//            // Use the REAL column name "Description" (not Ddescription)
//            const string sqlQuiz = @"
//                SELECT Title, Description, TimeLimit, AttemptLimit
//                FROM tblQuiz
//                WHERE QuizID = @QuizID;";

//            const string sqlQCount = @"SELECT COUNT(*) FROM tblQuestion WHERE QuizID = @QuizID;";

//            // Only used if you have userId > 0 (per-user attempt counting)
//            const string sqlAttemptsUsed = @"
//                SELECT COUNT(*) 
//                FROM tblQuizAttempt 
//                WHERE QuizID = @QuizID AND RID = @RID;";

//            using (var con = new SqlConnection(cs))
//            {
//                con.Open();

//                // 1) Quiz meta
//                string title = "Untitled";
//                string desc = "";
//                int timeLimitMinutes = 0;   // assume TimeLimit stored in minutes
//                int attemptLimit = 0;

//                using (var cmd = new SqlCommand(sqlQuiz, con))
//                {
//                    cmd.Parameters.AddWithValue("@QuizID", quizId);
//                    using (var r = cmd.ExecuteReader(CommandBehavior.SingleRow))
//                    {
//                        if (!r.Read())
//                        {
//                            ShowError("Quiz not found");
//                            return;
//                        }

//                        title = r["Title"] == DBNull.Value ? "Untitled" : r["Title"].ToString();
//                        desc = r["Description"] == DBNull.Value ? "" : r["Description"].ToString();
//                        timeLimitMinutes = r["TimeLimit"] == DBNull.Value ? 0 : Convert.ToInt32(r["TimeLimit"]);
//                        attemptLimit = r["AttemptLimit"] == DBNull.Value ? 0 : Convert.ToInt32(r["AttemptLimit"]);
//                    }
//                }

//                // 2) Question count
//                int qCount;
//                using (var cmd = new SqlCommand(sqlQCount, con))
//                {
//                    cmd.Parameters.AddWithValue("@QuizID", quizId);
//                    qCount = Convert.ToInt32(cmd.ExecuteScalar());
//                }

//                // 3) Attempts used (skip if you don’t have login yet)
//                //int used = 0;
//                // int userId = Session["RID"] == null ? 0 : Convert.ToInt32(Session["RID"]);
//                //int userId = 0; // TEMP: while login not wired

//                //if (attemptLimit > 0 && userId > 0)
//                //{
//                //    using (var cmd = new SqlCommand(sqlAttemptsUsed, con))
//                //    {
//                //        cmd.Parameters.AddWithValue("@QuizID", quizId);
//                //        cmd.Parameters.AddWithValue("@RID", userId);
//                //        used = Convert.ToInt32(cmd.ExecuteScalar());
//                //    }
//                //}
//                int used = 0;
//                if (attemptLimit > 0 && userId > 0)
//                {
//                    using (var cmd = new SqlCommand(sqlAttemptsUsed, con))
//                    {
//                        cmd.Parameters.AddWithValue("@QuizID", quizId);
//                        cmd.Parameters.AddWithValue("@RID", userId);
//                        used = Convert.ToInt32(cmd.ExecuteScalar());
//                    }
//                }
//                int remaining = (attemptLimit > 0) ? Math.Max(attemptLimit - used, 0) : 999; // 999=Unlimited

//                // 4) Bind UI
//                titleLiteral.Text = Server.HtmlEncode(title);
//                litDescription.Text = Server.HtmlEncode(desc);
//                lblTimeLimit.Text = timeLimitMinutes > 0 ? (timeLimitMinutes + " min") : "No time limit";
//                lblQuestions.Text = qCount + " Questions";
//                lblAttempts.Text = remaining == 999 ? "Unlimited" : (remaining + " / " + attemptLimit);

//                // If no time limit, hide radio and force untimed
//                if (timeLimitMinutes == 0)
//                {
//                    rblTimeMode.Visible = false;
//                    ViewState["ForceUntimed"] = true;
//                }

//                // Keep state for btnStart_Click
//                ViewState["QuizId"] = quizId;
//                ViewState["TimeLimitMin"] = timeLimitMinutes; // minutes
//                ViewState["AttemptLimit"] = attemptLimit;
//                ViewState["AttemptsUsed"] = used;
//            }
//        }

//        //protected void btnStart_Click(object sender, EventArgs e)
//        //{
//        //    int quizId = (int)(ViewState["QuizId"] ?? 0);
//        //    if (quizId == 0) return;

//        //    int attemptLimit = (int)(ViewState["AttemptLimit"] ?? 0);
//        //    int used = (int)(ViewState["AttemptsUsed"] ?? 0);

//        //    if (attemptLimit > 0 && used >= attemptLimit)
//        //    {
//        //        ShowError("You have reached the maximum number of attempts for this quiz.");
//        //        btnStart.Enabled = false;
//        //        return;
//        //    }

//        //    bool forceUntimed = ViewState["ForceUntimed"] as bool? ?? false;
//        //    string mode = forceUntimed ? "untimed" : rblTimeMode.SelectedValue; // "timed"/"untimed"

//        //    int timeLimitMin = (int)(ViewState["TimeLimitMin"] ?? 0);
//        //    int sec = timeLimitMin * 60;

//        //    // Quiz.aspx expects: quizId, timed (0/1), sec
//        //    string timedFlag = (mode == "timed" && timeLimitMin > 0) ? "1" : "0";

//        //    //string url = ResolveUrl($"~/Quiz_Student/Quiz.aspx?quizId={quizId}&timed={timedFlag}&sec={sec}");
//        //    //Response.Redirect(url, false);
//        //    //Context.ApplicationInstance.CompleteRequest(); // cleaner redirect
//        //    Response.Redirect("~/Quiz_Student/Quiz.aspx?quizId=" + quizId + "&timed=" + timedFlag + "&sec=" + sec);

//        //}
//        protected void btnStart_Click(object sender, EventArgs e)
//        {
//            int quizId = (int)(ViewState["QuizId"] ?? 0);
//            if (quizId == 0) return;

//            int attemptLimit = (int)(ViewState["AttemptLimit"] ?? 0);
//            int used = (int)(ViewState["AttemptsUsed"] ?? 0);

//            if (attemptLimit > 0 && used >= attemptLimit)
//            {
//                ShowError("You have reached the maximum number of attempts for this quiz.");
//                btnStart.Enabled = false;
//                return;
//            }

//            bool forceUntimed = ViewState["ForceUntimed"] as bool? ?? false;
//            string mode = forceUntimed ? "untimed" : rblTimeMode.SelectedValue; // "timed"/"untimed"

//            int timeLimitMin = (int)(ViewState["TimeLimitMin"] ?? 0);
//            int totalSec = timeLimitMin * 60;
//            int userId = (int)(ViewState["UserId"] ?? 5); // default fallback

//            // ✅ Correct redirect path
//            string url = $"~/Quiz_Student/Quiz.aspx?quizId={quizId}&timed={(mode == "timed" ? "1" : "0")}&sec={totalSec}&rid={userId}";

//            // Optional: pass totalSec if you want (but Quiz.aspx reads TimeLimit from DB)
//            // url += $"&sec={totalSec}";

//            Response.Redirect(url, false);
//            Context.ApplicationInstance.CompleteRequest();
//        }
//    }
//}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizDetailsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int quizId;
            if (!int.TryParse(Request.QueryString["quizId"], out quizId) || quizId <= 0)
            {
                ShowError("Quiz not found");
                return;
            }

            // ✅ Temporarily hardcode userId (replace with Session["RID"] later)
            int userId = 5;

            LoadQuiz(quizId, userId);
        }

        private void ShowError(string msg)
        {
            titleLiteral.Text = Server.HtmlEncode(msg);
            btnStart.Enabled = false;
        }

        private void LoadQuiz(int quizId, int userId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            const string sqlQuiz = @"
                SELECT Title, Description, TimeLimit, AttemptLimit
                FROM tblQuiz
                WHERE QuizID = @QuizID;";

            const string sqlQCount = @"SELECT COUNT(*) FROM tblQuestion WHERE QuizID = @QuizID;";

            const string sqlAttemptsUsed = @"
                SELECT COUNT(*) 
                FROM tblQuizAttempt 
                WHERE QuizID = @QuizID AND RID = @RID;";

            using (var con = new SqlConnection(cs))
            {
                con.Open();

                string title = "Untitled";
                string desc = "";
                int timeLimitMinutes = 0;
                int attemptLimit = 0;

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

                int qCount;
                using (var cmd = new SqlCommand(sqlQCount, con))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    qCount = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // ✅ Use the default userId (5) for attempts
                int used = 0;
                if (attemptLimit > 0 && userId > 0)
                {
                    using (var cmd = new SqlCommand(sqlAttemptsUsed, con))
                    {
                        cmd.Parameters.AddWithValue("@QuizID", quizId);
                        cmd.Parameters.AddWithValue("@RID", userId);
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                int remaining = (attemptLimit > 0)
                    ? Math.Max(attemptLimit - used, 0)
                    : 999; // 999 = Unlimited

                titleLiteral.Text = Server.HtmlEncode(title);
                litDescription.Text = Server.HtmlEncode(desc);
                lblTimeLimit.Text = timeLimitMinutes > 0
                    ? (timeLimitMinutes + " min")
                    : "No time limit";
                lblQuestions.Text = qCount + " Questions";
                lblAttempts.Text = remaining == 999
                    ? "Unlimited"
                    : (remaining + " / " + attemptLimit);

                if (timeLimitMinutes == 0)
                {
                    rblTimeMode.Visible = false;
                    ViewState["ForceUntimed"] = true;
                }

                // ✅ store state + default userId
                ViewState["QuizId"] = quizId;
                ViewState["TimeLimitMin"] = timeLimitMinutes;
                ViewState["AttemptLimit"] = attemptLimit;
                ViewState["AttemptsUsed"] = used;
                ViewState["UserId"] = userId; // new line
            }
        }

        //protected void btnStart_Click(object sender, EventArgs e)
        //{
        //    int quizId = (int)(ViewState["QuizId"] ?? 0);
        //    if (quizId == 0) return;

        //    int attemptLimit = (int)(ViewState["AttemptLimit"] ?? 0);
        //    int used = (int)(ViewState["AttemptsUsed"] ?? 0);

        //    if (attemptLimit > 0 && used >= attemptLimit)
        //    {
        //        ShowError("You have reached the maximum number of attempts for this quiz.");
        //        btnStart.Enabled = false;
        //        return;
        //    }

        //    bool forceUntimed = ViewState["ForceUntimed"] as bool? ?? false;
        //    string mode = forceUntimed ? "untimed" : rblTimeMode.SelectedValue;

        //    int timeLimitMin = (int)(ViewState["TimeLimitMin"] ?? 0);
        //    int totalSec = timeLimitMin * 60;
        //    int userId = (int)(ViewState["UserId"] ?? 5); // default fallback

        //    // ✅ Correct redirect path
        //    string url = $"~/Quiz_Student/Quiz.aspx?quizId={quizId}&timed={(mode == "timed" ? "1" : "0")}&sec={totalSec}&rid={userId}";
        //    Response.Redirect(url, false);
        //    Context.ApplicationInstance.CompleteRequest();
        //}
        protected void btnStart_Click(object sender, EventArgs e)
        {
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

            // pick mode
            bool forceUntimed = (bool?)ViewState["ForceUntimed"] ?? false;
            string mode = forceUntimed ? "untimed" : (rblTimeMode.SelectedValue ?? "untimed");

            // TEMP: ensure a RID exists for Quiz.aspx (it reads Session["RID"])
            if (Session["RID"] == null) Session["RID"] = 5;

            // ✅ Only pass what Quiz.aspx expects
            string url = ResolveUrl($"~/Quiz_Student/Quiz.aspx?QuizID={quizId}&mode={mode}");
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
