using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Runtime.Serialization;

namespace DataLayerMCA
{
    public partial class ContabilidadMCA
    {
        private Auditoria auditFactory;
        private List<Auditoria> auditList = new List<Auditoria>();
        private List<DbEntityEntry> objectList = new List<DbEntityEntry>();


        /*** SobreEscritura del evento Guardar, generado por Entity Framework ***/
        public override int SaveChanges()
        {
            /** Limpiar Contenido de Registros temporales de auditoria **/
            auditList.Clear();
            objectList.Clear();
            auditFactory = new Auditoria(this);

            /** Obtener las entidades que son afectadas por algun ejecucion CRUD **/
            var entityList = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified);
            foreach (var entity in entityList)
            {
                Auditoria audit = auditFactory.GetAudit(entity);
                bool isValid = true;
                if (entity.State == EntityState.Modified && string.IsNullOrWhiteSpace(audit.valoresNuevo) && string.IsNullOrWhiteSpace(audit.valoresAnterior))
                    isValid = false;

                if (isValid)
                {
                    auditList.Add(audit);
                    objectList.Add(entity);
                }
            }

            /** Ejecutar proceso de Guardar Cambios por parte del EF **/
            var retVal = base.SaveChanges();
            if (auditList.Count > 0)
            {
                /** Guardar Afectaciones por CRUD en la tabla de Auditoria **/
                int i = 0;
                foreach (var audit in auditList)
                {
                    if (audit.operacion == Auditoria.AuditActions.I.ToString())
                        audit.valorKeyEntidad = auditFactory.GetKeyValue(objectList[i]);

                    Auditoria.Add(audit);
                    i++;
                }
                base.SaveChanges();
            }

            return retVal;
        }
    }
}
