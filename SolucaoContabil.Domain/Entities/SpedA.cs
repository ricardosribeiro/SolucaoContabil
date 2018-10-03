using System;

namespace SolucaoContabil.Domain.Entities
{
    public class SpedA
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string CodigoTaxa { get; set; }
        public string DescricaoTaxa { get; set; }
        public string Competencia { get; set; }
        public decimal Valor { get; set; }
    }
}
