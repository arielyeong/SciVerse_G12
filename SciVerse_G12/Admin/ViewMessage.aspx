<%@ Page Title="View Messages" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ViewMessage.aspx.cs" Inherits="SciVerse_G12.ViewMessage" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href='<%= ResolveUrl("~/Styles/Admin.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-container">
        <h1 class="admin-title"><i class="fas fa-envelope-open-text me-2"></i>User Messages</h1>

        <asp:Label ID="lblNoMessages" runat="server" Text="No messages found." CssClass="no-messages" Visible="false"></asp:Label>

        <asp:Repeater ID="rptMessages" runat="server">
            <ItemTemplate>
                <div class="message-card shadow-sm">
                    <div class="message-left">
                        <h5 class="message-name">
                            <i class="fas fa-user me-2 text-primary"></i><%# Eval("contactName") %>
                        </h5>
                        <p class="message-email"><i class="fas fa-envelope me-2 text-secondary"></i><%# Eval("contactEmail") %></p>
                        <p class="message-body"><i class="fas fa-comment-dots me-2 text-info"></i><%# Eval("contactMessage") %></p>

                        <div class="message-meta mt-2">
                            <small><strong>Full Name:</strong> <%# Eval("fullName") %></small><br />
                            <small><strong>User RID:</strong> <%# Eval("userRID") %></small>
                        </div>
                    </div>

                    <div class="message-right">
                        <asp:HiddenField ID="hdnCID" runat="server" Value='<%# Eval("CID") %>' />
                        <div class='<%# Convert.ToBoolean(Eval("isReviewed")) ? "reviewed-badge" : "reviewed-badge unchecked" %>'>
                            <asp:CheckBox ID="chkReviewed" runat="server" AutoPostBack="true"
                                Checked='<%# Convert.ToBoolean(Eval("isReviewed")) %>'
                                OnCheckedChanged="chkReviewed_CheckedChanged" />
                            <span><%# Convert.ToBoolean(Eval("isReviewed")) ? "Reviewed" : "Pending" %></span>
                        </div>
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
                    c.isReviewed,
                    c.RID, 
                    r.fullName, 
                    r.RID AS userRID
                FROM tblContactUs c
                LEFT JOIN tblRegisteredUsers r ON c.RID = r.RID"
            UpdateCommand="UPDATE tblContactUs SET isReviewed = @isReviewed WHERE CID = @CID">
            <UpdateParameters>
                <asp:Parameter Name="isReviewed" Type="Boolean" />
                <asp:Parameter Name="CID" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
