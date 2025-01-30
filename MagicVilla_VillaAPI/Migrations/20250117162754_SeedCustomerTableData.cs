using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCustomerTableData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Age", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, 31, "Jacopo", "Grazioli" },
                    { 2, 35, "Paolo", "Rossi" },
                    { 3, 23, "Franco", "Bianchi" }
                });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 987, DateTimeKind.Local).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 987, DateTimeKind.Local).AddTicks(7693));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 987, DateTimeKind.Local).AddTicks(7712));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 980, DateTimeKind.Local).AddTicks(9098));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 986, DateTimeKind.Local).AddTicks(760));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 986, DateTimeKind.Local).AddTicks(819));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 986, DateTimeKind.Local).AddTicks(829));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 17, 27, 52, 986, DateTimeKind.Local).AddTicks(835));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 184, DateTimeKind.Local).AddTicks(3064));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 184, DateTimeKind.Local).AddTicks(3553));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 184, DateTimeKind.Local).AddTicks(3565));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 178, DateTimeKind.Local).AddTicks(5057));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 183, DateTimeKind.Local).AddTicks(1361));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 183, DateTimeKind.Local).AddTicks(1401));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 183, DateTimeKind.Local).AddTicks(1408));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 16, 49, 25, 183, DateTimeKind.Local).AddTicks(1413));
        }
    }
}
