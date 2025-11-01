<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ViewUserList.aspx.cs" Inherits="SciVerse_G12.ViewUserList" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/Admin.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />

    <!-- FOOTER FIX STYLES -->
    <style>
        .admin-content {
            position: relative;
        }
       
        
        /* Ensure the main content wrapper pushes footer down */
        #contentArea {
            flex: 1;
        }
        
        /* Add padding to bottom of content */
        .container.body-content {
            padding-bottom: 60px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Main Content -->
    <div class="admin-content" id="contentArea">
        <!-- Redesigned Header: Bold with icon and subtle underline -->
        <div class="content-header">
            <h1 class="page-title">
                <i class="fa fa-users me-2"></i>View User List
            </h1>
            <p class="page-subtitle">Manage and monitor registered users</p>
        </div>

        <!-- Redesigned Top Controls: Compact row with icon search button -->
        <div class="top-controls">
            <!-- Left: Search Input + Icon Button -->
            <div class="search-section">
                <div class="input-group input-group-sm" style="width: 300px;">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control search-input" Placeholder="Search..."></asp:TextBox>
                    <button type="submit" runat="server" id="btnSearch" class="btn-search-icon" onserverclick="btnSearch_Click"></button>
                    <button type="submit" runat="server" id="btnClear" class="btn-clear-icon ms-1" onserverclick="btnClear_Click"></button>

                </div>
            </div>

            <!-- Right: Custom Mode Buttons (No Bootstrap colors - custom soft palette) -->
            <div class="mode-buttons">
                <asp:Button ID="btnEditMode" runat="server" CssClass="btn-custom btn-edit-mode me-2" Text="Edit Mode" OnClick="btnEditMode_Click" ToolTip="Enable edit selection" />
                <asp:Button ID="btnDeleteMode" runat="server" CssClass="btn-custom btn-delete-mode me-2" Text="Delete Mode" OnClick="btnDeleteMode_Click" ToolTip="Enable delete selection" />
                <asp:Button ID="btnConfirm" runat="server" CssClass="btn-custom btn-confirm me-2" Text="Confirm" Visible="false" OnClick="btnConfirm_Click" ToolTip="Confirm selected actions" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn-custom btn-cancel" Text="Cancel" Visible="false" OnClick="btnCancel_Click" ToolTip="Cancel mode" />
            </div>
        </div>

        <!-- Responsive Table Wrapper -->
        <div class="table-responsive">
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                SelectCommand="SELECT * FROM [tblRegisteredUsers]">
            </asp:SqlDataSource>

            <asp:GridView ID="GridView1" runat="server" 
                AllowPaging="True" PageSize="10"
                AutoGenerateColumns="False"
                DataKeyNames="RID" 
                DataSourceID="SqlDataSource1"
                OnRowDataBound="GridView1_RowDataBound"
                OnPageIndexChanging="GridView1_PageIndexChanging"
                CssClass="user-table table table-striped table-hover"
                EmptyDataText="No users found matching your criteria."
                EnableViewState="true">


                <Columns>
                    <asp:TemplateField HeaderText="Select" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                        <ItemStyle CssClass="select-col" />
                        <HeaderStyle CssClass="select-col" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="RID" HeaderText="ID" HeaderStyle-CssClass="id-col">
                        <ItemStyle CssClass="id-col text-center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="username" HeaderText="Username" HeaderStyle-CssClass="username-col">
                        <ItemStyle CssClass="username-col" />
                    </asp:BoundField>

                    <asp:BoundField DataField="fullName" HeaderText="Full Name" HeaderStyle-CssClass="name-col">
                        <ItemStyle CssClass="name-col text-truncate" />
                    </asp:BoundField>

                    <asp:BoundField DataField="emailAddress" HeaderText="Email" HeaderStyle-CssClass="email-col">
                        <ItemStyle CssClass="email-col text-truncate" />
                    </asp:BoundField>

                    <asp:BoundField DataField="age" HeaderText="Age" HeaderStyle-CssClass="age-col">
                        <ItemStyle CssClass="age-col text-center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="gender" HeaderText="Gender" HeaderStyle-CssClass="gender-col">
                        <ItemStyle CssClass="gender-col text-center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="country" HeaderText="Country" HeaderStyle-CssClass="country-col">
                        <ItemStyle CssClass="country-col text-truncate" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Picture" HeaderStyle-CssClass="picture-col">
                        <ItemTemplate>
                            <asp:Image ID="imgUser" runat="server" ImageUrl='<%# Eval("picture") ?? "~/Images/Profile/default.png" %>' CssClass="user-avatar" AlternateText="Profile Picture" ToolTip="Profile Picture" OnError="imgUser_Error" />
                        </ItemTemplate>
                        <ItemStyle CssClass="picture-col text-center" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="dateRegister" HeaderText="Joined" HeaderStyle-CssClass="date-col" DataFormatString="{0:MMM dd, yyyy}" HtmlEncode="false">
                        <ItemStyle CssClass="date-col text-center" />
                    </asp:BoundField>
                </Columns>

                <EmptyDataRowStyle CssClass="text-center text-muted py-4" />
                <PagerStyle CssClass="d-flex justify-content-center" />
                <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
            </asp:GridView>
        </div>

        <!-- Error Label -->
        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger mt-3" Visible="false"></asp:Label>

        <!-- No-Results Message (Handled by EmptyDataText, but enhanced via CSS) -->
        <div id="noResults" class="no-results text-center py-5 text-muted" runat="server" visible="false">
            <i class="fa fa-search fa-3x mb-3"></i>
            <h4>No Users Found</h4>
            <p>Try adjusting your search criteria.</p>
        </div>
    </div>
</asp:Content>