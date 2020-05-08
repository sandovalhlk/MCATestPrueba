using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicMCA;
using DataLayerMCA;

namespace SistContabilidadMCA.Areas.Catalogos.Controllers
{
    [Authorize]
    public class ConvenioController : Controller
    {
        #region Instancias
        private ConveniosBL repoConvenios = new ConveniosBL();
        #endregion

        #region HTTP
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GetListaConvenios()
        {
            repoConvenios.ProxyCreationEnabled();
            var getConvenios = repoConvenios.Get(filter: x => x.estadoId == EstadosBL.KEY_ACTIVO).ToList();
            return PartialView("_GetListaConvenios", getConvenios);
        }


        #endregion

        #region Funciones AJAX

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConvenios(Convenios toConvenio)
        {
            string mensaje = "";
            bool existeConvenio = ConveniosBL.ExisteConvenios(toConvenio, out mensaje);
            if (existeConvenio == false)
            {
                //Elimino espacios principio y final, convierto cadena en mayusculas
                toConvenio.convenio = toConvenio.convenio.Trim().ToUpper();
                toConvenio.estadoId = EstadosBL.KEY_ACTIVO;
                if (toConvenio.convenioId == 0)
                    repoConvenios.Add(toConvenio);
                else
                    repoConvenios.Update(toConvenio);
            }
            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetConvenio(int convenioId)
        {
            string mensaje = "";
            repoConvenios.ProxyCreationEnabled();
            bool convenioProyecto = ConveniosBL.ConvenioProyecto(convenioId);
            if (convenioProyecto == false)
            {
                var toConvenios = repoConvenios.GetByID(convenioId);
                return Json(toConvenios, JsonRequestBehavior.AllowGet);
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BajaConvenios(int convenioId)
        {
            string mensaje = "";
            repoConvenios.ProxyCreationEnabled();
            bool EmpresaEnUso = ConveniosBL.ConvenioProyecto(convenioId);
            if (EmpresaEnUso == false)
            {
                var toConvenios = repoConvenios.GetByID(convenioId);
                toConvenios.estadoId = EstadosBL.KEY_ANULADO;
                repoConvenios.Update(toConvenios);
                return Json(toConvenios, JsonRequestBehavior.AllowGet);
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetComboConvenios()
        {
            repoConvenios.ProxyCreationEnabled();
            var toConvenios = repoConvenios.Get(filter: y => y.estadoId == EstadosBL.KEY_ACTIVO).Select(x => new {x.convenioId, x.convenio }).OrderBy(y => y.convenio).ToList();
            return Json(toConvenios, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}