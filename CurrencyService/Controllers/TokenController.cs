using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyService.Model;
using CurrencyService.Security.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CurrencyService.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration config;
        private ITokenBuilder tokenBuilder;

        public TokenController(IConfiguration config, ITokenBuilder tokenBuilder)
        {
            this.config = config;
            this.tokenBuilder = tokenBuilder;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody] LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = tokenBuilder.Authenticate(login);

            if (user != null)
            {
                var token = tokenBuilder.BuildToken(user);
                response = Ok(new { token = token.Token, expirationDate = token.ExpirationDate.ToString("dd-MM-yy H:mm:ss") });
            }

            Serilog.Log.Information(string.Format("Token requested, user requested: {0}, date of request: {1}",login.Username, DateTime.Now));
            return response;
        }


    }
}