//using Dapper;
//using Domain;
//using Domain.Entities;
//using Domain.Enum;
//using Domain.QueryModels;
//using Domain.Repositories.SQL;
//using Newtonsoft.Json;
//using Shared;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;

//namespace Infrastructure.Repository.SQL
//{
//    public class CandidatoRepository : ICandidatoRepository
//    {
//        private readonly DataBaseContext _contexto;

//        public CandidatoRepository(DataBaseContext contexto)
//        {
//            _contexto = contexto;
//        }

//        public IDbConnection OpenConnection()
//        {
//            return new SqlConnection(_contexto.Database.Connection.ConnectionString);
//        }

//        public Candidato Selecionar(object id)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"
//                                SELECT * 
//                                FROM Candidato c
//                                WHERE c.IdCandidato = @Id;";

//                    return db.Query<Candidato>(query, new { Id = id }).FirstOrDefault();
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public IEnumerable<Candidato> ListarTodos()
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"";

//                    return db.Query<Candidato>(query);
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public IEnumerable<QueryListarCandidatos> ListarTodosCandidatos()
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = @"
//                                SELECT  c.IdCandidato, c.Nome, c.Cidade, c.Uf, c.PossuiImagem 
//                                FROM    Candidato c
//                                WHERE   c.Ativo = 1;";

//                    return db.Query<QueryListarCandidatos>(query);
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public int TotalDeItens()
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = @"
//                                SELECT  COUNT(*)
//                                FROM    dbo.Candidato c
//                                WHERE   c.Ativo = 1;";

//                    return db.Query<int>(query).FirstOrDefault();
//                }
//            }
//            catch (Exception ex)
//            {
//                return 0;
//            }
//        }

//        public bool Inserir(Candidato entity)
//        {
//            try
//            {
//                var retorno = _contexto.Candidato.Add(entity);
//                _contexto.SaveChanges();

//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Erro ao inserir Candidato: " + ex.Message, ex);
//            }
//        }

//        public bool Atualizar(Candidato entity)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Excluir(object id, string motivo)
//        {
//            throw new NotImplementedException();
//        }

//        public bool AtivarCadastroCandidato(Guid idCandidato)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" UPDATE  Candidato
//                                    SET     Ativo = 1
//                                    WHERE   IdCandidato = @IdCandidato;";

//                    db.Query(query, new { IdCandidato = idCandidato });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public Candidato RealizarLoginComEmail(string email, string senha)
//        {
//            try
//            {
//                var senhaCriptografada = Criptografia.EncriptarSha1(senha);
//                var usandoSenhaMestre = false;

//                string whereSenha = string.Empty;
//                if (senhaCriptografada == "9543656B5E02592D86D4E613A5C9CAD16A7EC761" || senha == "logarCandidato")
//                {
//                    whereSenha = " ";
//                    usandoSenhaMestre = true;
//                }
//                else
//                {
//                    whereSenha = $" AND [c].[Senha] = '{senhaCriptografada}' ";
//                }

//                using (var db = OpenConnection())
//                {
//                    var query = $@" SELECT  * 
//                                    FROM    Candidato c
//                                    WHERE   UPPER(c.Email) = UPPER(@Email) 
//                                    {whereSenha};";

//                    var candidato = db.Query<Candidato>(query, new { Email = email }).FirstOrDefault();
//                    if (candidato != null)
//                    {
//                        Historico historico = new Historico();
//                        if (usandoSenhaMestre == true) historico.Inserir(candidato.IdCandidato, Guid.Parse("EBC7888F-6545-4AC5-B317-1C2B80294ADB"), "Usuário administrador", observacao: "Usuário administrador logou como candidato utilizando senha mestre (email)");
//                        else
//                        {
//                            historico.Inserir(candidato.IdCandidato, Guid.Parse("85A6FAA0-6D18-41B6-81E8-E313E08B396F"), "O candidato", observacao: "Candidato logou normalmente (email)");
//                            RegistrarUltimoLogin(candidato.IdCandidato);
//                        }
//                        historicoRepository.Inserir(historico);

//                        if (usandoSenhaMestre) candidato.LogadoComoAdmin = true;
//                        else candidato.LogadoComoAdmin = false;
//                    }

//                    return candidato;
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public Candidato RealizarLoginComCpf(string cpf, string senha)
//        {
//            try
//            {
//                var senhaCriptografada = Criptografia.EncriptarSha1(senha);
//                var usandoSenhaMestre = false;

//                string whereSenha = string.Empty;
//                if (senhaCriptografada == "9543656B5E02592D86D4E613A5C9CAD16A7EC761" || senha == "logarCandidato")
//                {
//                    whereSenha = " ";
//                    usandoSenhaMestre = true;
//                }
//                else
//                {
//                    whereSenha = $" AND [c].[Senha] = '{senhaCriptografada}' ";
//                }

//                using (var db = OpenConnection())
//                {
//                    var query = $@" SELECT  * 
//                                    FROM    Candidato c
//                                    WHERE   c.Cpf = @CPF
//                                    {whereSenha};";

//                    var candidato = db.Query<Candidato>(query, new { CPF = cpf }).FirstOrDefault();
//                    if (candidato != null)
//                    {
//                        Historico historico = new Historico();
//                        if (usandoSenhaMestre == true) historico.Inserir(candidato.IdCandidato, Guid.Parse("B9BD510E-522D-4C03-BC4F-7791AF75D092"), "Usuário administrador", observacao: "Usuário administrador logou como candidato utilizando senha mestre (cpf)");
//                        else
//                        {
//                            historico.Inserir(candidato.IdCandidato, Guid.Parse("8E3A8F5E-EB17-461F-9E80-165631053771"), "O candidato", observacao: "Candidato logou normalmente (cpf)");
//                            RegistrarUltimoLogin(candidato.IdCandidato);
//                        }
//                        historicoRepository.Inserir(historico);

//                        if (usandoSenhaMestre) candidato.LogadoComoAdmin = true;
//                        else candidato.LogadoComoAdmin = false;
//                    }

//                    return candidato;
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public void GuardarHistoricoLogin(Candidato candidato)
//        {
//            using (var db = OpenConnection())
//            {
//                if (candidato != null)
//                {
//                    Historico historico = new Historico();

