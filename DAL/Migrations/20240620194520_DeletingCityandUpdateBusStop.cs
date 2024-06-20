using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeletingCityandUpdateBusStop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "busStopByCityNames");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "busStop");

            migrationBuilder.CreateTable(
                name: "busStopByCityNames",
                columns: table => new
                {
                    city_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    lat = table.Column<double>(type: "float", nullable: false),
                    lon = table.Column<double>(type: "float", nullable: false),
                    stop_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stop_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_busStopByCityNames", x => x.city_name);
                });
        }
    }
}
