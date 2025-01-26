using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http.Description;
using ApplicationService.Commands;
using System.Security.Claims;

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
        public IEnumerable<dynamic> ListarHorariosPorAula(Guid aulaId)
        {
            return _repository.ListarHorariosPorAula(aulaId);
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
        public HttpResponseMessage CriarHorario([FromBody] HorarioCommand horario)
        {
            if (horario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Os dados do horário não podem ser nulos." });
            }

            try
            {
                // Mapeamento de HorarioCommand para Horario
                var horarioEntity = new Horario
                {
                    IdHorario = horario.IdHorario, // Gera um novo Guid caso seja necessário
                    DiaSemana = horario.DiaSemana,
                    Hora = horario.Hora,
                    VagasDisponiveis = horario.VagasDisponiveis,
                    IdAula = horario.IdAula
                };

                var sucesso = _repository.Inserir(horarioEntity);

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
        public HttpResponseMessage AtualizarHorario(Guid horarioId, [FromBody] HorarioCommand horario)
        {
            if (horario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Os dados do horário não podem ser nulos." });
            }

            try
            {
               
                // Mapeamento de HorarioCommand para Horario (entidade do domínio)
                var horarioEntity = new Horario
                {
                    IdHorario = horarioId, // Usa o ID fornecido na rota
                    DiaSemana = horario.DiaSemana,
                    Hora = horario.Hora,
                    VagasDisponiveis = horario.VagasDisponiveis,
                    IdAula = horario.IdAula
                };

                // Atualiza o horário no repositório
                var sucesso = _repository.Atualizar(horarioEntity);

                if (sucesso)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "Horário atualizado com sucesso." });
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao atualizar o horário. Verifique os dados fornecidos." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", detalhe = ex.Message });
            }
        }




        [HttpDelete]
        [Route("{horarioId}")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> ExcluirHorario(Guid horarioId)
        {
            if (_repository.Excluir(horarioId))
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "Horário excluído com sucesso." }));
            }

            return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao excluir horário." }));
        }
    }
}
