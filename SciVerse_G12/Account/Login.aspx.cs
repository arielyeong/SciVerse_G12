using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Include RID in the SELECT query
                string query = @"SELECT RID, username, role 
                         FROM tblRegisteredUsers 
                         WHERE (username = @username OR emailAddress = @username) 
                         AND password = @password";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string loggedInUser = reader["username"].ToString();
                    string role = reader["role"].ToString();
                    string rid = reader["RID"].ToString();   // ✅ get RID from the database

                    // Save all values into session
                    Session["Username"] = loggedInUser;
                    Session["Role"] = role;
                    Session["RID"] = rid;                    // ✅ store RID in session

                    // Redirect based on role
                    if (role == "Admin")
                    {
                        Response.Redirect("~/Admin/ViewUserList.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }

                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMessage.Text = "❌ Invalid username or password!";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

    }
}