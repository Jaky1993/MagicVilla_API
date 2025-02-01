using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addUsersToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUser", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 31, 18, 10, 22, 581, DateTimeKind.Local).AddTicks(8341));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 31, 18, 10, 22, 581, DateTimeKind.Local).AddTicks(8737));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 31, 18, 10, 22, 581, DateTimeKind.Local).AddTicks(8748));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 31, 18, 10, 22, 577, DateTimeKind.Local).AddTicks(3780), "https://dotnetmastery.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 31, 18, 10, 22, 580, DateTimeKind.Local).AddTicks(8589), "https://dotnetmastery.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 31, 18, 10, 22, 580, DateTimeKind.Local).AddTicks(8627), "https://dotnetmastery.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 31, 18, 10, 22, 580, DateTimeKind.Local).AddTicks(8633), "https://dotnetmastery/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 31, 18, 10, 22, 580, DateTimeKind.Local).AddTicks(8637), "https://dotnetmastery/bluevillaimages/villa6.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUser");

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 19, 20, 35, 556, DateTimeKind.Local).AddTicks(7673));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 19, 20, 35, 556, DateTimeKind.Local).AddTicks(7950));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 17, 19, 20, 35, 556, DateTimeKind.Local).AddTicks(7957));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 17, 19, 20, 35, 553, DateTimeKind.Local).AddTicks(7801), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 17, 19, 20, 35, 556, DateTimeKind.Local).AddTicks(761), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 17, 19, 20, 35, 556, DateTimeKind.Local).AddTicks(786), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 17, 19, 20, 35, 556, DateTimeKind.Local).AddTicks(790), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 1, 17, 19, 20, 35, 556, DateTimeKind.Local).AddTicks(793), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg" });
        }
    }
}
