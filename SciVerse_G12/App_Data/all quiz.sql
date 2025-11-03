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


