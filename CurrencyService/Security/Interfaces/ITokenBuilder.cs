using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Security.Interfaces
{
    public interface ITokenBuilder
    {
        TokenModel BuildToken(UserModel user);
        UserModel Authenticate(LoginModel login);
    }
}
