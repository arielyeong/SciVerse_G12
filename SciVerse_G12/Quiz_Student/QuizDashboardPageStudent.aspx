<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuizDashboardPageStudent.aspx.cs" Inherits="SciVerse_G12.Quiz_Student.QuizDashboardPageStudent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
    .body-content {
        margin-top:10px !important;
    }
    /*.quiz-section {
        display: flex;
        flex-direction: column;
        align-items: center;
        text-align: center;
        width: 100%;
    }

    .quiz-section h1 {
        text-align: center;
        margin-bottom: 25px;
    }*/

    /* === Toolbar === */
    /*.quiz-toolbar {
        display: flex;
        align-items: center;
        justify-content: center;
        flex-wrap: nowrap;*/ /* everything stays on one line */
        /*gap: 14px;*/ /* space between each element */
        /*max-width: 1400px;
        margin: 0 auto 28px;
        padding: 0 20px;*/ /* small side padding for balance */
    /*}*/

    /* Textbox + Dropdown unified style */
    /*.quiz-toolbar .form-control {
        height: 44px;
        font-size: 1rem;
        border-radius: 6px;
        flex: 1 1 600px;*/     /* 🔹 wider search box */
        /*min-width: 400px;*/    /* 🔹 ensures it doesn’t shrink too small */
        /*max-width: 800px;*/    /* 🔹 allows some expansion */
    /*}

    .quiz-toolbar .form-select {
        height: 44px;
        font-size: 1rem;
        border-radius: 6px;
        flex: 0 0 220px;*/     /* 🔹 fixed, smaller width for filter dropdown */
    /*}*/

    /* Buttons (Search / Clear) */
    /*.quiz-toolbar .btn {
        height: 44px;
        line-height: 1.2;
        border-radius: 6px;
        white-space: nowrap;
        font-weight: 500;
    }*/

    /* 🔹 Make Search button wider */
    /*.quiz-toolbar .btn-primary {
        padding: 10px 35px;*/      /* increase horizontal size */
        /*min-width: 140px;
    }*/

    /* Optional: match Clear button width for symmetry */
    /*.quiz-toolbar .btn-secondary {
        min-width: 110px;
    }*/

    /* === Grid of Cards === */
    /*.quiz-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));*/ /* auto-fit centers neatly */
        /*gap: 24px;
        width: 100%;
        max-width: 1100px;
        margin: 12px auto 72px;
        justify-content: center;*/       /* centers the grid columns */
        /*align-items: stretch;
        padding-inline: clamp(10px, 3vw, 30px);*/ /* equal, responsive side padding */
        /*box-sizing: border-box;*/        /* ensures padding is counted evenly */

        /*display: grid;
        grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
        gap: 24px;
        width: 100%;
        max-width: 1100px;
        margin: 12px auto 72px;*/   /* 🔹 smaller top gap (was too big) */
        /*justify-content: center;*/  /* 🔹 centers grid content */
        /*align-items: stretch;
        padding: 0 20px;*/          /* 🔹 small side padding for balance */
    /*}*/

    /* === Quiz Card === */
    /*.quiz-card {
        border: 1px solid #ddd;
        border-radius: 10px;
        padding: 15px;
        background: #f9f9f9;
        display: flex;
        flex-direction: column;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        transition: transform 0.2s;
        margin: 0;
    }

    .quiz-card:hover {
        transform: translateY(-5px);
    }

    .quiz-card img {
        width: 100%;
        height: 160px;
        object-fit: cover;
        border-radius: 6px;
        display: block;
    }

    .quiz-card h3 {
        font-size: 1.2em;
        font-weight: bold;
        margin-top: 10px;
        margin-bottom: 5px;
    }

    .quiz-card p {
        color: #555;
        margin: 4px 0;
        line-height: 1.4;
        text-align: left;
    }

    .actions {
        margin-top: auto;
        display: flex;
        gap: 10px;
        justify-content: space-between;
    }

    .start-quiz {
        background: #4CAF50;
        color: #fff;
        border: none;
        border-radius: 6px;
        padding: 10px 14px;
    }

    .view-result {
        background: #008CBA;
        color: #fff;
        border: none;
        border-radius: 6px;
        padding: 10px 14px;
    }*/

    /* --- Small & medium screens: make toolbar wrap and push grid down --- */
    /*@media (max-width: 1024px){
      .quiz-toolbar{
        flex-wrap: wrap;*/          /* allow multiple rows */
        /*align-items: flex-start;*/  /* compute proper height when wrapped */
        /*row-gap: 12px;
        column-gap: 10px;
        margin-bottom: 20px;*/      /* space between toolbar and cards */
        /*padding: 0 12px;
      }*/

      /* Search goes full row */
      /*.quiz-toolbar .form-control{
        flex: 1 1 100%;
        max-width: 100%;
      }*/

      /* Buttons stay compact (side-by-side) */
      /*.quiz-toolbar .btn{
        flex: 0 0 auto;
      }*/

      /* Dropdown on its own full row (prevents collision with first card) */
      /*.quiz-toolbar .form-select{
        flex: 1 1 100%;
        max-width: 100%;
        width: 100%;
        display: block;
      }*/

      /* Grid starts after toolbar, one column on phones, two on tablets */
      /*.quiz-grid{
        clear: both;*/              /* hard stop: never overlap preceding content */
        /*margin-top: 12px;*/         /* ensure gap below toolbar */
        /*padding: 0 12px;
        grid-template-columns: repeat(2, 1fr);*/  /* tablet */
      /*}
    }*/

    /* Phones: single column */
    /*@media (max-width: 768px){
      .quiz-grid{
        grid-template-columns: 1fr;
        gap: 18px;
      }*/

      /* Optional: stack card action buttons nicely */
      /*.actions{ flex-direction: column; }
      .actions .start-quiz, .actions .view-result{ width: 100%; }
    }*/
      /* Global box-sizing fix */
