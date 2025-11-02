<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ManageMaterials.aspx.cs" Inherits="SciVerse_G12.LearningMaterials.materials.ManageMaterials" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var pendingPostBackButton = null;
        var confirmModalInstance = null;
        var isConfirmingPostBack = false;
        var shouldTriggerPostback = false;

        // Show confirmation modal for single delete
        function showConfirmModal(message, button) {
            console.log('showConfirmModal called, isConfirmingPostBack:', isConfirmingPostBack);

            if (isConfirmingPostBack) {
                console.log('Allowing postback to proceed');
                isConfirmingPostBack = false;
                return true;
            }

            // Prevent showing modal if already pending
            if (pendingPostBackButton) {
                console.log('Modal already pending, ignoring');
                return false;
            }

            console.log('Showing modal');
            document.getElementById('confirmModalBody').innerHTML = message;
            pendingPostBackButton = button;
            shouldTriggerPostback = false;

            if (!confirmModalInstance) {
                confirmModalInstance = new bootstrap.Modal(document.getElementById('confirmModal'));
            }
            confirmModalInstance.show();

            return false;
        }

        // Show confirmation modal for delete selected - saves checked IDs first
        function showConfirmModalWithIds(message, button) {
            console.log('showConfirmModalWithIds called, isConfirmingPostBack:', isConfirmingPostBack);

            if (isConfirmingPostBack) {
                console.log('Allowing postback to proceed');
                isConfirmingPostBack = false;
                return true;
            }

            // Prevent showing modal if already pending
            if (pendingPostBackButton) {
                console.log('Modal already pending, ignoring');
                return false;
            }

            console.log('Saving checked IDs and showing modal');

            // Save checked row indices to hidden field
            var checkboxes = document.querySelectorAll('.chk-select-item input[type="checkbox"]');
            var checkedIds = [];

            checkboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    var row = checkbox.closest('tr');
                    if (row && row.rowIndex > 0) {
                        checkedIds.push(row.rowIndex - 1);
                    }
                }
            });

            var hdnField = document.getElementById('<%= hdnSelectedIds.ClientID %>');
            if (hdnField) {
                hdnField.value = checkedIds.join(',');
                console.log('Saved checked row indices:', hdnField.value);
            }

            document.getElementById('confirmModalBody').innerHTML = message;
            pendingPostBackButton = button;
            shouldTriggerPostback = false;

            if (!confirmModalInstance) {
                confirmModalInstance = new bootstrap.Modal(document.getElementById('confirmModal'));
            }
            confirmModalInstance.show();

            return false;
        }

        // Toggle all checkboxes when header is clicked
        function toggleAllCheckboxes(headerCheckbox) {
            var isChecked = headerCheckbox.checked;
            var checkboxes = document.querySelectorAll('.chk-select-item input[type="checkbox"]');

            checkboxes.forEach(function (checkbox) {
                checkbox.checked = isChecked;
            });

            updateDeleteButton();
        }

        // Update delete button visibility based on selected checkboxes
        function updateDeleteButton() {
            var checkboxes = document.querySelectorAll('.chk-select-item input[type="checkbox"]');
            var deleteButton = document.querySelector('.btn-delete-selected');
            var headerCheckbox = document.querySelector('.chk-select-header input[type="checkbox"]');

            var anyChecked = false;
            var allChecked = true;
            var checkedCount = 0;

            checkboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    anyChecked = true;
                    checkedCount++;
                } else {
                    allChecked = false;
                }
            });

            // Show/hide delete button - ONLY if MORE THAN 1 is selected
            if (checkedCount > 1) {
                deleteButton.classList.remove('d-none');
            } else {
                deleteButton.classList.add('d-none');
            }

            // Update header checkbox state
            if (headerCheckbox) {
                if (checkedCount === 0) {
                    headerCheckbox.checked = false;
                    headerCheckbox.indeterminate = false;
                } else if (allChecked && checkedCount > 0) { // Check count > 0
                    headerCheckbox.checked = true;
                    headerCheckbox.indeterminate = false;
                } else if (anyChecked) { // Use anyChecked for indeterminate
                    headerCheckbox.checked = false;
                    headerCheckbox.indeterminate = true;
                } else { // Explicitly handle no boxes
                    headerCheckbox.checked = false;
                    headerCheckbox.indeterminate = false;
                }
            }
        }

        // Handle modal cancel
        function handleModalCancel() {
            console.log('Modal cancelled');
            pendingPostBackButton = null;
            shouldTriggerPostback = false;
            isConfirmingPostBack = false;
        }

        // Handle modal confirm
        function handleModalConfirm(e) {
            console.log('handleModalConfirm called, pendingPostBackButton:', pendingPostBackButton);
            
            e.preventDefault();
            e.stopPropagation();
            
            if (pendingPostBackButton && !shouldTriggerPostback) {
                console.log('Setting shouldTriggerPostback to true');
                shouldTriggerPostback = true;
                
                // Remove focus
                var btnConfirm = document.getElementById('btnModalConfirm');
                if (btnConfirm) {
                    btnConfirm.blur();
                }
                
                // Close modal
                var modalEl = document.getElementById('confirmModal');
                var modal = bootstrap.Modal.getInstance(modalEl);
                if (modal) {
                    modal.hide();
                }
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            var modalEl = document.getElementById('confirmModal');

            if (modalEl) {
                modalEl.addEventListener('hidden.bs.modal', function () {
                    console.log('Modal hidden - shouldTriggerPostback:', shouldTriggerPostback, 'pendingPostBackButton:', pendingPostBackButton);
                    
                    if (shouldTriggerPostback && pendingPostBackButton) {
                        console.log('Triggering postback now');
                        
                        var buttonToClick = pendingPostBackButton;
                        var buttonName = buttonToClick.name;
                        
                        // Reset flags BEFORE triggering postback
                        shouldTriggerPostback = false;
                        isConfirmingPostBack = true;
                        pendingPostBackButton = null;
                        
                        // Use __doPostBack directly instead of clicking the button
                        console.log('Calling __doPostBack with:', buttonName);
                        __doPostBack(buttonName, '');
                    } else {
                        console.log('User cancelled or no button pending - resetting all flags');
                        // User cancelled - reset everything
                        pendingPostBackButton = null;
                        shouldTriggerPostback = false;
                        isConfirmingPostBack = false;
                    }
                });
            }

            updateDeleteButton();
        });

        // Re-initialize after postback (if using UpdatePanel)
        // Safe check for Sys object
        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm) {
                prm.add_endRequest(function () {
                    updateDeleteButton();
                });
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mb-5">
        <h2 class="text-center mb-4 fw-bold">Manage Learning Materials</h2>

        <%-- Bootstrap Confirmation Modal --%>
        <div class="modal fade" id="confirmModal" tabindex="-1" aria-labelledby="confirmModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h5 class="modal-title" id="confirmModalLabel">
                            <i class="bi bi-exclamation-triangle-fill me-2"></i>Confirm Delete
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body" id="confirmModalBody">
                        Are you sure you want to proceed?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" onclick="handleModalCancel()">Cancel</button>
                        <%-- Use onclick instead of addEventListener for reliability --%>
                        <button type="button" class="btn btn-danger" id="btnModalConfirm" onclick="handleModalConfirm(event)">Delete</button>
                    </div>
                </div>
            </div>
        </div>

       <div class="d-flex justify-content-between align-items-center mb-3">
            
            <asp:HyperLink ID="hlCreate" runat="server" 
                Text="Create New Material" 
                NavigateUrl="CreateMaterial.aspx"
                CssClass="btn btn-success" />

            <%-- Hidden field to store selected row indices --%>
            <asp:HiddenField ID="hdnSelectedIds" runat="server" />

            <%-- Delete Selected Button - Uses UseSubmitBehavior="false" for custom postback --%>
            <asp:Button ID="btnDeleteSelected" runat="server" 
                Text="Delete Selected" 
                CssClass="btn btn-danger btn-delete-selected d-none"
                OnClick="btnDeleteSelected_Click" 
                UseSubmitBehavior="false"
                OnClientClick="showConfirmModalWithIds('Are you sure you want to delete all selected materials?', this); return false;" />
        </div>

        <asp:GridView ID="Materials" runat="server" 
            AutoGenerateColumns="False" 
            DataKeyNames="MaterialID" 
            CssClass="table table-hover"
            GridLines="None"
            Width="100%"
            OnRowCommand="Materials_RowCommand">
            
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkHeader" runat="server" 
                            CssClass="chk-select-header" 
                            onclick="toggleAllCheckboxes(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" 
                            CssClass="chk-select-item" 
                            onclick="updateDeleteButton();" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Chapter" HeaderText="Chapter" SortExpression="Chapter" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" />
                <asp:BoundField DataField="Type" HeaderText="File Type" SortExpression="Type" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" 
                            NavigateUrl='<%# "EditMaterial.aspx?ID=" + Eval("MaterialID") %>' 
                            Text="Edit" 
                            CssClass="btn btn-sm btn-secondary" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Copy">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkCopy" runat="server" 
                            NavigateUrl='<%# "CreateMaterial.aspx?CopyFromID=" + Eval("MaterialID") %>' 
                            Text="Copy" 
                            CssClass="btn btn-sm btn-info" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" 
                            CommandName="DeleteRow" 
                            CommandArgument='<%# Eval("MaterialID") %>' 
                            Text="Delete" 
                            CssClass="btn btn-sm btn-danger"
                            UseSubmitBehavior="false"
                            OnClientClick="showConfirmModal('Are you sure you want to delete this material?', this); return false;" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>