using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoCE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoVerificacao",
                table: "Usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoVerificacaoExpiracao",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrialValidoAte",
                table: "Psicologos",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoVerificacao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoVerificacaoExpiracao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TrialValidoAte",
                table: "Psicologos");
        }
    }
}
