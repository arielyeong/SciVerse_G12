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
    public partial class AdminSimulation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnsureTableAndDataExists();
                BindSimulationData();
            }
        }

        private void BindSimulationData()
        {
            string searchTerm = txtSearch.Text.Trim();
            DataTable dt = GetSimulationData(searchTerm);
            
            gvSimulationMonitor.DataSource = dt;
            gvSimulationMonitor.DataBind();
        }

        private DataTable FilterSimulationData(DataTable dt, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return dt;
            }

            DataTable filteredTable = dt.Clone();
            
            foreach (DataRow row in dt.Rows)
            {
                // Check if search term matches Simulation ID (as string or number)
                string simulationID = row["SimulationID"].ToString();
                bool matchesID = simulationID.Equals(searchTerm, StringComparison.OrdinalIgnoreCase);
                
                // Check if search term matches Title (partial match, case-insensitive)
                string title = row["Title"].ToString();
                bool matchesTitle = title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                
                // Also try to match ID as integer if search term is numeric
                if (!matchesID && int.TryParse(searchTerm, out int searchID))
                {
                    if (int.TryParse(simulationID, out int rowID) && rowID == searchID)
                    {
                        matchesID = true;
                    }
                }
                
                if (matchesID || matchesTitle)
                {
                    filteredTable.ImportRow(row);
                }
            }
            
            return filteredTable;
        }

        private DataTable GetSimulationData(string searchTerm = "")
        {
            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        s.SimulationID,
                        s.Title,
                        s.Chapter,
                        COALESCE(
                            (SELECT STRING_AGG(CAST(r.username AS NVARCHAR(MAX)), ', ')
                             FROM tblAccessSimulation a
                             INNER JOIN tblRegisteredUsers r ON a.RID = r.RID
                             WHERE a.SimulationID = s.SimulationID), 
                            'N/A'
                        ) AS Username
                    FROM tblExperimentSimulation s
                    WHERE (@searchTerm = '' OR 
                           CAST(s.SimulationID AS NVARCHAR(10)) LIKE '%' + @searchTerm + '%' OR
                           s.Title LIKE '%' + @searchTerm + '%' OR
                           s.Chapter LIKE '%' + @searchTerm + '%' OR
                           EXISTS (
                               SELECT 1 
                               FROM tblAccessSimulation a
                               INNER JOIN tblRegisteredUsers r ON a.RID = r.RID
                               WHERE a.SimulationID = s.SimulationID 
                               AND r.username LIKE '%' + @searchTerm + '%'
                           ))
                    ORDER BY s.SimulationID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", searchTerm ?? "");
                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error or handle exception
                        // For now, return empty DataTable
                        System.Diagnostics.Debug.WriteLine($"Error loading simulation data: {ex.Message}");
                    }
                }
            }

            return dt;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSimulationData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindSimulationData();
        }

        protected void btnDetails_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string simulationID = btn.CommandArgument;
            Response.Redirect($"~/Simulation/EditSimulation.aspx?id={simulationID}");
        }

        private void EnsureTableAndDataExists()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Check if table exists and create if it doesn't
                    string checkTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblExperimentSimulation]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE [dbo].[tblExperimentSimulation] (
                                [SimulationID]       INT            IDENTITY (1, 1) NOT NULL,
                                [Title]        NVARCHAR (200) NOT NULL,
                                [Description]  NVARCHAR (MAX) NULL,
                                [Chapter]      NVARCHAR (100) NULL,
                                [Instruction]  NVARCHAR (MAX) NULL,
                                [Username]     NVARCHAR (100) NULL,
                                [LastModify]   DATETIME       NULL,
                                [URL]          NVARCHAR (MAX) NULL,
                                [Image]        NVARCHAR (MAX) NULL,
                                PRIMARY KEY CLUSTERED ([SimulationID] ASC)
                            );
                            
                            -- Add foreign key constraint if tblRegisteredUsers exists and constraint doesn't exist
                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblRegisteredUsers]') AND type in (N'U'))
                            AND NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ExperimentSimulation_RegisteredUsers')
                            BEGIN
                                ALTER TABLE [dbo].[tblExperimentSimulation]
                                ADD CONSTRAINT FK_ExperimentSimulation_RegisteredUsers 
                                FOREIGN KEY ([Username]) REFERENCES [dbo].[tblRegisteredUsers] ([username]) ON DELETE SET NULL;
                            END
                        END
                        ELSE
                        BEGIN
                            -- Add URL column if it doesn't exist
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tblExperimentSimulation]') AND name = 'URL')
                            BEGIN
                                ALTER TABLE [dbo].[tblExperimentSimulation] ADD [URL] NVARCHAR (MAX) NULL;
                            END
                            -- Add Image column if it doesn't exist
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tblExperimentSimulation]') AND name = 'Image')
                            BEGIN
                                ALTER TABLE [dbo].[tblExperimentSimulation] ADD [Image] NVARCHAR (MAX) NULL;
                            END
                        END";

                    SqlCommand createTableCmd = new SqlCommand(checkTableQuery, conn);
                    createTableCmd.ExecuteNonQuery();

                    // Check if data already exists
                    string checkDataQuery = "SELECT COUNT(*) FROM tblExperimentSimulation";
                    SqlCommand checkDataCmd = new SqlCommand(checkDataQuery, conn);
                    int count = (int)checkDataCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        // Execute INSERT statements
                        string insertQuery = @"
                            INSERT INTO [dbo].[tblExperimentSimulation](Title, Description, Chapter, Instruction, URL, Image)
                            VALUES
                            ('Pressure and density', 
                            'Explore how pressure changes with depth and how fluid density affects the pressure acting on submerged objects.','Chapter 6',
                            '1. Adjust the depth slider to observe how pressure increases with depth. 
                             2. Change the liquid type to compare the effect of density on pressure. 
                             3. Record the pressure readings for each liquid at different depths. 
                             4. Analyze the data to conclude how pressure relates to depth and density.',
                            'https://phet.colorado.edu/sims/html/under-pressure/latest/under-pressure_en.html',
                            'https://phet.colorado.edu/sims/html/under-pressure/latest/under-pressure-600.png'),

                            ('Reaction of acid and reactive metal','Observe how reactive metals like magnesium react with acids to produce hydrogen gas and form salts.','Chapter 2', 
                            '1. Drag the Hydrochloric acid bottle to the test tube. 
                             2. Drag the Magnesium ribbon to the test tube. 
                             3. Click the Bunsen burner to light it. 
                             4. Drag the wooden splint into the lit Bunsen flame to ignite it. 
                             5. Drag the lit splint to the test tube to show the pop image.
                             6. Drag the reacted test tube to the dish above the Bunsen.
                             7. Click the lit Bunsen to heat the dish mixture to see the product.',
                            'Images/Simulation/reaction.html',
                            'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT8nAOSyvPZMwgdn_pVbqDbIFhp4MuhRPllmQ&s')";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error ensuring table and data exists: {ex.Message}");
                }
            }
        }

    }
}