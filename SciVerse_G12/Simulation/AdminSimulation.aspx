<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminSimulation.aspx.cs" Inherits="SciVerse_G12.Simulation.AdminSimulation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .monitor-container {
            padding: 40px;
            margin-bottom: 40px;
        }
        .page-title {
            /*background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);*/
            padding: 20px;
            text-align: center;
            color: #0056b3;
            font-size: 2rem;
            font-weight: 700;
            letter-spacing: 1px;
        }
        .monitor-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        .monitor-table th,
        .monitor-table th a {
            background-color: #007bff !important;
            color: white !important;
            padding: 12px;
            text-align: left;
            border: 1px solid #ddd;
            font-weight: bold;
            text-decoration: none;
        }
        .monitor-table th a:hover {
            color: white !important;
            text-decoration: underline;
        }
        .monitor-table thead th {
            background-color: #007bff !important;
            color: white !important;
        }
        /* GridView specific header styling */
        table.monitor-table th,
        table.monitor-table > thead > tr > th {
            background-color: #007bff !important;
            color: white !important;
            font-weight: bold;
        }
        /* Target all header cells in GridView */
        .monitor-table th * {
            color: white !important;
        }
        .monitor-table td {
            padding: 10px;
            border: 1px solid #ddd;
        }
        .monitor-table tr:nth-child(even) {
            background-color: #f2f2f2;
        }
        .monitor-table tr:hover {
            background-color: #e8f5e9;
        }
        .action-button {
            padding: 6px 12px;
            margin: 2px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            background-color: #2196F3;
            color: white;
        }
        .action-button:hover {
            background-color: #0b7dda;
        }
        .search-container {
            margin-bottom: 20px;
            padding: 15px;
            background-color: #f8f9fa;
            border-radius: 8px;
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }
        .search-input {
            flex: 1;
            min-width: 250px;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }
        .search-button {
            padding: 10px 20px;
            background-color: #0b7dda;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            font-weight: 500;
        }
        .search-button:hover {
            background-color: #0056b3;
        }
        .clear-button {
            padding: 10px 20px;
            background-color: #6c757d;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            font-weight: 500;
        }
        .clear-button:hover {
            background-color: #5a6268;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="monitor-container">
        <h1 class="page-title">Experiment Simulation Monitor</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="false" Style="display: block; padding: 10px; margin-bottom: 15px; border-radius: 4px; font-weight: 500;"></asp:Label>
        <div class="search-container">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" placeholder="Search by ID, Title, Chapter, or Username..."></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="search-button" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="clear-button" OnClick="btnClear_Click" />
        </div>
        <asp:GridView ID="gvSimulationMonitor" runat="server" 
            CssClass="monitor-table" 
            AutoGenerateColumns="false" 
            AllowPaging="true" 
            PageSize="10"
            AllowSorting="true">
            <HeaderStyle BackColor="#007bff" ForeColor="White" Font-Bold="true" />
            <Columns>
                <asp:BoundField DataField="SimulationID" HeaderText="Simulation ID" SortExpression="SimulationID" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Chapter" HeaderText="Chapter" SortExpression="Chapter" />
                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnDetails" runat="server" Text="Details" CssClass="action-button" CommandArgument='<%# Eval("SimulationID") %>' OnClick="btnDetails_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <table class="monitor-table">
                    <thead>
                        <tr>
                            <th>Simulation ID</th>
                            <th>Title</th>
                            <th>Chapter</th>
                            <th>Username</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="5" style="text-align: center; padding: 20px;">
                                No simulation data available.
                            </td>
                        </tr>
                    </tbody>
                </table>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
