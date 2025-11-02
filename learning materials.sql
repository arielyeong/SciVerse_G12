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