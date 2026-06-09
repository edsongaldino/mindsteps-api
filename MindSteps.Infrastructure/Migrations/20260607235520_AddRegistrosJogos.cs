using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoCE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrosJogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrosJogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    JogoId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataPlay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DadosPlay = table.Column<string>(type: "text", nullable: false),
                    AtividadePacienteId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosJogos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosJogos_AtividadesPacientes_AtividadePacienteId",
                        column: x => x.AtividadePacienteId,
                        principalTable: "AtividadesPacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RegistrosJogos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosJogos_AtividadePacienteId",
                table: "RegistrosJogos",
                column: "AtividadePacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosJogos_PacienteId",
                table: "RegistrosJogos",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosJogos");
        }
    }
}
