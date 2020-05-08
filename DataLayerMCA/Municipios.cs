namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Municipios
    {
        [Key]
        public int municipioId { get; set; }

        [Required]
        [StringLength(50)]
        public string municipio { get; set; }

        public int departamentoId { get; set; }

        public virtual Departamentos Departamentos { get; set; }
    }
}
