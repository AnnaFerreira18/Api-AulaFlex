using Domain.Repositories;
using System.Web.Http;
using System.Web.Http.Description;
using Infrastructure.Transaction;
using Api.Helper;
using Infrastructure.Repository;
using Service;
using System.Net;
using System.Security.Claims;
using Domain.Entities;
using Domain.QueryModels;


namespace Api.Controllers
{
    public class ColaboradorController : BaseController
    {
        public ColaboradorController(IUnitOfWork uow, IColaborador repository, IConfiguration configuration) : base(uow) 
        {
            _configuration = configuration;
        }


        private readonly IColaborador _repository;
        private readonly IConfiguration _configuration;


        [HttpGet]
        [Route("api/v1/candidato/checarEmailDuplicado/{email}")]
        [ResponseType(typeof(bool))]
        public bool ChecarEmailDuplicado(string email)
        {
            bool result = _repository.ChecarEmailDuplicado(email);
            return result;
        }

        [HttpGet]
        [Route("api/v1/candidato")]
        [ResponseType(typeof(IEnumerable<QueryListarColaboradores>))]
        public IEnumerable<QueryListarColaboradores> Listar()
        {
            return _repository.ListarTodosColaboradores();
        }

        [HttpPost]
        [Route("api/v2/colaborador")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> PostColaborador([FromBody] Colaborador command)
        {
            var statusEmail = false;

            try
            {
                // Validação de entrada
                if (string.IsNullOrWhiteSpace(command.Email))
                {
                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "E-mail é obrigatório." }));
                }

                var colaboradorCriado = _repository.Inserir(command); // Simule o método de adição no repositório

                if (colaboradorCriado)
                {
                    var chave = Criptografia.EncriptarSha1($"{command.Email.Trim()}");
                    var assunto = "[SEST SENAT][Sistema Emprega Transporte] Confirmação de cadastro";
                    var urlApp = _configuration["BaseUrlApp"];
                    var link = $"{urlApp}/colaborador/confirmar-cadastro/{command.Email}/{chave}";
                    var textoEmail = $@"<p>Confirme seu cadastro no Emprega Transporte através do link abaixo:</p>
                                        <p><a href=""{link}"">{link}</a></p>";

                    IEnumerable<string> emails = new[] { command.Email };

                    // Enviar e-mail
                    if (Email.Enviar(emails, assunto, textoEmail))
                        statusEmail = true;

                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.Created, new { message = "Colaborador cadastrado com sucesso.", statusEmail }));
                }

                return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao cadastrar colaborador." }));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", details = ex.Message }));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/v2/colaborador/reenviarEmail")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> ReenviarEmailColaborador([FromBody] QueryListarColaboradores command)
        {
            var statusEmail = false;

            try
            {
                // Validar entrada
                if (string.IsNullOrWhiteSpace(command.Email))
                {
                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "E-mail são obrigatórios." }));
                }

                var chave = Criptografia.EncriptarSha1($"{command.Email.Trim()}");
                var assunto = "[SEST SENAT][Sistema Emprega Transporte] Confirmação de cadastro";

                // Acessando a configuração do BaseUrlApp usando _configuration
                var urlApp = _configuration["BaseUrlApp"];
                var link = $"{urlApp}/colaborador/confirmar-cadastro/{command.Email}/{chave}";
                var textoEmail = $@"<p>Confirme seu cadastro no Emprega Transporte através do link abaixo:</p>
                                <p><a href=""{link}"">{link}</a></p>";

                IEnumerable<string> emails = new[] { command.Email };

                // Enviar e-mail
                if (Email.Enviar(emails, assunto, textoEmail))
                    statusEmail = true;

                return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "E-mail enviado com sucesso.", statusEmail }));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", details = ex.Message }));
            }
        }
   

        [HttpPut]
        [Route("api/v2/colaborador/alterarSenha")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> AlterarSenhaColaborador([FromBody] Colaborador command)
        {
            try
            {
                // Validação de entrada
                if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Senha))
                {
                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "E-mail e Nova Senha são obrigatórios." }));
                }

                // Atualizar senha no banco de dados (simule a lógica no repositório)
                var senhaAtualizada = _repository.AlterarSenha(command.Email, command.Senha);

                if (senhaAtualizada)
                {
                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "Senha alterada com sucesso." }));
                }

                return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao alterar a senha. Verifique os dados enviados." }));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", details = ex.Message }));
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/candidato/login")]
        [ResponseType(typeof(Colaborador))]
        public Colaborador PostLogin([FromBody] Colaborador command)
        {
            return _repository.RealizarLoginComEmail(command.Email, command.Senha);
        }

        //[HttpPut]
        //[Route("api/v2/candidato/alterarSenha")]
        //[ResponseType(typeof(Task<HttpResponseMessage>))]
        //public Task<HttpResponseMessage> AlterarSenhaV2([FromBody] CandidatoAlterarSenhaCommand command)
        //{
        //    var service = new CandidatoAlterarSenhaService(command, _repository);
        //    service.Run();

        //    return ReturnResponse(service, new { message = "Senha alterada com sucesso." }, null);
        //}


        [HttpGet]
        [Route("api/v1/candidato/totalItens")]
        [ResponseType(typeof(int))]
        public int GetTotalItens()
        {
            return _repository.TotalDeItens();
        }

        private string GetUserCPFFromClaims()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userCpfClaim = identity.Claims.FirstOrDefault(c => c.Type == "Cpf");

            if (userCpfClaim != null && !string.IsNullOrWhiteSpace(userCpfClaim.Value))
            {
                return userCpfClaim.Value;
            }

            // Se a claim não puder ser encontrada ou o valor for inválido
            throw new InvalidOperationException("CPF do usuário não encontrado ou inválido nas claims.");
        }

    }
}
