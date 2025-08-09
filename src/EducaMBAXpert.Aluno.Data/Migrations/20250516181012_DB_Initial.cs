using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducaMBAXpert.Alunos.Data.Migrations
{
    /// <inheritdoc />
    public partial class DB_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 200, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rua = table.Column<string>(type: "varchar(100)", maxLength: 200, nullable: false),
                    Numero = table.Column<string>(type: "varchar(100)", nullable: false),
                    Complemento = table.Column<string>(type: "varchar(100)", nullable: false),
                    Bairro = table.Column<string>(type: "varchar(100)", nullable: false),
                    Cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    Cep = table.Column<string>(type: "varchar(100)", maxLength: 20, nullable: false),
                    AlunoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enderecos_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matriculas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlunoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DataMatricula = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matriculas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matriculas_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AulasConcluidas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AulaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AulasConcluidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AulasConcluidas_Matriculas_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matriculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AulasConcluidas_MatriculaId",
                table: "AulasConcluidas",
                column: "MatriculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_AlunoId",
                table: "Enderecos",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_AlunoId",
                table: "Matriculas",
                column: "AlunoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AulasConcluidas");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "Matriculas");

            migrationBuilder.DropTable(
                name: "Alunos");
        }
    }
}
