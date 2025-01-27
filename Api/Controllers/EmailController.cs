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
        [Route("email")]
        public IActionResult Post([FromBody] EnviarEmailCommand command)
        {
            var statusEmail = false;

            var assunto = "[SEST SENAT][Sistema Emprega Transporte] Dúvida";
            var textoEmail = command.Corpo;

            IEnumerable<string> emails = new[] { command.Email };

            if (Email.Enviar(emails, assunto, textoEmail))
            {
                statusEmail = true;
            }

            if (statusEmail)
            {
                return new OkObjectResult(new { mensagem = "Email enviado.", sucesso = statusEmail });
            }
            else
            {
                return new OkObjectResult(new { mensagem = "Email não enviado.", sucesso = statusEmail });
            }
        }

    }
}
