using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        [Column(TypeName = "nvarchar(128)")]
        public string Level { get; set; }
        [Column(TypeName = "datetimeoffset(7)")]
        public DateTimeOffset TimeStamp { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Exception { get; set; }
        [Column(TypeName = "xml")]
        public string Properties { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string LogEvent { get; set; }
    }
}
