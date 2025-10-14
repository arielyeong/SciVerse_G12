using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_and_Flashcard
{
    public partial class Quiz : System.Web.UI.Page
    {
        public class QuizItem
        {
            public int quiz_id { get; set; }
            public string title { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuizzes();
            }
        }

        private void LoadQuizzes()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            List<QuizItem> quizzes = new List<QuizItem>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT quizID, title FROM tblQuiz ORDER BY quizID";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        quizzes.Add(new QuizItem
                        {
                            quiz_id = Convert.ToInt32(reader["quizID"]),
                            title = reader["title"].ToString()
                        });
                    }

                    reader.Close();

                    System.Diagnostics.Debug.WriteLine($"Loaded {quizzes.Count} quizzes");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading quizzes: {ex.Message}");
                    lblNoQuizzes.Text = "Error loading quizzes. Please try again later.";
                    noQuizzesSection.Visible = true;
                    return;
                }
            }

            // Bind data to Repeater
            if (quizzes.Count > 0)
            {
                rptQuizzes.DataSource = quizzes;
                rptQuizzes.DataBind();
                noQuizzesSection.Visible = false;
            }
            else
            {
                noQuizzesSection.Visible = true;
            }
        }

        protected void btnGenerateFlashcard_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                int quizId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Quiz_Flashcard/ViewFlashcard.aspx?quiz_id={quizId}");
            }
        }


    }
}