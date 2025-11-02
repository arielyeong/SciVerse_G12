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
        protected string ProfileImageUrl = "~/Images/Profile/default.png"; // Declare property here

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user should be logged out (session timeout or manual logout)
            if (!IsPostBack && Session["Username"] != null && !string.IsNullOrEmpty(Session["Username"].ToString()))
            {
                LoadProfileImage(Session["Username"].ToString());
                // Directly set ImageUrl to ensure display (bypasses binding issues in conditional blocks)
                var profileImg = this.FindControl("ProfileImg") as Image;
                if (profileImg != null)
                {
                    profileImg.ImageUrl = ProfileImageUrl;
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

                    if (result != null && result != DBNull.Value && !string.IsNullOrEmpty(result.ToString().Trim()))
                    {
                        ProfileImageUrl = result.ToString(); // e.g., "~/Images/Profile/user123.jpg"
                    }
                    else
                    {
                        ProfileImageUrl = "~/Images/Profile/default.png"; // Ensure this file exists
                    }
                }
            }
            catch (Exception ex)
            {
                // Log for debugging: Response.Write("<script>console.log('Image Load Error: " + ex.Message + "');</script>");
                ProfileImageUrl = "~/Images/Profile/default.png"; // Fallback
            }
        }
    }
}