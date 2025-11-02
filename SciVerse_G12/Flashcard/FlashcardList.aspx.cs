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
    public partial class FlashcardList : System.Web.UI.Page
    {
        public class FlashcardItem
        {
            public int flashcard_id { get; set; }
            public string title { get; set; }
            public string chapter { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFlashcards();
            }
        }

        private void LoadFlashcards()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            List<FlashcardItem> flashcards = new List<FlashcardItem>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT QuizID as flashcardId, Title, Chapter 
                    FROM tblQuiz 
                    ORDER BY flashcardId";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        flashcards.Add(new FlashcardItem
                        {
                            flashcard_id = Convert.ToInt32(reader["flashcardId"]),
                            title = reader["Title"].ToString(),
                            chapter = reader["Chapter"].ToString()
                        });
                    }

                    reader.Close();

                    System.Diagnostics.Debug.WriteLine($"Loaded {flashcards.Count} flashcard");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading flashcard: {ex.Message}");
                    lblNoFlashcards.Text = "Error loading flashcard. Please try again later.";
                    noFlashcardsSection.Visible = true;
                    return;
                }
            }

            // Bind data to Repeater
            if (flashcards.Count > 0)
            {
                rptFlashcards.DataSource = flashcards;
                rptFlashcards.DataBind();
                noFlashcardsSection.Visible = false;
            }
            else
            {
                noFlashcardsSection.Visible = true;
            }
        }

        protected void btnGenerateFlashcard_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                int flashcardId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Flashcard/ViewFlashcardDetails.aspx?flashcard_id={flashcardId}");
            }
        }

        protected void rptFlashcards_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}