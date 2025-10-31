using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12.Quiz_Student
{
    public partial class Quiz : System.Web.UI.Page
    {
        // MCQ grid layout: "1x4" or "2x2"
        private const string DefaultMcqLayout = "2x2";

        // Arrays kept in ViewState
        private int[] QIDs;                 // QuestionID[]
        private string[] QTexts;            // QuestionText[]
        private string[] QTypes;            // "MCQ","TF","FIB"
        private int[] QMarks;               // Marks per question
        private string[][] McqOptions;      // options per MCQ question
        private bool[][] McqIsCorrect;      // which option is correct
        private string[] StudentAnswers;    // captured answers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Optional: hide site nav area
                var navLinks = Master?.FindControl("NavLinks");
                if (navLinks != null) navLinks.Visible = false;

                // quiz/user
                int quizId = int.Parse(Request.QueryString["QuizID"] ?? "0");
                int rid = int.Parse(Session["RID"]?.ToString() ?? "0");
                if (quizId == 0 || rid == 0) { Response.Redirect("~/Default.aspx"); return; }
                hfQuizID.Value = quizId.ToString();

                // meta (time limit in minutes → seconds)
                LoadQuizMeta(quizId);

                // timer mode via ?mode=timed
                string mode = (Request.QueryString["mode"] ?? "").ToLowerInvariant();
                int totalSec = int.Parse(hfTotalSeconds.Value ?? "0");
                bool isTimed = (mode == "timed" && totalSec > 0);
                hfIsTimed.Value = isTimed ? "1" : "0";
                hfRemainSeconds.Value = isTimed ? totalSec.ToString() : "0";
                lblTimer.Visible = isTimed;

                // questions
                LoadAllQuestions(quizId);

                // create attempt
                int attemptId = CreateAttempt(quizId, rid);
                hfAttemptID.Value = attemptId.ToString();
                ViewState["AttemptID"] = attemptId;

                // init answers
                StudentAnswers = new string[QIDs.Length];
                ViewState["Ans"] = StudentAnswers;

                // first Q
                ShowQuestion(0);
            }
            else
            {
                // restore arrays
                QIDs = (int[])ViewState["QIDs"];
                QTexts = (string[])ViewState["QTexts"];
                QTypes = (string[])ViewState["QTypes"];
                QMarks = (int[])ViewState["QMarks"];
                McqOptions = (string[][])ViewState["Opts"];
                McqIsCorrect = (bool[][])ViewState["IsCorr"];
                StudentAnswers = ViewState["Ans"] as string[] ?? new string[QIDs.Length];
                ViewState["Ans"] = StudentAnswers;

                if (ViewState["AttemptID"] != null)
                    hfAttemptID.Value = ViewState["AttemptID"].ToString();
                else
                    Response.Redirect("~/Default.aspx");
            }
        }

        private void LoadQuizMeta(int quizId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand("SELECT TimeLimit FROM tblQuiz WHERE QuizID=@QuizID", con))
            {
                cmd.Parameters.AddWithValue("@QuizID", quizId);
                con.Open();
                object val = cmd.ExecuteScalar();
                int totalSec = (val == DBNull.Value) ? 0 : Convert.ToInt32(val) * 60;
                hfTotalSeconds.Value = totalSec.ToString();
            }
        }

        private void LoadAllQuestions(int quizId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            {
                const string sqlQ = @"
                    SELECT QuestionID, QuestionText, QuestionType, Marks
                    FROM tblQuestion
                    WHERE QuizID=@QuizID
                    ORDER BY QuestionID";

                var da = new SqlDataAdapter(sqlQ, con);
                da.SelectCommand.Parameters.AddWithValue("@QuizID", quizId);
                var dt = new DataTable();
                da.Fill(dt);

                int n = dt.Rows.Count;
                QIDs = new int[n];
                QTexts = new string[n];
                QTypes = new string[n];
                QMarks = new int[n];
                McqOptions = new string[n][];
                McqIsCorrect = new bool[n][];

                for (int i = 0; i < n; i++)
                {
                    DataRow r = dt.Rows[i];
                    QIDs[i] = Convert.ToInt32(r["QuestionID"]);
                    QTexts[i] = r["QuestionText"].ToString();
                    QTypes[i] = r["QuestionType"].ToString().ToUpperInvariant();
                    QMarks[i] = Convert.ToInt32(r["Marks"]);

                    if (QTypes[i] == "MCQ")
                    {
                        var (opts, corr) = LoadMcqOptions(QIDs[i]);
                        McqOptions[i] = opts;
                        McqIsCorrect[i] = corr;
                    }
                }

                ViewState["QIDs"] = QIDs;
                ViewState["QTexts"] = QTexts;
                ViewState["QTypes"] = QTypes;
                ViewState["QMarks"] = QMarks;
                ViewState["Opts"] = McqOptions;
                ViewState["IsCorr"] = McqIsCorrect;
            }
        }

        private (string[] options, bool[] isCorrect) LoadMcqOptions(int questionId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                    SELECT OptionText, IsCorrect
                    FROM tblOptions
                    WHERE QuestionID=@QID
                    ORDER BY OptionID", con))
            {
                cmd.Parameters.AddWithValue("@QID", questionId);
                con.Open();
                var rd = cmd.ExecuteReader();

                var optList = new ArrayList();
                var corrList = new ArrayList();
                while (rd.Read())
                {
                    optList.Add(rd["OptionText"].ToString());
                    corrList.Add(Convert.ToBoolean(rd["IsCorrect"]));
                }
                rd.Close();

                return (optList.ToArray(typeof(string)) as string[],
                        corrList.ToArray(typeof(bool)) as bool[]);
            }
        }

        private int CreateAttempt(int quizId, int rid)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                INSERT INTO tblQuizAttempt (RID, QuizID, AttemptDate, Duration, Status)
                VALUES (@RID, @QuizID, GETDATE(), 0, 'InProgress');
                SELECT CAST(SCOPE_IDENTITY() AS INT);", con))
            {
                cmd.Parameters.AddWithValue("@RID", rid);
                cmd.Parameters.AddWithValue("@QuizID", quizId);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void ShowQuestion(int idx)
        {
            if (idx >= QIDs.Length) { FinishQuiz(); return; }

            hfCurrentIdx.Value = idx.ToString();
            lblIndex.Text = $"{idx + 1} / {QIDs.Length}";
            lblQuestionText.Text = QTexts[idx];

            // reset UI
            pMcq.Visible = pTf.Visible = pFib.Visible = false;
            rblMcq.Items.Clear();
            rblTf.ClearSelection();
            txtFib.Text = string.Empty;

            string qType = QTypes[idx];

            if (qType == "MCQ")
            {
                pMcq.Visible = true;
                string[] opts = McqOptions[idx];
                for (int i = 0; i < opts.Length; i++)
                    rblMcq.Items.Add(new ListItem(opts[i], opts[i]));

                var layoutClass = (DefaultMcqLayout == "1x4") ? "layout-1x4" : "layout-2x2";
                rblMcq.CssClass = $"rbl-mcq {layoutClass}";
            }
            else if (qType == "TF")
            {
                pTf.Visible = true;
            }
            else // FILL
            {
                pFib.Visible = true;
            }

            // restore previous selection
            if (StudentAnswers != null && StudentAnswers[idx] != null)
            {
                if (qType == "MCQ") rblMcq.SelectedValue = StudentAnswers[idx];
                else if (qType == "TF") rblTf.SelectedValue = StudentAnswers[idx];
                else txtFib.Text = StudentAnswers[idx];
            }

            // last question = submit
            if (idx == QIDs.Length - 1)
            {
                btnNext.Text = "Submit Quiz";
                btnNext.CssClass = "btn btn-success";
            }
            else
            {
                btnNext.Text = "Next >";
                btnNext.CssClass = "btn btn-primary";
            }

            // hide any previous error
            lblError.Text = string.Empty;
            lblError.Style["display"] = "none";
        }

        private bool IsAnswerProvided(int idx)
        {
            string qType = QTypes[idx];
            if (qType == "MCQ") return rblMcq.SelectedItem != null;
            if (qType == "TF") return rblTf.SelectedItem != null;
            if (qType == "FILL")
            {
                // Read posted value directly to avoid binding issues
                var posted = Request.Form[txtFib.UniqueID];
                return !string.IsNullOrWhiteSpace(posted);
            }
            return false;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int idx = int.Parse(hfCurrentIdx.Value);

            if (!IsAnswerProvided(idx))
            {
                lblError.Text = "Please select or enter an answer before continuing.";
                lblError.Style["display"] = "block";
                return;
            }
            lblError.Text = string.Empty;
            lblError.Style["display"] = "none";

            SaveCurrentAnswer(idx);

            if (idx == QIDs.Length - 1) FinishQuiz();
            else ShowQuestion(idx + 1);
        }

        private void SaveCurrentAnswer(int idx)
        {
            if (string.IsNullOrEmpty(hfAttemptID.Value)) return;

            string qType = QTypes[idx];
            string answer = null;

            if (qType == "MCQ" && rblMcq.SelectedItem != null)
                answer = rblMcq.SelectedValue;
            else if (qType == "TF" && rblTf.SelectedItem != null)
                answer = rblTf.SelectedValue;
            else if (qType == "FILL")
                answer = (Request.Form[txtFib.UniqueID] ?? "").Trim(); // <-- form-read

            StudentAnswers[idx] = answer;
            ViewState["Ans"] = StudentAnswers;

            int attemptId = int.Parse(hfAttemptID.Value);
            SaveAnswerToResult(attemptId, QIDs[idx], answer);
        }

        private void SaveAnswerToResult(int attemptId, int questionId, string studentAnswer)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM tblQuizResult WHERE AttemptID=@AttemptID AND Question=@QID)
                    UPDATE tblQuizResult 
                    SET Answer = @Ans
                    WHERE AttemptID=@AttemptID AND Question=@QID
                ELSE
                    INSERT INTO tblQuizResult (AttemptID, Question, Answer, Marks, Score)
                    VALUES (@AttemptID, @QID, @Ans, 
                            (SELECT Marks FROM tblQuestion WHERE QuestionID=@QID),
                            0)", con))
            {
                cmd.Parameters.AddWithValue("@AttemptID", attemptId);
                cmd.Parameters.AddWithValue("@QID", questionId);
                cmd.Parameters.AddWithValue("@Ans", (object)studentAnswer ?? DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void FinishQuiz()
        {
            int attemptId = int.Parse(hfAttemptID.Value);
            int totalSec = int.Parse(hfTotalSeconds.Value);
            int remainSec = int.Parse(hfRemainSeconds.Value);
            int usedSec = totalSec > 0 ? Math.Max(0, totalSec - remainSec) : 0;

            UpdateAttemptRow(attemptId, usedSec);
            CalculateAndStoreScore(attemptId);

            Response.Redirect($"QuizSummary.aspx?attemptId={attemptId}");
        }

        private void UpdateAttemptRow(int attemptId, int durationSec)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                UPDATE tblQuizAttempt
                SET Duration=@Dur, Status='Submitted'
                WHERE AttemptID=@AttemptID", con))
            {
                cmd.Parameters.AddWithValue("@Dur", durationSec);
                cmd.Parameters.AddWithValue("@AttemptID", attemptId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void CalculateAndStoreScore(int attemptId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                for (int i = 0; i < QIDs.Length; i++)
                {
                    int qid = QIDs[i];
                    string qType = QTypes[i];
                    string student = StudentAnswers[i];
                    int marks = QMarks[i];

                    string correctAns = "";
                    bool correct = false;

                    if (qType == "MCQ")
                    {
                        bool[] corrArr = McqIsCorrect[i];
                        string[] optArr = McqOptions[i];
                        for (int k = 0; k < corrArr.Length; k++)
                            if (corrArr[k]) { correctAns = optArr[k]; break; }
                        correct = (student == correctAns);
                    }
                    else if (qType == "TF" || qType == "FILL")
                    {
                        correctAns = GetCorrectAnswer(qid);
                        correct = string.Equals(student, correctAns, StringComparison.OrdinalIgnoreCase);
                    }

                    using (var cmd = new SqlCommand(@"
                        UPDATE tblQuizResult
                        SET CorrectAnswer = @Corr, Score = @Sc
                        WHERE AttemptID=@AttemptID AND Question=@QID", con))
                    {
                        cmd.Parameters.AddWithValue("@Corr", (object)correctAns ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Sc", correct ? marks : 0);
                        cmd.Parameters.AddWithValue("@AttemptID", attemptId);
                        cmd.Parameters.AddWithValue("@QID", qid);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private string GetCorrectAnswer(int questionId)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(
                "SELECT OptionText FROM tblOptions WHERE QuestionID=@QID AND IsCorrect=1", con))
            {
                cmd.Parameters.AddWithValue("@QID", questionId);
                con.Open();
                object o = cmd.ExecuteScalar();
                return o?.ToString() ?? "";
            }
        }

        protected void btnAutoSubmit_Click(object sender, EventArgs e)
        {
            int idx = int.Parse(hfCurrentIdx.Value);
            SaveCurrentAnswer(idx);
            FinishQuiz();
        }
    }
}
