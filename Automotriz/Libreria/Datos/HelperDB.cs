﻿using Libreria.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria.Datos
{
    public class HelperDB
    {
        private SqlConnection cnn;

        public HelperDB()
        {
            cnn = new SqlConnection(Properties.Resources.CnnString1);
        }

        public int Login(string usario, string pass)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("pa_login", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@usuario", usario);
                cmd.Parameters.AddWithValue("@pass", pass);

                SqlDataReader dr = cmd.ExecuteReader();
                
                if (dr.Read())
                {
                    return dr.GetInt32(0);
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                cnn.Close();
            }
            return -1;
        }

        public DataTable EjecutarSP(string nom_sp)
        {
            DataTable dt = new DataTable();

            cnn.Open();
            SqlCommand cmd = new SqlCommand(nom_sp, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            dt.Load(cmd.ExecuteReader());
            cnn.Close();

            return dt;
        }

        public List<Modelo> ObtenerModelos()
        {
            List<Modelo> lst = new List<Modelo>();

            DataTable t = EjecutarSP("pa_modelos");

            foreach (DataRow dr in t.Rows)
            {
                int codigo = int.Parse(dr["cod_modelo"].ToString());
                string nombre = dr["modelo"].ToString();

                Modelo aux = new Modelo(codigo, nombre);
                lst.Add(aux);
            }

            return lst;
        }

        public List<TipoVehiculo> ObtenerTiposVehiculos()
        {
            List<TipoVehiculo> lst = new List<TipoVehiculo>();

            DataTable t = EjecutarSP("pa_tipos_vehiculos");

            foreach (DataRow dr in t.Rows)
            {
                int codigo = int.Parse(dr["cod_tipo_vehiculo"].ToString());
                string tipo = dr["descripcion"].ToString();

                TipoVehiculo aux = new TipoVehiculo(codigo, tipo);
                lst.Add(aux);
            }

            return lst;
        }

        public List<Autoparte> ObtenerAutopartes()
        {
            List<Autoparte> lst = new List<Autoparte>();

            DataTable t = EjecutarSP("pa_autopartes");

            foreach (DataRow dr in t.Rows)
            {
                int codigo = int.Parse(dr["cod_tipo_producto"].ToString());
                string tipo = dr["descripcion"].ToString();

                //Autoparte aux = new Autoparte(codigo, tipo);
                //lst.Add(aux);
            }

            return lst;
        }
    }
}
