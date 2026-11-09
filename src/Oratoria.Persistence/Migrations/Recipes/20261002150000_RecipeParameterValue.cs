using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oratoria.Persistence.Migrations.Recipes
{
    /// <inheritdoc />
    public partial class RecipeParameterValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Parameters",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.Sql("""
                UPDATE "Parameters"
                SET "Value" = (SELECT v."Value" FROM "Values" v
                    WHERE v."ParameterId" = "Parameters"."ParameterId");
                """);

            migrationBuilder.DropTable(name: "Values");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    ValueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParameterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.ValueId);
                    table.ForeignKey(
                        name: "FK_Values_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "ParameterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Values_ParameterId",
                table: "Values",
                column: "ParameterId",
                unique: true);

            migrationBuilder.Sql("""
                INSERT INTO "Values" ("ValueId", "ParameterId", "Value")
                SELECT hex(randomblob(16)), "ParameterId", "Value" FROM "Parameters";
                """);

            migrationBuilder.DropColumn(name: "Value", table: "Parameters");
        }
    }
}
