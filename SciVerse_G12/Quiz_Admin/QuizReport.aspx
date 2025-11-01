<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="QuizReport.aspx.cs" Inherits="SciVerse_G12.Quiz_Admin.QuizReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
 
    <!-- External CSS in <head> -->
      <link rel="stylesheet" href='<%= ResolveUrl("~/Styles/AdminQuiz.css?v=" + DateTime.Now.Ticks) %>' />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* ===== Pretty KPI cards ===== */ 
        .kpi-card { 
            position: relative; 
            border-radius: 16px; 
            padding: 20px 18px; 
            background: linear-gradient(180deg, #ffffffEE, #ffffff); 
            border: 1px solid #E7ECF7; 
            box-shadow: 0 6px 20px rgba(15, 23, 42, 0.06); 
            transition: transform .12s ease, box-shadow .12s ease; 
        } 
        
        .kpi-card::before { 
            content:""; 
            position:absolute; 
            left:14px; 
            top:10px; 
            right:14px; 
            height:6px; 
            border-radius: 999px; 
            background: linear-gradient(90deg, #86B6F6, #B4D4FF, #86E5FF); 
            opacity: .85; 

        } .kpi-card:hover { 
              transform: translateY(-1px); 
              box-shadow: 0 10px 28px rgba(15, 23, 42, 0.10); } 
          .kpi-title { 
              display:block; 
              margin-top:18px; /* push below the accent bar */ 
              font-weight:800; 
              letter-spacing:.2px; 
              color:#2b3340; 
              text-align:center; 

          } 
          .kpi-value { 
              display:block; 
              font-size:34px; 
              line-height:1.1; 
              font-weight:900; 
              margin-top:8px; 
              color:#0f172a; 
              text-align:center; 

          } 
          
          /* Table tweaks */ 
          .table thead th { 
              white-space:nowrap; 

          } 
          .card-wrap { 
              background: transparent; } /* Controls spacing */ 
        
            .filters .form-label { 
                font-weight:600; 
                color:#2b3340; 

            } 

    </style>
    
    <h1 class="text-center mb-4">Question Details</h1> 
    
    <!-- KPI cards --> 
    <div class="row g-3 mb-3"> 
        <div class="col-md-4"> 
            <div class="kpi-card text-center"> 
                <asp:Label ID="lblTotalTitle" runat="server" 
                    CssClass="kpi-title" Text="Total Quizzes Attempted">
                </asp:Label> 
                <asp:Label ID="lblTotalAttempts" runat="server" 
                    CssClass="kpi-value" Text="0"></asp:Label> 

            </div> </div> 
        <div class="col-md-4"> 
            <div class="kpi-card text-center"> 
                <asp:Label ID="lblAverageTitle" runat="server" 
                    CssClass="kpi-title" Text="Average Score">
                </asp:Label> 
                <asp:Label ID="lblAverageScore" runat="server" 
                    CssClass="kpi-value" Text="0%"></asp:Label> 
            </div> </div> <div class="col-md-4"> 
                <div class="kpi-card text-center"> 
                    <asp:Label ID="lblPassTitle" runat="server" CssClass="kpi-title" Text="Pass Rate"></asp:Label> 
                    <asp:Label ID="lblPassRate" runat="server" CssClass="kpi-value" Text="0%"></asp:Label> </div> </div> </div> 
    
    <!-- === Filter Form Row === --> <div class="row g-3 align-items-end mb-3"> 
        <div class="col-md-3"> 
            <asp:Label runat="server" AssociatedControlID="txtStudentId" CssClass="form-label" Text="Student ID"></asp:Label> <asp:TextBox ID="txtStudentId" runat="server" CssClass="form-control" placeholder="RID / userId" /> </div> 
        <div class="col-md-3"> 
            <asp:Label runat="server" AssociatedControlID="ddlQuiz" CssClass="form-label" Text="Quiz Title"></asp:Label> <asp:DropDownList ID="ddlQuiz" runat="server" CssClass="form-select" /> </div> <div class="col-md-2"> <asp:Label runat="server" AssociatedControlID="ddlChapter" CssClass="form-label" Text="Chapter"></asp:Label> <asp:DropDownList ID="ddlChapter" runat="server" CssClass="form-select" /> </div> 
        <div class="col-md-2"> <asp:Label runat="server" AssociatedControlID="txtDate" CssClass="form-label" Text="Date"></asp:Label> <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" OnTextChanged="txtDate_TextChanged" /> </div> <div class="col-md-2 d-flex flex-column gap-2"> <asp:Button ID="btnFilter" runat="server" Text="Apply" CssClass="btn btn-primary w-100" OnClick="btnFilter_Click" /> 
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary w-100" OnClick="btnClear_Click" /> <asp:Button ID="btnUserReport" runat="server" Text="Overall User Report" CssClass="btn btn-outline-success w-100" OnClick="btnUserReport_Click" /> <asp:Button ID="Button1" runat="server" Text="Export User Report (CSV)" CssClass="btn btn-outline-primary w-100" OnClick="btnExportUserReport_Click" /> </div> </div> 
    
    <!-- === Results Table === --> <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="True" CssClass="table table-bordered table-hover" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvReport_PageIndexChanging"> <EmptyDataTemplate> 
        <div class="alert alert-info m-2"> No records found for the selected filters. </div> </EmptyDataTemplate> </asp:GridView> </asp:Content>