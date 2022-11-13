﻿using Aplicacion.Datos;
using Aplicacion.Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomotrizWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {
        private SQLControl oConexion;

        public ProductoController()
        {
            oConexion = new SQLControl();
        }

        [HttpGet("/modelos")]
        public IActionResult GetModelos()
        {
            List<Modelo> lst = null;
            try
            {
                lst = oConexion.ObtenerModelos();
                return Ok(lst);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno! Intente luego");
            }
        }

        
        [HttpGet("/tipos_vehiculos")]
        public IActionResult GetTiposVehiculos()
        {
            List<TipoVehiculo> lst = null;
            try
            {
                lst = oConexion.ObtenerTiposVehiculos();
                return Ok(lst);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno! Intente luego");
            }
        }

        [HttpGet("/autopartes")]
        public IActionResult GetAutopartes()
        {
            List<Autoparte> lst = null;
            try
            {
                lst = oConexion.ObtenerAutopartes();
                return Ok(lst);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno! Intente luego");
            }
        }
        /*
        [HttpGet("/automoviles")]
        public IActionResult GetAutomoviles()
        {
            List<Autoparte> lst = null;
            try
            {
                lst = oConexion.ObtenerAutomoviles();
                return Ok(lst);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno! Intente luego");
            }
        }
        */
        











    }
}
