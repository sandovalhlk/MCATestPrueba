namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuarios()
        {
            UsuarioModulosMCA = new HashSet<UsuarioModulosMCA>();
        }

        [Key]
        public int usuarioId { get; set; }

        [Required]
        [StringLength(25)]
        public string nombres { get; set; }

        [Required]
        [StringLength(40)]
        public string apellidos { get; set; }

        [Required]
        [StringLength(18)]
        public string cedula { get; set; }

        [StringLength(50)]
        public string telefonos { get; set; }

        [StringLength(400)]
        public string direccion { get; set; }

        [StringLength(128)]
        public string aspNetUserId { get; set; }

        public int? ucrId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual Ucrs Ucrs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioModulosMCA> UsuarioModulosMCA { get; set; }
    }
}
