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
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int CurrentMaterialID
        {
            get { return ViewState["CurrentMaterialID"] != null ? (int)ViewState["CurrentMaterialID"] : 0; }
            set { ViewState["CurrentMaterialID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Account/Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    int materialId;
                    if (int.TryParse(Request.QueryString["ID"], out materialId))
                    {
                        CurrentMaterialID = materialId;
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
                            txtTitle.Text = reader["Title"].ToString();
                            txtDescription.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                            txtChapter.Text = reader["Chapter"].ToString();
                            ddlType.SelectedValue = reader["Type"].ToString();
                            ddlType.Enabled = false;

                            // Store and display current file info
                            string filePath = reader["FilePath"].ToString();
                            hdnCurrentFilePath.Value = filePath;
                            string fileName = Path.GetFileName(filePath);
                            lblCurrentFileName.Text = fileName;
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

        /// Displays status messages to the user
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

        /// Checks if a file with this exact path already exists in the database,
        /// ignoring the material ID we are currently editing.
        private bool DoesFilePathExist(string filePath, int materialIdToIgnore)
        {
            string query = @"SELECT COUNT(*) FROM tblLearningMaterial 
                     WHERE FilePath = @FilePath 
                     AND MaterialID != @materialIdToIgnore";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FilePath", filePath);
                cmd.Parameters.AddWithValue("@materialIdToIgnore", materialIdToIgnore);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return (count > 0);
            }
        }

        /// Handles the Save button click - updates the material in database
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
                int materialId = CurrentMaterialID;
                if (materialId == 0)
                {
                    ShowStatusMessage("Invalid material ID. Session may have expired.", "danger");
                    return;
                }
                string title = txtTitle.Text.Trim();
                string description = txtDescription.Text.Trim();
                int chapterNum;
                if (!int.TryParse(txtChapter.Text.Trim(), out chapterNum))
                {
                    ShowStatusMessage("Invalid chapter number.", "danger");
                    return;
                }
                string materialType = ddlType.SelectedValue;

                string dbFilePath = hdnCurrentFilePath.Value;
                string oldFilePath = hdnCurrentFilePath.Value;

                if (fileUpload.HasFile)
                {
                    string originalFileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                    string saveDir = Server.MapPath("/LearningMaterials/materials/");
                    string savePath = Path.Combine(saveDir, originalFileName);
                    dbFilePath = "/LearningMaterials/materials/" + originalFileName;
                    if (File.Exists(savePath) || DoesFilePathExist(dbFilePath, materialId))
                    {
                        ShowStatusMessage("A file with this name already exists. Please rename your file before uploading.", "danger");
                        return;
                    }
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
                }
                UpdateMaterialInDatabase(materialId, title, description, chapterNum, materialType, dbFilePath);
                Response.Redirect("ManageMaterials.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"An error occurred while updating: {ex.Message}", "danger");
            }
        }

        /// Updates the material record in the database
        private void UpdateMaterialInDatabase(int materialId, string title, string description, int chapter, string type, string filePath)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
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
                        if (string.IsNullOrEmpty(description))
                            cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Chapter", chapter);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);
                        cmd.Parameters.AddWithValue("@MaterialID", materialId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // If no rows were updated, the ID was not found
                        if (rowsAffected == 0)
                        {
                            throw new Exception("The material was not found in the database and could not be updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                System.Diagnostics.Debug.WriteLine($"Database update error: {ex.Message}");
                // Re-throw the exception so btnSave_Click can catch it and show the user
                throw;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageMaterials.aspx");
        }
    }
}