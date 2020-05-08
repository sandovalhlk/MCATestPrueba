namespace DataLayerMCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetEmails
    {
        [Key]
        public int emailDataId { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public bool UseDefaultCredentials { get; set; }
    }
}
