using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Libreria.Dominio
{
    public class DetalleFactura
    {
        
        //aca del codigo no toque nada pero imagino que deberia de agregar un Cod_producto
        //public Automovil? Auto { get; set; }
        //public Autoparte? AutoP { get; set; }
        public Producto producto { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }

        public DetalleFactura()
        {

        }
        //public DetalleFactura(Automovil a, int cant)
        //{
        //    Auto = a;
        //    Cantidad = cant;
        //    AutoP = null;
        //}

        //public DetalleFactura(Autoparte ap, int cant)
        //{
        //    AutoP = ap;
        //    Cantidad = cant;
        //    Auto = null;
        //}
        public DetalleFactura(Producto p, int cant) 
        {
            producto = p;
            Cantidad= cant;
        }

        //public double CalcularSubTotal()
        //{
        //    if (AutoP is null)
        //    {
        //        return Auto.Precio * Cantidad;
        //    }
        //    else
        //    {
        //        return AutoP.Precio * Cantidad;
        //    }
        //}

        public double CalcularSubTotal() 
        {
            return producto.Precio * Cantidad;
        }
    }
}
