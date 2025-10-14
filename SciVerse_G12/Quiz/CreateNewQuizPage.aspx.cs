using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace SciVerse_G12.Quiz
{
    public partial class CreateNewQuizPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure uploads work even if the form is defined in the master page
            if (Page.Form != null)
                Page.Form.Enctype = "multipart/form-data";

            if (!IsPostBack)
            {
                PrefillNextChapter();
            }
        }

        private void PrefillNextChapter()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            const string sql = @"SELECT ISNULL(MAX(Chapter), 0) FROM dbo.tblQuiz";

            int next = 1;

            using (var con = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                object val = command.ExecuteScalar();
                int max = (val == null || val == DBNull.Value) ? 0 : Convert.ToInt32(val);
                next = max + 1;
            }

            txtChapter.Text = next.ToString();
            txtChapter.ReadOnly = true; // optional: make it not editable
        }


        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            // 1) Gather inputs
            string title = (txtQuizTitle.Text ?? "").Trim();
            string description = (txtDescription.Text ?? "").Trim();
            string chapter = (txtChapter.Text ?? "").Trim();

            int timeLimit = int.TryParse((txtTimeLimit.Text ?? "").Trim(), out var t) ? t : 0;
            int attemptLimit = int.TryParse((txtAttemptLimit.Text ?? "").Trim(), out var a) ? a : 0;

            if (string.IsNullOrEmpty(title))
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please enter a title.";
                return;
            }

            // 2) Save image (optional) into /Content/img and store a WEB path
            string imageRelUrl = null;
            if (FileUploadPicture.HasFile)
            {
                string ext = Path.GetExtension(FileUploadPicture.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "Image must be .jpg, .jpeg or .png";
                    return;
                }

                string fileName = Guid.NewGuid().ToString("N") + ext;
                string folder = Server.MapPath("~/Content/img/");
                Directory.CreateDirectory(folder);               // safe if exists
                string fullPath = Path.Combine(folder, fileName);
                FileUploadPicture.SaveAs(fullPath);

                imageRelUrl = "/Content/img/" + fileName;        // store web path
            }

            // 3) Insert into DB
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int newQuizId;


            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var command = new SqlCommand(@"
                    INSERT INTO dbo.tblQuiz
                        (Title, Description, Chapter, TimeLimit, ImageURL, CreatedDate, CreatedBy, AttemptLimit)
                    OUTPUT INSERTED.QuizID
                    VALUES
                        (@Title, @Description, @Chapter, @TimeLimit, @ImageURL, GETDATE(), @CreatedBy, @AttemptLimit);
                ", con))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Chapter", (object)chapter ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TimeLimit", timeLimit);
                    command.Parameters.AddWithValue("@ImageURL", (object)imageRelUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedBy", 1);              // TODO: set this from your logged-in admin ID
                    command.Parameters.AddWithValue("@AttemptLimit", attemptLimit);

                    con.Open();
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
            //Response.Redirect("EditQuizPage.aspx?quizId=" + newQuizId, false);
            //Context.ApplicationInstance.CompleteRequest();
            //return;

            // after inserting and getting: int newQuizId = (int)cmd.ExecuteScalar();
            Response.Redirect(ResolveUrl("~/Quiz/EditQuizPage.aspx?quizId=" + newQuizId), false);
            // IMPORTANT: stop the current request so no extra databinds run on this page
            Context.ApplicationInstance.CompleteRequest();
            return;

        }
    }
}