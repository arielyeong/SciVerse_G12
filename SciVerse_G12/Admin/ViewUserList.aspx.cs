using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
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
                // Validation: Ensure admin access
                if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
                {
                    Response.Redirect("~/Account/Login.aspx?ReturnUrl=" + Request.Url.PathAndQuery);
                    return;
                }

                // Initial bind
                BindGrid();
            }
        }
        private void BindGrid(string keyword = "")
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string query;

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    // Search only in RID (ID), username, fullName, emailAddress, country
                    query = @"
                SELECT * FROM [tblRegisteredUsers]
                WHERE CAST(RID AS VARCHAR(10)) LIKE @Keyword 
                   OR username LIKE @Keyword 
                   OR fullName LIKE @Keyword 
                   OR emailAddress LIKE @Keyword 
                   OR country LIKE @Keyword";
                    SqlDataSource1.SelectCommand = query;
                    SqlDataSource1.SelectParameters.Clear();
                    SqlDataSource1.SelectParameters.Add("Keyword", "%" + keyword + "%");  // Fixed: No @ prefix
                }
                else
                {
                    query = "SELECT * FROM [tblRegisteredUsers]";
                    SqlDataSource1.SelectCommand = query;
                    SqlDataSource1.SelectParameters.Clear();
                }

                SqlDataSource1.ConnectionString = cs;
                GridView1.DataBind();

                // Hide no-results if data exists
                noResults.Visible = (GridView1.Rows.Count == 0);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading users: " + ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Validation: Empty search = full list
                BindGrid();
                return;
            }

            // Length validation
            if (keyword.Length > 100)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Search term too long.');", true);
                return;
            }

            BindGrid(keyword);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindGrid(); // Full list
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
            if (string.IsNullOrEmpty(Mode))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('No mode selected.');", true);
                return;
            }

            if (GridView1.DataKeys.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('No data available for action.');", true);
                return;
            }

            bool hasSelection = false;
            var ridsToProcess = new List<string>();  // Collect RIDs first for batch processing

            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk != null && chk.Checked)
                {
                    hasSelection = true;
                    // Safe DataKeys access
                    if (row.RowIndex >= 0 && row.RowIndex < GridView1.DataKeys.Count)
                    {
                        string rid = GridView1.DataKeys[row.RowIndex].Value?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(rid))
                        {
                            ridsToProcess.Add(rid);
                        }
                    }
                }
            }

            if (!hasSelection)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('No users selected.');", true);
                return;
            }

            try
            {
                if (Mode == "Edit")
                {
                    // Edit only first selected (as before)
                    if (ridsToProcess.Count > 0)
                    {
                        Response.Redirect($"~/Admin/EditUserInfo.aspx?rid={ridsToProcess[0]}");
                    }
                    return;
                }
                else if (Mode == "Delete")
                {
                    // Batch delete
                    foreach (string rid in ridsToProcess)
                    {
                        DeleteUser(rid);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Action failed: {ex.Message}";
                lblError.Visible = true;
                return;
            }

            // Refresh after delete
            if (Mode == "Delete")
            {
                BindGrid(txtSearch.Text?.Trim() ?? "");
                Mode = "";
                ToggleSelectionMode(false);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('User deleted successfully.');", true);
            }
        }

        private void DeleteUser(string rid)
        {
            if (string.IsNullOrEmpty(rid)) return;

            try
            {
                string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();  // Open once outside

                    // Validate: Exists and not current user
                    string checkQuery = "SELECT COUNT(*) FROM tblRegisteredUsers WHERE RID = @rid AND username != @currentUser";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@rid", rid);
                        checkCmd.Parameters.AddWithValue("@currentUser", Session["Username"]?.ToString() ?? "");
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            throw new Exception("Invalid user or self-delete attempt.");
                        }
                    }

                    // Delete
                    string query = "DELETE FROM tblRegisteredUsers WHERE RID = @rid";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@rid", rid);
                        cmd.ExecuteNonQuery();
                    }
                    // No manual close; using con handles it
                }
            }
            catch (Exception ex)
            {
                // Log or rethrow if needed; for now, let caller handle
                throw new Exception($"Delete failed for RID {rid}: {ex.Message}");
            }
        }

        private void ToggleSelectionMode(bool enable)
        {
            // Validation: Only for admin
            if (Session["Role"]?.ToString() != "Admin") return;

            if (GridView1.Columns.Count > 0)
            {
                GridView1.Columns[0].Visible = enable;
            }

            btnConfirm.Visible = enable;
            btnCancel.Visible = enable;
            btnEditMode.Visible = !enable;
            btnDeleteMode.Visible = !enable;

            // Rebind
            BindGrid(txtSearch.Text?.Trim() ?? "");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Image fallback
                Image img = (Image)e.Row.FindControl("imgUser");
                if (img != null && string.IsNullOrEmpty(img.ImageUrl))
                {
                    img.ImageUrl = "~/Images/Profile/default.png";
                }

                // Truncate long text for layout (with bounds check)
                if (e.Row.Cells.Count > 3 && !string.IsNullOrEmpty(e.Row.Cells[3].Text) && e.Row.Cells[3].Text.Length > 20)
                    e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 20) + "..."; // Full Name
                if (e.Row.Cells.Count > 4 && !string.IsNullOrEmpty(e.Row.Cells[4].Text) && e.Row.Cells[4].Text.Length > 25)
                    e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 25) + "..."; // Email
                if (e.Row.Cells.Count > 7 && !string.IsNullOrEmpty(e.Row.Cells[7].Text) && e.Row.Cells[7].Text.Length > 15)
                    e.Row.Cells[7].Text = e.Row.Cells[7].Text.Substring(0, 15) + "..."; // Country
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex < 0 || e.NewPageIndex >= GridView1.PageCount) return; // Bounds validation

            GridView1.PageIndex = e.NewPageIndex;
            BindGrid(txtSearch.Text?.Trim() ?? "");
        }

        // Helper for safe image URL
        protected string GetImageUrl(object picture)
        {
            string imgUrl = picture?.ToString()?.Trim() ?? "";
            if (string.IsNullOrEmpty(imgUrl) || imgUrl == "NULL")
                return "~/Images/Profile/default.png";

            if (!imgUrl.StartsWith("~") && !imgUrl.StartsWith("/"))
                imgUrl = "~/Images/Profile/" + imgUrl;

            return imgUrl;
        }

        // Image error handler
        protected void imgUser_Error(object sender, ImageClickEventArgs e)
        {
            ((Image)sender).ImageUrl = "~/Images/Profile/default.png";
        }
    }
}