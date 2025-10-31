<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizResultPage.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizResultPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root { --border:#e5e7eb; --muted:#6b7280; }
        .wrap{ max-width:980px; margin:20px auto 40px; }
        .title{ font-size:24px; font-weight:800; margin-bottom:10px; }
        .back { display:inline-flex; align-items:center; gap:6px; padding:8px 12px;
                border:1px solid var(--border); border-radius:10px; background:#fff; cursor:pointer; }
        .sub{ color:var(--muted); margin:14px 0; }
        table{ width:100%; border-collapse:collapse; }
        th, td{ border:1px solid var(--border); padding:10px 12px; }
        th{ background:#f8f9fb; text-align:left; font-weight:600; }
        .action a, .action input[type=submit]{ border:1px solid var(--border); padding:6px 10px;
            border-radius:8px; background:#fff; cursor:pointer; font-size:14px; }
        .empty{ padding:16px; border:1px dashed var(--border); border-radius:12px; text-align:center; color:#666; }
    </style>
    <div class="wrap">
        <button type="button" class="back" onclick="history.back()">← Back</button>

        <div class="title"><asp:Literal ID="litTitle" runat="server" Text="Results" /></div>
        <div class="sub">Attempt History</div>

        <asp:PlaceHolder ID="phTable" runat="server" Visible="false">
            <asp:Repeater ID="repAttempts" runat="server" OnItemCommand="repAttempts_ItemCommand">
                <HeaderTemplate>
                    <table>
                        <thead>
                            <tr>
                                <th style="width:120px;">Attempt #</th>
                                <th style="width:160px;">Score</th>
                                <th style="width:220px;">Date</th>
                                <th class="action">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("AttemptNo") %></td>
                        <td><%# Eval("ScoreText") %></td>
                        <td><%# Eval("AttemptDate","{0:dd/MM/yyyy}") %></td>
                        <td class="action">
                            <asp:LinkButton ID="btnView" runat="server" Text="View Details"
                                CommandName="view" CommandArgument='<%# Eval("AttemptID") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </asp:PlaceHolder>

        <asp:Literal ID="litEmpty" runat="server" />
    </div>
</asp:Content>
