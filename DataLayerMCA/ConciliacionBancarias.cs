namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConciliacionBancarias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConciliacionBancarias()
        {
            DetalleConciliacionBancarias = new HashSet<DetalleConciliacionBancarias>();
        }

        [Key]
        public int conciliacionBancariaId { get; set; }

        public DateTime fecha { get; set; }

        public int mes { get; set; }

        public int anio { get; set; }

        public DateTime fechaTransaccion { get; set; }

        public int? bancoId { get; set; }

        public double saldoEstadoCuentaBanco { get; set; }

        public double partidaConsiliacion { get; set; }

        public int cuentaId { get; set; }

        public double saldoenLibro { get; set; }

        public int estadoId { get; set; }

        public virtual Bancos Bancos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleConciliacionBancarias> DetalleConciliacionBancarias { get; set; }
    }
}
