using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    public class Currency
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }

        public long CurrencyDateID { get; set; }

        public CurrencyDate CurrencyDate { get; set; } 
    }
}
