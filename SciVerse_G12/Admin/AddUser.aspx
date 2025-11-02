<%@ Page Title="Add User" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="SciVerse_G12.AddUser" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap & FontAwesome Links -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/Admin.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-content">
        <div class="content-header">
            <h1 class="page-title">
                <i class="fa fa-user-plus me-2"></i>Add New User
            </h1>
            <p class="page-subtitle">Fill the form below to add a new user.</p>
        </div>

        <!-- User Form -->
        <div class="user-form">
            <div class="mb-3">
                <label for="txtUsername" class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="txtFullName" class="form-label">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Required="true" TextMode="Email"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="txtAge" class="form-label">Age</label>
                <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="ddlGender" class="form-label">Gender</label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select" Required="true">
                    <asp:ListItem Text="Select Gender" Value=""></asp:ListItem>
                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="mb-3">
                <label for="txtCountry" class="form-label">Country</label>
                <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="fileUploadPicture" class="form-label">Profile Picture</label>
                <asp:FileUpload ID="fileUploadPicture" runat="server" CssClass="form-control" />
            </div>

            <div class="d-flex justify-content-between">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save User" OnClick="btnSave_Click" />
                <a href="ViewUserList.aspx" class="btn btn-secondary">Back to User List</a>
            </div>
        </div>
    </div>
</asp:Content>