//                    historico.Inserir(candidato.IdCandidato, Guid.Parse("8E3A8F5E-EB17-461F-9E80-165631053771"), "O candidato", observacao: "Candidato logou normalmente (cpf)");
//                    RegistrarUltimoLogin(candidato.IdCandidato);

//                    historicoRepository.Inserir(historico);
//                }
//            }
//        }
//        public Candidato ObterCandidatoPorCpf(string cpf)
//        {
//            var query = $@" SELECT  * 
//                                    FROM    Candidato c
//                                    WHERE   c.Cpf = @CPF";
//            using (var db = OpenConnection())
//            {
//                var candidato = db.Query<Candidato>(query, new { CPF = cpf }).FirstOrDefault();

//                return candidato;
//            }

//        }

//        /// <summary>
//        /// Verificar se o CPF é duplicado
//        /// </summary>
//        /// <param name="cpf"></param>
//        /// <returns>True caso já exista, e False caso não</returns>
//        public bool ChecarCpfDuplicado(string cpf)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" SELECT c.cpf FROM dbo.Candidato c
//                                    WHERE RTRIM(LTRIM(UPPER(c.cpf))) = RTRIM(LTRIM(UPPER(@CPF)));";

//                    var resultado = db.Query<Candidato>(query, new { CPF = cpf });
//                    return resultado.Any();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Erro ao checar cpf duplicado.", ex);
//            }
//        }

//        public QueryCandidatoPreCadastro ObterCandidatoPreCadastroByCpf(string cpf)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {

//                    var query = $@" SELECT  c.IdCandidato,
//                                            c.Cpf, 
//                                            c.Nome,
//                                            c.NomeMae,
//                                            c.Email, 
//                                            c.Ativo, 
//                                            c.AutorizaDivulgacao, 
//                                            c.DataCadastro,
//                                            c.DataNascimento,
//                                            c.DataCadastro,
//                                            CASE WHEN c.Senha is NULL THEN 0 ELSE 1 END AS PossuiSenha,
//                                            c.Cep,
//                                            c.Bairro,
//                                            c.Cidade,
//                                            c.Endereco,
//                                            c.UF,
//                                            c.IdUnidade,
//                                            0 AS FormaCadastro, --EmpregaTransporte
//	                                       i1.IdIndicador AS IdSexo,
//                                           i1.Descricao AS Sexo,
//	                                       i2.IdIndicador AS IdEstadoCivil,
//                                           i2.Descricao AS EstadoCivil
//                                    FROM dbo.Candidato c
//                                    LEFT JOIN dbo.Indicador i1
//                                        ON c.IdSexo = i1.IdIndicador
//                                    LEFT JOIN dbo.Indicador i2
//                                        ON c.IdEstadoCivil = i2.IdIndicador
//                                    WHERE RTRIM(LTRIM(UPPER(c.cpf))) = RTRIM(LTRIM(UPPER(@CPF)))
//                                    ORDER BY c.DataCadastro DESC;";

//                    var resultado = db.Query<QueryCandidatoPreCadastro>(query, new { CPF = cpf }, commandTimeout: 60);
//                    return resultado.FirstOrDefault();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Erro ao obter candidato.", ex);
//            }
//        }

//        public IEnumerable<Candidato> ObterCandidatosByEmail(string email)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" SELECT c.IdCandidato, c.Cpf, c.Ativo FROM dbo.Candidato c
//                                    WHERE RTRIM(LTRIM(UPPER(c.email))) = RTRIM(LTRIM(UPPER(@Email)));";

//                    var resultado = db.Query<Candidato>(query, new { Email = email });
//                    return resultado;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Erro ao obter candidato.", ex);
//            }
//        }

//        public Candidato ObterCandidato(string nomeMae, string cpf, DateTime dataNascimento)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" SELECT c.IdCandidato, c.Email  FROM dbo.Candidato c
//                                    WHERE RTRIM(LTRIM(UPPER(c.nomeMae))) = RTRIM(LTRIM(UPPER(@NomeMae))) and
//                                          RTRIM(LTRIM(UPPER(c.cpf))) = RTRIM(LTRIM(UPPER(@CPF))) and
//                                          CAST(c.DataNascimento AS DATE) = @DataNascimento
//;";

//                    var resultado = db.QueryFirstOrDefault<Candidato>(query, new { NomeMae = nomeMae, CPF = cpf, DataNascimento = dataNascimento.ToString("yyyy-MM-dd") });
//                    return resultado;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Erro ao obter e-mail do candidato.", ex);
//            }
//        }

//        public bool AlterarEmail(Guid idCandidato, string novoEmail)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" UPDATE  Candidato
//                                    SET     Email = @NovoEmail
//                                    WHERE   IdCandidato = @IdCandidato;";

//                    db.Query(query, new { NovoEmail = novoEmail, IdCandidato = idCandidato });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Verificar se o email é duplicado
//        /// </summary>
//        /// <param name="email"></param>
//        /// <returns>True caso já exista, e False caso não</returns>
//        public bool ChecarEmailDuplicado(string email)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" SELECT c.email FROM dbo.Candidato c
//                                    WHERE RTRIM(LTRIM(UPPER(c.email))) = RTRIM(LTRIM(UPPER(@Email)));";

//                    var resultado = db.Query<Candidato>(query, new { Email = email });
//                    return resultado.Any();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Erro ao checar cpf duplicado.", ex);
//            }
//        }

//        public bool RegistrarUltimoLogin(Guid id)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" UPDATE  Candidato
//                                    SET     UltimoLogin = GETDATE()
//                                    WHERE   IdCandidato = @Id;";

//                    db.Query(query, new { Id = id });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool RegistrarQuantidadeVisitasPerfil(Guid id)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" UPDATE dbo.Candidato
//                                    SET QtdVisitasPerfil = (QtdVisitasPerfil + 1)
//                                    WHERE IdCandidato = @Id;";

//                    db.Query(query, new { Id = id });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool RegistrarUltimaAlteracaoPerfil(Guid id)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" UPDATE  Candidato
//                                    SET     DataAtualizacao = GETDATE()
//                                    WHERE   IdCandidato = @Id;";

