using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistContabilidadMCA
{
    public class AlertType
    {
        /// <summary>
        /// Mensaje a presentar al usuario
        /// </summary>
        public string mensaje { get; set; }
        /// <summary>
        /// Lista de Mensajes a mandar a presentar al usuario
        /// </summary>
        public List<string> itemsLista { get; set; }
        /// <summary>
        /// Definir la presentacion de alertar ,(aviso, información,exito, peligro)
        /// </summary>
        public TypeAlert type { get; set; }
        /// <summary>
        /// Si se desea a mandar a presentar el icono segun el tipo de presentación de alerta
        /// </summary>
        public bool showIcon { get; set; }
        /// <summary>
        /// Si en vez de mandar un alert de HTML se presentara creara un bloque de Javascrip segun parametro de mensaje
        /// </summary>
        public bool showBlockJS { get; set; }
        /// <summary>
        /// Si se desea mostrar el alert de el plugin Swall
        /// </summary>
        public bool showAlertJS { get; set; }
        /// <summary>
        /// Enum de clasificación de tipos de alerta, para mejor ayuda para el programador
        /// </summary>
        public enum TypeAlert
        {
            success,
            info,
            warning,
            danger
        }
    }
}