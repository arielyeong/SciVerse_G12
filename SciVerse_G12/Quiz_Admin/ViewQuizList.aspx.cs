using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz
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
            if (Session["Username"] != null)
            {
                string username = Session["Username"].ToString();

                // Load data
                GridView1.DataBind();
            }
            else
            {
                // If not logged in, redirect to login page
                Response.Redirect("~/Account/Login.aspx");
                return;
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

            // Restore normal behavior
            btnConfirm.OnClientClick = null;
            btnConfirm.Text = "Confirm";
        }

        protected void btnDeleteMode_Click(object sender, EventArgs e)
        {
            Mode = "Delete";
            ToggleSelectionMode(true);

            // Open modal without postback
            btnConfirm.OnClientClick = "openDeleteModal(); return false;";
            btnConfirm.Text = "Confirm Deletion";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Mode = "";
            ToggleSelectionMode(false);

            // Restore normal behavior
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
                // just open the modal, NOT delete here.
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
            // show/hide first column (checkboxes)
            GridView1.Columns[0].Visible = enable;

            btnConfirm.Visible = enable;
            btnCancel.Visible = enable;
            btnEditMode.Visible = !enable;
            btnDeleteMode.Visible = !enable;

            GridView1.DataBind();
        }

        protected void btnConfirmDeleteYes_Click(object sender, EventArgs e)
        {
            int deleted = 0;
            foreach (GridViewRow row in GridView1.Rows)
            {
                var chk = row.FindControl("chkSelect") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    int quizId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                    DeleteQuiz(quizId);
                    deleted++;
                }
            }

            GridView1.DataBind();
            Mode = "";
            ToggleSelectionMode(false);

            ScriptManager.RegisterStartupScript(
                this, GetType(), "hideModalAndToast",
                "var m=bootstrap.Modal.getInstance(document.getElementById('deleteModal')); if(m){m.hide();} " +
                (deleted > 0 ? "alert('Deleted " + deleted + " quiz(es).');" : "alert('No quiz selected.');"),
                true
            );
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


    }
}
