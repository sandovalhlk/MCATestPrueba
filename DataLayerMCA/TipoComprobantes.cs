namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TipoComprobantes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoComprobantes()
        {
            Comprobantes = new HashSet<Comprobantes>();
        }

        [Key]
        public int tipoComprobanteId { get; set; }

        [Required]
        [StringLength(50)]
        public string tipoComprobante { get; set; }

        [Required]
        [StringLength(3)]
        public string nomeclatura { get; set; }

        [StringLength(150)]
        public string descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comprobantes> Comprobantes { get; set; }
    }
}
