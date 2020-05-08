namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TipoExtensiones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoExtensiones()
        {
            AcuerdoSupMCA = new HashSet<AcuerdoSupMCA>();
        }

        [Key]
        public int tipoExtensionId { get; set; }

        [Required]
        [StringLength(50)]
        public string tipoExtencion { get; set; }

        public int estadoId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcuerdoSupMCA> AcuerdoSupMCA { get; set; }

        public virtual Estados Estados { get; set; }
    }
}
