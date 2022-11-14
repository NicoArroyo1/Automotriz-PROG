﻿using Libreria.Datos;
using Libreria.Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomotrizWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : Controller
    {
        private HelperDB oConexion;

        public FacturaController()
        {
            oConexion = new HelperDB();
        }

        [HttpGet("/proxima_factura")]
        public IActionResult GetProxFactura()
        {
            int prox = 0;
            try
            {
                prox = oConexion.ObtenerProxFactura();
                return Ok(prox);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno! Intente luego");
            }
        }

        [HttpGet("/autoplanes")]
        public IActionResult GetAutopartes()
        {
            List<Autoplan> lst = null;
            try
            {
                lst = oConexion.ObtenerAutoplanes();
                return Ok(lst);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno! Intente luego");
            }
        }





    }
}
