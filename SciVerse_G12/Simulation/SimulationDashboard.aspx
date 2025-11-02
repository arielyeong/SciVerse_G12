<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SimulationDashboard.aspx.cs" Inherits="SciVerse_G12.Simulation.SimulationDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .dashboard-container {
            padding: 30px 20px;
            max-width: 1400px;
            margin: 0 auto;
        }
        .dashboard-title {
            text-align: center;
            color: #0056b3;
            font-size: 2.5rem;
            font-weight: 700;
            margin-bottom: 40px;
            letter-spacing: 1px;
        }
        .chapter-section {
            margin-bottom: 50px;
        }
        .chapter-title {
            font-size: 1.8rem;
            font-weight: 600;
            color: #007bff;
            margin-bottom: 25px;
            padding-bottom: 10px;
            border-bottom: 3px solid #007bff;
        }
        .experiments-row {
            display: flex;
            flex-wrap: wrap;
            gap: 25px;
            margin-bottom: 25px;
        }
        .experiment-card {
            flex: 1;
            min-width: calc(50% - 12.5px);
            background-color: #ffffff;
            border: 2px solid #e9ecef;
            border-radius: 12px;
            padding: 20px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            transition: transform 0.2s, box-shadow 0.2s;
            text-decoration: none;
            color: inherit;
            display: block;
        }
        .experiment-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 4px 16px rgba(0,0,0,0.15);
            border-color: #007bff;
            text-decoration: none;
            color: inherit;
        }
        .experiment-preview {
            width: 100%;
            height: 200px;
            background-color: #f8f9fa;
            border: 2px dashed #dee2e6;
            border-radius: 8px;
            margin-bottom: 15px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #6c757d;
            font-size: 16px;
        }
        .experiment-details {
            margin-top: 15px;
        }
        .experiment-detail-item {
            margin-bottom: 12px;
            font-size: 15px;
            line-height: 1.6;
        }
        .experiment-detail-item:last-child {
            margin-bottom: 0;
        }
        .detail-label {
            font-weight: 600;
            color: #495057;
            margin-right: 8px;
        }
        .detail-value {
            color: #212529;
        }
        @media (max-width: 768px) {
            .experiment-card {
                min-width: 100%;
            }
        }
        .empty-state {
            text-align: center;
            padding: 40px;
            color: #6c757d;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container">
        <h1 class="dashboard-title">Experiment Simulation</h1>

        <asp:Repeater ID="rptChapters" runat="server" OnItemDataBound="rptChapters_ItemDataBound">
            <ItemTemplate>
                <div class="chapter-section">
                    <h2 class="chapter-title">
                        <%# Eval("Key") %>
                    </h2>
                    <div class="experiments-row">
                        <asp:Repeater ID="rptExperiments" runat="server" OnItemDataBound="rptExperiments_ItemDataBound">
                            <ItemTemplate>
                                <a href='<%# "~/Simulation/StartSimulation.aspx?id=" + Eval("SimulationID") %>' runat="server" class="experiment-card">
                                    <div class="experiment-preview">
                                        <asp:Image ID="imgPreview" runat="server" style="width: 100%; height: 100%; object-fit: cover; border-radius: 6px;" />
                                        <asp:Label ID="lblNoPreview" runat="server" Text="Simulation Preview" Visible="false" style="display: flex; align-items: center; justify-content: center; width: 100%; height: 100%;"></asp:Label>
                                    </div>
                                    <div class="experiment-details">
                                        <div class="experiment-detail-item">
                                            <span class="detail-label">Title:</span>
                                            <span class="detail-value"><%# Eval("Title") %></span>
                                        </div>
                                        <div class="experiment-detail-item">
                                            <span class="detail-label">Description:</span>
                                            <span class="detail-value"><%# Eval("Description") %></span>
                                        </div>
                                        <div class="experiment-detail-item">
                                            <asp:Label ID="lblLastAccessed" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Label ID="lblEmptyState" runat="server" CssClass="empty-state" Visible="false">
            <p>No experiments available at the moment.</p>
        </asp:Label>
    </div>
</asp:Content>
