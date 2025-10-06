<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="SciVerse_G12.AdminProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <style>
            .profile-container {
                width: 100%;
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

            <!-- Sidebar -->
    <aside class="admin-sidebar" id="sidebar">
        <div class="sidebar-header">
            <div class="sidebar-logo">SCIVERSE<br>General Science Learning</div>
        </div>
        <nav class="sidebar-nav">
            <a href="~/Admin/AdminProfile.aspx" runat="server" class="sidebar-item"><i class="fa fa-user"></i> Profile</a>
            <a href="~/Admin/ViewUserList.aspx" runat="server" class="sidebar-item active"><i class="fa fa-users"></i> User List</a>
            <a href="~/Account/QuizList.aspx" runat="server" class="sidebar-item"><i class="fa fa-question-circle"></i> Quiz List</a>
            <a href="~/Account/LearningMaterial.aspx" runat="server" class="sidebar-item"><i class="fa fa-book"></i> Learning Material</a>
            <a href="~/Account/ExperimentSimulation.aspx" runat="server" class="sidebar-item"><i class="fa fa-flask"></i> Experiment Simulation</a>
            <a href="~/Account/LearningMaterial.aspx" runat="server" class="sidebar-item"><i class="fa fa-chart-bar"></i> User Performance</a>
            <a href="~/Admin/ViewMessage.aspx" runat="server" class="sidebar-item"><i class="fa fa-envelope"></i> View Messages</a>
        </nav>
</aside>

<h1 class="text-center mb-3">Your Profile</h1>

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

</asp:Content>

