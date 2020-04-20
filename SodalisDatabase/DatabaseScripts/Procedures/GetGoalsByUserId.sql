SET ANSI_NULLs ON;
SET QUOTED_IDENTIFIER ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET NOCOUNT ON;
GO

CREATE OR ALTER PROCEDURE [dbo].[GetGoalsByUserId]
	@userId INT,
	@includePrivate BIT,
	@pageNumber INT,
	@pageSize INT
AS

	SELECT [Id], [UserId], [Title], [Description], [CreatedDate], [Status], [IsPublic]
	FROM
		(SELECT
			ROW_NUMBER() OVER (ORDER BY [Id]) AS [RowNumber],
			[Id],
			[UserId],
			[Title],
			[Description],
			[CreatedDate],
			[Status],
			[IsPublic]
		FROM [dbo].[Goals]
		WHERE [UserId] = @userId AND (@includePrivate = 1 OR [IsPublic] = 1)) AS RowConstrainedTable
	WHERE [RowNumber] > ((@pageNumber - 1) * @pageSize)
	AND [RowNumber] <= (@pageSize * @pageNumber)
	ORDER BY [RowNumber];

GO