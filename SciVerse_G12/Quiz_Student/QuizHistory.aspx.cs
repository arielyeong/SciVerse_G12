using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizHistory : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private const int PASS_PCT = 50;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindFilters();
                RefreshAll();
            }
        }

        private void RefreshAll()
        {
            BindKpis();
            BindGrid();
            BindCharts();
            BindChapterPerformance(); // new graph
        }

        // ---------------- FILTER DROPDOWNS ----------------
        private void BindFilters()
        {
            using (var con = new SqlConnection(connStr))
            {
                con.Open();

                using (var da = new SqlDataAdapter("SELECT DISTINCT q.QuizID, q.Title FROM dbo.tblQuiz q ORDER BY q.Title", con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    ddlQuiz.DataSource = dt;
                    ddlQuiz.DataTextField = "Title";
                    ddlQuiz.DataValueField = "QuizID";
                    ddlQuiz.DataBind();
                }

                using (var da = new SqlDataAdapter("SELECT DISTINCT Chapter FROM dbo.tblQuiz ORDER BY Chapter", con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    ddlChapter.DataSource = dt;
                    ddlChapter.DataTextField = "Chapter";
                    ddlChapter.DataValueField = "Chapter";
                    ddlChapter.DataBind();
                }
            }
        }

        // ---------------- BASE QUERY ----------------
        private string BuildBaseSelect(bool includeOrderBy)
        {
            var sql = @"
                SELECT pa.AttemptID, pa.RID, pa.QuizID, pa.AttemptDate, pa.Duration,
                       pa.ScoreGot, pa.ScoreMax,
                       q.Title, q.Chapter,
                       CASE WHEN pa.ScoreMax>0 THEN (pa.ScoreGot*100.0/pa.ScoreMax) ELSE 0 END AS ScorePct
                FROM (
                    SELECT a.AttemptID, a.RID, a.QuizID, a.AttemptDate, a.Duration,
                           SUM(r.Score) AS ScoreGot,
                           SUM(r.Marks) AS ScoreMax
                    FROM dbo.tblQuizAttempt a
                    JOIN dbo.tblQuizResult r ON r.AttemptID = a.AttemptID
                    WHERE a.RID = @RID
                      AND a.Status IN ('Submitted','InProgress','Paused')
                    GROUP BY a.AttemptID, a.RID, a.QuizID, a.AttemptDate, a.Duration
                ) AS pa
                JOIN dbo.tblQuiz q ON q.QuizID = pa.QuizID
                WHERE 1=1
                ";
            if (!string.IsNullOrEmpty(ddlQuiz.SelectedValue))
                sql += " AND pa.QuizID = @QID";
            if (!string.IsNullOrEmpty(ddlChapter.SelectedValue))
                sql += " AND q.Chapter = @Chapter";
            if (!string.IsNullOrEmpty(dtDate.Text))
                sql += " AND CONVERT(date, pa.AttemptDate) = @Dt";
            if (includeOrderBy)
                sql += " ORDER BY pa.AttemptDate ASC";
            return sql;
        }

        private SqlCommand CreateBaseCommand(SqlConnection con, bool includeOrderBy)
        {
            var cmd = new SqlCommand(BuildBaseSelect(includeOrderBy), con);
            cmd.Parameters.AddWithValue("@RID", Convert.ToInt32(Session["RID"]));
            if (!string.IsNullOrEmpty(ddlQuiz.SelectedValue))
                cmd.Parameters.AddWithValue("@QID", Convert.ToInt32(ddlQuiz.SelectedValue));
            if (!string.IsNullOrEmpty(ddlChapter.SelectedValue))
                cmd.Parameters.AddWithValue("@Chapter", ddlChapter.SelectedValue);
            if (!string.IsNullOrEmpty(dtDate.Text))
                cmd.Parameters.AddWithValue("@Dt", DateTime.Parse(dtDate.Text).Date);
            return cmd;
        }

        // ---------------- KPIs ----------------
        private void BindKpis()
        {
            using (var con = new SqlConnection(connStr))
            using (var baseCmd = CreateBaseCommand(con, false))
            {
                var kpisql = @"
                SELECT 
                  COUNT(*) AS TotalAttempts,
                  CAST((SUM(b.ScoreGot) * 100.0 / NULLIF(SUM(b.ScoreMax), 0)) AS DECIMAL(5,2)) AS OverallPct,
                  SUM(CASE WHEN (CASE WHEN b.ScoreMax>0 THEN (b.ScoreGot * 100.0 / b.ScoreMax) ELSE 0 END) >= @PASS THEN 1 ELSE 0 END) AS Passed
                FROM (" + baseCmd.CommandText + ") AS b;";

                using (var cmd = new SqlCommand(kpisql, con))
                {
                    foreach (SqlParameter p in baseCmd.Parameters)
                        cmd.Parameters.Add((SqlParameter)((ICloneable)p).Clone());
                    cmd.Parameters.AddWithValue("@PASS", PASS_PCT);

                    con.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            int total = rd["TotalAttempts"] == DBNull.Value ? 0 : Convert.ToInt32(rd["TotalAttempts"]);
                            decimal pct = rd["OverallPct"] == DBNull.Value ? 0 : Convert.ToDecimal(rd["OverallPct"]);
                            int passed = rd["Passed"] == DBNull.Value ? 0 : Convert.ToInt32(rd["Passed"]);

                            litTotalAttempts.Text = total.ToString();
                            litAvgPct.Text = Math.Round(pct, 0).ToString("0");
                            litPassed.Text = passed.ToString();
                        }
                        else
                        {
                            litTotalAttempts.Text = "0";
                            litAvgPct.Text = "0";
                            litPassed.Text = "0";
                        }
                    }
                }
            }
        }

        // ---------------- GRID ----------------
        private void BindGrid()
        {
            using (var con = new SqlConnection(connStr))
            using (var baseCmd = CreateBaseCommand(con, false))
            {
                var gridsql = @"
                    SELECT 
                      b.AttemptID, b.Title, b.Chapter,
                      ROW_NUMBER() OVER (PARTITION BY b.QuizID ORDER BY b.AttemptDate) AS AttemptNo,
                      CAST(b.AttemptDate AS date) AS AttemptDate,
                      CONCAT(CAST(b.ScoreGot AS int),' / ',CAST(b.ScoreMax AS int)) AS ScoreText,
                      --  FORMAT seconds as mm:ss for display
                      RIGHT('0' + CAST((ISNULL(b.Duration,0)/60) AS varchar(2)), 2)
                      + ':' +
                      RIGHT('0' + CAST((ISNULL(b.Duration,0)%60) AS varchar(2)), 2) AS DurationText
                    FROM (
                    " + baseCmd.CommandText + @"
                    ) AS b
                    ORDER BY b.AttemptDate DESC;";

                using (var cmd = new SqlCommand(gridsql, con))
                {
                    foreach (SqlParameter p in baseCmd.Parameters)
                        cmd.Parameters.Add((SqlParameter)((ICloneable)p).Clone());

                    var dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    gvAttempts.DataSource = dt;
                    gvAttempts.DataBind();
                }
            }
        }

        // ---------------- CHARTS ----------------
        private void BindCharts()
        {
            using (var con = new SqlConnection(connStr))
            using (var baseCmd = CreateBaseCommand(con, true))
            {
                var dt = new DataTable();
                using (var cmd = new SqlCommand(baseCmd.CommandText, con))
                {
                    foreach (SqlParameter p in baseCmd.Parameters)
                        cmd.Parameters.Add((SqlParameter)((ICloneable)p).Clone());
                    new SqlDataAdapter(cmd).Fill(dt);
                }

                chartTrend.Series["Scores"].Points.Clear();
                chartPassFail.Series["PF"].Points.Clear();

                foreach (DataRow r in dt.Rows)
                {
                    chartTrend.Series["Scores"].Points.AddXY(Convert.ToDateTime(r["AttemptDate"]), Convert.ToDouble(r["ScorePct"]));
                }

                int pass = 0, fail = 0;
                foreach (DataRow r in dt.Rows)
                {
                    double pct = Convert.ToDouble(r["ScorePct"]);
                    if (pct >= PASS_PCT) pass++; else fail++;
                }
                chartPassFail.Series["PF"].Points.AddXY("Pass", pass);
                chartPassFail.Series["PF"].Points.AddXY("Fail", fail);
            }
        }

        // ---------------- CHAPTER PERFORMANCE GRAPH ----------------
        private void BindChapterPerformance()
        {
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
        SELECT qz.Chapter,
               AVG(CASE WHEN r.Score IS NULL THEN 0 
                        ELSE (CAST(r.Score AS FLOAT)/NULLIF(q.Marks,0)) END) * 100 AS AvgPct
        FROM dbo.tblQuizAttempt a
        JOIN dbo.tblQuizResult r ON r.AttemptID = a.AttemptID
        JOIN dbo.tblQuestion q ON q.QuestionID = r.Question
        JOIN dbo.tblQuiz qz ON qz.QuizID = a.QuizID
        WHERE a.RID = @RID AND a.Status = 'Submitted'
        GROUP BY qz.Chapter
        ORDER BY qz.Chapter;", con))
            {
                cmd.Parameters.AddWithValue("@RID", Convert.ToInt32(Session["RID"]));
                con.Open();

                var s = chartChapterPerf.Series["Chapters"];
                s.Points.Clear();
                s.ChartType = SeriesChartType.Column;
                s.IsValueShownAsLabel = true;
                s.LabelFormat = "0'%'";

                var area = chartChapterPerf.ChartAreas["AreaChapter"];
                area.AxisY.Minimum = 0;
                area.AxisY.Maximum = 100;
                area.AxisY.Title = "Average Score (%)";
                area.AxisX.Title = "Chapter";

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        string chapter = rd["Chapter"]?.ToString() ?? "(N/A)";
                        double avgLegend = rd["AvgPct"] == DBNull.Value ? 0 : Convert.ToDouble(rd["AvgPct"]);

                        // add bar
                        int idx = s.Points.AddXY(chapter, avgLegend);
                        var pt = s.Points[idx];

                        // color by threshold
                        if (avgLegend >= 75.0)
                            pt.Color = Color.FromArgb(34, 197, 94);   // green
                        else if (avgLegend >= 50.0)
                            pt.Color = Color.FromArgb(234, 179, 8);   // amber
                        else
                            pt.Color = Color.FromArgb(239, 68, 68);   // red
                    }
                }

                // simple legend: Good / Average / Poor
                chartChapterPerf.Legends.Clear();
                var legend = new Legend("LegendChapter") { Docking = Docking.Bottom };
                chartChapterPerf.Legends.Add(legend);

                LegendItem good = new LegendItem("≥ 75% (Good)", Color.FromArgb(34, 197, 94), "");
                LegendItem avg = new LegendItem("50–74% (Average)", Color.FromArgb(234, 179, 8), "");
                LegendItem poor = new LegendItem("< 50% (Poor)", Color.FromArgb(239, 68, 68), "");
                legend.CustomItems.Add(good);
                legend.CustomItems.Add(avg);
                legend.CustomItems.Add(poor);
            }
        }

        // ---------------- BUTTON EVENTS ----------------
        protected void btnFilter_Click(object sender, EventArgs e) => RefreshAll();
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlQuiz.SelectedIndex = 0;
            ddlChapter.SelectedIndex = 0;
            dtDate.Text = string.Empty;
            RefreshAll();
        }
        protected void gvAttempts_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvAttempts.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}
