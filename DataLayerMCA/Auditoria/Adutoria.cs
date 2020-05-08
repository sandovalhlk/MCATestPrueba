using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayerMCA
{   
    public partial class Auditoria
    {
        #region Propiedades Tabla Auditoria

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

        #endregion

        private ContabilidadMCA context;

        public  enum AuditActions
        {
            I,
            U,
            D
        }
        #region Constructores de la Clase

            public Auditoria()
            {
                this.context = new ContabilidadMCA();
            }

            public Auditoria(ContabilidadMCA context)
            {
                this.context = context;
            }

        #endregion

        #region Metodos

            /// <summary>
            /// Obtener datos de Auditoria por Entidad de parametro
            /// </summary>
            /// <param name="entry"></param>
            /// <returns></returns>
            public Auditoria GetAudit(DbEntityEntry entry)
            {
                Auditoria audit = new Auditoria();
                audit.fecha = DateTime.Now;            
                audit.usuarioName = HttpContext.Current.User.Identity.Name; //Change this line according to your needs
                audit.usuarioId = (HttpContext.Current.User.Identity.GetUserId() != null ? HttpContext.Current.User.Identity.GetUserId() : "");
                audit.entidad = GetTableName(entry);
                string tabla = GetTableName(entry);
                if (tabla != "AspNetUsers_C264157C17F45F6DCF2A7A202E0192BCFAFE6B2F490EDF5BD90A23843C685F38")
                {
                    audit.valorKeyEntidad = GetKeyValue(entry);
                }
                else
                    audit.valorKeyEntidad = 250;

                audit.maquinaIp = GetIP4Address(); // HttpContext.Current.Request.UserHostAddress;
                           
                //entry is Added 
                if (entry.State == EntityState.Added)
                {
                    var newValues = new StringBuilder();
                    SetAddedProperties(entry, newValues);
                    audit.valoresNuevo = newValues.ToString();
                    audit.operacion = AuditActions.I.ToString();
                }
                //entry in deleted
                else if (entry.State == EntityState.Deleted)
                {
                    var oldValues = new StringBuilder();
                    SetDeletedProperties(entry, oldValues);
                    audit.valoresAnterior = oldValues.ToString();
                    audit.operacion = AuditActions.D.ToString();
                }
                //entry is modified
                else if (entry.State == EntityState.Modified)
                {
                    var oldValues = new StringBuilder();
                    var newValues = new StringBuilder();
                    SetModifiedProperties(entry, oldValues, newValues);
                    audit.valoresAnterior = oldValues.ToString();
                    audit.valoresNuevo = newValues.ToString();
                    audit.operacion = AuditActions.U.ToString();
                }

                return audit;
            }

            /// <summary>
            /// Esablecer las propiedades afectadas en un Insert
            /// </summary>
            /// <param name="entry"></param>
            /// <param name="newData"></param>
            private void SetAddedProperties(DbEntityEntry entry, StringBuilder newData)
            {
                foreach (var propertyName in entry.CurrentValues.PropertyNames)
                {
                    var newVal = entry.CurrentValues[propertyName];
                    if (newVal != null)
                    {
                        newData.AppendFormat("{0}={1} || ", propertyName, newVal);
                    }
                }
                if (newData.Length > 0)
                    newData = newData.Remove(newData.Length - 3, 3);
            }

            /// <summary>
            /// Esablecer las propiedades afectadas en un Delete
            /// </summary>
            /// <param name="entry"></param>
            /// <param name="oldData"></param>
            private void SetDeletedProperties(DbEntityEntry entry, StringBuilder oldData)
            {
                DbPropertyValues dbValues = entry.GetDatabaseValues();
                foreach (var propertyName in dbValues.PropertyNames)
                {
                    var oldVal = dbValues[propertyName];
                    if (oldVal != null)
                    {
                        oldData.AppendFormat("{0}={1} || ", propertyName, oldVal);
                    }
                }
                if (oldData.Length > 0)
                    oldData = oldData.Remove(oldData.Length - 3, 3);
            }

            /// <summary>
            /// Establecer las propiedades afectadas en un Update
            /// </summary>
            /// <param name="entry"></param>
            /// <param name="oldData"></param>
            /// <param name="newData"></param>
            private void SetModifiedProperties(DbEntityEntry entry, StringBuilder oldData, StringBuilder newData)
            {
                DbPropertyValues dbValues = entry.GetDatabaseValues();
                foreach (var propertyName in entry.OriginalValues.PropertyNames)
                {
                    var oldVal = dbValues[propertyName];
                    var newVal = entry.CurrentValues[propertyName];
                    if (oldVal != null && newVal != null && !Equals(oldVal, newVal))
                    {
                        newData.AppendFormat("{0}={1} || ", propertyName, newVal);
                        oldData.AppendFormat("{0}={1} || ", propertyName, oldVal);
                    }
                }
                if (oldData.Length > 0)
                    oldData = oldData.Remove(oldData.Length - 3, 3);
                if (newData.Length > 0)
                    newData = newData.Remove(newData.Length - 3, 3);
            }

            /// <summary>
            /// Obtener valor de llave primaria si lo tiene del Objeto entity mapeado de tabla
            /// </summary>
            /// <param name="entry"></param>
            /// <returns></returns>
            public long? GetKeyValue(DbEntityEntry entry)
            {
                var objectStateEntry = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
                long id = 0;
                if (objectStateEntry.EntityKey.EntityKeyValues != null)
                //id = Convert.ToInt64(objectStateEntry.EntityKey.EntityKeyValues[0].Value);
                id = Convert.ToInt64(objectStateEntry.EntityKey.EntityKeyValues[0].Value);
            return id;
            }

            /// <summary>
            /// Obtener el nombre de la tabla entidad mapeada del entity
            /// </summary>
            /// <param name="dbEntry"></param>
            /// <returns></returns>
            private string GetTableName(DbEntityEntry dbEntry)
            {
                TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
                //string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().BaseType.FullName; // + dbEntry.Entity.GetType().BaseType.Name;
                //string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().BaseType.Name;
                return tableName;
            }

            /// <summary>
            /// Obtener Direccion IPV4 , local o remotamente del equipo
            /// </summary>
            /// <returns></returns>
            public string GetIP4Address()
            {
                string IP4Address = String.Empty;

            //    if(HttpContext.Current.Request.UserHostAddress == "::1")
            //    {
            //        foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            //        {
            //            if (IPA.AddressFamily.ToString() == "InterNetwork")
            //            {
            //                IP4Address = IPA.ToString();
            //                break;
            //            }
            //        }

            //        if (IP4Address != String.Empty)
            //        {
            //            return IP4Address;
            //        }

            //        foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            //        {
            //            if (IPA.AddressFamily.ToString() == "InterNetwork")
            //            {
            //                IP4Address = IPA.ToString();
            //                break;
            //            }
            //        }
            //    }
            //else
            //{
                // Invoker
                IP4Address = IPHelper.GetIPAddress(HttpContext.Current.Request.ServerVariables["HTTP_VIA"],
                                                                HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                                                                HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
          //  }
                //   IP4Address = HttpContext.Current.Request.UserHostAddress;

              return IP4Address;
            }

         

            /// <summary>
            /// Agregar o Salar regisro de Auditoria
            /// </summary>
            /// <param name="toObj"></param>
            /// <returns></returns>
            public static Auditoria Add(Auditoria toObj)
            {

            if (toObj.valoresAnterior != null)
            {
                toObj.valoresAnterior = toObj.valoresAnterior.Replace("&", "Y").Replace("'", " ");
            }

            if (toObj.valoresNuevo != null)
            {
                toObj.valoresNuevo = toObj.valoresNuevo.Replace("&", "Y").Replace("'", " ");
            }


            string query = string.Format("IF (SELECT OBJECT_ID('dbo.Auditoria') ) IS NOT  NULL	BEGIN "
                  +  "INSERT INTO [Auditoria] ([fecha],[entidad],[valorKeyEntidad]"
                  + ",[usuarioId]"
                  + ",[usuarioName]"
                  + ",[operacion]"
                  + ",[valoresAnterior]"
                  + ",[valoresNuevo]"
                  + ",[maquinaIp]) VALUES (GETDATE(),'{0}', {1}"             
                  + ",'{2}'"
                  + ",'{3}'"
                  + ",'{4}'"
                  + ",'{5}'"
                  + ",'{6}'"
                  + ",'{7}'" 
                  + ") END", toObj.entidad, (!toObj.valorKeyEntidad.HasValue ? "NULL" : toObj.valorKeyEntidad.Value.ToString()),
                  toObj.usuarioId,
                  toObj.usuarioName,
                  toObj.operacion,
                  toObj.valoresAnterior,
                  toObj.valoresNuevo,
                  toObj.maquinaIp                 
                  );
            var context = new ContabilidadMCA();             
            context.Database.ExecuteSqlCommand(query);
            return toObj;
            }

        #endregion
    }

}

