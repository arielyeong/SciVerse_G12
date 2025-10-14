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
    public partial class ViewFlashcard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["quiz_id"] != null)
                {
                    int quizId = Convert.ToInt32(Request.QueryString["quiz_id"]);
                    LoadQuiz(quizId);
                    Session["CurrentIndex"] = 0;
                    Session["IsShowingAnswer"] = false;
                    ShowFlashcard(0, false);
                }
            }
        }

        private void LoadQuiz(int quizId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                SELECT 
                    qz.quizID,
                    qz.title AS quizTitle,
                    qs.questionID,
                    qs.questionType,
                    qs.questionText,
                    op.optionText AS answer
                FROM tblQuiz qz
                JOIN tblQuestion qs ON qz.quizID = qs.quizID
                JOIN tblOptions op ON qs.questionID = op.questionID
                WHERE qz.quizID = @quizID AND op.isCorrect = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@quizID", quizId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> questions = new List<string>();
                List<string> answers = new List<string>();
                List<string> questionTypes = new List<string>();
                string quizTitle = "";

                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(quizTitle))
                    {
                        quizTitle = reader["quizTitle"].ToString();

                    }
                        questions.Add(reader["questionText"].ToString());
                        answers.Add(reader["answer"].ToString());
                        questionTypes.Add(reader["questionType"].ToString());
                }

                reader.Close();

                // Store in session
                Session["QuizTitle"] = quizTitle;
                Session["Questions"] = questions;
                Session["Answers"] = answers;
                Session["QuestionTypes"] = questionTypes;

                System.Diagnostics.Debug.WriteLine($"Loaded {questions.Count} flashcards");
                System.Diagnostics.Debug.WriteLine($"Quiz Title: {quizTitle}");
            }
        }


        private void ShowFlashcard(int index, bool showAnswer)
        {
            List<string> questions = Session["Questions"] as List<string>;
            List<string> answers = Session["Answers"] as List<string>;
            List<string> questionTypes = Session["QuestionTypes"] as List<string>;

            if (questions == null || questions.Count == 0)
            {
                lblQuizName.Text = "ERROR: No flashcards found";
                lblQuestion.Text = "No questions loaded";
                lblIndex.Text = "Debug: Questions list is empty or null";
                lblQuestionType.Text = ""; 
                return;
            }

            if (index < 0 || index >= questions.Count)
            {
                lblQuizName.Text = "ERROR: Invalid index";
                lblQuestion.Text = $"Index {index} out of range";
                lblIndex.Text = $"Debug: Valid range 0-{questions.Count - 1}";
                lblQuestionType.Text = ""; 
                return;
            }

            lblQuizName.Text = Session["QuizTitle"]?.ToString() ?? "Quiz";

            string questionText = questions[index];
            string answerText = answers[index];
            string displayQuestion = questionText.Replace("[blank]", "<span style='display:inline-block; border-bottom:2px solid currentColor; width:120px; vertical-align:middle;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            string questionType = questionTypes[index];
            string typeDisplay = "";
            switch (questionType.ToLower())
            {
                case "mcq":
                    typeDisplay = "Multiple Choice Question";
                    break;
                case "t/f":
                    typeDisplay = "True or False";
                    break;
                case "fib":
                    typeDisplay = "Fill in the Blanks";
                    break;
                default:
                    typeDisplay = "Unknown Type";
                    break;
            }

            lblQuestionType.Text = typeDisplay;
            // Show question or answer
            lblQuestion.Text = ""; // clear first
            lblQuestion.Controls.Clear();

            if (showAnswer)
            {
                lblQuestion.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(answerText)));
            }
            else
            {
                lblQuestion.Controls.Add(new LiteralControl(displayQuestion));
            }

            lblIndex.Text = $"Card {index + 1} of {questions.Count}";
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            Session["CurrentIndex"] = 0;
            Session["IsShowingAnswer"] = false;
            ShowFlashcard(0, false);
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            List<string> questions = Session["Questions"] as List<string>;
            if (questions == null || questions.Count == 0) return;

            int currentIndex = (int)(Session["CurrentIndex"] ?? 0);
            currentIndex = (currentIndex - 1 + questions.Count) % questions.Count;

            Session["CurrentIndex"] = currentIndex;
            Session["IsShowingAnswer"] = false;
            ShowFlashcard(currentIndex, false);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            List<string> questions = Session["Questions"] as List<string>;
            if (questions == null || questions.Count == 0) return;

            int currentIndex = (int)(Session["CurrentIndex"] ?? 0);
            currentIndex = (currentIndex + 1) % questions.Count;

            Session["CurrentIndex"] = currentIndex;
            Session["IsShowingAnswer"] = false;
            ShowFlashcard(currentIndex, false);
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            List<string> questions = Session["Questions"] as List<string>;
            if (questions == null || questions.Count == 0) return;

            int index = questions.Count - 1;
            Session["CurrentIndex"] = index;
            Session["IsShowingAnswer"] = false;
            ShowFlashcard(index, false);
        }

        protected void btnShowAns_Click(object sender, EventArgs e)
        {
            int index = (int)(Session["CurrentIndex"] ?? 0);
            bool showAnswer = !(bool)(Session["IsShowingAnswer"] ?? false);
            Session["IsShowingAnswer"] = showAnswer;

            ShowFlashcard(index, showAnswer);
        }
    }
}