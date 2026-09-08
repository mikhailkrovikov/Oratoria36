using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oratoria.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeviceSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceSettings",
                columns: table => new
                {
                    DeviceKey = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Max = table.Column<string>(type: "TEXT", nullable: true),
                    Min = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSettings", x => new { x.DeviceKey, x.Name });
                });
        }
    }
}
