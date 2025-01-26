using Api.Helper;
using Domain.Entities;
using Domain.QueryModels;
using Domain.Repositories;
using Infrastructure.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Web.Http.Description;
using ApplicationService.Commands;

namespace Api.Controllers
{
    public class ColaboradoresController : BaseController
    {
        private readonly IColaborador _repository;
        public ColaboradoresController(IUnitOfWork uow, IColaborador repository, IConfiguration configuration) : base(uow)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IConfiguration _configuration;


        [HttpGet]
        [Route("checarEmailDuplicado/{email}")]
        [ResponseType(typeof(bool))]
        public bool ChecarEmailDuplicado(string email)
        {
            bool result = _repository.ChecarEmailDuplicado(email);
            return result;
        }

        [HttpGet]
        [Route("candidato")]
        [ResponseType(typeof(IEnumerable<Colaborador>))]
        public IEnumerable<QueryListarColaboradores> Listar()
        {
            return _repository.ListarTodosColaboradores();
        }

        [HttpPost]
        [Route("api/v2/colaborador")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> PostColaborador([FromBody] ColaboradorCommand command)
        {
            var statusEmail = false;

            try
            {
                // Validação de entrada
                if (command == null || string.IsNullOrWhiteSpace(command.Email))
                {
                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "E-mail é obrigatório." }));
                }

                // Mapeamento de ColaboradorCommand para Colaborador
                var colaborador = new Colaborador
                {
                    IdColaborador = command.IdColaborador, 
                    Nome = command.Nome,
                    Email = command.Email,
                    Senha = Criptografia.EncriptarSha1(command.Senha)
                };

                // Inserção do colaborador
                var colaboradorCriado = _repository.Inserir(colaborador);

                if (colaboradorCriado)
                {
                    var chave = Criptografia.EncriptarSha1(command.Email.Trim());
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


        [HttpPost]
        [Route("api/v2/colaborador/reenviarEmail")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> ReenviarEmailColaborador([FromBody] ColaboradorCommand command)
        {
            try
            {
                // Validar entrada
                if (string.IsNullOrWhiteSpace(command.Email))
                {
                    return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "E-mail é obrigatório." }));
                }

                var chave = Criptografia.EncriptarSha1($"{command.Email.Trim()}");
                var assunto = "[SEST SENAT][Sistema Emprega Transporte] Confirmação de cadastro";

                // Acessando a configuração do BaseUrlApp usando _configuration
                var urlApp = _configuration["BaseUrlApp"];
                var link = $"{urlApp}/colaborador/confirmar-cadastro/{command.Email}/{chave}";
                var textoEmail = $@"<p>Confirme seu cadastro no Emprega Transporte através do link abaixo:</p>
                            <p><a href=""{link}"">{link}</a></p>";

                // Criando lista de e-mails com o e-mail do comando
                IEnumerable<string> emails = new[] { command.Email };

                // Enviar e-mail
                bool statusEmail = Email.Enviar(emails, assunto, textoEmail);

                // Retornar resposta
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "E-mail enviado com sucesso.", statusEmail }));
            }
            catch (Exception ex)
            {
                // Tratar erro e retornar resposta adequada
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Erro interno no servidor.", details = ex.Message }));
            }
        }



        [HttpPut]
        [Route("api/v2/colaborador/alterarSenha")]
        [ResponseType(typeof(Task<HttpResponseMessage>))]
        public Task<HttpResponseMessage> AlterarSenhaColaborador([FromBody] LoginOuSenhaCommand command)
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

        [HttpPost]
        [Route("api/v1/candidato/login")]
        [ResponseType(typeof(Colaborador))]
        public Colaborador PostLogin([FromBody] LoginOuSenhaCommand command)
        {
            return _repository.RealizarLoginComEmail(command.Email, command.Senha);
        }


        [HttpGet]
        [Route("api/v1/candidato/totalItens")]
        [ResponseType(typeof(int))]
        public int GetTotalItens()
        {
            return _repository.TotalDeItens();
        }

        [HttpDelete]
        [Route("ExcluirColaborador")]
        [ResponseType(typeof(HttpResponseMessage))]
        public Task<HttpResponseMessage> ExcluirAula(Guid idColaborador)
        {
            if (_repository.Excluir(idColaborador))
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new { message = "Aula excluída com sucesso." }));
            }

            return Task.FromResult(Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Erro ao excluir aula." }));
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
