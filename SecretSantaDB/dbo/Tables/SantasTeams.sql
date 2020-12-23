CREATE TABLE [dbo].[SantasTeams] (
    [TeamId]   INT           NOT NULL,
    [TeamName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SantasTeams] PRIMARY KEY CLUSTERED ([TeamId] ASC)
);

