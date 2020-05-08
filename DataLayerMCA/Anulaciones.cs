namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Anulaciones
    {
        [Key]
        public int anulacionId { get; set; }

        [StringLength(450)]
        public string razon { get; set; }

        public long? entidadId { get; set; }

        [StringLength(50)]
        public string entidadNombre { get; set; }

        [StringLength(20)]
        public string usuario { get; set; }

        public DateTime? fecha { get; set; }
    }
}
