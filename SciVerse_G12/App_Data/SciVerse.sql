/*----------------------------------------------------
 Create Registered Users Table
----------------------------------------------------*/

CREATE TABLE [dbo].[tblRegisteredUsers] (
    [RID]          INT            IDENTITY (1, 1) NOT NULL,
    [fullName]     NVARCHAR (100) NULL,
    [emailAddress] NVARCHAR (50)  NULL,
    [username]     NVARCHAR (50)  NULL,
    [password]     NVARCHAR (50)  NULL,
    [age]          INT            NULL,
    [gender]       NCHAR (10)     NULL,
    [country]      NVARCHAR (MAX) NULL,
    [picture]      NVARCHAR (MAX) NULL,
    [dateRegister] DATETIME       NULL,
    [role]         NVARCHAR (20)  DEFAULT ('User') NOT NULL,
    PRIMARY KEY CLUSTERED ([RID] ASC)
);


/*----------------------------------------------------
 Insert Sample Users
----------------------------------------------------*/

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('System Admin', 'admin@example.com', 'admin', 'admin123', 30, 'Male', 'Malaysia', '~/Images/Profile/AdminProfile.jpg', GETDATE(), 'Admin');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Charlotte Chen Zi Shan', 'charlotte@mail.com', 'charlotte', 'wapp', 21, 'Female', 'Colombia', 'https://robohash.org/suntquiet.png?size=50x50&set=set1', '2025-02-19 11:42:17', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Chong Ching Ying', 'chingying@mail.com', 'chingying', 'wapp', 21, 'Female', 'New Zealand', 'https://robohash.org/inventoreliberosit.png?size=50x50&set=set1', '2024-11-05 06:14:41', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Ong Ying Xin', 'yingxin@mail.com', 'yingxin', 'wapp', 21, 'Male', 'Thailand', 'https://robohash.org/sapientenihilfugit.png?size=50x50&set=set1', '2025-06-12 07:32:32', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Yeong Huey Yee', 'hueyyee@mail.com', 'hueyyee', 'wapp', 18, 'Female', 'China', '~/Images/Profile/panda.jpg', '2025-07-26 20:58:16', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Chen Jia Wei', 'chenjiawei@example.com', 'chenjiawei', 'wapp', 15, 'Female', 'Iceland', 'https://robohash.org/earumabvoluptatem.png?size=50x50&set=set1', '2025-09-15 07:16:42', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Li Jun Hao', 'lijunhao@example.com', 'lijunhao', 'wapp', 16, 'Female', 'Indonesia', 'https://robohash.org/facerequivelit.png?size=50x50&set=set1', '2025-02-20 01:28:28', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Wang Shi Han', 'wangshihan@example.com', 'wangshihan', 'wapp', 8, 'Female', 'China', 'https://robohash.org/magnamquiaeaque.png?size=50x50&set=set1', '2025-01-12 23:54:23', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Lin Zi Xuan', 'linzixuan@example.com', 'linzixuan', 'wapp', 11, 'Female', 'Czech Republic', 'https://robohash.org/voluptatemexet.png?size=50x50&set=set1', '2025-02-04 23:05:16', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Zhou Ya Ting', 'zhouyating@example.com', 'zhouyating', 'wapp', 8, 'Female', 'Canada', 'https://robohash.org/laborumquiain.png?size=50x50&set=set1', '2025-01-23 15:38:34', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Zhang Zi Chen', 'zhangzichen@example.com', 'zhangzichen', 'wapp', 18, 'Female', 'China', 'https://robohash.org/aspernaturvelrepudiandae.png?size=50x50&set=set1', '2025-07-26 20:58:16', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Tan Mei Ling', 'meiling@mail.com', 'meiling', 'wapp', 22, 'Female', 'Singapore', 'https://robohash.org/nesciuntvoluptasvoluptatem.png?size=50x50&set=set1', '2024-10-15 14:23:45', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Lim Wei Jie', 'weijie@example.com', 'weijie', 'wapp', 19, 'Male', 'Malaysia', '~/Images/Profile/maleprofile.jpg', '2025-03-08 09:17:32', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Huang Xiao Yu', 'xiaoyu@mail.com', 'xiaoyu', 'wapp', 17, 'Female', 'Taiwan', 'https://robohash.org/quoetaut.png?size=50x50&set=set1', '2025-05-22 16:04:19', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Goh Kai Wen', 'kaiwen@example.com', 'kaiwen', 'wapp', 24, 'Male', 'Australia', 'https://robohash.org/velitsedquia.png?size=50x50&set=set1', '2024-12-01 11:55:08', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Wong Jia Hui', 'jiahui@mail.com', 'jiahui', 'wapp', 13, 'Female', 'Hong Kong', '~/Images/Profile/femaleprofile.jpg', '2025-01-30 18:42:57', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Foo Ming Xuan', 'mingxuan@example.com', 'mingxuan', 'wapp', 20, 'Male', 'Philippines', 'https://robohash.org/explicaboeos.png?size=50x50&set=set1', '2025-04-12 13:29:41', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Ng Sze Yin', 'szeyin@mail.com', 'szeyin', 'wapp', 15, 'Female', 'Vietnam', 'https://robohash.org/estquiaet.png?size=50x50&set=set1', '2025-08-07 20:15:23', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Lau Hao Ran', 'haoran@example.com', 'haoran', 'wapp', 28, 'Male', 'Japan', '~/Images/Profile/japanprofile.jpg', '2024-09-18 07:36:14', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Chan Yi Ting', 'yiting@mail.com', 'yiting', 'wapp', 10, 'Female', 'South Korea', 'https://robohash.org/ipsumdoloremque.png?size=50x50&set=set1', '2025-11-03 12:48:09', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Teo Zhi Wei', 'zhiwei@example.com', 'zhiwei', 'wapp', 16, 'Male', 'United States', 'https://robohash.org/repellatdolor.png?size=50x50&set=set1', '2025-02-28 15:21:56', 'User');

