namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ModulosMCA")]
    public partial class ModulosMCA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModulosMCA()
        {
            AcuerdoSupMCA = new HashSet<AcuerdoSupMCA>();
            Cuentas = new HashSet<Cuentas>();
            UsuarioModulosMCA = new HashSet<UsuarioModulosMCA>();
        }

        [Key]
        public int moduloMCAId { get; set; }

        [Required]
        [StringLength(250)]
        public string nombreModuloMCA { get; set; }

        [Required]
        [StringLength(30)]
        public string ruc { get; set; }

        public DateTime fechaConstitucion { get; set; }

        [Required]
        [StringLength(30)]
        public string numContrato { get; set; }

        public double monto { get; set; }

        public double longuitud { get; set; }

        public DateTime fechaInicio { get; set; }

        public DateTime fechaFin { get; set; }

        public int proyectoId { get; set; }

        public int municipioId { get; set; }

        public DateTime pFiscalInicio { get; set; }

        public DateTime pFiscalFin { get; set; }

        public int departamentoId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcuerdoSupMCA> AcuerdoSupMCA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cuentas> Cuentas { get; set; }

        public virtual Departamentos Departamentos { get; set; }

        public virtual Municipios Municipios { get; set; }
        public virtual Proyectos Proyectos { get; set; }

        public virtual ICollection<Cierres> Cierres { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioModulosMCA> UsuarioModulosMCA { get; set; }
    }
}
