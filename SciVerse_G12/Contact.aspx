<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="SciVerse_G12.Contact" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href='<%= ResolveUrl("~/Styles/site.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<main aria-labelledby="title">
    <section id="course-main">
        <h1 class="heading">Contact Us</h1>
        <p style="color: #718096; font-size: 1.1rem; margin-top: 1rem;">We'd love to hear from you. Send us a message and we'll respond as soon as possible.</p>
    </section>

    <section id="contact">
        <div class="contact-form-container">
            <div class="form-fields-wrapper">
                <asp:Panel ID="pnlContactForm" runat="server" DefaultButton="btnSubmit">
                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Your Name"></asp:Label>
                    <asp:TextBox ID="txtName" runat="server" placeholder="Enter your full name"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" 
                                                ErrorMessage="Please enter your full name." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>

                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Your Email"></asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="your.email@example.com"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                                                ErrorMessage="Please enter your email address." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                                                   ErrorMessage="Please enter a valid email address." CssClass="text-danger" Display="Dynamic" 
                                                   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

                    <asp:Label ID="lblMessage" runat="server" AssociatedControlID="txtMessage" Text="Your Message"></asp:Label>
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5" placeholder="Tell us what's on your mind..."></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" 
                                                ErrorMessage="Please enter a message." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>

                    <asp:Button ID="btnSubmit" runat="server" Text="Send Message" OnClick="btnSubmit_Click" />
                    <asp:Label ID="lblMessageStatus" runat="server" CssClass="text-danger" EnableViewState="false"></asp:Label>
                </asp:Panel>
            </div>
        </div>
    </section>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
    InsertCommand="INSERT INTO [tblContactUs] ([contactName], [contactEmail], [contactMessage], [RID]) 
                   VALUES (@contactName, @contactEmail, @contactMessage, @RID)">
    <InsertParameters>
        <asp:ControlParameter Name="contactName" ControlID="txtName" PropertyName="Text" Type="String" />
        <asp:ControlParameter Name="contactEmail" ControlID="txtEmail" PropertyName="Text" Type="String" />
        <asp:ControlParameter Name="contactMessage" ControlID="txtMessage" PropertyName="Text" Type="String" />
        <asp:SessionParameter Name="RID" SessionField="RID" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>



    <address>
        <div class="mapouter">
            <div class="gmap_canvas">
                <iframe class="gmap_iframe" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.com/maps?width=600&amp;height=400&amp;hl=en&amp;q=Jalan Teknologi 5, Taman Teknologi Malaysia, 57000 Kuala Lumpur, Wilayah Persekutuan Kuala Lumpur&amp;t=&amp;z=14&amp;ie=UTF8&amp;iwloc=B&amp;output=embed"></iframe>
                <a href="https://wheremylocation.com/">where am i</a></div>
                <style>.mapouter{position:relative;text-align:right;width:600px;height:400px;}.gmap_canvas {overflow:hidden;background:none!important;width:600px;height:400px;}.gmap_iframe {width:600px!important;height:400px!important;}
                </style>
             </div>
    </address>

    <address>
        <strong>✉️ Email Support:</strong><br />
        <strong>Support:</strong> <a href="mailto:Support@example.com">Support@example.com</a><br />
        <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@example.com</a>
    </address>
</main>
</asp:Content>

