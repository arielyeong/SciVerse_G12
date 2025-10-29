<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SciVerse_G12.Register" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/site2.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-center align-items-center register-container">
        <div class="card shadow-lg">

            <h1 class="profile-header text-center mb-4">Create Your Account</h1>

            <!-- PERSONAL DETAILS -->
            <h4 class="section-title">Personal Details</h4>
            <div class="row mb-4">
                <div class="col-md-7">

                    <div class="mb-3">
                        <label class="form-label">Full Name</label>
                        <asp:TextBox ID="txtFullname" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Age</label>
                        <asp:TextBox ID="txtAge" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Gender</label><br />
                        <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" CssClass="form-check-inline">
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Country</label>
                        <asp:DropDownList ID="dlCountry" runat="server" CssClass="form-control">
                               
                        </asp:DropDownList>
                    </div>

                </div>

                <!-- Profile Picture Preview -->
                <div class="col-md-5 text-center image-preview-box">
                    <img id="imgPreview" src="../Images/default_profile.png" class="img-preview" />
                    <div class="mt-2">
                        <asp:FileUpload ID="FileUploadPic" runat="server" CssClass="form-control" accept="image/*" onchange="showPreview(event)" />
                        <asp:Label ID="lblPicture" runat="server" CssClass="text-danger"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- ACCOUNT DETAILS -->
            <h4 class="section-title">Account Details</h4>
            <div class="row">
                <div class="col-md-6">

                    <div class="mb-3">
                        <label class="form-label">Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                </div>
                <div class="col-md-6">

                    <div class="mb-3">
                        <label class="form-label">Email Address</label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                    </div>

                </div>
            </div>

            <div class="row">
                <div class="col-md-6">

                    <div class="mb-3">
                        <label class="form-label">Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                    </div>

                </div>
                <div class="col-md-6">

                    <div class="mb-3">
                        <label class="form-label">Confirm Password</label>
                        <asp:TextBox ID="txtConPass" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                    </div>

                </div>
            </div>


            <div class="text-center mb-3">
                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn-register" OnClick="btnRegister_Click" />
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-center text-danger"></asp:Label>

            <div class="text-center mt-3">
                <span>Already have an account?</span>
                <a href="~/Account/Login.aspx" runat="server" class="text-decoration-underline" style="color:#003399;">Login</a>
            </div>

        </div>
    </div>


    <script>
        function showPreview(event) {
            const preview = document.getElementById("imgPreview");
            preview.src = URL.createObjectURL(event.target.files[0]);
        }
    </script>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        InsertCommand="INSERT INTO [tblRegisteredUsers] ([fullName], [emailAddress], [username], [password], [age], [gender], [country], [picture], [dateRegister], [role]) VALUES (@fullName, @emailAddress, @username, @password, @age, @gender, @country, @picture, @dateRegister, @role)">
        <InsertParameters>
            <asp:ControlParameter Name="fullName" ControlID="txtFullname" PropertyName="Text" />
            <asp:ControlParameter Name="emailAddress" ControlID="txtEmail" PropertyName="Text" />
            <asp:ControlParameter Name="username" ControlID="txtUsername" PropertyName="Text" />
            <asp:ControlParameter Name="password" ControlID="txtPassword" PropertyName="Text" />
            <asp:ControlParameter Name="age" ControlID="txtAge" PropertyName="Text" />
            <asp:ControlParameter Name="gender" ControlID="rblGender" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="country" ControlID="dlCountry" PropertyName="SelectedValue" />
            <asp:Parameter Name="picture" />
            <asp:Parameter Name="dateRegister" Type="DateTime" />
            <asp:Parameter Name="role" DefaultValue="User" />
        </InsertParameters>

    </asp:SqlDataSource>

</asp:Content>
