<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ViewMessage.aspx.cs" Inherits="SciVerse_G12.ViewMessage" %>
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
                        </div>
                        <div class="message-actions">
                            <asp:CheckBox ID="chkReviewed" runat="server" AutoPostBack="true" OnCheckedChanged="chkReviewed_CheckedChanged" />
                            <asp:HiddenField ID="hdnCheckedState" runat="server" Value='<%# Eval("CID") + "|" + (IsChecked(Eval("CID").ToString()) ? "1" : "0") %>' />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                               ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                               SelectCommand="SELECT * FROM [tblContactUs]"></asp:SqlDataSource>
        </div>



</asp:Content>
