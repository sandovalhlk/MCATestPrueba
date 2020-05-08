namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comprobantes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comprobantes()
        {
            ComprobanteCuentas = new HashSet<ComprobanteCuentas>();
            DetalleConciliacionBancarias = new HashSet<DetalleConciliacionBancarias>();
        }

        [Key]
        public int comprobanteId { get; set; }

        [Required]
        [StringLength(8)]
        public string numero { get; set; }

        [StringLength(600)]
        public string concepto { get; set; }

        [StringLength(60)]
        public string beneficiario { get; set; }

        [StringLength(10)]
        public string numCheque { get; set; }

        public DateTime fecha { get; set; }

        public DateTime fechaComprobante { get; set; }

        public int tipoComprobanteId { get; set; }

        public int estadoId { get; set; }

        public bool? conciliado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComprobanteCuentas> ComprobanteCuentas { get; set; }

        public virtual TipoComprobantes TipoComprobantes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleConciliacionBancarias> DetalleConciliacionBancarias { get; set; }
    }
}
