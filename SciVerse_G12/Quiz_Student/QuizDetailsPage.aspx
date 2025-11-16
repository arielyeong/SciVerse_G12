<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizDetailsPage.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizDetailsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        :root {
            --beige-50:#fbf7f1;
            --beige-100:#f4ecdf;
            --beige-200:#eee2cc;
            --ink:#1c2833;
            --muted:#62707b;
            --accent:#8b7aa7;
            --accent-2:#c89a7d;
            --ok:#2a9d8f;
            --ok-bg:#e9fbf8;
            --shadow: 0 10px 28px rgba(28,40,51,.12);
            --radius: 18px;
        }
        /* === Full beige background across entire page === */
        html, body {
            height: 100%;
            background: var(--beige-50) !important;
            background-attachment: fixed;
        }
/*      === FIX TOP GAP === */
        body, 
        body .body-content,
        body .content-wrapper,
        body .main-content,
        body form,
        body #MainContent,
        body .container {
            background: transparent !important;
            border: none !important;
            box-shadow: none !important;
        }

        body .body-content {
            padding-top: 0 !important;
            margin-top: 0 !important;
        }
        /* === Reduce top gap === */
        .page-wrap {
            max-width: 880px;
            margin: 6px auto 40px; /* reduced top gap */
            padding: clamp(10px, 2vw, 16px);
        }

        .quiz-card {
            background: #fff;
            border: 1px solid rgba(0,0,0,.06);
            border-radius: var(--radius);
            box-shadow: var(--shadow);
            padding: clamp(18px, 2.4vw, 30px);
        }

        .title {
            font-size: clamp(1.6rem, 2.2vw, 2.1rem);
            font-weight: 800;
            color: var(--ink);
            text-align: center;
        }

        .hr {
            width: 72px;
            height: 6px;
            margin: 12px auto 20px;
            border-radius: 999px;
            background: linear-gradient(90deg, var(--accent), var(--accent-2));
            opacity: .9;
        }

        .meta {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 16px;
            margin: 8px 0 16px;
        }
        .meta > div {
            background: var(--beige-100);
            border: 1px solid var(--beige-200);
            border-radius: 14px;
            padding: 14px 12px;
            text-align: center;
            box-shadow: 0 4px 12px rgba(0,0,0,.04) inset;
        }
        .meta .k {
            font-size: .85rem;
            color: var(--muted);
            font-weight: 700;
            letter-spacing: .2px;
            background: rgba(139,122,167,.08);
            border: 1px dashed rgba(139,122,167,.25);
            padding: 6px 10px;
            border-radius: 10px;
            margin-bottom: 6px;
        }
        .meta > div:nth-child(1),
        .meta > div:nth-child(2),
        .meta > div:nth-child(3) {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }

        .summary {
            margin: 10px 0 6px;
            color: var(--muted);
            line-height: 1.6;
        }

        .rules {
            margin-top: 12px;
            background: linear-gradient(180deg, #fff, var(--beige-100));
            border: 1px solid var(--beige-200);
            border-radius: 14px;
            padding: 14px 16px;
        }
        .rules h4 {
            margin: 0 0 6px;
            color: var(--ink);
            font-weight: 800;
        }
        .rules ul {
            margin: 0;
            padding-left: 1.1rem;
            color: var(--muted);
            line-height: 1.55;
        }
        .rules li { margin: 6px 0; }

        .mode {
            margin: 20px 0 6px;
            text-align: center;
        }
        .mode strong {
            display: block;
            margin-bottom: 10px;
            color: var(--ink);
            font-weight: 700;
        }
        .mode-options {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 16px;
        }
        .mode-options input[type="radio"] {
            display: none;
        }
        .mode-options label {
            border: 2px solid var(--beige-200);
            background: #fff;
            border-radius: 999px;
            padding: 10px 28px;
            font-weight: 700;
            cursor: pointer;
            transition: all 0.2s ease;
            color: var(--ink);
            box-shadow: 0 4px 10px rgba(0,0,0,0.03);
        }
        .mode-options input[type="radio"]:checked + label {
            background: linear-gradient(180deg, #fff, #f6f1ff);
            color: var(--accent);
            border-color: var(--accent);
            box-shadow: 0 6px 18px rgba(139,122,167,0.18);
        }
        .start-btn,
        .back-btn{
          width: 260px;          /* must match grid column width above */
          text-align: center;
        }
        .start-wrap {
            display: grid;
            grid-template-columns: repeat(2, 260px);
            justify-content: center;
            margin-top: 22px;
            gap: 16px; /* added to space Back + Start buttons */
        }
        .start-btn {
            border: none;
            border-radius: 999px;
            padding: 14px 38px;
            font-weight: 800;
            color: #fff;
            background: #c6b89f;
            box-shadow: 0 12px 24px rgba(200,154,125,0.25);
            transition: all 0.15s ease;
        }
        .start-btn:hover {
            transform: translateY(-2px);
            filter: brightness(1.05);
        }

        .back-btn {
            border: 2px solid var(--accent);
            border-radius: 999px;
            padding: 12px 32px;
            font-weight: 700;
            background: #fff;
            color: var(--accent);
            transition: all 0.2s ease;
        }
        .back-btn:hover {
            background: var(--accent);
            color: #fff;
            box-shadow: 0 8px 20px rgba(139,122,167,0.2);
        }

        @media (max-width: 680px){
          .start-wrap{
            grid-template-columns: 1fr;
            max-width: 360px;
            margin-left: auto;
            margin-right: auto;
          }
          .start-btn,
          .back-btn{
            width: 100%;
          }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-wrap">
        <div class="quiz-card">
            <div class="title"><asp:Literal ID="titleLiteral" runat="server" /></div>
            <div class="hr"></div>

            <!-- Meta -->
            <div class="meta">
                <div>
                    <div class="k">Time Limit</div>
                    <div><asp:Label ID="lblTimeLimit" runat="server" /></div>
                </div>
                <div>
                    <div class="k">Questions</div>
                    <div><asp:Label ID="lblQuestions" runat="server" /></div>
                </div>
                <div>
                    <div class="k">Attempts Left</div>
                    <div><asp:Label ID="lblAttempts" runat="server" /></div>
                </div>
            </div>

            <!-- Summary -->
            <p class="summary"><asp:Literal ID="litDescription" runat="server" /></p>

            <!-- Rules -->
            <div class="rules">
                <h4>Instructions &amp; Rules</h4>
                <ul>
                    <li>Once started, the timer cannot be paused (if Timed mode).</li>
                    <li>Ensure you have a stable internet connection before beginning.</li>
                    <li>Answers are saved/submitted automatically when the timer ends.</li>
                    <li>Do not refresh the page during the quiz to avoid losing progress.</li>
                </ul>
            </div>

            <!-- Mode -->
            <div class="mode">
                <strong>Time Mode</strong>
                <div class="mode-options">
                    <asp:RadioButton ID="rbTimed" runat="server" GroupName="tm" Text="Timed" Checked="true" />
                    <asp:RadioButton ID="rbUntimed" runat="server" GroupName="tm" Text="Untimed" />
                </div>
            </div>

            <!-- Buttons -->
            <div class="start-wrap">
                <asp:Button ID="btnBack" runat="server" Text="← Back to Dashboard"
                            CssClass="back-btn"
                            PostBackUrl="~/Quiz_Student/QuizDashboardPageStudent.aspx" />
                <asp:Button ID="btnStart" runat="server" Text="Start Quiz Now"
                            CssClass="start-btn"
                            OnClick="btnStart_Click" />
            </div>
        </div>
    </div>
</asp:Content>