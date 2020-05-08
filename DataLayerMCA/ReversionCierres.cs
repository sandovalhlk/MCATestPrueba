namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReversionCierres
    {
        [Key]
        public int reversionCierreId { get; set; }

        public int cierreInicio { get; set; }

        public int cierreFin { get; set; }

        public int? solicitante { get; set; }

        public int? autorizador { get; set; }

        [Required]
        [StringLength(350)]
        public string razonReversion { get; set; }

        public DateTime fecha { get; set; }

        public DateTime fechaTransaccion { get; set; }

        public int estadoId { get; set; }
    }
}
