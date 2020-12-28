using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace project.api.Migrations
{
    public partial class ChangeImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Un_Image_Path",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Images",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Images",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Images",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Images",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Images",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "Un_Image_BoekId_Name_Extension",
                table: "Images",
                columns: new[] { "ProductId", "Name", "Extension" },
                unique: true,
                filter: "[ProductId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Un_Image_BoekId_Name_Extension",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Images",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "Un_Image_Path",
                table: "Images",
                column: "Path",
                unique: true,
                filter: "[Path] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");
        }
    }
}
