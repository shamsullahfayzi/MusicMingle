using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicMingle.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Concert" },
                    { 2, "Festival" },
                    { 3, "Workshop" },
                    { 4, "Open Mic" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CategoryId", "Description", "EndDateTime", "LargeImagePathsJson", "Location", "MaxAttendees", "StartDateTime", "ThumbnailImagePath", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Join us for a night of electrifying indie rock performances.", new DateTime(2025, 5, 7, 23, 0, 0, 0, DateTimeKind.Utc), null, "The Underground Venue", 150, new DateTime(2025, 5, 7, 19, 0, 0, 0, DateTimeKind.Utc), "/images/events/thumbnails/indie_rock.jpg", "Indie Rock Night" },
                    { 2, 2, "Three days of non-stop music across multiple stages.", new DateTime(2025, 6, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "Greenfield Park", 5000, new DateTime(2025, 6, 27, 12, 0, 0, 0, DateTimeKind.Utc), "/images/events/thumbnails/summer_fest.jpg", "Summer Music Fest" },
                    { 3, 3, "Learn the art of songwriting from industry professionals.", new DateTime(2025, 5, 17, 16, 0, 0, 0, DateTimeKind.Utc), null, "Creative Hub Studios", 30, new DateTime(2025, 5, 17, 10, 0, 0, 0, DateTimeKind.Utc), "/images/events/thumbnails/workshop.jpg", "Songwriting Workshop" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
