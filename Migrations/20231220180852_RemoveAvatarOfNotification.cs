using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKConnect.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAvatarOfNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
