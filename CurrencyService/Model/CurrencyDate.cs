using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    public class CurrencyDate
    {
        public long ID { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Currency> Currencies { get; set; }
    }
}
