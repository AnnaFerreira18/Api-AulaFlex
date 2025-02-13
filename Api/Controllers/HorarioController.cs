using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http.Description;
using ApplicationService.Commands;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    public class HorarioController : BaseController
    {
        private readonly IHorario _repository;
        public HorarioController(IUnitOfWork uow, IHorario repository) : base(uow) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Authorize]
        [HttpGet]
        [Route("listarHorarios/")]
        [ResponseType(typeof(IEnumerable<dynamic>))]
        public IActionResult ListarHorariosPorAula(Guid aulaId)
        {
            var horarios = _repository.ListarHorariosPorAula(aulaId);

            if (horarios == null || !horarios.Any())
            {
                return new NotFoundObjectResult(new { message = "Nenhum horário encontrado para esta aula." });
            }

            return new OkObjectResult(horarios);
        }

        [Authorize]
        [HttpPost]
        [Route("criarHorario/{idAula}")]
        public IActionResult CriarHorario([FromBody] HorarioCommand horario)
        {
            if (horario == null)
            {
                return new BadRequestObjectResult(new { message = "Os dados do horário não podem ser nulos." });
            }

            try
            {
                var horarioEntity = new Horario
                {
                    IdHorario = horario.IdHorario,
                    DiaSemana = horario.DiaSemana,
                    Hora = horario.Hora,
                    VagasDisponiveis = horario.VagasDisponiveis,
                    IdAula = horario.IdAula
                };

                var sucesso = _repository.Inserir(horarioEntity);

                if (sucesso)
                {
                    return new OkObjectResult(new { message = "Horário criado com sucesso." });
                }

                return new BadRequestObjectResult(new { message = "Erro ao criar o horário." });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "Erro interno no servidor.", detalhe = ex.Message }) { StatusCode = 500 };
            }
        }

        [Authorize]
        [HttpPut]
        [Route("atualizarHorario/{idAula}")]
        public IActionResult AtualizarHorario(Guid horarioId, [FromBody] HorarioCommand horario)
        {
            if (horario == null)
            {
                return new BadRequestObjectResult(new { message = "Os dados do horário não podem ser nulos." });
            }

            try
            {
                var horarioEntity = new Horario
                {
                    IdHorario = horarioId,
                    DiaSemana = horario.DiaSemana,
                    Hora = horario.Hora,
                    VagasDisponiveis = horario.VagasDisponiveis,
                    IdAula = horario.IdAula
                };

                var sucesso = _repository.Atualizar(horarioEntity);

                if (sucesso)
                {
                    return new OkObjectResult(new { message = "Horário atualizado com sucesso." });
                }

                return new BadRequestObjectResult(new { message = "Erro ao atualizar o horário. Verifique os dados fornecidos." });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "Erro interno no servidor.", detalhe = ex.Message }) { StatusCode = 500 };
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{horarioId}")]
        public IActionResult ExcluirHorario(Guid horarioId)
        {
            if (_repository.Excluir(horarioId))
            {
                return new ObjectResult(new { message = "Horário excluído com sucesso." })
                {
                    StatusCode = 200
                };
            }

            return new ObjectResult(new { message = "Erro ao excluir horário." })
            {
                StatusCode = 400
            };
        }
    }
}
