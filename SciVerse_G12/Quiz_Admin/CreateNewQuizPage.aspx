<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="CreateNewQuizPage.aspx.cs" Inherits="SciVerse_G12.Quiz_Admin.CreateNewQuizPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <!-- Bootstrap -->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

        <!-- Page-specific CSS -->
        <link href='<%= ResolveUrl("~/Styles/QuizAdmin.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <style>
        /* ================================
           QUIZ CREATION CARD STYLING
        ================================= */
        .quiz-card {
            max-width: 1000px;
            width: 100%;
            background-color: #fffdf8;   /* very light warm white */
            border: 1px solid #e8dcc7;
            border-radius: 16px;
        }

        .quiz-card h1 {
            font-weight: 700;
            letter-spacing: 0.02em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height:90vh;">
    <div class="card p-4 shadow-lg" style="max-width:1000px; width:100%; border-radius:10px;">
      <h1 class="text-center mb-4">Create New Quiz</h1>

      <div class="row g-4">
        <div class="col-12">

          <!-- Title -->
          <div class="mb-3">
            <asp:Label ID="lblQuizTitle" runat="server" Text="Quiz Title:" CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtQuizTitle" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuizTitle"
                ErrorMessage="Title is required" ForeColor="Red" Display="Dynamic" />
          </div>

          <!-- Description -->
          <div class="mb-3">
            <asp:Label ID="lblDescription" runat="server" Text="Description:" CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescription"
                ErrorMessage="Description is required" ForeColor="Red" Display="Dynamic" />
          </div>

          <!-- Chapter / Time / Attempt -->
          <div class="mb-3">
            <div class="d-flex justify-content-between gap-3">
              <div class="col-md-4">
                <asp:Label ID="lblChapter" runat="server" Text="Chapter:" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtChapter" runat="server" CssClass="form-control" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtChapter"
                    ErrorMessage="Chapter is required" ForeColor="Red" Display="Dynamic" />
              </div>

              <div class="col-md-4">
                <asp:Label ID="lblTimeLimit" runat="server" Text="Time Limit (minutes):" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtTimeLimit" runat="server" CssClass="form-control" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTimeLimit"
                    ErrorMessage="Time Limit is required" ForeColor="Red" Display="Dynamic" />
              </div>

              <div class="col-md-3">
                <asp:Label ID="lblAttemptLimit" runat="server" Text="Attempt Limit:" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtAttemptLimit" runat="server" CssClass="form-control" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAttemptLimit"
                    ErrorMessage="Attempt Limit is required" ForeColor="Red" Display="Dynamic" />
              </div>
            </div>
          </div>

          <!-- Picture -->
          <div class="mb-3">
            <asp:Label ID="lblPicture" runat="server" Text="Picture:" CssClass="form-label"></asp:Label>
            <asp:FileUpload ID="FileUploadPicture" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="FileUploadPicture"
                ErrorMessage="Picture is required" ForeColor="Red" Display="Dynamic" />
          </div>

          <!-- Actions -->
          <div class="d-flex justify-content-center gap-4 mt-4">
            <asp:Button ID="btnSaveAndContinue" runat="server" Text="Save and Continue"
                CssClass="btn btn-primary" OnClick="btnSaveAndContinue_Click" />
            <asp:HyperLink ID="lnkBack" runat="server" CssClass="btn btn-secondary"
                NavigateUrl="~/Quiz_Admin/ViewQuizList.aspx" Text="Back to List" />
          </div>

          <div class="text-center mt-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold"></asp:Label>
          </div>

        </div>
      </div>
    </div>
  </div>

</asp:Content>