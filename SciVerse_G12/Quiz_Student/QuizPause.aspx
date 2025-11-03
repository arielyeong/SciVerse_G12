<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizPause.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizPause" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
    html, body {
      height: 100%;
      margin: 0;
      overflow: hidden;            /* no scroll on tall screens */
    }

    .pause-wrap {                  
      height: 100vh;
    }

    /* Hide site nav/footer only on this page */
    .navbar .menu-toggle,
    .navbar .navbar-toggler,
    .navbar .navbar-nav{
      display: none !important;
    }
    footer,
    hr {
        display: none !important;
    }
  
     .pause-card {
        position: fixed;            /* <-- anchor to viewport */
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        width: 100%;
        max-width: 720px;
        background: #fff;
        border-radius: 18px;
        box-shadow: 0 10px 40px rgba(0,0,0,.15);
        text-align: center;
        padding: 40px 25px;
        margin: 0;                  /* no layout margins */
    }

    .pause-title { font-size:26px; font-weight:800; margin-bottom:6px; color:#111827; }
    .pause-sub   { color:#6b7280; margin-bottom:18px; font-weight:600; }
    .count-pill  { display:inline-block; padding:8px 16px; border:1px solid #e5e7eb;
                   border-radius:999px; font-weight:800; background:#fff; margin-bottom:12px; }

    .capsule-track { position:relative; height:12px; border-radius:999px; background:#e5e7eb;
                     overflow:hidden; max-width:520px; margin:10px auto; }
    .capsule-fill  { position:absolute; left:0; top:0; bottom:0; width:0%;
                     background:linear-gradient(90deg,#3b82f6,#60a5fa); transition:width .4s ease; }
    .capsule-legend { display:flex; justify-content:space-between; max-width:520px; margin:8px auto 20px;
                      color:#6b7280; font-size:14px; font-weight:600; }

    .btn-row { display:flex; gap:16px; justify-content:center; flex-wrap:wrap; margin-top:20px; }
    .btn { min-width:160px; height:48px; border-radius:12px; font-weight:800; cursor:pointer; border:none;
           transition:transform .15s, box-shadow .15s; }
    .btn:hover { transform:translateY(-2px); box-shadow:0 8px 20px rgba(0,0,0,.1); }

    .btn-danger  { background:#fee2e2; color:#b91c1c; }
    .btn-danger:hover  { background:#fecaca; }
    .btn-primary { background:#2563eb; color:#fff; }
    .btn-primary:hover { background:#1d4ed8; }

    /* Fallback: on very short screens, allow scrolling instead of clipping */
    @media (max-height: 640px) {
      html, body { overflow: auto; }
      .pause-card { position: static; transform: none; margin: 40px auto; }
      .pause-wrap { height: auto; display: block; padding: 16px; }
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pause-card">
      <div class="pause-title">Quiz Paused</div>
      <div class="pause-sub">
        You are on question <strong><asp:Literal ID="litOn" runat="server" /></strong>
        of <strong><asp:Literal ID="litTotal" runat="server" /></strong>.
      </div>

      <div class="count-pill"><asp:Literal ID="litCounter" runat="server" /></div>

      <div class="capsule-track">
        <div id="capsuleFill" runat="server" class="capsule-fill"></div>
      </div>
      <div class="capsule-legend"><span>Start</span><span>End</span></div>

      <div class="btn-row">
        <asp:Button ID="btnSaveExit" runat="server" Text="Exit Quiz"
                    CssClass="btn btn-danger" OnClick="btnSaveExit_Click" />
        <asp:Button ID="btnResume" runat="server" Text="Resume Quiz"
                    CssClass="btn btn-primary" OnClick="btnResume_Click" />
      </div>
    </div>
</asp:Content>