<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizDetailsPage.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizDetailsPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .body-content { margin-top:10px !important; }
        .page-wrap { padding: 24px; }
        .quiz-card {
            max-width: 900px; margin: 0 auto; background: #fff;
            border: 1px solid #e6e6e6; border-radius: 12px; padding: 28px 28px 36px;
            box-shadow: 0 2px 8px rgba(0,0,0,.05);
        }
        .title { font-size: 22px; font-weight: 700; margin: 6px 0 14px; }
        .hr { height:1px; background:#e7e7e7; margin: 10px 0 18px; }
        .meta { display: grid; grid-template-columns: 180px 1fr; row-gap: 10px; column-gap: 18px; margin: 14px 0 8px; }
        .meta .k { color:#777; }
        .summary { color:#333; margin: 12px 0 8px; }
        .rules { background:#fbfbfb; border:1px solid #eee; border-radius:10px; padding:14px 16px; margin-top:8px; }
        .rules h4 { margin:0 0 8px; font-size:15px; }
        .rules ul { margin:0 0 0 18px; }
        .rules li { margin:6px 0; color:#555; }
        .mode { display:flex; align-items:center; gap:18px; margin:18px 0; }
        .start-btn {
            display:block; width:100%; max-width:520px; margin:18px auto 0;
            padding:12px 18px; border-radius:24px; border:1px solid #ddd;
            background:#fff; cursor:pointer; font-weight:600;
        }
        .start-btn:hover { background:#f6f6f6; }
        @media (max-width:560px) { .meta { grid-template-columns:1fr; } }
    </style>

    <div class="page-wrap">
        <div class="quiz-card">
            <div class="title"><asp:Literal ID="titleLiteral" runat="server" /></div>
            <div class="hr"></div>

            <div class="meta">
                <div class="k">Time Limit</div>
                <div><asp:Label ID="lblTimeLimit" runat="server" /></div>

                <div class="k">Questions</div>
                <div><asp:Label ID="lblQuestions" runat="server" /></div>

                <div class="k">Attempts Left</div>
                <div><asp:Label ID="lblAttempts" runat="server" /></div>
            </div>

            <p class="summary">
                A brief summary of the topics covered in the quiz:
                <asp:Literal ID="litDescription" runat="server" />
            </p>

            <div class="rules">
                <h4>Instructions / Rules</h4>
                <ul>
                    <li>The timer will begin as soon as you click ‘Start Quiz Now’ (if Timed mode).</li>
                    <li>You cannot pause the quiz once started.</li>
                    <li>Ensure you have a stable internet connection.</li>
                    <li>Your score will be displayed immediately upon completion.</li>
                </ul>
            </div>

            <div class="mode">
                <strong>Time Mode</strong>
                <asp:RadioButtonList ID="rblTimeMode" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Timed" Value="timed" Selected="True" />
                    <asp:ListItem Text="Untimed" Value="untimed" />
                </asp:RadioButtonList>
            </div>

            <asp:Button ID="btnStart" runat="server" Text="Start Quiz Now"
                        CssClass="start-btn" OnClick="btnStart_Click" />
        </div>
    </div>
</asp:Content>