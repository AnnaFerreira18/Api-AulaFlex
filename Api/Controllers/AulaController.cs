using ApplicationService.Commands;
using Azure.Core;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http.Description;

namespace Api.Controllers
{
    public class AulaController : BaseController
    {
        private readonly IAula _repository;
        public AulaController(IUnitOfWork uow, IAula repository) : base(uow) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        [HttpGet]
        [Route("listarAulas")]

        public IActionResult ListarAulas()
        {
            var aulas = _repository.ListarTodasAulas();
            if (aulas == null || !aulas.Any())
            {
                return new NotFoundObjectResult(new { message = "Nenhuma aula encontrada." });
            }
            return new OkObjectResult(aulas);
        }


        //[HttpGet]
        //[Route("{aulaId}")]
        //[ResponseType(typeof(dynamic))]
        //public dynamic ObterAulaPorId(int aulaId)
        //{
        //    return _repository.ObterAulaPorId(aulaId);
        //}

        [HttpPost]
        [Route("adicionarAula")]
        [Authorize]
        public IActionResult CriarAula([FromBody] AulaCommand aula)
        {
            if (aula == null)
            {
                return new BadRequestObjectResult(new { message = "Os dados da aula não podem ser nulos." });
            }

            try
            {
                var aulaEntity = new Aula
                {
                    IdAula = aula.IdAula,
                    Nome = aula.Nome,
                    Descricao = aula.Descricao,
                    Categoria = aula.Categoria
                };

                var sucesso = _repository.Inserir(aulaEntity);

                if (sucesso)
                {
                    return new OkObjectResult(new { message = "Aula criada com sucesso." });
                }

                return new BadRequestObjectResult(new { message = "Erro ao criar aula." });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        [HttpPut]
        [Route("atualizarAula")]
        [Authorize]
        public IActionResult AtualizarAula(Guid aulaId, [FromBody] AulaCommand aula)
        {
            if (aula == null)
            {
                return new BadRequestObjectResult(new { message = "Os dados da aula não podem ser nulos." });
            }

            try
            {
                var aulaEntity = new Aula
                {
                    IdAula = aulaId,
                    Nome = aula.Nome,
                    Descricao = aula.Descricao,
                    Categoria = aula.Categoria
                };

                var sucesso = _repository.Atualizar(aulaEntity);

                if (sucesso)
                {
                    return new OkObjectResult(new { message = "Aula atualizada com sucesso." });
                }

                return new BadRequestObjectResult(new { message = "Erro ao atualizar aula." });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500); 
            }
        }

        [HttpDelete]
        [Route("excluirAula/{idAula}")]
        [Authorize]
        public IActionResult ExcluirAula(Guid aulaId)
        {
            if (_repository.Excluir(aulaId))
            {
                return new OkObjectResult(new { message = "Aula excluída com sucesso." });
            }
            return new BadRequestObjectResult(new { message = "Erro ao excluir aula." });
        }

    }
}
