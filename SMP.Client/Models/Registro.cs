using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMP.Client.Models
{
    public class Registro
    {
        public long Id { get; set; }
        public string Ip{ get; set; }
        public string Processo { get; set; }
        public DateTime Data { get; set; }

    }
}