*, *::before, *::after {
    box-sizing: border-box;
}

/* ============ QUIZ SECTION ============ */
.quiz-section {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
    width: 100%;
}

.quiz-section h1 {
    margin-bottom: 25px;
}

/* ============ TOOLBAR ============ */
.quiz-toolbar {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-wrap: nowrap;          /* stay on one line on desktop */
    gap: 14px;
    max-width: 1400px;
    margin: 0 auto 25px;
    padding: 0 20px;
    position: relative;
    z-index: 1;
}

/* Search textbox */
.quiz-toolbar .form-control {
    height: 44px;
    font-size: 1rem;
    border-radius: 6px;
    flex: 1 1 600px;            /* wider search box */
    min-width: 400px;
    max-width: 800px;
}

/* Dropdown filter */
.quiz-toolbar .form-select {
    height: 44px;
    font-size: 1rem;
    border-radius: 6px;
    flex: 0 0 220px;            /* compact fixed width */
}

/* Toolbar buttons */
.quiz-toolbar .btn {
    height: 44px;
    line-height: 1.2;
    border-radius: 6px;
    white-space: nowrap;
    font-weight: 500;
    cursor: pointer;
}

/* Primary (Search) */
.quiz-toolbar .btn-primary {
    background-color: #007bff;
    border: none;
    color: #fff;
    padding: 10px 35px;
    min-width: 140px;
    transition: background 0.2s ease;
}
.quiz-toolbar .btn-primary:hover {
    background-color: #0056b3;
}

/* Secondary (Clear) */
.quiz-toolbar .btn-secondary {
    background-color: #6c757d;
    border: none;
    color: #fff;
    min-width: 110px;
    transition: background 0.2s ease;
}
.quiz-toolbar .btn-secondary:hover {
    background-color: #5a6268;
}

/* ============ QUIZ GRID ============ */
.quiz-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 24px;
    width: 100%;
    max-width: 1100px;
    margin: 12px auto 72px;
    justify-content: center;
    align-items: stretch;
    padding-inline: clamp(10px, 3vw, 30px);
    clear: both;
    position: relative;
}

/* ============ QUIZ CARD ============ */
.quiz-grid .quiz-card {
    border: 1px solid #ddd;
    border-radius: 10px;
    padding: 15px;
    background: #f9f9f9;
    display: flex;
    flex-direction: column;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
    transition: transform 0.2s;
    margin: 0;

    /* override any flashcard styles elsewhere */
    width: auto !important;
    max-width: none !important;
    aspect-ratio: auto !important;
    align-items: stretch !important;
    justify-content: flex-start !important;
}
.quiz-grid .quiz-card:hover {
    transform: translateY(-5px);
}

/* Image */
.quiz-grid .quiz-card img {
    width: 100%;
    height: 160px;
    object-fit: cover;
    border-radius: 6px;
    display: block;
}

/* Text */
.quiz-grid .quiz-card h3 {
    font-size: 1.2em;
    font-weight: bold;
    margin-top: 10px;
    margin-bottom: 5px;
    text-align: left;
}
.quiz-grid .quiz-card p {
    color: #555;
    margin: 4px 0;
    line-height: 1.4;
    text-align: left;
}

/* ============ ACTION BUTTONS (ASP BUTTONS) ============ */
.actions {
    margin-top: auto;
    display: flex;
    gap: 10px;
    justify-content: space-between;
}

