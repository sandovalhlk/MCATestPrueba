using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;
using System.Data.Entity;
using System.Web;

namespace BusinessLogicMCA
{
    public class ComprobantesBL : BaseRepository<Comprobantes>
    {

        #region Metodos
        public static List<CListadoCheques> GetListadoCheques(int @cuentaId, DateTime fechaTransaccion)
        {
            var repo = new BaseRepository<CListadoCheques>();
            repo.AddParameter("@cuentaId", cuentaId);
            repo.AddParameter("@fechaTransaccion", fechaTransaccion);
            return repo.GetFromDatabaseWithQuery("getListadoCheques").ToList<CListadoCheques>();
        }

        public static List<CListadoCheques> GetListadoChequesEdit(int conciliacionBancariaId)
        {
            var repo = new BaseRepository<CListadoCheques>();
            repo.AddParameter("@conciliacionBancariaId", conciliacionBancariaId);
            return repo.GetFromDatabaseWithQuery("getListadoChequesEdit").ToList<CListadoCheques>();
        }

        public static List<CListaComprobanteCuenta> getComprobantePago(int comprobanteId)
        {
            var repo = new BaseRepository<CListaComprobanteCuenta>();
            repo.AddParameter("@comprobanteId", comprobanteId);
            return repo.GetFromDatabaseWithQuery("getComprobantePago").ToList<CListaComprobanteCuenta>();
        }


        public static string AnularComprobante(int comprobanteId = 0, string razonAnulacion = "")
        {
            string msj = "";

            using (var contabilidadMCA = new ContabilidadMCA())
            {
                using (var transaction = contabilidadMCA.Database.BeginTransaction())
                {

                    try
                    {
                        var comprobante = contabilidadMCA.Comprobantes.Find(comprobanteId);
                        comprobante.estadoId = EstadosBL.KEY_ANULADO;

                        contabilidadMCA.Comprobantes.Attach(comprobante);
                        contabilidadMCA.Entry(comprobante).State = EntityState.Modified;
                        contabilidadMCA.SaveChanges();

                        var anulacion = new Anulaciones();
                        anulacion.entidadId = comprobante.comprobanteId;
                        anulacion.entidadNombre = "Comprobantes";
                        anulacion.fecha = DateTime.Now;
                        anulacion.razon = razonAnulacion;
                        anulacion.usuario = HttpContext.Current.User.Identity.Name;
                        contabilidadMCA.Anulaciones.Add(anulacion);
                        contabilidadMCA.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        msj = "Ocurrio un error en el proceso " + e.Message;
                        transaction.Rollback();
                        Console.WriteLine("Transaction Roll backed due to some exception" + e.Message);

                    }
                }

            }

            return msj;
        }

        public static bool ValidarComprobanteCierre(int comprobanteId)
        {
            bool estado = false;

            var comprobante = new ComprobantesBL().GetByID(comprobanteId);

            if (comprobante != null)
            {
                int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(HttpContext.Current.User.Identity.Name);
                var Lastcierres = new CierresBL().Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ASENTADO).LastOrDefault();

                if (Lastcierres != null)
                {//comprobar si el comprobante es mayor q la fecha del ultimo cierre. si es mayor es q no esta sentado y se puede anular.
                    if ((comprobante.fechaComprobante.Month > Lastcierres.fechaTransaccion.Month && comprobante.fechaComprobante.Year == Lastcierres.fechaTransaccion.Year) || (comprobante.fechaComprobante.Year > Lastcierres.fechaTransaccion.Year))
                        estado = true;

                }
                else
                    estado = true;

            }

            return estado;
        }
        #endregion

        #region Clases

        public class CListadoCheques
        {
            public int comprobanteId { get; set; }

            public string numCheque { get; set; }
            public string beneficiario { get; set; }
            public double cantidad { get; set; }

        }


        //public class CListaComprobanteCuenta : ComprobanteCuentas
        //{
        //    public string beneficiario { get; set; }
        //    public string numCheque { get; set; }
        //    public DateTime fechaComprobante { get; set; }
        //    public int moduloMCAId { get; set; }


        //}

        public class CListaComprobanteCuenta
        {
            public string municipio { get; set; }
            public string beneficiario { get; set; }
            public string numCheque { get; set; }
            public string dia { get; set; }
            public string mes { get; set; }
            public string ano { get; set; }
            public double cantidad { get; set; }
            public string cantidaletra { get; set; }
        }



        #endregion


    }
}
