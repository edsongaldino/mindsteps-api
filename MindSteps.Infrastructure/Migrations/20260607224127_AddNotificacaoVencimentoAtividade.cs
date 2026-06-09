using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoCE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificacaoVencimentoAtividade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificacaoVencimentoEnviada",
                table: "AtividadesPacientes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificacaoVencimentoEnviada",
                table: "AtividadesPacientes");
        }
    }
}
