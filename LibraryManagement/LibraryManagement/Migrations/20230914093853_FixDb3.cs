using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixDb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_RequestDetails_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDetails_BookBorrowingRequest_RequestId",
                table: "RequestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestDetails",
                table: "RequestDetails");

            migrationBuilder.RenameTable(
                name: "RequestDetails",
                newName: "BookBorrowingRequestDetails");

            migrationBuilder.RenameIndex(
                name: "IX_RequestDetails_RequestId",
                table: "BookBorrowingRequestDetails",
                newName: "IX_BookBorrowingRequestDetails_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookBorrowingRequestDetails",
                table: "BookBorrowingRequestDetails",
                column: "RequestDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_BookBorrowingRequestDetails_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails",
                column: "BookBorrowingRequestDetailsRequestDetailsId",
                principalTable: "BookBorrowingRequestDetails",
                principalColumn: "RequestDetailsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequestDetails_BookBorrowingRequest_RequestId",
                table: "BookBorrowingRequestDetails",
                column: "RequestId",
                principalTable: "BookBorrowingRequest",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_BookBorrowingRequestDetails_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequestDetails_BookBorrowingRequest_RequestId",
                table: "BookBorrowingRequestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookBorrowingRequestDetails",
                table: "BookBorrowingRequestDetails");

            migrationBuilder.RenameTable(
                name: "BookBorrowingRequestDetails",
                newName: "RequestDetails");

            migrationBuilder.RenameIndex(
                name: "IX_BookBorrowingRequestDetails_RequestId",
                table: "RequestDetails",
                newName: "IX_RequestDetails_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestDetails",
                table: "RequestDetails",
                column: "RequestDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBookBorrowingRequestDetails_RequestDetails_BookBorrowingRequestDetailsRequestDetailsId",
                table: "BookBookBorrowingRequestDetails",
                column: "BookBorrowingRequestDetailsRequestDetailsId",
                principalTable: "RequestDetails",
                principalColumn: "RequestDetailsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDetails_BookBorrowingRequest_RequestId",
                table: "RequestDetails",
                column: "RequestId",
                principalTable: "BookBorrowingRequest",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
