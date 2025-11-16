<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizHistory.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizHistory" %>
<%@ Register Assembly="System.Web.DataVisualization"
    Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
    :root{
        --beige-50:#fbf7f1;
        --beige-100:#f4ecdf;
        --beige-200:#eee2cc;
        --ink:#1c2833;
        --muted:#62707b;
        --accent:#8b7aa7;
        --accent-2:#c89a7d;
        --ok:#2a9d8f;
        --ok-bg:#e9fbf8;
        --shadow:0 10px 28px rgba(28,40,51,.12);
        --radius:18px;
    }

   /* Hide site nav/footer only on this page */
    .quiz-page .navbar .menu-toggle,
    .quiz-page .navbar .navbar-toggler,
    .navbar .navbar-nav{
    display: none !important;
    }
    footer,
    hr {
        display: none !important;
    }
    /* kill the default HR gap if master injects it */
    .body-content>hr{display:none!important;}

    /* single, fixed background for the whole viewport */
    html, body{height:100%;}
    body::before{
      content:"";
      position:fixed; inset:0; z-index:-1;
      background:
        radial-gradient(900px 520px at 12% 10%, rgba(248,240,229,.95) 0%, rgba(248,240,229,0) 60%),
        radial-gradient(800px 460px at 88% 92%, rgba(255,236,214,.55) 0%, rgba(255,236,214,0) 55%),
        var(--beige-50);
      background-attachment:fixed,fixed,fixed;
    }
    /* ensure all wrappers are transparent so the gradient shows through */
    .body-content,
    .content-wrapper,
    .page-wrap,
    form,
    footer,
    .footer{ background:transparent!important; }


        /* scope to this page only (optional) */
    body.quiz-history-page .main-content-wrapper .card:first-child {
        margin-top: 24px !important;   /* about 1.5rem space */
    }

    /* remove the extra gap you added earlier */
    body.quiz-history-page,
    body.quiz-history-page .body-content,
    body.quiz-history-page .card:first-child {
    margin-top: 0 !important;
    padding-top: 0 !important;
    }

    /* also ensure the very first child inside body-content has no top margin */
    body.quiz-history-page .body-content > *:first-child {
    margin-top: 0 !important;
    }

    .card {
        margin-top: 10px !important; /* slightly smaller spacing between cards */
    }
    .kpi-wrap{display:grid;grid-template-columns:repeat(3,1fr);gap:18px;margin:18px 0 12px}
    .kpi{background:#fff;border:1px solid #e8e8e8;border-radius:16px;padding:18px;text-align:center;box-shadow:0 8px 24px rgba(0,0,0,.05)}
    .kpi h4{margin:0 0 6px;font-weight:800}
    .kpi .num{font-size:40px;font-weight:900}
    .filters{display:grid;grid-template-columns:1fr 1fr 1fr auto;gap:12px;margin:14px 0}
    .card{background:#fff;border:1px solid #e8e8e8;border-radius:16px;padding:18px;box-shadow:0 8px 24px rgba(0,0,0,.05)}
    .charts{display:grid;grid-template-columns:2fr 1fr;gap:18px;margin-bottom:20px}
    .btn{min-width:120px;height:40px;border:none;border-radius:10px;font-weight:800;cursor:pointer}
    .btn-primary{background:#2563eb;color:#fff}
    .btn-ghost{background:#f3f4f6}

  </style>
    <script>
  // tag the body so the overrides only apply on this page
  document.addEventListener('DOMContentLoaded', function () {
    document.body.classList.add('quiz-history-page');
  });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
    <h2 style="text-align:center;margin:0 0 6px;font-weight:900">My Quiz Results</h2>
    <p style="text-align:center;color:#6b7280;margin:0 0 14px">
      A concise overview of your quiz performance.
    </p>

    <!-- KPI Summary -->
    <div class="kpi-wrap">
      <div class="kpi">
        <h4>Total Quizzes Attempted</h4>
        <div class="num"><asp:Literal ID="litTotalAttempts" runat="server" /></div>
      </div>
      <div class="kpi">
        <h4>Average Score</h4>
        <div class="num"><asp:Literal ID="litAvgPct" runat="server" />%</div>
      </div>
      <div class="kpi">
        <h4>Quizzes Passed</h4>
        <div class="num"><asp:Literal ID="litPassed" runat="server" /></div>
      </div>
    </div>

    <!-- Filters -->
    <div class="filters">
      <asp:DropDownList ID="ddlQuiz" runat="server" AppendDataBoundItems="true" CssClass="textbox">
        <asp:ListItem Text="All Quizzes" Value="" />
      </asp:DropDownList>

      <asp:DropDownList ID="ddlChapter" runat="server" AppendDataBoundItems="true" CssClass="textbox">
        <asp:ListItem Text="All Chapters" Value="" />
      </asp:DropDownList>

      <asp:TextBox ID="dtDate" runat="server" TextMode="Date" CssClass="textbox" />

      <div style="display:flex;gap:8px">
        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-ghost" OnClick="btnClear_Click" />
      </div>
    </div>

    <!-- Charts -->
    <div class="charts">
      <!-- Trend Chart -->
      <asp:Chart ID="chartTrend" runat="server" Width="800" Height="300">
        <ChartAreas>
          <asp:ChartArea Name="Area1" />
        </ChartAreas>
        <Series>
          <asp:Series Name="Scores" ChartType="Line" BorderWidth="3" ChartArea="Area1" />
        </Series>
        <Legends>
          <asp:Legend Name="Legend1" Enabled="false" />
        </Legends>
      </asp:Chart>

      <!-- Pass / Fail Chart -->
      <asp:Chart ID="chartPassFail" runat="server" Width="400" Height="300">
        <ChartAreas>
          <asp:ChartArea Name="AreaPF" />
        </ChartAreas>
        <Series>
          <asp:Series Name="PF" ChartType="Doughnut" IsValueShownAsLabel="true" ChartArea="AreaPF" />
        </Series>
        <Legends>
          <asp:Legend Name="Legend2" Docking="Bottom" />
        </Legends>
      </asp:Chart>
    </div>

    <!-- Overall Performance by Chapter -->
    <div class="card">
      <h3 style="text-align:center;margin-bottom:10px;font-weight:800">Overall Performance by Chapter</h3>
      <asp:Chart ID="chartChapterPerf" runat="server" Width="1000" Height="350">
        <ChartAreas>
          <asp:ChartArea Name="AreaChapter" />
        </ChartAreas>
        <Series>
          <asp:Series Name="Chapters" ChartType="Column" ChartArea="AreaChapter" />
        </Series>
        <Legends>
          <asp:Legend Name="LegendChapter" Docking="Bottom" />
        </Legends>
      </asp:Chart>
    </div>

    <!-- Table -->
    <div class="card" style="margin-top:18px">
      <asp:GridView ID="gvAttempts" runat="server"
        AutoGenerateColumns="False" CssClass="table"
        GridLines="None" CellPadding="8" BorderStyle="None"
        AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvAttempts_PageIndexChanging">
        <Columns>
          <asp:BoundField DataField="Title" HeaderText="Quiz Title" />
          <asp:BoundField DataField="Chapter" HeaderText="Chapter" />
          <asp:BoundField DataField="AttemptNo" HeaderText="Attempt #" />
          <asp:BoundField DataField="ScoreText" HeaderText="Score" />
          <asp:BoundField DataField="AttemptDate" HeaderText="Attempt Date" DataFormatString="{0:dd/MM/yyyy}" />
          <asp:BoundField DataField="DurationText" HeaderText="Time Taken" />
          <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
              <asp:HyperLink runat="server" Text="View Details"
                NavigateUrl='<%# Eval("AttemptID","~/Quiz_Student/QuizSummary.aspx?attemptId={0}") %>' />
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
      </asp:GridView>
    </div>
  </div>
</asp:Content>