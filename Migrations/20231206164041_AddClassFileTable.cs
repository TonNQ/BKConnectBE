using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddClassFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Faculties_FacultyId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Rooms_RoomId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_RootMessageId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_User1Id",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_User2Id",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInvitations_Rooms_RoomId",
                table: "RoomInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInvitations_Users_ReceiverId",
                table: "RoomInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInvitations_Users_SenderId",
                table: "RoomInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Classes_ClassId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersOfRoom_Rooms_RoomId",
                table: "UsersOfRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersOfRoom_Users_UserId",
                table: "UsersOfRoom");

            migrationBuilder.CreateTable(
                name: "ClassFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoomId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassFiles_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassFiles_RoomId",
                table: "ClassFiles",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassFiles_UserId",
                table: "ClassFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Faculties_FacultyId",
                table: "Classes",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Rooms_RoomId",
                table: "Files",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_RootMessageId",
                table: "Messages",
                column: "RootMessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_User1Id",
                table: "Relationships",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_User2Id",
                table: "Relationships",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInvitations_Rooms_RoomId",
                table: "RoomInvitations",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInvitations_Users_ReceiverId",
                table: "RoomInvitations",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInvitations_Users_SenderId",
                table: "RoomInvitations",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Classes_ClassId",
                table: "Users",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersOfRoom_Rooms_RoomId",
                table: "UsersOfRoom",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersOfRoom_Users_UserId",
                table: "UsersOfRoom",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Faculties_FacultyId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Rooms_RoomId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_RootMessageId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_User1Id",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_User2Id",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInvitations_Rooms_RoomId",
                table: "RoomInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInvitations_Users_ReceiverId",
                table: "RoomInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInvitations_Users_SenderId",
                table: "RoomInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Classes_ClassId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersOfRoom_Rooms_RoomId",
                table: "UsersOfRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersOfRoom_Users_UserId",
                table: "UsersOfRoom");

            migrationBuilder.DropTable(
                name: "ClassFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Faculties_FacultyId",
                table: "Classes",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Rooms_RoomId",
                table: "Files",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_RootMessageId",
                table: "Messages",
                column: "RootMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_User1Id",
                table: "Relationships",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_User2Id",
                table: "Relationships",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInvitations_Rooms_RoomId",
                table: "RoomInvitations",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInvitations_Users_ReceiverId",
                table: "RoomInvitations",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInvitations_Users_SenderId",
                table: "RoomInvitations",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Classes_ClassId",
                table: "Users",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersOfRoom_Rooms_RoomId",
                table: "UsersOfRoom",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersOfRoom_Users_UserId",
                table: "UsersOfRoom",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
