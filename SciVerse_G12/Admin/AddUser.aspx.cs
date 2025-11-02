using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions; // For email regex

namespace SciVerse_G12
{
    public partial class AddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure lblMessage is styled properly on load (let CSS handle visibility via :empty)
            if (!IsPostBack)
            {
                lblMessage.CssClass = "lblMessage";
                lblMessage.Text = "";
                // Removed: lblMessage.Style.Add("display", "none"); // Let CSS :empty rule handle hiding
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Clear previous message (set empty text; CSS will hide via :empty)
            lblMessage.Text = "";
            lblMessage.CssClass = "lblMessage";

            // *** Check Empty/Required Fields ***
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(ddlGender.SelectedValue) ||
                string.IsNullOrWhiteSpace(dlCountry.SelectedValue) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(ddlRole.SelectedValue))
            {
                ShowMessage("Please fill in all required fields.", "error");
                return;
            }

            // *** NEW: Require Image Upload ***
            if (!fileUploadPicture.HasFile)
            {
                ShowMessage("Please upload a profile image (required).", "error");
                return;
            }

            string username = txtUsername.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string age = txtAge.Text.Trim();
            string gender = ddlGender.SelectedValue;
            string country = dlCountry.SelectedValue; // Fixed: Use SelectedValue, not Text
            string password = txtPassword.Text.Trim(); // New: Password
            string role = ddlRole.SelectedValue; // New: Role from dropdown

            // *** Password Validation ***
            if (password.Length < 4)
            {
                ShowMessage("Password must be at least 4 characters.", "error");
                return;
            }

            // *** Email Format Validation ***
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ShowMessage("Please enter a valid email address.", "error");
                return;
            }

            // *** Age Validation ***
            if (!int.TryParse(age, out int userAge) || userAge < 1 || userAge > 120)
            {
                ShowMessage("Please enter a valid age (1-120).", "error");
                return;
            }

            // *** Duplicate Username Check ***
            if (IsDuplicate("username", username))
            {
                ShowMessage("Username already exists. Please choose another.", "error");
                return;
            }

            // *** Duplicate Email Check ***
            if (IsDuplicate("emailAddress", email))
            {
                ShowMessage("Email already registered. Please use another.", "error");
                return;
            }

            // *** Handle Image Upload (Now Required - Validation Here) ***
            string picture = string.Empty;
            try
            {
                // Validate file size and type
                if (fileUploadPicture.PostedFile.ContentLength > 5 * 1024 * 1024) // 5MB
                {
                    ShowMessage("Image must be <5MB.", "error");
                    return;
                }
                if (!fileUploadPicture.PostedFile.ContentType.StartsWith("image/"))
                {
                    ShowMessage("Only image files (JPG, PNG, etc.) are allowed.", "error");
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
                ShowMessage("Image upload failed: " + ex.Message, "error");
                return;
            }

            // Save to database
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO tblRegisteredUsers (username, fullName, emailAddress, age, gender, country, picture, dateRegister, role, password) " +
                               "VALUES (@username, @fullName, @email, @age, @gender, @country, @picture, @dateRegister, @role, @password)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@age", userAge);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@picture", picture);
                    cmd.Parameters.AddWithValue("@dateRegister", DateTime.Now);
                    cmd.Parameters.AddWithValue("@role", role); // New: Dynamic role from dropdown
                    cmd.Parameters.AddWithValue("@password", password); // New: Plain text password (as requested; consider hashing in production)

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("✅ User added successfully! Redirecting...", "success");
                        // Delay redirect slightly to show message
                        ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                            "setTimeout(function(){ window.location.href = '" + ResolveUrl("ViewUserList.aspx") + "'; }, 1500);", true);
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Database error: " + ex.Message, "error");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a value exists for a given column (username or emailAddress) in the DB.
        /// </summary>
        private bool IsDuplicate(string columnName, string value)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
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
                // If query fails, assume no duplicate (fallback)
                return false;
            }
        }

        /// <summary>
        /// Helper to show messages via lblMessage.
        /// </summary>
        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            string colorClass = type == "success" ? "text-success" : "text-danger";
            lblMessage.CssClass = $"lblMessage {colorClass}"; // Rely on CSS for display: block !important; no need for d-block inline
            // Optional: Force show with inline style if CSS issues persist
            // lblMessage.Style["display"] = "block";
        }
    }
}