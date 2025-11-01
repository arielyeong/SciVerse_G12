<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LearningMaterialList.aspx.cs" Inherits="SciVerse_G12.LearningMaterials.LearningMaterialList" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href='<%= ResolveUrl("~/Styles/Flashcard.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <style>
        .body-content {
            margin-top: 10px !important; 
        }
        mb-6 {
            margin-bottom: 30px !important;
        } 
        .material-card {
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            transition: transform 0.2s ease-in-out;
            text-decoration: none; /* Remove underline from the link */
            color: inherit; /* Inherit text color for children */
            display: block; /* Make the whole card clickable */
        }
        .material-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            text-decoration: none; /* Ensure no underline on hover */
        }
        .material-card .card-body {
            padding: 1.5rem;
        }
        .material-card .card-chapter {
            font-size: 0.9rem;
            color: #6c757d; /* Muted text color */
            margin-bottom: 0.5rem;
        }
        .material-card .card-title {
            font-size: 1.5rem;
            font-weight: bold;
            color: #343a40;
            margin-bottom: 0.75rem;
        }
        .material-card .card-text {
            font-size: 1rem;
            color: #495057;
            margin-bottom: 0.5rem; /* Reduced margin since no button below */
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="text-center mb-6 fw-bold">Learning Materials</h2>

        <div class="row g-4 justify-content-center">
            <%-- Changed Repeater ID to rptLearningMaterials --%>
            <asp:Repeater ID="rptLearningMaterials" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-8 col-lg-6"> <%-- Each item takes full width on small, reduced width on medium/large for better readability --%>
                        <%-- Wrap the entire card in an asp:HyperLink --%>
                        <asp:HyperLink ID="hlMaterialLink" runat="server"
                            CssClass="card material-card h-100"
                            NavigateUrl='<%# Eval("FilePath") %>'
                            Target="_blank"> <%-- Target="_blank" opens in a new tab --%>
                            <div class="card-body">
                                <p class="card-chapter mb-1">Chapter: <%# Eval("Chapter") %></p>
                                <h4 class="card-title mb-2">
                                    <%# Eval("Title") %>
                                </h4>
                                <p class="card-text">
                                    <%# Eval("Description") %>
                                </p>
                            </div>
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="row mt-5" id="noMaterialsSection" runat="server" visible="false">
            <div class="col-12">
                <div class="alert alert-info text-center" role="alert">
                    <i class="bi bi-info-circle me-2"></i>
                    <asp:Label ID="lblNoMaterials" runat="server"
                        Text="No learning materials available at the moment."></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>