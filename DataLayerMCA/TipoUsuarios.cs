namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TipoUsuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoUsuarios()
        {
            UsuarioModulosMCA = new HashSet<UsuarioModulosMCA>();
        }

        [Key]
        public int tipoUsuarioId { get; set; }

        [Required]
        [StringLength(50)]
        public string tipoUsuario { get; set; }

        [StringLength(250)]
        public string descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioModulosMCA> UsuarioModulosMCA { get; set; }
    }
}
