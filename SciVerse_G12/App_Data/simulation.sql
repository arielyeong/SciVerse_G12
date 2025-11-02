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
('Pressure and density', 
'Explore how pressure changes with depth and how fluid density affects the pressure acting on submerged objects.','Chapter 6',
'1. Adjust the depth slider to observe how pressure increases with depth. 
 2. Change the liquid type to compare the effect of density on pressure. 
 3. Record the pressure readings for each liquid at different depths. 
 4. Analyze the data to conclude how pressure relates to depth and density.',
 'https://phet.colorado.edu/sims/html/under-pressure/latest/under-pressure_en.html',
 'https://phet.colorado.edu/sims/html/under-pressure/latest/under-pressure-600.png'),

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
