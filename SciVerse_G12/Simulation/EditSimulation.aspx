<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditSimulation.aspx.cs" Inherits="SciVerse_G12.Simulation.EditSimulation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .page-title {
            padding: 20px;
            text-align: left;
            color: #0056b3;
            font-size: 2rem;
            font-weight: 700;
            letter-spacing: 1px;
            margin-bottom: 30px;
        }
        .details-container {
            padding: 20px;
            max-width: 1200px;
            margin: 0 auto;
        }
        .detail-section {
            background-color: #f8f9fa;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            /*border-left: 4px solid #007bff;*/
        }
        .detail-label {
            font-weight: bold;
            color: #495057;
            font-size: 14px;
            margin-bottom: 8px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        .detail-textbox {
            width: 100%;
            padding: 10px;
            border: 1px solid #ced4da;
            border-radius: 4px;
            font-size: 16px;
            margin-bottom: 10px;
            box-sizing: border-box;
        }
        .detail-textbox:focus {
            border-color: #007bff;
            outline: none;
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
        }
        .detail-textarea {
            width: 100%;
            padding: 10px;
            border: 1px solid #ced4da;
            border-radius: 4px;
            font-size: 16px;
            margin-bottom: 10px;
            min-height: 120px;
            resize: vertical;
            font-family: inherit;
            box-sizing: border-box;
        }
        .detail-textarea:focus {
            border-color: #007bff;
            outline: none;
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
        }
        .done-button {
            padding: 12px 40px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            font-weight: 500;
            margin: 10px;
        }
        .done-button:hover {
            background-color: #218838;
        }
        .back-button {
            padding: 12px 40px;
            background-color: #6c757d;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            font-weight: 500;
            margin: 10px;
        }
        .back-button:hover {
            background-color: #5a6268;
        }
        .save-message {
            display: block;
            margin-top: 15px;
            padding: 10px;
            border-radius: 4px;
            font-size: 14px;
            font-weight: 500;
            text-align: center;
        }
        .button-container {
            text-align: center;
            margin-top: 30px;
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }
        .simulation-preview {
            background-color: #ffffff;
            border: 2px dashed #dee2e6;
            border-radius: 8px;
            padding: 20px;
            margin-top: 20px;
            text-align: center;
            color: #6c757d;
            min-height: 300px;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .preview-placeholder {
            font-size: 18px;
            color: #adb5bd;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="details-container">
        <h1 class="page-title">Experiment Simulation Details</h1>

        <asp:HiddenField ID="hfSimulationID" runat="server" />

        <div class="detail-section">
            <div class="detail-label">Simulation Title</div>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="detail-textbox"></asp:TextBox>
        </div>

        <div class="detail-section">
            <div class="detail-label">Chapter</div>
            <asp:TextBox ID="txtChapter" runat="server" CssClass="detail-textbox"></asp:TextBox>
        </div>

        <div class="detail-section">
            <div class="detail-label">Description</div>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="detail-textarea" TextMode="MultiLine" Rows="5"></asp:TextBox>
        </div>

        <div class="detail-section">
            <div class="detail-label">Instruction</div>
            <asp:TextBox ID="txtInstruction" runat="server" CssClass="detail-textarea" TextMode="MultiLine" Rows="8"></asp:TextBox>
        </div>

        <div class="detail-section">
            <div class="detail-label">Simulation Preview</div>
            <div class="simulation-preview">
                <iframe id="previewIframe" runat="server" style="width: 100%; height: 450px; border: 0; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);" allowfullscreen></iframe>
            </div>
        </div>

        <div class="button-container">
            <asp:Button ID="btnSaveAll" runat="server" Text="Done" CssClass="done-button" OnClick="btnSaveAll_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="back-button" OnClick="btnBack_Click" />
        </div>
        <div class="button-container">
            <asp:Label ID="lblSaveMessage" runat="server" CssClass="save-message" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
