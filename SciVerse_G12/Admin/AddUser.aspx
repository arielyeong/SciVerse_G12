<%@ Page Title="Add User" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="SciVerse_G12.AddUser" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap & FontAwesome Links -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/Admin.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="add-user-container">
        <div class="add-user-card">
            <h1 class="add-user-header text-center mb-4">Add New User</h1>

            <!-- PERSONAL DETAILS -->
            <h4 class="section-title">Personal Details</h4>
            <div class="row mb-4 add-user-form">
                <div class="col-md-7">
                    <div class="mb-3">
                        <label class="form-label">Full Name</label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Age</label>
                        <asp:TextBox ID="txtAge" runat="server" TextMode="Number" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Gender</label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select" Required="true">
                            <asp:ListItem Text="Select Gender" Value=""></asp:ListItem>
                            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Country</label>
                        <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-5 text-center image-preview-box">
                    <img id="imgPreview" src="../Images/default_profile.png" class="img-preview" alt="Profile Preview" />
                    <div class="mt-2">
                        <asp:FileUpload ID="fileUploadPicture" runat="server" CssClass="form-control" accept="image/*" onchange="showPreview(event)" />
                        <asp:RequiredFieldValidator runat="server" 
                                                    ControlToValidate="fileUploadPicture" 
                                                    CssClass="text-danger" 
                                                    ErrorMessage="Profile image is required." 
                                                    Display="Dynamic" 
                                                    InitialValue="" />
                        <asp:RegularExpressionValidator runat="server" 
                                                        ControlToValidate="fileUploadPicture" 
                                                        CssClass="text-danger" 
                                                        ErrorMessage="Only image files (JPG, PNG, GIF) are allowed." 
                                                        Display="Dynamic" 
                                                        ValidationExpression="^.+(\.jpg|\.jpeg|\.png|\.gif|\.bmp)$" />
                    </div>
                </div>
            </div>

            <!-- ACCOUNT DETAILS (Admin-only: No password) -->
            <h4 class="section-title">Account Details</h4>
            <div class="row add-user-form">
                <div class="col-md-6">
                    <label class="form-label">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                </div>
                <div class="col-md-6">
                    <label class="form-label">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" Required="true"></asp:TextBox>
                </div>
            </div>
            <div class="row add-user-form"> <!-- New row for password -->
                <div class="col-md-6 offset-md-3"> <!-- Centered single field -->
                    <label class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Required="true"></asp:TextBox>
                    <asp:RegularExpressionValidator runat="server" 
                                                    ControlToValidate="txtPassword" 
                                                    CssClass="text-danger" 
                                                    ErrorMessage="Password must be at least 4 characters." 
                                                    Display="Dynamic" 
                                                    ValidationExpression="^.{4,}$" />

            <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>

            <div class="text-center mt-4">
                <asp:Button ID="btnSave" runat="server" CssClass="btn-save-user" Text="Save User" OnClick="btnSave_Click" />
            </div>

            <div class="text-center mt-3">
                <a href="ViewUserList.aspx" class="btn-back">Back to User List</a>
            </div>
        </div>
    </div>

    <script>
        function showPreview(event) {
            const preview = document.getElementById("imgPreview");
            if (event.target.files && event.target.files[0]) {
                preview.src = URL.createObjectURL(event.target.files[0]);
            }
        }
    </script>
</asp:Content>