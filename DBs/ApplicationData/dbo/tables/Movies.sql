CREATE TABLE [dbo].[Movies]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Title] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	[DateAdded] DATETIME2(3) NOT NULL,
	[UserId] NVARCHAR(128) NOT NULL,
	CONSTRAINT [PK_Movies] PRIMARY KEY ([Id]), 
)

GO

ALTER TABLE [dbo].[Movies] 
	ADD CONSTRAINT [DF_Movies_DateAdded] 
	DEFAULT SYSUTCDATETIME() 
	FOR DateAdded
GO  

CREATE INDEX [IX_Movies_Users] ON [dbo].[Movies] ([UserId])
