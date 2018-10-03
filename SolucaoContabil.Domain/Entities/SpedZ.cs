using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoContabil.Domain.Entities
{
    public class SpedZ
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string CodigoImovelProprietario { get; set; }
        public string TipoLancamento { get; set; }
        public string CodigoTaxa { get; set; }
        public string DescricaoTaxa { get; set; }
        public string ComplementoTaxa { get; set; }
        public string Competencia { get; set; }
        public decimal Valor { get; set; }

    }
}
