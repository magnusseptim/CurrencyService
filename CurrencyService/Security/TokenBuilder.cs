using CurrencyService.Model;
using CurrencyService.Security.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyService.Repositories
{
    public class TokenBuilder : ITokenBuilder
    {
        IConfiguration configuration;

        public TokenBuilder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public UserModel Authenticate(LoginModel login)
        {
            UserModel model = null;
            if (login.Username == configuration["AuthorizedConsumer:Login"] && login.Password == configuration["AuthorizedConsumer:Password"])
            {
                model = new UserModel()
                {
                    Name = configuration["AuthorizedConsumerData:Name"],
                    Email = configuration["AuthorizedConsumerData:Email"],
                };
            }
            return model;
        }

        public TokenModel BuildToken(UserModel user)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                                          configuration["Jwt:Issuer"],
                                                          expires: DateTime.Now.AddMinutes(5),
                                                          signingCredentials: creds);

            return new TokenModel()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationDate = DateTime.Now.AddMinutes(5)
            };
        }
    }
}
