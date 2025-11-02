<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnswersReview.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.AnswersReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        :root { --muted:#6b7280; --ok:#16a34a; --bad:#dc2626; --border:#e5e7eb; --shadow:0 6px 18px rgba(0,0,0,.06); }
        body { background:#f7f7f7; }
        .wrap { max-width:980px; margin:20px auto 40px; }
        .header { display:flex; align-items:flex-start; justify-content:space-between; gap:16px; margin-bottom:16px; }
        .title { font-size:24px; font-weight:800; }
        .sub { color:var(--muted); margin-top:4px; }
        .chips { display:flex; gap:12px; }
        .chip { min-width:120px; padding:10px 14px; border:1px solid var(--border); border-radius:12px; background:#fff; text-align:center; box-shadow:var(--shadow); }
        .chip .big { font-weight:800; font-size:18px; display:block; }
        .back { display:inline-flex; align-items:center; gap:6px; padding:8px 12px; border:1px solid var(--border); border-radius:10px; background:#fff; cursor:pointer; margin-bottom:14px; }
        .qbox { background:#fff; border:1px solid var(--border); border-radius:14px; padding:16px; box-shadow:var(--shadow); margin-bottom:14px; }
        .qrow { display:flex; align-items:flex-start; gap:12px; }
        .status { width:34px; height:34px; border-radius:50%; display:flex; align-items:center; justify-content:center; font-size:18px; color:#fff; flex:0 0 34px; }
        .status.ok { background:var(--ok); }
        .status.bad { background:var(--bad); }
        .qtitle { font-weight:700; margin-bottom:6px; }
        .muted { color:var(--muted); }
        .score { margin-left:auto; white-space:nowrap; color:#111; }
        .ansline { margin:2px 0; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="wrap">

        <button type="button" class="back" onclick="history.back()">
            ← Back
        </button>

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
                            <div class="ansline"><span class="muted">Your Answer:</span> <%# Eval("UserAnswer") %></div>
                            <div class="ansline"><span class="muted">Correct Answer:</span> <%# Eval("CorrectAnswer") %></div>
                            <div class="ansline muted">Explanation: <%# Eval("Explanation") %></div>                        </div>
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