/*----------------------------------------------------
 Create Contact Us Table
----------------------------------------------------*/

CREATE TABLE [dbo].[tblContactUs] (
    [CID]            INT            IDENTITY (1, 1) NOT NULL,
    [contactName]    NVARCHAR (50)  NULL,
    [contactEmail]   NVARCHAR (50)  NULL,
    [contactMessage] NVARCHAR (MAX) NULL,
    [RID]            INT            NULL,
    [isReviewed]     BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CID] ASC)
);

/*----------------------------------------------------
 Insert Sample Contact Messages
----------------------------------------------------*/

INSERT INTO [dbo].[tblContactUs] (contactName, contactEmail, contactMessage, RID, isReviewed)
VALUES 
('Charlotte Chen Zi Shan', 'charlotte@mail.com', 'Hi admin! I wanted to ask if you will be adding new science quizzes soon?', 2, 0),

('Chong Ching Ying', 'chingying@mail.com', 'Hello! I think the simulation game is really fun, keep it up!', 3, 1),

('Ong Ying Xin', 'yingxin@mail.com', 'I encountered a small issue when submitting the quiz. The loading screen got stuck.', 4, 0),

('Yeong Huey Yee', 'hueyyee@mail.com', 'HILO, just testing the contact form. Everything works great so far!', 5, 1),

('Chen Jia Wei', 'chenjiawei@example.com', 'Good afternoon! Will there be any upcoming events for registered users?', 6, 0);


--===========================================================
-- Create Learning Materials Table
--===========================================================

CREATE TABLE [dbo].[tblLearningMaterial] (
    [materialID]  INT            IDENTITY (1, 1) NOT NULL,
    [title]       NVARCHAR (255) NOT NULL,
    [description] NVARCHAR (MAX) NULL,
    [chapter]     INT NULL,
    [type]        NVARCHAR (50)  NULL,
    [filePath]    NVARCHAR (500) NOT NULL,
    [uploadDate]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [uploadedBy]  INT            NULL,
    PRIMARY KEY CLUSTERED ([materialID] ASC),
    FOREIGN KEY ([uploadedBy]) REFERENCES [dbo].[tblRegisteredUsers] ([RID]) ON DELETE SET NULL
);

/*----------------------------------------------------
 Insert Learning Materials 
----------------------------------------------------*/

INSERT INTO [dbo].[tblLearningMaterial] (title, description, chapter, [type], filePath, uploadedBy)
VALUES
-- chap 1
('Forces and Motion', 'Comprehensive notes on forces, motion, and Newton''s laws.', 1, 'Word', '/LearningMaterials/materials/Chapter1.docx', 10),
('Forces and Motion', 'Comprehensive notes on forces, motion, and Newton''s laws.', 1, 'Video', '/LearningMaterials/materials/force-and-motion.mp4', 10),

