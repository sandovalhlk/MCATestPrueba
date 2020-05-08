namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioModulosMCA")]
    public partial class UsuarioModulosMCA
    {
        [Key]
        public int usuarioModuloMCAId { get; set; }

        public int usuarioId { get; set; }

        public int moduloMCAId { get; set; }

        public DateTime fecha { get; set; }

        public int tipoUsuarioId { get; set; }

        public bool firmante { get; set; }

        public int estadoId { get; set; }

        public virtual ModulosMCA ModulosMCA { get; set; }

        public virtual TipoUsuarios TipoUsuarios { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
