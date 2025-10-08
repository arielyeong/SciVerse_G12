<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SciVerse_G12._Default" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
     <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Page-specific CSS -->
    <link href="<%= ResolveUrl("~/Styles/Home.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="min-vh-100">

        <!-- Hero Section -->
        <section id="home" class="hero-section text-center py-5">
            <div class="container-fluid position-relative px-0">
                <div class="hero-badge mb-3">🚀 Science Made Interactive</div>
                <h1 class="hero-title mb-3">
                    Discover the Universe of <span class="gradient-text">Science</span>
                </h1>
                <p class="hero-subtitle mb-4">
                    Transform your learning journey with interactive experiments, engaging quizzes, and immersive virtual labs.
                    Join thousands of students exploring physics, chemistry, and biology in a whole new way.
                </p>
                <div class="d-flex justify-content-center gap-3">
                    <a href="#" class="btn btn-primary btn-lg">Start Learning Now →</a>
                    <a href="#" class="btn btn-outline-primary btn-lg">Watch Demo</a>
                </div>
                <div class="row mt-5 hero-stats">
                    <div class="col-md-4">
                        <div class="stat-item">
                            <div class="stat-number">10K+</div>
                            <div class="stat-label">Active Students</div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="stat-item">
                            <div class="stat-number">500+</div>
                            <div class="stat-label">Experiments</div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="stat-item">
                            <div class="stat-number">95%</div>
                            <div class="stat-label">Success Rate</div>
                        </div>
                    </div>
                </div>

                <div class="hero-visual position-absolute top-0 end-0">
                    <div class="floating-element atom">⚛️</div>
                    <div class="floating-element molecule">🧬</div>
                    <div class="floating-element flask">🧪</div>
                    <div class="floating-element bulb">💡</div>
                </div>
            </div>
        </section>

        <!-- Did You Know Section -->
        <section id="did-you-know" class="did-you-know-section py-5 bg-light">
            <div class="container text-center">
                <h2 class="section-title mb-4">Did You Know?</h2>
                <div class="row g-4">
                    <div class="col-md-4">
                        <div class="fact-card p-4 h-100 shadow-sm">
                            <div class="fact-icon fs-1">⚡</div>
                            <h3 class="fact-title mt-3">Lightning Speed</h3>
                            <p>Lightning can heat the air around it to temperatures five times hotter than the sun's surface!</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="fact-card p-4 h-100 shadow-sm">
                            <div class="fact-icon fs-1">🌊</div>
                            <h3 class="fact-title mt-3">Water Memory</h3>
                            <p>Water can remember substances that were once dissolved in it at the molecular level.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="fact-card p-4 h-100 shadow-sm">
                            <div class="fact-icon fs-1">🧠</div>
                            <h3 class="fact-title mt-3">Brain Power</h3>
                            <p>Your brain generates enough electricity to power a small light bulb while you’re awake!</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Features Section -->
        <section id="features" class="features-section py-5">
            <div class="container text-center">
                <h2 class="section-title mb-3">Explore Our Features</h2>
                <p class="section-subtitle mb-5">Everything you need to master science concepts</p>
                <div class="row g-4">
                    <div class="col-md-3">
                        <div class="feature-card p-4 h-100 shadow-sm">
                            <div class="feature-icon fs-1 mb-3">📚</div>
                            <h3>Interactive Lessons</h3>
                            <p>Dive into multimedia-rich content with videos, animations, and interactive diagrams.</p>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="feature-card p-4 h-100 shadow-sm featured position-relative">
                            <span class="featured-badge">Most Popular</span>
                            <div class="feature-icon fs-1 mb-3">🔬</div>
                            <h3>Virtual Lab</h3>
                            <p>Conduct experiments in a safe virtual environment and discover scientific principles.</p>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="feature-card p-4 h-100 shadow-sm">
                            <div class="feature-icon fs-1 mb-3">🎯</div>
                            <h3>Smart Quizzes</h3>
                            <p>Test your knowledge with adaptive quizzes that adjust to your skill level.</p>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="feature-card p-4 h-100 shadow-sm">
                            <div class="feature-icon fs-1 mb-3">🏆</div>
                            <h3>Gamification</h3>
                            <p>Earn badges and achievements while making learning fun and rewarding!</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

       

        <!-- Featured Topics Section -->
        <section id="topics" class="topics-section">
            <div class="container">
                <h2 class="section-title text-center">Featured Topics</h2>

                <div class="topics-grid">
                    <!-- Physics -->
                    <div class="topic-card topic-physics">
                        <div class="topic-header">
                            <span class="topic-icon">⚛️</span>
                            <span class="topic-badge">Physics</span>
                        </div>
                        <h3 class="topic-title">Quantum Mechanics</h3>
                        <p class="topic-description">Explore the fascinating world of subatomic particles.</p>
                        <div class="topic-meta">
                            <span class="topic-lessons">12 Lessons</span>
                            <span class="topic-duration">6 hours</span>
                        </div>
                    </div>

                    <!-- Chemistry -->
                    <div class="topic-card topic-chemistry">
                        <div class="topic-header">
                            <span class="topic-icon">🧪</span>
                            <span class="topic-badge">Chemistry</span>
                        </div>
                        <h3 class="topic-title">Organic Chemistry</h3>
                        <p class="topic-description">Master the chemistry of carbon compounds.</p>
                        <div class="topic-meta">
                            <span class="topic-lessons">15 Lessons</span>
                            <span class="topic-duration">8 hours</span>
                        </div>
                    </div>

                    <!-- Biology -->
                    <div class="topic-card topic-biology">
                        <div class="topic-header">
                            <span class="topic-icon">🧬</span>
                            <span class="topic-badge">Biology</span>
                        </div>
                        <h3 class="topic-title">Genetics & DNA</h3>
                        <p class="topic-description">Understand heredity and genetic engineering.</p>
                        <div class="topic-meta">
                            <span class="topic-lessons">10 Lessons</span>
                            <span class="topic-duration">5 hours</span>
                        </div>
                    </div>

                    <!-- Earth Science -->
                    <div class="topic-card topic-earth">
                        <div class="topic-header">
                            <span class="topic-icon">🌍</span>
                            <span class="topic-badge">Earth Science</span>
                        </div>
                        <h3 class="topic-title">Climate Change</h3>
                        <p class="topic-description">Learn about our planet's changing climate.</p>
                        <div class="topic-meta">
                            <span class="topic-lessons">8 Lessons</span>
                            <span class="topic-duration">4 hours</span>
                        </div>
                    </div>
                </div>
            </div>
        </section>


        <!-- Why Choose Section -->
        <section id="why-choose" class="why-choose-section py-5">
            <div class="container text-center">
                <h2 class="section-title mb-4">Why Choose SciVerse?</h2>
                <div class="row justify-content-center">
                    <div class="col-md-6">
                        <ul class="list-unstyled text-start">
                            <li class="mb-3">✓ <strong>Personalized Learning:</strong> AI-powered recommendations.</li>
                            <li class="mb-3">✓ <strong>Expert Content:</strong> Designed by educators and scientists.</li>
                            <li class="mb-3">✓ <strong>Progress Tracking:</strong> Monitor learning with detailed insights.</li>
                            <li class="mb-3">✓ <strong>Community Support:</strong> Connect with peers and mentors.</li>
                        </ul>
                    </div>
                    <div class="col-md-4">
                        <div class="achievement-card p-3 shadow-sm mb-3">
                            <div class="achievement-icon fs-1">🎓</div>
                            <div class="achievement-number">95%</div>
                            <div class="achievement-label">Student Satisfaction</div>
                        </div>
                        <div class="achievement-card p-3 shadow-sm">
                            <div class="achievement-icon fs-1">⭐</div>
                            <div class="achievement-number">4.9/5</div>
                            <div class="achievement-label">Average Rating</div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Courses Section -->
        <section id="courses" class="courses-section py-5 bg-light">
            <div class="container text-center">
                <h2 class="section-title mb-4">Start Your Journey</h2>
                <p class="section-subtitle mb-5">Choose your path and begin exploring</p>
                <div class="row g-4">
                    <div class="col-md-4">
                        <div class="course-card p-4 shadow-sm h-100">
                            <div class="course-icon fs-1 mb-3">⚛️</div>
                            <h3>Physics</h3>
                            <p>Explore the fundamental forces that govern our universe.</p>
                            <a href="#" class="btn btn-course btn-primary mt-3">Explore Physics</a>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="course-card p-4 shadow-sm h-100">
                            <div class="course-icon fs-1 mb-3">🧪</div>
                            <h3>Chemistry</h3>
                            <p>Discover the magic of molecules and reactions in virtual labs.</p>
                            <a href="#" class="btn btn-course btn-primary mt-3">Explore Chemistry</a>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="course-card p-4 shadow-sm h-100">
                            <div class="course-icon fs-1 mb-3">🧬</div>
                            <h3>Biology</h3>
                            <p>Understand the living world through simulations and models.</p>
                            <a href="#" class="btn btn-course btn-primary mt-3">Explore Biology</a>
                        </div>
                    </div>
                </div>
            </div>
        </section>



        <!-- CTA Section -->
        <section id="cta" class="cta-section py-5 text-center">
            <div class="container">
                <h2 class="cta-title mb-3">Ready to Transform Your Learning?</h2>
                <p class="cta-subtitle mb-4">Join thousands of students exploring science in a whole new way.</p>
                <a href="#" class="btn btn-lg btn-primary">Get Started Free</a>
            </div>
        </section>
    </main>

</asp:Content>