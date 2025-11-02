<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Experiment.aspx.cs" Inherits="SciVerse_G12.Simulation.Experiment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .btn-sidebar, .btn-exit {
            position: absolute;
            top: 100px;
            left: 70px;
            z-index: 1000;
        }
        .btn-exit {
            left: 200px;
        }
        .btn-restart {
            position: absolute;
            top: 100px;
            right: 20px;
            z-index: 1000;
        }
        .experiment-button {
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            font-weight: 500;
            transition: background-color 0.2s, transform 0.1s;
        }
        .btn-sidebar {
            background-color: #6c757d;
            color: white;
        }
        .btn-sidebar:hover {
            background-color: #5a6268;
        }
        .btn-restart {
            background-color: #007bff;
            color: white;
        }
        .btn-restart:hover {
            background-color: #0056b3;
        }
        .btn-exit {
            background-color: #dc3545;
            color: white;
        }
        .btn-exit:hover {
            background-color: #c82333;
        }
        .experiment-button:active {
            transform: scale(0.98);
        }
        .experiment-container {
            position: relative;
            width: 100%;
            max-width: 960px;
            margin: 0 auto;
            display: flex;
            justify-content: center;
            align-items: center;
            padding-top: 80px;
        }
        .experiment-iframe {
            width: 100%;
            max-width: 960px;
            aspect-ratio: 4/3;
            border: 0;
            display: block;
        }
        .sidebar {
            position: fixed;
            left: -350px;
            top: 0;
            width: 350px;
            height: 100vh;
            background-color: #f8f9fa;
            box-shadow: 2px 0 8px rgba(0,0,0,0.1);
            transition: left 0.3s ease;
            z-index: 1001;
            overflow-y: auto;
            padding: 20px;
        }
        .sidebar.open {
            left: 0;
        }
        .sidebar-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            padding-bottom: 15px;
            border-bottom: 2px solid #dee2e6;
        }
        .sidebar-header h3 {
            margin: 0;
            color: #495057;
            font-size: 20px;
        }
        .close-sidebar {
            background: none;
            border: none;
            font-size: 24px;
            cursor: pointer;
            color: #6c757d;
            padding: 0;
            width: 30px;
            height: 30px;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .close-sidebar:hover {
            color: #dc3545;
        }
        .instructions-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }
        .instruction-step {
            background: white;
            border-left: 4px solid #007bff;
            padding: 15px;
            margin-bottom: 15px;
            border-radius: 4px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            opacity: 0;
            transform: translateX(-20px);
            transition: opacity 0.3s ease, transform 0.3s ease;
        }
        .instruction-step.visible {
            opacity: 1;
            transform: translateX(0);
        }
        .instruction-step h4 {
            margin: 0 0 10px 0;
            color: #007bff;
            font-size: 18px;
        }
        .instruction-step p {
            margin: 0;
            color: #495057;
            line-height: 1.6;
        }
        .sidebar-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5);
            z-index: 1000;
        }
        .sidebar-overlay.show {
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="sidebar-overlay" id="sidebarOverlay"></div>
    <div class="sidebar" id="instructionSidebar">
        <div class="sidebar-header">
            <h3>Instructions</h3>
            <button type="button" class="close-sidebar" onclick="closeSidebar()">&times;</button>
        </div>
        <ul class="instructions-list" id="instructionsList" runat="server">
        </ul>
    </div>
    <div>
        <asp:Button ID="btnSidebar" runat="server" Text="Instruction" CssClass="experiment-button btn-sidebar" OnClientClick="openSidebar(); return false;" />
    </div>
    <div>
        <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="experiment-button btn-exit" OnClick="btnExit_Click" />
    </div>
    <div>
        <asp:Button ID="btnRestart" runat="server" Text="Restart" CssClass="experiment-button btn-restart" OnClick="btnRestart_Click" />
    </div>
    <div class="experiment-container">
        <iframe id="experimentIframe" runat="server" class="experiment-iframe" allowfullscreen></iframe>
    </div>
    <script type="text/javascript">
        function openSidebar() {
            var sidebar = document.getElementById('instructionSidebar');
            var overlay = document.getElementById('sidebarOverlay');
            
            sidebar.classList.add('open');
            overlay.classList.add('show');
            
            return false;
        }
        
        function closeSidebar() {
            var sidebar = document.getElementById('instructionSidebar');
            var overlay = document.getElementById('sidebarOverlay');
            
            sidebar.classList.remove('open');
            overlay.classList.remove('show');
        }
        
        // Close sidebar when clicking on overlay
        document.getElementById('sidebarOverlay').addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            closeSidebar();
        });
        
        // Animate instructions on load
        window.addEventListener('load', function() {
            var steps = document.querySelectorAll('.instruction-step');
            steps.forEach(function(step, index) {
                setTimeout(function() {
                    step.classList.add('visible');
                }, index * 200);
            });
        });
    </script>
</asp:Content>
