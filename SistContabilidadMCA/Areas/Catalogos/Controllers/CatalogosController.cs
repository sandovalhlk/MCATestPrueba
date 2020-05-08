using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using System.Web.Mvc;
using BusinessLogicMCA;
using DataLayerMCA;
using System.Globalization;

namespace SistContabilidadMCA.Areas.Catalogos.Controllers
{
    [Authorize]
    public class CatalogosController : Controller
    {
        #region Instancias
        private ProyectosBL repoProyectos = new ProyectosBL();
        private UcrsBL repoUcrs = new UcrsBL();
        private ConveniosBL repoConvenios = new ConveniosBL();
        private UsuariosBL repoUsuario = new UsuariosBL();

        #endregion


        #region HTTP
        // GET: Catalogos/Catalogos
        public ActionResult Index()
        {
            var usuario = User.Identity.Name;
            var toUsuario = repoUsuario.Get(filter: x => x.AspNetUsers.UserName == usuario).FirstOrDefault();
            //int ucrId = toUsuario.Ucrs.ucrId;
            int ucrId = toUsuario == null ? 0 : toUsuario.Ucrs.ucrId;
            var toUcr = repoUcrs.Get(filter: x => x.ucrId == ucrId).SingleOrDefault();

            //combos
            ViewBag.convenio = new SelectList(repoConvenios.Get(filter: x => x.estadoId == EstadosBL.KEY_ACTIVO).OrderBy(s => s.convenio).ToList(), "convenioId", "convenio");
            //ViewBag.convenio = new SelectList(repoProyectos.Get(filter: x => x.estadoId == EstadosBL.KEY_ACTIVO).Select(x => new { x.Convenios.convenio,x.Convenios.convenioId}).OrderBy(s => s.convenio).ToList(), "convenioId", "convenio");

            ViewBag.ucrs = new SelectList(repoUcrs.GetAll().Select(x => new { x.ucrId, x.ucr }).OrderBy(s => s.ucr).ToList(), "ucrId", "ucr");
            ViewBag.ucrsusuario = toUsuario == null ? 0 : toUsuario.Ucrs.ucrId;
            ViewBag.ucr = toUcr.ucr;
            return View();
        }



        #endregion

        #region Funciones AJAX


        
        [ValidateAntiForgeryToken]
        public ActionResult SaveProyecto(Proyectos toProyecto , FormCollection collection)
        {


            string mensaje = "";
            bool existeProyecto = ProyectosBL.ExisteProyecto(toProyecto, out mensaje);
            if (existeProyecto == false)
            {
                //Elimino espacios principio y final, convierto cadena en mayusculas
                toProyecto.nombreProyecto = toProyecto.nombreProyecto.Trim().ToUpper();
                //toProyecto.longuitud = Convert.ToDouble(toProyecto.longuitud);
                //toProyecto.monto = Convert.ToDouble(toProyecto.monto);
               //toProyecto.longuitud = Convert.ToDouble(collection["longuitud"].ToString());
               // toProyecto.monto = Convert.ToDouble(collection["monto"].ToString());


                toProyecto.fechaInicio = DateTime.Parse(collection["fechaInicio"].ToString());
                toProyecto.fechaFin = DateTime.Parse(collection["fechaFin"].ToString());
                toProyecto.convenioId = toProyecto.convenioId;
                toProyecto.ucrId = toProyecto.ucrId;
                toProyecto.estadoId = EstadosBL.KEY_ACTIVO;
                if (toProyecto.proyectoId == 0)
                {
                    repoProyectos.Add(toProyecto);
                }
                else
                {
                    repoProyectos.Update(toProyecto);
                }

            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetProyecto(int proyectoId)
        {
            string mensaje = "";
            repoProyectos.ProxyCreationEnabled();
            bool proyectoModulo = ProyectosBL.Proyectomodulo(proyectoId);
            if (proyectoModulo == false)
            {

                var toProyecto = repoProyectos.Get(filter: x => x.proyectoId == proyectoId).Select(x => new { x.proyectoId, x.nombreProyecto, x.longuitud, x.convenioId, x.ucrId, monto = String.Format("{0:#,##0.00}", x.monto), fechaInicio = x.fechaInicio.ToString("dd/MM/yyyy"), fechaFin = x.fechaFin.ToString("dd/MM/yyyy") }).FirstOrDefault();              
                return Json(toProyecto, JsonRequestBehavior.AllowGet);
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult BajaProyecto(int proyectoId)
        {
            string mensaje = "";
            repoProyectos.ProxyCreationEnabled();
            bool ProyectoModulo = ProyectosBL.Proyectomodulo(proyectoId);
            if (ProyectoModulo == false)
            {
                var toProyecto = repoProyectos.GetByID(proyectoId);
                toProyecto.estadoId = EstadosBL.KEY_ANULADO;
                repoProyectos.Update(toProyecto);
                return Json(toProyecto, JsonRequestBehavior.AllowGet);
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Vistas Parciales
        public ActionResult GetListaProyectos(int ucrId)
        {

            var ucrs = repoUcrs.Get(filter: x => x.ucrId == ucrId).Select(x => new { x.ucr }).FirstOrDefault();
            ViewBag.ucr = ucrs.ucr;
            var toProyecto = repoProyectos.Get(filter: x => x.ucrId == ucrId && x.estadoId == EstadosBL.KEY_ACTIVO).Select(x => new { x.nombreProyecto, monto = "C$" + String.Format("{0:#,##0.00}", x.monto), x.longuitud, fechaInicio = x.fechaInicio.ToString("dd/MM/yyyy"), fechaFin = x.fechaFin.ToString("dd/MM/yyyy"), x.Ucrs.ucr, x.Convenios.convenio, x.ucrId, x.proyectoId }).ToList();
            //var ListProyectos = repoProyectos.Get(filter: x => x.ucrId == ucrId && x.estadoId == EstadosBL.KEY_ACTIVO).Select(x => new { x.nombreProyecto, x.monto, x.longuitud, x.fechaInicio, x.fechaFin, x.Ucrs.ucr, x.Convenios.convenio, x.ucrId, x.proyectoId }).OrderBy(y => y.proyectoId).ToList();          
            return PartialView("_GetListaProyectos", toProyecto );
        }
        #endregion





    }
}