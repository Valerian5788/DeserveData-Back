using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStationFacilitiesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "facilities",
                columns: table => new
                {
                    Id_Stations_fr = table.Column<int>(type: "int", nullable: false),
                    PaidToilets = table.Column<bool>(type: "bit", nullable: false),
                    Taxi = table.Column<bool>(type: "bit", nullable: false),
                    LuggageLockers = table.Column<bool>(type: "bit", nullable: false),
                    TVMCount = table.Column<bool>(type: "bit", nullable: false),
                    Wifi = table.Column<bool>(type: "bit", nullable: false),
                    BikesPointPresence = table.Column<bool>(type: "bit", nullable: false),
                    CambioInformation = table.Column<bool>(type: "bit", nullable: false),
                    ConnectingBusesPresence = table.Column<bool>(type: "bit", nullable: false),
                    ConnectingTramPresence = table.Column<bool>(type: "bit", nullable: false),
                    ParkingPlacesForPMR = table.Column<int>(type: "int", nullable: false),
                    PMRToilets = table.Column<bool>(type: "bit", nullable: false),
                    Escalator = table.Column<bool>(type: "bit", nullable: false),
                    BlueBikesPresence = table.Column<bool>(type: "bit", nullable: false),
                    PMRAssistance = table.Column<bool>(type: "bit", nullable: false),
                    LiftOnPlatform = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facilities", x => x.Id_Stations_fr);
                    table.ForeignKey(
                        name: "FK_facilities_stations_fr_Id_Stations_fr",
                        column: x => x.Id_Stations_fr,
                        principalTable: "stations_fr",
                        principalColumn: "Id_Stations_fr",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "facilities");
        }
    }
}
