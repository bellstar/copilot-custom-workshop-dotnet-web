using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MeowWorld.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Breed = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cats",
                columns: new[] { "Id", "Age", "Breed", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 2, "Scottish Fold", new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), "おっとりした性格の猫", "Momo" },
                    { 2, 4, "Maine Coon", new DateTime(2026, 1, 11, 10, 30, 0, 0, DateTimeKind.Unspecified), "大きくて人懐っこい猫", "Sora" },
                    { 3, 1, "Munchkin", new DateTime(2026, 1, 12, 14, 15, 0, 0, DateTimeKind.Unspecified), "短い足がチャームポイント", "Hana" },
                    { 4, 6, "Bombay", new DateTime(2026, 1, 13, 16, 45, 0, 0, DateTimeKind.Unspecified), "つやのある黒毛の猫", "Kuro" },
                    { 5, 3, "Ragdoll", new DateTime(2026, 1, 14, 8, 20, 0, 0, DateTimeKind.Unspecified), "穏やかで抱っこ好きな猫", "Yuki" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cats");
        }
    }
}