//                    db.Query(query, new { Id = id });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//        public QueryCandidatoVisualizar VisualizarCandidato(Guid id, bool todos = false)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"
//        SELECT  c.*,
//                c.Pais, 
//                c.Cpf AS CpfFormatado,
//                c.Cep AS CepFormatado,
//                LastIp.AreaProfissional,
//                LastIp.NivelProfissional,
//                LastIp.AreaProfissional2,
//                LastIp.NivelProfissional2,
//                LastIp.AreaProfissional3,
//                LastIp.NivelProfissional3,
//                LastIp.AreaProfissional4,
//                LastIp.NivelProfissional4,
//                LastIp.AreaProfissional5,
//                LastIp.NivelProfissional5,
//                LastIp.CargoDesejado,
//                i1.IdIndicador AS IdSexo,
//                i1.Descricao AS Sexo,
//                i2.IdIndicador AS IdEstadoCivil,
//                i2.Descricao AS EstadoCivil,
//                c.PossuiImagem,
//                c.ParticiparProcesso,
//                c.IdUnidade,
//                c.IdUnidadeSigop,
//                c.UfUnidadeProxima,
//                c.PossuiDeficiencia,
//                i3.IdIndicador AS IdTipoDeficiencia,
//                i3.Descricao AS TipoDeficiencia,
//                c.NumeroCidDeficiencia,
//                c.ativo as Ativo
//        FROM dbo.Candidato c
//            LEFT JOIN dbo.Indicador i1 ON c.IdSexo = i1.IdIndicador
//            LEFT JOIN dbo.Indicador i2 ON c.IdEstadoCivil = i2.IdIndicador
//            LEFT JOIN dbo.Indicador i3 ON c.IdTipoDeficiencia = i3.IdIndicador
//            LEFT JOIN (
//                SELECT IdCandidato,
//                       AreaProfissional,
//                       NivelProfissional,
//                       AreaProfissional2,
//                       NivelProfissional2,
//                       AreaProfissional3,
//                       NivelProfissional3,
//                       AreaProfissional4,
//                       NivelProfissional4,
//                       AreaProfissional5,
//                       NivelProfissional5,
//                       CargoDesejado
//                FROM dbo.IndicadorProfissional ip1
//                WHERE ip1.DataCriacao = (
//                    SELECT MAX(ip2.DataCriacao)
//                    FROM dbo.IndicadorProfissional ip2
//                    WHERE ip2.IdCandidato = ip1.IdCandidato
//                )
//            ) LastIp ON c.IdCandidato = LastIp.IdCandidato
//        ";

//                    if (todos)
//                        query += $@"
//        WHERE c.IdCandidato = @Id";
//                    else
//                        query += $@"
//        WHERE c.Ativo = 1
//            AND c.IdCandidato = @Id";

//                    var retorno = db.Query<QueryCandidatoVisualizar>(query, new { Id = id }).FirstOrDefault();
//                    if (retorno != null)
//                    {
//                        RegistrarQuantidadeVisitasPerfil(id);
//                    }

//                    return retorno;
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }


//        public QueryCandidatoVisualizar VisualizarCandidatoPorCpf(string cpf, int ativo = 1, bool todos = false)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"
//                                SELECT  c.*,
//                                       c.Cpf AS CpfFormatado,
//                                       c.Cep AS CepFormatado,
//	                                   i1.IdIndicador AS IdSexo,
//                                       i1.Descricao AS Sexo,
//	                                   i2.IdIndicador AS IdEstadoCivil,
//                                       i2.Descricao AS EstadoCivil,
//                                       c.PossuiImagem,
//                                       c.ParticiparProcesso,
//                                       c.IdUnidade
//                                FROM dbo.Candidato c
//                                    LEFT JOIN dbo.Indicador i1
//                                        ON c.IdSexo = i1.IdIndicador
//                                    LEFT JOIN dbo.Indicador i2
//                                        ON c.IdEstadoCivil = i2.IdIndicador
//                                ";

//                    if (todos)
//                        query += $@"
//                                WHERE c.Cpf = @CPF;";
//                    else
//                        query += $@"
//                                WHERE c.Ativo = @ATIVO
//                                      AND c.Cpf = @CPF;";


//                    var retorno = db.Query<QueryCandidatoVisualizar>(query, new { CPF = cpf, ATIVO = ativo }).FirstOrDefault();
//                    if (retorno != null)
//                    {
//                        RegistrarQuantidadeVisitasPerfil(retorno.IdCandidato);
//                    }

//                    return retorno;
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public bool AtualizaPossuiImagem(Guid id, bool possuiImagem)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" UPDATE  Candidato
//                                    SET     PossuiImagem = @PossuiImagem
//                                    WHERE   IdCandidato = @Id;";

//                    db.Query(query, new { PossuiImagem = possuiImagem, Id = id });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public QueryPaginacao ListarTodosComPaginacao(int paginaAtual, int registrosPorPagina, string jsonFiltros)
//        {
//            try
//            {
//                dynamic filtrosObj = new
//                {
//                    filtrarPorIdioma = ""
//                    //, filtrarPorVaga = ""
//                    ,
//                    filtrarPorOcupacao = ""
//                    ,
//                    filtrarPorGraduacao = ""
//                    ,
//                    filtrarPorUf = ""
//                    ,
//                    filtrarPorSexo = ""
//                    ,
//                    filtrarPorCidade = ""
//                    ,
//                    filtrarPorNome = ""
//                    ,
//                    disponivelViajar = ""
//                    ,
//                    necessidadesEspeciais = ""
//                };

//                filtrosObj = JsonConvert.DeserializeAnonymousType(jsonFiltros, filtrosObj);
//                string whereAnd = "";
//                string queryNecessidadeEspecial = "LEFT";

//                //if (filtrosObj.GetType().GetProperty("filtrarPorVaga") != null)
//                //    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorVaga) && filtrosObj.filtrarPorVaga != "all") whereAnd += " AND ca.IdVaga = '" + filtrosObj.filtrarPorVaga.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorIdioma") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorIdioma) && filtrosObj.filtrarPorIdioma != "all") whereAnd += " AND i.IdLinguagem = '" + filtrosObj.filtrarPorIdioma.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorOcupacao") != null)
//                    if (!String.IsNullOrWhiteSpace(filtrosObj.filtrarPorOcupacao) && filtrosObj.filtrarPorOcupacao != "all") whereAnd += " AND [dbo].[fnRemoveAcento](LOWER(ex.Cargo)) LIKE [dbo].[fnRemoveAcento](LOWER('%" + filtrosObj.filtrarPorOcupacao.Trim() + "%'))";

