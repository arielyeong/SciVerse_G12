using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SciVerse_G12.LearningMaterials
{
    public partial class LearningMaterialList : System.Web.UI.Page
    {
        public class LearningMaterialItem
        {
            public int MaterialID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Chapter { get; set; }
            public string Type { get; set; }
            public string FilePath { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLearningMaterials();
            }
        }

        private void LoadLearningMaterials()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            // Use a List<LearningMaterialItem> to store the fetched data
            List<LearningMaterialItem> learningMaterials = new List<LearningMaterialItem>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT
                        MaterialID,
                        Title,
                        Description,
                        Chapter,
                        Type,
                        FilePath
                    FROM
                        tblLearningMaterial
                    ORDER BY Chapter, Title"; // Order for presentation

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Read data row by row and populate the list
                    while (reader.Read())
                    {
                        learningMaterials.Add(new LearningMaterialItem
                        {
                            MaterialID = Convert.ToInt32(reader["MaterialID"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Chapter = reader["Chapter"].ToString(),
                            Type = reader["Type"].ToString(),
                            FilePath = reader["FilePath"].ToString()
                        });
                    }
                    reader.Close();

                    System.Diagnostics.Debug.WriteLine($"Loaded {learningMaterials.Count} learning materials.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading learning materials: {ex.Message}");
                    lblNoMaterials.Text = "Error loading learning materials. Please try again later.";
                    noMaterialsSection.Visible = true;
                    return; // Exit if an error occurs
                }
            }

            // Bind data to Repeater
            if (learningMaterials.Count > 0)
            {
                rptLearningMaterials.DataSource = learningMaterials;
                rptLearningMaterials.DataBind();
                noMaterialsSection.Visible = false; // Hide "No materials" message
            }
            else
            {
                noMaterialsSection.Visible = true; // Show "No materials" message
                lblNoMaterials.Text = "No learning materials available at the moment.";
            }
        }
    }
}