using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SciVerse_G12.LearningMaterials.materials
{
    public partial class ManageMaterials : System.Web.UI.Page
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
            // --- SECURITY CHECK ---
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            // --- END SECURITY CHECK ---

            if (!IsPostBack)
            {
                LoadLearningMaterials();
            }
        }

        private void LoadLearningMaterials()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            List<LearningMaterialItem> learningMaterials = new List<LearningMaterialItem>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT MaterialID, Title, Description, Chapter, Type, FilePath FROM tblLearningMaterial ORDER BY Chapter";
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
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
                }
                catch (Exception ex)
                {
                    // Handle error - could log it or show message
                    System.Diagnostics.Debug.WriteLine($"Error loading materials: {ex.Message}");
                }
            }

            Materials.DataSource = learningMaterials;
            Materials.DataBind();
        }

        // Handles only the "Delete" command now (Edit is handled by HyperLink)
        protected void Materials_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int materialID = Convert.ToInt32(e.CommandArgument);

                System.Diagnostics.Debug.WriteLine($"RowCommand triggered: {e.CommandName}, ID: {materialID}");

                if (e.CommandName == "DeleteRow")
                {
                    // Delete the single material
                    DeleteMaterialFromDatabase(materialID);
                    LoadLearningMaterials(); // Refresh the grid
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RowCommand: {ex.Message}");
            }
        }

        // Handles the "Delete Selected" button click
        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btnDeleteSelected_Click triggered");

            List<int> idsToDelete = new List<int>();

            // Check if we have saved indices in the hidden field
            string savedIndices = hdnSelectedIds.Value;

            if (!string.IsNullOrEmpty(savedIndices))
            {
                System.Diagnostics.Debug.WriteLine($"Using saved indices: {savedIndices}");

                // Parse the saved row indices
                string[] indices = savedIndices.Split(',');
                foreach (string indexStr in indices)
                {
                    int rowIndex;
                    if (int.TryParse(indexStr, out rowIndex) && rowIndex < Materials.Rows.Count)
                    {
                        int materialID = Convert.ToInt32(Materials.DataKeys[rowIndex].Value);
                        idsToDelete.Add(materialID);
                        System.Diagnostics.Debug.WriteLine($"Found saved item: Row {rowIndex}, ID {materialID}");
                    }
                }

                // Clear the hidden field
                hdnSelectedIds.Value = string.Empty;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No saved indices, checking checkboxes directly");

                // Fallback: Loop through all rows in the GridView
                foreach (GridViewRow row in Materials.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                    if (chkSelect != null && chkSelect.Checked)
                    {
                        int materialID = Convert.ToInt32(Materials.DataKeys[row.RowIndex].Value);
                        idsToDelete.Add(materialID);
                        System.Diagnostics.Debug.WriteLine($"Found checked item: {materialID}");
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"Total items to delete: {idsToDelete.Count}");

            if (idsToDelete.Count > 0)
            {
                // Delete all selected items
                DeleteMultipleMaterials(idsToDelete);
                LoadLearningMaterials(); // Refresh the grid
            }
        }

        // --- DATABASE HELPER METHODS ---

        private void DeleteMaterialFromDatabase(int materialID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    // Get the file path before deleting
                    string filePath = GetMaterialFilePath(materialID);

                    // Delete from database
                    string query = "DELETE FROM tblLearningMaterial WHERE MaterialID = @MaterialID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaterialID", materialID);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // Delete the physical file
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        DeletePhysicalFile(filePath);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deleting material: {ex.Message}");
                }
            }
        }

        private void DeleteMultipleMaterials(List<int> materialIDs)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                // Get all file paths before deleting
                List<string> filePaths = new List<string>();
                foreach (int id in materialIDs)
                {
                    string path = GetMaterialFilePath(id);
                    if (!string.IsNullOrEmpty(path))
                    {
                        filePaths.Add(path);
                    }
                }

                // Delete from database
                string idList = string.Join(",", materialIDs);
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = $"DELETE FROM tblLearningMaterial WHERE MaterialID IN ({idList})";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Delete all physical files
                foreach (string path in filePaths)
                {
                    DeletePhysicalFile(path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting multiple materials: {ex.Message}");
            }
        }

        private string GetMaterialFilePath(int materialID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string filePath = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FilePath FROM tblLearningMaterial WHERE MaterialID = @MaterialID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaterialID", materialID);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        filePath = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error getting file path: {ex.Message}");
                }
            }

            return filePath;
        }

        private void DeletePhysicalFile(string virtualPath)
        {
            try
            {
                string physicalPath = Server.MapPath(virtualPath);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting physical file: {ex.Message}");
            }
        }

        protected void Materials_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}