﻿using System.Collections.Generic;
using Data_Management;
using Dominio;
using System;
using System.Net;
using System.Data;

namespace Negocio
{
    public class RecepcionNegocio
    {
        AccesoDatos datos = new AccesoDatos();
        public Recepcion GetCliente(string DNI)
        {
            Recepcion cliente = null;
            try
            {
                datos.setearConsulta("SELECT * FROM SOLICITUDES_TRABAJO WHERE Dni = @Dni");
                datos.setearParametro("@Dni", DNI);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    cliente = new Recepcion();


                    cliente.Documento = DNI;
                    cliente.Nombre = (string)datos.Lector["Nombre"];
                    cliente.Telefono = (int)datos.Lector["Telefono"];
                    cliente.Direccion = (string)datos.Lector["Direccion"];
                    cliente.Localidad = (string)datos.Lector["Localidad"];
                    cliente.Provincia = (string)datos.Lector["Provincia"];

                }


            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return cliente;
        }


        public List<TipoTrabajo> ListarTipos()
        {
            List<TipoTrabajo> lista = new List<TipoTrabajo>();
            try
            {
                datos.setearConsulta("SELECT Nombre FROM TIPOS_TRABAJO");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new TipoTrabajo
                    {
                        Nombre = (string)datos.Lector["Nombre"]
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public List<Recepcion> Listar()
        {
            List<Recepcion> lista = new List<Recepcion>();
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("select rec.CodRecepcionista, rec.NivelRol, rec.Celular, rec.Nombre, rec.Apellido, rec.NroDocumento from RECEPCIONISTAS as rec");
                datos.ejecutarLectura(); ;

                while (datos.Lector.Read())
                {
                    Recepcion aux = new Recepcion();
                    aux.CodRecepcionista = (string)datos.Lector["CodRecepcionista"];
                    aux.NivelRol = (int)datos.Lector["NivelRol"];
                    aux.Celular = (int)datos.Lector["Celular"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.NroDocumento = (int)datos.Lector["NroDocumento"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //public List<Recepcion> Listar()
        //{
        //    List<Recepcion> lista = new List<Recepcion>();

        //    try
        //    {

        //        datos.setearConsulta("select Cli.Documento , Cli.Nombre,Cli.Telefono  Cli.Direccion, Cli.Localidad, Cli.Provincia from CLIENTES as Cli");
        //        datos.ejecutarLectura(); ;

        //        while (datos.Lector.Read())
        //        {
        //            Recepcion aux = new Recepcion();
        //            aux.Documento = (string)datos.Lector["dni"];
        //            aux.Telefono = (int)datos.Lector["Telefono"];
        //            aux.Nombre = (string)datos.Lector["Nombre"];
        //            aux.Direccion = (string)datos.Lector["Direccion"];
        //            aux.Localidad = (string)datos.Lector["Localidad"];


        //            lista.Add(aux);
        //        }

        //        return lista;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        datos.cerrarConexion();
        //    }
        //}
        public int Agregar(Recepcion cli)
        {


            try
            {
                datos.limpiarParametros();
                datos.setearConsulta("SELECT COUNT(1) FROM SOLICITUDES_TRABAJO WHERE Dni = @Dni");
                datos.setearParametro("@Dni", cli.Documento);
                datos.ejecutarLectura();
                if (datos.Lector.Read() && Convert.ToInt32(datos.Lector[0]) > 0)
                {
                    // El DNI ya existe
                    return -1;
                }
                datos.cerrarConexion();
                datos.limpiarParametros();

                datos.setearConsulta("INSERT INTO SOLICITUDES_TRABAJO (Dni, Nombre, Apellido, Descripcion, Telefono, Direccion, Localidad, Provincia, TipoTrabajo, Estado) VALUES (@Dni, @Nombre, @Apellido, @Descripcion, @Telefono, @Direccion, @Localidad, @Provincia, @TipoTrabajo, @Estado)");
                datos.setearParametro("@Dni", cli.Documento);
                datos.setearParametro("@Nombre", cli.Nombre);
                datos.setearParametro("@Apellido", cli.Apellido);
                datos.setearParametro("@Descripcion", cli.Descripcion);
                datos.setearParametro("@Telefono", cli.Telefono);
                datos.setearParametro("@Direccion", cli.Direccion);
                datos.setearParametro("@Localidad", cli.Localidad);
                datos.setearParametro("@Provincia", cli.Provincia);
                datos.setearParametro("@TipoTrabajo", cli.TipoTrabajo);
                datos.setearParametro("@Estado", 1);
                datos.ejecutarAccion();
                return 1;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Recepcion cli)
        {

            try
            {
                datos.setearConsulta("update CLIENTES set Documento= @Dni , Nombre = @Nombre, Telefono = @Telefono, Direccion = @Direccion, Localidad = @Localidad, Provincia = @Provincia where Documento = @Dni ");
                datos.setearParametro("@Dni", cli.Documento);
                datos.setearParametro("@Nombre", cli.Nombre);
                datos.setearParametro("@Telefono", cli.Telefono);
                datos.setearParametro("@Direccion", cli.Direccion);
                datos.setearParametro("@Localidad", cli.Localidad);
                datos.setearParametro("@Provincia", cli.Provincia);
                datos.ejecutarLectura();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
         public DataTable ObtenerHistorialTrabajos()
        {
            DataTable dt = new DataTable();

            try
            {
                datos.setearConsulta("SELECT Dni,TipoTrabajo, Nombre,Apellido,Descripcion, Telefono, Direccion, Localidad, Provincia,Estado,TecnicoAsignado FROM SOLICITUDES_TRABAJO");

                datos.ejecutarLectura();
                dt.Load(datos.Lector);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public DataTable ObtenerHistorialCliente(string dni)
        {
            DataTable dt = new DataTable();

            try
            {
                datos.setearConsulta("SELECT id, TipoTrabajo, Descripcion, Estado, TecnicoAsignado " +
                                    "FROM SOLICITUDES_TRABAJO WHERE Dni = @Dni ORDER BY id DESC");
                datos.setearParametro("@Dni", dni);
                datos.ejecutarLectura();
                dt.Load(datos.Lector);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
    


