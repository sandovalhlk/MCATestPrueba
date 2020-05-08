/***************************************************************************************************
*Inicio                             Variables Globales
***************************************************************************************************
*/
//var data = [];
var sum = 0;
var total = 0;


///**********************************************************************************************************************************************
// * Función  Sumar o Restar la columna cantidad de lo seleccionado en la tabla
// **********************************************************************************************************************************************/
function sumar(comp, estaChequeado) {  

    var tipoFormulario = $('#tipoFormulario').val();
    document.getElementById("sumatoria").style.display = "block";
    if (estaChequeado == false) {
        total = total + comp;
    } else {
        total = total - comp;
    }

    //$('#suma').val(total);
    $('#suma').val(Number(parseFloat(total).toFixed(2)).toLocaleString('en', {
        minimumFractionDigits: 2
    }));
    calcular_saldo();


}

///**********************************************************************************************************************************************
// * Función calcular saldo en Libros
// **********************************************************************************************************************************************/

// eval- el valor que recupero del campo input text del formulario es una cadena, la función eval() es para convertirlo en un numerico y poder hacer las operaciones
function calcular_saldo() {

    var saldoBanco = eval(deleteComas($.trim($('#monto').val())));
    var chequesFlotante = eval(deleteComas($.trim($('#suma').val())));
    //var chequesFlotante = eval($.trim($('#suma').val()));
    var partidaConciliacion = eval(parseFloat(deleteComas($.trim($('#partidaConsiliacion').val()))));
    var saldoLibros = 0;
    var menosPartidaConc = document.getElementById("menosPartidaConc");
    if (menosPartidaConc.checked == false) {
        saldoLibros = (saldoBanco - chequesFlotante) + partidaConciliacion
    }
    else {
        saldoLibros = (saldoBanco - chequesFlotante) - partidaConciliacion
    }

    if (isNaN(saldoLibros)) {
        saldoLibros = 0;
    }
    //$('#Slibro').val(saldoLibros);
    $('#Slibro').val(Number(parseFloat(saldoLibros).toFixed(2)).toLocaleString('en', {
        minimumFractionDigits: 2
    }));
    //document.getElementById("sumatoria").style.display = "block";
}

/**********************************************************************************************************************************************
 * Función Boton regreso de Detalle  a Anomalia
 **********************************************************************************************************************************************/
function fnRegreso() {
    // Activa tab.
    $("#tabrevision").removeClass("active");
    $("#tabConciliacion").addClass("active");
    // Cambio de vista centro.
    $("#revision").removeClass("tab-pane active");
    $("#revision").addClass("tab-pane");
    $("#Conciliacion").removeClass("tab-pane");
    $("#Conciliacion").addClass("tab-pane active");
    $("#barProgress").width('5%');
    $('#barProgress').html('0%');
    return false;
}


/**********************************************************************************************************************************************
 * Función Boton regreso a pantalla conciliacion
 **********************************************************************************************************************************************/
function fnRegresoConciliacion() {
    var cuentaId = $.trim($('#cuentaId').val());
    location.href = $('input[name="pathRoot"]').val() + "/EstadosFinancieros/ConciliacionBancaria/Conciliacion?cuentaId=" + cuentaId;
    return false;
}


/**********************************************************************************************************************************************
 * Función imprimir reporte-
 **********************************************************************************************************************************************/
function fnSetImprimir(conciliacionBancariaId) {
    //location.href = $('input[name="pathRoot"]').val() + "Informes/TipoReportes/GenerarComp?tipoComprobanteId=" + $('#tipoComprobanteId').val() + "&comprobanteId=" + $('#comprobanteId').val();
    location.href = $('input[name="pathRoot"]').val() + "/Informes/TipoReportes/GenerarConciliacion?conciliacionBancariaId=" + conciliacionBancariaId;
    document.getElementById("btnEjecutar1").style.display = "block";
    return false;
}

