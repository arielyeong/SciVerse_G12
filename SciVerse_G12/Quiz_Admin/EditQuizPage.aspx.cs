using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz
{
    public partial class EditQuizPage : System.Web.UI.Page
    {
        private int QuizId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["quizId"], out id) ? id : 0;
            }
        }

        private string Mode
        {
            get { return ViewState["Mode"] as string ?? ""; }
            set { ViewState["Mode"] = value ?? ""; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (QuizId <= 0)
            {
                Response.Redirect("~/Quiz_Admin/ViewQuizList.aspx", false);
                Context.ApplicationInstance?.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadQuizHeader(QuizId);
                SetUiForMode(""); // normal mode at start
            }
        }

        private void LoadQuizHeader(int quizId)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(connStr))
            using (var command = new SqlCommand("SELECT Title FROM dbo.tblQuiz WHERE QuizID=@id", con))
            {
                command.Parameters.AddWithValue("@id", quizId);
                con.Open();
                var titleObj = command.ExecuteScalar();
                var title = (titleObj == null || titleObj == DBNull.Value) ? "(Untitled Quiz)" : titleObj.ToString();
                headQuizTitle.InnerText = $"Edit Quiz: {title} (ID {quizId})";
            }
        }


        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Quiz_Admin/NewQuestion.aspx?quizId={QuizId}", false);
        }

        protected void btnEditMode_Click(object sender, EventArgs e)
        {
            Mode = "Edit";
            SetUiForMode(Mode);
        }

        protected void btnDeleteMode_Click(object sender, EventArgs e)
        {
            Mode = "Delete";
            SetUiForMode(Mode);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Mode = "";
            SetUiForMode(Mode);
        }

        // Confirm for EDIT mode (server)
        protected void btnConfirmEdit_Click(object sender, EventArgs e)
        {
            int? selectedQid = GetFirstCheckedQuestionId();
            if (selectedQid == null)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please select one question to edit.";
                return;
            }

            Response.Redirect($"~/Quiz_Admin/NewQuestion.aspx?quizId={QuizId}&questionId={selectedQid.Value}", false);
        }

        // Confirm for DELETE mode (from modal "Yes, delete")
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

                    int questionid = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

                    // delete options first
                    using (var cmdOpt = new SqlCommand("DELETE FROM dbo.tblOption WHERE QuestionID=@questionid", con))
                    {
                        cmdOpt.Parameters.AddWithValue("@questionid", questionid);
                        cmdOpt.ExecuteNonQuery();
                    }

                    // then delete the question
                    using (var cmdQ = new SqlCommand("DELETE FROM dbo.tblQuestion WHERE QuestionID=@questionid", con))
                    {
                        cmdQ.Parameters.AddWithValue("@questionid", questionid);
                        deleted += cmdQ.ExecuteNonQuery();
                    }
                }
            }

            GridView1.DataBind();
            Mode = "";
            SetUiForMode(Mode);

            lblMessage.CssClass = "text-success";
            lblMessage.Text = $"{deleted} question(s) deleted.";
        }


        private void SetUiForMode(string mode)
        {
            // Column[0] is the checkbox column
            GridView1.Columns[0].Visible = (mode == "Edit" || mode == "Delete");

            btnConfirmEdit.Visible = (mode == "Edit");
            btnConfirmDelete.Visible = (mode == "Delete");
            btnCancel.Visible = (mode == "Edit" || mode == "Delete");

            btnEditMode.Visible = (mode == "");
            btnDeleteMode.Visible = (mode == "");

            lblHint.Visible = (mode == "Edit" || mode == "Delete");
            lblHint.Text = (mode == "Edit")
                            ? "Edit mode: tick one row, then Confirm"
                            : (mode == "Delete")
                            ? "Delete mode: tick rows, then Confirm"
                            : "";
        }

        private int? GetFirstCheckedQuestionId()
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                var check = row.FindControl("chkSelect") as CheckBox;
                if (check != null && check.Checked)
                {
                    return Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                }
            }
            return null;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Quiz_Admin/ViewQuizList.aspx", false);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Keep the checkbox column aligned with current Mode
            bool showSelect = (Mode == "Edit" || Mode == "Delete");

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Hide/show first column in the header
                if (e.Row.Cells.Count > 0)
                    e.Row.Cells[0].Visible = showSelect;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Hide/show first column in each data row
                if (e.Row.Cells.Count > 0)
                    e.Row.Cells[0].Visible = showSelect;

                // Also toggle the checkbox control itself 
                var check = e.Row.FindControl("chkSelect") as CheckBox;
                if (check != null) check.Visible = showSelect;
            }
        }
    }
}