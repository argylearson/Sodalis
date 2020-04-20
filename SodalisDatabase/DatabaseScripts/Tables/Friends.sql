SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET NOCOUNT ON;
GO

IF OBJECT_ID(N'dbo.Friendships', N'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[Friendships](
	[SenderId] INT FOREIGN KEY REFERENCES [dbo].[Users]([Id]),
	[ReceiverId] INT FOREIGN KEY REFERENCES [dbo].[Users]([Id]),
	[Accepted] BIT DEFAULT 0,
	[CreatedDate] DATETIME2 DEFAULT CURRENT_TIMESTAMP
	PRIMARY KEY([SenderId], [ReceiverId])
	) WITH (DATA_COMPRESSION = PAGE);
END;

IF INDEXPROPERTY(OBJECT_ID(N'dbo.Friendships'), 'IX_Friendships_ReceiverId_SenderId', 'IndexId') IS NULL
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Friendships_ReceiverId_SenderId] ON [dbo].[Friendships]([ReceiverId], [SenderId]);
END;

GO
