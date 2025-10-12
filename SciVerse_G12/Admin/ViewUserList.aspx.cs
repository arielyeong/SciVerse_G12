using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SciVerse_G12
{
    public partial class ViewUserList : System.Web.UI.Page
    {
        private string Mode
        {
            get { return ViewState["Mode"] as string ?? ""; }
            set { ViewState["Mode"] = value; }
        }

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
                SELECT * FROM [tblRegisteredUsers]
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

        protected void btnEditMode_Click(object sender, EventArgs e)
        {
            Mode = "Edit";
            ToggleSelectionMode(true);
        }

        protected void btnDeleteMode_Click(object sender, EventArgs e)
        {
            Mode = "Delete";
            ToggleSelectionMode(true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Mode = "";
            ToggleSelectionMode(false);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk != null && chk.Checked)
                {
                    string username = GridView1.DataKeys[row.RowIndex].Value.ToString();

                    if (Mode == "Edit")
                    {
                        Response.Redirect("EditUserInfo.aspx?username=" + username);
                        return; // Redirect ends the loop
                    }
                    else if (Mode == "Delete")
                    {
                        DeleteUser(username);
                    }
                }
            }

            // Refresh list after delete
            if (Mode == "Delete")
            {
                GridView1.DataBind();
                Mode = "";
                ToggleSelectionMode(false);
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

        private void ToggleSelectionMode(bool enable)
        {
            // Show/hide checkbox column
            GridView1.Columns[0].Visible = enable;
            btnConfirm.Visible = enable;
            btnCancel.Visible = enable;
            btnEditMode.Visible = !enable;
            btnDeleteMode.Visible = !enable;
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind(); // Rebind data after page index changes
        }
    }
}
