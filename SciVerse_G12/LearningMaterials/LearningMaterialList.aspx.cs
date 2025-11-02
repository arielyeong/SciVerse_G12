using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.LearningMaterials
{
    public partial class LearningMaterialList : System.Web.UI.Page
    {
        public class MaterialFile
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
        }
        public class GroupedLearningMaterial
        {
            public int Chapter { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public List<MaterialFile> Notes { get; set; }
            public List<MaterialFile> Videos { get; set; }
            public bool HasNote => Notes != null && Notes.Count > 0;
            public bool HasVideo => Videos != null && Videos.Count > 0;
        }

        private void LoadLearningMaterials()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var allFiles = new List<LearningMaterialItem>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT
                MaterialID, Title, Description, Chapter, Type, FilePath
            FROM
                tblLearningMaterial
            ORDER BY Chapter, Title";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allFiles.Add(new LearningMaterialItem
                        {
                            MaterialID = Convert.ToInt32(reader["MaterialID"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "", // Handle NULL
                            Chapter = Convert.ToInt32(reader["Chapter"]),
                            Type = reader["Type"].ToString(),
                            FilePath = reader["FilePath"].ToString()
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading learning materials: {ex.Message}");
                    lblNoMaterials.Text = "Error loading learning materials. Please try again later.";
                    noMaterialsSection.Visible = true;
                    return;
                }
            }

            var groupedData = allFiles.GroupBy(
                // Group by Chapter, Title, and Description
                file => new { file.Chapter, file.Title, file.Description },
                (key, files) => new GroupedLearningMaterial
                {
                    Chapter = key.Chapter,
                    Title = key.Title,
                    Description = key.Description,

                    // Select ALL matching notes into the list
                    Notes = files
                        .Where(f => f.Type == "PDF" || f.Type == "Word")
                        .Select(f => new MaterialFile
                        {
                            FileName = Path.GetFileName(f.FilePath), // "notes.pdf"
                            FilePath = f.FilePath                    // "/materials/notes.pdf"
                        })
                        .ToList(),

                    // Select ALL matching videos into the list
                    Videos = files
                        .Where(f => f.Type == "Video")
                        .Select(f => new MaterialFile
                        {
                            FileName = Path.GetFileName(f.FilePath),
                            FilePath = f.FilePath
                        })
                        .ToList()
                });
            var groupedList = groupedData.ToList();
            if (groupedList.Count > 0)
            {
                rptLearningMaterials.DataSource = groupedList;
                rptLearningMaterials.DataBind();
                noMaterialsSection.Visible = false;
            }
            else
            {
                noMaterialsSection.Visible = true;
                lblNoMaterials.Text = "No learning materials available at the moment.";
            }
        }

        public class LearningMaterialItem
        {
            public int MaterialID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Chapter { get; set; }
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

    }
}