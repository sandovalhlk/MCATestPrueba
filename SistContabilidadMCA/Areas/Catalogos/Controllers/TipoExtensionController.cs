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
    public class TipoExtensionController : Controller
    {
        #region Instancias
        private TipoExtensionesBL repoTipoExtensiones = new TipoExtensionesBL();
        #endregion

        #region HTTP
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GetListaTipoExtensiones()
        {
            repoTipoExtensiones.ProxyCreationEnabled();
            var getTipoExtensiones = repoTipoExtensiones.Get(filter: x => x.estadoId == EstadosBL.KEY_ACTIVO).ToList();
            return PartialView("_GetListaTipoExtensiones", getTipoExtensiones);
        }

        #endregion

        #region Funciones AJAX

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SaveTipoExtension(TipoExtensiones toTipoExtension)
        {
            string mensaje = "";
            bool existeTipoE = TipoExtensionesBL.ExisteTipoExtensiones(toTipoExtension, out mensaje);
            if (existeTipoE == false)
            {
                //Elimino espacios principio y final, convierto cadena en mayusculas
                toTipoExtension.tipoExtencion = toTipoExtension.tipoExtencion.Trim().ToUpper();
                toTipoExtension.estadoId = EstadosBL.KEY_ACTIVO;
                if (toTipoExtension.tipoExtensionId == 0)
                    repoTipoExtensiones.Add(toTipoExtension);
                else
                    repoTipoExtensiones.Update(toTipoExtension);
            }
            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetTipoExtension(int tipoExtensionId)
        {
            string mensaje = "";
            repoTipoExtensiones.ProxyCreationEnabled();
            bool acuerdoTipoE = TipoExtensionesBL.tipoExtAcuerdo(tipoExtensionId);
            if (acuerdoTipoE == false)
            {
                var toTipoExtension = repoTipoExtensiones.GetByID(tipoExtensionId);
                return Json(toTipoExtension, JsonRequestBehavior.AllowGet);
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult BajaTipoE(int tipoExtensionId)
        {
            string mensaje = "";
            repoTipoExtensiones.ProxyCreationEnabled();
            bool acuerdoTipoE = TipoExtensionesBL.tipoExtAcuerdo(tipoExtensionId);
            if (acuerdoTipoE == false)
            {
                var toTipoExtension = repoTipoExtensiones.GetByID(tipoExtensionId);
                toTipoExtension.estadoId = EstadosBL.KEY_ANULADO;
                repoTipoExtensiones.Update(toTipoExtension);
                return Json(toTipoExtension, JsonRequestBehavior.AllowGet);
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetComboTextensiones()
        {
            repoTipoExtensiones.ProxyCreationEnabled();
            var toTipoE = repoTipoExtensiones.Get(filter: y => y.estadoId == EstadosBL.KEY_ACTIVO).Select(x => new { x.tipoExtensionId, x.tipoExtencion }).OrderBy(y => y.tipoExtencion).ToList();
            return Json(toTipoE, JsonRequestBehavior.AllowGet);
        }



        #endregion


    }
}