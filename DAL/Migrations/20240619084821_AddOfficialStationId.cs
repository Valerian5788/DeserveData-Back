using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficialStationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_facilities_stations_fr_Id_Stations_fr",
                table: "facilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stations_fr",
                table: "stations_fr");

            migrationBuilder.RenameTable(
                name: "stations_fr",
                newName: "stations");

            migrationBuilder.RenameColumn(
                name: "Id_Stations_fr",
                table: "facilities",
                newName: "Id_Station");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "stations",
                newName: "name_nl");

            migrationBuilder.RenameColumn(
                name: "Id_Stations_fr",
                table: "stations",
                newName: "Id_Station");

            migrationBuilder.AddColumn<string>(
                name: "Official_Station_id",
                table: "stations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name_eng",
                table: "stations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name_fr",
                table: "stations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stations",
                table: "stations",
                column: "Id_Station");

            migrationBuilder.AddForeignKey(
                name: "FK_facilities_stations_Id_Station",
                table: "facilities",
                column: "Id_Station",
                principalTable: "stations",
                principalColumn: "Id_Station",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_facilities_stations_Id_Station",
                table: "facilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stations",
                table: "stations");

            migrationBuilder.DropColumn(
                name: "Official_Station_id",
                table: "stations");

            migrationBuilder.DropColumn(
                name: "name_eng",
                table: "stations");

            migrationBuilder.DropColumn(
                name: "name_fr",
                table: "stations");

            migrationBuilder.RenameTable(
                name: "stations",
                newName: "stations_fr");

            migrationBuilder.RenameColumn(
                name: "Id_Station",
                table: "facilities",
                newName: "Id_Stations_fr");

            migrationBuilder.RenameColumn(
                name: "name_nl",
                table: "stations_fr",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id_Station",
                table: "stations_fr",
                newName: "Id_Stations_fr");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stations_fr",
                table: "stations_fr",
                column: "Id_Stations_fr");

            migrationBuilder.AddForeignKey(
                name: "FK_facilities_stations_fr_Id_Stations_fr",
                table: "facilities",
                column: "Id_Stations_fr",
                principalTable: "stations_fr",
                principalColumn: "Id_Stations_fr",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
