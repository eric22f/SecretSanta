CREATE TABLE [dbo].[SecretSantas] (
    [SantaId]         INT             IDENTITY (1, 1) NOT NULL,
    [FullName]        NVARCHAR (75)   NOT NULL,
    [AddressAndNotes] NVARCHAR (2000) NULL,
    [Selected]        BIT             CONSTRAINT [DF_SecretSantas_Selected] DEFAULT ((0)) NOT NULL,
    [UserCode]        NVARCHAR (50)   NULL,
    [TeamId]          INT             CONSTRAINT [DF_SecretSantas_TeamId] DEFAULT ((0)) NOT NULL,
    [CreatedDate]     DATETIME        CONSTRAINT [DF_SecretSantas_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [SentGift]        BIT             CONSTRAINT [DF_SecretSantas_SentGift] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SecretSantas] PRIMARY KEY CLUSTERED ([SantaId] ASC)
);

