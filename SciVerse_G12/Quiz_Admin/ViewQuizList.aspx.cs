using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Admin
{
    public partial class ViewQuizList : System.Web.UI.Page
    {
        private string Mode
        {
            get { return ViewState["Mode"] as string ?? ""; }
            set { ViewState["Mode"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // validate session
            if (Session["RID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            SqlDataSource1.SelectCommand = @"
                SELECT QuizID, Title, Description, Chapter, TimeLimit, ImageURL, CreatedDate, CreatedBy, AttemptLimit
                FROM dbo.tblQuiz
                WHERE Title       LIKE '%' + @Keyword + '%'
                   OR Chapter     LIKE '%' + @Keyword + '%'
                   OR Description LIKE '%' + @Keyword + '%'
                ORDER BY CreatedDate DESC, QuizID DESC";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("Keyword", keyword);
            GridView1.PageIndex = 0;
            GridView1.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectCommand = @"
                SELECT QuizID, Title, Description, Chapter, TimeLimit, ImageURL, CreatedDate, CreatedBy, AttemptLimit
                FROM dbo.tblQuiz
                ORDER BY CreatedDate DESC, QuizID DESC";
            GridView1.PageIndex = 0;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateNewQuizPage.aspx");
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void btnEditMode_Click(object sender, EventArgs e)
        {
            Mode = "Edit";
            ToggleSelectionMode(true);
            btnConfirm.OnClientClick = null;
            btnConfirm.Text = "Confirm";
        }

        protected void btnDeleteMode_Click(object sender, EventArgs e)
        {
            Mode = "Delete";
            ToggleSelectionMode(true);
            btnConfirm.OnClientClick = "openDeleteModal(); return false;";
            btnConfirm.Text = "Confirm Deletion";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Mode = "";
            ToggleSelectionMode(false);
            btnConfirm.OnClientClick = null;
            btnConfirm.Text = "Confirm";
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Mode == "Edit")
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    var check = row.FindControl("chkSelect") as CheckBox;
                    if (check != null && check.Checked)
                    {
                        int quizId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                        Response.Redirect(ResolveUrl("~/Quiz_Admin/EditQuizPage.aspx?quizId=" + quizId), false);
                        return;
                    }
                }
                return;
            }

            if (Mode == "Delete")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "openDeleteModal", "openDeleteModal();", true);
                return;
            }
        }

        private void DeleteQuiz(int quizId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand command = new SqlCommand("DELETE FROM dbo.tblQuiz WHERE QuizID = @id", conn))
            {
                command.Parameters.AddWithValue("@id", quizId);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        private void ToggleSelectionMode(bool enable)
        {
            GridView1.Columns[0].Visible = enable;

            btnConfirm.Visible = enable;
            btnCancel.Visible = enable;
            btnEditMode.Visible = !enable;
            btnDeleteMode.Visible = !enable;

            GridView1.DataBind();
        }

        protected void btnYesDelete_Click(object sender, EventArgs e)
        {
            int deleted = 0;
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (var con = new SqlConnection(connStr))
            {
                con.Open();

                foreach (GridViewRow row in GridView1.Rows)
                {
                    var check = row.FindControl("chkSelect") as CheckBox;
                    if (check == null || !check.Checked) continue;

                    int quizId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

                    // Delete all related questions first (optional, if cascade is not set)
                    using (var cmd1 = new SqlCommand("DELETE FROM dbo.tblQuestion WHERE QuizID=@id", con))
                    {
                        cmd1.Parameters.AddWithValue("@id", quizId);
                        cmd1.ExecuteNonQuery();
                    }

                    // Delete the quiz itself
                    using (var cmd2 = new SqlCommand("DELETE FROM dbo.tblQuiz WHERE QuizID=@id", con))
                    {
                        cmd2.Parameters.AddWithValue("@id", quizId);
                        deleted += cmd2.ExecuteNonQuery();
                    }
                }
            }

            // Refresh grid and show message
            GridView1.DataBind();
            //lblMessage.CssClass = "text-success";
            //lblMessage.Text = $"{deleted} quiz(es) deleted successfully.";
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
    }
}