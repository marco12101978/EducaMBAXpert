using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducaMBAXpert.Alunos.Data.Migrations
{
    /// <inheritdoc />
    public partial class DB_Initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataDeConclusao",
                table: "Matriculas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentualConclusao",
                table: "Matriculas",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataDeConclusao",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "PercentualConclusao",
                table: "Matriculas");
        }
    }
}
