<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="SciVerse_G12.AdminProfile" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
     <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Page-specific CSS -->
<%--    <link href="<%= ResolveUrl("~/Styles/Home.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href='<%= ResolveUrl("~/Styles/Profile.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .profile-wrapper {
            margin-top: -50px !important;
        }
     </style>

<div class="profile-wrapper">
    <div class="profile-container">
        <div class="profile-header">
            Your Profile
        </div>

        <div class="profile-content">
            <div class="profile-layout">
                <div class="profile-picture-section">
                    <asp:Image ID="imgProfile" runat="server" CssClass="profile-picture" />
                </div>

                <div class="profile-details">
                    <div class="details-card">
                        <div class="profile-item">
                            <span class="profile-label">Full Name:</span>
                            <span class="profile-value"><asp:Label ID="lblFullname" runat="server" /></span>
                        </div>
                        <div class="profile-item">
                            <span class="profile-label">Email:</span>
                            <span class="profile-value"><asp:Label ID="lblEmail" runat="server" /></span>
                        </div>
                        <div class="profile-item">
                            <span class="profile-label">Username:</span>
                            <span class="profile-value"><asp:Label ID="lblUsername" runat="server" /></span>
                        </div>
                        <div class="profile-item">
                            <span class="profile-label">Age:</span>
                            <span class="profile-value"><asp:Label ID="lblAge" runat="server" /></span>
                        </div>
                        <div class="profile-item">
                            <span class="profile-label">Gender:</span>
                            <span class="profile-value"><asp:Label ID="lblGender" runat="server" /></span>
                        </div>
                        <div class="profile-item">
                            <span class="profile-label">Country:</span>
                            <span class="profile-value"><asp:Label ID="lblCountry" runat="server" /></span>
                        </div>
                        <div class="profile-item">
                            <span class="profile-label">Registered On:</span>
                            <span class="profile-value"><asp:Label ID="lblDate" runat="server" /></span>
                        </div>
                    </div>
                </div>
            </div>

            <div class="button-container">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update Profile" CssClass="btn-update" />
            </div>
        </div>
    </div>
</div>
</asp:Content>

