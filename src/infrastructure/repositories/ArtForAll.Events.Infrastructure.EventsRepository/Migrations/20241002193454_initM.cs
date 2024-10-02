using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtForAll.Events.Infrastructure.EFRepository.Migrations
{
    /// <inheritdoc />
    public partial class initM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "dateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "dateTime", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Price_CurrencyExchange = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Price_MonetaryValue = table.Column<float>(type: "real", maxLength: 10, nullable: false),
                    State = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "dateTime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "dateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EventId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_EventId",
                table: "Images",
                column: "EventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