/* ASP.NET buttons in quiz cards */
.actions .start-quiz,
.actions .view-result {
    border: none;
    border-radius: 6px;
    color: #fff;
    padding: 10px 14px;
    font-weight: 500;
    text-align: center;
    cursor: pointer;
    width: 48%;
}

/* Start Quiz */
.actions .start-quiz {
    background-color: #28a745;
}
.actions .start-quiz:hover {
    background-color: #218838;
}

/* View Result */
.actions .view-result {
    background-color: #17a2b8;
}
.actions .view-result:hover {
    background-color: #117a8b;
}

/* ============ RESPONSIVE ============ */
@media (max-width: 1024px) {
    .quiz-toolbar {
        flex-wrap: wrap;
        align-items: flex-start;
        row-gap: 12px;
        column-gap: 10px;
        margin-bottom: 20px;
        padding: 0 12px;
    }

    .quiz-toolbar .form-control {
        flex: 1 1 100%;
        max-width: 100%;
    }

    .quiz-toolbar .form-select {
        flex: 1 1 100%;
        max-width: 100%;
        width: 100%;
        display: block; /* own line */
    }

    .quiz-grid {
        grid-template-columns: repeat(2, 1fr);
        margin-top: 12px;
        padding: 0 12px;
    }
}

@media (max-width: 768px) {
    /* Toolbar stacks vertically */
    .quiz-toolbar {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        width: 100%;
        gap: 12px;
        margin-bottom: 20px;
    }

    /* Search textbox full width */
    .quiz-toolbar .form-control {
        width: 90%;
        max-width: 500px;
        flex: 0 0 auto;
    }

    /* Search + Clear buttons inline but centered */
    .quiz-toolbar .btn-primary,
    .quiz-toolbar .btn-secondary {
        width: 45%;
        max-width: 200px;
        text-align: center;
    }

    /* Dropdown full width */
    .quiz-toolbar .form-select {
        width: 90%;
        max-width: 500px;
    }

    /* Quiz cards full width */
    .quiz-grid {
        grid-template-columns: 1fr;
        gap: 20px;
        max-width: 500px;
        margin: 0 auto 40px;
        padding: 0 10px;
    }

    /* Action buttons inside cards stack vertically */
    .actions {
        flex-direction: column;
    }

    .actions .start-quiz,
    .actions .view-result {
        width: 100%;
        text-align: center;
    }
}
    </style>
    <div class="quiz-section">
        <h1>Quizz</h1>

        <div class="quiz-toolbar">
        <!--<div class="col-lg-8 d-flex align-items-center gap-2">-->
            <!-- Textbox for Search -->
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                
                Placeholder="Search by Title / Chapter / Description" />

            <!-- Search Button -->
            <asp:Button ID="btnSearch" runat="server" Text="Search"
                CssClass="btn btn-primary" OnClick="btnSearch_Click" />

            <!-- Clear Button -->
            <asp:Button ID="btnClear" runat="server" Text="Clear"
                CssClass="btn btn-secondary" OnClick="btnClear_Click" />

            <!-- DropDown for Chapter Filter -->
            <asp:DropDownList ID="DropDownList_FilterByChapter" runat="server" 
                CssClass="form-select" ClientIDMode="AutoID" OnSelectedIndexChanged="DropDownList_FilterByChapter_SelectedIndexChanged" AutoPostBack="true">   
            </asp:DropDownList>
        </div>

        <div class="quiz-grid">

        <asp:Repeater ID="Repeater_QuizCards" runat="server" OnItemCommand="Repeater_QuizCards_ItemCommand">
            <ItemTemplate>
                <div class="quiz-card">
    <div class="qc-media">
      <img src='<%# Eval("ImageURL") %>' alt="" />
    </div>

    <div class="qc-body">
      <h3><%# Eval("Title") %></h3>
      <p>Chapter: <%# Eval("Chapter") %></p>
      <p>Time Limit: <%# Eval("TimeLimit") %> mins</p>
      <p>Attempts Left: <%# Eval("AttemptLimit") %></p>

      <!-- Description & Status stay inside the card body -->
      <p class="desc">Description: <%# Eval("Description") %></p>
      <p class="status">Status: Active</p>
    </div>

    <div class="actions">
      <asp:Button ID="btnStartQuiz" runat="server"
          Text="Start Quiz"
          CssClass="start-quiz"
          CommandName="start"
          CommandArgument='<%# Eval("QuizID") %>' />

      <asp:Button ID="btnViewResult" runat="server"
          Text="View Result"
          CssClass="view-result"
          CommandName="result"
          CommandArgument='<%# Eval("QuizID") %>' />
    </div>
  </div>
            </ItemTemplate>
        </asp:Repeater>
        </div>
    </div>
</asp:Content>
