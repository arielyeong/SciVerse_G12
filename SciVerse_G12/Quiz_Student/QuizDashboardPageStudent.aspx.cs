using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Student
{
    public partial class QuizDashboardPageStudent : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    // On initial page load, we don't show any quiz cards. This happens when the user clicks the Search button.
        //    if (!IsPostBack)
        //    {
        //        LoadData();
        //        LoadChapterDropdown();  // Load chapter options for the dropdown
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            // ✅ TEMP: Hardcode RID = 5 for testing (replace with login system later)
            if (Session["RID"] == null)
            {
                Session["RID"] = 5; // default student ID for development
            }

            if (!IsPostBack)
            {
                LoadData();
                LoadChapterDropdown();  // Load chapter options for the dropdown
            }
        }


        // Load data method with optional keyword and chapter filter
        private void LoadData(string keyword = "", string chapterFilter = "")
        {
            string query = "SELECT QuizID, Title, Description, Chapter, TimeLimit, ImageURL, AttemptLimit FROM tblQuiz";

            // Add filtering logic based on keyword and chapter
            if (!string.IsNullOrEmpty(keyword) || !string.IsNullOrEmpty(chapterFilter))
            {
                query += " WHERE 1=1";  // Always true, allows easy appending of multiple filters

                // Apply the keyword filter (searching in Title, Description, TimeLimit)
                if (!string.IsNullOrEmpty(keyword))
                {
                    query += " AND (Title LIKE @Keyword OR Description LIKE @Keyword OR TimeLimit LIKE @Keyword)";
                }

                // Apply chapter filter if provided
                if (!string.IsNullOrEmpty(chapterFilter) && chapterFilter != "0")  // "0" is for "Select Chapter"
                {
                    query += " AND Chapter = @ChapterFilter";
                }
            }

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Add the parameters for keyword search
                if (!string.IsNullOrEmpty(keyword))
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }

                // Add the parameter for chapter filter
                if (!string.IsNullOrEmpty(chapterFilter) && chapterFilter != "0")
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@ChapterFilter", chapterFilter);
                }

                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                // Bind the fetched data to the Repeater control
                Repeater_QuizCards.DataSource = dt;
                Repeater_QuizCards.DataBind();
            }
        }

        // Event handler for the search button click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the keyword from the TextBox
            string keyword = txtSearch.Text.Trim();

            // Get the selected chapter from the dropdown
            string chapterFilter = DropDownList_FilterByChapter.SelectedValue;

            // Only show the quiz cards after the user clicks Search, based on the chapter and keyword
            LoadData(keyword, chapterFilter);
        }

        // Event handler for the clear button click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            // Clear the search box
            txtSearch.Text = string.Empty;

            // Reset the chapter dropdown to default ("Select Chapter")
            DropDownList_FilterByChapter.SelectedIndex = 0;

            // Load all quizzes without any filter
            LoadData();
        }

        // Method to load chapter options into the DropDownList
        private void LoadChapterDropdown()
        {
            // Query to get unique chapters from the database
            string query = "SELECT DISTINCT Chapter FROM tblQuiz ORDER BY Chapter";

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                // Bind the data to the DropDownList
                DropDownList_FilterByChapter.DataSource = dt;
                DropDownList_FilterByChapter.DataTextField = "Chapter";
                DropDownList_FilterByChapter.DataValueField = "Chapter";
                DropDownList_FilterByChapter.DataBind();

                // Add a default "Select Chapter" option
                DropDownList_FilterByChapter.Items.Insert(0, new ListItem("Select Chapter", "0"));
            }
        }

        protected void DropDownList_FilterByChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected chapter from the dropdown
            string selectedChapter = DropDownList_FilterByChapter.SelectedValue;

            // Reload quizzes based on the selected chapter and the current search query
            string keyword = txtSearch.Text.Trim();
            LoadData(keyword, selectedChapter);
        }

        protected void Repeater_QuizCards_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Retrieve the QuizID passed via CommandArgument
            if (!int.TryParse(e.CommandArgument.ToString(), out int quizId))
                return;

            if (e.CommandName == "start")
            {
                // When "Start Quiz" button clicked
                Response.Redirect($"~/Quiz_Student/QuizDetailsPage.aspx?quizId={quizId}");
            }
            else if (e.CommandName == "result")
            {
                // When "View Result" button clicked
                Response.Redirect($"~/Quiz_Student/QuizResultPage.aspx?quizId={quizId}");
            }
        }
    }
}