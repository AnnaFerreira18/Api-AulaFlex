using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aula",
                columns: table => new
                {
                    IdAula = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aula", x => x.IdAula);
                });

            migrationBuilder.CreateTable(
                name: "Colaborador",
                columns: table => new
                {
                    IdColaborador = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colaborador", x => x.IdColaborador);
                });

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    IdHorario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiaSemana = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Hora = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VagasDisponiveis = table.Column<int>(type: "int", nullable: false),
                    IdAula = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horario", x => x.IdHorario);
                    table.ForeignKey(
                        name: "FK_Horario_Aula_IdAula",
                        column: x => x.IdAula,
                        principalTable: "Aula",
                        principalColumn: "IdAula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscricao",
                columns: table => new
                {
                    IdIncricao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAula = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHorario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdColaborador = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AulaIdAula = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ColaboradorIdColaborador = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HorarioIdHorario = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscricao", x => x.IdIncricao);
                    table.ForeignKey(
                        name: "FK_Inscricao_Aula_AulaIdAula",
                        column: x => x.AulaIdAula,
                        principalTable: "Aula",
                        principalColumn: "IdAula");
                    table.ForeignKey(
                        name: "FK_Inscricao_Aula_IdAula",
                        column: x => x.IdAula,
                        principalTable: "Aula",
                        principalColumn: "IdAula",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscricao_Colaborador_ColaboradorIdColaborador",
                        column: x => x.ColaboradorIdColaborador,
                        principalTable: "Colaborador",
                        principalColumn: "IdColaborador");
                    table.ForeignKey(
                        name: "FK_Inscricao_Colaborador_IdColaborador",
                        column: x => x.IdColaborador,
                        principalTable: "Colaborador",
                        principalColumn: "IdColaborador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscricao_Horario_HorarioIdHorario",
                        column: x => x.HorarioIdHorario,
                        principalTable: "Horario",
                        principalColumn: "IdHorario");
                    table.ForeignKey(
                        name: "FK_Inscricao_Horario_IdHorario",
                        column: x => x.IdHorario,
                        principalTable: "Horario",
                        principalColumn: "IdHorario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Horario_IdAula",
                table: "Horario",
                column: "IdAula");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricao_AulaIdAula",
                table: "Inscricao",
                column: "AulaIdAula");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricao_ColaboradorIdColaborador",
                table: "Inscricao",
                column: "ColaboradorIdColaborador");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricao_HorarioIdHorario",
                table: "Inscricao",
                column: "HorarioIdHorario");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricao_IdAula",
                table: "Inscricao",
                column: "IdAula");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricao_IdColaborador",
                table: "Inscricao",
                column: "IdColaborador");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricao_IdHorario",
                table: "Inscricao",
                column: "IdHorario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscricao");

            migrationBuilder.DropTable(
                name: "Colaborador");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropTable(
                name: "Aula");
        }
    }
}
