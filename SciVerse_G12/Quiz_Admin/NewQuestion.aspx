<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="NewQuestion.aspx.cs" Inherits="SciVerse_G12.Quiz_Admin.NewQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/AdminQuiz.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <style>
        .card-max { max-width: 1000px; width: 100%; border-radius: 12px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="container py-5">
    <div class="card p-4 shadow-lg mx-auto card-max">
      <h1 class="text-center mb-4">Question Details</h1>

      <!-- keep the QuizID we’re adding to -->
      <asp:HiddenField ID="hiddenQuizID" runat="server" />

      <!-- show all validation messages (in red) -->
      <asp:ValidationSummary ID="valSummary" runat="server"
          ValidationGroup="Q"
          CssClass="text-danger mb-3"
          HeaderText="Please fix the following:" />

      <div class="row g-4">
        <div class="col-12">

          <!-- QuestionText -->
          <div class="mb-3">
            <asp:Label ID="lblQuestionText" runat="server" Text="Question Text:" CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="rfvQuestion" runat="server"
                ControlToValidate="txtQuestion"
                ErrorMessage="Question Text is required"
                Display="Dynamic" CssClass="text-danger"
                ValidationGroup="Q" />
          </div>

          <!-- Type + Marks -->
          <div class="row g-3 mb-3">
            <div class="col-md-6">
              <asp:Label runat="server" AssociatedControlID="dropdownType" CssClass="form-label" Text="Question Type:" />
              <asp:DropDownList ID="dropdownType" runat="server" CssClass="form-select" AutoPostBack="true"
                                OnSelectedIndexChanged="dropdownType_SelectedIndexChanged">
              </asp:DropDownList>
              <asp:RequiredFieldValidator ID="rfvType" runat="server"
                ControlToValidate="dropdownType"
                InitialValue=""
                ErrorMessage="Please choose a type."
                CssClass="text-danger"
                Display="Dynamic"
                ValidationGroup="Q" />
            </div>

            <div class="col-md-6">
              <asp:Label runat="server" AssociatedControlID="txtMarks" CssClass="form-label" Text="Marks:" />
              <asp:TextBox ID="txtMarks" runat="server" CssClass="form-control" TextMode="Number" />
              <asp:RequiredFieldValidator ID="rfvMarks" runat="server"
                 ControlToValidate="txtMarks"
                 CssClass="text-danger" Display="Dynamic"
                 ErrorMessage="Marks are required."
                 ValidationGroup="Q" />
              <asp:RangeValidator ID="rngMarks" runat="server"
                 ControlToValidate="txtMarks" Type="Integer"
                 MinimumValue="1" MaximumValue="100"
                 CssClass="text-danger" Display="Dynamic"
                 ErrorMessage="Marks must be between 1 and 100."
                 ValidationGroup="Q" />
            </div>
          </div>

          <!-- ========== TYPE-SPECIFIC ========== -->

          <!-- MCQ -->
          <asp:Panel ID="panelMCQ" runat="server" Visible="false" CssClass="mb-3">
            <label class="form-label d-block">Options:</label>

            <div class="row g-3">
              <!-- A -->
              <div class="col-md-6">
                <div class="d-flex gap-2 align-items-center">
                  <asp:RadioButton ID="radiobtnChoice1" runat="server" GroupName="mcqCorrect" />
                  <asp:TextBox ID="txtChoiceA" runat="server" CssClass="form-control" Placeholder="Option A" />
                </div>
                <asp:RequiredFieldValidator ID="rfvA" runat="server"
                    ControlToValidate="txtChoiceA"
                    ErrorMessage="Option A is required."
                    CssClass="text-danger" Display="Dynamic"
                    ValidationGroup="Q" />
              </div>

              <!-- B -->
              <div class="col-md-6">
                <div class="d-flex gap-2 align-items-center">
                  <asp:RadioButton ID="radiobtnChoice2" runat="server" GroupName="mcqCorrect" />
                  <asp:TextBox ID="txtChoiceB" runat="server" CssClass="form-control" Placeholder="Option B" />
                </div>
                <asp:RequiredFieldValidator ID="rfvB" runat="server"
                    ControlToValidate="txtChoiceB"
                    ErrorMessage="Option B is required."
                    CssClass="text-danger" Display="Dynamic"
                    ValidationGroup="Q" />
              </div>

              <!-- C -->
              <div class="col-md-6">
                <div class="d-flex gap-2 align-items-center">
                  <asp:RadioButton ID="radiobtnChoice3" runat="server" GroupName="mcqCorrect" />
                  <asp:TextBox ID="txtChoiceC" runat="server" CssClass="form-control" Placeholder="Option C" />
                </div>
                <asp:RequiredFieldValidator ID="rfvC" runat="server"
                    ControlToValidate="txtChoiceC"
                    ErrorMessage="Option C is required."
                    CssClass="text-danger" Display="Dynamic"
                    ValidationGroup="Q" />
              </div>

              <!-- D -->
              <div class="col-md-6">
                <div class="d-flex gap-2 align-items-center">
                  <asp:RadioButton ID="radiobtnChoice4" runat="server" GroupName="mcqCorrect" />
                  <asp:TextBox ID="txtChoiceD" runat="server" CssClass="form-control" Placeholder="Option D" />
                </div>
                <asp:RequiredFieldValidator ID="rfvD" runat="server"
                    ControlToValidate="txtChoiceD"
                    ErrorMessage="Option D is required."
                    CssClass="text-danger" Display="Dynamic"
                    ValidationGroup="Q" />
              </div>
            </div>

            <!-- require one radio selected -->
            <asp:CustomValidator ID="cvMcqCorrect" runat="server"
                ErrorMessage="Please choose the correct MCQ option."
                CssClass="text-danger" Display="Dynamic"
                ClientValidationFunction="validateMcq"
                ValidationGroup="Q" />
            <script type="text/javascript">
                function validateMcq(sender, args) {
                    var r1 = document.getElementById('<%= radiobtnChoice1.ClientID %>');
                  var r2 = document.getElementById('<%= radiobtnChoice2.ClientID %>');
                var r3 = document.getElementById('<%= radiobtnChoice3.ClientID %>');
                var r4 = document.getElementById('<%= radiobtnChoice4.ClientID %>');
                    args.IsValid = (r1 && r1.checked) || (r2 && r2.checked) || (r3 && r3.checked) || (r4 && r4.checked);
                }
            </script>

            <small class="text-muted d-block mt-2">Select the radio next to the correct option.</small>
          </asp:Panel>

          <!-- True/False -->
          <asp:Panel ID="panelTF" runat="server" Visible="false" CssClass="mb-3">
            <label class="form-label d-block">Correct Answer:</label>
            <asp:RadioButtonList ID="radiobtnTF" runat="server" CssClass="form-check" RepeatDirection="Horizontal">
              <asp:ListItem Text="True"  Value="True"></asp:ListItem>
              <asp:ListItem Text="False" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvTF" runat="server"
                ControlToValidate="radiobtnTF"
                InitialValue=""
                ErrorMessage="Please choose True or False."
                CssClass="text-danger" Display="Dynamic"
                ValidationGroup="Q" />
          </asp:Panel>

          <!-- Fill in the Blanks -->
          <asp:Panel ID="panelFill" runat="server" Visible="false" CssClass="mb-3">
            <asp:Label runat="server" AssociatedControlID="txtFillInBlank" CssClass="form-label" Text="Answer for Blank:" />
            <asp:TextBox ID="txtFillInBlank" runat="server" CssClass="form-control" Placeholder="Correct answer" />
            <asp:RequiredFieldValidator ID="rfvFill" runat="server"
                ControlToValidate="txtFillInBlank"
                ErrorMessage="Please enter the fill-in answer."
                CssClass="text-danger" Display="Dynamic"
                ValidationGroup="Q" />
          </asp:Panel>

          <!-- Explanation -->
          <div class="mb-3">
            <asp:Label runat="server" AssociatedControlID="txtExplanation" CssClass="form-label" Text="Explanation:" />
            <asp:TextBox ID="txtExplanation" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
            <asp:RequiredFieldValidator ID="rfvExplanation" runat="server"
                ControlToValidate="txtExplanation"
                ErrorMessage="Explanation is required."
                CssClass="text-danger" Display="Dynamic"
                ValidationGroup="Q" />
          </div>

          <!-- Actions -->
          <div class="d-flex justify-content-center gap-3">
            <asp:Button ID="btnSave" runat="server" Text="Save and continue"
                CssClass="btn btn-primary"
                OnClick="btnSave_Click"
                CausesValidation="true" ValidationGroup="Q" />
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