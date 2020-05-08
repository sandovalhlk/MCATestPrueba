using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;
using System.Data.Entity;

namespace BusinessLogicMCA
{
    public class CierresBL : BaseRepository<Cierres>
    {
        #region Metodos
        /// <summary>
        /// CREA EL CIERRE O CALCULO DE SALDOS CORRESPONDIENTE AL PERIODO CIERRE CATALOGO
        /// </summary>
        /// <param name="fechaTrans"></param>
        /// <param name="tipoCierreId"></param>
        /// <returns>cierreId PARA CREAR DETALLE DEL CIERRE CON SUS CUENTAS</returns>
        public static int CrearCierre(DateTime fechaTrans, int tipoCierreId, int estadoId, int moduloMCAId)
        {
            int cierreId = 0;
            using (var contabilidadMCA = new ContabilidadMCA())
            {
                using (var transaction = contabilidadMCA.Database.BeginTransaction())
                {
                    try
                    {
                        var cierres = new CierresBL().Get(filter: x => x.moduloMCAId == moduloMCAId); //SE OBTENDRAN TODOS LOS CIERRES DEL MCA //COMPROBAR QUE NO EXISTA UN CIERRE OFICIAL EN ESE MES 
                                                                                                      //SE TIENEN QUE EXCEPTUAR LOS CIERRES ANUALES Y TOTALES
                        if (cierres.Where(x => x.fechaTransaccion.Month == fechaTrans.Month && x.fechaTransaccion.Year == fechaTrans.Year && x.estadoId == EstadosBL.KEY_ASENTADO
                        && x.tipoCierreId == tipoCierreId).Count() > 0)
                            cierreId = 0;
                        else
                        {
                            //BORAR EL Cierre Actual si existe ♦BUG♦
                            var cierreAnular = cierres.Where(x => x.fechaTransaccion.Month == fechaTrans.Month && x.fechaTransaccion.Year == fechaTrans.Year && x.estadoId == EstadosBL.KEY_EJECUTADO).FirstOrDefault();
                            if (cierreAnular != null)
                            {
                                var objCierre = contabilidadMCA.Cierres.Where(s => s.cierreId == cierreAnular.cierreId).FirstOrDefault();
                                objCierre.estadoId = EstadosBL.KEY_ANULADO;

                                //PRIMERAMENTE HAY Q ELIMINAR LOS DATOS DE LA TABLA DE CIERRESCUENTAS ******************************
                                contabilidadMCA.CierreCuentas.RemoveRange(contabilidadMCA.CierreCuentas.Where(x => x.cierreId == objCierre.cierreId));
                                //SEGUNDO HAY Q ANULAR EL CIERRE ******************************
                                contabilidadMCA.Cierres.Attach(objCierre);
                                contabilidadMCA.Entry(objCierre).State = EntityState.Modified;
                                contabilidadMCA.SaveChanges();

                            }

                            Cierres toCierre = new Cierres();
                            toCierre.anio = fechaTrans.Year;
                            toCierre.mes = fechaTrans.Month;
                            toCierre.tipoCierreId = tipoCierreId;
                            toCierre.fechaTransaccion = fechaTrans;
                            toCierre.fecha = DateTime.Now;
                            toCierre.estadoId = estadoId;
                            toCierre.moduloMCAId = moduloMCAId;

                            contabilidadMCA.Cierres.Add(toCierre);
                            contabilidadMCA.SaveChanges();

                            cierreId = toCierre.cierreId;
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        Console.WriteLine(exception.Message + "Transaction Roll backed due to some exception");
                    }
                }
            }
            return cierreId;
        }

        public static string RevertirCierre(ReversionCierres reversionCierre, int moduloMCAId, int usuarioId)
        {
            string msj = "";
            //   int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            using (var contabilidadMCA = new ContabilidadMCA())
            {
                using (var transaction = contabilidadMCA.Database.BeginTransaction())
                {
                    try
                    {
                        var repoCierre = new CierresBL();

                        // var repoReversion = new ReversionCierreBL();
                        var cierres = contabilidadMCA.Cierres.Where(x => x.cierreId >= reversionCierre.cierreInicio && x.estadoId != 2);
                        int cierreId = cierres.ToList().LastOrDefault().cierreId;
                        foreach (Cierres c in cierres) //ANULO TODOS LOS CIERRE DESPUES DEL CIERRE SELECCIONADO
                        {

                            c.estadoId = EstadosBL.KEY_ANULADO;
                            c.fecha = DateTime.Now;

                            contabilidadMCA.Cierres.Attach(c);
                            contabilidadMCA.Entry(c).State = EntityState.Modified;
                            contabilidadMCA.SaveChanges();

                            if (c.tipoCierreId == 2)//SE TIENEN QUE ELIMINAR LOS COMPROBANTES DE RESULTADO DEL EJERCICIO
                            {
                                //var comprobantes = repoComprobanteBL.Get(filter: x => x.tipoComprobanteId == 3 || x.tipoComprobanteId == 4).Where(s => s.ComprobanteCuentas.Select(k => k.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId);
                                var comprobantes = contabilidadMCA.Comprobantes.Where(x => x.tipoComprobanteId == 3 || x.tipoComprobanteId == 4).Where(s => s.ComprobanteCuentas.Select(k => k.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId);
                                foreach (Comprobantes comp in comprobantes)
                                {
                                    comp.estadoId = 2;
                                    //  repoComprobanteBL.Update(comp);*********************
                                    contabilidadMCA.Comprobantes.Attach(comp);
                                    contabilidadMCA.Entry(comp).State = EntityState.Modified;
                                    contabilidadMCA.SaveChanges();
                                }
                            }
                        }

                        reversionCierre.cierreFin = cierreId;
                        reversionCierre.estadoId = EstadosBL.KEY_EJECUTADO;
                        reversionCierre.fecha = DateTime.Now;
                        reversionCierre.fechaTransaccion = DateTime.Now;
                        reversionCierre.razonReversion = reversionCierre.razonReversion;
                        reversionCierre.solicitante = usuarioId;

                        //repoReversion.Add(reversionCierre);******************
                        contabilidadMCA.ReversionCierres.Add(reversionCierre);
                        contabilidadMCA.SaveChanges();

                        msj = "";
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction Roll backed due to some exception" + exception.Message);
                        // return false;
                    }
                }

            }
            return msj;
        }

        public string fechaCierreValidacion(int moduloMCAId, DateTime fechaTransaccion)
        {

            string msj = "";
            var toModulo = new ModulosMCABL().GetByID(moduloMCAId);
            var objAcuerdo = toModulo.AcuerdoSupMCA.ToList();
            var cierre = new CierresBL().Get(filter: x => x.moduloMCAId == moduloMCAId && (x.estadoId == EstadosBL.KEY_EJECUTADO || x.estadoId == EstadosBL.KEY_ASENTADO) && x.fechaTransaccion.Month == fechaTransaccion.Month && x.fechaTransaccion.Year == fechaTransaccion.Year).FirstOrDefault();

            if (objAcuerdo != null)
                if (objAcuerdo.Count() > 0)
                {
                    if ((fechaTransaccion.Month) > objAcuerdo.LastOrDefault().fechaFinal.Month  && objAcuerdo.LastOrDefault().fechaFinal.Year == fechaTransaccion.Year && cierre.tipoCierreId != 3)
                        msj = "LA FECHA SELECCIONADA PERTENECE AL PERIODO CONTABLE SEGUN ACUERDOS SUPLEMENTARIO, REALICE CALCULO DE SALDOS PARA CIERRE CONTABLE";
                }
                else
                {
                    if ((fechaTransaccion.Month) > toModulo.fechaFin.Month && toModulo.fechaFin.Year == fechaTransaccion.Year && cierre.tipoCierreId != 3)
                        msj = "LA FECHA SELECCIONADA PERTENECE AL CIERRE CONTABLE, REALICE CALCULO DE SALDOS PARA CIERRE CONTABLE";
                }

            return msj;
        }

        #endregion
    }
}
