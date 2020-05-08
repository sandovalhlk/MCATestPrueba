

/*
***************************************************************************************************
*Inicio                             Variables Globales
***************************************************************************************************/

var comprobanteId = [];
var conciliacionId;

///**********************************************************************************************************************************************
// * Función General
// **********************************************************************************************************************************************/
$(document).ready(function () {
    //Existecuenta();
    //fnGuardarAcuerdo();
    // var ucrRet = ucrIdR
    fnDataTables();
    sumar(comp, estaChequeado);
    calcular_saldo();


});


/**********************************************************************************************************************************************
 * Función  para llamar pantalla conciliaciones
 **********************************************************************************************************************************************/
function fnconciliaciones(cuentaId) {
    //location.href = $('input[name="pathRoot"]').val() + "/EstadosFinancieros/ConciliacionBancaria/DetalleConciliacion?moduloMCAId=" + moduloMCAId;
    location.href = $('input[name="pathRoot"]').val() + "/EstadosFinancieros/ConciliacionBancaria/Conciliacion?cuentaId=" + cuentaId;
    return false;
}

/**********************************************************************************************************************************************
 * Función de llamado a Vista parcial de los Listado de MCA- conciliación bancaria
 **********************************************************************************************************************************************/
function GetListMCA() {

    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), ucrId: $("#ucrId").val() };
    $.post($('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/GetListadoMCA', info, function (result) {
        $('#ViewGeneral').fadeOut(1);
        $('#divMca').html('');
        $('#divMca').html(result);
        fnDataTables();
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });

    return true;
}

/**********************************************************************************************************************************************
 * Función validar cuenta
 **********************************************************************************************************************************************/
function Existecuenta() {

    var cuentaId = $.trim($('#cuentaId').val());

    if (cuentaId != 0) {

        // location.href = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/Index';
        return false;
    }
    else {
        setTimeout(function () {
            swal("", "No existe una Cuenta de Banco registrada, por favor ingrese la Cuenta de Banco especifica.", "warning")
        }, 1500);

        location.href = $('input[name="pathRoot"]').val() + '/Home/Index';
        return false;
    }

}

/**********************************************************************************************************************************************
 * Función validar cuenta si hay cierre
 **********************************************************************************************************************************************/
function validarCuenta() {
    var cuentaId = $('#cuentaId').val();
    var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/validarCuenta';

    $.getJSON(url, { cuentaId: cuentaId }, function (data) {
        if ($.trim(data) != "") {
            swal("", data, "warning");
            return false;
        }
        else {           
            location.href = $('input[name="pathRoot"]').val() + "/EstadosFinancieros/ConciliacionBancaria/DetalleConciliacion?cuentaId=" + cuentaId;           
        }
    });
    return false;
}

/**********************************************************************************************************************************************
 * Función de llamado a Vista parcial de los Listado de MCA- conciliación bancaria
 **********************************************************************************************************************************************/
function GetListMCA() {

    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), ucrId: $("#ucrId").val() };
    $.post($('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/GetListadoMCA', info, function (result) {
        $('#ViewGeneral').fadeOut(1);
        $('#divMca').html('');
        $('#divMca').html(result);
        fnDataTables();
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });

    return true;
}

///**********************************************************************************************************************************************
// * Función  Generar tabla
// **********************************************************************************************************************************************/
function creartabla() {
    if ($('#fechaTransaccion').val() == 0 || $('#cuentaId').val().length == 0) {
        swal('', 'Ingrese el Periodo de la Transacción', 'error');
        return false;
    }

    document.getElementById("Formtable").style.display = "block";
    //document.getElementById("divNuevo").style.display = "block";

    var fechaTransaccion = $.trim($('#fechaTransaccion').val());
    var cuentaId = $.trim($('#cuentaId').val());
    var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/GetListadoCheques';
    $.post(url, { fechaTransaccion: fechaTransaccion, cuentaId: cuentaId }, function (result) {
        data = result;
        if (result != 0) {
            var d = '<tr>' +
                        '<th> </th>' +
                        '<th>N° Cheque</th>' +
                        '<th>Beneficiario</th>' +
                        '<th>Cantidad</th>' +
                        '</tr>';
            $.each(result, function (i, item) {


                d += '<tr>' +


               //'<td>' + '<input type="checkbox"  id= "md_checkbox_' + item.comprobanteId + '" class="filled-in chk-col-indigo" value="" onclick=" if (this.checked) sumar(' + item.credito + '); else restar(' + item.credito + ') "/> <label for="md_checkbox_' + item.comprobanteId + '" id="lbl_md_checkbox_' + item.comprobanteId + '"></label>' + '</td>' +
                '<td>' + '<input type="checkbox"  id= "md_checkbox_' + item.comprobanteId + '" class="filled-in chk-col-indigo" value="' + item.comprobanteId + '" onclick="sumar(' + item.cantidad + ',this.checked)"/> <label for="md_checkbox_' + item.comprobanteId + '" id="lbl_md_checkbox_' + item.comprobanteId + '"></label>' + '</td>' +
               '<td>' + item.numCheque + '</td>' +
               '<td>' + item.beneficiario + '</td>' +
               '<td id = "cantidad">' + item.cantidad + '</td>' +
               '</tr>';


                $("#Cheques").empty('');

                $("#Cheques").append(d);

                sumar(item.cantidad, false);
            });

        }

        sumar(0, false);

    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });

    return false;
}

