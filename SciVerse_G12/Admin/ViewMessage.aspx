<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ViewMessage.aspx.cs" Inherits="SciVerse_G12.ViewMessage" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
     <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/Admin.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Main Content -->
        <div class="admin-content" id="contentArea">
        <h1>Messages by User</h1>

        <asp:Repeater ID="rptMessages" runat="server" OnItemDataBound="rptMessages_ItemDataBound">
            <ItemTemplate>
                <div class="message-card">
                    <div class="message-content">
                        <p><strong>Name:</strong> <%# Eval("contactName") %></p>
                        <p><strong>Email:</strong> <%# Eval("contactEmail") %></p>
                        <p><strong>Message:</strong> <%# Eval("contactMessage") %></p>

                        <div class="message-meta">
                            <p><strong>Full Name:</strong> <%# Eval("fullName") %></p>
                            <p><strong>User RID:</strong> <%# Eval("userRID") %></p>
                        </div>
                    </div>
                    <div class="message-actions">
                        <asp:CheckBox ID="chkReviewed" runat="server" AutoPostBack="true" OnCheckedChanged="chkReviewed_CheckedChanged" />
                        <asp:HiddenField ID="hdnCheckedState" runat="server" 
                                         Value='<%# Eval("CID") + "|" + (IsChecked(Eval("CID").ToString()) ? "1" : "0") %>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="
                SELECT 
                    c.CID, 
                    c.contactName, 
                    c.contactEmail, 
                    c.contactMessage, 
                    c.RID, 
                    r.fullName, 
                    r.RID AS userRID
                FROM tblContactUs c
                LEFT JOIN tblRegisteredUsers r 
                    ON c.RID = CAST(r.RID AS NVARCHAR(10))">
        </asp:SqlDataSource>
    </div>



</asp:Content>
