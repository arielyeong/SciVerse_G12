SELECT * FROM [dbo].[tblExperimentSimulation];
DROP TABLE [dbo].[tblExperimentSimulation];
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

INSERT INTO [dbo].[tblExperimentSimulation](Title, Description, Chapter, Instruction,URL,Image)
VALUES
('My solar system', 
'Explore how gravity controls the motion of planets, moons, and stars. Create your own solar system and observe how the objects move and interact with each other based on mass, distance, and velocity.',
,'Chapter 5',
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
