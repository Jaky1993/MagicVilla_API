using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 688, DateTimeKind.Local).AddTicks(3240));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 688, DateTimeKind.Local).AddTicks(3522));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 688, DateTimeKind.Local).AddTicks(3530));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 685, DateTimeKind.Local).AddTicks(1352));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 687, DateTimeKind.Local).AddTicks(5984));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 687, DateTimeKind.Local).AddTicks(6013));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 687, DateTimeKind.Local).AddTicks(6017));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreateDate",
                value: new DateTime(2025, 1, 15, 10, 37, 58, 687, DateTimeKind.Local).AddTicks(6020));
        }
    }
}
