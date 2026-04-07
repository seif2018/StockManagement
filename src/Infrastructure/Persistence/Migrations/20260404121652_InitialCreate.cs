using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrixHT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrixTTC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantiteStock = table.Column<int>(type: "int", nullable: false),
                    DernierInventaire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeArticle = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    DLC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeVente = table.Column<int>(type: "int", nullable: true),
                    Packaging = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceArticle = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    AncienneQuantite = table.Column<int>(type: "int", nullable: false),
                    NouvelleQuantite = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventaires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mouvements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceArticle = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mouvements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mouvements_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Reference",
                table: "Articles",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_ArticleId",
                table: "Mouvements",
                column: "ArticleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventaires");

            migrationBuilder.DropTable(
                name: "Mouvements");

            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
