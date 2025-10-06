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
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    LoadProfile(Session["Username"].ToString());
                }
                else
                {
                    // If not logged in, send back to login page
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
        }

        private void LoadProfile(string username)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM tblRegisteredUsers WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblFullname.Text = reader["fullName"].ToString();
                    lblEmail.Text = reader["emailAddress"].ToString();
                    lblUsername.Text = reader["username"].ToString();
                    lblAge.Text = reader["age"].ToString();
                    lblGender.Text = reader["gender"].ToString();
                    lblCountry.Text = reader["country"].ToString();
                    lblDate.Text = Convert.ToDateTime(reader["dateRegister"]).ToString("dd MMM yyyy");

                    if (!string.IsNullOrEmpty(reader["picture"].ToString()))
                        imgProfile.ImageUrl = reader["picture"].ToString();
                    else
                        imgProfile.ImageUrl = "~/Images/default-avatar.png"; // fallback
                }

                reader.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/UpdateProfile.aspx");
        }
    }
}