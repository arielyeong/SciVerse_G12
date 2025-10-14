<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ViewUserList.aspx.cs" Inherits="SciVerse_G12.ViewUserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .admin-content {
            padding: 20px;
            margin-top: -30px !important;
            transition: margin-left 0.3s ease;
        }

        .admin-content.shifted {
            margin-left: 220px;
        }

        /* Container for top controls */
        .top-controls {
            display: flex;
            justify-content: space-between; /* separate left and right sections */
            align-items: center;
            flex-wrap: wrap;
            margin-bottom: 20px;
        }

        /* Left section: search tools */
        .left-controls {
            display: flex;
            gap: 10px;
            align-items: center;
            flex-wrap: wrap;
        }

        /* Right section: action buttons */
        .right-controls {
            display: flex;
            gap: 10px;
            align-items: center;
            flex-wrap: wrap;
        }

        .top-controls .form-control {
            width: 250px;
        }
    </style>

    <!-- Main Content -->
    <div class="admin-content" id="contentArea">
        <div class="content-header">
            <h1>View User List</h1>
        </div>

        <!-- Search + Action buttons -->
        <div class="top-controls">
            <!-- Left side: Search + Clear -->
            <div class="left-controls">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search by Username or Email"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
            </div>

            <!-- Right side: Edit/Delete/Confirm/Cancel -->
            <div class="right-controls">
                <asp:Button ID="btnEditMode" runat="server" Text="Edit Mode" CssClass="btn btn-warning" OnClick="btnEditMode_Click" />
                <asp:Button ID="btnDeleteMode" runat="server" Text="Delete Mode" CssClass="btn btn-danger" OnClick="btnDeleteMode_Click" />
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm Action" CssClass="btn btn-success" Visible="false" OnClick="btnConfirm_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" Visible="false" OnClick="btnCancel_Click" />
            </div>
        </div>

        <!-- GridView -->
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
            SelectCommand="SELECT * FROM [tblRegisteredUsers]">
        </asp:SqlDataSource>

        <asp:GridView ID="GridView1" runat="server" 
            AllowPaging="True" 
            AutoGenerateColumns="False"
            DataKeyNames="username" 
            DataSourceID="SqlDataSource1"
            OnRowDataBound="GridView1_RowDataBound"
            OnPageIndexChanging="GridView1_PageIndexChanging"
            CssClass="table table-bordered table-hover"
            BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" 
            CellPadding="4" CellSpacing="2" ForeColor="Black">

            <Columns>
                <asp:TemplateField HeaderText="Select" Visible="false">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                    <HeaderStyle Width="40px" />
                </asp:TemplateField>

                <asp:BoundField DataField="RID" HeaderText="RID">
                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="username" HeaderText="Username">
                    <ItemStyle Width="150px" />
                </asp:BoundField>

                <asp:BoundField DataField="fullName" HeaderText="Full Name">
                    <ItemStyle Width="180px" />
                </asp:BoundField>

                <asp:BoundField DataField="emailAddress" HeaderText="Email Address">
                    <ItemStyle Width="220px" />
                </asp:BoundField>

                <asp:BoundField DataField="age" HeaderText="Age">
                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="gender" HeaderText="Gender">
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="country" HeaderText="Country">
                    <ItemStyle Width="120px" />
                </asp:BoundField>

                <asp:ImageField DataImageUrlField="picture" HeaderText="Picture">
                    <ControlStyle Width="80px" Height="80px" />
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                </asp:ImageField>

                <asp:BoundField DataField="dateRegister" HeaderText="Date Register">
                    <ItemStyle Width="150px" />
                </asp:BoundField>
            </Columns>

            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="#007bff" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
    </div>
</asp:Content>
