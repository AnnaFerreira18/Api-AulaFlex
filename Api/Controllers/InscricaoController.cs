using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http.Description;
using Domain.Repositories;
using Infrastructure.Transaction;
using Domain.Entities;
using ApplicationService.Commands;

namespace Api.Controllers
{
    public class InscricaoController : BaseController
    {
        public InscricaoController(IUnitOfWork uow, IInscricao repository) : base(uow)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IInscricao _repository;
           // private readonly IInscricaoService _service;

            [HttpGet]
            [Route("inscricoes/{idColaborador}")]
            [ResponseType(typeof(IEnumerable<dynamic>))]
            public IEnumerable<dynamic> ListarInscricoesPorColaborador(Guid idColaborador)
            {
                return _repository.ListarInscricoesPorColaborador(idColaborador);
            }

            [HttpGet]
            [Route("categorias")]
            [ResponseType(typeof(IEnumerable<dynamic>))]
            public IEnumerable<dynamic> TotalInscricoesPorCategoria()
            {
                return _repository.TotalInscricoesPorCategoria();
            }

            [HttpGet]
            [Route("verificarDisponibilidade/{aulaId}/{horarioId}")]
            [ResponseType(typeof(bool))]
            public bool VerificarDisponibilidade(Guid aulaId, Guid horarioId)
            {
                return _repository.VerificarDisponibilidadeHorario(horarioId);
            }

            [HttpPost]
            [Route("inscrever")]
            public HttpResponseMessage InscreverColaborador([FromBody] InscricaoCommand command)
            {
                if (command == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Os dados da inscrição não podem ser nulos." });
                }

                try
                {
                var sucesso = _repository.InscreverColaborador(command.IdColaborador, command.IdAula, command.IdHorario);


                if (sucesso)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { message = "Inscrição realizada com sucesso." });
                    }

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Não foi possível realizar a inscrição." });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", detalhe = ex.Message });
                }
            }


            [HttpDelete]
            [Route("cancelar/{inscricaoId}")]
            [ResponseType(typeof(Task<HttpResponseMessage>))]
            public Task<HttpResponseMessage> CancelarInscricao(Guid inscricaoId)
            {
                if (_repository.CancelarInscricao(inscricaoId))
                {
                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "Inscrição cancelada com sucesso." }));
                }

                return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao cancelar a inscrição." }));
            }

            [HttpGet]
            [Route("verificar/{colaboradorId}/{aulaId}/{horarioId}")]
            [ResponseType(typeof(bool))]
            public bool VerificarInscricaoExistente(Guid colaboradorId, Guid aulaId, Guid horarioId)
            {
                return _repository.VerificarInscricaoExistente(colaboradorId, aulaId, horarioId);
            }
    }


}