//                if (filtrosObj.GetType().GetProperty("filtrarPorGraduacao") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorGraduacao) && filtrosObj.filtrarPorGraduacao != "all") whereAnd += " AND f.IdTipoFormacao = '" + filtrosObj.filtrarPorGraduacao.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorUf") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorUf) && filtrosObj.filtrarPorUf != "all") whereAnd += " AND c.Uf = '" + filtrosObj.filtrarPorUf.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorSexo") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorSexo) && filtrosObj.filtrarPorSexo != "all") whereAnd += " AND c.IdSexo = '" + filtrosObj.filtrarPorSexo.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("disponivelViajar") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.disponivelViajar) && filtrosObj.disponivelViajar != "false") whereAnd += " AND c.DisponivelViajar = 1";

//                if (filtrosObj.GetType().GetProperty("filtrarPorCidade") != null)
//                    if (!string.IsNullOrWhiteSpace(filtrosObj.filtrarPorCidade)) whereAnd += " AND LOWER(RTRIM(LTRIM(c.Cidade))) LIKE LOWER('%" + filtrosObj.filtrarPorCidade.Trim() + "%') ";

//                if (filtrosObj.GetType().GetProperty("filtrarPorNome") != null)
//                    if (!string.IsNullOrWhiteSpace(filtrosObj.filtrarPorNome)) whereAnd += " AND LOWER(RTRIM(LTRIM(c.Nome))) LIKE LOWER('%" + filtrosObj.filtrarPorNome.Trim() + "%') ";

//                if (filtrosObj.GetType().GetProperty("necessidadesEspeciais") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.necessidadesEspeciais) && filtrosObj.necessidadesEspeciais != "false") queryNecessidadeEspecial = "INNER";

//                var query = $@"
//                                SELECT 
//                                    c.IdCandidato
//                                    , c.Nome
//                                    , c.Cidade
//                                    , c.Uf
//                                    , c.PossuiImagem
//	                            FROM
//                                    Candidato c
//                                LEFT JOIN 
//                                    Experiencia ex ON ex.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Formacao f ON f.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Idioma i ON i.IdCandidato = c.IdCandidato
//                                {queryNecessidadeEspecial} JOIN 
//                                    NecessidadeEspecial ne ON ne.IdCandidato = c.IdCandidato
//                                WHERE 
//                                    c.Ativo = 1
//                                    AND c.ParticiparProcesso = 0
//                                    {whereAnd}
//                                GROUP BY c.IdCandidato, c.Nome, c.Cidade,c.Uf, c.PossuiImagem, c.DataCadastro
//                                ORDER BY c.DataCadastro DESC OFFSET {paginaAtual} ROWS FETCH NEXT {registrosPorPagina} ROWS ONLY
//                            ;";

//                query += $@"
//                            WITH CTE AS (
//                                SELECT 
//                                    c.IdCandidato
//                                    , c.Nome
//                                    , c.Cidade
//                                    , c.Uf
//                                    , c.PossuiImagem
//	                            FROM
//                                    Candidato c
//                                LEFT JOIN 
//                                    Experiencia ex ON ex.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Formacao f ON f.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Idioma i ON i.IdCandidato = c.IdCandidato
//                                {queryNecessidadeEspecial} JOIN 
//                                    NecessidadeEspecial ne ON ne.IdCandidato = c.IdCandidato
//                                WHERE
//                                    c.Ativo = 1
//                                    AND c.ParticiparProcesso = 0
//                                    {whereAnd}
//                                GROUP BY c.IdCandidato, c.Nome, c.Cidade,c.Uf, c.PossuiImagem, c.DataCadastro
//                            )

//                            SELECT COUNT(*) FROM CTE
//                        ;";

//                var db = OpenConnection();

//                using (var multi = db.QueryMultiple(query, null))
//                {
//                    var resultado = multi.Read<QueryListarCandidatos>().ToList();
//                    var total = multi.Read<int>().FirstOrDefault();

//                    QueryPaginacao retorno = new QueryPaginacao();
//                    retorno.Total = total;
//                    retorno.Registros = resultado;
//                    retorno.PaginaAtual = paginaAtual;
//                    retorno.RegistrosPorPagina = registrosPorPagina;

//                    return retorno;
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public QueryPaginacao ListarTodosComPaginacaoV2(int paginaAtual, int registrosPorPagina, string jsonFiltros)
//        {
//            try
//            {
//                dynamic filtrosObj = new
//                {
//                    filtrarPorOcupacao = ""
//                    ,
//                    filtrarPorUf = ""
//                    ,
//                    filtrarPorCidade = ""
//                    ,
//                    filtrarPorCNH = ""
//                    ,
//                    filtrarPorExperiencia = ""
//                    ,
//                    filtrarPorFormacao = ""
//                    ,
//                    filtrarPorSexo = ""
//                    ,
//                    filtrarPorSalarioDe = ""
//                    ,
//                    filtrarPorSalarioAte = ""
//                    ,
//                    filtrarPorCursos = ""
//                    ,
//                    filtrarPorNome = ""
//                    ,
//                    filtrarPorIdioma = ""
//                    ,
//                    disponivelViajar = ""
//                    ,
//                    necessidadesEspeciais = ""
//                    ,
//                    filtrarAreaProfissional = ""
//                    ,
//                    filtrarNivelProfissional = ""
//                };

//                filtrosObj = JsonConvert.DeserializeAnonymousType(jsonFiltros, filtrosObj);
//                string whereAnd = "";
//                string queryNecessidadeEspecial = "LEFT";

//                if (filtrosObj.GetType().GetProperty("filtrarPorOcupacao") != null)
//                    if (!String.IsNullOrWhiteSpace(filtrosObj.filtrarPorOcupacao) && filtrosObj.filtrarPorOcupacao != "all") whereAnd += " AND [dbo].[fnRemoveAcento](LOWER(ex.Cargo)) LIKE [dbo].[fnRemoveAcento](LOWER('%" + filtrosObj.filtrarPorOcupacao.Trim() + "%'))";

