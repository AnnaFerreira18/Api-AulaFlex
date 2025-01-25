using Azure.Core;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Transaction;
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
        [Route("ListarAula")]
        [ResponseType(typeof(IEnumerable<Aula>))]
        public IEnumerable<Aula> ListarAulas()
        {
            return _repository.ListarTodasAulas();
        }


        //[HttpGet]
        //[Route("{aulaId}")]
        //[ResponseType(typeof(dynamic))]
        //public dynamic ObterAulaPorId(int aulaId)
        //{
        //    return _repository.ObterAulaPorId(aulaId);
        //}

        [HttpPost]
        [Route("criarAula")]
        public HttpResponseMessage CriarAula([FromBody] Aula aula)
        {
            //if (aula == null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Os dados da aula não podem ser nulos." });
            //}

            try
            {
                var sucesso = _repository.Inserir(aula);

                if (sucesso)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, new { message = "Aula criada com sucesso." });
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao criar aula." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", detalhe = ex.Message });
            }
        }

        [HttpPut]
        [Route("AtualizarAula")]
        public HttpResponseMessage AtualizarAula(Guid aulaId, [FromBody] Aula aula)
        {
            if (aula == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Os dados da aula não podem ser nulos." });
            }

            try
            {
                aula.IdAula = aulaId;
                var sucesso = _repository.Atualizar(aula);

                if (sucesso)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "Aula atualizada com sucesso." });
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao atualizar aula." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", detalhe = ex.Message });
            }
        }



        [HttpDelete]
        [Route("ExcluirAula")]
        [ResponseType(typeof(HttpResponseMessage))]
        public Task<HttpResponseMessage> ExcluirAula(Guid aulaId)
        {
            if (_repository.Excluir(aulaId))
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "Aula excluída com sucesso." }));
            }

            return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao excluir aula." }));
        }

    }
}
