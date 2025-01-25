using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Domain;
using Domain.Entities;
using Domain.QueryModels;
using Domain.Repositories;
//using Humanizer;
using Microsoft.IdentityModel.Tokens;

namespace Service
{
    public class CandidatoReadService
    {
        private readonly IColaborador _repository;
        //private readonly ICursoRepository _repositoryCurso;

        public CandidatoReadService(IColaborador repository)
        {
            _repository = repository;
            //_repositoryCurso = repositoryCurso;
           
        }

        //public Colaborador RealizarLogin(string emailOuCpf, string senha)
        //{
        //    Colaborador colaborador;
        //    var mensagem = "";
        //    var modoCpf = emailOuCpf.Replace(".", "").Replace("-", "").Length == 11 && !emailOuCpf.Contains("@");

        //    colaborador = RealizarLoginComEmail(emailOuCpf, senha);

        //    if (candidato == null)
        //    {
        //        return new QueryLogin()
        //        {
        //            Candidato = null,
        //            Mensagem = @"Prezado(a) cliente, seu cadastro não foi encontrado em nossa base de dados.
        //                        <br /> Favor verificar se o CPF ou e-mail estão digitados corretamente, se você possui cadastro ou se o seu cadastro esteja desatualizado.
        //                        <br /> Caso tenha esquecido sua senha, tente a <a ng-reflect-router-link=""/candidato/recuperarSenha"" href=""#/candidato/recuperarSenha"">recuperação de senha</a>."
        //        };
        //    }

        //    return new QueryLogin()
        //    {
        //        Candidato = candidato,
        //        Mensagem = mensagem
        //    };
        //}

        private string GerarToken(Colaborador candidato)
        {
            // Autenticação bem-sucedida, vamos gerar um token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("36B7247D-535D-4D46-8AA1-EBC5A01A696B");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                           new Claim(ClaimTypes.Role, "Candidato"),
                           new Claim("Nome", candidato.Nome),
                           new Claim("Email", candidato.Email),
                           new Claim("IdCandidato", candidato.IdColaborador.ToString()),
                }),

                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
 