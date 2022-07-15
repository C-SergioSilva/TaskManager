using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Service 
{
    public class TokenServices
    {
        public static string CreateToken(User user)
        {
            // manipulador onde será realizada ações pra utilizar o token criado
            var tokenHandle = new JwtSecurityTokenHandler();
            // convertendo a chave criada para bit e assim criptografando a cahve. 
            var encryptionKey = Encoding.ASCII.GetBytes(KeyJwt.KeySecurity);
            // criptografa e descriptografa a chave
            var descryptionKey = new SecurityTokenDescriptor
            {
                // reividicando a chave para obter a identidade e nome do especifico usuário
                Subject = new ClaimsIdentity(new Claim[] {

                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Nome.ToString())

                }),
                // obtendo a chave criptografada e o algoritmo que a criptografou
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.HmacSha256)

            };
            // cria literalmente o token passado por paramentro
            var token = tokenHandle.CreateToken(descryptionKey);
            //retorna o token criado.
            return tokenHandle.WriteToken(token);
        }
    }
}
