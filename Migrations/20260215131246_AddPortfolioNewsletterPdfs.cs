using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HavitGroup.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioNewsletterPdfs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewsletterPdfOriginalName",
                table: "SiteSettings",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsletterPdfPath",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioPdfOriginalName",
                table: "SiteSettings",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioPdfPath",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsletterPdfOriginalName",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "NewsletterPdfPath",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioPdfOriginalName",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioPdfPath",
                table: "SiteSettings");
        }
    }
}
