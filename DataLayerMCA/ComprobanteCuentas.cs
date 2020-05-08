namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ComprobanteCuentas
    {
        [Key]
        public int comprobanteCuentaId { get; set; }

        public int comprobanteId { get; set; }

        public int cuentaId { get; set; }

        public double credito { get; set; }

        public double debito { get; set; }

        public DateTime fecha { get; set; }

        public DateTime fechaTransaccion { get; set; }

        public int estadoId { get; set; }

        public virtual Comprobantes Comprobantes { get; set; }

        public virtual Cuentas Cuentas { get; set; }
    }
}
