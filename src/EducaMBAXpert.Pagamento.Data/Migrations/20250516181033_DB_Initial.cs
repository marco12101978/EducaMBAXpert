﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducaMBAXpert.Pagamentos.Data.Migrations
{
    /// <inheritdoc />
    public partial class DB_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CobrancaCursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlunoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    NomeCartao = table.Column<string>(type: "varchar(250)", nullable: false),
                    NumeroCartao = table.Column<string>(type: "varchar(16)", nullable: false),
                    ExpiracaoCartao = table.Column<string>(type: "varchar(10)", nullable: false),
                    CvvCartao = table.Column<string>(type: "varchar(4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CobrancaCursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PagamentoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    StatusTransacao = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacoes_Pagamentos_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_PagamentoId",
                table: "Transacoes",
                column: "PagamentoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
