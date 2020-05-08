namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    
    public partial class ReinicioCheques
    {
        [Key]
        public int reinicioChequeId { get; set; }

        public int? comprobanteId { get; set; }

        public int numero { get; set; }
       
        [Required]
        [StringLength(50)]
        public string usuario { get; set; }

        [Required]
        [StringLength(350)]
        public string justificacion { get; set; }

        public DateTime fecha { get; set; }

        public int moduloMCAId { get; set; }
    }
}
