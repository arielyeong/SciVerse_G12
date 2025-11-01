using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.LearningMaterials
{
    public partial class EditMaterial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Account/Login.aspx");
            }

            if (!IsPostBack)
            {
                // Get the MaterialID from query string
                if (Request.QueryString["ID"] != null)
                {
                    int materialId;
                    if (int.TryParse(Request.QueryString["ID"], out materialId))
                    {
                        LoadMaterialData(materialId);
                    }
                    else
                    {
                        Response.Redirect("ManageMaterials.aspx");
                    }
                }
                else
                {
                    Response.Redirect("ManageMaterials.aspx");
                }
            }
        }

        /// <summary>
        /// Loads the existing material data from database and populates the form
        /// </summary>
        private void LoadMaterialData(int materialId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Title, Description, Chapter, Type, FilePath FROM tblLearningMaterial WHERE MaterialID = @MaterialID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaterialID", materialId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate form fields
                            txtTitle.Text = reader["Title"].ToString();
                            txtDescription.Text = reader["Description"].ToString();

                            // Extract chapter number from "Chapter 6" format
                            string chapterStr = reader["Chapter"].ToString();
                            string chapterNum = chapterStr.Replace("Chapter", "").Trim();
                            txtChapter.Text = chapterNum;

                            ddlType.SelectedValue = reader["Type"].ToString();

                            // Store and display current file info
                            string filePath = reader["FilePath"].ToString();
                            hdnCurrentFilePath.Value = filePath;

                            // Extract filename from path
                            string fileName = Path.GetFileName(filePath);
                            lblCurrentFileName.Text = fileName;

                            // Set the view link
                            lnkViewFile.NavigateUrl = ResolveUrl(filePath);
                        }
                        else
                        {
                            // Material not found
                            ShowStatusMessage("Material not found.", "danger");
                            Response.Redirect("ManageMaterials.aspx");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays status messages to the user
        /// </summary>
        private void ShowStatusMessage(string message, string alertType)
        {
            string cleanMessage = message.Replace("\"", "'").Replace("\r\n", " ").Replace("\n", " ");
            string script = $@"
                var msgDiv = document.getElementById('statusMessageContainer');
                if (msgDiv) {{
                    msgDiv.innerHTML = ""<div class='alert alert-{alertType} alert-dismissible fade show' role='alert'>{cleanMessage}<button type='button' class='btn-close' data-bs-dismiss='alert'></button></div>"";
                }}
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "StatusMessage", script, true);
        }

        /// <summary>
        /// Handles the Save button click - updates the material in database
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Validate all fields
            Page.Validate("EditMaterial");
            if (!Page.IsValid)
            {
                ShowStatusMessage("Please fix the errors shown below.", "danger");
                return;
            }

            try
            {
                // Get MaterialID from query string
                if (Request.QueryString["ID"] == null)
                {
                    ShowStatusMessage("Invalid material ID.", "danger");
                    return;
                }

                int materialId = int.Parse(Request.QueryString["ID"]);

                // Get form values
                string title = txtTitle.Text.Trim();
                string description = txtDescription.Text.Trim();
                string chapterNum = txtChapter.Text.Trim();
                string chapter = "Chapter " + chapterNum;
                string materialType = ddlType.SelectedValue;

                // Use existing file path by default
                string dbFilePath = hdnCurrentFilePath.Value;
                string oldFilePath = hdnCurrentFilePath.Value;

                // Check if user uploaded a new file to replace the old one
                if (fileUpload.HasFile)
                {
                    // Get file extension and create new filename
                    string fileExtension = Path.GetExtension(fileUpload.PostedFile.FileName).ToLower();
                    string newFileName = $"Chapter{chapterNum}{fileExtension}";
                    string saveDir = Server.MapPath("/LearningMaterials/materials/");
                    string savePath = Path.Combine(saveDir, newFileName);

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(saveDir))
                    {
                        Directory.CreateDirectory(saveDir);
                    }

                    // Delete old file from server (only if it's different from new file)
                    string oldFileFullPath = Server.MapPath(oldFilePath);
                    if (File.Exists(oldFileFullPath) && oldFileFullPath != savePath)
                    {
                        try
                        {
                            File.Delete(oldFileFullPath);
                        }
                        catch (Exception ex)
                        {
                            // Log but don't fail - old file might be in use
                            System.Diagnostics.Debug.WriteLine($"Could not delete old file: {ex.Message}");
                        }
                    }

                    // Save new file to server
                    fileUpload.SaveAs(savePath);

                    // Update the database file path
                    dbFilePath = "/LearningMaterials/materials/" + newFileName;
                }

                // Update the material in database
                bool updateSuccess = UpdateMaterialInDatabase(materialId, title, description, chapter, materialType, dbFilePath);

                if (updateSuccess)
                {
                    // Redirect immediately to ManageMaterials page
                    Response.Redirect("ManageMaterials.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    ShowStatusMessage("Failed to update material. Please try again.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"An error occurred while updating: {ex.Message}", "danger");
            }
        }

        /// <summary>
        /// Updates the material record in the database
        /// </summary>
        private bool UpdateMaterialInDatabase(int materialId, string title, string description, string chapter, string type, string filePath)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"UPDATE tblLearningMaterial 
                                   SET Title = @Title, 
                                       Description = @Description, 
                                       Chapter = @Chapter, 
                                       Type = @Type, 
                                       FilePath = @FilePath 
                                   WHERE MaterialID = @MaterialID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Chapter", chapter);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);
                        cmd.Parameters.AddWithValue("@MaterialID", materialId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Database update error: {ex.Message}");
                return false;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageMaterials.aspx");
        }
    }
}