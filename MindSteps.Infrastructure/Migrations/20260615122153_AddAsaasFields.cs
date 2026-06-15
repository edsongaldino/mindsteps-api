using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoCE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAsaasFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AsaasCustomerId",
                table: "Psicologos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AsaasSubscriptionId",
                table: "Psicologos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documento",
                table: "Psicologos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Pago",
                table: "Psicologos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Plano",
                table: "Psicologos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsaasCustomerId",
                table: "Psicologos");

            migrationBuilder.DropColumn(
                name: "AsaasSubscriptionId",
                table: "Psicologos");

            migrationBuilder.DropColumn(
                name: "Documento",
                table: "Psicologos");

            migrationBuilder.DropColumn(
                name: "Pago",
                table: "Psicologos");

            migrationBuilder.DropColumn(
                name: "Plano",
                table: "Psicologos");
        }
    }
}
