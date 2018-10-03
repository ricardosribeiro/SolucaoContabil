using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoContabil.Domain.Entities
{
    public class Lancamento
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Competencia { get; set; }
        public decimal ValorDebito { get; set; }
        public decimal ValorCredito { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

    }
}
