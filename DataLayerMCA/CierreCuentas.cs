namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CierreCuentas
    {
        [Key]
        public int cierreCuentaId { get; set; }

        public int cierreId { get; set; }

        public int cuentaId { get; set; }

        public double saldoInicialCredito { get; set; }

        public double saldoFinalCredito { get; set; }

        public double saldoInicialDebito { get; set; }

        public double saldoFinalDebito { get; set; }

        public double credito { get; set; }

        public double debito { get; set; }

        public DateTime fecha { get; set; }

        public DateTime fechaTransaccion { get; set; }

        public virtual Cierres Cierres { get; set; }

        public virtual Cuentas Cuentas { get; set; }
    }
}
