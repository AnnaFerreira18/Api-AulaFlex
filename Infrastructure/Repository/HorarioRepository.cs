using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class HorarioRepository : IHorario
    {
        private readonly AppDbContext _contexto;
        private readonly string _connectionString;

        public HorarioRepository(AppDbContext contexto, IConfiguration configuration)
        {
            _contexto = contexto;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection OpenConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public bool Atualizar(Horario entity)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                UPDATE Horario
                                SET DiaSemana = @DiaSemana, Hora = @Hora, VagasDisponiveis = @VagasDisponiveis
                                WHERE IdHorario = @IdHorario;
                            ";

                    db.Execute(query, new
                    {
                        IdHorario = entity.IdHorario,
                        DiaSemana = entity.DiaSemana,
                        Hora = entity.Hora,
                        VagasDisponiveis = entity.VagasDisponiveis
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o horário.", ex);
            }
        }

        public bool Excluir(object id)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                DELETE FROM Horario
                                WHERE IdHorario = @IdHorario; 
                            ";

                    db.Execute(query, new { IdHorario = id });
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir o horário.", ex);
            }
        }


        public bool Inserir(Horario entity)
        {
            try
            {
                var retorno = _contexto.Horario.Add(entity);
                _contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir Idioma: " + ex.Message, ex);
            }
        }

        public IEnumerable<Horario> ListarTodos()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                SELECT IdHorario, DiaSemana, Hora, VagasDisponiveis, IdAula
                                FROM Horario;
                            ";

                    return db.Query<Horario>(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar todos os horários.", ex);
            }
        }


        public Horario Selecionar(object id)
        {
            throw new NotImplementedException();
        }

        public int TotalDeItens()
        {
            throw new NotImplementedException();
        }

       public IEnumerable<dynamic> ListarHorariosPorAula(Guid aulaId)
    {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                    SELECT H.DiaSemana, H.Hora, H.VagasDisponiveis,H.IdHorario
                    FROM Horario H
                    JOIN Aula A ON H.IdAula = A.IdAula
                    WHERE H.IdAula = @IdAula;
                    ";

                    return db.Query(query, new { IdAula = aulaId });
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Erro ao listar horários da aula.", ex);
            }
        }

        public IEnumerable<dynamic> ListarHorariosDisponiveis()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                    SELECT A.Nome AS Aula, H.DiaSemana, H.Hora, H.VagasDisponiveis
                    FROM Horario H
                    JOIN Aula A ON H.IdAula = A.IdAula
                    WHERE H.VagasDisponiveis > 0;
                ";

                    return db.Query(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar horários disponíveis.", ex);
            }
        }
    }
}
