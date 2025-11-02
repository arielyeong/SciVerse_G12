<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Quiz.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.Quiz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
/* Hide site nav/footer only on this page */
    .quiz-page .navbar .navbar-nav { display: none; }
    .quiz-page .footer { display: none; }
    .body-content { margin-top: 8px !important; }

    /* Lock page to one screen */
    html.quiz-page, body.quiz-page { overflow: hidden; height: 100%; }

    /* Shell and card */
    .quiz-shell{
      min-height:90vh; background:#f7f7f8; padding:24px 16px;
      display:flex; align-items:center; justify-content:center;
    }
    .quiz-frame{
      width:100%; max-width:900px; background:#fff; border:1px solid #eaeaea;
      border-radius:18px; box-shadow:0 10px 30px rgba(0,0,0,.06);
      padding:40px 40px 50px; display:flex; flex-direction:column; justify-content:center;
    }

    /* Top pills (timer + index) */
    .quiz-top{ display:flex; gap:12px; justify-content:center; align-items:center; margin-bottom:30px; }
    .quiz-pill{ display:inline-block; padding:8px 16px; border-radius:20px; border:1px solid #eee; background:#fafafa; color:#333; font-weight:600; font-size:15px; }

    /* Question */
    .quiz-q{ font-size:22px; font-weight:700; text-align:center; margin:0 0 40px; color:#1a1a1a; }
    /* Error message */
    .quiz-error{
      margin: 6px 0 0;
      color: #b42318;
      background:#fee4e2;
      border:1px solid #fecdca;
      padding:10px 12px;
      border-radius:10px;
      font-weight:600;
      display:none; /* shown when needed */
    }
    /* ====== MCQ styled-as-buttons (RadioButtonList) ====== */
    /* Layout grid */
    .rbl-mcq{
      display:grid;
      grid-template-columns:repeat(auto-fit, minmax(220px, 1fr));
      gap:18px;
      justify-items:stretch;
      margin-bottom:40px;
    }

    /* choose one of these per question via CssClass: */
.layout-1x4 { grid-template-columns: repeat(4, 1fr); }  /* 1 row, 4 columns */
.layout-2x2 { grid-template-columns: repeat(2, 1fr); }  /* 2 rows, 2 columns */
    /* Hide native radios */
    .rbl-mcq input[type="radio"]{ position:absolute; opacity:0; pointer-events:none; }
    /* Make each label look like a button card */
    .rbl-mcq label{
      display:block;
      width:100%;
      border:2px solid #e2e2e2;
      border-radius:14px;
      padding:16px 18px;
      text-align:center;
      font-weight:600;
      cursor:pointer;
      background:#fff;
      transition:all .25s ease;
      user-select:none;
    }
    .rbl-mcq label:hover{
      border-color:#0d6efd;
      background:#eef5ff;
      box-shadow:0 6px 15px rgba(13,110,253,.12);
      transform:translateY(-1px);
    }
    /* Selected state (when radio is checked) */
    .rbl-mcq input[type="radio"]:checked + label{
      background:#0d6efd; color:#fff; border-color:#0d6efd;
      box-shadow:0 6px 18px rgba(13,110,253,.28);
    }

    /* ====== TRUE/FALSE styled buttons (RadioButtonList) ====== */
    .tf-row{ display:flex; gap:30px; justify-content:center; margin-bottom:40px; }
    .rbl-tf input{ position:absolute; opacity:0; pointer-events:none; }
    .rbl-tf label{
      display:inline-block; min-width:180px; text-align:center;
      padding:16px 20px; border:2px solid #ccc; border-radius:14px;
      font-weight:700; font-size:18px; cursor:pointer; transition:all .25s ease; background:#fff;
    }
    .rbl-tf label:hover{ border-color:#0d6efd; background:#eef5ff; }
    .rbl-tf input:checked + label{
      background:#0d6efd; color:#fff; border-color:#0d6efd;
      box-shadow:0 6px 18px rgba(13,110,253,.28);
    }

    /* ====== Fill-in-the-blank ====== */
    .fib-wrap{ display:flex; justify-content:center; margin-bottom:40px; }
    .fib-wrap input{ max-width:520px; height:48px; border-radius:10px; font-size:18px; text-align:center; border:1px solid #ccc; }

    /* Next/Submit */
    .quiz-actions{ display:flex; justify-content:flex-end; margin-top:20px; }
    .quiz-actions .btn{ min-width:160px; height:48px; border-radius:12px; font-weight:600; font-size:16px; transition:all .25s ease; }
    .quiz-actions .btn:hover{ transform:translateY(-2px); box-shadow:0 6px 15px rgba(0,0,0,.1); }

    /* Small screens / short viewports */
    @media (max-height: 640px){
      .quiz-shell{ padding:10px; }
      .quiz-frame{ padding:24px; }
      .quiz-q{ margin-bottom:28px; }
      .rbl-mcq{ margin-bottom:28px; }
      .tf-row{ margin-bottom:28px; }
    }
  </style>

  <script type="text/javascript">
      // Add a class so CSS can hide navbar/footer and lock scroll
      document.addEventListener('DOMContentLoaded', function () {
          document.documentElement.classList.add('quiz-page');
          document.body.classList.add('quiz-page');
      });

      // Timer (unchanged, just wired to your hidden fields)
      function startTimerIfNeeded() {
          var isTimed = document.getElementById("<%= hfIsTimed.ClientID %>").value;
        if (isTimed !== "1") return;

        var remainEl = document.getElementById("<%= hfRemainSeconds.ClientID %>");
      var label    = document.getElementById("<%= lblTimer.ClientID %>");
      var postback = "<%= btnAutoSubmit.UniqueID %>";
          var remain = parseInt(remainEl.value || "0", 10);

          function tick() {
              var m = Math.floor(remain / 60), s = remain % 60;
              label.innerText = (m < 10 ? "0" + m : m) + ":" + (s < 10 ? "0" + s : s);
              remain--; remainEl.value = remain.toString();
              if (remain < 0) { remainEl.value = "0"; __doPostBack(postback, ""); return; }
              setTimeout(tick, 1000);
          }
          tick();
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

  <div class="quiz-shell">
    <div class="quiz-frame">

      <!-- top bar -->
      <div class="quiz-top">
        <asp:Label ID="lblTimer" runat="server" CssClass="quiz-pill" Visible="False" Width="126px" />
        <asp:Label ID="lblIndex" runat="server" CssClass="quiz-pill" Width="104px" />
      </div>

      <!-- question text -->
      <asp:Label ID="lblQuestionText" runat="server" CssClass="quiz-q"></asp:Label>

      <!-- MCQ -->
      <asp:Panel ID="pMcq" runat="server" Visible="false">
        <asp:RadioButtonList ID="rblMcq" runat="server"
          CssClass="rbl-mcq"
          RepeatLayout="Flow"
          RepeatDirection="Vertical" />
      </asp:Panel>

      <!-- True / False -->
      <asp:Panel ID="pTf" runat="server" Visible="false">
        <div class="tf-row">
          <asp:RadioButtonList ID="rblTf" runat="server" CssClass="rbl-tf" RepeatLayout="Flow">
            <Items>
              <asp:ListItem Text="TRUE"  Value="True"  />
              <asp:ListItem Text="FALSE" Value="False" />
            </Items>
          </asp:RadioButtonList>
        </div>
      </asp:Panel>

      <!-- Fill in the blank -->
      <asp:Panel ID="pFib" runat="server" Visible="false">
        <div class="fib-wrap">
          <asp:TextBox ID="txtFib" runat="server" Width="920px" />
        </div>
        <asp:RequiredFieldValidator ID="rfvFib" runat="server"
          ControlToValidate="txtFib"
          Display="Dynamic"
          ErrorMessage="Please enter your answer."
          ForeColor="#b42318" />
      </asp:Panel>

      <!-- validation message -->
      <asp:Label ID="lblError" runat="server" CssClass="quiz-error" EnableViewState="false" />

      <!-- actions -->
      <div class="quiz-actions">
        <asp:Button ID="btnNext" runat="server" Text="Next >" CssClass="btn btn-primary"
                    OnClick="btnNext_Click" Width="111px" />
      </div>

      <!-- hidden submit for auto-timeout -->
      <asp:Button ID="btnAutoSubmit" runat="server" Text="autosubmit"
                  Style="display:none" OnClick="btnAutoSubmit_Click" />
    </div>
  </div>
</asp:Content>