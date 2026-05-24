using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoCE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAtividadeNovosCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AtividadeObrigatoria",
                table: "Atividades",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CategoriaEmocional",
                table: "Atividades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiasSemana",
                table: "Atividades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeedbackAutomatico",
                table: "Atividades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frequencia",
                table: "Atividades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HorarioSugerido",
                table: "Atividades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LembreteSuave",
                table: "Atividades",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NivelSugerido",
                table: "Atividades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificarEmail",
                table: "Atividades",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificarPush",
                table: "Atividades",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermitirAnexos",
                table: "Atividades",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrazoConclusao",
                table: "Atividades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoResposta",
                table: "Atividades",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtividadeObrigatoria",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "CategoriaEmocional",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "DiasSemana",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "FeedbackAutomatico",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "Frequencia",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "HorarioSugerido",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "LembreteSuave",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "NivelSugerido",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "NotificarEmail",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "NotificarPush",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "PermitirAnexos",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "PrazoConclusao",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "TipoResposta",
                table: "Atividades");
        }
    }
}