-- chap 2
('The Nature of Matter', 'Detailed notes on the states of matter, atoms, and chemical properties.', 2, 'Word', '/LearningMaterials/materials/Chapter2.docx', 10),
('The Nature of Matter', 'Detailed notes on the states of matter, atoms, and chemical properties.', 2, 'Video', '/LearningMaterials/materials/nature-of-matter.mp4', 10),

-- chap 3
('Heat and Temperature', 'In-depth notes on heat transfer, temperature, and thermodynamics.', 3, 'Word', '/LearningMaterials/materials/Chapter3.docx', 10),
('Heat and Temperature', 'In-depth notes on heat transfer, temperature, and thermodynamics.', 3, 'Video', '/LearningMaterials/materials/heat-and-temp.mp4', 10),

-- chap 4
('The Human Body Systems', 'A complete guide to the major systems of the human body.', 4, 'Word', '/LearningMaterials/materials/Chapter4.docx', 10),
('The Human Body Systems', 'A complete guide to the major systems of the human body.', 4, 'Video', '/LearningMaterials/materials/human-body-system.mp4', 10),

-- chap 5
('Solar System', 'Explore the planets, celestial bodies, and wonders of our solar system.', 5, 'Word', '/LearningMaterials/materials/Chapter5.docx', 10),
('Solar System', 'Explore the planets, celestial bodies, and wonders of our solar system.', 5, 'Video', '/LearningMaterials/materials/solar-system.mp4', 10);

--reset script if tbl id got prob
DELETE FROM tblLearningMaterial;
DBCC CHECKIDENT ('tblLearningMaterial', RESEED, 0);

/*----------------------------------------------------
 Create Experiment Simulation Table
----------------------------------------------------*/

-- CREATE
CREATE TABLE [dbo].[tblExperimentSimulation] (
    [SimulationID]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (200) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Chapter]      NVARCHAR (100) NULL,
    [Instruction]  NVARCHAR (MAX) NULL,
    [URL]          NVARCHAR (MAX) NULL,
    [Image]        NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([SimulationID] ASC),
);

/*----------------------------------------------------
 Insert Experiment Simulation
----------------------------------------------------*/

INSERT INTO [dbo].[tblExperimentSimulation](Title, Description, Chapter, Instruction,URL,Image)
VALUES
('My solar system', 
'Explore how gravity controls the motion of planets, moons, and stars. Create your own solar system and observe how the objects move and interact with each other based on mass, distance, and velocity.',
'Chapter 5',
'1. Select planets, moons, or stars into the simulation space. 
 2. Adjust the mass, velocity, and position of each object. 
 3. Start the simulation and observe the orbital paths formed by gravitational forces. 
 4. Experiment with different configurations to see how changing mass or velocity affects motion. 
 5. Record your observations and explain how gravitational pull keeps planets in orbit.',
 'https://phet.colorado.edu/sims/html/my-solar-system/latest/my-solar-system_en.html',
 'https://phet.colorado.edu/sims/html/my-solar-system/latest/my-solar-system-600.png'),

('Reaction of acid and reactive metal','Observe how reactive metals like magnesium react with acids to produce hydrogen gas and form salts.','Chapter 2', 
'1. Drag the Hydrochloric acid bottle to the test tube. 
 2. Drag the Magnesium ribbon to the test tube. 
 3. Click the Bunsen burner to light it. 
 4. Drag the wooden splint into the lit Bunsen flame to ignite it. 
 5. Drag the lit splint to the test tube to show the pop image.
 6. Drag the reacted test tube to the dish above the Bunsen.
 7. Click the lit Bunsen to heat the dish mixture to see the product.'
 ,'Images/Simulation/reaction.html',
 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT8nAOSyvPZMwgdn_pVbqDbIFhp4MuhRPllmQ&s'
 )

 /*----------------------------------------------------
Create Access Simulation
----------------------------------------------------*/

 CREATE TABLE [dbo].[tblAccessSimulation]
(
	[SID]			INT         IDENTITY (1, 1) NOT NULL,
	[RID]			INT			NULL,
	[LastModify]	DATETIME	NULL,
	[SimulationID]	INT			NULL 
    PRIMARY KEY CLUSTERED ([SID] ASC),
    FOREIGN KEY ([RID]) REFERENCES [dbo].[tblRegisteredUsers] ([RID]) ON DELETE SET NULL,
	FOREIGN KEY ([SimulationID]) REFERENCES [dbo].[tblExperimentSimulation] ([SimulationID]) ON DELETE SET NULL
)

 /*----------------------------------------------------
Create All QUiz Tables
----------------------------------------------------*/

