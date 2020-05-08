namespace DataLayerMCA
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ContabilidadMCA : DbContext
    {
        public ContabilidadMCA()
            : base("name=ContabilidadMCA")
        {
        }

        public virtual DbSet<AcuerdoSupMCA> AcuerdoSupMCA { get; set; }
        public virtual DbSet<Anulaciones> Anulaciones { get; set; }
        public virtual DbSet<AspNetEmails> AspNetEmails { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        //public virtual DbSet<Auditoria> Auditoria { get; set; }
        public virtual DbSet<Bancos> Bancos { get; set; }
        public virtual DbSet<CierreCuentas> CierreCuentas { get; set; }
        public virtual DbSet<CierreCuentasOficial> CierreCuentasOficial { get; set; }
        public virtual DbSet<Cierres> Cierres { get; set; }
        public virtual DbSet<ComprobanteCuentas> ComprobanteCuentas { get; set; }
        public virtual DbSet<Comprobantes> Comprobantes { get; set; }
        public virtual DbSet<ConciliacionBancarias> ConciliacionBancarias { get; set; }
        public virtual DbSet<Convenios> Convenios { get; set; }
        public virtual DbSet<Cuentas> Cuentas { get; set; }
        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<DetalleConciliacionBancarias> DetalleConciliacionBancarias { get; set; }
        public virtual DbSet<Estados> Estados { get; set; }
        public virtual DbSet<HistoricoCuentas> HistoricoCuentas { get; set; }
        public virtual DbSet<ModulosMCA> ModulosMCA { get; set; }
        public virtual DbSet<Municipios> Municipios { get; set; }
        public virtual DbSet<Proyectos> Proyectos { get; set; }
        public virtual DbSet<ReversionCierres> ReversionCierres { get; set; }
        public virtual DbSet<TipoCierres> TipoCierres { get; set; }
        public virtual DbSet<TipoComprobantes> TipoComprobantes { get; set; }
        public virtual DbSet<TipoCuentas> TipoCuentas { get; set; }
        public virtual DbSet<TipoExtensiones> TipoExtensiones { get; set; }
        public virtual DbSet<TipoUsuarios> TipoUsuarios { get; set; }
        public virtual DbSet<Ucrs> Ucrs { get; set; }
        public virtual DbSet<UsuarioModulosMCA> UsuarioModulosMCA { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        public virtual DbSet<ReinicioCheques> ReinicioCheques { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcuerdoSupMCA>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Anulaciones>()
                .Property(e => e.razon)
                .IsUnicode(false);

            modelBuilder.Entity<Anulaciones>()
                .Property(e => e.entidadNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Anulaciones>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetUsers>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetUsers>()
                .Property(e => e.cedula)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Usuarios)
                .WithOptional(e => e.AspNetUsers)
                .HasForeignKey(e => e.aspNetUserId);

            modelBuilder.Entity<Bancos>()
                .Property(e => e.banco)
                .IsUnicode(false);

            modelBuilder.Entity<Bancos>()
                .Property(e => e.siglas)
                .IsUnicode(false);

            modelBuilder.Entity<Cierres>()
                .HasMany(e => e.CierreCuentas)
                .WithRequired(e => e.Cierres)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comprobantes>()
                .Property(e => e.numero)
                .IsUnicode(false);

            modelBuilder.Entity<Comprobantes>()
                .Property(e => e.concepto)
                .IsUnicode(false);

            modelBuilder.Entity<Comprobantes>()
                .Property(e => e.beneficiario)
                .IsUnicode(false);

            modelBuilder.Entity<Comprobantes>()
                .Property(e => e.numCheque)
                .IsUnicode(false);

            modelBuilder.Entity<Comprobantes>()
                .HasMany(e => e.ComprobanteCuentas)
                .WithRequired(e => e.Comprobantes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comprobantes>()
                .HasMany(e => e.DetalleConciliacionBancarias)
                .WithRequired(e => e.Comprobantes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConciliacionBancarias>()
                .HasMany(e => e.DetalleConciliacionBancarias)
                .WithRequired(e => e.ConciliacionBancarias)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Convenios>()
                .Property(e => e.convenio)
                .IsUnicode(false);

            modelBuilder.Entity<Convenios>()
                .HasMany(e => e.Proyectos)
                .WithRequired(e => e.Convenios)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cuentas>()
                .Property(e => e.codigo)
                .IsUnicode(false);

            modelBuilder.Entity<Cuentas>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Cuentas>()
                .Property(e => e.naturaleza)
                .IsUnicode(false);

            modelBuilder.Entity<Cuentas>()
                .HasMany(e => e.CierreCuentas)
                .WithRequired(e => e.Cuentas)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cuentas>()
                .HasMany(e => e.ComprobanteCuentas)
                .WithRequired(e => e.Cuentas)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departamentos>()
                .Property(e => e.departamento)
                .IsUnicode(false);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.ModulosMCA)
                .WithRequired(e => e.Departamentos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departamentos>()
                .HasMany(e => e.Municipios)
                .WithRequired(e => e.Departamentos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Estados>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Estados>()
                .HasMany(e => e.AcuerdoSupMCA)
                .WithRequired(e => e.Estados)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Estados>()
                .HasMany(e => e.Cuentas)
                .WithRequired(e => e.Estados)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Estados>()
                .HasMany(e => e.TipoExtensiones)
                .WithRequired(e => e.Estados)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoricoCuentas>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<HistoricoCuentas>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<ModulosMCA>()
                .Property(e => e.nombreModuloMCA)
                .IsUnicode(false);

            modelBuilder.Entity<ModulosMCA>()
                .Property(e => e.ruc)
                .IsUnicode(false);

            modelBuilder.Entity<ModulosMCA>()
                .Property(e => e.numContrato)
                .IsUnicode(false);

            modelBuilder.Entity<ModulosMCA>()
                .HasMany(e => e.AcuerdoSupMCA)
                .WithRequired(e => e.ModulosMCA)
                .HasForeignKey(e => e.moduloMACId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ModulosMCA>()
                .HasMany(e => e.Cuentas)
                .WithRequired(e => e.ModulosMCA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ModulosMCA>()
                .HasMany(e => e.UsuarioModulosMCA)
                .WithRequired(e => e.ModulosMCA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Municipios>()
                .Property(e => e.municipio)
                .IsUnicode(false);

            modelBuilder.Entity<Proyectos>()
                .Property(e => e.nombreProyecto)
                .IsUnicode(false);

            modelBuilder.Entity<Proyectos>()
                .HasMany(e => e.ModulosMCA)
                .WithRequired(e => e.Proyectos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReversionCierres>()
                .Property(e => e.razonReversion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoCierres>()
                .Property(e => e.tipoCierre)
                .IsUnicode(false);

            modelBuilder.Entity<TipoCierres>()
                .HasMany(e => e.Cierres)
                .WithRequired(e => e.TipoCierres)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoComprobantes>()
                .Property(e => e.tipoComprobante)
                .IsUnicode(false);

            modelBuilder.Entity<TipoComprobantes>()
                .Property(e => e.nomeclatura)
                .IsUnicode(false);

            modelBuilder.Entity<TipoComprobantes>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoComprobantes>()
                .HasMany(e => e.Comprobantes)
                .WithRequired(e => e.TipoComprobantes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoCuentas>()
                .Property(e => e.tipoCuenta)
                .IsUnicode(false);

            modelBuilder.Entity<TipoCuentas>()
                .HasMany(e => e.Cuentas)
                .WithRequired(e => e.TipoCuentas)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoExtensiones>()
                .Property(e => e.tipoExtencion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoExtensiones>()
                .HasMany(e => e.AcuerdoSupMCA)
                .WithRequired(e => e.TipoExtensiones)
                .HasForeignKey(e => e.tipoExtencionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoUsuarios>()
                .Property(e => e.tipoUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<TipoUsuarios>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoUsuarios>()
                .HasMany(e => e.UsuarioModulosMCA)
                .WithRequired(e => e.TipoUsuarios)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ucrs>()
                .Property(e => e.ucr)
                .IsUnicode(false);

            modelBuilder.Entity<Ucrs>()
                .Property(e => e.organismo)
                .IsUnicode(false);

            modelBuilder.Entity<Ucrs>()
                .HasMany(e => e.Proyectos)
                .WithRequired(e => e.Ucrs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.nombres)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.apellidos)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.cedula)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.telefonos)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.UsuarioModulosMCA)
                .WithRequired(e => e.Usuarios)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReinicioCheques>()
              .Property(e => e.usuario)
              .IsUnicode(false);

            modelBuilder.Entity<ReinicioCheques>()
                .Property(e => e.justificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Cuentas>().HasOptional(e => e.padre).WithMany(e => e.hijos).HasForeignKey(e => e.jerarquia).WillCascadeOnDelete(false);
        }
    }
}