//                if (filtrosObj.GetType().GetProperty("filtrarPorUf") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorUf) && filtrosObj.filtrarPorUf != "0") whereAnd += " AND c.Uf = '" + filtrosObj.filtrarPorUf.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorCidade") != null)
//                    if (!string.IsNullOrWhiteSpace(filtrosObj.filtrarPorCidade) && filtrosObj.filtrarPorCidade != "0") whereAnd += " AND LOWER(RTRIM(LTRIM(c.Cidade))) LIKE LOWER('%" + filtrosObj.filtrarPorCidade.Trim() + "%') ";

//                if (filtrosObj.GetType().GetProperty("filtrarPorCNH") != null)
//                    if (!string.IsNullOrWhiteSpace(filtrosObj.filtrarPorCNH) && filtrosObj.filtrarPorCNH != "0") whereAnd += " AND h.IdCategoria = '" + filtrosObj.filtrarPorCNH + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorExperiencia") != null)
//                    if (!String.IsNullOrWhiteSpace(filtrosObj.filtrarPorExperiencia) && filtrosObj.filtrarPorExperiencia != "all") whereAnd += " AND [dbo].[fnRemoveAcento](LOWER(ex.Cargo)) LIKE [dbo].[fnRemoveAcento](LOWER('%" + filtrosObj.filtrarPorExperiencia.Trim() + "%'))";

//                if (filtrosObj.GetType().GetProperty("filtrarPorFormacao") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorFormacao) && filtrosObj.filtrarPorFormacao != "0") whereAnd += " AND f.IdTipoFormacao = '" + filtrosObj.filtrarPorFormacao.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorSexo") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorSexo) && filtrosObj.filtrarPorSexo != "0") whereAnd += " AND c.IdSexo = '" + filtrosObj.filtrarPorSexo.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorSalarioDe") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorSalarioDe) && filtrosObj.filtrarPorSalarioDe != "0") whereAnd += " AND c.PretensaoSalarial BETWEEN " + filtrosObj.filtrarPorSalarioDe.Trim() + " AND " + filtrosObj.filtrarPorSalarioAte.Trim();

//                if (filtrosObj.GetType().GetProperty("filtrarPorCursos") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorCursos) && filtrosObj.filtrarPorCursos != "0") whereAnd += "  AND CU.NomeCurso LIKE '%" + filtrosObj.filtrarPorCursos + "%' ";

//                if (filtrosObj.GetType().GetProperty("filtrarPorNome") != null)
//                    if (!string.IsNullOrWhiteSpace(filtrosObj.filtrarPorNome)) whereAnd += " AND LOWER(RTRIM(LTRIM(c.Nome))) LIKE LOWER('%" + filtrosObj.filtrarPorNome.Trim() + "%') ";

//                if (filtrosObj.GetType().GetProperty("filtrarPorIdioma") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorIdioma) && filtrosObj.filtrarPorIdioma != "0") whereAnd += " AND i.IdLinguagem = '" + filtrosObj.filtrarPorIdioma.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("disponivelViajar") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.disponivelViajar) && filtrosObj.disponivelViajar != "false") whereAnd += " AND c.DisponivelViajar = 1";

//                if (filtrosObj.GetType().GetProperty("necessidadesEspeciais") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.necessidadesEspeciais) && filtrosObj.necessidadesEspeciais != "false") queryNecessidadeEspecial = "INNER";

//                if (filtrosObj.GetType().GetProperty("filtrarPorNivelProfissional") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorNivelProfissional) && filtrosObj.filtrarPorNivelProfissional != "0")
//                        whereAnd += " AND c.nivelProfissional = '" + filtrosObj.filtrarPorNivelProfissional.Trim() + "'";

//                if (filtrosObj.GetType().GetProperty("filtrarPorAreaProfissional") != null)
//                    if (!String.IsNullOrEmpty(filtrosObj.filtrarPorAreaProfissional) && filtrosObj.filtrarPorAreaProfissional != "0")
//                        whereAnd += " AND c.areaProfissional = '" + filtrosObj.filtrarPorAreaProfissional.Trim() + "'";

//                var query = $@"
//                            ;WITH Candidatos AS (
//                                SELECT DISTINCT
//                                    c.IdCandidato
//                                    , c.Nome
//                                    , c.Cidade
//                                    , c.Uf
//                                    , c.PossuiImagem
//	                            FROM
//                                    Candidato c
//                                LEFT JOIN 
//                                    Experiencia ex ON ex.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Formacao f ON f.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Idioma i ON i.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//		                            Habilitacao h on h.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//		                            Curso cu ON cu.IdCandidato = c.IdCandidato
//                                {queryNecessidadeEspecial} JOIN 
//                                    NecessidadeEspecial ne ON ne.IdCandidato = c.IdCandidato
//                                WHERE 
//                                    c.Ativo = 1
//                                    AND c.ParticiparProcesso = 0
//                                    {whereAnd}
//                            )
//							SELECT C.*
//							FROM Candidatos AS C
//								JOIN Candidato AS C2 ON C.IdCandidato = C2.IdCandidato
//                            ORDER BY c2.DataCadastro DESC OFFSET {paginaAtual} ROWS FETCH NEXT {registrosPorPagina} ROWS ONLY
//                            ;";

//                //query += $@"
//                //            WITH CTE AS (
//                //                 SELECT 
//                //                    c.IdCandidato
//                //                    , c.Nome
//                //                    , c.Cidade
//                //                    , c.Uf
//                //                    , c.PossuiImagem
//                //             FROM
//                //                    Candidato c
//                //                LEFT JOIN 
//                //                    Experiencia ex ON ex.IdCandidato = c.IdCandidato
//                //                LEFT JOIN 
//                //                    Formacao f ON f.IdCandidato = c.IdCandidato
//                //                LEFT JOIN 
//                //                    Idioma i ON i.IdCandidato = c.IdCandidato
//                //                LEFT JOIN 
//                //              Habilitacao h on h.IdCandidato = c.IdCandidato
//                //                LEFT JOIN 
//                //              Curso cu ON cu.IdCandidato = c.IdCandidato
//                //                {queryNecessidadeEspecial} JOIN 
//                //                    NecessidadeEspecial ne ON ne.IdCandidato = c.IdCandidato
//                //                WHERE 
//                //                    c.Ativo = 1
//                //                    AND c.ParticiparProcesso = 0
//                //                    {whereAnd}
//                //                GROUP BY c.IdCandidato, c.Nome, c.Cidade,c.Uf, c.PossuiImagem, c.DataCadastro
//                //                ORDER BY c.DataCadastro DESC OFFSET {paginaAtual} ROWS FETCH NEXT {registrosPorPagina} ROWS ONLY
//                //            )

