using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryId",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryId", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "UserId",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserId", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BookId",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookId", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_BookId_CategoryId_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryId",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestId",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateRequested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LibrarianId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestId", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_RequestId_UserId_UserId",
                        column: x => x.UserId,
                        principalTable: "UserId",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestDetailsId",
                columns: table => new
                {
                    RequestDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDetailsId", x => x.RequestDetailsId);
                    table.ForeignKey(
                        name: "FK_RequestDetailsId_RequestId_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestId",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_BookBookBorrowingRequestDetails_BookId_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "BookId",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBookBorrowingRequestDetails_RequestDetailsId_BookBorrowingRequestDetailsRequestDetailsId",
                        column: x => x.BookBorrowingRequestDetailsRequestDetailsId,
                        principalTable: "RequestDetailsId",
                        principalColumn: "RequestDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserId",
                columns: new[] { "UserId", "Dob", "Email", "FirstName", "LastName", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2002, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "thienvu2911@gmail.com", "La ", "Vu", "29112002", "SuperUser", "vu2911" },
                    { 2, new DateTime(2002, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "tuanduc@gmail.com", "Do ", "Duc", "06052002", "NormalUser", "duc0605" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBookBorrowingRequestDetails_BooksBookId",
                table: "BookBookBorrowingRequestDetails",
                column: "BooksBookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookId_CategoryId",
                table: "BookId",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetailsId_RequestId",
                table: "RequestDetailsId",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestId_UserId",
                table: "RequestId",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBookBorrowingRequestDetails");

            migrationBuilder.DropTable(
                name: "BookId");

            migrationBuilder.DropTable(
                name: "RequestDetailsId");

            migrationBuilder.DropTable(
                name: "CategoryId");

            migrationBuilder.DropTable(
                name: "RequestId");

            migrationBuilder.DropTable(
                name: "UserId");
        }
    }
}
