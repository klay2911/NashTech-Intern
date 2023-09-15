using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequest_UserId_UserId",
                table: "BookBorrowingRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserId",
                table: "UserId");

            migrationBuilder.RenameTable(
                name: "UserId",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequest_User_UserId",
                table: "BookBorrowingRequest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequest_User_UserId",
                table: "BookBorrowingRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserId",
                table: "UserId",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequest_UserId_UserId",
                table: "BookBorrowingRequest",
                column: "UserId",
                principalTable: "UserId",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
