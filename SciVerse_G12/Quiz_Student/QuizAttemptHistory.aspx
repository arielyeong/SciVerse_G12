<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizAttemptHistory.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizAttemptHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <style>
    :root {
      --page-bg: #f8f3e9;
      --ink: #1c2833;
      --muted: #62707b;
      --green: #16a34a;
      --amber: #f59e0b;
      --red: #dc2626;
      --blue: #2563eb;
      --blue-dark: #1e40af;
      --card: #ffffff;
      --line: #e5e7eb;
      --shadow: 0 10px 30px rgba(2,6,23,.08);
      --radius: 16px;
    }

    /* ===== FULL PAGE BACKGROUND FIX ===== */
    html, body,
    form,
    .page-wrap,
    .content-wrapper,
    .main-content,
    .container,
    .body-content,
    #MainContent {
      background-color: var(--page-bg) !important;
      background-image: none !important;
      border: none !important;
      margin: 0;
      padding: 0;
    }

    /* Remove any decorative overlays or gradients */
    .page-wrap::before,
    .page-wrap::after,
    .body-content::before,
    .body-content::after,
    .container::before,
    .container::after {
      content: none !important;
      background: none !important;
    }

    /* Keep footer in original dark color */
    .footer {
      background-color: #1e2225 !important;
      background-image: none !important;
      color: #fff !important;
    }

    .footer * {
      background: transparent !important;
      color: inherit !important;
    }

    /* Remove master layout horizontal rule */
    .body-content > hr { display: none !important; }

    /* === Main wrapper === */
    .hist-wrap {
      max-width: 1000px;
      margin: 28px auto 40px;
      padding: 0 16px;
    }

    /* === Header === */
    .hist-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      flex-wrap: wrap;
      gap: 12px;
      margin-bottom: 12px;
    }

    .hist-title {
      font-size: 28px;
      font-weight: 900;
      color: var(--ink);
    }

    .btn-back {
      background: var(--blue);
      color: #fff;
      font-weight: 800;
      padding: 10px 18px;
      border: none;
      border-radius: 10px;
      cursor: pointer;
      transition: background 0.2s, transform 0.2s;
    }

    .btn-back:hover {
      background: var(--blue-dark);
      transform: translateY(-2px);
    }

    .hist-sub {
      color: var(--muted);
      margin-bottom: 20px;
      font-weight: 500;
    }

    /* === Attempt cards === */
    .attempt-card {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 16px;
      background: var(--card);
      border: 1px solid var(--line);
      border-radius: var(--radius);
      padding: 18px 20px;
      box-shadow: var(--shadow);
      margin-bottom: 14px;
    }

    .attempt-left {
      display: flex;
      align-items: center;
      gap: 18px;
      min-width: 0;
    }

    .pct-badge {
      width: 72px;
      height: 72px;
      border-radius: 50%;
      display: grid;
      place-items: center;
      font-size: 20px;
      font-weight: 900;
      color: #fff;
      box-shadow: inset 0 6px 16px rgba(255,255,255,.25);
      flex: 0 0 72px;
    }

    .pct-green { background: var(--green); }
    .pct-amber { background: var(--amber); }
    .pct-red   { background: var(--red); }

    .attempt-info { min-width: 0; }
    .attempt-title { font-weight: 900; color: var(--ink); }
    .attempt-meta { color: var(--muted); font-weight: 600; }

    .attempt-right a {
      color: var(--blue);
      font-weight: 800;
      text-decoration: none;
      display: inline-flex;
      align-items: center;
      gap: 6px;
      padding: 8px 10px;
      border-radius: 8px;
    }

    .attempt-right a:hover { background: #eef2ff; }

    /* === Empty state === */
    .empty {
      background: #fff;
      border: 1px dashed var(--line);
      border-radius: var(--radius);
      padding: 26px;
      text-align: center;
      color: var(--muted);
      font-weight: 700;
      box-shadow: var(--shadow);
    }

    @media (max-width: 640px) {
      .attempt-card { flex-direction: column; align-items: flex-start; }
      .attempt-right { width: 100%; display: flex; justify-content: flex-end; }
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="hist-wrap">

    <!-- HEADER ROW -->
    <div class="hist-header">
      <div class="hist-title">
        <asp:Literal ID="litHeader" runat="server" Text="Quiz Attempt History" />
      </div>
      <asp:Button ID="btnBack" runat="server" Text="← Back to Quizzes"
                  CssClass="btn-back" OnClick="btnBack_Click" />
    </div>

    <div class="hist-sub">
      <asp:Literal ID="litSub" runat="server"
        Text="Track your progress and review your past quiz attempts." />
    </div>

    <!-- EMPTY STATE -->
    <asp:PlaceHolder ID="phEmpty" runat="server" Visible="false">
      <div class="empty">
        <asp:Literal ID="litEmptyMsg" runat="server"
          Text="No attempts yet. Take a quiz to see your history here." />
      </div>
    </asp:PlaceHolder>

    <!-- ATTEMPTS LIST -->
    <asp:Repeater ID="repAttempts" runat="server">
      <ItemTemplate>
        <div class="attempt-card">
          <div class="attempt-left">
            <div class='pct-badge <%# GetPctClass(Eval("Percent")) %>'>
              <%# Eval("Percent") %>%
            </div>

            <div class="attempt-info">
              <div class="attempt-title">
                Attempt <%# Eval("RowNo") %> • <%# Eval("QuizTitle") %>
              </div>
              <div class="attempt-meta">
                <%# Convert.ToDateTime(Eval("AttemptDate")).ToString("MMMM dd, yyyy") %> |
                <%# Convert.ToDateTime(Eval("AttemptDate")).ToString("hh:mm tt") %>
              </div>
            </div>
          </div>

          <div class="attempt-right">
            <a href='<%# "AnswersReview.aspx?attemptId=" + Eval("AttemptID") %>'>
              View Summary →
            </a>
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>

    <!-- Hidden field to store quizId -->
    <asp:HiddenField ID="hidQuizId" runat="server" />
  </div>
</asp:Content>