-- SQL Script to Create SiteSettings Table
-- Run this script directly in SQL Server Management Studio if migration fails
-- Database: HavitGroupDb
-- Server: HAMADA-MOSTAFA-\SQLEXPRESS

USE [HavitGroupDb];
GO

-- Check if table already exists, drop it if needed
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND type in (N'U'))
BEGIN
    PRINT 'Table SiteSettings already exists. Dropping it...';
    DROP TABLE [dbo].[SiteSettings];
END
GO

-- Create the SiteSettings table
CREATE TABLE [dbo].[SiteSettings] (
    [Id] INT NOT NULL PRIMARY KEY,
    [Email] NVARCHAR(200) NULL,
    [Phone] NVARCHAR(50) NULL,
    [Mobile] NVARCHAR(50) NULL,
    [AddressLine1] NVARCHAR(200) NULL,
    [AddressLine2] NVARCHAR(200) NULL,
    [City] NVARCHAR(100) NULL,
    [State] NVARCHAR(100) NULL,
    [PostalCode] NVARCHAR(20) NULL,
    [Country] NVARCHAR(100) NULL,
    [CompanyName] NVARCHAR(200) NULL,
    [Tagline] NVARCHAR(500) NULL,
    [Mission] NVARCHAR(2000) NULL,
    [Values] NVARCHAR(2000) NULL,
    [WhyChooseUs] NVARCHAR(2000) NULL,
    [FacebookUrl] NVARCHAR(500) NULL,
    [TwitterUrl] NVARCHAR(500) NULL,
    [LinkedInUrl] NVARCHAR(500) NULL,
    [InstagramUrl] NVARCHAR(500) NULL,
    [YouTubeUrl] NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL
);
GO

-- Insert default record with Id = 1 (singleton pattern)
INSERT INTO [dbo].[SiteSettings] ([Id], [CreatedAt])
VALUES (1, GETUTCDATE());
GO

PRINT 'SiteSettings table created successfully!';
PRINT 'You can now restart your application and use /Admin/Settings';
GO

