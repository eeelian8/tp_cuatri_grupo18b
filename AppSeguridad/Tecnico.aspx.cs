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


        protected void Page_Load(object sender, EventArgs e)
        {
            string usr = Request.QueryString["usr"].ToString();
            string Cod = BusquedaCodPorUsr(usr);


            agendaXtecnico = agendaXtecnicoNegocio.Listar(Cod);

            Calendario.FirstDayOfWeek = FirstDayOfWeek.Sunday;
            Calendario.ShowGridLines = true;
            Calendario.DayStyle.Height = new Unit(50);
            Calendario.DayStyle.Width = new Unit(150);
            Calendario.OtherMonthDayStyle.BackColor = System.Drawing.Color.AliceBlue;
        }

        protected void Calendario_DayRender(object sender, DayRenderEventArgs e)
        {
            foreach (TrabajosPorTecnico aux in agendaXtecnico)
            {
                if(aux.FechaInicio.Date == e.Day.Date)
                {
                    Literal literal1 = new Literal();
                    literal1.Text = "<br/>";
                    e.Cell.Controls.Add(literal1);
                    Label label1 = new Label();
                    label1.Text = aux.NombreTrabajo;
                    label1.Font.Size = new FontUnit(FontSize.Small);
                    e.Cell.Controls.Add(label1);
                }
                if (aux.FechaFin.Date == e.Day.Date)
                {
                    Literal literal1 = new Literal();
                    literal1.Text = "<br/>";
                    e.Cell.Controls.Add(literal1);
                    Label label1 = new Label();
                    label1.Text = aux.NombreTrabajo;
                    label1.Font.Size = new FontUnit(FontSize.Small);
                    e.Cell.Controls.Add(label1);
                }
            }
        }

        protected void Calendario_SelectionChanged(object sender, EventArgs e)
        {
            LabelAction.Text = "Date changed to :" + Calendario.SelectedDate.ToShortDateString();
        }

        protected void Calendario_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            LabelAction.Text = "Month changed to :" + e.NewDate.ToShortDateString();
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

            if(usrAux != null)
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

    }
}