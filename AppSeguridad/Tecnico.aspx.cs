﻿using Dominio;
using Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppSeguridad
{
    public partial class Tecnico : System.Web.UI.Page
    {

        List<TrabajosPorTecnico> agendaXtecnico = new List<TrabajosPorTecnico>();
        TrabajosPorTecnicoNegocio agendaXtecnicoNegocio = new TrabajosPorTecnicoNegocio();
        int st = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string usr = Request.QueryString["usr"].ToString();
            string Cod = BusquedaCodPorUsr(usr);

            txtUsuario.Text = Cod;

            agendaXtecnico = agendaXtecnicoNegocio.Listar(Cod);

            Calendario.FirstDayOfWeek = FirstDayOfWeek.Sunday;
            Calendario.DayStyle.Height = new Unit(50);
            Calendario.DayStyle.Width = new Unit(150);

            lbl_FechaActual.Text = "FECHA ACTUAL: " + Calendario.TodaysDate.Date.ToShortDateString();

            lbl_Trabajo.ForeColor = System.Drawing.Color.Black;
            lbl_FechaInicio.ForeColor = System.Drawing.Color.Black;
            lbl_FechaFin.ForeColor = System.Drawing.Color.Black;

            foreach (TrabajosPorTecnico tpt in agendaXtecnico)
            {
                if (tpt.FechaInicio < Calendario.TodaysDate.Date && tpt.FechaFin > Calendario.TodaysDate.Date)
                {
                    lbl_Trabajo.Text = tpt.NombreTrabajo;
                    lbl_FechaInicio.Text = "FECHA INICIO: " + tpt.FechaInicio.ToShortDateString();
                    lbl_FechaFin.Text = "FECHA FIN: " + tpt.FechaFin.ToShortDateString();
                    st = tpt.IdTrabajo;
                }
                else
                {
                    lbl_Trabajo.Text = "POR EL MOMENTO NO TIENEN NINGUN TRABAJO ASIGNADO ...";
                }
            }

        }

        protected void Calendario_DayRender(object sender, DayRenderEventArgs e)
        {
            foreach (TrabajosPorTecnico aux in agendaXtecnico)
            {
                for (int i = aux.FechaInicio.Day; i <= aux.FechaFin.Day; i++)
                {
                    if (i == e.Day.Date.Day && aux.FechaInicio.Month == e.Day.Date.Month)
                    {
                        Literal literal1 = new Literal();
                        literal1.Text = "<br/>";
                        e.Cell.Controls.Add(literal1);
                        Label label1 = new Label();
                        label1.Text = aux.NombreTrabajo;
                        label1.Font.Size = new FontUnit(FontSize.Small);
                        e.Cell.Controls.Add(label1);
                        e.Cell.BackColor = System.Drawing.Color.LightGray;
                        e.Cell.ForeColor = System.Drawing.Color.Black;
                        e.Cell.BorderStyle = BorderStyle.Outset;
                        e.Cell.BorderWidth = 2;
                        e.Cell.BorderColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        public string BusquedaCodPorUsr(string usuario)
        {
            string codigo = " ";

            AdministradorNegocio administradorNegocio = new AdministradorNegocio();
            GerenteNegocio gerenteNegocio = new GerenteNegocio();
            RecepcionNegocio recepcionNegocio = new RecepcionNegocio();
            TecnicoNegocio tecnicoNegocio = new TecnicoNegocio();
            UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

            List<Dominio.Administrador> listaAdministradores = administradorNegocio.Listar();
            List<Dominio.Gerente> listaGerentes = gerenteNegocio.Listar();
            List<Dominio.Recepcion> listaRecepcionistas = recepcionNegocio.Listar();
            List<Dominio.Tecnico> listaTecnicos = tecnicoNegocio.Listar();
            List<Dominio.Usuario> listaUsuarios = usuarioNegocio.Listar();

            Usuario usrAux = listaUsuarios.Find(x => x.usuario == usuario);
            Dominio.Administrador admAux;
            Dominio.Recepcion recAux;
            Dominio.Gerente gerAux;
            Dominio.Tecnico tecAux;

            if (usrAux != null)
            {
                admAux = listaAdministradores.Find(x => x.NroDocumento == usrAux.NroDocumento);
                gerAux = listaGerentes.Find(x => x.NroDocumento == usrAux.NroDocumento);
                recAux = listaRecepcionistas.Find(x => x.NroDocumento == usrAux.NroDocumento);
                tecAux = listaTecnicos.Find(x => x.NroDocumento == usrAux.NroDocumento);

                if (admAux != null)
                {
                    return admAux.CodAdministrador;
                }
                if (gerAux != null)
                {
                    return gerAux.CodGerente;
                }
                if (recAux != null)
                {
                    return recAux.CodRecepcionista;
                }
                if (tecAux != null)
                {
                    return tecAux.CodTecnico;
                }
            }

            return codigo;
        }

        protected void Calendario_SelectionChanged(object sender, EventArgs e)
        {

        }

        protected void btn_informar_Click(object sender, EventArgs e)
        {

        }

        protected void btn_verDetalle_Click(object sender, EventArgs e)
        {
            if (st != 0)
            {
                Response.Redirect("VerDetalle.aspx?st=" + st, false);
            } else
            {

            }
        }
    }
}