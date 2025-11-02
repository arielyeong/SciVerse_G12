CREATE TABLE [dbo].[tblQuiz] (
    [QuizID]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (200) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Chapter]      NVARCHAR (100) NULL,
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
    [Explanation]     NVARCHAR (MAX) NULL,
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

--===========================================================
-- INSERT QUIZZES
--===========================================================
INSERT INTO [dbo].[tblQuiz] (Title, Description, Chapter, TimeLimit, ImageURL, CreatedBy, AttemptLimit)
VALUES 
('Forces and Motion', 'A quiz on the fundamental concepts of forces, motion, and Newton''s laws.', 'Chapter 1', 0, '/Images/Quiz/force-and-motion.png', 10, 10),
('The Nature of Matter', 'Test your knowledge on the states of matter, atoms, and physical vs. chemical changes.', 'Chapter 2', 0, '/Images/Quiz/the-nature-of-matter.png', 10, 10),
('Heat and Temperature', 'Explore concepts of heat transfer, temperature measurement, and thermodynamics.', 'Chapter 3', 0, '/Images/Quiz/heat-and-temp.png', 10, 10),
('The Human Body Systems', 'A journey through the major systems of the human body.', 'Chapter 4', 0, '/Images/Quiz/the-human-body-systems.png', 10, 10),
('Solar System', 'Explore the planets, celestial bodies, and wonders of our solar system.', 'Chapter 5', 20, '/Images/Quiz/solar-system.png', 10, 10);

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
VALUES (1, 'An object at rest stays at rest unless acted upon by an external force.', 'T/F', 2, 'This is the definition of Newton''s First Law of Motion, also known as the law of inertia.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q3
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'Force equals mass times [blank].', 'FIB', 2, 'This is Newton''s Second Law, mathematically expressed as F = ma.');
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
VALUES (1, 'Gravity always acts toward the center of the Earth.', 'T/F', 2, 'Gravity is an attractive force that pulls objects toward the center of mass. For Earth, this is its center.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'A car moving at constant velocity has zero net force.', 'T/F', 2, 'According to Newton''s First Law, constant velocity (including zero velocity) means the net force acting on the object is zero.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (1, 'The rate of change of velocity is called [blank].', 'FIB', 2, 'Acceleration is the vector quantity that describes the rate at which an object changes its velocity.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'acceleration', 1);

-------------------------------------------------------------
-- QUIZ 2: The Nature of Matter
-- Q1
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Matter is made up of tiny particles called [blank].', 'FIB', 2, 'Atoms are the basic building blocks of all matter.');
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
VALUES (2, 'Evaporation occurs at the boiling point.', 'T/F', 2, 'False. Evaporation is a surface phenomenon that can occur at any temperature. Boiling occurs throughout the liquid at a specific boiling point.');
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
VALUES (2, 'Atoms of the same element are identical in mass and properties.', 'T/F', 2, 'This is a fundamental concept in basic chemistry, though we now know isotopes (atoms of the same element with different neutron counts) do exist.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Condensation changes gas into [blank].', 'FIB', 2, 'Condensation is the phase change from a gaseous state to a liquid state.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'liquid', 1);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (2, 'Solid, liquid, and gas are the three main [blank] of matter.', 'FIB', 2, 'These are the three common states (or phases) of matter. Plasma is often considered the fourth.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'states', 1);

-------------------------------------------------------------
-- QUIZ 3: Heat and Temperature
-- Q1
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Heat flows from a [blank] object to a [blank] object.', 'FIB', 2, 'Heat is thermal energy in transfer, and it always moves from a region of higher temperature to a region of lower temperature.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect)
VALUES (@QID, 'hot; cold', 1);

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
VALUES (3, 'Temperature is a measure of the average kinetic energy of particles.', 'T/F', 2, 'This is the definition of temperature in thermodynamics.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q4
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Conduction, convection, and radiation are methods of [blank] transfer.', 'FIB', 2, 'These are the three primary mechanisms by which heat energy moves.');
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
VALUES (3, 'When ice melts, heat is absorbed by the ice.', 'T/F', 2, 'True. This is an endothermic process. The absorbed heat is called the latent heat of fusion.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (3, 'Boiling of water involves a change of state from liquid to [blank].', 'FIB', 2, 'Boiling (or vaporization) is the phase transition from liquid to gas.');
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
VALUES (4, 'The heart pumps blood throughout the body.', 'T/F', 2, 'The heart is the central pump of the circulatory system.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q3
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'The brain is part of the [blank] system.', 'FIB', 2, 'The brain is the central organ of the nervous system.');
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
VALUES (4, 'The skeletal system provides structure and support to the body.', 'T/F', 2, 'The skeletal system also protects internal organs and allows for movement.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'True', 1),
(@QID, 'False', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (4, 'Digestion starts in the [blank].', 'FIB', 2, 'Both mechanical digestion (chewing) and chemical digestion (salivary amylase) begin in the mouth.');
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
VALUES (5, 'The asteroid belt is located between Mars and Jupiter.', 'T/F', 2, 'The main asteroid belt is indeed located in the gap between the orbits of Mars and Jupiter.');
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
VALUES (5, 'Neptune is closer to the Sun than Uranus.', 'T/F', 2, 'False. The order from the Sun is: ...Uranus, Neptune. Therefore, Uranus is closer.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'False', 1),
(@QID, 'True', 0);

-- Q6
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'The planet that takes the shortest time to orbit the Sun is [blank].', 'FIB', 2, 'Mercury is the innermost planet, and according to Kepler''s laws, it has the fastest orbital period (about 88 Earth days).');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Mercury', 1);

-- Q7
INSERT INTO tblQuestion (QuizID, QuestionText, QuestionType, Marks, Explanation)
VALUES (5, 'The dwarf planet located in the Kuiper Belt is [blank].', 'FIB', 2, 'Pluto was reclassified as a dwarf planet in 2006 and is the largest object in the Kuiper Belt.');
SET @QID = SCOPE_IDENTITY();
INSERT INTO tblOptions (QuestionID, optionText, isCorrect) VALUES
(@QID, 'Pluto', 1);



--reset script if tbl id got prob
-- 1. Delete all existing rows in tblOptions first (because of FK to tblQuestion)
DELETE FROM tblOptions;

-- 2. Delete all Questions
DELETE FROM tblQuestion;

-- 3. Reset identity seed for tblQuestion
DBCC CHECKIDENT ('tblQuestion', RESEED, 0);

-- 4. (Optional) Reset identity seed for tblOptions too, if it has an identity column
DBCC CHECKIDENT ('tblOptions', RESEED, 0);
