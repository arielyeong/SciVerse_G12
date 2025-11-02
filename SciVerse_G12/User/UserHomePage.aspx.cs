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
    public partial class UserHomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is logged in
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                // Load user's full name
                LoadUserFullName(Session["Username"].ToString());
            }
        }

        private void LoadUserFullName(string username)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT fullName FROM tblRegisteredUsers WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                if (result != null && !string.IsNullOrEmpty(result.ToString()))
                {
                    lblFullName.Text = result.ToString();
                }
                else
                {
                    lblFullName.Text = username; // Fallback to username if fullName not found
                }
            }
        }
    }
}