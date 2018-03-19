using CurrencyService.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Repositories.Interfaces
{
    public interface ICurrencyRepository 
    {
        IEnumerable<Currency> GetAll();
        Currency GetByID(long id);
        IEnumerable<Currency> GetByDate(DateTime dateTime);
        IEnumerable<Currency> GetByName(string name);
    }
}
