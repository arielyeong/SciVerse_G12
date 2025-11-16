using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Admin
{
    public partial class QuizReport : System.Web.UI.Page
    {
        private const int PASS_THRESHOLD = 50; // percent
        private string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["RID"] == null)
            //{
            //    Response.Redirect("~/Account/Login.aspx");
            //    return;
            //}

            if (!IsPostBack)
            {
                LoadFilters();
                BindReport();
            }
        }

        private void LoadFilters()
        {
            // === Quiz dropdown ===
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand("SELECT QuizID, Title FROM dbo.tblQuiz ORDER BY Title;", con))
            {
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    ddlQuiz.Items.Clear();
                    ddlQuiz.Items.Add(new ListItem("-- All --", ""));
                    while (rd.Read())
                        ddlQuiz.Items.Add(new ListItem(rd["Title"].ToString(), rd["QuizID"].ToString()));
                }
            }

            // === Chapter dropdown ===
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(@"
                SELECT DISTINCT 
                    Chapter, TRY_CONVERT(int, Chapter) AS ChapNum
                FROM dbo.tblQuiz
                WHERE Chapter IS NOT NULL AND LTRIM(RTRIM(Chapter)) <> ''
                ORDER BY ChapNum, Chapter;", con))
            {
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    ddlChapter.Items.Clear();
                    ddlChapter.Items.Add(new ListItem("-- All --", ""));
                    while (rd.Read())
                        ddlChapter.Items.Add(new ListItem(rd["Chapter"].ToString(), rd["Chapter"].ToString()));
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvReport.PageIndex = 0;
            BindReport();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStudentId.Text = "";
            ddlQuiz.SelectedIndex = 0;
            ddlChapter.SelectedIndex = 0;
            txtDate.Text = "";
            gvReport.PageIndex = 0;
            BindReport();
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindReport();
        }

        private void BindReport()
        {
            var where = new StringBuilder(" WHERE 1=1 ");
            var cmd = new SqlCommand();

            if (!string.IsNullOrWhiteSpace(txtStudentId.Text))
            {
                where.Append(" AND qa.RID = @RID ");
                if (int.TryParse(txtStudentId.Text.Trim(), out int rid))
                    cmd.Parameters.AddWithValue("@RID", rid);
                else
                    cmd.Parameters.AddWithValue("@RID", -1);
            }

            if (!string.IsNullOrEmpty(ddlQuiz.SelectedValue))
            {
                where.Append(" AND qa.QuizID = @QuizID ");
                cmd.Parameters.AddWithValue("@QuizID", int.Parse(ddlQuiz.SelectedValue));
            }

            if (!string.IsNullOrEmpty(ddlChapter.SelectedValue))
            {
                where.Append(" AND q.Chapter = @Chapter ");
                cmd.Parameters.AddWithValue("@Chapter", ddlChapter.SelectedValue);
            }

            if (!string.IsNullOrWhiteSpace(txtDate.Text))
            {
                if (DateTime.TryParse(txtDate.Text, out DateTime day))
                {
                    where.Append(" AND qa.AttemptDate >= @D1 AND qa.AttemptDate < @D2 ");
                    cmd.Parameters.AddWithValue("@D1", day.Date);
                    cmd.Parameters.AddWithValue("@D2", day.Date.AddDays(1));
                }
            }

            string sql = $@"
                SELECT
                    qa.AttemptID,
                    qa.RID AS StudentID,
                    ISNULL(u.Username, '-') AS StudentName,
                    q.Title AS QuizTitle,
                    q.Chapter,
                    qa.AttemptDate,
                    SUM(r.Marks) AS TotalMarks,
                    SUM(r.Score) AS Score,
                    CASE WHEN SUM(r.Marks) > 0 THEN (SUM(r.Score)*100.0 / SUM(r.Marks)) ELSE 0 END AS [Percent]
                FROM dbo.tblQuizAttempt qa
                JOIN dbo.tblQuiz q ON q.QuizID = qa.QuizID
                LEFT JOIN dbo.tblQuizResult r ON r.AttemptID = qa.AttemptID
                LEFT JOIN dbo.tblRegisteredUsers u ON u.RID = qa.RID
                {where}
                GROUP BY qa.AttemptID, qa.RID, u.Username, q.Title, q.Chapter, qa.AttemptDate
                ORDER BY qa.AttemptDate DESC, qa.AttemptID DESC;";

            DataTable dt = new DataTable();
            using (var con = new SqlConnection(CS))
            {
                cmd.CommandText = sql;
                cmd.Connection = con;
                using (var da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
            }

            // Pass/fail logic
            if (!dt.Columns.Contains("StatusText"))
                dt.Columns.Add("StatusText", typeof(string));

            int totalAttempts = dt.Rows.Count, passCount = 0;
            double avgPercent = 0;
            foreach (DataRow row in dt.Rows)
            {
                double pct = row["Percent"] == DBNull.Value ? 0.0 : Convert.ToDouble(row["Percent"]);
                // convert to 2 decimal places
                pct = Math.Round(pct, 2);
                row["Percent"] = pct;

                bool pass = pct >= PASS_THRESHOLD;
                row["StatusText"] = pass ? "Pass" : "Fail";
                if (pass) passCount++;
                avgPercent += pct;
            }

            gvReport.DataSource = dt;
            gvReport.DataBind();

            lblTotalAttempts.Text = totalAttempts.ToString();
            lblAverageScore.Text = totalAttempts > 0 ? (avgPercent / totalAttempts).ToString("0.#") + "%" : "0%";
            lblPassRate.Text = totalAttempts > 0 ? ((passCount * 100.0 / totalAttempts).ToString("0.#") + "%") : "0%";
        }

        // ===== NEW FEATURE 1: User Overall Performance =====
        protected void btnUserReport_Click(object sender, EventArgs e)
        {
            string sql = @"
                SELECT 
                    qa.RID AS StudentID,
                    ISNULL(u.Username, '-') AS StudentName,
                    COUNT(DISTINCT qa.AttemptID) AS TotalAttempts,
                    COUNT(DISTINCT qa.QuizID) AS QuizzesTaken,
                    AVG(CASE WHEN r.Marks > 0 THEN (r.Score*100.0 / r.Marks) ELSE 0 END) AS AvgPercent,
                    SUM(CASE WHEN (r.Score*100.0 / NULLIF(r.Marks,0)) >= 50 THEN 1 ELSE 0 END) AS PassCount
                FROM dbo.tblQuizAttempt qa
                JOIN dbo.tblQuiz q ON q.QuizID = qa.QuizID
                LEFT JOIN dbo.tblQuizResult r ON r.AttemptID = qa.AttemptID
                LEFT JOIN dbo.tblRegisteredUsers u ON u.RID = qa.RID
                GROUP BY qa.RID, u.Username
                ORDER BY qa.RID;";

            DataTable dt = new DataTable();
            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con))
                da.Fill(dt);

            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        // ===== NEW FEATURE 2: Export User Report to Excel =====
        protected void btnExportUserReport_Click(object sender, EventArgs e)
        {
            string sql = @"
            SELECT 
                qa.RID AS StudentID,
                ISNULL(u.Username, '-') AS StudentName,
                COUNT(DISTINCT qa.AttemptID) AS TotalAttempts,
                COUNT(DISTINCT qa.QuizID)   AS QuizzesTaken,
                AVG(CASE WHEN r.Marks > 0 THEN (r.Score*100.0 / r.Marks) ELSE 0 END) AS AvgPercent,
                SUM(CASE WHEN (r.Score*100.0 / NULLIF(r.Marks,0)) >= 50 THEN 1 ELSE 0 END) AS PassCount
            FROM dbo.tblQuizAttempt qa
            JOIN dbo.tblQuiz q ON q.QuizID = qa.QuizID
            LEFT JOIN dbo.tblQuizResult r ON r.AttemptID = qa.AttemptID
            LEFT JOIN dbo.tblRegisteredUsers u ON u.RID = qa.RID
            GROUP BY qa.RID, u.Username
            ORDER BY qa.RID;";

            var dt = new DataTable();
            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con)) { da.Fill(dt); }

            var sb = new StringBuilder();
            // header
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append("\"").Append(dt.Columns[i].ColumnName.Replace("\"", "\"\"")).Append("\"");
            }
            sb.AppendLine();
            // rows
            foreach (DataRow r in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(",");
                    var val = r[i]?.ToString() ?? "";
                    sb.Append("\"").Append(val.Replace("\"", "\"\"")).Append("\"");
                }
                sb.AppendLine();
            }

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=UserPerformanceReport.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
        }

        protected void gvReport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
