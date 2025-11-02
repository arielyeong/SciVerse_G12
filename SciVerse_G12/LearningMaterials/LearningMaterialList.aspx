<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LearningMaterialList.aspx.cs" Inherits="SciVerse_G12.LearningMaterials.LearningMaterialList" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />
    <link href='<%= ResolveUrl("~/Styles/Flashcard.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <style>
        .body-content {
            margin-top: 10px !important; 
        }
        .mb-6 {
            margin-bottom: 30px !important;
        } 
        .material-card {
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            transition: transform 0.2s ease-in-out;
            display: flex;
            flex-direction: column;
        }
        .material-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .material-card .card-body {
            padding: 1rem 1.5rem;
            flex-grow: 1;
        }
        .material-card .card-chapter {
            font-size: 0.9rem;
            color: #6c757d;
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
            margin-bottom: 1rem;
        }
        .material-card .card-footer {
            background-color: #f9f9f9;
            padding: 0.75rem 1.5rem;
            border-top: 1px solid #e0e0e0;
            display: flex;
            gap: 0.75rem;
        }
        
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="text-center mb-6 fw-bold">Learning Materials</h2>
        <div class="row g-4 justify-content-center">
            <asp:Repeater ID="rptLearningMaterials" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-8 col-lg-6"> 
                        <div class="card material-card h-100">
                            <div class="card-body">
                                <p class="card-chapter mb-1">Chapter <%# Eval("Chapter") %></p>
                                <h4 class="card-title mb-2">
                                    <%# Eval("Title") %>
                                </h4>
                                <p class="card-text">
                                    <%# Eval("Description") %>
                                </p>
                            </div>
                            
                            <%-- Footer with buttons --%>
                            <div class="card-footer">
                                <%-- Notes button - only visible if HasNote is true --%>
                                <asp:Repeater ID="rptNotes" runat="server" DataSource='<%# Eval("Notes") %>'>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="btnNotes" runat="server"
                                            NavigateUrl='<%# Eval("FilePath") %>'
                                            Target="_blank"
                                            CssClass="btn btn-primary btn-sm"
                                            Visible='<%# !string.IsNullOrEmpty(Eval("FilePath") as string) %>'>
                                            <i class="bi bi-file-text me-2"></i> <%# Eval("FileName") %>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                                <%-- Video button - only visible if HasVideo is true --%>
                                <asp:Repeater ID="rptVideos" runat="server" DataSource='<%# Eval("Videos") %>'>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="btnVideo" runat="server"
                                            NavigateUrl='<%# Eval("FilePath") %>'
                                            Target="_blank"
                                            CssClass="btn btn-danger btn-sm"
                                            Visible='<%# !string.IsNullOrEmpty(Eval("FilePath") as string) %>'>
                                            <i class="bi bi-play-circle me-2"></i> <%# Eval("FileName") %>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
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