using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace SciVerse_G12.Quiz_Admin
{
    public partial class CreateNewQuizPage : System.Web.UI.Page
    {
        private const string UploadSubFolder = "~/Images/QuizList/";
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure uploads work even if the form is defined in the master page
            if (Page.Form != null) Page.Form.Enctype = "multipart/form-data";

            // Require login
            if (Session["RID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                PrefillNextChapter();
            }
        }

        private void PrefillNextChapter()
        {
            // Works whether Chapter is INT or NVARCHAR (will ignore non-numeric)
            const string sql = @"SELECT ISNULL(MAX(TRY_CONVERT(int, Chapter)), 0) FROM dbo.tblQuiz";
            try
            {
                int next;
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    object value = cmd.ExecuteScalar();
                    int max = (value == null || value == DBNull.Value) ? 0 : Convert.ToInt32(value);
                    next = max + 1;
                }
                txtChapter.Text = next.ToString();
                txtChapter.ReadOnly = true; // make it not editable
            }
            catch (Exception ex)
            {
                ShowMessage("Could not prefill Chapter: " + ex.Message, isError: true);
            }
        }


        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            // 1) Gather inputs
            string title = (txtQuizTitle.Text ?? "").Trim();
            string description = (txtDescription.Text ?? "").Trim();
            string chapter = (txtChapter.Text ?? "").Trim();

            int timeLimit = int.TryParse((txtTimeLimit.Text ?? "").Trim(), out var t) ? t : 0;
            int attemptLimit = int.TryParse((txtAttemptLimit.Text ?? "").Trim(), out var a) ? a : 0;

            // validation for empty
            if (string.IsNullOrWhiteSpace(title))
            {
                ShowMessage("Please enter a title.", isError: true);
                return;
            }

            // 2) Handle image upload (save to ~/Images/QuizList, default if none)
            string imageRelUrl = UploadSubFolder + "default.png";  // ensure this file exists

            if (FileUploadPicture.HasFile)
            {
                // Validate size (<5 MB) and extension
                var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                string originalName = Path.GetFileName(FileUploadPicture.FileName);
                string ext = Path.GetExtension(originalName)?.ToLowerInvariant() ?? "";

                if (FileUploadPicture.PostedFile.ContentLength > 5 * 1024 * 1024)
                {
                    ShowMessage("Image must be smaller than 5 MB.", isError: true);
                    return;
                }
                if (!allowed.Contains(ext))
                {
                    ShowMessage("Only JPG, PNG, GIF or WEBP images are allowed.", isError: true);
                    return;
                }

                try
                {
                    string folderPath = Server.MapPath(UploadSubFolder);
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    // Sanitize base name: keep letters, digits, dash, underscore
                    string baseName = Path.GetFileNameWithoutExtension(originalName);
                    var sb = new System.Text.StringBuilder();
                    foreach (char c in baseName)
                        if (char.IsLetterOrDigit(c) || c == '-' || c == '_') sb.Append(c);
                    string safeBase = sb.Length == 0 ? "image" : sb.ToString();
                    if (safeBase.Length > 80) safeBase = safeBase.Substring(0, 80);

                    // Ensure unique name (dog.png → dog(1).png)
                    string finalFileName = safeBase + ext;
                    string fullPath = Path.Combine(folderPath, finalFileName);
                    int i = 1;
                    while (File.Exists(fullPath))
                    {
                        finalFileName = $"{safeBase}({i}){ext}";
                        fullPath = Path.Combine(folderPath, finalFileName);
                        i++;
                    }

                    FileUploadPicture.SaveAs(fullPath);
                    imageRelUrl = UploadSubFolder + finalFileName; // ~/Images/QuizList/dog.png
                }
                catch (Exception ex)
                {
                    ShowMessage("Image upload failed: " + ex.Message, isError: true);
                    return;
                }
            }

            // 3) Get RID (int) and insert
            int creatorRid;
            if (Session["RID"] is int ridInt) creatorRid = ridInt;
            else if (!int.TryParse(Convert.ToString(Session["RID"]), out creatorRid))
            {
                Response.Redirect("~/Account/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            int newQuizId;
            const string insertSql = @"
                INSERT INTO dbo.tblQuiz
                    (Title, Description, Chapter, TimeLimit, ImageURL, CreatedDate, CreatedBy, AttemptLimit)
                OUTPUT INSERTED.QuizID
                VALUES
                    (@Title, @Description, @Chapter, @TimeLimit, @ImageURL, GETDATE(), @CreatedBy, @AttemptLimit);";

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(insertSql, conn))
                {
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = title;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value =
                        string.IsNullOrEmpty(description) ? (object)DBNull.Value : description;
                    cmd.Parameters.Add("@Chapter", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrEmpty(chapter) ? (object)DBNull.Value : chapter;
                    cmd.Parameters.Add("@TimeLimit", SqlDbType.Int).Value = timeLimit;
                    // match NVARCHAR(255)
                    cmd.Parameters.Add("@ImageURL", SqlDbType.NVarChar, 255).Value =
                        string.IsNullOrEmpty(imageRelUrl) ? (object)DBNull.Value : imageRelUrl;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = creatorRid; // RID int
                    cmd.Parameters.Add("@AttemptLimit", SqlDbType.Int).Value = attemptLimit;

                    conn.Open();
                    newQuizId = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving quiz: " + ex.Message, isError: true);
                return;
            }

            // 4) Redirect
            Response.Redirect(ResolveUrl("~/Quiz_Admin/EditQuizPage.aspx?quizId=" + newQuizId), false);
            Context.ApplicationInstance.CompleteRequest();
        }
        private void ShowMessage(string text, bool isError = false)
        {
            lblMessage.CssClass = isError ? "text-danger" : "text-success";
            lblMessage.Text = text;
        }
    }
}