CREATE TABLE [dbo].[tblQuiz] (
    [QuizID]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (200) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Chapter]      INT			  NULL,
    [TimeLimit]    INT            NULL,
    [ImageURL]     NVARCHAR (255) NULL,
    [CreatedDate]  DATETIME       DEFAULT (getdate()) NULL,
    [CreatedBy]    INT            NULL,
    [AttemptLimit] INT            NULL,
    PRIMARY KEY CLUSTERED ([QuizID] ASC),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblRegisteredUsers] ([RID]) ON DELETE SET NULL
);

CREATE TABLE [dbo].[tblQuestion] (
    [QuestionID]   INT            IDENTITY (1, 1) NOT NULL,
    [QuizID]       INT            NOT NULL,
    [QuestionText] NVARCHAR (MAX) NOT NULL,
    [QuestionType] NVARCHAR (50)  NULL,
    [Marks]        INT            NULL,
    [Explanation]  NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([QuestionID] ASC),
    FOREIGN KEY ([QuizID]) REFERENCES [dbo].[tblQuiz] ([QuizID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[tblOptions] (
    [OptionID]   INT            IDENTITY (1, 1) NOT NULL,
    [QuestionID] INT            NOT NULL,
    [OptionText] NVARCHAR (255) NOT NULL,
    [IsCorrect]  BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([OptionID] ASC),
    FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[tblQuestion] ([QuestionID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[tblQuizAttempt] (
    [AttemptID]   INT           IDENTITY (1, 1) NOT NULL,
    [RID]         INT           NULL,
    [QuizID]      INT           NOT NULL,
    [AttemptDate] DATETIME2 (0) DEFAULT (sysutcdatetime()) NOT NULL,
    [Duration]    INT           DEFAULT ((0)) NOT NULL,
    [Status]      NVARCHAR (20) DEFAULT (N'InProgress') NOT NULL,
    PRIMARY KEY CLUSTERED ([AttemptID] ASC),
    CONSTRAINT [FK_Attempt_User] FOREIGN KEY ([RID]) REFERENCES [dbo].[tblRegisteredUsers] ([RID]) ON DELETE SET NULL,
    CONSTRAINT [FK_Attempt_Quiz] FOREIGN KEY ([QuizID]) REFERENCES [dbo].[tblQuiz] ([QuizID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[tblQuizResult] (
    [ResultID]      INT            IDENTITY (1, 1) NOT NULL,
    [AttemptID]     INT            NOT NULL,
    [Score]         INT            NULL,
    [Question]      NVARCHAR (255) NULL,
    [Answer]        NVARCHAR (255) NULL,
    [CorrectAnswer] NVARCHAR (255) NULL,
    [Marks]         INT            NULL,
    PRIMARY KEY CLUSTERED ([ResultID] ASC),
    FOREIGN KEY ([AttemptID]) REFERENCES [dbo].[tblQuizAttempt] ([AttemptID]) ON DELETE CASCADE
);

--===========================================================
-- INSERT QUESTIONS AND OPTIONS
--===========================================================

INSERT INTO [dbo].[tblQuiz] (Title, Description, Chapter, TimeLimit, ImageURL, CreatedBy, AttemptLimit)
VALUES 
('Forces and Motion', 'A quiz on the fundamental concepts of forces, motion, and Newton''s laws.', 1, 0, '/Images/Quiz/force-and-motion.png', 10, 10),
('The Nature of Matter', 'Test your knowledge on the states of matter, atoms, and physical vs. chemical changes.', 2, 0, '/Images/Quiz/the-nature-of-matter.png', 10, 10),
('Heat and Temperature', 'Explore concepts of heat transfer, temperature measurement, and thermodynamics.', 3, 0, '/Images/Quiz/heat-and-temp.png', 10, 10),
('The Human Body Systems', 'A journey through the major systems of the human body.', 4, 0, '/Images/Quiz/the-human-body-systems.png', 10, 10),
('Solar System', 'Explore the planets, celestial bodies, and wonders of our solar system.', 5, 20, '/Images/Quiz/solar-system.png', 10, 10);

--===========================================================
-- INSERT QUESTIONS AND OPTIONS
--===========================================================
DECLARE @QID INT;

-- QUIZ 1: Forces and Motion
-- Q1
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'What is the SI unit of force?', 'MCQ', 2, 'The SI unit for force is the Newton (N), named after Sir Isaac Newton.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Newton', 1),
(@QID, 'Joule', 0),
(@QID, 'Watt', 0),
(@QID, 'Pascal', 0);

-- Q2
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'An object at rest stays at rest unless acted upon by an external force.', 'TrueFalse', 2, 'This is the definition of Newton''s First Law of Motion, also known as the law of inertia.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q3
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'Force equals mass times [blank].', 'FillInBlanks', 2, 'This is Newton''s Second Law, mathematically expressed as F = ma.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'acceleration', 1);

-- Q4
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'Which law states that every action has an equal and opposite reaction?', 'MCQ', 2, 'This is Newton''s Third Law of Motion.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Newton''s Third Law', 1),
(@QID, 'Newton''s First Law', 0),
(@QID, 'Newton''s Second Law', 0),
(@QID, 'Law of Gravitation', 0);

-- Q5
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'Gravity always acts toward the center of the Earth.', 'TrueFalse', 2, 'Gravity is an attractive force that pulls objects toward the center of mass. For Earth, this is its center.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'A car moving at constant velocity has zero net force.', 'TrueFalse', 2, 'According to Newton''s First Law, constant velocity (including zero velocity) means the net force acting on the object is zero.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'The rate of change of velocity is called [blank].', 'FillInBlanks', 2, 'Acceleration is the vector quantity that describes the rate at which an object changes its velocity.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'acceleration', 1);


-------------------------------------------------------------
-- QUIZ 2: The Nature of Matter
-- Q1
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Matter is made up of tiny particles called [blank].', 'FillInBlanks', 2, 'Atoms are the basic building blocks of all matter.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'atoms', 1);

-- Q2
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Which state of matter has a definite volume but no definite shape?', 'MCQ', 2, 'Liquids take the shape of their container but maintain a constant volume.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Liquid', 1),
(@QID, 'Gas', 0),
(@QID, 'Solid', 0),
(@QID, 'Plasma', 0);

-- Q3
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Evaporation occurs at the boiling point.', 'TrueFalse', 2, 'False. Evaporation is a surface phenomenon that can occur at any temperature. Boiling occurs throughout the liquid at a specific boiling point.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 0),
(@QID, 'False', 1);

-- Q4
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Which of the following is a physical change?', 'MCQ', 2, 'Melting ice is a change of state (physical), not a change in chemical composition.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Melting ice', 1),
(@QID, 'Burning wood', 0),
(@QID, 'Rusting iron', 0),
(@QID, 'Baking cake', 0);

-- Q5
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Atoms of the same element are identical in mass and properties.', 'TrueFalse', 2, 'This is a fundamental concept in basic chemistry, though we now know isotopes (atoms of the same element with different neutron counts) do exist.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Condensation changes gas into [blank].', 'FillInBlanks', 2, 'Condensation is the phase change from a gaseous state to a liquid state.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'liquid', 1);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Solid, liquid, and gas are the three main [blank] of matter.', 'FillInBlanks', 2, 'These are the three common states (or phases) of matter. Plasma is often considered the fourth.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'states', 1);

-------------------------------------------------------------
-- QUIZ 3: Heat and Temperature
-- Q1
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Heat flows from a [blank] object to a cold object.', 'FillInBlanks', 2, 'Heat is thermal energy in transfer, and it always moves from a region of higher temperature to a region of lower temperature.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'hot', 1);

-- Q2
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Which device is used to measure temperature?', 'MCQ', 2, 'A thermometer measures temperature. A barometer measures pressure, a hygrometer measures humidity, and an anemometer measures wind speed.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Thermometer', 1),
(@QID, 'Barometer', 0),
(@QID, 'Hygrometer', 0),
(@QID, 'Anemometer', 0);

-- Q3
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Temperature is a measure of the average kinetic energy of particles.', 'TrueFalse', 2, 'This is the definition of temperature in thermodynamics.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q4
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Conduction, convection, and radiation are methods of [blank] transfer.', 'FillInBlanks', 2, 'These are the three primary mechanisms by which heat energy moves.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'heat', 1);

-- Q5
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Which material is a good conductor of heat?', 'MCQ', 2, 'Metals, like copper, are excellent conductors because of their free-moving electrons.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Copper', 1),
(@QID, 'Wood', 0),
(@QID, 'Plastic', 0),
(@QID, 'Glass', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'When ice melts, heat is absorbed by the ice.', 'TrueFalse', 2, 'True. This is an endothermic process. The absorbed heat is called the latent heat of fusion.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Boiling of water involves a change of state from liquid to [blank].', 'FillInBlanks', 2, 'Boiling (or vaporization) is the phase transition from liquid to gas.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'gas', 1);

-------------------------------------------------------------
-- QUIZ 4: The Human Body Systems
-- Q1
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'Which system is responsible for transporting oxygen in the body?', 'MCQ', 2, 'The circulatory system uses blood to transport oxygen from the lungs (respiratory system) to the rest of the body.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Circulatory system', 1),
(@QID, 'Digestive system', 0),
(@QID, 'Respiratory system', 0),
(@QID, 'Nervous system', 0);

-- Q2
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'The heart pumps blood throughout the body.', 'TrueFalse', 2, 'The heart is the central pump of the circulatory system.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q3
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'The brain is part of the [blank] system.', 'FillInBlanks', 2, 'The brain is the central organ of the nervous system.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'nervous', 1);

-- Q4
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'Which organ filters waste from the blood?', 'MCQ', 2, 'The kidneys are the primary organs of the urinary system, responsible for filtering blood and creating urine.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Kidneys', 1),
(@QID, 'Liver', 0),
(@QID, 'Lungs', 0),
(@QID, 'Heart', 0);

