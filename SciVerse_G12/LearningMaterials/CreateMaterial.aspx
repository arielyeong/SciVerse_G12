<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="CreateMaterial.aspx.cs" Inherits="SciVerse_G12.LearningMaterials.CreateMaterial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .body-content {
            margin-top: 10px !important; 
        }
    </style>
    <script type="text/javascript">
        function autoDetectType(fileUploader) {
            var ddlType = document.getElementById('<%= ddlType.ClientID %>');
            var fileName = fileUploader.value.toLowerCase();

            if (fileName.endsWith(".pdf")) {
                ddlType.value = "PDF";
            } else if (fileName.endsWith(".doc") || fileName.endsWith(".docx")) {
                ddlType.value = "Word";
            } else if (fileName.endsWith(".mp4")) {
                ddlType.value = "Video";
            } else if (fileName.length > 0) {
                ddlType.value = "Other";
            } else {
                ddlType.value = "";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-1">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <h2 class="text-center mb-4 fw-bold">Create New Learning Material</h2>

                <%-- Status Message --%>
               <div id="statusMessageContainer"></div>
                <div class="position-fixed top-0 end-0 p-3" style="z-index: 1100">
                    <div id="successToast" class="toast align-items-center text-white bg-success border-0" 
                         role="alert" aria-live="assertive" aria-atomic="true">
                        <div class="d-flex">
                            <div class="toast-body">
                                Learning material saved successfully!
                            </div>
                            <button type="button" class="btn-close btn-close-white me-2 m-auto" 
                                    data-bs-dismiss="toast" aria-label="Close"></button>
                        </div>
                    </div>
                </div>
                <div class="card shadow-sm">
                    <div class="card-body p-4">
                        
                        <%-- Title (No changes, this is correct) --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="txtTitle" CssClass="form-label fw-bold">Title</asp:Label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valTitle" runat="server" 
                                ControlToValidate="txtTitle" 
                                ErrorMessage="Title is required." 
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="CreateMaterial" />
                        </div>

                        <%-- Description (No changes) --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="txtDescription" CssClass="form-label fw-bold">Description</asp:Label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        </div>

                        <%-- START: UPDATED CHAPTER BLOCK --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="txtChapter" CssClass="form-label fw-bold">Chapter Number</asp:Label>
                            <asp:TextBox ID="txtChapter" runat="server" CssClass="form-control" TextMode="Number" placeholder="e.g., 1"></asp:TextBox>

                            <%-- 1. Validation for NULL (empty) --%>
                            <asp:RequiredFieldValidator ID="valChapterRequired" runat="server" 
                                ControlToValidate="txtChapter" 
                                ErrorMessage="Chapter number is required." 
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="CreateMaterial" />
                            
                            <%-- 2. Validation for Number (no negative) --%>
                            <asp:RangeValidator ID="valChapterRange" runat="server" 
                                ControlToValidate="txtChapter" 
                                ErrorMessage="Chapter number must be 1 or greater."
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="CreateMaterial"
                                Type="Integer" 
                                MinimumValue="1" 
                                MaximumValue="9999" /> 
                            <asp:CustomValidator ID="valChapterExists" runat="server"
                                ControlToValidate="txtChapter"
                                ErrorMessage="This chapter number already exists."
                                Display="Dynamic" 
                                CssClass="text-danger"
                                ValidationGroup="CreateMaterial"
                                OnServerValidate="valChapterExists_ServerValidate" />
                        </div>
                        <%-- END: UPDATED CHAPTER BLOCK --%>
                        
                        <%-- File Upload (No changes, this is correct) --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="fileUpload" CssClass="form-label fw-bold">File</asp:Label>
                            <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" 
                                onchange="autoDetectType(this);" />
                            <asp:RequiredFieldValidator ID="valFile" runat="server" 
                                ControlToValidate="fileUpload" 
                                ErrorMessage="A file is required." 
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="CreateMaterial" />
                        </div>
                        
                        <div class="mb-3">
                            <asp:Label runat="server" For="ddlType" CssClass="form-label fw-bold">Material Type</asp:Label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                <%-- 1. ADD THIS EMPTY ITEM --%>
                                <asp:ListItem Value="" Text="-- Select Type --"></asp:ListItem>
        
                                <asp:ListItem Value="PDF">PDF Document</asp:ListItem>
                                <asp:ListItem Value="Word">Word Document</asp:ListItem>
                                <asp:ListItem Value="Video">MP4 Video</asp:ListItem>
                                <asp:ListItem Value="Other">Other</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="valType" runat="server"
                                ControlToValidate="ddlType"
                                InitialValue=""
                                ErrorMessage="Please select a material type."
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="CreateMaterial" />
                        </div>

                        <%-- Action Buttons (No changes) --%>
                        <div class="mt-4 text-end">
                            <asp:Button ID="btnSave" runat="server" Text="Save Material" 
                                CssClass="btn btn-primary" OnClick="btnSave_Click" 
                                ValidationGroup="CreateMaterial" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                CssClass="btn btn-secondary ms-2" OnClick="btnCancel_Click" 
                                CausesValidation="false" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
