﻿using Api.Helper;
using Domain.Entities;
using Domain.QueryModels;
using Domain.Repositories;
using Infrastructure.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using ApplicationService.Commands;
using Api.Acesso;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;

namespace Api.Controllers
{
    public class ColaboradoresController : BaseController
    {
        private readonly IColaborador _repository;
        private readonly IConfiguration _configuration;
        public ColaboradoresController(IUnitOfWork uow, IColaborador repository, IConfiguration configuration)
            : base(uow)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Route("checarEmailDuplicado/{email}")]
        public IActionResult ChecarEmailDuplicado(string email)
        {
            bool result = _repository.ChecarEmailDuplicado(email);
            return new OkObjectResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        [Route("listarColaboradores")]
        public IActionResult Listar()
        {
            var colaboradores = _repository.ListarTodosColaboradores();

            if (colaboradores == null || !colaboradores.Any())
            {
                return new NotFoundObjectResult(new { message = "Nenhum colaborador encontrado." }) { StatusCode = 404 };
            }

            return new OkObjectResult(colaboradores) { StatusCode = 200 };
        }

        [HttpPost]
        [Route("colaborador/criarConta")]
        public IActionResult PostColaborador([FromBody] ColaboradorCommand command)
        {
            try
            {
                if (command == null || string.IsNullOrWhiteSpace(command.Email))
                {
                    return new BadRequestObjectResult(new { message = "E-mail é obrigatório." }) { StatusCode = 400 };
                }

                var colaborador = new Colaborador
                {
                    IdColaborador = command.IdColaborador,
                    Nome = command.Nome,
                    Email = command.Email,
                    Senha = Criptografia.EncriptarSha1(command.Senha)
                };

                var colaboradorCriado = _repository.Inserir(colaborador);

                if (colaboradorCriado)
                {
                    var assunto = "[AulaFlex] Confirmação de cadastro";

                    var textoEmail = $@"
                    <p>Olá {command.Nome},</p>
                    <p>Seu cadastro foi realizado com sucesso no sistema AulaFlex!</p>
                    <p>Atenciosamente,</p>
                    <p>Anna.</p>";

                    IEnumerable<string> emails = new[] { command.Email };
                    bool statusEmail = Email.Enviar(emails, assunto, textoEmail);

                    return new OkObjectResult(new { message = "Colaborador cadastrado com sucesso." }) { StatusCode = 200 };
                }

                return new BadRequestObjectResult(new { message = "Erro ao cadastrar colaborador." }) { StatusCode = 400 };
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        [HttpPost]
        [Route("colaborador/enviarEmailRedefinirSenha")]
        public IActionResult EnviarEmailRedefinirSenha([FromBody] LoginOuSenhaCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.Email))
                {
                    return new BadRequestObjectResult(new { message = "E-mail é obrigatório." }) { StatusCode = 400 };
                }

                // Criptografa o e-mail para gerar um token de redefinição único
                var chave = Criptografia.EncriptarSha1($"{command.Email.Trim()}");

                // Define o assunto do e-mail
                var assunto = "[AulaFlex] Redefinição de Senha";

                // Obtém a URL da aplicação para incluir no link

                // Gera o link para a página de redefinição de senha
                var link = $"http://localhost:4200/novaSenha/{command.Email}/{chave}"; // URL fixa para o frontend

                // Texto do e-mail com o link para redefinição de senha
                var textoEmail = $@"
                <p>Olá,</p>
                <p>Recebemos uma solicitação de redefinição de senha para o seu e-mail. Se você não fez essa solicitação, pode ignorar este e-mail.</p>
                <p>Para redefinir sua senha, clique no link abaixo:</p>
                <p><a href=""{link}"">{link}</a></p>
                <p>Atenciosamente,</p>
                <p>Equipe</p>";

                // Envia o e-mail
                IEnumerable<string> emails = new[] { command.Email };
                bool statusEmail = Email.Enviar(emails, assunto, textoEmail);

                // Verifica se o e-mail foi enviado com sucesso
                if (statusEmail)
                {
                    return new OkObjectResult(new { message = "E-mail de redefinição de senha enviado com sucesso." }) { StatusCode = 200 };
                }
                else
                {
                    return new BadRequestObjectResult(new { message = "Erro ao enviar e-mail de redefinição de senha." }) { StatusCode = 500 };
                }
            }
            catch (Exception ex)
            {
                // Retorna erro interno em caso de falha inesperada
                return new StatusCodeResult(500);
            }
        }


        [HttpPut]
        [Route("colaborador/redefinirSenha/{email}/{token}")]
        public IActionResult RedefinirSenha(string email, string token, [FromBody] LoginOuSenhaCommand command)
        {
            try
            {
                // Criptografa o e-mail novamente para gerar o token correspondente
                var chaveCriptografada = Criptografia.EncriptarSha1($"{email.Trim()}");

                // Verifica se o token recebido é igual ao token criptografado
                if (token != chaveCriptografada)
                {
                    return new BadRequestObjectResult(new { message = "Token inválido ou expirado." }) { StatusCode = 400 };
                }

                // Verifica se a nova senha foi fornecida
                if (string.IsNullOrWhiteSpace(command.Senha))
                {
                    return new BadRequestObjectResult(new { message = "Nova senha é obrigatória." }) { StatusCode = 400 };
                }

                // Altera a senha do usuário no banco de dados
                var senhaAlterada = _repository.AlterarSenha(email, command.Senha);

                if (senhaAlterada)
                {
                    return new OkObjectResult(new { message = "Senha alterada com sucesso." }) { StatusCode = 200 };
                }

                return new BadRequestObjectResult(new { message = "Erro ao alterar a senha." }) { StatusCode = 400 };
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        [HttpPost]
        [Route("colaborador/login")]
        public ActionResult PostLogin([FromBody] LoginOuSenhaCommand command)
        {
            var colaborador = _repository.RealizarLoginComEmail(command.Email, command.Senha);

            if (colaborador == null)
            {
                // Retorna 401 Unauthorized com a mensagem personalizada
                return new UnauthorizedObjectResult(new { message = "Login ou senha inválidos." }) { StatusCode = 401 };
            }

            var token = TokenService.GenerateJwtToken(colaborador);

            // Retorna 200 OK com o token gerado
            return new OkObjectResult(new { token, colaborador }) { StatusCode = 200 };
        }



        [HttpGet]
        [Route("colaborador/totalItens")]
        [Authorize]
        public IActionResult GetTotalItens()
        {
            int totalItens = _repository.TotalDeItens();
            return new OkObjectResult(totalItens) { StatusCode = 200 };
        }


        [HttpDelete]
        [Route("excluirColaborador")]
        public IActionResult ExcluirColaborador(Guid idColaborador)
        {
            var excluido = _repository.Excluir(idColaborador);

            if (excluido)
            {
                return new OkObjectResult(new { message = "Colaborador excluído com sucesso." }) { StatusCode = 200 };
            }

            return new BadRequestObjectResult(new { message = "Erro ao excluir colaborador." }) { StatusCode = 400 };
        }

    }
}
