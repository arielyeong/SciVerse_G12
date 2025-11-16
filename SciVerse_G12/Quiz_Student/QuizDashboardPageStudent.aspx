<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizDashboardPageStudent.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizDashboardPageStudent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/StudentQuiz.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="quiz-section">
    <h1>Quizz</h1>

    <!-- toolbar -->
    <div class="quiz-toolbar">
      <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
        Placeholder="Search by Title / Chapter / Description" />
      <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary"
        OnClick="btnSearch_Click" />
      <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary"
        OnClick="btnClear_Click" />
      <asp:DropDownList ID="DropDownList_FilterByChapter" runat="server"
        CssClass="form-select" AutoPostBack="true"
        OnSelectedIndexChanged="DropDownList_FilterByChapter_SelectedIndexChanged" />
    </div>

    <!-- grid -->
    <div class="quiz-grid">
      <asp:Repeater ID="Repeater_QuizCards" runat="server"
        OnItemCommand="Repeater_QuizCards_ItemCommand"
        OnItemDataBound="Repeater_QuizCards_ItemDataBound">
        <ItemTemplate>
          <div class="quiz-card">
            <div class="qc-media">
              <img src='<%# ResolveUrl(Eval("ImageURL").ToString()) %>' alt="Quiz image" />
            </div>

            <div class="qc-body">
              <div class="title"><%# Eval("Title") %></div>

              <div class="q-metrics">
                <div class="q-metric">
                  <i class="fa-solid fa-book-open"></i>
                  <span>Chapter: <%# Eval("Chapter") %></span>
                </div>
                <div class="q-metric">
                  <i class="fa-regular fa-clock"></i>
                  <span>Time Limit: <%# Eval("TimeLimit") %> mins</span>
                </div>
                <div class="q-metric">
                  <i class="fa-solid fa-arrows-rotate"></i>
                  <span>Attempts Left: <asp:Label ID="lblAttemptsLeft" runat="server" /></span>
                </div>
              </div>

              <div class="q-divider"></div>
              <p class="q-desc"><%# Eval("Description") %></p>

              <!-- dynamic status -->
              <asp:Literal ID="litStatus" runat="server" />
            </div>

            <div class="actions">
              <asp:Button ID="btnStartQuiz" runat="server"
                Text="Start Quiz" CssClass="btn-pill btn-primary"
                CommandName="start" CommandArgument='<%# Eval("QuizID") %>' />
              <asp:Button ID="btnViewResult" runat="server"
                Text="View Result" CssClass="btn-pill btn-outline"
                CommandName="result" CommandArgument='<%# Eval("QuizID") %>' />
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>
</asp:Content>