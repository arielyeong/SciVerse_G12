<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizPause.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizPause" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .quiz-menu { max-width: 760px; margin: 32px auto 80px; text-align: center; }
    .topbar { display: flex; align-items: center; gap: 12px; margin-bottom: 24px; }
    .logo-btn { border: 1px solid #ccc; background:#fff; padding:6px 12px; border-radius:8px; }

    .count-pill {
      display:inline-block; padding:8px 16px; border:1px solid #c9b9b9;
      border-radius:999px; font-weight:600; margin-bottom:16px; min-width:110px;
    }

    .capsule-track {
      position:relative; height:24px; border:1px solid #c9b9b9; border-radius:999px;
      background:#fff; overflow:hidden; margin:0 auto 6px; max-width:520px;
    }
    .capsule-fill { position:absolute; left:0; top:0; bottom:0; width:0%; background:#e9dede; }
    .capsule-legend { display:flex; justify-content:space-between; max-width:520px; margin:6px auto 28px; color:#666; font-size:14px; }

    .btn-row { display:flex; gap:24px; justify-content:center; margin-top:16px; flex-wrap:wrap; }
    .btn-outline { border:1px solid #c9b9b9; background:#fff; padding:10px 18px; border-radius:8px; min-width:140px; cursor:pointer; }
    .btn-outline:hover { background:#fafafa; }

    .msg { margin-top:12px; }
  </style>

  <div class="quiz-menu">

    <!-- Top bar -->
    <div class="topbar">
      <span style="color:#bbb;flex:1 1 auto;border-top:1px solid #ddd;"></span>
    </div>

    <!-- Question counter -->
    <div class="count-pill">
      <asp:Literal ID="litCounter" runat="server" /> <!-- e.g., "1 / 20" -->
    </div>

    <!-- Progress -->
    <div class="capsule-track">
      <div id="capsuleFill" runat="server" class="capsule-fill"></div>
    </div>
    <div class="capsule-legend"><span>Start</span><span>End</span></div>

    <!-- Score (only when finished) -->
    <asp:PlaceHolder ID="phScore" runat="server" Visible="false">
      <div class="count-pill" style="margin-top:10px;">
        <asp:Literal ID="litScore" runat="server" />  <!-- "15 / 20 (75%)" -->
      </div>
    </asp:PlaceHolder>

    <!-- No-question message -->
    <asp:PlaceHolder ID="phNoQuestions" runat="server" Visible="false">
      <div class="msg text-warning">There are no questions in this quiz.</div>
    </asp:PlaceHolder>

    <!-- Buttons -->
    <div class="btn-row">
      <asp:Button ID="btnResume"   runat="server" Text="Resume"     CssClass="btn-outline" OnClick="btnResume_Click" />
      <asp:Button ID="btnSaveExit" runat="server" Text="Save &amp; Exit" CssClass="btn-outline" OnClick="btnSaveExit_Click" />
    </div>

  </div>
</asp:Content>
