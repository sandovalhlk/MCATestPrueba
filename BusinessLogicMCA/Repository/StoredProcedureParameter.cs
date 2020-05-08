using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Clase que maneja las variables y su respectivo valor
    /// </summary>
    /// <remarks></remarks>
    class StoredProcedureParameter
    {
        private string parameter;
        private object value;

        /// <summary>
        ///  Nombre de la variable, debe ser igual a la declarada en el procedimiento almacenado
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        /// <summary>
        /// Valor de la variable, puede ser de cualquier tipo de dato. preferible que  coincida con las variables declaradas en GetTypeProperty
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        ///   Constructor para la inicializacion de la variable.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public StoredProcedureParameter(string parameter, object value)
        {
            try
            {
                this.Parameter = parameter;
                this.Value = value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la creacion del Parametro \n " + ex.Message);
            }
        }

        /// <summary>
        ///  Se definen los posibles tipos de datos que se le van a enviar al procedimiento almacenado
        /// Esta lista podria aumentar conforme se usen otro tipo de variable.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SqlDbType GetTypeProperty
        {
            get
            {
                if (value.GetType().FullName == "System.String;")
                {
                    return SqlDbType.NVarChar;
                }
                else if (value.GetType().FullName == "System.Int16;")
                {
                    return SqlDbType.Int;
                }
                else if (value.GetType().FullName == "System.Int32;")
                {
                    return SqlDbType.Int;
                }
                else if (value.GetType().FullName == "System.Int64;")
                {
                    return SqlDbType.BigInt;
                }
                else if (value.GetType().FullName == "System.Integer;")
                {
                    return SqlDbType.Int;
                }
                else if (value.GetType().FullName == "System.Decimal;")
                {
                    return SqlDbType.Decimal;
                }
                else if (value.GetType().FullName == "System.Double;")
                {
                    return SqlDbType.Float;
                }
                else if (value.GetType().FullName == "System.DateTime;")
                {
                    return SqlDbType.DateTime;
                }
                else if (value.GetType().FullName == "System.Byte;")
                {
                    return SqlDbType.Image;
                }

                return SqlDbType.NVarChar;
            }
        }
    }
}
