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
    public partial class SiteMaster : MasterPage
    {
        public string ProfileImageUrl { get; set; } = "~/Images/Profile/default.png";
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user should be logged out (session timeout or manual logout)
            if (!IsPostBack)
            {
                // Ensure session is properly maintained
                if (Session["Username"] != null)
                {
                    LoadProfileImage(Session["Username"].ToString());
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session variables
            Session.Clear();
            Session.Abandon();

            // Clear authentication cookie if using forms authentication
            if (Response.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            // Redirect to login page
            Response.Redirect("~/Account/Login.aspx");
        }

        private void LoadProfileImage(string username)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT picture FROM tblRegisteredUsers WHERE username = @username";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    con.Close();

                    if (result != null && !string.IsNullOrEmpty(result.ToString()))
                    {
                        ProfileImageUrl = result.ToString();
                    }
                    else
                    {
                        ProfileImageUrl = "~/Images/Profile/default.png";
                    }
                }
            }
            catch
            {
                // Fallback on error
                ProfileImageUrl = "~/Images/Profile/default.png";
            }
        }
    }
}