SET ANSI_NULLs ON;
SET QUOTED_IDENTIFIER ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET NOCOUNT ON;
GO

CREATE OR ALTER PROCEDURE [dbo].[GetFriendshipsByUserId]
	@userId INT,
	@pageNumber INT,
	@pageSize INT
AS

	SELECT [SenderId], [ReceiverId], [Accepted], [CreatedDate]
	FROM
		(SELECT
			ROW_NUMBER() OVER (ORDER BY [SenderId], [ReceiverId]) AS [RowNumber],
			[SenderId],
			[ReceiverId],
			[Accepted],
			[CreatedDate]
		FROM [dbo].[Friendships]
		WHERE [SenderId] = @userId OR [ReceiverId] = @userId) AS RowConstrainedTable
	WHERE [RowNumber] > ((@pageNumber - 1) * @pageSize)
	AND [RowNumber] <= (@pageSize * @pageNumber)
	ORDER BY [RowNumber];

GO