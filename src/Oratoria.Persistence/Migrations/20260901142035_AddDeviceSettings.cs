using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oratoria.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceSettings",
                columns: table => new
                {
                    DeviceKey = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Min = table.Column<string>(type: "TEXT", nullable: true),
                    Max = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSettings", x => new { x.DeviceKey, x.Name });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceSettings");
        }
    }
}
