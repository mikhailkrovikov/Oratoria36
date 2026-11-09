using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oratoria.Persistence.Migrations.Recipes
{
    public partial class RecipeParameterKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Parameter",
                table: "Parameters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE "Parameters"
                SET "Parameter" = CASE "Name"
                    WHEN 'Время нагрева, сек' THEN 1
                    WHEN 'Мощность нагрева, Вт' THEN 2
                    WHEN 'Давление, Па' THEN 3
                    WHEN 'Расход, л/ч' THEN 4
                    WHEN 'Время напыления, сек' THEN 5
                    WHEN 'Время отпыла, сек' THEN 6
                    WHEN 'Мощность магнетрона 1, Вт' THEN 7
                    WHEN 'Мощность магнетрона 2, Вт' THEN 8
                    WHEN 'Мощность магнетрона 3, Вт' THEN 9
                    WHEN 'Температура нагрева, °C' THEN 10
                    ELSE NULL
                END;
                """);

            migrationBuilder.DropColumn(name: "Name", table: "Parameters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Parameters",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE "Parameters"
                SET "Name" = CASE "Parameter"
                    WHEN 1 THEN 'Время нагрева, сек'
                    WHEN 2 THEN 'Мощность нагрева, Вт'
                    WHEN 3 THEN 'Давление, Па'
                    WHEN 4 THEN 'Расход, л/ч'
                    WHEN 5 THEN 'Время напыления, сек'
                    WHEN 6 THEN 'Время отпыла, сек'
                    WHEN 7 THEN 'Мощность магнетрона 1, Вт'
                    WHEN 8 THEN 'Мощность магнетрона 2, Вт'
                    WHEN 9 THEN 'Мощность магнетрона 3, Вт'
                    WHEN 10 THEN 'Температура нагрева, °C'
                    ELSE NULL
                END;
                """);

            migrationBuilder.DropColumn(name: "Parameter", table: "Parameters");
        }
    }
}
