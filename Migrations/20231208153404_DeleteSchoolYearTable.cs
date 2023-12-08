using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKConnect.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSchoolYearTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_SchoolYears_SchoolYearId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "SchoolYears");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_SchoolYearId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SchoolYearId",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SchoolYearId",
                table: "Rooms",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolYears",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Schemes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolYears", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SchoolYearId",
                table: "Rooms",
                column: "SchoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_SchoolYears_SchoolYearId",
                table: "Rooms",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id");
        }
    }
}
