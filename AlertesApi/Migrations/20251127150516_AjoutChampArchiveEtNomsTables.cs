using Microsoft.EntityFrameworkCore.Migrations;
using Pomelo.EntityFrameworkCore.MySql.Extensions;

#nullable disable

namespace AlertesApi.Migrations
{
    /// <inheritdoc />
    public partial class AjoutChampArchiveEtNomsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstArchivee",
                table: "Alertes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstArchivee",
                table: "Alertes");
        }
    }
}