///**********************************************************************************************************************************************
// * Fechas 
// **********************************************************************************************************************************************/
//-al utilizar la function datapicker actualiza el formato a mm-dd-yy por lo cual hay que ponerle el dateFormat:"dd-mm-yy"
$(function () {
    $("#fechaTransaccion").datepicker({ dateFormat: "dd-mm-yy" })
        .on("change", function () {
            //console.log("Date changed: ", e.target.value);
            var fechaTransaccion = $.trim($('#fechaTransaccion').val());
            $('#fecha').text(fechaTransaccion);
            $('#fecha1').text(fechaTransaccion);
            //$('#fecha').val(fechaTransaccion);
            //$('#fecha1').val(fechaTransaccion);

        });
});


/**********************************************************************************************************************************************
 * Función Next revision total
 **********************************************************************************************************************************************/
function DetalleTramite() {
    // obtener los valores seleccionados de la tabla   
    //var comprobanteId = new Array();//inicializando el array   
    //$("input:checkbox:checked").each(function () {
    //    if (this.id != "menosPartidaConc") {
    //        comprobanteId.push($(this).val());          
    //    }
    //    ///console.log(comprobanteId);
    //});

    if ($('#fechaTransaccion').val() == 0 || $('#monto').val() == 0 ) {
        swal('', 'Todos los Datos son requeridos', 'error');
        return false;
    }


    //obtengo los cheques no conciliados 
    var comprobanteId = new Array(); //inicializando el array   
    $("input:checkbox:not(:checked)").each(function () {
        if (this.id != "menosPartidaConc") {
            comprobanteId.push($(this).val());
        }
    });


    //if (comprobanteId.length > 0) {
        var proyecto = $.trim($('#proy').val());
        var fechaTransaccionD = $.trim($('#fechaTransaccion').val());
        var estadoBanco = $.trim($('#monto').val());
        var chequesFlotantes = $.trim($('#suma').val());
        var pconciliacion = $.trim($('#partidaConsiliacion').val());
        var saldoenLibro = $.trim($('#Slibro').val());

        // Activa tab.
        $("#tabConciliacion").removeClass("active");
        $("#tabrevision").addClass("active");
        // Cambio de vista centro.
        $("#Conciliacion").removeClass("tab-pane active");
        $("#Conciliacion").addClass("tab-pane");
        $("#revision").removeClass("tab-pane");
        $("#revision").addClass("tab-pane active");



        console.log(comprobanteId);
        //llenado del Detalle

        $('#fconciliacion').text(fechaTransaccionD);
        $('#sbanco').text(estadoBanco);
        $('#chequesF').text(chequesFlotantes);
        $('#pconciliacion').text(pconciliacion);
        $('#saldoL').text(saldoenLibro);


    //llenado de la tabla DETALLE DE CHEQUES///////////   
    if (comprobanteId.length > 0) {
        var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/GetListadoChequesDetalles';
        var parameter = {
            comprobanteId: comprobanteId
        };

        $.post(url, parameter, function (data) {
            //data = result;
            var d = '<tr>' +
                        '<th>N° Cheque</th>' +
                        '<th>Beneficiario</th>' +
                        '<th>Cantidad</th>' +
                        '</tr>';
            for (var i = 0; i < data.toListadoCheques.length; i++) {
                d += '<tr>' +
                     '<td>' + data.toListadoCheques[i].numCheque + '</td>' +
                     '<td>' + data.toListadoCheques[i].beneficiario + '</td>' +
                     '<td>' + data.toListadoCheques[i].cantidad + '</td>' +
                     '</tr>';
            }

            $("#chequesSeleccionados").empty('');
            $("#chequesSeleccionados").append(d);

        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
    }
    //else {
    //    swal("", "Seleccione al menos un cheque.", "warning");
    //}
}


/**********************************************************************************************************************************************
 * Función  para editar Conciliacion
 **********************************************************************************************************************************************/
function fnSetEditConciliacion(conciliacionBancariaId) {
    var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/ValidarEdicion';

    $.getJSON(url, { conciliacionBancariaId: conciliacionBancariaId }, function (data) {
        if ($.trim(data) != "") {
            swal("", data , "warning");
            return false;
        }
        else {
            location.href = $('input[name="pathRoot"]').val() + "/EstadosFinancieros/ConciliacionBancaria/EditConciliacion?conciliacionBancariaId=" + conciliacionBancariaId;
        }
    });
    return false;
}

/**********************************************************************************************************************************************
 * Función Registrar Conciliacion
 **********************************************************************************************************************************************/

function RegistrarConciliacion() {


    var fechaTransaccion = $.trim($('#fechaTransaccion').val());
    var CuentaBanco = parseFloat(deleteComas($.trim($('#monto').val())));
    var PartidaConciliacion = parseFloat(deleteComas($.trim($('#partidaConsiliacion').val())));
    var cuenta = $.trim($('#cuentaId').val());
    var saldoLibro = parseFloat(deleteComas($.trim($('#Slibro').val())));
    //var banco = $.trim($('#bancoId').val());

    //monto: deleteComas($.trim($('#monto').val())),
    // comprobantes conciliados
    var comprobanteConciliadoId = new Array(); //inicializando el array   
    $("input:checkbox:checked").each(function () {
        if (this.id != "menosPartidaConc") {
            comprobanteConciliadoId.push($(this).val());
        }
    });

    //obtengo cheques flotantes
    var comprobanteId = new Array(); //inicializando el array   
    $("input:checkbox:not(:checked)").each(function () {
        if (this.id != "menosPartidaConc") {
            comprobanteId.push($(this).val());
        }
    });

    var comprobantes = {
        "fechaTransaccion": fechaTransaccion,
        "saldoEstadoCuentaBanco": CuentaBanco,
        "partidaConsiliacion": PartidaConciliacion,
        "cuentaId": cuenta,
        "saldoenLibro": saldoLibro
        //"bancoId": banco
    }

    var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/SaveConciliacion';
    var parameter = {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
        conciliacionBancariaId: ($('input[name="conciliacionBancariaId"]').val() == '' ? 0 : $('input[name="conciliacionBancariaId"]').val()),
        toConciliacionBancaria: comprobantes,
        comprobanteConciliadoId: comprobanteConciliadoId,
        comprobanteId: comprobanteId

    };
    $.post(url, parameter, function (data) {
        if ($.trim(data.mensaje) != "") {
            swal("", data.mensaje, "warning");
            return false;
        }
        //document.getElementById('btn_RegistrarCita').disabled = true;
        document.getElementById("btn_RegistrarCita").style.display = "none";
        document.getElementById("btn_regresar").style.display = "none";
        document.getElementById("print").style.display = "block";
        document.getElementById("btn_regresar1").style.display = "block";      
        conciliacionId = data.conciliacionBId;       
        swal("", "La Conciliación Bancaria fue Registrada Exitosamente.", "success");   
        //fnOpenModal('#ModalTipoProyecto', false, false)
        //GetListaModulos.Refresh();


    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
    return false;

}
/**********************************************************************************************************************************************
 * Función imprimir reporte- desde el guardar
 **********************************************************************************************************************************************/
function fnSetImprimirInsert() {      
    location.href = $('input[name="pathRoot"]').val() + "/Informes/TipoReportes/GenerarConciliacion?conciliacionBancariaId=" + conciliacionId;
      document.getElementById("btnEjecutar").style.display = "block";
  
    return false;
}




/**********************************************************************************************************************************************
 * Función ejecutar conciliacion
 **********************************************************************************************************************************************/
function fnEjecutarConciliacion() {
    //obtengo los cheques no conciliados 
    var comprobanteNcId = new Array(); //inicializando el array   
    $("input:checkbox:not(:checked)").each(function () {
        if (this.id != "menosPartidaConc") {
            comprobanteNcId.push($(this).val());
        }
    });

    var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/EjecutarConciliacion';
    var parameter = {
        comprobanteNcId: comprobanteNcId,
        conciliacionId: conciliacionId

    };
    $.post(url, parameter, function (data) {
        if ($.trim(data.mensaje) != "") {
            swal("", data.mensaje, "warning");
            return false;
        }

        swal("", "La Conciliación Bancaria ha sido Ejecutada.", "success");
        location.href = $('input[name="pathRoot"]').val() + "/EstadosFinancieros/ConciliacionBancaria/Index"
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });

    return false;

}

