<%@ Page Title="Edit Quiz" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="EditQuizPage.aspx.cs"
    Inherits="SciVerse_G12.Quiz.EditQuizPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <div class="quiz-question-content" id="contentArea">
    <!-- Header -->
    <div class="content-header">
      <h1 id="hQuizTitle" runat="server">Quiz</h1>
    </div>

    <!-- Toolbar -->
    <div class="mb-3">
      <asp:Button ID="btnNew"    runat="server" Text="New"    CssClass="btn btn-secondary mt-2" OnClick="btnNew_Click" />
      <asp:Button ID="btnEdit"   runat="server" Text="Edit"   CssClass="btn btn-secondary mt-2" OnClick="btnEdit_Click" />
      <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-secondary mt-2" OnClick="btnDelete_Click" />
    </div>

    <!-- SqlDataSource for questions of this quiz -->
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="
          SELECT QuestionID, QuestionText, QuestionType
          FROM dbo.tblQuestion
          WHERE QuizID = @QuizID
          ORDER BY QuestionID">
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
          <ItemStyle Width="60px" HorizontalAlign="Center" />
        </asp:TemplateField>

        <asp:BoundField DataField="QuestionText" HeaderText="Question">
          <ItemStyle Width="400px" />
        </asp:BoundField>

        <asp:BoundField DataField="QuestionType" HeaderText="Type">
          <ItemStyle Width="120px" />
        </asp:BoundField>
      </Columns>

      <EmptyDataTemplate>
        <div class="alert alert-info" style="margin:8px 0;">No questions yet. Click <b>New</b> to add one.</div>
      </EmptyDataTemplate>
    </asp:GridView>
  </div>

</asp:Content>
