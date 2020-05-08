/***********************************************************************************************
* Simulacion de la clase string de punto net
*
* Funciones basicas para tratos con string 
*
* Author: Juan Carlos Aleman Mayorga  - 2015
***********************************************************************************************/


/* Formatear un valor de numeros a 2 decimales y comas por decimos
*  Example: 
*  console.info(currencyFormat(2665));   // 2,665.00
*  console.info(currencyFormat(102665)); // 102,665.00
*/
function currencyFormat(num) {
    num = num.toString().replace('%', '').replace(new RegExp(",", "g"), "");
    return parseFloat(num).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
}

/* Formatear un valor de numeros a 2 decimales y comas por decimos
*  Example: 
*  console.info(currencyFormat(2665));   // 2,665
*  console.info(currencyFormat(102665)); // 102,665
*/
function currencyFormatInt(num) {
    num = num.toString().replace('%', '').replace(new RegExp(",", "g"), "");
    return parseFloat(num).toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
}

/* Formatear un valor de numeros a 2 decimales y comas 
*  Example: 
*  console.info(currencyFormatDE(1234567.89)); // output 1.234.567,89
*/
function currencyFormatDE(num) {
    num = num.toString().replace('%', '').replace(new RegExp(",", "g"), "");
    return parseFloat(num)
       .toFixed(2) // always two decimal digits
       .replace(".", ",") // replace decimal point character with ,
       .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.")  // use . as a separator
}

/* Borrar las comas para de un string numerico 
*  Example: 5,471,208.50  // 5471208.50
*/
function deleteComas(valueInput) {
   return   valueInput.toString().replace(new RegExp(",", "g"), "");
}

/* Borrar las comas y signos de interrogacion para de un string numerico 
*  Example: 1,508.50%  // 1508.50
*/
function deleteComasAndSignosAdmiracion(valueInput) {
    return valueInput.toString().replace('%', '').replace(new RegExp(",", "g"), "");
}

/* Dar formato de fecha  de string a datetime para json data
*  Example: 
*  console.info(getFecha(10/12/2015)); // 
*/
function getFecha(input) {
    var temp1 = "";
    var dt1 = input.substring(0, 2);
    var mon1 = input.substring(3, 5);
    var yr1 = input.substring(6, 10);

    temp1 = mon1 + "/" + dt1 + "/" + yr1;
    var cfd = Date.parse(temp1);
    var date1 = new Date(cfd);
    return date1;
}

/* Dar formato de fecha  dd/MM/yyyy  de json datetime a strgin en con su mascara
*/
function formatoFecha(old_fecha) {
    var date_fechaInicio = new Date(parseInt(old_fecha.substr(6)));

    var dia_fechaInicio = (date_fechaInicio.getDate() < 10 ? '0' + date_fechaInicio.getDate() : date_fechaInicio.getDate());
    var mes_fechaInicio = (date_fechaInicio.getMonth() + 1);
    mes_fechaInicio = (mes_fechaInicio < 10 ? '0' + mes_fechaInicio : mes_fechaInicio);

    var fechaInicio = dia_fechaInicio + '/' + mes_fechaInicio + '/' + date_fechaInicio.getFullYear();
    return fechaInicio;
}

/* Validar texto si tiene formato de correo electronico, usando expresiones regulares
*  /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3,4})+$/
*  /^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$/
*  Si el dato escrito no es formato de correo devuelve verdadero, caso contrario falso
*/
function validarEmail(email) {
   var  expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
   if (!expr.test(email))
       return false;
   return true;
}