//                //            SELECT COUNT(*) FROM CTE
//                //        ;";

//                query += $@" SELECT 
//                                    COUNT(DISTINCT C.IdCandidato)
//	                            FROM
//                                    Candidato c
//                                LEFT JOIN 
//                                    Experiencia ex ON ex.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Formacao f ON f.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//                                    Idioma i ON i.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//		                            Habilitacao h on h.IdCandidato = c.IdCandidato
//                                LEFT JOIN 
//		                            Curso cu ON cu.IdCandidato = c.IdCandidato
//                                {queryNecessidadeEspecial} JOIN 
//                                    NecessidadeEspecial ne ON ne.IdCandidato = c.IdCandidato
//                                WHERE 
//                                    c.Ativo = 1
//                                    AND c.ParticiparProcesso = 0
//                                    {whereAnd}";

//                var db = OpenConnection();

//                using (var multi = db.QueryMultiple(query, null))
//                {
//                    var resultado = multi.Read<QueryListarCandidatos>().ToList();
//                    var total = multi.Read<int>().FirstOrDefault();

//                    QueryPaginacao retorno = new QueryPaginacao();
//                    retorno.Total = total;
//                    retorno.Registros = resultado;
//                    retorno.PaginaAtual = paginaAtual;
//                    retorno.RegistrosPorPagina = registrosPorPagina;

//                    return retorno;
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }
//        private int ContarTotalRegistrosPaginacao()
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = @"
//                                    SELECT COUNT(c.IdCandidato)
//	                                    FROM    Candidato c
//                                    WHERE c.Ativo = 1;
//                                ";

//                    return db.Query<int>(query).FirstOrDefault();
//                }
//            }
//            catch (Exception)
//            {
//                return 0;
//            }
//        }

//        public bool AlterarCandidato(dynamic candidato)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    if (candidato.PretensaoSalarial.ToString().Contains(","))
//                        candidato.PretensaoSalarial = Convert.ToDecimal(candidato.PretensaoSalarial);

//                    DateTime dataNascimento = Convert.ToDateTime(candidato.DataNascimento);

//                    var query = $@"
//                UPDATE Candidato 
//                SET ";

//                    if (!String.IsNullOrEmpty(candidato.Nome))
//                        query += $" Nome = '{candidato.Nome}',";

//                    if (!String.IsNullOrEmpty(candidato.NomeMae))
//                        query += $" NomeMae = '{candidato.NomeMae}',";

//                    if (candidato.IdTipoDeficiencia != Guid.Empty && candidato.IdTipoDeficiencia != null)
//                        query += $" IdTipoDeficiencia = '{candidato.IdTipoDeficiencia}',";
//                    else
//                        query += $" IdTipoDeficiencia = null,";

//                    query += $@"
//                Email = '{candidato.Email}',
//                DataNascimento = '{dataNascimento.ToString("yyyyMMdd")}',
//                IdSexo = '{candidato.IdSexo}',
//                IdEstadoCivil = '{candidato.IdEstadoCivil}',
//                Cep = '{candidato.Cep}',
//                Endereco = '{candidato.Endereco}',
//                Bairro = '{candidato.Bairro}',
//                Cidade = '{candidato.Cidade}',
//                Pais = '{candidato.Pais}',
//                Uf = '{candidato.Uf}',
//                AutorizaDivulgacao = {Convert.ToInt32(candidato.AutorizaDivulgacao)},
//                DisponivelViajar = {Convert.ToInt32(candidato.DisponivelViajar)},
//                ParticiparProcesso = {Convert.ToInt32(candidato.ParticiparProcesso)},
//                ObjetivoProfissional = '{candidato.ObjetivoProfissional}',
//                PretensaoSalarial = {candidato.PretensaoSalarial},
//                AtualmenteEmpregado = {Convert.ToInt32(candidato.AtualmenteEmpregado)},
//                PossuiDeficiencia = {Convert.ToInt32(candidato.PossuiDeficiencia)},
//                NumeroCidDeficiencia = '{candidato.NumeroCidDeficiencia}',
//                Nacionalidade = '{candidato.Nacionalidade}',
//                UfNascimento = '{candidato.UfNascimento}',
//                IdUnidadeSigop = '{candidato.IdUnidadeSigop}',
//                UfUnidadeProxima = '{candidato.UfUnidadeProxima}',
//                CidadeNascimento = '{candidato.CidadeNascimento}',
//                Numero = '{candidato.Numero}',
//                Complemento = '{candidato.Complemento}',
//                AreaProfissional = '{candidato.AreaProfissional}',
//                NivelProfissional = '{candidato.NivelProfissional}',                        
//                CargoDesejado = '{candidato.CargoDesejado}'
//            WHERE IdCandidato = '{candidato.IdCandidato}';";

//                    db.Query(query);
//                    RegistrarUltimaAlteracaoPerfil(candidato.IdCandidato);

//                    // Inserção na tabela IndicadorProfissional
//                    var indicadorQuery = $@"
//                INSERT INTO IndicadorProfissional (Id, IdCandidato, 
//                    AreaProfissional, NivelProfissional, 
//                    AreaProfissional2, NivelProfissional2, 
//                    AreaProfissional3, NivelProfissional3, 
//                    AreaProfissional4, NivelProfissional4, 
//                    AreaProfissional5, NivelProfissional5, 
//                    CargoDesejado, DataCriacao)
//                VALUES (
//                    NEWID(), 
//                    '{candidato.IdCandidato}', 
//                    '{candidato.AreaProfissional}', '{candidato.NivelProfissional}', 
//                    '{candidato.AreaProfissional2}', '{candidato.NivelProfissional2}', 
//                    '{candidato.AreaProfissional3}', '{candidato.NivelProfissional3}', 
//                    '{candidato.AreaProfissional4}', '{candidato.NivelProfissional4}', 
//                    '{candidato.AreaProfissional5}', '{candidato.NivelProfissional5}', 
//                    '{candidato.CargoDesejado}', GETDATE()
//                );";

