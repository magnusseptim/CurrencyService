using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.GlobalHelpers.Interfaces
{
    public interface ISqlHelper
    {
        bool AddLog(Log log);
        bool AddLogs(IEnumerable<Log> logs);
    }
}
