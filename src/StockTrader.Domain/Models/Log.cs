using System;
using System.ComponentModel.DataAnnotations;

namespace StockTrader.Domain.Models
{
    /// <summary>
    /// Log table built off log4net
    /// </summary>
    public partial class Log : DatabaseObject
    {
        public DateTime Date { get; set; }

        [StringLength(255)]
        public string Thread { get; set; }

        [StringLength(50)]
        public string Level { get; set; }

        [StringLength(255)]
        public string Logger { get; set; }

        [StringLength(4000)]
        public string Message { get; set; }

        [StringLength(2000)]
        public string Exception { get; set; }
    }
}
