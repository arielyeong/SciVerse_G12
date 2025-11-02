<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditMaterial.aspx.cs" Inherits="SciVerse_G12.LearningMaterials.EditMaterial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .body-content {
            margin-top: 10px !important; 
        }
        .current-file-box {
            background-color: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            padding: 10px;
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        function autoDetectType(fileUploader) {
            var ddlType = document.getElementById('<%= ddlType.ClientID %>');
            var fileName = fileUploader.value.toLowerCase();
            ddlType.disabled = false;
            if (fileName.endsWith(".pdf")) {
                ddlType.value = "PDF";
            } else if (fileName.endsWith(".doc") || fileName.endsWith(".docx")) {
                ddlType.value = "Word";
            } else if (fileName.endsWith(".mp4")) {
                ddlType.value = "Video";
            } else if (fileName.length > 0) {
                ddlType.value = "Other";
            } else {
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-1">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <h2 class="text-center mb-4 fw-bold">Edit Learning Material</h2>

                <%-- Status Message --%>
                <div id="statusMessageContainer"></div>

                <div class="card shadow-sm">
                    <div class="card-body p-4">
                        
                        <%-- Title --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="txtTitle" CssClass="form-label fw-bold">Title</asp:Label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valTitle" runat="server" 
                                ControlToValidate="txtTitle" 
                                ErrorMessage="Title is required." 
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="EditMaterial" />
                        </div>

                        <%-- Description --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="txtDescription" CssClass="form-label fw-bold">Description</asp:Label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        </div>

                        <%-- Chapter --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="txtChapter" CssClass="form-label fw-bold">Chapter Number</asp:Label>
                            <asp:TextBox ID="txtChapter" runat="server" CssClass="form-control" TextMode="Number" placeholder="e.g., 1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valChapterRequired" runat="server" 
                                ControlToValidate="txtChapter" 
                                ErrorMessage="Chapter number is required." 
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="EditMaterial" />
                            <asp:RangeValidator ID="valChapterRange" runat="server" 
                                ControlToValidate="txtChapter" 
                                ErrorMessage="Chapter number must be 1 or greater."
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="EditMaterial"
                                Type="Integer" 
                                MinimumValue="1" 
                                MaximumValue="9999" />
                        </div>
                        
                        <%-- File Upload --%>
                        <div class="mb-3">
                            <asp:Label runat="server" CssClass="form-label fw-bold">Current File</asp:Label>
                            <%-- Display current file info --%>
                            <div class="current-file-box">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <i class="bi bi-file-earmark"></i>
                                        <asp:Label ID="lblCurrentFileName" runat="server" CssClass="ms-2"></asp:Label>
                                    </div>
                                    <asp:HyperLink ID="lnkViewFile" runat="server" 
                                        CssClass="btn btn-sm btn-outline-primary" 
                                        Target="_blank">
                                        <i class="bi bi-eye"></i> View
                                    </asp:HyperLink>
                                </div>
                            </div>

                            <%-- Option to upload new file --%>
                            <asp:Label runat="server" For="fileUpload" CssClass="form-label fw-bold mt-3">
                                Replace File (Optional)
                            </asp:Label>
                            <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" 
                                onchange="autoDetectType(this);" />
                            <small class="text-muted">Leave empty to keep the current file</small>
                            <asp:HiddenField ID="hdnCurrentFilePath" runat="server" />
                        </div>
                        
                        <%-- Material Type --%>
                        <div class="mb-3">
                            <asp:Label runat="server" For="ddlType" CssClass="form-label fw-bold">Material Type</asp:Label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
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
                                Display="Dynamic" CssClass="text-danger" ValidationGroup="EditMaterial" />
                        </div>

                        <%-- Action Buttons --%>
                        <div class="mt-4 text-end">
                            <asp:Button ID="btnSave" runat="server" Text="Update Material" 
                                CssClass="btn btn-primary" OnClick="btnSave_Click" 
                                ValidationGroup="EditMaterial" />
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