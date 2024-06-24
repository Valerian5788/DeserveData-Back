using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "busStop",
                columns: table => new
                {
                    StopId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lon = table.Column<double>(type: "float", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_busStop", x => x.StopId);
                });

            migrationBuilder.CreateTable(
                name: "stations",
                columns: table => new
                {
                    Id_Station = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Official_Station_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name_fr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_nl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lon = table.Column<float>(type: "real", nullable: false),
                    lat = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stations", x => x.Id_Station);
                });

            migrationBuilder.CreateTable(
                name: "facilities",
                columns: table => new
                {
                    Id_Station = table.Column<int>(type: "int", nullable: false),
                    PaidToilets = table.Column<bool>(type: "bit", nullable: false),
                    Taxi = table.Column<bool>(type: "bit", nullable: false),
                    LuggageLockers = table.Column<bool>(type: "bit", nullable: false),
                    FreeToilets = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_facilities", x => x.Id_Station);
                    table.ForeignKey(
                        name: "FK_facilities_stations_Id_Station",
                        column: x => x.Id_Station,
                        principalTable: "stations",
                        principalColumn: "Id_Station",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "platforms",
                columns: table => new
                {
                    Perron_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Quai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hauteur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongNameFrench = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongNameDutch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Station = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platforms", x => x.Perron_Id);
                    table.ForeignKey(
                        name: "FK_platforms_stations_Id_Station",
                        column: x => x.Id_Station,
                        principalTable: "stations",
                        principalColumn: "Id_Station",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_platforms_Id_Station",
                table: "platforms",
                column: "Id_Station");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "busStop");

            migrationBuilder.DropTable(
                name: "facilities");

            migrationBuilder.DropTable(
                name: "platforms");

            migrationBuilder.DropTable(
                name: "stations");
        }
    }
}
