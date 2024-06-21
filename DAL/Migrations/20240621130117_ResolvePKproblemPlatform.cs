using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ResolvePKproblemPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_platforms",
                table: "platforms");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Quai",
                table: "platforms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Perron_Id",
                table: "platforms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_platforms",
                table: "platforms",
                column: "Perron_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_platforms",
                table: "platforms");

            migrationBuilder.DropColumn(
                name: "Perron_Id",
                table: "platforms");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Quai",
                table: "platforms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_platforms",
                table: "platforms",
                column: "Id_Quai");
        }
    }
}
