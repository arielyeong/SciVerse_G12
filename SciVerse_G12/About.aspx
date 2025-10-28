<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="SciVerse_G12.About" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
     <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/Home.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">

        <!-- Team Section -->
        <section class="team-section">
            <h2 class="team-header">Meet Our Team</h2>
            <div class="team-subtitle">
                <p>We are from Group 12</p>
            </div>
            <div class="team-container">
                <div class="team-member">
                    <div class="member-image">
                        <img src="Images/About/charlotte.png" alt="Charlotte" />
                    </div>
                    <div class="member-info">
                        <h3>Charlotte Chen Zi Shan</h3>
                        <p>TP072017</p>
                    </div>
                </div>
                
                <div class="team-member">
                    <div class="member-image">
                        <img src="Images/About/chingying.png" alt="Ching Ying" />
                    </div>
                    <div class="member-info">
                        <h3>Chong Ching Ying</h3>
                        <p>TP070004</p>
                    </div>
                </div>
                
                <div class="team-member">
                    <div class="member-image">
                        <img src="Images/About/hueyyee.png" alt="Huey Yee" />
                    </div>
                    <div class="member-info">
                        <h3>Yeong Huey Yee</h3>
                        <p>TP071856</p>
                    </div>
                </div>
                
                <div class="team-member">
                    <div class="member-image">
                        <img src="Images/About/yingxin.png" alt="Ying Xin" />
                    </div>
                    <div class="member-info">
                        <h3>Ong Ying Xin</h3>
                        <p>TP071008</p>
                    </div>
                </div>
            </div>

        </section>
    </main>
</asp:Content>