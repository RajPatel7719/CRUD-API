using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_And_Authorization_Using_JWT_Token.Migrations
{
    public partial class SaveImageDataInBinaryForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "AspNetUsers");
        }
    }
}
