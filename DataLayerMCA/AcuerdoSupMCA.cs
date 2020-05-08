namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AcuerdoSupMCA")]
    public partial class AcuerdoSupMCA
    {
        public int acuerdoSupMCAId { get; set; }

        public int moduloMACId { get; set; }

        public int numero { get; set; }

        [StringLength(250)]
        public string descripcion { get; set; }

        public int tipoExtencionId { get; set; }

        public double? monto { get; set; }

        public double? loguitud { get; set; }

        public DateTime fechaInicial { get; set; }

        public DateTime fechaFinal { get; set; }

        public int estadoId { get; set; }

        public virtual Estados Estados { get; set; }

        public virtual ModulosMCA ModulosMCA { get; set; }

        public virtual TipoExtensiones TipoExtensiones { get; set; }
    }
}
