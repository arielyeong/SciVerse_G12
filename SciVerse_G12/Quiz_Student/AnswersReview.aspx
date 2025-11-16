<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnswersReview.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.AnswersReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        :root {
            --beige-50:#fbf7f1;
            --muted:#6b7280;
            --ok:#16a34a;
            --bad:#dc2626;
            --border:#e5e7eb;
            --shadow:0 6px 18px rgba(0,0,0,.06);
            --primary:#1d4ed8;
            --radius:16px;
        }
        html, body {
            height: 100%;
            background: var(--beige-50) !important;
            background-attachment: fixed;
        }

/*        body { background:#f5f6f9; }*/
        .body-content { margin-top:12px!important; }

        .wrap {
          max-width:980px;
          margin:20px auto 50px;
          padding:0 10px;
        }

        /* Header */
        .back {
          display:inline-flex; align-items:center; gap:6px;
          padding:8px 12px;
          border:1px solid var(--border);
          border-radius:10px;
          background:#fff;
          cursor:pointer;
          margin-bottom:18px;
          box-shadow:var(--shadow);
          font-weight:600;
          transition:transform .15s;
        }
        .back:hover{ transform:translateY(-2px); }

        .header {
          display:flex; align-items:flex-start; justify-content:space-between;
          gap:16px; margin-bottom:20px;
          flex-wrap:wrap;
        }
        .title { font-size:26px; font-weight:900; color:var(--primary); }
        .sub { color:var(--muted); margin-top:4px; font-size:14px; }

        /* Chips (summary stats) */
        .chips { display:flex; gap:12px; flex-wrap:wrap; }
        .chip {
          min-width:150px;
          padding:10px 14px;
          border:1px solid var(--border);
          border-radius:var(--radius);
          background:#fff;
          text-align:center;
          box-shadow:var(--shadow);
        }
        .chip .big { font-weight:900; font-size:20px; display:block; margin-bottom:2px; }

        /* Question Box */
        .qbox {
          background:#fff;
          border:1px solid var(--border);
          border-radius:var(--radius);
          padding:18px 18px 14px;
          box-shadow:var(--shadow);
          margin-bottom:14px;
        }
        .qrow { display:flex; align-items:flex-start; gap:12px; }
        .status {
          width:36px; height:36px; border-radius:50%;
          display:flex; align-items:center; justify-content:center;
          font-size:20px; color:#fff; flex:0 0 36px;
          font-weight:bold;
        }
        .status.ok { background:var(--ok); }
        .status.bad { background:var(--bad); }
        .qtitle { font-weight:700; margin-bottom:6px; font-size:15px; }
        .muted { color:var(--muted); }
        .score { margin-left:auto; white-space:nowrap; color:#111; font-weight:600; }
        .ansline { margin:3px 0; font-size:14px; }

        .empty {
          text-align:center;
          margin-top:40px;
          color:var(--muted);
          font-weight:600;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="wrap">

  <button type="button" class="back" onclick="history.back()">← Back</button>

  <div class="header">
    <div>
      <div class="title">
        <asp:Literal ID="litHeader" runat="server" Text="Quiz Results"></asp:Literal>
      </div>
      <div class="sub">
        Attempted Date:
        <asp:Literal ID="litDate" runat="server" />
      </div>
    </div>

    <div class="chips">
      <div class="chip">
        <span class="big"><asp:Literal ID="litTimeTaken" runat="server" /></span>
        Time Taken
      </div>
      <div class="chip">
        <span class="big"><asp:Literal ID="litOverallScore" runat="server" /></span>
        Overall Score
      </div>
    </div>
  </div>

  <asp:Repeater ID="repQuestions" runat="server" OnItemDataBound="repQuestions_ItemDataBound">
    <ItemTemplate>
      <div class="qbox">
        <div class="qrow">
          <div class="status" runat="server" id="statusBubble">✓</div>
          <div style="flex:1 1 auto;">
            <div class="qtitle">
              Question <%# Eval("QuestionNo") %>: <%# Eval("QuestionText") %>
            </div>
            <div class="ansline">
              <span class="muted">Your Answer:</span>
              <%# string.IsNullOrWhiteSpace(Eval("UserAnswer")?.ToString()) ? "<i>Not answered</i>" : Eval("UserAnswer") %>
            </div>
            <div class="ansline">
              <span class="muted">Correct Answer:</span> <%# Eval("CorrectAnswer") %>
            </div>
            <div class="ansline muted">
              Explanation: <%# Eval("Explanation") %>
            </div>
          </div>
          <div class="score">
            <%# Eval("ScoreAwarded") %>/<%# Eval("ScorePossible") %>
          </div>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>

  <asp:Literal ID="litEmpty" runat="server" />
  <asp:HiddenField ID="hidAttemptId" runat="server" />
  <asp:HiddenField ID="hidQuizId" runat="server" />

</div>
</asp:Content>