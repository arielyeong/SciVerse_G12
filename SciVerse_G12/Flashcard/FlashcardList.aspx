<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FlashcardList.aspx.cs" Inherits="SciVerse_G12.Flashcard.FlashcardList" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href='<%= ResolveUrl("~/Styles/Flashcard.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <style>
        .body-content {
            margin-top: 10px !important; 
        }
        .mb-6 {
            margin-bottom: 35px !important;
        }  
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mb-5">
        <h2 class="text-center mb-6 fw-bold">Available Flashcards</h2>
        
        <!-- Grid layout: 4 boxes per row -->
        <div class="row g-4 justify-content-center row-cols-2 row-cols-md-3 row-cols-lg-4 row-cols-xl-5">
            <asp:Repeater ID="rptFlashcards" runat="server" OnItemCommand="rptFlashcards_ItemCommand">
                <ItemTemplate>
                    <div class="col">
                        <div class="card quiz-card shadow-sm h-100 d-flex flex-column align-items-center">
                            <div class="card-body d-flex flex-column justify-content-center align-items-center text-center w-100">
                                <p class="card-text text-secondary display-5 fw-normal mb-1" style="font-size: 0.85rem; min-height: 20px;">
                                    <%# Eval("chapter") %>
                                </p>
                                <h4 class="card-title text-primary fw-bold" style="font-size: 1.05rem; min-height: 48px;">
                                    <%# Eval("title") %>
                                </h4>
                                <asp:Button runat="server"
                                    ID="btnShowFlashcard"
                                    CssClass="btn btn-primary"
                                    Text="Show Flashcard"
                                    CommandArgument='<%# Eval("flashcard_id") %>'
                                    OnCommand="btnGenerateFlashcard_Command" />
                            </div>
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>
        </div>

        
        <!-- Message when no quizzes available -->
        <div class="row mt-5" id="noFlashcardsSection" runat="server" visible="false">
            <div class="col-12">
                <div class="alert alert-info text-center" role="alert">
                    <i class="bi bi-info-circle me-2"></i>
                    <asp:Label ID="lblNoFlashcards" runat="server" 
                        Text="No flashcards available at the moment."></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>