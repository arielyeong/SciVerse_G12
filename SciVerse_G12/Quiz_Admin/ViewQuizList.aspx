<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true"
    CodeBehind="ViewQuizList.aspx.cs" Inherits="SciVerse_G12.Quiz.ViewQuizList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        h1 {
          font-size: 1.8rem;
        }

        .form-control {
          min-width: 600px; /* wide search bar on large screens */
        }

        /* Responsive styles */
        @media (max-width: 992px) {
          /* For tablets or medium screens */
          .form-control {
            min-width: 100%;
          }

          .row.g-2.align-items-center.mb-3.text-start {
            flex-wrap: wrap;
          }

          .col-lg-8,
          .col-lg-4 {
            width: 100%;
            text-align: center;
            justify-content: center !important;
          }

          .col-lg-4 .btn {
            width: 100%;
            margin-bottom: 5px;
          }

          .table {
            font-size: 0.9rem;
          }
        }

        @media (max-width: 768px) {
          /* For mobile phones */
          h1 {
            font-size: 1.4rem;
            text-align: center;
          }

          .form-control {
            min-width: 100%;
            font-size: 0.9rem;
          }

          .btn {
            width: 100%;
            margin-top: 6px;
          }

          .table {
            font-size: 0.85rem;
          }

          /* Stack buttons vertically */
          .col-lg-4 {
            flex-direction: column;
            align-items: stretch !important;
          }
        }  
    </style>
    <h1>View Quiz List</h1>
    <br />

    <!-- Controls Row -->
    <div class="container-fluid px-0">
      <div class="row g-2 align-items-center mb-3 text-start">
          <!-- LEFT: Search + Clear -->
          <div class="col-lg-8 d-flex align-items-center gap-2">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
              Style="flex:1 1 auto; min-width:600px;"
              Placeholder="Search by Title / Chapter / Description" />
            <asp:Button ID="btnSearch" runat="server" Text="Search"
              CssClass="btn btn-primary" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear"
              CssClass="btn btn-secondary" OnClick="btnClear_Click" />
          </div>

          <!-- RIGHT: Edit/Delete/Confirm/Cancel/Add -->
          <div class="col-lg-4 d-flex justify-content-end align-items-center gap-2">
            <asp:Button ID="btnEditMode" runat="server" Text="Edit mode"
              CssClass="btn btn-warning" OnClick="btnEditMode_Click" />
            <asp:Button ID="btnDeleteMode" runat="server" Text="Delete mode"
              CssClass="btn btn-danger" OnClick="btnDeleteMode_Click" />
            <asp:Button ID="btnConfirm" runat="server" Text="Confirm"
              CssClass="btn btn-success" OnClick="btnConfirm_Click" Visible="false" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel"
              CssClass="btn btn-secondary" OnClick="btnCancel_Click" Visible="false" />
            <asp:Button ID="btnAdd" runat="server" Text="Add New Quiz"
              CssClass="btn btn-success" OnClick="btnAdd_Click" />
          </div>
        </div>
    </div>

    <!-- Data source -->
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT QuizID, Title, Description, Chapter, TimeLimit, ImageURL, CreatedDate, CreatedBy, AttemptLimit
                       FROM dbo.tblQuiz
                       ORDER BY CreatedDate DESC, QuizID DESC"
        UpdateCommand="
            UPDATE dbo.tblQuiz
            SET Title=@Title, Description=@Description, Chapter=@Chapter, TimeLimit=@TimeLimit,
                ImageURL=@ImageURL, AttemptLimit=@AttemptLimit
            WHERE QuizID=@QuizID"
        DeleteCommand="
            DELETE FROM dbo.tblQuiz
            WHERE QuizID=@QuizID">
    </asp:SqlDataSource>

    <!-- Grid -->
    <asp:GridView ID="GridView1" runat="server"
        AllowPaging="True" PageSize="10"
        AutoGenerateColumns="False"
        DataKeyNames="QuizID"
        DataSourceID="SqlDataSource1"
        OnPageIndexChanging="GridView1_PageIndexChanging"
        OnRowDataBound="GridView1_RowDataBound"
        CssClass="table table-bordered table-hover">

        <Columns>
            <asp:TemplateField HeaderText="Select" Visible="false">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" />
                </ItemTemplate>
                <ItemStyle Width="60px" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="QuizID" HeaderText="Quiz ID" ReadOnly="True" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Chapter" HeaderText="Chapter" />
            <asp:BoundField DataField="TimeLimit" HeaderText="Time (min)" />
            <asp:ImageField DataImageUrlField="ImageURL" HeaderText="Image">
                <ControlStyle Width="80px" Height="80px" />
            </asp:ImageField>
            <asp:BoundField DataField="CreatedDate" HeaderText="Created" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
            <asp:BoundField DataField="AttemptLimit" HeaderText="Attempts" />
        </Columns>

        <EmptyDataTemplate>
            <div class="alert alert-info" style="margin:8px 0;">
                No quizzes found.
            </div>
        </EmptyDataTemplate>
    </asp:GridView>

    <!-- Delete confirmation modal -->
    <div class="modal fade" id="deleteModal" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Confirm Deletion</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            Are you sure you want to delete the selected quiz(es)? This action cannot be undone.
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancel</button>
            <asp:Button ID="btnConfirmDeleteYes" runat="server" Text="Yes, Delete"
                        CssClass="btn btn-danger"
                        OnClick="btnConfirmDeleteYes_Click" />
          </div>
        </div>
      </div>
    </div>

    <script type="text/javascript">
      function openDeleteModal() {
        var modalEl = document.getElementById('deleteModal');
        if (!modalEl) return;
        var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
      }
    </script>
</asp:Content>

