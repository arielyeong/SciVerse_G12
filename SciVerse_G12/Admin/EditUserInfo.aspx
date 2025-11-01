<%@ Page Title="Edit User Info" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditUserInfo.aspx.cs" Inherits="SciVerse_G12.EditUserInfo" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/Profile.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <style>
        .update-wrapper {
            margin-top: -50px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="update-wrapper">
        <div class="update-container">
            <div class="update-header">Edit User Profile</div>

            <div class="update-content">
                <div class="update-layout">
                    <!-- Left: Profile Picture -->
                    <div class="update-picture-section text-center">
                        <asp:Image ID="imgPreview" runat="server" CssClass="update-picture" Width="150px" Height="150px" />
                        <div class="mt-3">
                            <label for="FileUploadPic" class="form-label">Change Picture</label>
                            <asp:FileUpload ID="FileUploadPic" runat="server" CssClass="form-control text-start" />
                        </div>
                    </div>

                    <!-- Right: Form Details -->
                    <div class="update-details">
                        <div class="update-card">
                            <div class="update-item">
                                <span class="update-label">Full Name:</span>
                                <asp:TextBox ID="txtFullname" runat="server" CssClass="form-control update-value"></asp:TextBox>
                            </div>

                            <div class="update-item">
                                <span class="update-label">Email:</span>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control update-value"></asp:TextBox>
                            </div>

                            <div class="update-item">
                                <span class="update-label">Username:</span>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control update-value"></asp:TextBox>
                            </div>

                            <div class="update-item">
                                <span class="update-label">Age:</span>
                                <asp:TextBox ID="txtAge" runat="server" CssClass="form-control update-value age-input"></asp:TextBox>
                            </div>

                            <div class="update-item gender-item">
                                <span class="update-label">Gender:</span>
                                <asp:RadioButtonList ID="rbGender" runat="server" RepeatDirection="Horizontal" CssClass="form-check-inline">
                                    <asp:ListItem>Male</asp:ListItem>
                                    <asp:ListItem>Female</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                            <div class="update-item">
                                <span class="update-label">Country:</span>
                                <asp:DropDownList ID="dlCountry" runat="server" CssClass="form-select update-value">
                                    <asp:ListItem Value="">-- Select Country --</asp:ListItem>
                                    <asp:ListItem>Malaysia</asp:ListItem>
                                    <asp:ListItem>Singapore</asp:ListItem>
                                    <asp:ListItem>China</asp:ListItem>
                                    <asp:ListItem>Thailand</asp:ListItem>
                                    <asp:ListItem>New Zealand</asp:ListItem>
                                    <asp:ListItem>Canada</asp:ListItem>
                                    <asp:ListItem>United Kingdom</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="update-button-container text-center">
                <asp:Button ID="btnSubmit" runat="server" Text="Save Changes" CssClass="btn-update" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="update-message text-center fw-bold mt-3"></asp:Label>
        </div>
    </div>

    <!-- Image Preview Script -->
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const fileUpload = document.getElementById('<%= FileUploadPic.ClientID %>');
            const imgPreview = document.getElementById('<%= imgPreview.ClientID %>');

            fileUpload.addEventListener("change", function (e) {
                const file = e.target.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = function (evt) {
                        imgPreview.src = evt.target.result;
                    };
                    reader.readAsDataURL(file);
                }
            });
        });
    </script>

    <!-- SqlDataSource -->
    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT * FROM [tblRegisteredUsers] WHERE [username] = @username"
        UpdateCommand="UPDATE [tblRegisteredUsers] 
                        SET [fullName] = @fullName, 
                            [emailAddress] = @emailAddress, 
                            [age] = @age, 
                            [gender] = @gender, 
                            [country] = @country, 
                            [picture] = @picture
                        WHERE [username] = @username">

        <SelectParameters>
            <asp:SessionParameter Name="username" SessionField="Username" Type="String" />
        </SelectParameters>

        <UpdateParameters>
            <asp:ControlParameter Name="fullName" ControlID="txtFullname" PropertyName="Text" />
            <asp:ControlParameter Name="emailAddress" ControlID="txtEmail" PropertyName="Text" />
            <asp:ControlParameter Name="age" ControlID="txtAge" PropertyName="Text" />
            <asp:ControlParameter Name="gender" ControlID="rbGender" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="country" ControlID="dlCountry" PropertyName="SelectedValue" />
            <asp:Parameter Name="picture" Type="String" />
            <asp:ControlParameter Name="username" ControlID="txtUsername" PropertyName="Text" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
