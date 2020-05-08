namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetalleConciliacionBancarias
    {
        [Key]
        public int detalleConcialicionBancariaId { get; set; }

        public int comprobanteId { get; set; }

        public DateTime fecha { get; set; }

        public int conciliacionBancariaId { get; set; }

        public int estadoId { get; set; }

        public virtual Comprobantes Comprobantes { get; set; }

        public virtual ConciliacionBancarias ConciliacionBancarias { get; set; }
    }
}
