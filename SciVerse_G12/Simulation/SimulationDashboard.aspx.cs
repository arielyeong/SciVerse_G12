using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Simulation
{
    public partial class SimulationDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["RID"] == null || Session["Username"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSimulationsByChapter();
            }
        }

        private void LoadSimulationsByChapter()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            Dictionary<string, List<SimulationItem>> simulationsByChapter = new Dictionary<string, List<SimulationItem>>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Join with tblAccessSimulation to get LastModify for the current user
                string query = @"
                    SELECT 
                        s.SimulationID,
                        s.Title,
                        s.Description,
                        s.Chapter,
                        s.Instruction,
                        s.Image,
                        a.LastModify
                    FROM tblExperimentSimulation s
                    LEFT JOIN tblAccessSimulation a ON s.SimulationID = a.SimulationID AND a.RID = @UserRID
                    WHERE s.Chapter IS NOT NULL AND s.Chapter != ''
                    ORDER BY s.Chapter, s.SimulationID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserRID", Session["RID"]);
                    
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string chapter = reader["Chapter"] != DBNull.Value ? reader["Chapter"].ToString() : "Unknown";
                            if (!simulationsByChapter.ContainsKey(chapter))
                            {
                                simulationsByChapter[chapter] = new List<SimulationItem>();
                            }

                            simulationsByChapter[chapter].Add(new SimulationItem
                            {
                                SimulationID = Convert.ToInt32(reader["SimulationID"]),
                                Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : "",
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "",
                                Chapter = chapter,
                                Instruction = reader["Instruction"] != DBNull.Value ? reader["Instruction"].ToString() : "",
                                Image = reader["Image"] != DBNull.Value ? reader["Image"].ToString() : "",
                                LastModify = reader["LastModify"] != DBNull.Value ? Convert.ToDateTime(reader["LastModify"]) : (DateTime?)null
                            });
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading simulations: {ex.Message}");
                    }
                }
            }

            // Bind data to repeater
            if (simulationsByChapter.Count > 0)
            {
                rptChapters.DataSource = simulationsByChapter.OrderBy(kvp => kvp.Key);
                rptChapters.DataBind();
                lblEmptyState.Visible = false;
            }
            else
            {
                rptChapters.Visible = false;
                lblEmptyState.Visible = true;
            }
        }

        protected void rptChapters_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeyValuePair<string, List<SimulationItem>> chapterData = (KeyValuePair<string, List<SimulationItem>>)e.Item.DataItem;
                Repeater rptExperiments = (Repeater)e.Item.FindControl("rptExperiments");
                
                if (rptExperiments != null)
                {
                    rptExperiments.DataSource = chapterData.Value;
                    rptExperiments.DataBind();
                }
            }
        }

        protected void rptExperiments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SimulationItem item = (SimulationItem)e.Item.DataItem;
                Image imgPreview = (Image)e.Item.FindControl("imgPreview");
                Label lblNoPreview = (Label)e.Item.FindControl("lblNoPreview");
                Label lblLastAccessed = (Label)e.Item.FindControl("lblLastAccessed");
                
                if (!string.IsNullOrEmpty(item.Image))
                {
                    imgPreview.ImageUrl = item.Image;
                    imgPreview.Visible = true;
                    lblNoPreview.Visible = false;
                }
                else
                {
                    imgPreview.Visible = false;
                    lblNoPreview.Visible = true;
                }
                
                // Display Last Accessed date
                if (item.LastModify.HasValue)
                {
                    lblLastAccessed.Text = "<span class='detail-label'>Last Accessed:</span><span class='detail-value'>" + 
                        item.LastModify.Value.ToString("dd MMM yyyy HH:mm") + "</span>";
                }
                else
                {
                    lblLastAccessed.Text = "<span class='detail-label'>Last Accessed:</span><span class='detail-value'>Never</span>";
                }
            }
        }

        public class SimulationItem
        {
            public int SimulationID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Chapter { get; set; }
            public string Instruction { get; set; }
            public string Image { get; set; }
            public DateTime? LastModify { get; set; }
        }
    }
}