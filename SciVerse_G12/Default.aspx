<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SciVerse_G12._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
            <!-- home -->
    <section id="home">
        <h2 class="text-center">Welcome to SciVerse, Guest – Science Made Interactive!</h2>
        <p class="text-center">
            SciVerse is a web-based e-learning application developed to educate secondary school students in general science. 
            It combines multimedia resources, interactive quizzes, and virtual experiments to inspire students and build 
            a strong foundation in science. With features like gamification, performance tracking, and self-assessment, 
            SciVerse transforms learning into an engaging and playful experience.
        </p>
        <div class="button">
            <a class="blue" href="">Join Us</a>
        </div>
    </section>

    <!-- about us -->
    <section id="about-us">
        <h1>ABOUT US</h1>
        <p>
            <strong>SciVerse</strong> is designed to make science learning accessible, interactive, and fun. 
            Our mission is to transform traditional science education into a dynamic experience by providing students and teachers 
            with tools such as learning materials, quizzes, and experiment simulations. SciVerse promotes curiosity and deeper 
            understanding while supporting revision anywhere, anytime.
        </p>
    </section>

    <!-- features -->
    <section id="features">
        <div class="fea-base">
            <div class="fea-box">
                <div class="box-image">
                    <img src="" alt="Learning Materials">
                </div>
                <div class="fea-content">
                    <h3>Learning Materials</h3>
                    <p>
                        Access videos, documents, and interactive resources that support secondary school science education 
                        and help students learn concepts in a clear and engaging way.
                    </p>
                </div>
            </div>
            <div class="fea-box">
                <div class="box-image">
                    <img src="" alt="Quizzes">
                </div>
                <div class="fea-content">
                    <h3>Quizzes & Performance Tracking</h3>
                    <p>
                        Practice with multiple types of quizzes, including MCQs, true/false, and fill-in-the-blank. 
                        Automatic grading and performance logs allow students to review mistakes and improve steadily.
                    </p>
                </div>
            </div>
            <div class="fea-box">
                <div class="box-image">
                    <img src="" alt="Virtual Experiments">
                </div>
                <div class="fea-content">
                    <h3>Virtual Experiments</h3>
                    <p>
                        Perform interactive simulations like chemical mixing and visual experiments. 
                        Learn practical science skills through a safe, virtual lab environment.
                    </p>
                </div>
            </div>
            <div class="fea-box">
                <div class="box-image">
                    <img src="" alt="Gamification">
                </div>
                <div class="fea-content">
                    <h3>Gamification Elements</h3>
                    <p>
                        Use flashcards and playful learning tools to make revision fun and engaging. 
                        SciVerse keeps students motivated while reinforcing their knowledge.
                    </p>
                </div>
            </div>
        </div>
    </section>

    <!-- why choose -->
    <section id="fea-bullet">
        <h3>Why Choose SciVerse?</h3>
        <ul class="bullet-points">
            <li><strong>Accessible & Engaging:</strong> Learn anywhere, anytime with interactive materials and resources.</li>
            <li><strong>Interactive Learning:</strong> Quizzes, experiments, and gamification create an active learning environment.</li>
            <li><strong>Performance Tracking:</strong> Students can review quiz history, track mistakes, and improve progressively.</li>
            <li><strong>Support for Teachers:</strong> Instructors can incorporate materials and simulations into their lessons.</li>
        </ul>
    </section>

    <!-- course -->
    <section id="course">
        <h1>Courses We Provide</h1>
        <p>Explore general science with interactive tools and resources.</p>
    </section>
    <section id="course-main">
        <div class="three-info">
            <div class="three-box">
                <div class="box-image">
                    <img src="" alt="General Science">
                    <div class="box-content">
                        <a href="" class="btn">General Science</a>
                    </div>
                </div>
            </div>
        </div>
    </section>
    </main>

</asp:Content>

