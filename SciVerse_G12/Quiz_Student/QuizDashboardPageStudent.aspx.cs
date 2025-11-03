using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizDashboardPageStudent : System.Web.UI.Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private const int PASS_THRESHOLD = 50; // % considered "Completed"

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
            // Enforce login: must have RID in session
            if (Session["RID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadData();
                LoadChapterDropdown();
            }
        }

        // === Load quizzes + per-user attempts in ONE query (no Dictionary) ===
        private void LoadData(string keyword = "", string chapterFilter = "")
        {
            string sql = @"
SELECT
    q.QuizID,
    q.Title,
    q.Description,
    q.Chapter,
    q.TimeLimit,
    q.ImageURL,
    q.AttemptLimit,
    ISNULL(P.AttemptsTaken, 0) AS AttemptsTaken,
    ISNULL(P.BestPercent, 0)   AS BestPercent
FROM dbo.tblQuiz AS q
LEFT JOIN (
    /* Per-user progress:
       1) Compute each attempt's percentage
       2) Then aggregate per QuizID: AttemptsTaken + BestPercent (max of attempts) */
    SELECT S.QuizID,
           COUNT(*) AS AttemptsTaken,
           MAX(S.AttemptPercent) AS BestPercent
    FROM (
        SELECT qa.QuizID,
               qa.AttemptID,
               CASE WHEN SUM(r.Marks) > 0
                        THEN (SUM(r.Score) * 100.0 / SUM(r.Marks))
                    ELSE 0 END AS AttemptPercent
        FROM dbo.tblQuizAttempt qa
        LEFT JOIN dbo.tblQuizResult r ON r.AttemptID = qa.AttemptID
        WHERE qa.RID = @rid
        GROUP BY qa.QuizID, qa.AttemptID
    ) AS S
    GROUP BY S.QuizID
) AS P ON P.QuizID = q.QuizID
WHERE 1=1 ";

            bool hasKeyword = !string.IsNullOrWhiteSpace(keyword);
            bool hasChapter = !string.IsNullOrWhiteSpace(chapterFilter) && chapterFilter != "0";

            if (hasKeyword)
                sql += " AND (q.Title LIKE @kw OR q.Description LIKE @kw OR CONVERT(varchar(10), q.TimeLimit) LIKE @kw)";
            if (hasChapter)
                sql += " AND q.Chapter = @chap";

            // ✅ Simple numeric sort — no duplication issue
            sql += " ORDER BY q.Chapter, q.QuizID;";

            var dt = new DataTable();
            using (var con = new SqlConnection(connStr))
            using (var da = new SqlDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@rid", CurrentRid);
                if (hasKeyword) da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                if (hasChapter) da.SelectCommand.Parameters.AddWithValue("@chap", chapterFilter);
                da.Fill(dt);
            }

            Repeater_QuizCards.DataSource = dt;
            Repeater_QuizCards.DataBind();
        }


        // === Chapter dropdown ===
        // === Chapter dropdown ===
        private void LoadChapterDropdown()
        {
            const string sql = @"
        SELECT DISTINCT Chapter
        FROM dbo.tblQuiz
        WHERE Chapter IS NOT NULL
        ORDER BY Chapter;";

            var dt = new DataTable();
            using (var con = new SqlConnection(connStr))
            using (var da = new SqlDataAdapter(sql, con))
            {
                da.Fill(dt);
            }

            DropDownList_FilterByChapter.DataSource = dt;
            DropDownList_FilterByChapter.DataTextField = "Chapter";
            DropDownList_FilterByChapter.DataValueField = "Chapter";
            DropDownList_FilterByChapter.DataBind();
            DropDownList_FilterByChapter.Items.Insert(0, new ListItem("Select Chapter", "0"));
        }


        // === Search/Clear/Filter ===
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text.Trim(), DropDownList_FilterByChapter.SelectedValue);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            DropDownList_FilterByChapter.SelectedIndex = 0;
            LoadData();
        }

        protected void DropDownList_FilterByChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text.Trim(), DropDownList_FilterByChapter.SelectedValue);
        }

        // === Repeater commands ===
        protected void Repeater_QuizCards_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument?.ToString(), out int quizId) || quizId <= 0) return;

            if (e.CommandName == "start")
                Response.Redirect($"~/Quiz_Student/QuizDetailsPage.aspx?quizId={quizId}", false);
            else if (e.CommandName == "result")
                Response.Redirect($"~/Quiz_Student/QuizAttemptHistory.aspx?quizId={quizId}", false);
        }

        // === Repeater data-binding: compute AttemptsLeft and Status badge ===
        protected void Repeater_QuizCards_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var row = e.Item.DataItem as DataRowView;
            if (row == null) return;

            int attemptLimit = row["AttemptLimit"] == DBNull.Value ? 0 : Convert.ToInt32(row["AttemptLimit"]);
            int attemptsTaken = row["AttemptsTaken"] == DBNull.Value ? 0 : Convert.ToInt32(row["AttemptsTaken"]);
            double bestPct = row["BestPercent"] == DBNull.Value ? 0 : Convert.ToDouble(row["BestPercent"]);

            int attemptsLeft = Math.Max(0, attemptLimit - attemptsTaken);

            // Attempts Left label
            var lblAttemptsLeft = (Label)e.Item.FindControl("lblAttemptsLeft");
            if (lblAttemptsLeft != null)
                lblAttemptsLeft.Text = (attemptLimit <= 0) ? "Unavailable" : attemptsLeft.ToString();

            // Status badge
            string statusHtml;
            if (attemptLimit <= 0)
                statusHtml = BadgeLocked("Unavailable");
            else if (bestPct >= PASS_THRESHOLD)
                statusHtml = BadgeComplete("Completed");
            else if (attemptsLeft <= 0)
                statusHtml = BadgeLocked("No attempts left");
            else if (attemptsTaken > 0)
                statusHtml = BadgeProgress("In progress");
            else
                statusHtml = BadgeActive("Active");

            var litStatus = (Literal)e.Item.FindControl("litStatus");
            if (litStatus != null) litStatus.Text = statusHtml;

            // Hide Start ONLY when Unavailable or No attempts left (Completed still shows)
            var btnStart = (Button)e.Item.FindControl("btnStartQuiz");
            if (btnStart != null)
            {
                bool hideForUnavailable = (attemptLimit <= 0);
                bool hideForNoAttempts = (attemptLimit > 0 && attemptsLeft <= 0);
                btnStart.Visible = !(hideForUnavailable || hideForNoAttempts);
            }
        }

        // === Badge HTML helpers ===
        private static string BadgeActive(string text)
            => $"<span class='badge-active'><i class=\"fa-solid fa-circle-check\"></i> {text}</span>";

        private static string BadgeProgress(string text)
            => $"<span class='badge-progress'><i class=\"fa-solid fa-hourglass-half\"></i> {text}</span>";

        private static string BadgeComplete(string text)
            => $"<span class='badge-complete'><i class=\"fa-solid fa-trophy\"></i> {text}</span>";

        private static string BadgeLocked(string text)
            => $"<span class='badge-locked'><i class=\"fa-solid fa-lock\"></i> {text}</span>";
    }
}