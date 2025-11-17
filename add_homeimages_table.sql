-- SQL Script to add HomeImages table if it doesn't exist
-- Run this directly in SQL Server Management Studio or via sqlcmd

-- Check if HomeImages table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HomeImages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[HomeImages] (
        [Id] int NOT NULL IDENTITY(1,1),
        [ImagePath] nvarchar(500) NOT NULL,
        [OriginalFileName] nvarchar(255) NULL,
        [AltText] nvarchar(200) NULL,
        [DisplayOrder] int NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_HomeImages] PRIMARY KEY ([Id])
    );

    -- Create indexes
    CREATE INDEX [IX_HomeImages_DisplayOrder] ON [dbo].[HomeImages] ([DisplayOrder]);
    CREATE INDEX [IX_HomeImages_IsActive] ON [dbo].[HomeImages] ([IsActive]);

    PRINT 'HomeImages table created successfully.';
END
ELSE
BEGIN
    PRINT 'HomeImages table already exists.';
END

-- Mark migrations as applied in the migration history table
-- Only insert if they don't already exist

IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251113225549_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251113225549_InitialCreate', '9.0.0');
END

IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251114000000_AddSiteSettings')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251114000000_AddSiteSettings', '9.0.0');
END

IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251117213846_addHomeImges')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251117213846_addHomeImges', '9.0.0');
END

PRINT 'Migration history updated.';

