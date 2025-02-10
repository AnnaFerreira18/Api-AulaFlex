using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Acesso
{
    public class TokenService
    {
        public static string GenerateJwtToken(Colaborador colaborador)
        {
            var key = Encoding.ASCII.GetBytes("36B7247D-535D-4D46-8AA1-EBC5A01A696B"); // Troque pela sua chave secreta
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, colaborador.IdColaborador.ToString()),
                new Claim(ClaimTypes.Email, colaborador.Email),
                new Claim(ClaimTypes.Name, colaborador.Nome)
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = "sua-api", 
                SigningCredentials = credentials
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); // Retorna o token como string
        }


    }
}
