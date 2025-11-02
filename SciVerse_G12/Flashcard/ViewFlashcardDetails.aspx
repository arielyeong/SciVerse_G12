<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewFlashcardDetails.aspx.cs" Inherits="SciVerse_G12.Flashcard.ViewFlashcardDetails" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href='<%= ResolveUrl("~/Styles/Flashcard.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <style>
        .body-content {
            margin-top: 20px !important;  
            margin-bottom: 40px !important;  
        }
        .my-7 {
            margin-top: 35px !important;
            margin-bottom: 35px !important;
        }   
        /* In your custom CSS file (e.g., Site.css) or <style> block */
        #flashcardContainer {
            padding-left: 1rem;  
            padding-right: 1rem; 
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row align-items-center">
    
    <div class="col-auto">
        <a href="FlashcardList.aspx" class="btn btn-outline-secondary">
            <i class="fas fa-arrow-left"></i>
        </a>
    </div>
    
    <div class="col">
        <h2 class="text-center mb-0"> Flashcard for 
            <asp:Label ID="lblFlashcardTitle" runat="server"></asp:Label>
        </h2>
    </div>
</div>

 <div id="flashcardContainer" class="container-fluid my-7" style="height:30vh;">
     <asp:Panel ID="Panel1" runat="server" 
         CssClass="mx-auto my-2 px-3 py-7 rounded d-flex flex-column justify-content-center align-items-center text-center" 
         Width="100%" Height="100%" Style="background-color:#298fba;">
         <asp:Label ID="lblIndex" runat="server" CssClass="h5 d-block mb-4 text-wrap"></asp:Label>
         <asp:Label ID="lblQuestionType" runat="server" CssClass="h5 d-block mb-4 text-wrap text-black mb-2"></asp:Label>
         <asp:Label ID="lblQuestion" runat="server" TextMode="Html" CssClass="h2 d-block text-wrap mb-2 text-center text-white"></asp:Label>
         
     </asp:Panel>
 </div>
 <div class="d-flex justify-content-evenly gap-4 my-4">
     <asp:Button ID="btnFirst" runat="server" CssClass="btn btn-outline-primary flex-fill" OnClick="btnFirst_Click" Text="First" />
     <asp:Button ID="btnPrevious" runat="server" CssClass="btn btn-outline-primary flex-fill" OnClick="btnPrevious_Click" Text="Previous" />
     <asp:Button ID="btnShowAns" runat="server" CssClass="btn btn-success flex-fill" OnClick="btnShowAns_Click" Text="Show Answer" />
     <asp:Button ID="btnNext" runat="server" CssClass="btn btn-outline-primary flex-fill" OnClick="btnNext_Click" Text="Next" />
     <asp:Button ID="btnLast" runat="server" CssClass="btn btn-outline-primary flex-fill" OnClick="btnLast_Click" Text="Last" />
 </div>

</asp:Content>
