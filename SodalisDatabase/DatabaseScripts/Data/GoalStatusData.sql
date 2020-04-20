SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET NOCOUNT ON;
GO

CREATE TABLE #goalStatusUpsert (
	[Id] INT PRIMARY KEY,
	[Status] NVARCHAR(64) NOT NULL,
	[Description] NVARCHAR(128) NOT NULL
);

INSERT INTO #goalStatusUpsert
VALUES
	(1, N'New', N'This goal was newly created and no progress has been made towards completing it.'),
	(2, N'In Progress', N'Steps have been taken towards completing this goal.'),
	(3, N'Completed', N'This goal was successfully completed.'),
	(4, N'Not Completed', N'This goal was not successfully completed.');

MERGE [dbo].[GoalStatuses] gs
USING #goalStatusUpsert upsert
ON (gs.[Id] = upsert.[Id])
WHEN MATCHED THEN
	UPDATE SET
		gs.[Status] = upsert.[Status],
		gs.[Description] = upsert.[Description]
WHEN NOT MATCHED THEN
	INSERT ([Id], [Status], [Description])
	VALUES (upsert.[Id], upsert.[Status], upsert.[Description]);

DROP TABLE #goalStatusUpsert;

GO
