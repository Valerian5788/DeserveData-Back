using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModifPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongNameDutch",
                table: "platforms");

            migrationBuilder.DropColumn(
                name: "LongNameFrench",
                table: "platforms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LongNameDutch",
                table: "platforms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LongNameFrench",
                table: "platforms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
