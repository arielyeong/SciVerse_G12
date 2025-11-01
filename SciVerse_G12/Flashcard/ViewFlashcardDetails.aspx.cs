using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Flashcard
{
    public partial class ViewFlashcardDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["flashcard_id"] != null)
                {
                    int flashcardId = Convert.ToInt32(Request.QueryString["flashcard_id"]);
                    LoadFlashcards(flashcardId);
                    Session["CurrentIndex"] = 0;
                    Session["IsShowingAnswer"] = false;
                    ShowFlashcard(0, false);
                }
            }
        }

        private void LoadFlashcards(int flashcardId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                SELECT 
                    qz.QuizID,
                    qz.Title AS flashcardTitle,
                    qz.Chapter,
                    qs.QuestionID,
                    qs.QuestionType,
                    qs.QuestionText,
                    op.optionText AS answer
                FROM tblQuiz qz
                JOIN tblQuestion qs ON qz.QuizID = qs.QuizID
                JOIN tblOptions op ON qs.QuestionID = op.QuestionID
                WHERE qz.QuizID = @FlashcardID AND op.isCorrect = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FlashcardID", flashcardId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> questions = new List<string>();
                List<string> answers = new List<string>();
                List<string> questionTypes = new List<string>();
                string flashcardTitle = "";
                string chapter = "";

                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(flashcardTitle))
                    {
                        flashcardTitle = reader["FlashcardTitle"].ToString();
                        chapter = reader["Chapter"].ToString();

                    }
                        questions.Add(reader["questionText"].ToString());
                        answers.Add(reader["answer"].ToString());
                        questionTypes.Add(reader["questionType"].ToString());
                }

                reader.Close();

                // Store in session
                Session["FlashcardTitle"] = flashcardTitle;
                Session["Chapter"] = chapter;
                Session["Questions"] = questions;
                Session["Answers"] = answers;
                Session["QuestionTypes"] = questionTypes;

                System.Diagnostics.Debug.WriteLine($"Loaded {questions.Count} flashcards");
                System.Diagnostics.Debug.WriteLine($"Flashcard Title: {flashcardTitle}");
            }
        }


        private void ShowFlashcard(int index, bool showAnswer)
        {
            List<string> questions = Session["Questions"] as List<string>;
            List<string> answers = Session["Answers"] as List<string>;
            List<string> questionTypes = Session["QuestionTypes"] as List<string>;

            if (questions == null || questions.Count == 0)
            {
                lblFlashcardTitle.Text = "ERROR: No flashcards found";
                lblQuestion.Text = "No questions loaded";
                lblIndex.Text = "Debug: Questions list is empty or null";
                lblQuestionType.Text = ""; 
                return;
            }

            if (index < 0 || index >= questions.Count)
            {
                lblFlashcardTitle.Text = "ERROR: Invalid index";
                lblQuestion.Text = $"Index {index} out of range";
                lblIndex.Text = $"Debug: Valid range 0-{questions.Count - 1}";
                lblQuestionType.Text = ""; 
                return;
            }

            string title = Session["FlashcardTitle"]?.ToString() ?? "Flashcard";
            string chapter = Session["Chapter"]?.ToString();

            if (!string.IsNullOrEmpty(chapter))
            {
                lblFlashcardTitle.Text = $"{chapter}: {title}";
            }
            else
            {
                lblFlashcardTitle.Text = title;
            }

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
                    typeDisplay = "Type";
                    break;
            }

            lblQuestionType.Text = typeDisplay;
            // Show question or answer
            lblQuestion.Text = ""; 
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