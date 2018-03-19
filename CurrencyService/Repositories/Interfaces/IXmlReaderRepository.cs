using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Repositories.Interfaces
{
    public interface IXmlReaderRepository
    {
        IEnumerable<Currency> ReadCurrencyFromUrl(string url);
    }
}
