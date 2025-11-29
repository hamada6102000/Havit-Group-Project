-- Script to manually add FooterDescription column or create SiteSettings table
-- Run this script directly in SQL Server Management Studio or Azure Data Studio

-- Check if SiteSettings table exists, if not create it with FooterDescription
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SiteSettings')
BEGIN
    CREATE TABLE [SiteSettings] (
        [Id] int NOT NULL,
        [Email] nvarchar(200) NULL,
        [Phone] nvarchar(50) NULL,
        [Mobile] nvarchar(50) NULL,
        [AddressLine1] nvarchar(200) NULL,
        [AddressLine2] nvarchar(200) NULL,
        [City] nvarchar(100) NULL,
        [State] nvarchar(100) NULL,
        [PostalCode] nvarchar(20) NULL,
        [Country] nvarchar(100) NULL,
        [CompanyName] nvarchar(200) NULL,
        [Tagline] nvarchar(500) NULL,
        [FooterDescription] nvarchar(500) NULL,
        [Mission] nvarchar(2000) NULL,
        [Values] nvarchar(2000) NULL,
        [WhyChooseUs] nvarchar(2000) NULL,
        [FacebookUrl] nvarchar(500) NULL,
        [TwitterUrl] nvarchar(500) NULL,
        [LinkedInUrl] nvarchar(500) NULL,
        [InstagramUrl] nvarchar(500) NULL,
        [YouTubeUrl] nvarchar(500) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_SiteSettings] PRIMARY KEY ([Id])
    );
    
    -- Insert default record
    INSERT INTO [SiteSettings] ([Id], [CreatedAt], [FooterDescription])
    VALUES (1, GETUTCDATE(), 'Leading the way in innovation and excellence. Building sustainable solutions for tomorrow''s challenges.');
END
ELSE
BEGIN
    -- Add column only if it doesn't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SiteSettings') AND name = 'FooterDescription')
    BEGIN
        ALTER TABLE [SiteSettings] ADD [FooterDescription] nvarchar(500) NULL;
        
        -- Update existing record with default value if exists
        UPDATE [SiteSettings] 
        SET [FooterDescription] = 'Leading the way in innovation and excellence. Building sustainable solutions for tomorrow''s challenges.'
        WHERE [Id] = 1 AND [FooterDescription] IS NULL;
    END
END

-- Also ensure the migration history is updated
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251128133728_AddFooterDescription')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251128133728_AddFooterDescription', '9.0.0');
END

