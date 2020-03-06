using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog_Rest_Api.Migrations
{
    public partial class storyAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(100)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(70)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    StoryId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true),
                    Body = table.Column<string>(nullable: true),
                    PublishedDate = table.Column<DateTime>(nullable: false),
                    AuthorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.StoryId);
                    table.ForeignKey(
                        name: "FK_Stories_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stories_AuthorId",
                table: "Stories",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
