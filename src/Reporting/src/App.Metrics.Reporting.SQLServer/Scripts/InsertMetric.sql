-- <copyright file="InsertMetric.sql" company="App Metrics Contributors">
-- Copyright (c) App Metrics Contributors. All rights reserved.
-- </copyright>

INSERT INTO @TableName (
		[Timestamp],
		[Context],
		[Name],
		[Field],
		[Value],
		[Tags]
	)
	VALUES
	(
		@Timestamp,
		@Context,
		@Name,
		@Field,
		@Value,
		@Tags
	)
