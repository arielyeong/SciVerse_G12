<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizSummary.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
      :root{
        --beige-50:#fbf7f1;
        --ink:#0f172a;
        --muted:#6b7280;
        --primary:#1d4ed8;
        --primary-600:#1e40af;
        --ok:#16a34a;
        --bad:#dc2626;
        --blue-50:#eef2ff;
        --blue-100:#e0e7ff;
        --panel:#f8fafc;
        --shadow:0 12px 36px rgba(2,6,23,.10);
        --radius:22px;
      }
      /* === Full beige background across entire page === */
        html, body {
            height: 100%;
            background: var(--beige-50) !important;
            background-attachment: fixed;
        }
      /* page */
/*      body{ background:#f5f6f9; }*/
      .body-content{ margin-top:8px !important; }

      /* card */
      .qs-card{
        max-width: 720px;
        margin: 26px auto 40px;
        background:#fff;
        border-radius: var(--radius);
        box-shadow: var(--shadow);
        padding: 28px 26px 34px;
      }

      /* trophy + heading */
      .qs-hero{
        display:flex; flex-direction:column; align-items:center; text-align:center;
      }
      .qs-trophy{
        width:88px; height:88px; border-radius:50%;
        background: radial-gradient(circle at 30% 30%, #ffe88a, #ffd84d 60%, #ffcd34);
        display:grid; place-items:center;
        box-shadow: 0 8px 24px rgba(250,204,21,.45), inset 0 2px 8px rgba(255,255,255,.5);
        margin-top: 6px;
      }
      .qs-trophy span{ font-size:38px; }
      .qs-title{
        margin: 14px 0 4px;
        font-size:32px; font-weight:900; color:#1d4ed8; letter-spacing:.2px;
      }
      .qs-sub{ color:var(--muted); font-weight:600; margin-bottom: 16px; }

      /* big score panel */
      .score-panel{
        background: var(--panel);
        border:1px solid #e5e7eb;
        border-radius: 18px;
        padding: 18px 20px;
        margin: 10px auto 18px;
        text-align:center;
      }
      .score-label{ color:var(--muted); font-weight:800; margin-bottom: 6px; }
      .score-value{
        font-size:64px; font-weight:900; color:#0b3cc4; line-height:1.05;
      }
      .score-value small{ font-size:36px; font-weight:800; color:#334155; }

      /* three stat pills */
      .stat-row{
        display:flex; gap:16px; justify-content:center; flex-wrap:wrap;
        margin: 10px 0 20px;
      }
      .pill{
        min-width: 180px;
        border-radius: 16px;
        padding: 14px 18px;
        border:1px solid #e5e7eb;
        background:#fff;
        box-shadow: var(--shadow);
        text-align:center;
      }
      .pill .k{ color:var(--muted); font-weight:700; font-size:13px; letter-spacing:.3px; }
      .pill .v{ font-size:22px; font-weight:900; margin-top: 4px; }
      .pill.time  .v{ color:#0b3cc4; }
      .pill.ok    { background:#ecfdf5; border-color:#bbf7d0; }
      .pill.ok .v{ color: var(--ok); }
      .pill.bad   { background:#fef2f2; border-color:#fecaca; }
      .pill.bad .v{ color: var(--bad); }

      /* centered actions */
      .actions{
        display:flex; flex-direction:column; align-items:center; gap:14px; margin-top: 10px;
      }
      .btn{
        width: 360px; height: 52px; border-radius:14px; border:1px solid #d1d5db;
        background:#fff; cursor:pointer; font-weight:800; font-size:16px;
        box-shadow:0 6px 14px rgba(2,6,23,.08); transition: transform .15s, box-shadow .15s, background .15s;
        display:flex; align-items:center; justify-content:center; gap:10px;
      }
      .btn:hover{ background:#f3f4f6; transform: translateY(-2px); box-shadow:0 10px 20px rgba(2,6,23,.12); }
      .btn-primary{
        color:#fff; background: var(--primary); border-color: transparent;
      }
      .btn-primary:hover{ background: var(--primary-600); }
      .btn-outline{
        background:#fff; color: var(--primary); border:2px solid var(--primary);
      }

      /* hide site nav on this page (optional; comment if not needed) */
      .navbar .navbar-nav{ display:none; }
      .footer{ display:none; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="qs-card">
    <div class="qs-hero">
      <div class="qs-trophy"><span>🏆</span></div>
      <div class="qs-title">Achievement Summary!</div>
      <div class="qs-sub">You did great! Here’s your performance breakdown.</div>
    </div>

    <div class="score-panel">
      <div class="score-label">Your Score:</div>
      <div class="score-value">
        <asp:Literal ID="litScoreNum" runat="server" />/<small><asp:Literal ID="litScoreDen" runat="server" /></small>
      </div>
    </div>

    <div class="stat-row">
      <div class="pill time">
        <div class="k">Time Taken</div>
        <div class="v"><asp:Literal ID="litTime" runat="server" /></div>
      </div>
      <div class="pill ok">
        <div class="k">Correct</div>
        <div class="v"><asp:Literal ID="litCorrect" runat="server" /></div>
      </div>
      <div class="pill bad">
        <div class="k">Incorrect</div>
        <div class="v"><asp:Literal ID="litIncorrect" runat="server" /></div>
      </div>
    </div>

    <div class="actions">
      <asp:Button ID="btnShowAnswers" runat="server" CssClass="btn btn-primary" Text="Review Answers" OnClick="btnShowAnswers_Click" />
      <asp:Button ID="btnFlashcards" runat="server" CssClass="btn" Text="Generate Flashcards" OnClick="btnFlashcards_Click" />
      <asp:Button ID="btnStartAgain"  runat="server" CssClass="btn btn-outline" Text="Take Another Quiz" OnClick="btnStartAgain_Click" />
    </div>

    <!-- hidden for navigation -->
    <asp:HiddenField ID="hidQuizId" runat="server" />
    <asp:HiddenField ID="hidAttemptId" runat="server" />
  </div>
</asp:Content>