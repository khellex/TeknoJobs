IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250611060548_Added_Departments_tbl_to_Db', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Locations] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [City] nvarchar(max) NULL,
    [State] nvarchar(max) NULL,
    [Country] nvarchar(max) NULL,
    [Zip] nvarchar(max) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250611094037_Added_Locations_tbl_to_Db', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Jobs] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(max) NULL,
    [Title] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [LocationId] int NOT NULL,
    [PostedDate] datetime2 NULL,
    [ClosingDate] datetime2 NULL,
    [DepartmentId] int NOT NULL,
    CONSTRAINT [PK_Jobs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Jobs_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Jobs_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Jobs_DepartmentId] ON [Jobs] ([DepartmentId]);
GO

CREATE INDEX [IX_Jobs_LocationId] ON [Jobs] ([LocationId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250611102801_Added_Jobs_tbl_to_Db', N'8.0.16');
GO

COMMIT;
GO

