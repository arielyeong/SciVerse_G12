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
        private string[] QTypes;            // "MCQ","TrueFalse","FillInBlanks"
        private int[] QMarks;               // Marks per question
        private string[][] McqOptions;      // options per MCQ question (texts)
        private bool[][] McqIsCorrect;      // which option is correct
        private string[] StudentAnswers;    // captured answers

        private string ConnStr => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        // Read RID strictly from Session (set during login)
        private int CurrentRid
        {
            get
            {
                int rid = 0;
                if (Session["RID"] != null)
                    int.TryParse(Session["RID"].ToString(), out rid);
                return rid;
            }
        }

        // ========= STRING NORMALIZATION HELPERS (fix strict matching) =========
        private static string Normalize(string s)
        {
            if (s == null) return "";
            // Convert NBSP to normal space
            s = s.Replace('\u00A0', ' ');

            // Trim
            s = s.Trim();

            // Collapse multiple spaces to single
            while (s.Contains("  ")) s = s.Replace("  ", " ");

            return s;
        }

        private static bool TextEqual(string a, string b)
        {
            return string.Equals(Normalize(a), Normalize(b), StringComparison.OrdinalIgnoreCase);
        }
        // =====================================================================

        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce login for ALL requests (first load + postbacks)
            if (Session["RID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                // quiz/user
                int quizId = int.Parse(Request.QueryString["QuizID"] ?? "0");
                int rid = CurrentRid;
                if (quizId == 0)
                {
                    // Invalid or missing quizId → back to dashboard
                    Response.Redirect("~/Quiz_Student/QuizDashboardPageStudent.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                hfQuizID.Value = quizId.ToString();

                // ===================== RESUME FROM PAUSE ======================
                bool resuming = (Session["Paused_AttemptId"] != null);
                if (resuming)
                {
                    // 1) Restore minimal state for timer + attempt
                    int attemptId = Convert.ToInt32(Session["Paused_AttemptId"]);
                    int pausedIdx = Convert.ToInt32(Session["Paused_Idx"] ?? 0);
                    int remain = Convert.ToInt32(Session["Paused_Remain"] ?? 0);

                    // Meta (total seconds) still needed for duration calc later
                    LoadQuizMeta(quizId);

                    // 2) Questions
                    LoadAllQuestions(quizId);

                    // 3) Restore attempt + timer hidden fields
                    hfAttemptID.Value = attemptId.ToString();
                    ViewState["AttemptID"] = attemptId;

                    hfIsTimed.Value = (remain > 0) ? "1" : "0";
                    hfRemainSeconds.Value = remain.ToString();

                    // Toggle timer UI (label + pill container)
                    bool showTimer = (hfIsTimed.Value == "1");
                    lblTimer.Visible = showTimer;
                    if (timerWrap != null) timerWrap.Visible = showTimer;

                    // 4) Prepare answers array (if not yet)
                    StudentAnswers = new string[QIDs.Length];
                    ViewState["Ans"] = StudentAnswers;

                    // 5) Show the exact paused question
                    ShowQuestion(pausedIdx);

                    // 6) One-shot cleanup
                    Session.Remove("Paused_AttemptId");
                    Session.Remove("Paused_QuizId");
                    Session.Remove("Paused_Idx");
                    Session.Remove("Paused_TotalQ");
                    Session.Remove("Paused_Remain");
                    Session.Remove("Paused_TotalSec");
                    Session.Remove("Paused_Mode");

                    return; // IMPORTANT: do NOT create a new attempt when resuming
                }
                // ===============================================================

                // Fresh start path (not resuming)
                // meta (time limit in minutes → seconds)
                LoadQuizMeta(quizId);

                // timer mode via ?mode=timed OR ?mode=untimed
                string mode = (Request.QueryString["mode"] ?? "").ToLowerInvariant();

                // If caller passed &sec for timed mode, use it. Otherwise default to full limit.
                int totalSecMeta = int.Parse(hfTotalSeconds.Value ?? "0");
                int secParam = 0;
                int.TryParse(Request.QueryString["sec"], out secParam);

                bool isTimed = (mode == "timed" && (secParam > 0 || totalSecMeta > 0));
                int totalSec = isTimed ? (secParam > 0 ? secParam : totalSecMeta) : 0;

                hfIsTimed.Value = isTimed ? "1" : "0";
                hfRemainSeconds.Value = isTimed ? totalSec.ToString() : "0";

                // Toggle timer UI (label + pill container)
                lblTimer.Visible = isTimed;
                if (timerWrap != null) timerWrap.Visible = isTimed;

                // questions
                LoadAllQuestions(quizId);

                // create attempt
                int newAttemptId = CreateAttempt(quizId, rid);
                hfAttemptID.Value = newAttemptId.ToString();
                ViewState["AttemptID"] = newAttemptId;

                // init answers
                StudentAnswers = new string[QIDs.Length];
                ViewState["Ans"] = StudentAnswers;

                // first Q
                ShowQuestion(0);
            }
            else
            {
                // restore arrays after postback
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
                {
                    // If we somehow lost attempt on postback, bail to dashboard
                    Response.Redirect("~/Quiz_Student/QuizDashboardPageStudent.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
        }

        private void LoadQuizMeta(int quizId)
        {
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand("SELECT TimeLimit FROM dbo.tblQuiz WHERE QuizID=@QuizID", con))
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
            using (var con = new SqlConnection(ConnStr))
            {
                const string sqlQ = @"
                    SELECT QuestionID, QuestionText, QuestionType, Marks
                    FROM dbo.tblQuestion
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
                    QTypes[i] = r["QuestionType"].ToString().Trim();
                    QMarks[i] = Convert.ToInt32(r["Marks"]);

                    if (QTypes[i] == "MCQ")
                    {
                        var (opts, corr) = LoadMcqOptions(QIDs[i]);
                        // normalize texts once here to avoid trailing-space issues
                        for (int k = 0; k < opts.Length; k++) opts[k] = Normalize(opts[k]);
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
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(@"
                    SELECT OptionText, IsCorrect
                    FROM dbo.tblOptions
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
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.tblQuizAttempt (RID, QuizID, AttemptDate, Duration, Status)
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
                    rblMcq.Items.Add(new ListItem(opts[i], opts[i])); // value = text (kept for now)

                var layoutClass = (DefaultMcqLayout == "1x4") ? "layout-1x4" : "layout-2x2";
                rblMcq.CssClass = $"rbl-mcq {layoutClass}";
            }
            else if (qType == "TrueFalse")
            {
                pTf.Visible = true;
            }
            else if (qType == "FillInBlanks")
            {
                pFib.Visible = true;
            }

            // restore previous selection
            if (StudentAnswers != null && StudentAnswers[idx] != null)
            {
                if (qType == "MCQ") rblMcq.SelectedValue = StudentAnswers[idx];
                else if (qType == "TrueFalse") rblTf.SelectedValue = StudentAnswers[idx];
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
            if (qType == "TrueFalse") return rblTf.SelectedItem != null;
            if (qType == "FillInBlanks")
            {
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
                answer = rblMcq.SelectedValue; // currently stores text
            else if (qType == "TrueFalse" && rblTf.SelectedItem != null)
                answer = rblTf.SelectedValue;
            else if (qType == "FillInBlanks")
                answer = (Request.Form[txtFib.UniqueID] ?? "").Trim();

            StudentAnswers[idx] = answer;
            ViewState["Ans"] = StudentAnswers;

            int attemptId = int.Parse(hfAttemptID.Value);
            SaveAnswerToResult(attemptId, QIDs[idx], answer);
        }

        private void SaveAnswerToResult(int attemptId, int questionId, string studentAnswer)
        {
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM dbo.tblQuizResult WHERE AttemptID=@AttemptID AND Question=@QID)
                    UPDATE dbo.tblQuizResult 
                    SET Answer = @Ans
                    WHERE AttemptID=@AttemptID AND Question=@QID
                ELSE
                    INSERT INTO dbo.tblQuizResult (AttemptID, Question, Answer, Marks, Score)
                    VALUES (@AttemptID, @QID, @Ans, 
                            (SELECT Marks FROM dbo.tblQuestion WHERE QuestionID=@QID),
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

            // For untimed quizzes totalSec == 0 → usedSec = 0
            int usedSec = totalSec > 0 ? Math.Max(0, totalSec - remainSec) : 0;

            UpdateAttemptRow(attemptId, usedSec);
            CalculateAndStoreScore(attemptId);

            Response.Redirect($"QuizSummary.aspx?attemptId={attemptId}", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void UpdateAttemptRow(int attemptId, int durationSec)
        {
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.tblQuizAttempt
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
            using (var con = new SqlConnection(ConnStr))
            {
                con.Open();
                for (int i = 0; i < QIDs.Length; i++)
                {
                    int qid = QIDs[i];
                    string qType = QTypes[i];
                    string student = StudentAnswers[i];
                    int marks = QMarks[i];

                    // normalize student answer once
                    string studentNorm = Normalize(student);
                    string correctAns = "";
                    bool correct = false;

                    if (qType == "MCQ")
                    {
                        // find the correct option text (already normalized in LoadAllQuestions)
                        bool[] corrArr = McqIsCorrect[i];
                        string[] optArr = McqOptions[i];
                        for (int k = 0; k < corrArr.Length; k++)
                            if (corrArr[k]) { correctAns = optArr[k]; break; }

                        correct = TextEqual(studentNorm, correctAns);
                    }
                    else if (qType == "TrueFalse" || qType == "FillInBlanks")
                    {
                        // Support multiple correct answers (synonyms)
                        var answers = GetAllCorrectAnswers(qid);
                        foreach (var a in answers)
                        {
                            if (TextEqual(studentNorm, a)) { correct = true; correctAns = a; break; }
                            // if none matched, still show the first correct as reference
                            if (string.IsNullOrEmpty(correctAns)) correctAns = a;
                        }
                    }

                    using (var cmd = new SqlCommand(@"
                        UPDATE dbo.tblQuizResult
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

        // Returns ALL correct answers for a question (handles TF and FIB synonyms)
        private string[] GetAllCorrectAnswers(int questionId)
        {
            var list = new System.Collections.Generic.List<string>();
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                "SELECT OptionText FROM dbo.tblOptions WHERE QuestionID=@QID AND IsCorrect=1 ORDER BY OptionID", con))
            {
                cmd.Parameters.AddWithValue("@QID", questionId);
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var t = rd["OptionText"]?.ToString() ?? "";
                        list.Add(Normalize(t));
                    }
                }
            }
            if (list.Count == 0) list.Add(""); // safe fallback
            return list.ToArray();
            // Note: For MCQ we already loaded + normalized texts once; TF/FIB read here is fine.
        }

        private string GetCorrectAnswer(int questionId) // kept in case other pages call it
        {
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                "SELECT TOP(1) OptionText FROM dbo.tblOptions WHERE QuestionID=@QID AND IsCorrect=1", con))
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

        protected void btnPause_Click(object sender, EventArgs e)
        {
            // Still enforce login on action
            if (Session["RID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect($"~/Account/Login.aspx?returnUrl={returnUrl}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Safeguards
            if (string.IsNullOrEmpty(hfAttemptID.Value) || string.IsNullOrEmpty(hfQuizID.Value))
            {
                Response.Redirect("~/Quiz_Student/QuizDashboardPageStudent.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Gather current state from hidden fields
            int attemptId = Convert.ToInt32(hfAttemptID.Value);
            int quizId = Convert.ToInt32(hfQuizID.Value);
            int idx = Convert.ToInt32(hfCurrentIdx.Value); // 0-based
            int totalQ = (QIDs != null) ? QIDs.Length : 0;
            int remainSec = Convert.ToInt32(string.IsNullOrEmpty(hfRemainSeconds.Value) ? "0" : hfRemainSeconds.Value);
            int totalSec = Convert.ToInt32(string.IsNullOrEmpty(hfTotalSeconds.Value) ? "0" : hfTotalSeconds.Value);
            string mode = (hfIsTimed.Value == "1") ? "timed" : "untimed";

            // Persist to Session for QuizPause.aspx to read
            Session["Paused_AttemptId"] = attemptId;
            Session["Paused_QuizId"] = quizId;
            Session["Paused_Idx"] = idx;
            Session["Paused_TotalQ"] = totalQ;
            Session["Paused_Remain"] = remainSec;
            Session["Paused_TotalSec"] = totalSec;
            Session["Paused_Mode"] = mode;

            // Optional: mark attempt as Paused in DB
            try
            {
                using (var con = new SqlConnection(ConnStr))
                using (var cmd = new SqlCommand(
                    "UPDATE dbo.tblQuizAttempt SET Status='Paused' WHERE AttemptID=@A", con))
                {
                    cmd.Parameters.AddWithValue("@A", attemptId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* ignore and continue */ }

            // Go to Pause screen
            Response.Redirect("~/Quiz_Student/QuizPause.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
