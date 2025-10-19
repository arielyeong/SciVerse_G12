using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            // Require a valid quizId in the URL
            if (QuizId <= 0)
            {
                Response.Redirect("ViewQuizList.aspx", endResponse: false);
                Context.ApplicationInstance?.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadQuizHeader(QuizId);
            }
        }

        private void LoadQuizHeader(int quizId)
        {
            // Fetch the quiz title to show in the H1
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT Title FROM dbo.tblQuiz WHERE QuizID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", quizId);
                con.Open();
                var titleObj = cmd.ExecuteScalar();
                var title = (titleObj == null || titleObj == DBNull.Value) ? "(Untitled Quiz)" : titleObj.ToString();
                hQuizTitle.InnerText = $"Edit Quiz: {title} (ID {quizId})";
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Optional per-row UI tweaks
        }

        // === Buttons ===

        protected void btnNew_Click(object sender, EventArgs e)
        {
            // go to your Add-Question page (create this page if you don't have one)
            Response.Redirect("AddQuestionMCQ.aspx?quizId=" + QuizId);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Example: get selected QuestionID then redirect to an edit-question page
            foreach (GridViewRow row in GridView1.Rows)
            {
                var chk = row.FindControl("chkSelect") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    int questionId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                    Response.Redirect($"EditQuestion.aspx?quizId={QuizId}&questionId={questionId}");
                    return;
                }
            }
            // If you want, show a message when nothing is selected
            // ClientScript.RegisterStartupScript(GetType(), "msg", "alert('Please select a question to edit.');", true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Example: delete selected question(s)
            var cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand("DELETE FROM [tblQuestion] WHERE QuestionID = @qid AND QuizID = @quiz", con))
            {
                cmd.Parameters.Add("@qid", System.Data.SqlDbType.Int);
                cmd.Parameters.Add("@quiz", System.Data.SqlDbType.Int).Value = QuizId;

                con.Open();
                foreach (GridViewRow row in GridView1.Rows)
                {
                    var chk = row.FindControl("chkSelect") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        int questionId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                        cmd.Parameters["@qid"].Value = questionId;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}