CREATE TABLE [dbo].[MoviesUsersDisposition]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[MovieId] INT NOT NULL,
	[UserId] NVARCHAR(128) NOT NULL,
	[Like] BIT NOT NULL,
	[DateTimeSet] DATETIME2(3) NOT NULL,
	CONSTRAINT [PK_MoviesUsersDisposition]	PRIMARY KEY ([Id]),
	CONSTRAINT [FK_MoviesUsersDisposition_Movies] FOREIGN KEY (MovieId) REFERENCES dbo.Movies ([Id])
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	CONSTRAINT [AK_MoviesUsersDisposition_MoviePerUser] UNIQUE ([MovieId], [UserId]), 
)

GO

ALTER TABLE dbo.MoviesUsersDisposition
	ADD CONSTRAINT [DF_MoviesUsersDisposition_DateTimeSet] 
	DEFAULT SYSUTCDATETIME() 
	FOR DateTimeSet

GO