-- Q5
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'The skeletal system provides structure and support to the body.', 'TrueFalse', 2, 'The skeletal system also protects internal organs and allows for movement.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'Digestion starts in the [blank].', 'FillInBlanks', 2, 'Both mechanical digestion (chewing) and chemical digestion (salivary amylase) begin in the mouth.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'mouth', 1);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'Which system controls voluntary and involuntary actions?', 'MCQ', 2, 'The nervous system controls everything you do, from thinking (voluntary) to breathing (involuntary).');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Nervous system', 1),
(@QID, 'Circulatory system', 0),
(@QID, 'Digestive system', 0),
(@QID, 'Skeletal system', 0);

-------------------------------------------------------------
-- QUIZ 5: Solar System
-- Q1
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'Which planet is known as the "Red Planet"?', 'MCQ', 2, 'Mars gets its red color from the iron oxide (rust) on its surface.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Mars', 1),
(@QID, 'Venus', 0),
(@QID, 'Jupiter', 0),
(@QID, 'Mercury', 0);

-- Q2
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'What is the largest planet in our Solar System?', 'MCQ', 2, 'Jupiter is a gas giant and is the most massive planet in our solar system.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Saturn', 0),
(@QID, 'Jupiter', 1),
(@QID, 'Neptune', 0),
(@QID, 'Earth', 0);

