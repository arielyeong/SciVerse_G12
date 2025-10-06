<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SciVerse_G12.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .profile-container {
            width: 60%;
            margin: 40px auto;
            border: 1px solid #ddd;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0px 4px 10px rgba(0,0,0,0.1);
        }
        .profile-item {
            margin: 12px 0;
            font-size: 18px;
        }
        .profile-label {
            font-weight: bold;
            color: #333;
            display: inline-block;
            width: 150px;
        }
        .profile-picture {
            width: 150px;
            height: 150px;
            border-radius: 50%;
            object-fit: cover;
            margin-bottom: 20px;

        }
    </style>

    <h1 class="text-center mb-3">Your Profile</h1>

<div class="d-flex justify-content-center align-items-center login-container">

    <div class="profile-container">
        <asp:Image ID="imgProfile" runat="server" CssClass="profile-picture" />

        <div class="profile-item">
            <span class="profile-label">Full Name:</span>
            <asp:Label ID="lblFullname" runat="server" />
        </div>

        <div class="profile-item">
            <span class="profile-label">Email:</span>
            <asp:Label ID="lblEmail" runat="server" />
        </div>

        <div class="profile-item">
            <span class="profile-label">Username:</span>
            <asp:Label ID="lblUsername" runat="server" />
        </div>

        <div class="profile-item">
            <span class="profile-label">Age:</span>
            <asp:Label ID="lblAge" runat="server" />
        </div>

        <div class="profile-item">
            <span class="profile-label">Gender:</span>
            <asp:Label ID="lblGender" runat="server" />
        </div>

        <div class="profile-item">
            <span class="profile-label">Country:</span>
            <asp:Label ID="lblCountry" runat="server" />
        </div>

        <div class="profile-item">
            <span class="profile-label">Registered On:</span>
            <asp:Label ID="lblDate" runat="server" />
        </div>

            <div class="d-grid mb-3 justify-content-center">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" />
            </div>

    </div>
    </div>

</asp:Content>
