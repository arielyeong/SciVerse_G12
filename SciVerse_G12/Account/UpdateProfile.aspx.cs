using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions; // For email regex

namespace SciVerse_G12
{
    public partial class UpdateProfile : System.Web.UI.Page
    {
        private string originalEmail; // To track changes for duplicate check

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    LoadUserDetails(Session["Username"].ToString());
                }
                else
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // *** Validation: Check Empty/Required Fields ***
            if (string.IsNullOrWhiteSpace(txtFullname.Text))
            {
                lblMessage.Text = "Full name is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblMessage.Text = "Email is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Email Format Validation
            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblMessage.Text = "Please enter a valid email address.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Check for duplicate email if changed
            if (txtEmail.Text != originalEmail && IsDuplicate("emailAddress", txtEmail.Text))
            {
                lblMessage.Text = "Email already registered. Please use another.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAge.Text))
            {
                lblMessage.Text = "Age is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Age Validation
            if (!int.TryParse(txtAge.Text, out int age) || age < 1 || age > 90)
            {
                lblMessage.Text = "Please enter a valid age (1-90).";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            if (string.IsNullOrWhiteSpace(rbGender.SelectedValue))
            {
                lblMessage.Text = "Gender is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            if (string.IsNullOrWhiteSpace(dlCountry.SelectedValue))
            {
                lblMessage.Text = "Country is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Default image path (no change if not uploaded)
            string imagePath = null;

            // Handle file upload (Optional, but validate if uploaded)
            if (FileUploadPic.HasFile)
            {
                // Basic validation: size < 5MB, image type
                if (FileUploadPic.PostedFile.ContentLength > 5 * 1024 * 1024 ||
                    !FileUploadPic.PostedFile.ContentType.StartsWith("image/"))
                {
                    lblMessage.Text = "Image must be <5MB and a valid image type (JPG, PNG, etc.).";
                    lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                    return;
                }

                try
                {
                    string folderPath = Server.MapPath("~/Images/Profile/"); // Standardized to Profile folder
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(FileUploadPic.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName); // Unique to avoid overwrite
                    string fullPath = Path.Combine(folderPath, uniqueFileName);
                    FileUploadPic.SaveAs(fullPath);

                    // Save relative path for database
                    imagePath = "~/Images/Profile/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Image upload failed: " + ex.Message;
                    lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                    return;
                }
            }

            // Update user info
            using (SqlConnection con = new SqlConnection(cs))
            {
                // Include picture only if a new one was uploaded
                string query;
                SqlCommand cmd;
                if (imagePath != null)
                {
                    query = "UPDATE tblRegisteredUsers SET fullName = @fullName, emailAddress = @email, age = @age, gender = @gender, country = @country, picture = @picture WHERE username = @username";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@fullName", txtFullname.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@age", age); // Use parsed int
                    cmd.Parameters.AddWithValue("@gender", rbGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@country", dlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@picture", imagePath);
                }
                else
                {
                    query = "UPDATE tblRegisteredUsers SET fullName = @fullName, emailAddress = @email, age = @age, gender = @gender, country = @country WHERE username = @username";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@fullName", txtFullname.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@age", age); // Use parsed int
                    cmd.Parameters.AddWithValue("@gender", rbGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@country", dlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                }

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    lblMessage.Text = "Profile updated successfully!";
                    lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-success";

                    // Refresh image preview if new image
                    if (imagePath != null)
                        imgPreview.ImageUrl = imagePath;

                    // Redirect after delay
                    string script = "setTimeout(function() { window.location.href = '" + ResolveUrl("~/Account/Profile.aspx") + "'; }, 2000);";
                    ScriptManager.RegisterStartupScript(this, GetType(), "redirectAfterUpdate", script, true);
                }
                else
                {
                    lblMessage.Text = "No changes were made or update failed.";
                    lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Profile.aspx");
        }

        private void LoadUserDetails(string username)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT fullName, emailAddress, username, age, gender, country, picture FROM tblRegisteredUsers WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtFullname.Text = dr["fullName"].ToString();
                    originalEmail = dr["emailAddress"].ToString(); // Capture original for duplicate check
                    txtEmail.Text = originalEmail;
                    txtUsername.Text = dr["username"].ToString();
                    txtAge.Text = dr["age"].ToString();

                    string gender = dr["gender"].ToString().Trim();
                    if (rbGender.Items.FindByValue(gender) != null)
                    {
                        rbGender.SelectedValue = gender;
                    }

                    dlCountry.SelectedValue = dr["country"].ToString();

                    // Show the current picture
                    if (dr["picture"] != DBNull.Value && !string.IsNullOrEmpty(dr["picture"].ToString()))
                        imgPreview.ImageUrl = dr["picture"].ToString();
                    else
                        imgPreview.ImageUrl = "~/Images/Profile/default.png"; // Standardized path
                }
                con.Close();
            }
        }

        /// <summary>
        /// Checks if a value exists for a given column (emailAddress) in the DB.
        /// </summary>
        private bool IsDuplicate(string columnName, string value)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    string query = $"SELECT COUNT(*) FROM tblRegisteredUsers WHERE [{columnName}] = @value AND username != @currentUsername";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@currentUsername", txtUsername.Text); // Exclude current user
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                // If query fails, assume no duplicate (fallback to insert + exception handling)
                return false;
            }
        }
    }
}