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
    public partial class ViewUserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            SqlDataSource1.SelectCommand = @"
                SELECT * 
                FROM [tblRegisteredUsers] 
                WHERE username LIKE '%' + @Keyword + '%' 
                   OR emailAddress LIKE '%' + @Keyword + '%' 
                   OR fullName LIKE '%' + @Keyword + '%' 
                   OR country LIKE '%' + @Keyword + '%'";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("Keyword", keyword);

            GridView1.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            SqlDataSource1.SelectCommand = "SELECT * FROM [tblRegisteredUsers]";
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                string username = e.CommandArgument.ToString();
                Response.Redirect("EditUserInfo.aspx?username=" + username);
            }
            else if (e.CommandName == "DeleteUser")
            {
                string username = e.CommandArgument.ToString();
                DeleteUser(username);
                GridView1.DataBind();
            }
        }

        private void DeleteUser(string username)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM tblRegisteredUsers WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}