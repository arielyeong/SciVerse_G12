<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StartSimulation.aspx.cs" Inherits="SciVerse_G12.Simulation.StartSimulation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .simulation-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 30px 20px;
        }
        .experiment-title {
            text-align: center;
            color: #0056b3;
            font-size: 2.5rem;
            font-weight: 700;
            margin-bottom: 30px;
            letter-spacing: 1px;
        }
        .preview-container {
            display: flex;
            justify-content: center;
            margin-bottom: 30px;
        }
        .simulation-preview {
            width: 100%;
            max-width: 800px;
            height: 400px;
            background-color: #f8f9fa;
            border: 2px dashed #dee2e6;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #6c757d;
            font-size: 18px;
        }
        .button-container {
            text-align: center;
            margin-bottom: 40px;
        }
        .btn-start {
            padding: 15px 50px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 18px;
            font-weight: 600;
            transition: background-color 0.2s, transform 0.1s;
        }
        .btn-start:hover {
            background-color: #218838;
            transform: scale(1.05);
        }
        .details-section {
            background-color: #f8f9fa;
            border-radius: 12px;
            padding: 30px;
            margin-top: 30px;
        }
        .detail-section {
            margin-bottom: 25px;
        }
        .detail-section:last-child {
            margin-bottom: 0;
        }
        .detail-label {
            font-weight: 700;
            font-size: 1.2rem;
            color: #495057;
            margin-bottom: 10px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        .detail-content {
            font-size: 16px;
            color: #212529;
            line-height: 1.8;
            white-space: pre-wrap;
            word-wrap: break-word;
        }
        .instruction-content {
            font-size: 16px;
            color: #212529;
            line-height: 1.8;
            white-space: pre-wrap;
            word-wrap: break-word;
            padding-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="simulation-container">
        <h1 class="experiment-title" id="h1Title" runat="server"></h1>

        <div class="preview-container">
            <div class="simulation-preview">
                <asp:Image ID="imgPreview" runat="server" style="width: 100%; height: 100%; object-fit: cover; border-radius: 10px;" />
                <asp:Label ID="lblNoPreview" runat="server" Text="Simulation Preview" Visible="false" style="display: flex; align-items: center; justify-content: center; width: 100%; height: 100%;"></asp:Label>
            </div>
        </div>

        <div class="button-container">
            <asp:Button ID="btnStart" runat="server" Text="Start" CssClass="btn-start" OnClick="btnStart_Click" />
        </div>

        <div class="details-section">
            <div class="detail-section">
                <div class="detail-label">Description</div>
                <div class="detail-content" id="divDescription" runat="server"></div>
            </div>
            <div class="detail-section">
                <div class="detail-label">Instruction</div>
                <div class="instruction-content" id="divInstruction" runat="server"></div>
            </div>
        </div>
    </div>
</asp:Content>
