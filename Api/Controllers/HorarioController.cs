using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http.Description;

namespace Api.Controllers
{
    public class HorarioController : BaseController
    {
        public HorarioController(IUnitOfWork uow, IHorario repository) : base(uow) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IHorario _repository;

        [HttpGet]
        [Route("aula/")]
        [ResponseType(typeof(IEnumerable<dynamic>))]
        public IEnumerable<dynamic> ListarHorariosPorAula(Guid horarioId)
        {
            return _repository.ListarHorariosPorAula(horarioId);
        }

        //[HttpGet]
        //[Route("{horarioId}")]
        //[ResponseType(typeof(dynamic))]
        //public dynamic ObterHorarioPorId(int horarioId)
        //{
        //    return _repository.ListarHorariosPorAula(horarioId);
        //}

        [HttpPost]
        [Route("criarHorario")]
        public HttpResponseMessage CriarHorario([FromBody] Horario horario)
        {
            if (horario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Os dados do horário não podem ser nulos." });
            }

            try
            {
                var sucesso = _repository.Inserir(horario);

                if (sucesso)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, new { message = "Horário criado com sucesso." });
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao criar o horário." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", detalhe = ex.Message });
            }
        }



        [HttpPut]
        [Route("AtualizarHorario")]
        public HttpResponseMessage AtualizarHorario(Guid horarioId, [FromBody] Horario horario)
        {
            if (horario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Os dados do horário não podem ser nulos." });
            }

            try
            {
                horario.IdHorario = horarioId; 
                var sucesso = _repository.Atualizar(horario);

                if (sucesso)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "Horário atualizado com sucesso." });
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao atualizar horário. Verifique os dados fornecidos." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", detalhe = ex.Message });
            }
        }


        [HttpDelete]
        [Route("{horarioId}")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> ExcluirHorario(int horarioId)
        {
            if (_repository.Excluir(horarioId))
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "Horário excluído com sucesso." }));
            }

            return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao excluir horário." }));
        }
    }
}
