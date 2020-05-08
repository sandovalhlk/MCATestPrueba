namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cuentas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cuentas()
        {
            CierreCuentas = new HashSet<CierreCuentas>();
            ComprobanteCuentas = new HashSet<ComprobanteCuentas>();
        }

        [Key]
        public int cuentaId { get; set; }

        public int tipoCuentaId { get; set; }

        [Required]
        [StringLength(50)]
        public string codigo { get; set; }

        [StringLength(450)]
        public string descripcion { get; set; }

        public int moduloMCAId { get; set; }

        public int nivel { get; set; }

        public int? jerarquia { get; set; }

        public virtual Cuentas padre { get; set; }
        public virtual ICollection<Cuentas> hijos { get; set; }

        [Required]
        [StringLength(50)]
        public string naturaleza { get; set; }

        public int estadoId { get; set; }

        public int? bancoId { get; set; }

        public bool? Cuentabanco { get; set; }

        public virtual Bancos Bancos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CierreCuentas> CierreCuentas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComprobanteCuentas> ComprobanteCuentas { get; set; }

        public virtual Estados Estados { get; set; }

        public virtual ModulosMCA ModulosMCA { get; set; }

        public virtual TipoCuentas TipoCuentas { get; set; }
    }
}