-- Q3 (FIXED: Was T/F, now has correct T/F options)
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'The asteroid belt is located between Mars and Jupiter.', 'TrueFalse', 2, 'The main asteroid belt is indeed located in the gap between the orbits of Mars and Jupiter.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q4 (FIXED: Was MCQ, now has correct MCQ options)
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'Which celestial body is the primary source of energy for Earth?', 'MCQ', 2, 'The Sun provides energy for almost all life on Earth through photosynthesis and drives our planet''s climate and weather.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'The Sun', 1),
(@QID, 'The Moon', 0),
(@QID, 'Jupiter', 0),
(@QID, 'Mars', 0);

-- Q5
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'Neptune is closer to the Sun than Uranus.', 'TrueFalse', 2, 'False. The order from the Sun is: ...Uranus, Neptune. Therefore, Uranus is closer.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'False', 1),
(@QID, 'True', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'The planet that takes the shortest time to orbit the Sun is [blank].', 'FillInBlanks', 2, 'Mercury is the innermost planet, and according to Kepler''s laws, it has the fastest orbital period (about 88 Earth days).');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Mercury', 1);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'The dwarf planet located in the Kuiper Belt is [blank].', 'FillInBlanks', 2, 'Pluto was reclassified as a dwarf planet in 2006 and is the largest object in the Kuiper Belt.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Pluto', 1);

/*----------------------------------------------------
  Database Maintenance & Reset Operations
----------------------------------------------------*/

DROP TABLE tblContactUs;

DROP TABLE tblRegisteredUsers;

update tblRegisteredUsers Set Password='wapp'

select * from tblRegisteredUsers

DELETE FROM tblRegisteredUsers;

DBCC CHECKIDENT ('tblRegisteredUsers', RESEED, 0);

TRUNCATE TABLE tblRegisteredUsers;

DELETE FROM tblRegisteredUsers
WHERE RID IN (13);

ALTER TABLE tblRegisteredUsers
ADD Role NVARCHAR(20) NOT NULL DEFAULT 'User';

SELECT * FROM [tblContactUs]

DROP TABLE tblContactUs;

DROP TABLE tblRegisteredUsers;

