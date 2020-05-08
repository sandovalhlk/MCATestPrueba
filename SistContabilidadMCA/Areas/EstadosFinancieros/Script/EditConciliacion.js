
/*
***************************************************************************************************
*Inicio                             Variables Globales
***************************************************************************************************/

var comprobanteIdNS = [];
var sum = 0;
var total = 0;


///**********************************************************************************************************************************************
// * Función General
// **********************************************************************************************************************************************/
$(document).ready(function () {
    ListCheques();
});


///**********************************************************************************************************************************************
// * Función  Generar tabla
// **********************************************************************************************************************************************/
function ListCheques() {

    var conciliacionBancariaId = $.trim($('#conciliacionBanId').val());
    var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/GetListadoChequesEdit';
    $.post(url, { conciliacionBancariaId: conciliacionBancariaId }, function (result) {
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


            //'<td>' + '<input type="checkbox"  id= "md_checkbox_' + item.comprobanteId + '" class="filled-in chk-col-indigo" value="' + item.comprobanteId + '" onclick="sumarEdit(' + item.cantidad + ',this.checked)" checked/> <label for="md_checkbox_' + item.comprobanteId + '" id="lbl_md_checkbox_' + item.comprobanteId + '"></label>' + '</td>' +
             '<td>' + '<input type="checkbox"  id= "md_checkbox_' + item.comprobanteId + '" class="filled-in chk-col-indigo" value="' + item.comprobanteId + '" onclick="sumar(' + item.cantidad + ',this.checked)" /> <label for="md_checkbox_' + item.comprobanteId + '" id="lbl_md_checkbox_' + item.comprobanteId + '"></label>' + '</td>' +
           '<td>' + item.numCheque + '</td>' +
           '<td>' + item.beneficiario + '</td>' +
           '<td id = "cantidad">' +
           Number(parseFloat(item.cantidad).toFixed(2)).toLocaleString('en', {
               minimumFractionDigits: 2
           });
           + '</td>' +
           '</tr>';


            $("#Cheques").empty('');

            $("#Cheques").append(d);

            sumar(item.cantidad, false);

        });
        }

        sumar(0, false);

        var saldoBanco = eval(deleteComas($.trim($('#monto').val())));
        var chequesFlotante = eval($.trim($('#suma').val()));
        var partidaConciliacion = eval(deleteComas($.trim($('#partidaConsiliacion').val())));


        var saldo = saldoBanco - chequesFlotante + partidaConciliacion;
        var saldoLibroBD = eval(deleteComas($.trim($('#saldoBD').val())));
        if (saldo > saldoLibroBD) {
            $("#menosPartidaConc").prop("checked", true);
            calcular_saldo();
        }


    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    return false;
}

/**********************************************************************************************************************************************
 * Función Next revision total
 **********************************************************************************************************************************************/

function DetalleTramiteE() {
    // obtener los valores seleccionados de la tabla   
    //var comprobanteId = new Array();//inicializando el array   
    //$("input:checkbox:checked").each(function () {
    //    if (this.id != "menosPartidaConc") {
    //        comprobanteId.push($(this).val());
    //    }
    //    ///console.log(comprobanteId);
    //});

    //obtengo los cheques seleccionados que no corresponden a la conciliacion

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

    //llenado de la tabla DETALLE DE CHEQUES////////////
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
 * Función Registrar Conciliacion
 **********************************************************************************************************************************************/

function RegistrarConciliacionD() {


    var fechaTransaccion = $.trim($('#fechaTransaccion').val());
    var CuentaBanco = parseFloat(deleteComas($.trim($('#monto').val())));
    var PartidaConciliacion = parseFloat(deleteComas($.trim($('#partidaConsiliacion').val())));
    var cuenta = $.trim($('#cuentaId').val());
    var banco = $.trim($('#bancoId').val());
    var saldoLibro = parseFloat(deleteComas($.trim($('#Slibro').val())));
    var conciliacionBancariaId = $.trim($('#conciliacionBanId').val());
    //monto: deleteComas($.trim($('#monto').val())),




    //obtengo lo seleccionado
    var comprobanteeditId = new Array(); //inicializando el array   
    $("input:checkbox:checked").each(function () {
        if (this.id != "menosPartidaConc") {
            comprobanteeditId.push($(this).val());
        }
    });

    var comprobantes = {
        //"fechaTransaccion": fechaTransaccion,
        "saldoEstadoCuentaBanco": CuentaBanco,
        "partidaConsiliacion": PartidaConciliacion,
        "cuentaId": cuenta,
        "saldoenLibro": saldoLibro,
        "bancoId": banco,
        "conciliacionBancariaId": conciliacionBancariaId
    }

    var url = $('input[name="pathRoot"]').val() + '/EstadosFinancieros/ConciliacionBancaria/SaveConciliacionEdit';
    var parameter = {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
        conciliacionBancariaId: ($('input[name="conciliacionBancariaId"]').val() == '' ? 0 : $('input[name="conciliacionBancariaId"]').val()),
        toConciliacionBancaria: comprobantes,
        comprobanteeditId: comprobanteeditId
    };
    $.post(url, parameter, function (data) {
        if ($.trim(data.mensaje) == "") {
            document.getElementById("btn_RegistrarCita").style.display = "none";
            document.getElementById("btn_atras1").style.display = "none";
            document.getElementById("print1").style.display = "block";
            document.getElementById("btn_regresar2").style.display = "block";           
            swal("", "La Conciliación Bancaria fue Actualizada Exitosamente.", "success");
            //location.href = $('input[name="pathRoot"]').val() + "/EstadosFinancieros/ConciliacionBancaria/Index"         
            return false;
        }
       



    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
    return false;

}




/**********************************************************************************************************************************************
 * Función ejecutar conciliacion
 **********************************************************************************************************************************************/
function fnEjecutarConciliacionEdit() {
    var conciliacionBancariaId = $.trim($('#conciliacionBanId').val());
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
        conciliacionId: conciliacionBancariaId

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




