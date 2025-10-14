<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Flashcard.aspx.cs" Inherits="SciVerse_G12.Quiz_and_Flashcard.Quiz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <h2 class="text-center mb-4 fw-bold">Available Flashcards</h2>
        
        <!-- Grid layout: 4 boxes per row -->
        <div class="row g-4 justify-content-start">
            <asp:Repeater ID="rptQuizzes" runat="server">
                <ItemTemplate>
                    <div class="col-6 col-md-4 col-lg-3">
                        <div class="card quiz-card shadow-sm h-100 d-flex flex-column justify-content-between align-items-center text-center" style="max-width: 220px; margin: 0 auto;">
                            <div class="card-body d-flex flex-column justify-content-center align-items-center text-center w-100">
                                <h5 class="card-title text-primary fw-bold" style="font-size: 1.05rem; min-height: 48px;">
                                    <%# Eval("title") %>
                                </h5>
                                <asp:Button runat="server"
                                    ID="btnShowFlashcard"
                                    CssClass="btn btn-primary"
                                    Text="Show Flashcard"
                                    CommandArgument='<%# Eval("quiz_id") %>'
                                    OnCommand="btnGenerateFlashcard_Command" />
                            </div>
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>
        </div>

        
        <!-- Message when no quizzes available -->
        <div class="row mt-5" id="noQuizzesSection" runat="server" visible="false">
            <div class="col-12">
                <div class="alert alert-info text-center" role="alert">
                    <i class="bi bi-info-circle me-2"></i>
                    <asp:Label ID="lblNoQuizzes" runat="server" 
                        Text="No quizzes available at the moment."></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>