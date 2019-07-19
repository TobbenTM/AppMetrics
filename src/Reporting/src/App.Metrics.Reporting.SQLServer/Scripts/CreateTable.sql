-- <copyright file="CreateTable.sql" company="App Metrics Contributors">
-- Copyright (c) App Metrics Contributors. All rights reserved.
-- </copyright>

BEGIN TRY
	BEGIN TRANSACTION
		IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'dbo' AND TABLE_NAME = N'#TableName#')
		BEGIN
			CREATE TABLE [dbo].[#TableName#] (
				[Id]			BIGINT IDENTITY(1,1)	NOT NULL,
				[Timestamp]		DATETIME2				NOT NULL,	-- The timestamp of the metrics snapshot
				[Context]		NVARCHAR(256)			NOT NULL,	-- The metric's context
				[Name]			NVARCHAR(256)			NOT NULL,	-- The name of the metric
				[Field]			NVARCHAR(256)			NOT NULL,	-- The label for the metric value
				[Value]			BIGINT					NOT NULL,	-- The value of the metrics
				[Tags]			NVARCHAR(MAX)			NULL,		-- The metric's tags
				CONSTRAINT [PK_#TableName#] PRIMARY KEY CLUSTERED ([Id] ASC),
				CONSTRAINT [JSON_#TableName#_Tags] CHECK(ISJSON([Tags])=1),
			);
		END
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
END CATCH
