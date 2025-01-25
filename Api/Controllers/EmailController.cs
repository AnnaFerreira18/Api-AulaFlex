using Api.Helper;
using Infrastructure.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http.Description;
using ApplicationService.Commands;

namespace Api.Controllers
{
    public class EmailController : BaseController
    {

        public EmailController(IUnitOfWork uow) : base(uow)
        {

        }
        [HttpPost]
        [Route("api/v1/email")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> Post([FromBody]  EnviarEmailCommand command)
        {
            var statusEmail = false;

            var assunto = "[SEST SENAT][Sistema Emprega Transporte] Dúvida";

            var textoEmail = command.Corpo;

            IEnumerable<string> emails = new[] { command.Email };

            if (Email.Enviar(emails, assunto, textoEmail))
                statusEmail = true;

            HttpResponseMessage response;

            if (statusEmail)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, new { mensagem = "Email enviado.", sucesso = statusEmail });
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK, new { mensagem = "Email não enviado.", sucesso = statusEmail });
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}
