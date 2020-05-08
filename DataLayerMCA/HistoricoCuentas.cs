namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HistoricoCuentas
    {
        [Key]
        public int historicoCuentaId { get; set; }

        public int? cuentaId { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(400)]
        public string Descripcion { get; set; }
    }
}
