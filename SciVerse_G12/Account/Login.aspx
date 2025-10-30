<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SciVerse_G12.Login" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
     <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/site2.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <!-- Bootstrap Icon -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-center align-items-center login-container">
        <div class="card p-5 shadow-lg" style="width: 380px; border-radius: 12px;">
            
            <h1 class="text-center mb-3">Welcome!</h1>
            <p class="text-center mb-4"><strong>Login below or create an account</strong></p>

            <!-- Username -->
            <div class="mb-3">
                <label for="txtUsername" class="form-label">Username / Email:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <!-- Password -->
            <div class="mb-3">
                <label for="txtPassword" class="form-label">Password:</label>

                <div class="input-group">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"
                        AutoComplete="new-password"></asp:TextBox>

                    <span class="input-group-text" id="togglePassword" style="cursor:pointer;">
                        <i class="bi bi-eye-slash"></i>
                    </span>
                </div>
            </div>

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mb-3 text-center"></asp:Label>

            <!-- Login Button -->
            <div class="d-grid mb-3">
                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" CssClass="btn btn-primary" />
            </div>

            <!-- Links -->
            <div class="text-center">
                <a href="~/Account/ForgotPassword.aspx" runat="server" class="text-decoration-underline" style="color:#003399;">Forgot Password?</a>
                <span class="mx-3">|</span>
                <a href="~/Account/Register.aspx" runat="server" class="text-decoration-underline">Sign Up</a>
            </div>
        </div>
    </div>

    <script>
        document.getElementById("togglePassword").addEventListener("click", function () {
            var passwordField = document.getElementById("<%= txtPassword.ClientID %>");
            var icon = this.querySelector("i");

            if (passwordField.type === "password") {
                passwordField.type = "text";
                icon.classList.remove("bi-eye-slash");
                icon.classList.add("bi-eye");
            } else {
                passwordField.type = "password";
                icon.classList.remove("bi-eye");
                icon.classList.add("bi-eye-slash");
            }
        });
    </script>


</asp:Content>
