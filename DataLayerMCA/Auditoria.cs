namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Auditoria")]
    public partial class Auditoria
    {
        public long auditoriaId { get; set; }

        public DateTime fecha { get; set; }

        [Required]
        [StringLength(400)]
        public string entidad { get; set; }

        public long? valorKeyEntidad { get; set; }

        [Required]
        [StringLength(50)]
        public string usuarioId { get; set; }

        [StringLength(50)]
        public string usuarioName { get; set; }

        [Required]
        [StringLength(10)]
        public string operacion { get; set; }

        [Column(TypeName = "xml")]
        public string valoresAnterior { get; set; }

        [Column(TypeName = "xml")]
        public string valoresNuevo { get; set; }

        [StringLength(50)]
        public string maquinaIp { get; set; }
    }
}
