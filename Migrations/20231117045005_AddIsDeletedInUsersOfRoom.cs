﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedInUsersOfRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UsersOfRoom",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UsersOfRoom");
        }
    }
}
