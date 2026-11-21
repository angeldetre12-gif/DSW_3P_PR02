using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSW_3P_PR02.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPersonajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Anime = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Poder = table.Column<int>(type: "int", nullable: false),
                    Habilidad = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Favorito = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personajes");
        }
    }
}
