using DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.TokenManager
{
    public static class GenerateTokenManager
    {
        static public string key = "ijahfggoh yeah jnhgjuhnjuahgjhakndjnnnndadadidoudiididialkkkyeahyeahyeah";

        public static string GenerateToken(User user)
        {
            //Generate signature key of my token 
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //Algo
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            //Create payload (data in token)
            Claim[] myclaims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email.ToString())
            };
            JwtSecurityToken token = new JwtSecurityToken(
                claims: myclaims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1)
                );
            //A Microsoft.IdentityModel.Tokens.SecurityTokenHandler designed for creating and
            //     validating Json Web Tokens. See: https://datatracker.ietf.org/doc/html/rfc7519
            //     and http://www.rfc-editor.org/info/rfc7515
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token); //return token in string 
        }
    }
}
