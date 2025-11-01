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
        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Account/Login.aspx");
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
                // 1. Get form data
                string title = txtTitle.Text;
                string description = txtDescription.Text;
                string chapterNum = txtChapter.Text.Trim(); // e.g., "6"
                string chapter = "Chapter " + chapterNum;    // e.g., "Chapter 6"
                string materialType = ddlType.SelectedValue;

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
                string fileExtension = Path.GetExtension(fileUpload.PostedFile.FileName).ToLower();
                string newFileName = $"Chapter{chapterNum}{fileExtension}";
                string saveDir = Server.MapPath("/LearningMaterials/materials/");
                string savePath = Path.Combine(saveDir, newFileName);

                // Create directory if it doesn't exist
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                // 4. Save the file to the server
                fileUpload.SaveAs(savePath);

                // 5. Save the record to the database
                string dbFilePath = "/LearningMaterials/materials/" + newFileName;
                string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"INSERT INTO tblLearningMaterial 
                                     (title, description, chapter, type, filePath, uploadDate, uploadedBy) 
                                     VALUES 
                                     (@title, @description, @chapter, @type, @filePath, @uploadDate, @uploadedBy)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@chapter", chapter);
                        cmd.Parameters.AddWithValue("@type", materialType);
                        cmd.Parameters.AddWithValue("@filePath", dbFilePath);
                        cmd.Parameters.AddWithValue("@uploadDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@uploadedBy", uploadedById);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                ShowStatusMessage("Learning material saved successfully! Redirecting...", "success");
                string redirectScript = "setTimeout(function() { window.location.href = 'ManageMaterials.aspx'; }, 2000);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveSuccessRedirect", redirectScript, true);

            }
            catch (Exception ex)
            {
                // Show any save errors using the new helper
                ShowStatusMessage($"An error occurred while saving: {ex.Message}", "danger");
            }
        }

        protected void valChapterExists_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = true; 
                return;
            }

            try
            {
                // Format the chapter string exactly as it will be stored in the database
                // (Your btnSave_Click logic uses "Chapter " + number)
                string chapterForDB = "Chapter " + args.Value.Trim();

                string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Check if any record with this chapter name already exists
                    string query = "SELECT COUNT(*) FROM tblLearningMaterial WHERE chapter = @chapter";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@chapter", chapterForDB);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();

                        // If count is 0, the chapter does NOT exist (which is valid)
                        // If count is > 0, the chapter DOES exist (which is invalid)
                        args.IsValid = (count == 0);
                    }
                }
            }
            catch (Exception)
            {
                // If the database check fails, play it safe and declare it invalid
                // to prevent accidental duplicates.
                args.IsValid = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageMaterials.aspx");
        }
    }
}