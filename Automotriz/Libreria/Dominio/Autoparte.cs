using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Dominio
{
    public class Autoparte
    {
        public int NroSerie { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public DateTime fecha_fabricacion { get; set; }
    }
}
