using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace project.api.Migrations
{
    public partial class JWT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByIp = table.Column<string>(nullable: true),
                    Revoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(nullable: true),
                    ReplacedByToken = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Un_Product_Name_Description",
                table: "Products",
                columns: new[] { "Name", "Description" },
                unique: true,
                filter: "[Description] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Un_OrderProduct_ProductId_OrderId",
                table: "OrderProducts",
                columns: new[] { "ProductId", "OrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Un_Image_Path",
                table: "Images",
                column: "Path",
                unique: true,
                filter: "[Path] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Un_Company_Name_Email_AccountNumber",
                table: "Companies",
                columns: new[] { "Name", "Email", "AccountNumber" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Email] IS NOT NULL AND [AccountNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Un_Category_Name_ParentId",
                table: "Categories",
                columns: new[] { "Name", "ParentId" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [ParentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "Un_Product_Name_Description",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "Un_OrderProduct_ProductId_OrderId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "Un_Image_Path",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "Un_Company_Name_Email_AccountNumber",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "Un_Category_Name_ParentId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");
        }
    }
}
