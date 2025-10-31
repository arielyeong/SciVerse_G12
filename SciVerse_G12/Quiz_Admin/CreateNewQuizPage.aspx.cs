using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace SciVerse_G12.Quiz
{
    public partial class CreateNewQuizPage : System.Web.UI.Page
    {
        private const string UploadSubFolder = "~/Images/QuizList/";
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure uploads work even if the form is defined in the master page
            if (Page.Form != null) Page.Form.Enctype = "multipart/form-data";

            // Require login
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                PrefillNextChapter();
            }
        }

        private void PrefillNextChapter()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            // Works whether Chapter is INT or NVARCHAR (will ignore non-numeric)
            const string sql = @"SELECT ISNULL(MAX(TRY_CONVERT(int, Chapter)), 0) FROM dbo.tblQuiz";

            int next = 1;

            using (var connStr = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connStr))
            {
                connStr.Open();
                object value = command.ExecuteScalar();
                int max = (value == null || value == DBNull.Value) ? 0 : Convert.ToInt32(value);
                next = max + 1;
            }

            txtChapter.Text = next.ToString();
            txtChapter.ReadOnly = true; //make it not editable
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
            if (string.IsNullOrEmpty(title))
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please enter a title.";
                return;
            }

            // 2) Handle image upload (save to ~/Images, default if none)
            string imageRelUrl = UploadSubFolder + "default.png";  // default image path (ensure the file exists)

            if (FileUploadPicture.HasFile)
            {
                try
                {
                    string folderPath = Server.MapPath(UploadSubFolder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(FileUploadPicture.FileName);
                    // Optional: ensure uniqueness
                    string uniqueName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
                    string fullPath = Path.Combine(folderPath, uniqueName);

                    FileUploadPicture.SaveAs(fullPath);

                    // Store virtual path for database
                    imageRelUrl = UploadSubFolder + uniqueName;
                }
                catch (Exception ex)
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "⚠️ Image upload failed: " + ex.Message;
                    return;
                }
            }

            // 3) Insert into DB
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int newQuizId;


            try
            {
                using (var connStr = new SqlConnection(connectionString))
                using (var command = new SqlCommand(@"
                    INSERT INTO dbo.tblQuiz
                        (Title, Description, Chapter, TimeLimit, ImageURL, CreatedDate, CreatedBy, AttemptLimit)
                    OUTPUT INSERTED.QuizID
                    VALUES
                        (@Title, @Description, @Chapter, @TimeLimit, @ImageURL, GETDATE(), @CreatedBy, @AttemptLimit);
                ", connStr))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Chapter", (object)chapter ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TimeLimit", timeLimit);
                    command.Parameters.AddWithValue("@ImageURL", (object)imageRelUrl ?? DBNull.Value);

                    // get current user id from session
                    int createdBy = 0;
                    if (Session["UserID"] is int uid) createdBy = uid;
                    else if (!int.TryParse(Convert.ToString(Session["UserID"]), out createdBy))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                        return;
                    }
                    command.Parameters.AddWithValue("@CreatedBy", createdBy);
                    command.Parameters.AddWithValue("@AttemptLimit", attemptLimit);

                    connStr.Open();
                    newQuizId = (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Error saving quiz: " + ex.Message;
                return;
            }

            // 4) Redirect to next step (e.g., add questions or back to list)
            Response.Redirect(ResolveUrl("~/Quiz_Admin/EditQuizPage.aspx?quizId=" + newQuizId), false);
            // IMPORTANT: stop the current request so no extra databinds run on this page
            Context.ApplicationInstance.CompleteRequest();
            //return;

        }
    }
}