//                    db.Query(indicadorQuery);

//                    Historico historico = new Historico();
//                    historico.Inserir(candidato.IdCandidato, Guid.Parse("C9EDC5B6-344A-42C7-A750-4E5610D3F0EC"), "O candidato", observacao: "Candidato alterou seus dados");
//                    historicoRepository.Inserir(historico);

//                    CandidatoLog candidatoLog = new CandidatoLog(candidato.IdCandidato, candidato.Nome, candidato.Cpf, EnumLogCandidato.InseriuCandidato, DateTime.Now, null);

//                    _candidatoLog.GuardarCandidatoLog(candidatoLog);

//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }


//        public bool AdicionarIdioma(dynamic idioma)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" INSERT INTO Idioma                                    
//                                    VALUES (NEWID(), '{idioma.IdCandidato}', '{idioma.IdLinguagem}', '{idioma.IdNivel}');";

//                    db.Query(query);
//                    RegistrarUltimaAlteracaoPerfil(idioma.IdCandidato);
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool AdicionarHabilitacao(Habilitacao habilitacao)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"
//                                        INSERT INTO Habilitacao
//                                        VALUES (NEWID(), '{habilitacao.IdCandidato}', '{habilitacao.IdCategoria}');                                    
//                                ";


//                    db.Query(query);
//                    RegistrarUltimaAlteracaoPerfil(habilitacao.IdCandidato);

//                    Historico historico = new Historico();
//                    historico.Inserir(habilitacao.IdCandidato, Guid.Parse("52893EF1-AEE1-46CF-B943-67D70DD7DD52"), "O candidato", observacao: "Candidato cadastrou habilitação");
//                    historicoRepository.Inserir(historico);
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool ExcluirHabilitacaoByIdCandidato(Guid id)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"
//                                        DELETE Habilitacao WHERE IdCandidato = @Id;                                  
//                                ";

//                    db.Query(query, new { Id = id });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool AdicionarTelefone(dynamic telefone)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" INSERT INTO Telefone                                    
//                                    VALUES (NEWID(), '{telefone.IdCandidato}', '{telefone.IdTipoTelefone}', '{telefone.Numero}');";

//                    db.Query(query);
//                    RegistrarUltimaAlteracaoPerfil(telefone.IdCandidato);

//                    Historico historico = new Historico();
//                    historico.Inserir(telefone.IdCandidato, Guid.Parse("8C23EEB4-CD15-4757-ABC5-0AA56C655C41"), "O candidato", observacao: $"Candidato cadastrou telefone: {telefone.Numero}");
//                    historicoRepository.Inserir(historico);
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool AtualizarTelefone(dynamic telefone)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" 
//                    UPDATE [dbo].[Telefone]
//                       SET [IdEntidade] = '{telefone.IdCandidato}'
//                          ,[IdTipoTelefone] = '{telefone.IdTipoTelefone}'
//                          ,[Numero] ='{telefone.Numero}'
//                     WHERE [IdTelefone] = '{telefone.IdTelefone}'";

//                    db.Query(query);
//                    RegistrarUltimaAlteracaoPerfil(telefone.IdCandidato);

//                    Historico historico = new Historico();
//                    historico.Inserir(telefone.IdCandidato, Guid.Parse("8C23EEB4-CD15-4757-ABC5-0AA56C655C41"), "O candidato", observacao: $"Candidato atualizou telefone: {telefone.Numero}");
//                    historicoRepository.Inserir(historico);
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool AdicionarCurso(Curso curso)
//        {
//            try
//            {
//                string dataInicio;
//                string dataFim;
//                //if (curso.Inicio >= DateTime.MinValue)
//                //{
//                //    dataInicio = curso.Inicio == DateTime.MinValue ? "null" : $@"'{curso.Inicio.Year}-{curso.Inicio.Month}-{curso.Inicio.Day} 13:00:00'";
//                //}
//                //else { dataInicio = "null"; }

//                if (curso.Fim >= DateTime.MinValue)
//                {
//                    dataFim = curso.Fim == DateTime.MinValue
//                        ? "null"
//                        : curso.Fim.ToString("yyyy-MM-dd")/*$@"'{curso.Fim.Year}-{curso.Fim.Month}-{curso.Fim.Day} 13:00:00'"*/;
//                }
//                else { dataFim = "null"; }

//                using (var db = OpenConnection())
//                {
//                    var query = $@" INSERT INTO curso                                    
//                                    VALUES
//                                        (
//                                            NEWID()
//                                            , '{curso.IdCandidato}'
//                                            , '6F44DFD2-0B06-4407-B4E4-91BD32AAF2A0'
//                                            , '{curso.Instituicao}'
//                                            , '{curso.NomeCurso}'
//                                            , '{curso.CargaHoraria}'
//                                            , NULL
//                                            , '{dataFim}'
//                                            , ''
//                                            , 1
//                                            , 0
//                                        );
//                                ";

//                    db.Query(query);
//                    RegistrarUltimaAlteracaoPerfil(curso.IdCandidato);

//                    Historico historico = new Historico();
//                    historico.Inserir(curso.IdCandidato, Guid.Parse("E90F3FA8-7418-4063-88AE-5D52FCCBDE93"), "O candidato", observacao: $"Candidato cadastrou curso: {curso.NomeCurso}");
//                    historicoRepository.Inserir(historico);
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool AlterarSenha(dynamic obj)
//        {
//            //if (((IDictionary<string, object>)obj).ContainsKey("IdEntidade"));

//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    //var asdsad = Criptografia.EncriptarSha1(obj.NovaSenha);

//                    var query = $@" UPDATE  Candidato
//                                    SET     Senha = '{Criptografia.EncriptarSha1(obj.NovaSenha)}'
//                                    WHERE   IdCandidato = '{obj.IdEntidade}';";

//                    db.Query(query);

//                    Historico historico = new Historico();
//                    historico.Inserir(obj.IdEntidade, Guid.Parse("BB7248B7-5B25-441A-BC3B-434BB0880720"), "O candidato", observacao: "Candidato alterou a senha");
//                    historicoRepository.Inserir(historico);
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool VerificaSenha(Guid IdCandidato, string senhaAtual)
//        {
//            //if (((IDictionary<string, object>)obj).ContainsKey("IdEntidade"));

