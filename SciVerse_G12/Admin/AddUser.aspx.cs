using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace SciVerse_G12
{
    public partial class AddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Page load logic if needed
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(ddlGender.SelectedValue) ||
                string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                // Show error message
                return;
            }

            string username = txtUsername.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string age = txtAge.Text.Trim();
            string gender = ddlGender.SelectedValue;
            string country = txtCountry.Text.Trim();

            // Handle file upload
            string picture = string.Empty;
            if (fileUploadPicture.HasFile)
            {
                try
                {
                    // Validate file size and type
                    if (fileUploadPicture.PostedFile.ContentLength > 5 * 1024 * 1024) // 5MB
                    {
                        // Show error message
                        return;
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUploadPicture.FileName);
                    string folderPath = Server.MapPath("~/Images/Profile/");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fullPath = Path.Combine(folderPath, fileName);
                    fileUploadPicture.SaveAs(fullPath);
                    picture = "~/Images/Profile/" + fileName;
                }
                catch (Exception ex)
                {
                    // Handle upload error
                    return;
                }
            }

            // Save to database
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO tblRegisteredUsers (username, fullName, emailAddress, age, gender, country, picture, dateRegister) " +
                               "VALUES (@username, @fullName, @email, @age, @gender, @country, @picture, @dateRegister)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@picture", picture);
                    cmd.Parameters.AddWithValue("@dateRegister", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            // Redirect to the ViewUserList page after saving the user
            Response.Redirect("ViewUserList.aspx");
        }
    }
}