using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSW_3P_PR02.Migrations
{
    /// <inheritdoc />
    public partial class CharacterHubSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Anime",
                table: "Personajes",
                newName: "Videojuego");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Personajes",
                newName: "Clase");

            migrationBuilder.AlterColumn<string>(
                name: "Clase",
                table: "Personajes",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "Personajes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Personajes",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "https://placehold.co/600x800.png");

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                table: "Personajes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "Favorito",
                table: "Personajes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Personajes");

            migrationBuilder.RenameColumn(
                name: "Videojuego",
                table: "Personajes",
                newName: "Anime");

            migrationBuilder.RenameColumn(
                name: "Clase",
                table: "Personajes",
                newName: "Tipo");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Personajes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<bool>(
                name: "Favorito",
                table: "Personajes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
