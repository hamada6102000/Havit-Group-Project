using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HavitGroup.Migrations
{
    /// <inheritdoc />
    public partial class addcontactusimages3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationHeading",
                table: "SiteSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationSubheading",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationHeading",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "LocationSubheading",
                table: "SiteSettings");
        }
    }
}
