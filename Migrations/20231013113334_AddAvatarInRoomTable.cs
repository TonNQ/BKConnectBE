using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarInRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_SchoolYears_SchoolYearId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersOfRoom_Messages_ReadMessageId",
                table: "UsersOfRoom");

            migrationBuilder.AlterColumn<long>(
                name: "ReadMessageId",
                table: "UsersOfRoom",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "SchoolYearId",
                table: "Rooms",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_SchoolYears_SchoolYearId",
                table: "Rooms",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersOfRoom_Messages_ReadMessageId",
                table: "UsersOfRoom",
                column: "ReadMessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_SchoolYears_SchoolYearId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersOfRoom_Messages_ReadMessageId",
                table: "UsersOfRoom");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Rooms");

            migrationBuilder.AlterColumn<long>(
                name: "ReadMessageId",
                table: "UsersOfRoom",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SchoolYearId",
                table: "Rooms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_SchoolYears_SchoolYearId",
                table: "Rooms",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersOfRoom_Messages_ReadMessageId",
                table: "UsersOfRoom",
                column: "ReadMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
