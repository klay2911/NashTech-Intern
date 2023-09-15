using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixDb4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookBookBorrowingRequestDetails",
                columns: table => new
                {
                    BookBorrowingRequestDetailsRequestDetailsId = table.Column<int>(type: "int", nullable: false),
                    BooksBookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBookBorrowingRequestDetails", x => new { x.BookBorrowingRequestDetailsRequestDetailsId, x.BooksBookId });
                    table.ForeignKey(
                        name: "FK_BookBookBorrowingRequestDetails_BookBorrowingRequestDetails_BookBorrowingRequestDetailsRequestDetailsId",
                        column: x => x.BookBorrowingRequestDetailsRequestDetailsId,
                        principalTable: "BookBorrowingRequestDetails",
                        principalColumn: "RequestDetailsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBookBorrowingRequestDetails_Book_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Book",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBookBorrowingRequestDetails_BooksBookId",
                table: "BookBookBorrowingRequestDetails",
                column: "BooksBookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBookBorrowingRequestDetails");
        }
    }
}
