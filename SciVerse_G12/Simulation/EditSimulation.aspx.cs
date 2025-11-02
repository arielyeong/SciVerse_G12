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
    public partial class EditSimulation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string simulationId = Request.QueryString["id"];
                
                if (string.IsNullOrEmpty(simulationId))
                {
                    Response.Redirect("~/Simulation/AdminSimulation.aspx");
                    return;
                }

                int id = GetSimulationID();
                if (id > 0)
                {
                    LoadSimulationDetails(id);
                }
                else
                {
                    Response.Redirect("~/Simulation/AdminSimulation.aspx");
                }
            }
        }

        private int GetSimulationID()
        {
            if (ViewState["SimulationID"] != null)
            {
                return (int)ViewState["SimulationID"];
            }
            
            string simulationId = Request.QueryString["id"];
            if (int.TryParse(simulationId, out int id))
            {
                ViewState["SimulationID"] = id;
                hfSimulationID.Value = id.ToString();
                return id;
            }
            return 0;
        }

        private void LoadSimulationDetails(int simulationId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        SimulationID,
                        Title,
                        Description,
                        Chapter,
                        Instruction,
                        URL
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
                            txtTitle.Text = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : "";
                            txtChapter.Text = reader["Chapter"] != DBNull.Value ? reader["Chapter"].ToString() : "";
                            txtDescription.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                            
                            // Clean up instruction text by removing extra whitespace and indentation
                            string instruction = reader["Instruction"] != DBNull.Value ? reader["Instruction"].ToString() : "";
                            txtInstruction.Text = CleanInstructionText(instruction);
                            
                            // Load the simulation URL for preview
                            string simulationUrl = reader["URL"] != DBNull.Value ? reader["URL"].ToString() : "";
                            previewIframe.Src = string.IsNullOrEmpty(simulationUrl) ? "" : simulationUrl;
                        }
                        else
                        {
                            Response.Redirect("~/Simulation/AdminSimulation.aspx");
                        }
                        
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading simulation details: {ex.Message}");
                    }
                }
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            int simulationId = GetSimulationID();
            if (simulationId == 0)
            {
                lblSaveMessage.Text = "Error: Invalid simulation ID";
                lblSaveMessage.ForeColor = System.Drawing.Color.Red;
                lblSaveMessage.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    UPDATE tblExperimentSimulation 
                    SET Title = @Title,
                        Chapter = @Chapter,
                        Description = @Description,
                        Instruction = @Instruction
                    WHERE SimulationID = @SimulationID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Chapter", txtChapter.Text.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Instruction", txtInstruction.Text.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SimulationID", simulationId);
                    
                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            lblSaveMessage.Text = "All changes saved successfully!";
                            lblSaveMessage.ForeColor = System.Drawing.Color.Green;
                            lblSaveMessage.Visible = true;
                        }
                        else
                        {
                            lblSaveMessage.Text = "No changes saved.";
                            lblSaveMessage.ForeColor = System.Drawing.Color.Orange;
                            lblSaveMessage.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblSaveMessage.Text = $"Error: {ex.Message}";
                        lblSaveMessage.ForeColor = System.Drawing.Color.Red;
                        lblSaveMessage.Visible = true;
                        System.Diagnostics.Debug.WriteLine($"Error saving simulation: {ex.Message}");
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Simulation/AdminSimulation.aspx");
        }

        private string CleanInstructionText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            // Split into lines
            string[] lines = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            
            // Process each line
            List<string> cleanedLines = new List<string>();
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    cleanedLines.Add(trimmedLine);
                }
            }
            
            // Join back with proper line breaks
            return string.Join("\r\n", cleanedLines);
        }

    }
}