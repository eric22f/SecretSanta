CREATE TABLE [dbo].[SelectedSantas] (
    [SantaId]         INT      NOT NULL,
    [SelectedSantaId] INT      NOT NULL,
    [CreatedDate]     DATETIME CONSTRAINT [DF_SelectedSantas_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SelectedSantas] PRIMARY KEY CLUSTERED ([SantaId] ASC),
    CONSTRAINT [FK_SelectedSantas_SelectedSantas] FOREIGN KEY ([SantaId]) REFERENCES [dbo].[SelectedSantas] ([SantaId])
);

