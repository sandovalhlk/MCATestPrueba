namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Proyectos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proyectos()
        {
            ModulosMCA = new HashSet<ModulosMCA>();
        }

        [Key]
        public int proyectoId { get; set; }

        [Required]
        [StringLength(200)]
        public string nombreProyecto { get; set; }
          
        public double monto { get; set; }

        public double longuitud { get; set; }

        public DateTime fechaInicio { get; set; }

        public DateTime fechaFin { get; set; }

        public int convenioId { get; set; }

        public int ucrId { get; set; }

        public int estadoId { get; set; }

        public virtual Convenios Convenios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModulosMCA> ModulosMCA { get; set; }

        public virtual Ucrs Ucrs { get; set; }
    }
}
