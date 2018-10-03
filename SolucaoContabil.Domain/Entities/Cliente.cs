using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoContabil.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public int Cod { get; set; }
        public string Descricao { get; set; }

        public int  ClienteTipoId { get; set; }
        public virtual ClienteTipo ClienteTipo { get; set; }
    }
}
