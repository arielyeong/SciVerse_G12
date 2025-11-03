<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SciVerse_G12.ForgotPassword" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/site2.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-center align-items-center forgot-container">
        <div class="card p-5 shadow-lg" style="width: 420px; border-radius: 12px;">
            
            <!-- Title -->
            <h1 class="text-center mb-3" style="font-size:1.9rem; font-weight:600; color:#0d47a1;">Forgot Your Password?</h1>
            <p class="text-center mb-4"><strong>Enter your username, email, and new password</strong></p>

            <!-- Username -->
            <div class="mb-3">
                <label for="txtUsername" class="form-label">Username:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" 
                    ErrorMessage="* Username is required" CssClass="text-danger small" Display="Dynamic" />
            </div>

            <!-- Email -->
            <div class="mb-3">
                <label for="txtEmail" class="form-label">Email Address:</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your email" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                    ErrorMessage="* Email is required" CssClass="text-danger small" Display="Dynamic" />
            </div>

            <!-- New Password -->
            <div class="mb-3">
                <label for="txtPassword" class="form-label">New Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" 
                    placeholder="Enter new password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                    ErrorMessage="* Password is required" CssClass="text-danger small" Display="Dynamic" />
            </div>

            <!-- Confirm Password -->
            <div class="mb-3">
                <label for="txtConfirmPassword" class="form-label">Confirm Password:</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" 
                    placeholder="Confirm new password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" 
                    ErrorMessage="* Confirm password is required" CssClass="text-danger small" Display="Dynamic" />
                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" 
                    ControlToCompare="txtPassword" ErrorMessage="* Passwords do not match" CssClass="text-danger small" Display="Dynamic" />
            </div>

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mb-3 text-center message" EnableViewState="false"></asp:Label>

            <!-- Reset Button -->
            <div class="d-grid mb-3">
                <asp:Button ID="btnReset" runat="server" Text="Reset Password" CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="true" />
            </div>

            <!-- Links -->
            <div class="text-center">
                <a href="~/Account/Login.aspx" runat="server" class="text-decoration-underline" style="color:#003399;">Back to Login</a>
                <span class="mx-3">|</span>
                <a href="~/Account/Register.aspx" runat="server" class="text-decoration-underline">Sign Up</a>
            </div>
        </div>
    </div>

</asp:Content>