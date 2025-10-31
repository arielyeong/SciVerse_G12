<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizSummary.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
    /* Hide top navigation bar during quiz */
/* Hide right side navigation links during quiz */
/*.navbar-nav,        if using Bootstrap 
.top-nav, 
.site-menu, 
ul[role="menu"], 
nav ul {
    display: none !important;
}*/

/* Optional: keep the logo centered or left */
/*.navbar-brand, 
.logo {
    visibility: visible !important;
    opacity: 1 !important;
}*/
/* Optional: reduce height of the navbar for a cleaner quiz look */
/*.navbar, header {
    background-color: white;
    height: 60px;
}*/
.body-content{margin-top:8px!important;}
/* Card */
.qs-wrap{
  max-width:760px;
  margin:24px auto;
  background:#fff;
  border-radius:24px;
  box-shadow:0 6px 18px rgba(0,0,0,.06);
  padding:28px 28px 36px;
}
.qs-topbar{display:flex;align-items:center;justify-content:space-between;}
.qs-close{border:none;background:#fff;padding:8px;cursor:pointer;font-size:18px}
.qs-center{text-align:center;margin-top:8px}
.qs-medal{font-size:42px;line-height:1}
.qs-title{margin:10px 0 4px;font-size:28px;font-weight:800}
.qs-sub{color:#6b7280;margin-bottom:22px}

/* Stats cards */
.qs-stats{display:flex;gap:20px;justify-content:center;flex-wrap:wrap;margin:12px 0 22px}
.qs-card{
  min-width:150px;padding:18px 22px;border:1px solid #e5e7eb;
  border-radius:18px;text-align:center;box-shadow:0 6px 18px rgba(0,0,0,.06);
}
.qs-num{font-size:26px;font-weight:800;margin-bottom:6px}
.qs-lbl{color:#6b7280;font-size:14px}

/* Actions row (CENTERED BUTTONS) */
.qs-actions {
    display: flex;
    flex-direction: column;      /* stack vertically */
    align-items: center;         /* center horizontally */
    gap: 18px;                   /* space between buttons */
    margin-top: 26px;
}

.qs-btn {
    width: 320px;                /* fixed width for neat alignment */
    height: 50px;
    border-radius: 14px;
    border: 1px solid #d1d5db;   /* subtle grey border */
    background: #ffffff;
    cursor: pointer;
    font-weight: 600;
    font-size: 16px;
    box-shadow: 0 4px 10px rgba(0,0,0,0.08);
    transition: all 0.2s ease-in-out;
    text-align: center;
}

.qs-btn:hover {
    background: #f3f4f6;
    transform: translateY(-3px);
}

body{background:#f7f7f7}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="qs-wrap">
        <div class="qs-topbar">
           
            <button type="button" class="qs-close" onclick="history.back()">✕</button>
        </div>

        <div class="qs-center">
            <div class="qs-medal">🏅</div>
            <div class="qs-title">Quiz Complete!</div>
            <div class="qs-sub"><asp:Literal ID="litQuizTitle" runat="server" /></div>
        </div>

        <div class="qs-stats">
            <div class="qs-card">
                <div class="qs-num"><asp:Literal ID="litTime" runat="server" /></div>
                <div class="qs-lbl">Total Time</div>
            </div>
            <div class="qs-card">
                <div class="qs-num"><asp:Literal ID="litIncorrect" runat="server" /></div>
                <div class="qs-lbl">Incorrect</div>
            </div>
            <div class="qs-card">
                <div class="qs-num"><asp:Literal ID="litCorrect" runat="server" /></div>
                <div class="qs-lbl">Correct</div>
            </div>
        </div>

        <div class="qs-actions">
            <asp:Button ID="btnShowAnswers" runat="server" CssClass="qs-btn"
                Text="Show Answers" OnClick="btnShowAnswers_Click" />
            <asp:Button ID="btnFlashcards" runat="server" CssClass="qs-btn"
                Text="Generate Flashcards" OnClick="btnFlashcards_Click" />
            <asp:Button ID="btnStartAgain" runat="server" CssClass="qs-btn"
                Text="Start Again" OnClick="btnStartAgain_Click" />
        </div>

        <asp:HiddenField ID="hidQuizId" runat="server" />
        <asp:HiddenField ID="hidAttemptId" runat="server" />
    </div>
</asp:Content>
