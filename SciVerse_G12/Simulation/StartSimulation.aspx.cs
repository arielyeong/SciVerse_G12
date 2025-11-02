using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Simulation
{
    public partial class StartSimulation : System.Web.UI.Page
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
                string simulationId = Request.QueryString["id"];
                
                if (string.IsNullOrEmpty(simulationId))
                {
                    Response.Redirect("~/Simulation/SimulationDashboard.aspx");
                    return;
                }

                if (int.TryParse(simulationId, out int id))
                {
                    LoadSimulationDetails(id);
                }
                else
                {
                    Response.Redirect("~/Simulation/SimulationDashboard.aspx");
                }
            }
        }

        private void LoadSimulationDetails(int simulationId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        Title,
                        Description,
                        Instruction,
                        Image
                    FROM tblExperimentSimulation
                    WHERE SimulationID = @SimulationID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SimulationID", simulationId);
                    
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        if (reader.Read())
                        {
                            h1Title.InnerText = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : "Experiment";
                            divDescription.InnerText = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                            divInstruction.InnerText = reader["Instruction"] != DBNull.Value ? reader["Instruction"].ToString() : "";
                            
                            // Load image if available
                            string imageUrl = reader["Image"] != DBNull.Value ? reader["Image"].ToString() : "";
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                                imgPreview.ImageUrl = imageUrl;
                                imgPreview.Visible = true;
                                lblNoPreview.Visible = false;
                            }
                            else
                            {
                                imgPreview.Visible = false;
                                lblNoPreview.Visible = true;
                            }
                        }
                        else
                        {
                            Response.Redirect("~/Simulation/SimulationDashboard.aspx");
                        }
                        
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading simulation details: {ex.Message}");
                        Response.Redirect("~/Simulation/SimulationDashboard.aspx");
                    }
                }
            }
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            string simulationId = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(simulationId) && int.TryParse(simulationId, out int simId))
            {
                // Log the simulation access
                LogSimulationAccess(simId);
                
                Response.Redirect($"~/Simulation/Experiment.aspx?id={simulationId}");
            }
            else
            {
                Response.Redirect("~/Simulation/SimulationDashboard.aspx");
            }
        }

        private void LogSimulationAccess(int simulationId)
        {
            // Check if user is logged in
            if (Session["RID"] == null)
            {
                return; // User not logged in, don't log
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        MERGE INTO tblAccessSimulation AS target
                        USING (SELECT @RID AS RID, @SimulationID AS SimulationID) AS source
                        ON target.RID = source.RID AND target.SimulationID = source.SimulationID
                        WHEN MATCHED THEN
                            UPDATE SET LastModify = @LastModify
                        WHEN NOT MATCHED THEN
                            INSERT (RID, SimulationID, LastModify)
                            VALUES (@RID, @SimulationID, @LastModify);";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RID", Session["RID"]);
                        cmd.Parameters.AddWithValue("@SimulationID", simulationId);
                        cmd.Parameters.AddWithValue("@LastModify", DateTime.Now);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error logging simulation access: {ex.Message}");
                }
            }
        }
    }
}