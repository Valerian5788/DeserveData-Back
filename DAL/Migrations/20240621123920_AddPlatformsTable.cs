using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatformsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "platforms",
                columns: table => new
                {
                    Id_Quai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hauteur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongNameFrench = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongNameDutch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Station = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platforms", x => x.Id_Quai);
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
                name: "platforms");
        }
    }
}
