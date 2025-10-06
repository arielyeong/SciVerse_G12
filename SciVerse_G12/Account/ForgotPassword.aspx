<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SciVerse_G12.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="d-flex justify-content-center align-items-center login-container">
        <div class="card p-4 shadow-lg" style="max-width:700px; width:100%; border-radius:10px;">

            <!-- Title -->
            <h1 class="text-center mb-3">Forgot Your Password?</h1>
            <p class="text-center mb-4"><strong>Enter your username and new password</strong></p>

            <!-- Username -->
            <div class="mb-3">
                <label for="txtUsername" class="form-label">Username:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="* Username is required" CssClass="text-danger" />
            </div>

            <!-- New Password -->
            <div class="mb-3">
                <label for="txtPassword" class="form-label">New Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="* Password is required" CssClass="text-danger" />
            </div>

            <!-- Confirm Password -->
            <div class="mb-3">
                <label for="txtConfirmPassword" class="form-label">Confirm Password:</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="* Confirm password is required" CssClass="text-danger" />
                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="* Passwords do not match" CssClass="text-danger" />
            </div>

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mb-3 text-center"></asp:Label>

            <!-- Reset Button -->
            <div class="d-grid mb-3">
                <asp:Button ID="btnReset" runat="server" Text="Reset Password" CssClass="btn btn-primary" OnClick="btnReset_Click" />
            </div>

            <!-- Links -->
            <div class="text-center">
                <a href="~/Account/Login.aspx" runat="server" class="text-decoration-underline" style="color:#003399;">Back to Login</a>
                <span class="mx-3">|</span>
                <a href="~/Account/Register.aspx" runat="server" class="text-decoration-underline">Sign Up</a>
            </div>
        </div>
    </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
            DeleteCommand="DELETE FROM [tblRegisteredUsers] WHERE [RID] = @RID" 
            InsertCommand="INSERT INTO [tblRegisteredUsers] ([fullName], [emailAddress], [username], [password], [age], [gender], [country], [picture], [dateRegister]) VALUES (@fullName, @emailAddress, @username, @password, @age, @gender, @country, @picture, @dateRegister)" 
            SelectCommand="SELECT * FROM [tblRegisteredUsers]" 
            UpdateCommand="UPDATE [tblRegisteredUsers] SET [fullName] = @fullName, [emailAddress] = @emailAddress, [username] = @username, [password] = @password, [age] = @age, [gender] = @gender, [country] = @country, [picture] = @picture, [dateRegister] = @dateRegister WHERE [RID] = @RID">
            <DeleteParameters>
                <asp:Parameter Name="RID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="fullName" Type="String" />
                <asp:Parameter Name="emailAddress" Type="String" />
                <asp:Parameter Name="username" Type="String" />
                <asp:Parameter Name="password" Type="String" />
                <asp:Parameter Name="age" Type="Int32" />
                <asp:Parameter Name="gender" Type="String" />
                <asp:Parameter Name="country" Type="String" />
                <asp:Parameter Name="picture" Type="String" />
                <asp:Parameter Name="dateRegister" Type="DateTime" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="fullName" Type="String" />
                <asp:Parameter Name="emailAddress" Type="String" />
                <asp:Parameter Name="username" Type="String" />
                <asp:Parameter Name="password" Type="String" />
                <asp:Parameter Name="age" Type="Int32" />
                <asp:Parameter Name="gender" Type="String" />
                <asp:Parameter Name="country" Type="String" />
                <asp:Parameter Name="picture" Type="String" />
                <asp:Parameter Name="dateRegister" Type="DateTime" />
                <asp:Parameter Name="RID" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>

</asp:Content>

