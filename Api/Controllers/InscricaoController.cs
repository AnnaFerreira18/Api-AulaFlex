using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http.Description;
using Domain.Repositories;
using Infrastructure.Transaction;
using Domain.Entities;
using ApplicationService.Commands;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    public class InscricaoController : BaseController
    {
        private readonly IInscricao _repository;
        public InscricaoController(IUnitOfWork uow, IInscricao repository) : base(uow)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Authorize]
        [HttpGet]
        [Route("inscricoes/{idColaborador}")]
        public IActionResult ListarInscricoesPorColaborador(Guid idColaborador)
        {
            var inscricoes = _repository.ListarInscricoesPorColaborador(idColaborador);

            if (inscricoes == null || !inscricoes.Any())
            {
                return new ObjectResult(new { message = "Nenhuma inscrição encontrada para o colaborador." })
                {
                    StatusCode = 404
                };
            }

            return new ObjectResult(inscricoes) { StatusCode = 200 };
        }

        [Authorize]
        [HttpGet]
        [Route("categoriasAulas")]
        public IActionResult TotalInscricoesPorCategoria()
        {
            var categorias = _repository.TotalInscricoesPorCategoria();

            if (categorias == null || !categorias.Any())
            {
                return new ObjectResult(new { message = "Nenhuma categoria de aula encontrada." })
                {
                    StatusCode = 404
                };
            }

            return new ObjectResult(categorias) { StatusCode = 200 };
        }

        [Authorize]
        [HttpGet]
        [Route("verificarDisponibilidade/{aulaId}/{horarioId}")]
        public IActionResult VerificarDisponibilidade(Guid aulaId, Guid horarioId)
        {
            var disponibilidade = _repository.VerificarDisponibilidadeHorario(horarioId);

            if (disponibilidade)
            {
                return new ObjectResult(new { message = "Horário disponível." }) { StatusCode = 200 };
            }

            return new ObjectResult(new { message = "Horário indisponível." }) { StatusCode = 400 };
        }

        [Authorize]
        [HttpPost]
        [Route("inscrever/{idColaborador}/{idAula}/{idHorario}")]
        public IActionResult InscreverColaborador([FromBody] InscricaoCommand command)
        {
            if (command == null)
            {
                return new ObjectResult(new { message = "Os dados da inscrição não podem ser nulos." })
                {
                    StatusCode = 400
                };
            }

            try
            {
                var inscricao = new Inscricao
                {
                    IdColaborador = command.IdColaborador,
                    IdAula = command.IdAula,
                    IdHorario = command.IdHorario,
                    IdInscricao = command.IdInscricao,
                    DataInicio = command.DataInicio,
                    DataFim = command.DataFim,
                    Status = command.Status
                };

                var sucesso = _repository.Inserir(inscricao);

                if (sucesso)
                {
                    return new ObjectResult(new { message = "Inscrição realizada com sucesso." }) { StatusCode = 200 };
                }

                return new ObjectResult(new { message = "Não foi possível realizar a inscrição." }) { StatusCode = 400 };
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "Erro interno no servidor.", detalhe = ex.Message }) { StatusCode = 500 };
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("cancelar/{inscricaoId}")]
        public IActionResult CancelarInscricao(Guid inscricaoId)
        {
            if (_repository.CancelarInscricao(inscricaoId))
            {
                return new ObjectResult(new { message = "Inscrição cancelada com sucesso." }) { StatusCode = 200 };
            }

            return new ObjectResult(new { message = "Erro ao cancelar a inscrição." }) { StatusCode = 400 };
        }

        [Authorize]
        [HttpGet]
        [Route("verificar/{colaboradorId}/{aulaId}/{horarioId}")]
        public IActionResult VerificarInscricaoExistente(Guid colaboradorId, Guid aulaId, Guid horarioId)
        {
            var existe = _repository.VerificarInscricaoExistente(colaboradorId, aulaId, horarioId);

            return new ObjectResult(existe) { StatusCode = 200 };
        }

    }
}

