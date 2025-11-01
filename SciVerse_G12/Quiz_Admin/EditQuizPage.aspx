<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditQuizPage.aspx.cs" Inherits="SciVerse_G12.Quiz_Admin.EditQuizPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- External CSS in <head> -->
    <link rel="stylesheet" href='<%= ResolveUrl("~/Styles/AdminQuiz.css?v=" + DateTime.Now.Ticks) %>' />   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- keep the current mode for postbacks -->
  <asp:HiddenField ID="hiddenMode" runat="server" />

  <div class="quiz-question-content" id="contentArea">
    <!-- Header row with Back on the right -->
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h1 id="headQuizTitle" runat="server">Quiz</h1>
      <asp:Button ID="btnBack" runat="server" CssClass="btn btn-outline-secondary"
                  Text="Back to List" OnClick="btnBack_Click" />
    </div>

    <!-- Hint + messages -->
    <div class="mb-2">
      <asp:Label ID="lblHint" runat="server" CssClass="text-muted" Visible="false" />
      <asp:Label ID="lblMessage" runat="server" CssClass="ms-3 fw-semibold" />
    </div>

    <!-- Toolbar: normal vs mode -->
    <div class="mb-3 d-flex gap-2">
      <!-- Normal mode buttons -->
      <asp:Button ID="btnNew" runat="server" Text="New Question"
                  CssClass="btn btn-success" OnClick="btnNew_Click" />
      <asp:Button ID="btnEditMode" runat="server" Text="Edit mode"
                  CssClass="btn btn-warning" OnClick="btnEditMode_Click" />
      <asp:Button ID="btnDeleteMode" runat="server" Text="Delete mode"
                  CssClass="btn btn-danger" OnClick="btnDeleteMode_Click" />

      <!-- Mode action buttons (shown only in Edit/Delete mode by SetUiForMode) -->
      <asp:Button ID="btnConfirmEdit" runat="server" Text="Confirm Edit"
                  CssClass="btn btn-primary" Visible="false" OnClick="btnConfirmEdit_Click" />
      <!-- This opens the Bootstrap modal; actual delete happens on btnYesDelete -->
      <button id="btnConfirmDelete" runat="server" visible="false"
              type="button" class="btn btn-danger"
              data-bs-toggle="modal" data-bs-target="#confirmDeleteModal">
        Confirm Delete
      </button>

      <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                  CssClass="btn btn-secondary" Visible="false" OnClick="btnCancel_Click" />
    </div>

    <!-- Data source for questions -->
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="
          SELECT QuestionID, QuestionText, QuestionType
          FROM dbo.tblQuestion
          WHERE QuizID = @QuizID
          ORDER BY QuestionID DESC">
      <SelectParameters>
        <asp:QueryStringParameter Name="QuizID" QueryStringField="quizId" Type="Int32" />
      </SelectParameters>
    </asp:SqlDataSource>

    <!-- Questions grid -->
    <asp:GridView ID="GridView1" runat="server"
        AllowPaging="True" PageSize="10"
        AutoGenerateColumns="False"
        DataKeyNames="QuestionID"
        DataSourceID="SqlDataSource1"
        OnRowDataBound="GridView1_RowDataBound"
        OnPageIndexChanging="GridView1_PageIndexChanging"
        CssClass="table table-bordered table-hover">

      <Columns>
        <asp:TemplateField HeaderText="Select" Visible="false">
          <ItemTemplate>
            <asp:CheckBox ID="chkSelect" runat="server" />
          </ItemTemplate>
          <ItemStyle Width="70px" HorizontalAlign="Center" />
        </asp:TemplateField>

        <asp:BoundField DataField="QuestionText" HeaderText="Question">
          <ItemStyle Width="70%" />
        </asp:BoundField>

        <asp:BoundField DataField="QuestionType" HeaderText="Type">
          <ItemStyle Width="20%" />
        </asp:BoundField>
      </Columns>

      <EmptyDataTemplate>
        <div class="alert alert-info m-2">No questions yet. Click <b>New Question</b> to add one.</div>
      </EmptyDataTemplate>
    </asp:GridView>
  </div>

  <!-- Bootstrap Delete Confirmation Modal -->
  <div class="modal fade" id="confirmDeleteModal" tabindex="-1" aria-labelledby="confirmDeleteLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered"> <!-- Centered vertically -->
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="confirmDeleteLabel">Delete selected question(s)</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            Are you sure you want to delete the selected question(s)? This action cannot be undone.
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No, cancel</button>
            <asp:Button ID="btnYesDelete" runat="server" Text="Yes, delete"
                        CssClass="btn btn-danger" OnClick="btnYesDelete_Click" />
          </div>
        </div>
      </div>
    </div>

</asp:Content>
