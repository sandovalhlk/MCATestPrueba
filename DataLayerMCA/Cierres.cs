namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cierres
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cierres()
        {
            CierreCuentas = new HashSet<CierreCuentas>();
        }

        [Key]
        public int cierreId { get; set; }

        public int mes { get; set; }

        public int anio { get; set; }

        public DateTime fecha { get; set; }

        public DateTime fechaTransaccion { get; set; }

        public int tipoCierreId { get; set; }

        public int? estadoId { get; set; }

        public int? moduloMCAId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CierreCuentas> CierreCuentas { get; set; }

        
        public virtual ModulosMCA ModulosMCA { get; set; }

        public virtual TipoCierres TipoCierres { get; set; }
    }
}
