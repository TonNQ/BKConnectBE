using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusInFriendRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FriendRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FriendRequests");
        }
    }
}
