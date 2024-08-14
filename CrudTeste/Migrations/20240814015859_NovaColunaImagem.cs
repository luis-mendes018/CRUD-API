using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrudTeste.Migrations
{
    public partial class NovaColunaImagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "AspNetUsers");
        }
    }
}
