CREATE TABLE [dbo].[tblExperimentSimulation] (
    [SimulationID]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (200) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Chapter]      NVARCHAR (100) NULL,
    [Instruction]  NVARCHAR (MAX) NULL,
    [Username]     NVARCHAR (50)  NULL,
    [LastModify]   DATETIME       NULL,
    [URL]          NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([SimulationID] ASC),
    FOREIGN KEY ([Username]) REFERENCES [dbo].[tblRegisteredUsers] ([username]) ON DELETE SET NULL
);

INSERT INTO [dbo].[tblExperimentSimulation](Title, Description, Chapter, Instruction, URL)
VALUES
('Pressure and density', 
'Explore how pressure changes with depth and how fluid density affects the pressure acting on submerged objects.','Chapter 6',
'1. Adjust the depth slider to observe how pressure increases with depth. 
 2. Change the liquid type to compare the effect of density on pressure. 
 3. Record the pressure readings for each liquid at different depths. 
 4. Analyze the data to conclude how pressure relates to depth and density.'
 ,'https://phet.colorado.edu/sims/html/under-pressure/latest/under-pressure_en.html'),

('Reaction of acid and reactive metal','Observe how reactive metals like magnesium react with acids to produce hydrogen gas and form salts.','Chapter 2', 
'1. Add the metal pieces into the virtual test tube. 
 3. Pour dilute acid into the test tube to start the reaction. 
 4. Observe the bubbling and record the amount of hydrogen gas produced. 
 5. Use the “Test Hydrogen” button to perform the pop test for hydrogen. 
 6. Repeat with different metals or acid strengths to compare reaction rates.'
 ,'https://phet.colorado.edu/sims/html/ph-scale/latest/ph-scale_en.html')
