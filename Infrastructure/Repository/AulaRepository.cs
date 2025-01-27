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
    public class AulaRepository : IAula
    {
        private readonly AppDbContext _contexto;
        private readonly string _connectionString;

        public AulaRepository(AppDbContext contexto, IConfiguration configuration)
        {
            _contexto = contexto;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection OpenConnection()
        {
            return new SqlConnection(_connectionString);
        }


        public bool Atualizar(Aula entity)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                UPDATE Aula
                                SET Nome = @Nome, Descricao = @Descricao, Categoria = @Categoria
                                WHERE IdAula = @IdAula;
                            ";

                    db.Execute(query, new
                    {
                        IdAula = entity.IdAula,
                        Nome = entity.Nome,
                        Descricao = entity.Descricao
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a aula.", ex);
            }
        }

        public bool Excluir(object id)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                DELETE FROM Aula
                                WHERE IdAula = @IdAula;
                            ";

                    db.Execute(query, new { IdAula = id });
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir a aula.", ex);
            }
        }

        public bool Inserir(Aula entity)
        {
            try
            {
                var retorno = _contexto.Aula.Add(entity);
                _contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir Idioma: " + ex.Message, ex);
            }
        }

        public IEnumerable<Aula> ListarTodos()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                SELECT IdAula, Nome, Descricao
                                FROM Aula;
                            ";

                    return db.Query<Aula>(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar todas as aulas.", ex);
            }
        }


        public Aula Selecionar(object id)
        {
            throw new NotImplementedException();
        }

        public int TotalDeItens()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Aula> ListarTodasAulas()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                    SELECT IdAula, Nome, Categoria, Descricao 
                    FROM Aula;
                ";

                    return db.Query<Aula>(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar aulas.", ex);
            }
        }

        public bool AssociarHorarioAula(Guid idHorario, Guid idAula)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                UPDATE Horario
                                SET IdAula = @IdAula
                                WHERE IdHorario = @IdHorario;
                            ";

                    db.Execute(query, new
                    {
                        IdHorario = idHorario,
                        IdAula = idAula
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao associar o horário à aula.", ex);
            }
        }

    }
}
