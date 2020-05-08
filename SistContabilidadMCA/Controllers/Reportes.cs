using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicMCA;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using SistContabilidadMCA;

namespace Correspondencia.Controllers
{
    public class ReportesController : Controller
    {
        #region Variables

        public string nombreRepore = "_file";
        public enum TipoExportacion
        {
            pdf = 0,
            xlsx = 1
        }

        #endregion
        #region Acciones

        public ActionResult DownloadReport(TipoExportacion tipoExp = TipoExportacion.pdf)
        {
            var stream = new MemoryStream(); string formato = string.Empty;
            if (Session["reporte"] != null)
            {
                var report = new XtraReport();
                report = (XtraReport)Session["reporte"];
                report.CreateDocument();

                switch (tipoExp)
                {
                    case TipoExportacion.pdf:

                        report.ExportToPdf(stream);
                        nombreRepore = report.Name + nombreRepore;

                        var cd = new System.Net.Mime.ContentDisposition
                        {
                            FileName = nombreRepore + ".pdf",
                            Inline = false,
                        };
                        Response.AppendHeader("Content-Disposition", cd.ToString());
                        formato = "application/pdf";

                        break;
                    case TipoExportacion.xlsx:

                        report.Name = nombreRepore + ".xlsx";
                        Response.AppendHeader("content-disposition", "attachment; filename=" + report.Name);
                        report.ExportToXlsx(stream, new XlsxExportOptions() { ExportMode = XlsxExportMode.SingleFile });
                        formato = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        break;
                }
                Session.Remove("reporte");
            }

            return File(stream.GetBuffer(), formato);
        }

        public ActionResult OpenReport(TipoExportacion tipoExp = TipoExportacion.pdf)
        {
            var stream = new MemoryStream(); string formato = string.Empty;
            if (Session["reporte"] != null)
            {
                var report = new XtraReport();
                report = (XtraReport)Session["reporte"];
                report.CreateDocument();

                switch (tipoExp)
                {
                    case TipoExportacion.pdf:
                        report.Name = nombreRepore + ".pdf";
                        report.ExportToPdf(stream);
                        formato = "application/pdf";
                        break;

                    case TipoExportacion.xlsx:
                        report.Name = nombreRepore + ".xlsx";
                        report.ExportToXlsx(stream);
                        formato = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        break;
                }
                Session.Remove("reporte");
            }

            return File(stream.GetBuffer(), formato);
        }

        public ActionResult OpenImpresion(TipoExportacion tipoExp = TipoExportacion.pdf)
        {
            var stream = new MemoryStream(); string formato = string.Empty;
            if (Session["reporte"] != null)
            {
                var report = new XtraReport();
                report = (XtraReport)Session["reporte"];
                report.CreateDocument();

                switch (tipoExp)
                {
                    case TipoExportacion.pdf:
                        var opts = new PdfExportOptions();
                        opts.ShowPrintDialogOnOpen = true;
                        report.ExportToPdf(stream, opts);
                        stream.Seek(0, SeekOrigin.Begin);

                        Byte[] reportArray = stream.ToArray();
                        formato = "application/pdf";

                        break;
                    case TipoExportacion.xlsx:

                        var opts1 = new XlsxExportOptions();
                        opts1.ShowGridLines = true;
                        report.ExportToXlsx(stream, opts1);
                        stream.Seek(0, SeekOrigin.Begin);

                        Byte[] reportArray1 = stream.ToArray();
                        formato = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        break;
                }
                Session.Remove("reporte");
            }
            return File(stream.GetBuffer(), formato);
        }

        #endregion
    }
}