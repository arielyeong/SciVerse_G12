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
                lblMessage.Text = "❌ Please fix the errors before continuing.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string username = txtUsername.Text.Trim();
            string newPassword = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // ✅ Double-check passwords match (for extra safety)
            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "❌ Passwords do not match!";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE tblRegisteredUsers SET password = @password WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@password", newPassword);
                cmd.Parameters.AddWithValue("@username", username);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    lblMessage.Text = "✅ Password updated successfully! Redirecting to Login...";
                    lblMessage.ForeColor = System.Drawing.Color.Green;

                    // Redirect after 3 seconds
                    string script = "setTimeout(function(){ window.location='Login.aspx'; }, 3000);";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                }
                else
                {
                    lblMessage.Text = "❌ Username not found!";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
