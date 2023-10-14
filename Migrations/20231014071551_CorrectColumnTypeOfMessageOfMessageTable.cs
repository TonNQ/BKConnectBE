using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKConnect.Migrations
{
    /// <inheritdoc />
    public partial class CorrectColumnTypeOfMessageOfMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeOfMessag",
                table: "Messages",
                newName: "TypeOfMessage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeOfMessage",
                table: "Messages",
                newName: "TypeOfMessag");
        }
    }
}
