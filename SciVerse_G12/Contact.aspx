<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="SciVerse_G12.Contact" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href='<%= ResolveUrl("~/Styles/site2.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title" class="contact-main">
        <!-- Hero Header -->
        <section class="contact-hero">
            <div class="container text-center">
                <h1 class="hero-title">
                    <i class="fas fa-envelope-open-text me-3"></i>Contact Us
                </h1>
                <p class="hero-subtitle">We'd love to hear from you. Send us a message and we'll respond as soon as possible.</p>
            </div>
        </section>

        <!-- Two-Column Layout -->
        <section class="contact-section">
            <div class="container-fluid px-0">
                <div class="row g-4 justify-content-center align-items-start">
                    
                    <!-- Left: Form -->
                    <div class="col-12 col-lg-6">
                        <div class="contact-form-card animate-fade-in mx-3 mx-lg-0">
                            <h2 class="form-title"><i class="fas fa-comment-dots me-2"></i>Send a Message</h2>
                            
                            <asp:Panel ID="pnlContactForm" runat="server" DefaultButton="btnSubmit" CssClass="form-panel">
                                <!-- Name -->
                                <div class="form-group">
                                    <label for="txtName" class="form-label"><i class="fas fa-user me-1"></i>Your Name</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control form-input" placeholder="Enter your full name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="Please enter your full name." CssClass="invalid-feedback" Display="Dynamic" ForeColor="Red" />
                                </div>

                                <!-- Email -->
                                <div class="form-group">
                                    <label for="txtEmail" class="form-label"><i class="fas fa-envelope me-1"></i>Your Email</label>
                                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control form-input" placeholder="your.email@example.com"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Please enter your email address." CssClass="invalid-feedback" Display="Dynamic" ForeColor="Red" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Please enter a valid email address." CssClass="invalid-feedback" Display="Dynamic" ForeColor="Red"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                </div>

                                <!-- Message -->
                                <div class="form-group">
                                    <label for="txtMessage" class="form-label"><i class="fas fa-message me-1"></i>Your Message</label>
                                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5"
                                        CssClass="form-control form-textarea" placeholder="Tell us what's on your mind..."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage"
                                        ErrorMessage="Please enter your message." CssClass="invalid-feedback" Display="Dynamic" ForeColor="Red" />
                                </div>

                                <!-- Submit -->
                                <div class="form-group text-center">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Send Message" OnClick="btnSubmit_Click" CssClass="btn-submit" />
                                    <br />
                                    <asp:Label ID="lblMessageStatus" runat="server" CssClass="status-message" EnableViewState="false"></asp:Label>
                                </div>

                            </asp:Panel>
                        </div>
                    </div>

                    <!-- Right: Map and Info -->
                    <div class="col-12 col-lg-6">
                        <div class="contact-info-card animate-slide-in mx-3 mx-lg-0">
                            <h3 class="section-title"><i class="fas fa-map-marker-alt me-2"></i>Our Location</h3>
                            <div class="map-container mb-4">
                                <iframe class="gmap_iframe" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"
                                    src="https://maps.google.com/maps?width=100%&amp;height=400&amp;hl=en&amp;q=Jalan Teknologi 5, Taman Teknologi Malaysia, 57000 Kuala Lumpur&amp;t=&amp;z=14&amp;ie=UTF8&amp;iwloc=B&amp;output=embed"></iframe>
                            </div>

                            <h3 class="section-title"><i class="fas fa-info-circle me-2"></i>Get in Touch</h3>
                            <div class="contact-details">
                                <div class="contact-item">
                                    <i class="fas fa-envelope icon"></i>
                                    <div><strong>Support:</strong> <a href="mailto:sciverse@mail.com">sciverse@mail.com</a></div>
                                </div>
                                <div class="contact-item">
                                    <i class="fas fa-phone icon"></i>
                                    <div><strong>Phone:</strong> <a href="tel:+60123456789">+60 12-345 6789</a></div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
    </main>

    <!-- SQL Data Source -->
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
</asp:Content>
