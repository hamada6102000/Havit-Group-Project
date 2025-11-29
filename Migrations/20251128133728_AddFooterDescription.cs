using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HavitGroup.Migrations
{
    /// <inheritdoc />
    public partial class AddFooterDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if SiteSettings table exists, if not create it with FooterDescription
            migrationBuilder.Sql(@"
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
                END
            ");

            // Add column only if table exists and column doesn't exist
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'SiteSettings')
                AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SiteSettings') AND name = 'FooterDescription')
                BEGIN
                    ALTER TABLE [SiteSettings] ADD [FooterDescription] nvarchar(500) NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SiteSettings') AND name = 'FooterDescription')
                BEGIN
                    ALTER TABLE [SiteSettings] DROP COLUMN [FooterDescription];
                END
            ");
        }
    }
}
