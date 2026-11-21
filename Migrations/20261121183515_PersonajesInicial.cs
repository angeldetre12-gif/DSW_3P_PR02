using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSW_3P_PR02.Migrations
{
    /// <inheritdoc />
    public partial class PersonajesInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[Eventos]', N'U') IS NOT NULL
    DROP TABLE [dbo].[Eventos];

IF OBJECT_ID(N'[dbo].[Personajes]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Personajes] (
        [Id] int NOT NULL IDENTITY(1, 1),
        [Nombre] nvarchar(120) NOT NULL,
        [Anime] nvarchar(120) NULL,
        [Tipo] nvarchar(50) NOT NULL,
        [Poder] int NOT NULL,
        [Habilidad] nvarchar(120) NULL,
        [Favorito] bit NOT NULL,
        CONSTRAINT [PK_Personajes] PRIMARY KEY ([Id])
    );
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personajes");
        }
    }
}
