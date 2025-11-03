<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Quiz.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.Quiz" %>
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

  /* pull the content closer to the navbar */
  .quiz-page .navbar{ margin-bottom:6px!important; }
  .quiz-page .body-content{ padding-top:6px!important; }
  .quiz-page .footer{ display:none; }

  /* ===== Card ===== */
  .quiz-frame{
    margin:5px auto 20px;             /* tighter gaps above/below */
    padding:20px 24px 28px;            /* tighter inner padding */
    width:100%;
    max-width:980px;
    background:#fff;
    border:1px solid #eaeaea;
    border-radius:18px;
    box-shadow:0 10px 30px rgba(0,0,0,.06);
    display:block;
  }

  .quiz-top{
    position: relative;                /* enables absolute child */
    display: flex;
    justify-content: center;           /* ⬅️ center timer + index */
    align-items: center;
    margin-bottom: 14px;
    width: 100%;               /* slightly smaller */
  }
  .quiz-left-group{
    display: flex;
    align-items: center;
    gap: 12px;
  }
  /* pause button (in the top row) */
  .quiz-pause{
    position: absolute;
    right: 0;                          /* ⬅️ stick to right */
    top: 50%;
    transform: translateY(-50%);       /* vertical center */
    padding: 10px 18px;
    border-radius: 12px;
    background: #2563eb;
    color: #fff;
    border: none;
    font-weight: 800;
    cursor: pointer;
    transition: background .2s ease;
  }
  .quiz-pause:hover{ background:#1e4ed8; }

  /* pill styles */
  .quiz-pill{
    display:inline-flex; align-items:center; justify-content:center;
    min-width:110px; height:36px; padding:0 14px;
    border:1px solid #eee; border-radius:20px; background:#fafafa;
    font-weight:600; font-size:15px; color:#333;
  }
  .quiz-pill.time{
    gap:6px; font-size:17px; font-weight:700; color:#333;
  }
  .timer-icon{ font-size:18px; color:#2563eb; }

  /* question */
  .quiz-q{
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
    font-size: 28px;
    font-weight: 800;
    line-height: 1.35;
    color: #111;
    margin: 20px auto 40px;   /* even space top and bottom */
    min-height: 100px;        /* keeps vertical alignment neat */
    max-width: 900px;
  }
  .quiz-pane{ width:100%; }

  /* MCQ list */
  .rbl-mcq{
    max-width:640px; width:100%;
    margin:5px auto 22px;
    display:block;
  }
  .rbl-mcq input[type="radio"]{ position:absolute; opacity:0; pointer-events:none; }
  .rbl-mcq label{
    display:block; width:100%; max-width:640px; margin:0 auto 10px;
    text-align:center; padding:16px 20px;
    border:2px solid #e2e2e2; border-radius:14px; background:#fff;
    font-size:16px; line-height:1.4; cursor:pointer; transition:all .25s ease;
  }
  .rbl-mcq label:hover{ border-color:#0d6efd; background:#eef5ff; }
  .rbl-mcq input[type="radio"]:checked + label{
    background:#0d6efd; color:#fff; border-color:#0d6efd;
    box-shadow:0 6px 18px rgba(13,110,253,.28);
  }

  /* True / False */
  .rbl-tf{
    width:100%; max-width:560px; margin:24px auto 28px;
    display:flex; gap:20px; justify-items:center;
  }
  .rbl-tf > *{ justify-self:center; width:100%; max-width:360px; }
  .rbl-tf input{ position:absolute; opacity:0; pointer-events:none; }
  .rbl-tf label{
    flex:1; min-width:180px; max-width:260px; text-align:center;
    padding:18px 20px; border:2px solid #ccc; border-radius:14px;
    font-weight:800; font-size:18px; cursor:pointer; background:#fff;
    transition:all .25s ease; display:flex; align-items:center; justify-content:center;
  }
  .rbl-tf label:hover{ border-color:#0d6efd; background:#eef5ff; }
  .rbl-tf input:checked + label{ background:#0d6efd; color:#fff; border-color:#0d6efd; }

  /* Fill in the blank */
  .fib-wrap{ display:flex; justify-content:center; margin:24px auto 30px; }
  .fib-wrap input{
    width:100%; max-width:520px; height:48px; border:1px solid #ccc; border-radius:10px;
    text-align:center; padding:8px 14px; font-size:18px; font-weight:500;
  }
  .fib-wrap input:focus{ border-color:#2563eb; box-shadow:0 0 0 3px rgba(37,99,235,.15); }
  input:-webkit-autofill{
    -webkit-text-fill-color:#111!important;
    -webkit-box-shadow:0 0 0px 1000px #fff inset;
    transition:background-color 9999s ease-out 0s;
  }

  /* error */
  .quiz-error{
    max-width:720px; margin:16px auto 16px;
    color:#b42318; background:#fee4e2; border:1px solid #fecdca;
    padding:12px 14px; border-radius:10px; font-weight:700; text-align:center; display:none;
  }

  /* actions */
  .quiz-actions{ display:flex; justify-content:flex-end; margin-top:10px; }
  .quiz-actions .btn{ min-width:160px; height:48px; border-radius:12px; font-weight:800; font-size:16px; border:none; cursor:pointer; }
  .btn-primary{ background:#2563eb; color:#fff; }
  .btn-success{ background:#16a34a; color:#fff; }

  @media (max-height:640px){
    .quiz-frame{ padding:18px 20px 24px; }
    .quiz-q{ margin-bottom:24px; }
    .rbl-mcq{ margin-bottom:24px; }
  }
</style>
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            document.documentElement.classList.add('quiz-page');
            document.body.classList.add('quiz-page');
        });

        function startTimerIfNeeded() {
            var isTimed = document.getElementById("<%= hfIsTimed.ClientID %>").value;
        if (isTimed !== "1") return;

        var remainEl = document.getElementById("<%= hfRemainSeconds.ClientID %>");
      var label    = document.getElementById("<%= lblTimer.ClientID %>");
      var postback = "<%= btnAutoSubmit.UniqueID %>";
            var remain = parseInt(remainEl.value || "0", 10);

            (function tick() {
                var m = Math.floor(remain / 60), s = remain % 60;
                label.innerText = (m < 10 ? "0" + m : m) + ":" + (s < 10 ? "0" + s : s);
                remain--; remainEl.value = remain.toString();
                if (remain < 0) {
                    remainEl.value = "0";
                    __doPostBack(postback, ""); return;
                }
                setTimeout(tick, 1000);
            })();
        }
        window.onload = startTimerIfNeeded;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hidden fields -->
  <asp:HiddenField ID="hfAttemptID" runat="server" />
  <asp:HiddenField ID="hfQuizID"   runat="server" />
  <asp:HiddenField ID="hfIsTimed"  runat="server" />
  <asp:HiddenField ID="hfTotalSeconds" runat="server" />
  <asp:HiddenField ID="hfRemainSeconds" runat="server" />
  <asp:HiddenField ID="hfCurrentIdx"   runat="server" Value="0" />

    

    <div class="quiz-top">
        <div class="quiz-left-group">
            <div class="quiz-pill time" runat="server" id="timerWrap">
                <span class="timer-icon">⏱</span>
                <asp:Label ID="lblTimer" runat="server" Visible="True"></asp:Label>
            </div>
            <asp:Label ID="lblIndex" runat="server" CssClass="quiz-pill" />
            </div>

            <asp:Button ID="btnPause" runat="server"
                        Text="Pause"
                        CssClass="quiz-pause"
                        OnClick="btnPause_Click" />
        </div>
        
      <asp:Label ID="lblQuestionText" runat="server" CssClass="quiz-q"></asp:Label>

      <!-- MCQ -->
      <asp:Panel ID="pMcq" runat="server" Visible="false">
        <asp:RadioButtonList ID="rblMcq" runat="server"
          CssClass="rbl-mcq"
          RepeatLayout="Flow" RepeatDirection="Vertical" />
      </asp:Panel>

      <!-- TRUE/FALSE -->
      <asp:Panel ID="pTf" runat="server" Visible="false">
        <asp:RadioButtonList ID="rblTf" runat="server" CssClass="rbl-tf" RepeatLayout="Flow">
          <Items>
            <asp:ListItem Text="TRUE"  Value="True"  />
            <asp:ListItem Text="FALSE" Value="False" />
          </Items>
        </asp:RadioButtonList>
      </asp:Panel>

      <!-- FILL -->
      <asp:Panel ID="pFib" runat="server" Visible="false">
          <div class="fib-wrap">
            <asp:TextBox ID="txtFib" runat="server"
                 TextMode="SingleLine"
                 placeholder="Type your answer…"
                 EnableViewState="false"
                 AutoCompleteType="Disabled"
                 autocomplete="off"
                 autocorrect="off"
                 autocapitalize="off"
                 spellcheck="false" />
          </div>
        </asp:Panel>

      <asp:Label ID="lblError" runat="server" CssClass="quiz-error" EnableViewState="false" />

      <div class="quiz-actions">
        <asp:Button ID="btnNext" runat="server" Text="Next >"
                    CssClass="btn btn-primary" OnClick="btnNext_Click" />
      </div>

      <!-- hidden autosubmit for timeout -->
      <asp:Button ID="btnAutoSubmit" runat="server" Text="autosubmit"
                  Style="display:none" OnClick="btnAutoSubmit_Click" />
</asp:Content>
