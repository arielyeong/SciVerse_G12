<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="NewQuestion.aspx.cs" Inherits="SciVerse_G12.Quiz.NewQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  <div class="d-flex justify-content-center align-items-center" style="min-height:90vh;">
    <div class="card p-4 shadow-lg" style="max-width:1000px; width:100%; border-radius:10px;">
      <h1 class="text-center mb-4">Question Details</h1>

       <!-- keep the QuizID we’re adding to -->
      <asp:HiddenField ID="hiddenQuizID" runat="server" />

      <div class="row g-4">
        <div class="col-12">

          <!-- QuestionText -->
          <div class="mb-3">
            <asp:Label ID="lblQuestionText" runat="server" Text="Question Text:" CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuestion"
                ErrorMessage="Question Text is required" ForeColor="Red" Display="Dynamic" />
          </div>

          <!-- Type + Marks -->
          <div class="row g-3 mb-3">
            <div class="col-md-6">
              <asp:Label runat="server" AssociatedControlID="dropdownType" CssClass="form-label" Text="Question Type:" />
              <asp:DropDownList ID="dropdownType" runat="server" CssClass="form-select" AutoPostBack="true"
                                OnSelectedIndexChanged="dropdownType_SelectedIndexChanged">
              </asp:DropDownList>
              <asp:RequiredFieldValidator runat="server" ControlToValidate="dropdownType"
                CssClass="text-danger" InitialValue="" ErrorMessage="Please choose a type." />
            </div>

            <div class="col-md-6">
              <asp:Label runat="server" AssociatedControlID="txtMarks" CssClass="form-label" Text="Marks:" />
              <asp:TextBox ID="txtMarks" runat="server" CssClass="form-control" TextMode="Number" />
              <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMarks"
                 CssClass="text-danger" ErrorMessage="Marks are required." />
            </div>
          </div>

           <!-- ========== TYPE-SPECIFIC ========== -->

      <!-- MCQ -->
      <asp:Panel ID="panelMCQ" runat="server" Visible="false" CssClass="mb-3">
          <label class="form-label d-block">Options:</label>

          <div class="row g-3">
            <div class="col-md-6">
              <div class="d-flex gap-2 align-items-center">
                <asp:RadioButton ID="radiobtnChoice1" runat="server" GroupName="mcqCorrect" />
                <asp:TextBox ID="txtChoiceA" runat="server" CssClass="form-control" Placeholder="Option A" />
              </div>
            </div>

            <div class="col-md-6">
              <div class="d-flex gap-2 align-items-center">
                <asp:RadioButton ID="radiobtnChoice2" runat="server" GroupName="mcqCorrect" />
                <asp:TextBox ID="txtChoiceB" runat="server" CssClass="form-control" Placeholder="Option B" />
              </div>
            </div>

            <div class="col-md-6">
              <div class="d-flex gap-2 align-items-center">
                <asp:RadioButton ID="radiobtnChoice3" runat="server" GroupName="mcqCorrect" />
                <asp:TextBox ID="txtChoiceC" runat="server" CssClass="form-control" Placeholder="Option C" />
              </div>
            </div>

            <div class="col-md-6">
              <div class="d-flex gap-2 align-items-center">
                <asp:RadioButton ID="radiobtnChoice4" runat="server" GroupName="mcqCorrect" />
                <asp:TextBox ID="txtChoiceD" runat="server" CssClass="form-control" Placeholder="Option D" />
              </div>
            </div>
          </div>

          <small class="text-muted d-block mt-2">Select the radio next to the correct option.</small>
        </asp:Panel>

      <!-- True/False -->
      <asp:Panel ID="panelTF" runat="server" Visible="false" CssClass="mb-3">
        <label class="form-label d-block">Correct Answer:</label>
        <asp:RadioButtonList ID="radiobtnTF" runat="server" CssClass="form-check"
          RepeatDirection="Horizontal" gap="2">
          <asp:ListItem Text="True"  Value="True"></asp:ListItem>
          <asp:ListItem Text="False" Value="False"></asp:ListItem>
        </asp:RadioButtonList>
      </asp:Panel>

      <!-- Fill in the Blanks (start simple with 1 blank) -->
      <asp:Panel ID="panelFill" runat="server" Visible="false" CssClass="mb-3">
        <asp:Label runat="server" AssociatedControlID="txtFillInBlank" CssClass="form-label" Text="Answer for Blank:" />
        <asp:TextBox ID="txtFillInBlank" runat="server" CssClass="form-control" Placeholder="Correct answer" />
      </asp:Panel>

      <!-- Feedback -->
      <div class="mb-3">
        <asp:Label runat="server" AssociatedControlID="txtFeedback" CssClass="form-label" Text="Feedback:" />
        <asp:TextBox ID="txtFeedback" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
      </div>

      <!-- Actions -->
      <div class="d-flex justify-content-center gap-3">
        <asp:Button ID="btnSave" runat="server" Text="Save and continue" 
            CssClass="btn btn-primary" OnClick="btnSave_Click" />
        <asp:HyperLink ID="linkBack" runat="server"
               Text="Back to List"
               CssClass="btn btn-secondary" />
      </div>

      <div class="text-center mt-3">
        <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold" />
      </div>

    </div>
  </div>
          </div>
  </div>

</asp:Content>