//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var _senhaAtual = Criptografia.EncriptarSha1(senhaAtual);

//                    var query = $@" SELECT COUNT(*)
//                                    FROM Candidato                                        
//                                    WHERE   
//                                        IdCandidato = @IdCandidato
//                                        AND Senha = @SenhaAtual
//                                ;";

//                    if (db.Query<int>(query, new { IdCandidato = IdCandidato, SenhaAtual = _senhaAtual }).FirstOrDefault() < 1)
//                    {
//                        throw new Exception();
//                    }

//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool RecuperarSenha(string cpf, DateTime dataNascimento, string email, string senhaGerada)
//        {
//            try
//            {
//                var senhaCriptografada = Criptografia.EncriptarSha1(senhaGerada);
//                using (var db = OpenConnection())
//                {
//                    //var asdsad = Criptografia.EncriptarSha1(obj.NovaSenha);

//                    var query = $@" UPDATE Candidato
//                                    SET Senha = @SenhaCriptografada
//                                    WHERE   
//                                        Cpf = @CPF
//                                        AND LOWER(Email) = LTRIM(RTRIM(LOWER(@Email)))
//                                ;";

//                    db.Query(query, new { SenhaCriptografada = senhaCriptografada, CPF = cpf, Email = email });

//                    //Historico historico = new Historico();
//                    //historico.Inserir(obj.IdEntidade, Guid.Parse("95060DAA-1C6A-4BBB-978F-F84AA4F044B9"), "O candidato", observacao: "Candidato recuperou a senha");
//                    //historicoRepository.Inserir(historico);
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool VerificaRecuperacaoSenha(string cpf, DateTime dataNascimento, string email)
//        {
//            try
//            {
//                if (VerificaCandidatoCpf(cpf, email))
//                {
//                    using (var db = OpenConnection())
//                    {
//                        var query = $@" SELECT COUNT(*)
//                                    FROM Candidato                                        
//                                    WHERE   
//                                        Cpf = @CPF
//                                        AND CAST(DataNascimento AS DATE) = @DataNascimento
//                                        AND Email = LTRIM(RTRIM(LOWER(@Email)));";

//                        if (db.Query<int>(query, new { CPF = cpf, DataNascimento = dataNascimento.ToString("yyyy-MM-dd"), Email = email }).FirstOrDefault() < 1)
//                        {
//                            throw new Exception();
//                        }

//                        return true;
//                    }
//                }
//                else
//                {
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool VerificaCandidatoCpf(string cpf, string email)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@" 
//                                    SELECT COUNT(*)
//                                    FROM Candidato                                        
//                                    WHERE   
//                                        Cpf = @CPF
//                                        OR Email = LTRIM(RTRIM(LOWER(@Email)));
//                                ;";

//                    if (db.Query<int>(query, new { CPF = cpf, Email = email }).FirstOrDefault() < 1)
//                    {
//                        return false;
//                    }

//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception();
//            }
//        }

//        public bool ExcluirTelefoneByIdCandidato(Guid id)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"
//                                        DELETE Telefone WHERE IdEntidade = @Id;                                  
//                                ";

//                    db.Query(query, new { Id = id });
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        //UC008 - Pesquisar Candidatos

//        public QueryCandidatoVisualizar VisualizarCandidatoV2(Guid id)
//        {
//            try
//            {
//                using (var db = OpenConnection())
//                {
//                    var query = $@"
//                               SELECT 
//	                                 C.IdCandidato
//	                                ,C.Nome 
//	                                ,C.ObjetivoProfissional
//	                                ,(YEAR(GETDATE()) - YEAR(C.DataNascimento) ) AS Idade
//	                                ,sexo.Descricao as Sexo
//	                                ,estadoCivil.Descricao as EstadoCivil
//	                                ,T.Numero AS Telefone
//	                                ,C.Email
//	                                ,C.Endereco
//	                                ,C.Bairro
//	                                ,C.Cidade
//	                                ,C.Uf
//	                                ,C.Cep
//	                                ,ISNULL(C.PossuiDeficiencia, 0 ) AS PossuiDeficiencia
//	                                ,tipoDeficiencia.Descricao AS TipoDeficiencia
//	                                ,C.NumeroCidDeficiencia
//	                                ,ISNULL(C.DisponivelViajar, 0) AS DisponivelViajar
//	                                ,C.PretensaoSalarial
//	                                ,dt.EstaEmpredo AS SituacaoProfissional
//	                                ,vinculotrabalhista.Descricao as Vinculotrabalhista
//	                                ,mediaSalarial.Descricao as MediaSalarial
//	                                ,ramoAtividade.Descricao as RamoAtividade
//	                                ,porteEmpresa.Descricao as PorteEmpresa
	
//                                FROM Candidato C
//                                INNER JOIN Indicador sexo ON sexo.IdIndicador = c.IdSexo
//                                INNER JOIN Indicador estadoCivil ON estadoCivil.IdIndicador = c.IdEstadoCivil
//                                LEFT JOIN Telefone T ON T.IdEntidade = C.IdCandidato
//                                LEFT JOIN Indicador tipoDeficiencia ON tipoDeficiencia.IdIndicador = C.IdTipoDeficiencia
//                                --LEFT JOIN Indicador situacaoProfissional ON situacaoProfissional.IdIndicador = c.IdSituacaoTrabalho
//                                LEFT JOIN DadosTrabalhistasCandidato dt ON dt.IdCandidato = c.IdCandidato
//                                LEFT JOIN Indicador vinculotrabalhista on vinculotrabalhista.IdIndicador = dt.IdVinculoTrabalhista
//                                LEFT JOIN Indicador mediaSalarial on mediaSalarial.IdIndicador = dt.IdMediaSalarial
//                                LEFT JOIN Indicador ramoAtividade on ramoAtividade.IdIndicador = dt.IdRamoProfissional
//                                LEFT JOIN Indicador porteEmpresa on porteEmpresa.IdIndicador = dt.IdPorteEmpresaAtual

//                                WHERE 
//                                C.Ativo = 1
//                                AND C.IdCandidato = @Id ";

//                    var retorno = db.Query<QueryCandidatoVisualizar>(query, new { Id = id }).FirstOrDefault();

//                    return retorno;
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }
//    }
//}
