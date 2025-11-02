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
    public partial class Experiment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    LoadSimulationUrl(id);
                    LoadInstructions(id);
                }
                else
                {
                    Response.Redirect("~/Simulation/SimulationDashboard.aspx");
                }
            }
        }

        private void LoadSimulationUrl(int simulationId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string iframeUrl = "https://phet.colorado.edu/sims/html/under-pressure/latest/under-pressure_en.html"; // Default URL

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT SimulationID, Title, URL
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
                            // Get URL from database if it exists
                            iframeUrl = reader["URL"] != DBNull.Value ? reader["URL"].ToString() : iframeUrl;
                        }
                        else
                        {
                            Response.Redirect("~/Simulation/SimulationDashboard.aspx");
                            return;
                        }
                        
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading simulation URL: {ex.Message}");
                    }
                }
            }

            // Convert relative URLs to absolute server URLs
            if (!string.IsNullOrEmpty(iframeUrl) && !iframeUrl.StartsWith("http"))
            {
                // Remove leading slash if present in database value to avoid double slash
                string cleanUrl = iframeUrl.StartsWith("/") ? iframeUrl.Substring(1) : iframeUrl;
                iframeUrl = ResolveUrl("~/" + cleanUrl);
            }

            experimentIframe.Src = iframeUrl;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Simulation/StartSimulation.aspx");
        }

        protected void btnRestart_Click(object sender, EventArgs e)
        {
            // Reload the page with the same simulation ID
            string simulationId = Request.QueryString["id"];
            Response.Redirect($"~/Simulation/Experiment.aspx?id={simulationId}");
        }

        private void LoadInstructions(int simulationId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT Instruction
                    FROM tblExperimentSimulation
                    WHERE SimulationID = @SimulationID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SimulationID", simulationId);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string instructions = result.ToString();
                            DisplayInstructions(instructions);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading instructions: {ex.Message}");
                    }
                }
            }
        }

        private void DisplayInstructions(string instructions)
        {
            // Split instructions by lines and filter empty lines
            string[] lines = instructions.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            
            int stepNumber = 1;
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                // Remove step number if it exists (e.g., "1. " or "1.")
                if (trimmedLine.StartsWith(stepNumber + ". "))
                {
                    trimmedLine = trimmedLine.Substring((stepNumber + ". ").Length).Trim();
                }
                else if (trimmedLine.StartsWith(stepNumber + "."))
                {
                    trimmedLine = trimmedLine.Substring((stepNumber + ".").Length).Trim();
                }

                // Create list item for instruction
                LiteralControl lit = new LiteralControl(
                    $"<li class=\"instruction-step\">" +
                    $"<h4>Step {stepNumber}</h4>" +
                    $"<p>{System.Security.SecurityElement.Escape(trimmedLine)}</p>" +
                    $"</li>"
                );
                
                instructionsList.Controls.Add(lit);
                stepNumber++;
            }
        }
    }
}