using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Dominio
{
    public class DetalleFactura
    {
        public Automovil? Auto { get; set; }
        public Autoparte? AutoP { get; set; }
        public int Cantidad { get; set; }
        
        public DetalleFactura(Automovil a, int cant)
        {
            Auto = a;
            Cantidad = cant;
        }

        public DetalleFactura(Autoparte ap, int cant)
        {
            AutoP = ap;
            Cantidad = cant;
        }

        public double CalcularSubTotal()
        {
            if (AutoP is null)
            {
                return Auto.Precio * Cantidad;
            }
            else
            {
                return AutoP.Precio * Cantidad;
            }
        }
    }
}
