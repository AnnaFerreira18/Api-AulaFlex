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
    public class InscricaoRepository : IInscricao
    {
        private readonly string _connectionString;
        private readonly AppDbContext _contexto;

        public InscricaoRepository(AppDbContext contexto, IConfiguration configuration)
        {
            _contexto = contexto;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection OpenConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public bool Atualizar(Inscricao entity)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(object id)
        {
            throw new NotImplementedException();
        }

        public bool Inserir(Inscricao entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inscricao> ListarTodos()
        {
            throw new NotImplementedException();
        }

        public Inscricao Selecionar(object id)
        {
            throw new NotImplementedException();
        }

        public int TotalDeItens()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> ListarInscricoesPorColaborador(Guid idColaborador)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                    SELECT I.IdInscricao AS InscricaoId, A.Nome AS Aula, H.DiaSemana, H.Hora, 
                           I.DataInicio, I.DataFim, I.Status
                    FROM Inscricao I
                    JOIN Colaborador C ON I.IdColaborador = C.IdColaborador
                    JOIN Aula A ON I.IdAula = A.IdAula
                    JOIN Horario H ON I.IdHorario = H.IdHorario
                    WHERE C.IdColaborador = @IdColaborador;
                ";

                    return db.Query(query, new { IdColaborador = idColaborador });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar inscrições do colaborador.", ex);
            }
        }

        public int ObterVagasRestantes(string nomeAula, string diaSemana, string hora)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                    SELECT H.VagasDisponiveis - 
                           (SELECT COUNT(*) 
                            FROM Inscricao I 
                            WHERE I.IdHorario = H.IdHorario AND I.Status = 'Ativa') AS VagasRestantes
                    FROM Horario H
                    JOIN Aula A ON H.IdAula = A.IdAula
                    WHERE A.Nome = @NomeAula AND H.DiaSemana = @DiaSemana AND H.Hora = @Hora;
                ";

                    return db.Query<int>(query, new { NomeAula = nomeAula, DiaSemana = diaSemana, Hora = hora }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter vagas restantes.", ex);
            }
        }

        public IEnumerable<dynamic> TotalInscricoesPorCategoria()
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = @"
                    SELECT A.Categoria, COUNT(I.IdInscricao) AS TotalInscricoes
                    FROM Inscricao I
                    JOIN Aula A ON I.IdAula = A.IdAula
                    GROUP BY A.Categoria;
                ";

                    return db.Query(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter total de inscrições por categoria.", ex);
            }
        }

        public bool InscreverColaborador(Guid idColaborador, Guid idAula, Guid idHorario)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    // Verifica se o colaborador já está inscrito
                    var queryVerificar = @"
    SELECT 1 
    FROM Inscricao i
    WHERE i.IdColaborador = @IdColaborador 
    AND i.IdAula = @IdAula 
    AND i.IdHorario = @IdHorario 
    AND i.Status = 'Ativa';";

                    var existeInscricao = db.Query<int>(queryVerificar,
                        new { IdColaborador = idColaborador, IdAula = idAula, IdHorario = idHorario }).Any();

                    if (existeInscricao)
                    {
                        return false; // Colaborador já está inscrito nessa aula e horário
                    }

                    // Verifica a disponibilidade do horário
                    var queryDisponibilidade = @"
    SELECT COUNT(*) 
    FROM Inscricao i
    WHERE i.IdHorario = @IdHorario AND i.Status = 'Ativa';";

                    var vagasOcupadas = db.Query<int>(queryDisponibilidade, new { IdHorario = idHorario }).FirstOrDefault();

                    var queryVagasDisponiveis = @"
    SELECT VagasDisponiveis
    FROM Horario h
    WHERE h.IdHorario = @IdHorario;";

                    var vagasDisponiveis = db.Query<int>(queryVagasDisponiveis, new { IdHorario = idHorario }).FirstOrDefault();

                    if (vagasOcupadas >= vagasDisponiveis)
                    {
                        return false; // Não há vagas disponíveis para o horário
                    }

                    // Se tudo estiver ok, inscreve o colaborador
                    var queryInserir = @"
    INSERT INTO Inscricao (IdColaborador, IdAula, IdHorario, DataInicio, Status)
    VALUES (@IdColaborador, @IdAula, @IdHorario, GETDATE(), 'Ativa');";

                    db.Execute(queryInserir, new { IdColaborador = idColaborador, IdAula = idAula, IdHorario = idHorario });

                    return true; // Inscrição realizada com sucesso
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inscrever colaborador na aula.", ex);
            }
        }



        // Método para cancelar a inscrição do colaborador
        public bool CancelarInscricao(Guid inscricaoId)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = $@"
                    UPDATE Inscricao
                    SET Status = 'Cancelada', DataFim = GETDATE()
                    WHERE IdInscricao = @InscricaoId AND Status = 'Ativa';";

                    var resultado = db.Execute(query, new { InscricaoId = inscricaoId });

                    return resultado > 0; // Retorna verdadeiro se a inscrição foi cancelada
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cancelar a inscrição.", ex);
            }
        }

        // Método para listar todas as inscrições de um colaborador
        //public IEnumerable<Inscricao> ListarInscricoesPorColaborador(Guid idColaborador)
        //{
        //    try
        //    {
        //        using (var db = OpenConnection())
        //        {
        //            var query = $@"
        //            SELECT i.IdInscricao, i.IdColaborador, i.IdAula, i.IdHorario, i.DataInicio, i.DataFim, i.Status
        //            FROM Inscricao i
        //            WHERE i.IdColaborador = @ColaboradorId;";

        //            return db.Query<Inscricao>(query, new { IdColaborador = idColaborador });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao listar as inscrições do colaborador.", ex);
        //    }
        //}

        // Método para verificar a disponibilidade de um horário
        public bool VerificarDisponibilidadeHorario(Guid horarioId)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = $@"
                    SELECT VagasDisponiveis - (SELECT COUNT(*) FROM Inscricao WHERE IdHorario = @HorarioId AND Status = 'Ativa') 
                    FROM Horario 
                    WHERE IdHorario = @HorarioId;";

                    var vagasDisponiveis = db.Query<int>(query, new { HorarioId = horarioId }).FirstOrDefault();

                    return vagasDisponiveis > 0; // Retorna verdadeiro se houver vagas disponíveis
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar disponibilidade do horário.", ex);
            }
        }

        // Método para verificar se uma inscrição já existe
        public bool VerificarInscricaoExistente(Guid colaboradorId, Guid aulaId, Guid horarioId)
        {
            try
            {
                using (var db = OpenConnection())
                {
                    var query = $@"
                    SELECT 1
                    FROM Inscricao i
                    WHERE i.IdColaborador = @ColaboradorId 
                    AND i.IdAula = @AulaId 
                    AND i.IdHorario = @HorarioId
                    AND i.Status = 'Ativa';";

                    return db.Query<int>(query, new { ColaboradorId = colaboradorId, AulaId = aulaId, HorarioId = horarioId }).Any();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar inscrição existente.", ex);
            }
        }

    }
}
