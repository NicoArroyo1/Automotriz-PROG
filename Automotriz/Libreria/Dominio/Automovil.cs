﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Dominio
{
    public class Automovil
    {
        public string nroSerie { get; set; }
        public int cod_tipo_producto { get; set; }
        public int cod_modelo { get; set; }
        public string color { get; set; }
        public int cod_tipo_vehiculo { get; set; }
        public int pre_unitario { get; set; }
        public DateTime fecha_fabricacion { get; set; }
        public int codigo { get; set; }
    }
}
