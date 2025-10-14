using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
            //if (!IsPostBack)
            //{
            //    // When page first loads, show all quizzes (no filter)
            //    //SqlDataSource1.SelectParameters["kw"].DefaultValue = "";
            //}
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

        // 📝 Row command handler (Edit/Delete)
        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "EditQuiz")
        //    {
        //        var quizId = e.CommandArgument.ToString();
        //        Response.Redirect("EditQuizPage.aspx?quizId=" + quizId);
        //    }
        //    else if (e.CommandName == "DeleteQuiz")
        //    {
        //        // ⬇️ Put this line here:
        //        SqlDataSource1.DeleteParameters["QuizID"].DefaultValue = e.CommandArgument.ToString();

        //        SqlDataSource1.Delete();
        //        GridView1.DataBind();
        //    }
        //}

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
                var chk = row.FindControl("chkSelect") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    int quizId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

                    if (Mode == "Edit")
                    {
                        Response.Redirect("EditQuizPage.aspx?quizId=" + quizId);
                        return; // stop here; redirect ends the request
                    }
                    else if (Mode == "Delete")
                    {
                        DeleteQuiz(quizId);
                    }
                }
            }

            if (Mode == "Delete")
            {
                GridView1.DataBind();
                Mode = "";
                ToggleSelectionMode(false);
            }
        }

        private void DeleteQuiz(int quizId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM dbo.tblQuiz WHERE QuizID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", quizId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            // If you have FK CASCADE from tblQuestion to tblQuiz, its questions will auto-delete.
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // optional: put row-level tweaks here
        }

        //protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridView1.PageIndex = e.NewPageIndex;
        //    GridView1.DataBind();
        //}
    }
}
