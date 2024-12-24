using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddVillaID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaID",
                table: "VillaNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 1,
                columns: new[] { "CreateDate", "VillaID" },
                values: new object[] { new DateTime(2024, 12, 24, 10, 31, 29, 991, DateTimeKind.Local).AddTicks(7379), 0 });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 2,
                columns: new[] { "CreateDate", "VillaID" },
                values: new object[] { new DateTime(2024, 12, 24, 10, 31, 29, 991, DateTimeKind.Local).AddTicks(7692), 0 });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                columns: new[] { "CreateDate", "VillaID" },
                values: new object[] { new DateTime(2024, 12, 24, 10, 31, 29, 991, DateTimeKind.Local).AddTicks(7700), 0 });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 12, 24, 10, 31, 29, 988, DateTimeKind.Local).AddTicks(522));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 12, 24, 10, 31, 29, 990, DateTimeKind.Local).AddTicks(9695));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 12, 24, 10, 31, 29, 990, DateTimeKind.Local).AddTicks(9730));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2024, 12, 24, 10, 31, 29, 990, DateTimeKind.Local).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreateDate",
                value: new DateTime(2024, 12, 24, 10, 31, 29, 990, DateTimeKind.Local).AddTicks(9737));

            migrationBuilder.CreateIndex(
                name: "IX_VillaNumbers_VillaID",
                table: "VillaNumbers",
                column: "VillaID");

            migrationBuilder.AddForeignKey(
                name: "FK_VillaNumbers_Villas_VillaID",
                table: "VillaNumbers",
                column: "VillaID",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillaNumbers_Villas_VillaID",
                table: "VillaNumbers");

            migrationBuilder.DropIndex(
                name: "IX_VillaNumbers_VillaID",
                table: "VillaNumbers");

            migrationBuilder.DropColumn(
                name: "VillaID",
                table: "VillaNumbers");

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 666, DateTimeKind.Local).AddTicks(8174));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 666, DateTimeKind.Local).AddTicks(8601));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 666, DateTimeKind.Local).AddTicks(8616));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 663, DateTimeKind.Local).AddTicks(4640));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 665, DateTimeKind.Local).AddTicks(7684));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 665, DateTimeKind.Local).AddTicks(7727));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 665, DateTimeKind.Local).AddTicks(7733));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreateDate",
                value: new DateTime(2024, 12, 23, 14, 55, 44, 665, DateTimeKind.Local).AddTicks(7737));
        }
    }
}
