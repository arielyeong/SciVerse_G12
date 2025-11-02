using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.LearningMaterials
{
    public partial class CreateMaterial : System.Web.UI.Page
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["CopyFromID"] != null)
                {
                    int materialIdToCopy;
                    if (int.TryParse(Request.QueryString["CopyFromID"], out materialIdToCopy))
                    {
                        // This is a new method you need to add
                        LoadDataToCopy(materialIdToCopy);
                    }
                }
                else
                {
                    PrefillNextChapter();
                }
            }
        }

        /// Pre-fills the form with data from an existing material to be copied.
        private void LoadDataToCopy(int materialId)
        {
            const string sql = @"SELECT chapter, title, description 
                         FROM dbo.tblLearningMaterial 
                         WHERE materialID = @materialID";

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@materialID", materialId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Pre-fill all data from the item to copy
                            txtChapter.Text = reader["chapter"].ToString();
                            txtTitle.Text = reader["title"].ToString();
                            txtDescription.Text = reader["description"] != DBNull.Value ? reader["description"].ToString() : "";
                        }
                    }
                }
                // Make sure fields are editable
                txtChapter.ReadOnly = false;
                txtTitle.ReadOnly = false;
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Could not load material data to copy: " + ex.Message, "danger");
                txtChapter.ReadOnly = false;
            }
        }

        private void PrefillNextChapter()
        {
            const string sql = @"SELECT ISNULL(MAX(chapter), 0) + 1 FROM dbo.tblLearningMaterial";

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    object nextChapter = cmd.ExecuteScalar();
                    txtChapter.Text = nextChapter.ToString();
                }
                txtChapter.ReadOnly = false;
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Could not prefill Chapter: " + ex.Message, "danger");
                txtChapter.ReadOnly = false;
            }
        }

        private void ShowStatusMessage(string message, string alertType)
        {
            // Simple sanitization for JavaScript string
            string cleanMessage = message.Replace("\"", "'").Replace("\r\n", " ").Replace("\n", " ");

            string script = $@"
                var msgDiv = document.getElementById('statusMessageContainer');
                if (msgDiv) {{
                    msgDiv.innerHTML = ""<div class='alert alert-{alertType}'>{cleanMessage}</div>"";
                }}
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "StatusMessage", script, true);
        }

        /// Checks if a file with this exact path already exists in the database.
        private bool DoesFilePathExist(string filePath)
        {
            string query = @"SELECT COUNT(*) FROM tblLearningMaterial WHERE FilePath = @FilePath";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FilePath", filePath);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return (count > 0);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("CreateMaterial");
            if (!Page.IsValid)
            {
                ShowStatusMessage("Please fix the errors shown below.", "danger");
                return;
            }
            if (!fileUpload.HasFile)
            {
                ShowStatusMessage("All fields are valid, but you forgot to select a file.", "danger");
                return;
            }

            try
            {
                string title = txtTitle.Text;
                string description = txtDescription.Text;
                int chapterNum;
                if (!int.TryParse(txtChapter.Text.Trim(), out chapterNum))
                {
                    ShowStatusMessage("Invalid chapter number.", "danger");
                    return;
                }
                string materialType = ddlType.SelectedValue;

                string originalFileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                string saveDir = Server.MapPath("/LearningMaterials/materials/");
                string savePath = Path.Combine(saveDir, originalFileName);
                string dbFilePath = "/LearningMaterials/materials/" + originalFileName;

                // 2. Check if this file *name* already exists on the server or in the DB
                if (File.Exists(savePath) || DoesFilePathExist(dbFilePath))
                {
                    ShowStatusMessage("A file with this name already exists.", "danger");
                    return;
                }

                int uploadedById = -1;
                if (Session["RID"] != null)
                {
                    int.TryParse(Session["RID"].ToString(), out uploadedById);
                }

                if (uploadedById <= 0)
                {
                    ShowStatusMessage("Your session has expired or is invalid. Please log in again.", "danger");
                    return;
                }
                // Create directory if it doesn't exist
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                // 4. Save the file to the server
                fileUpload.SaveAs(savePath);
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string query = @"INSERT INTO tblLearningMaterial 
                             (title, description, chapter, type, filePath, uploadDate, uploadedBy) 
                             VALUES 
                             (@title, @description, @chapter, @type, @filePath, @uploadDate, @uploadedBy)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        if (string.IsNullOrEmpty(description))
                            cmd.Parameters.AddWithValue("@description", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@chapter", chapterNum);
                        cmd.Parameters.AddWithValue("@type", materialType);

                        // 4. Save the full path (with original name) to the DB
                        cmd.Parameters.AddWithValue("@filePath", dbFilePath);

                        cmd.Parameters.AddWithValue("@uploadDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@uploadedBy", uploadedById);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                ShowStatusMessage("Learning material saved successfully!", "success");
                string redirectScript = "setTimeout(function() { window.location.href = 'ManageMaterials.aspx'; }, 500);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveSuccessRedirect", redirectScript, true);
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"An error occurred while saving: {ex.Message}", "danger");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageMaterials.aspx");
        }
    }
}