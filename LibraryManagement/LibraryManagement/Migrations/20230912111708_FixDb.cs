using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_BookId_BooksBookId",
                table: "BookBookBorrowingRequestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_RequestDetailsId_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookId_CategoryId_CategoryId",
                table: "BookId");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDetailsId_RequestId_RequestId",
                table: "RequestDetailsId");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestId_UserId_UserId",
                table: "RequestId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestId",
                table: "RequestId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestDetailsId",
                table: "RequestDetailsId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryId",
                table: "CategoryId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookId",
                table: "BookId");

            migrationBuilder.RenameTable(
                name: "RequestId",
                newName: "BookBorrowingRequest");

            migrationBuilder.RenameTable(
                name: "RequestDetailsId",
                newName: "RequestDetails");

            migrationBuilder.RenameTable(
                name: "CategoryId",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "BookId",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_RequestId_UserId",
                table: "BookBorrowingRequest",
                newName: "IX_BookBorrowingRequest_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestDetailsId_RequestId",
                table: "RequestDetails",
                newName: "IX_RequestDetails_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_BookId_CategoryId",
                table: "Book",
                newName: "IX_Book_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookBorrowingRequest",
                table: "BookBorrowingRequest",
                column: "RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestDetails",
                table: "RequestDetails",
                column: "RequestDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Category_CategoryId",
                table: "Book",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_Book_BooksBookId",
                table: "BookBookBorrowingRequestDetails",
                column: "BooksBookId",
                principalTable: "Book",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_RequestDetails_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails",
                column: "BookBorrowingRequestDetailsRequestDetailsId",
                principalTable: "RequestDetails",
                principalColumn: "RequestDetailsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequest_UserId_UserId",
                table: "BookBorrowingRequest",
                column: "UserId",
                principalTable: "UserId",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDetails_BookBorrowingRequest_RequestId",
                table: "RequestDetails",
                column: "RequestId",
                principalTable: "BookBorrowingRequest",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Category_CategoryId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_Book_BooksBookId",
                table: "BookBookBorrowingRequestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_RequestDetails_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequest_UserId_UserId",
                table: "BookBorrowingRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDetails_BookBorrowingRequest_RequestId",
                table: "RequestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestDetails",
                table: "RequestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookBorrowingRequest",
                table: "BookBorrowingRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "RequestDetails",
                newName: "RequestDetailsId");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "CategoryId");

            migrationBuilder.RenameTable(
                name: "BookBorrowingRequest",
                newName: "RequestId");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestDetails_RequestId",
                table: "RequestDetailsId",
                newName: "IX_RequestDetailsId_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_BookBorrowingRequest_UserId",
                table: "RequestId",
                newName: "IX_RequestId_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_CategoryId",
                table: "BookId",
                newName: "IX_BookId_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestDetailsId",
                table: "RequestDetailsId",
                column: "RequestDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryId",
                table: "CategoryId",
                column: "CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestId",
                table: "RequestId",
                column: "RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookId",
                table: "BookId",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_BookId_BooksBookId",
                table: "BookBookBorrowingRequestDetails",
                column: "BooksBookId",
                principalTable: "BookId",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_RequestDetailsId_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails",
                column: "BookBorrowingRequestDetailsRequestDetailsId",
                principalTable: "RequestDetailsId",
                principalColumn: "RequestDetailsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookId_CategoryId_CategoryId",
                table: "BookId",
                column: "CategoryId",
                principalTable: "CategoryId",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDetailsId_RequestId_RequestId",
                table: "RequestDetailsId",
                column: "RequestId",
                principalTable: "RequestId",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestId_UserId_UserId",
                table: "RequestId",
                column: "UserId",
                principalTable: "UserId",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
