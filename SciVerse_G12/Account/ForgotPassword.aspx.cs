using System;
using System.Data.SqlClient;
using System.Configuration;

namespace SciVerse_G12
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // ✅ Stop execution if page validators fail (RequiredField / CompareValidator)
            if (!Page.IsValid)
            {
                lblMessage.Text = "Please fix the errors before continuing.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string newPassword = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // ✅ Double-check passwords match (for extra safety)
            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match!";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // ✅ First, verify that the provided username and email exactly match a single account in the database
                string checkQuery = "SELECT COUNT(*) FROM tblRegisteredUsers WHERE username = @username AND emailAddress = @email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@username", username);
                checkCmd.Parameters.AddWithValue("@email", email);

                int matchCount = (int)checkCmd.ExecuteScalar();

                if (matchCount != 1)  // ✅ Ensure exactly one match (prevents duplicates if any)
                {
                    lblMessage.Text = "The provided username and email do not match any existing account!";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // ✅ If exact match found, proceed with password update using the same WHERE clause
                string updateQuery = "UPDATE tblRegisteredUsers SET password = @password WHERE username = @username AND emailAddress = @email";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@password", newPassword);
                updateCmd.Parameters.AddWithValue("@username", username);
                updateCmd.Parameters.AddWithValue("@email", email);

                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected == 1)  // ✅ Confirm exactly one row was updated
                {
                    lblMessage.Text = "Password updated successfully for your account! Redirecting to Login...";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.CssClass = "text-success d-block mb-3 text-center message";

                    // Redirect after 3 seconds
                    string script = "setTimeout(function(){ window.location='Login.aspx'; }, 3000);";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                }
                else
                {
                    lblMessage.Text = "Failed to update password. Please try again or contact support.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}