using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions; // For email regex
using System.Xml.Linq;
using System.Data.SqlClient; // For DB queries
using System.Data; // For SqlDataReader, etc.

namespace SciVerse_G12
{
    public partial class Register : System.Web.UI.Page
    {
        // Connection string from web.config
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Populate dlCountry if needed (example; adjust as per your data source)
            if (!IsPostBack)
            {
                // Example: dlCountry.Items.Add(new ListItem("USA", "USA")); etc.
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // *** Check Empty/Required Fields ***
            if (string.IsNullOrWhiteSpace(txtFullname.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConPass.Text) ||
                string.IsNullOrWhiteSpace(rblGender.SelectedValue) ||
                string.IsNullOrWhiteSpace(dlCountry.SelectedValue))
            {
                lblMessage.Text = "Please fill in all required fields.";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** NEW: Require Image Upload ***
            if (!FileUploadPic.HasFile)
            {
                lblMessage.Text = "Please upload a profile image (required).";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** Email Format Validation ***
            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblMessage.Text = "Please enter a valid email address.";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** Age Validation ***
            if (!int.TryParse(txtAge.Text, out int age) || age < 1 || age > 120)
            {
                lblMessage.Text = "Please enter a valid age (1-120).";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** Password Validation ***
            if (txtPassword.Text.Length < 4)
            {
                lblMessage.Text = "Password must be at least 4 characters.";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }
            if (txtPassword.Text != txtConPass.Text)
            {
                lblMessage.Text = "Passwords do not match!";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** Duplicate Username Check ***
            if (IsDuplicate("username", txtUsername.Text))
            {
                lblMessage.Text = "Username already exists. Please choose another.";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** Duplicate Email Check ***
            if (IsDuplicate("emailAddress", txtEmail.Text))
            {
                lblMessage.Text = "Email already registered. Please use another or login.";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** Handle Image Upload (Now Required - Validation Here) ***
            string imagePath = null;
            // Basic validation: size < 5MB, image type
            if (FileUploadPic.PostedFile.ContentLength > 5 * 1024 * 1024 ||
                !FileUploadPic.PostedFile.ContentType.StartsWith("image/"))
            {
                lblMessage.Text = "Image must be <5MB and a valid image type (JPG, PNG, etc.).";
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            try
            {
                string folderPath = Server.MapPath("~/Images/Profile/");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath); // Ensure folder exists

                string fileName = Path.GetFileName(FileUploadPic.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                string fullPath = Path.Combine(folderPath, uniqueFileName);

                FileUploadPic.SaveAs(fullPath);
                imagePath = "~/Images/Profile/" + uniqueFileName;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Image upload failed: " + ex.Message;
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
                return;
            }

            // *** Pass values into SqlDataSource parameters ***
            SqlDataSource1.InsertParameters["dateRegister"].DefaultValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SqlDataSource1.InsertParameters["picture"].DefaultValue = imagePath; // Now always set (since required)

            try
            {
                SqlDataSource1.Insert();
                lblMessage.Text = "✅ Registration Successful! Redirecting to login...";
                lblMessage.CssClass = "d-block text-center text-success mt-2";
                // Delay redirect slightly to show message
                ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                    "setTimeout(function(){ window.location.href = '" + ResolveUrl("~/Account/Login.aspx") + "'; }, 1500);", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Registration failed: " + ex.Message; // Fallback for other errors
                lblMessage.CssClass = "d-block text-center text-danger mt-2";
            }
        }

        /// <summary>
        /// Checks if a value exists for a given column (username or emailAddress) in the DB.
        /// </summary>
        private bool IsDuplicate(string columnName, string value)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"SELECT COUNT(*) FROM tblRegisteredUsers WHERE [{columnName}] = @value";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@value", value);
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