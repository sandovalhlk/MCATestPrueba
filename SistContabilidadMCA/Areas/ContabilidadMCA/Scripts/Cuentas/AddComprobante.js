var CONTADOR = 0;
var objComp = new Object();
/********************************
*Funcion principal
*****************************/
$(document).ready(function () {

    if ($('#tipoComprobanteId option:selected').text() == "PAGO")
        $('.chequeForm').show();


setTimeout(function () { validarEdicion(); }, 100);

});

$('#tipoComprobanteId').on("change", function () {

    if ($('#tipoComprobanteId option:selected').text() == "PAGO") {
        $('.chequeForm').show();

        if ($('#tipoComprobanteId option:selected').val() == objComp.tipo) {
            $("#numero").val(objComp.numero);
            $("#beneficiario").val(objComp.beneficiario);
            $("#numCheque").val(objComp.numCheque);
            $('#tipoComprobanteId option:selected').val(objComp.tipo);

        } else {

        
        //AQUI SE TIENE QUE MANDAR A LLAMAR UNA FUNCION QUE DEVUELVA EL NUMERO DE COMPROBANTE.
        var info =
                {
                    __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                    comprobanteId: $('#comprobanteId').val(),
                    tipoComprobanteId: 2, 
                    invertTipo:1,

                };
        $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/GetLastNumComprobante', info, function (result) {
            if (result != "") {
              
                $('#numCheque').val(result);
                $('#numero').val(result);

            } else {
                swal("", "El numero de cheque no se pudo calcular / El comprobante a seleccionar debe ser el ultimo ingresado al sistema", "error");
                $('.chequeForm').hide();
                $('#tipoComprobanteId').val(1)
            }

        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
        }
    }

    if ($('#tipoComprobanteId option:selected').text() == "DIARIO") {
        if ($('#tipoComprobanteId option:selected').val() == objComp.tipo) {
            $("#numero").val(objComp.numero);
            $("#beneficiario").val(objComp.beneficiario);
            $("#numCheque").val(objComp.numCheque);
            $('#tipoComprobanteId option:selected').val(objComp.tipo);

        } else {
        var info =
              {
                  __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                  comprobanteId: $('#comprobanteId').val(),
                  tipoComprobanteId: 1, 
                   invertTipo:2,
              };
        $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/GetLastNumComprobante', info, function (result) {
            if (result != "") {

                $('#numero').val('');
                $("#reinicio").val('');
                $("#numCheque").val('');
                $("#beneficiario").val('');
                $('#numero').val(result);

            } else {

                swal("", "El numero de cheque no se pudo calcular / El comprobante a seleccionar debe ser el ultimo ingresado al sistema", "error");
                $('.chequeForm').show();
                $('#tipoComprobanteId').val(2)
            }

        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
        }
    }
});




$('#btnCancelar').on('click', function () {
    location.href = $('input[name="pathRoot"]').val() + "/ContabilidadMCA/Cuentas/Comprobantes/Index"
});

function validarEdicion() {
    //if ($('#estadoId').val() == 3) {
    if ($("#editC").val() == false) {
        //$('.modificacion').attr('readonly', 'readonly');
        //$('.modificacion').attr('disabled', 'disabled');
        $('.modificacion').prop("readonly", true);
        $('.modificacion').prop("disabled", true);
        // $('#btnPrintComp').removeAttr('disabled', 'disabled');
    }
    return false;
}

/********************************
*Eventos click que llaman a funciones
*****************************/
$('#btnReinicioCheque').on('click', function () {
    //fnOpenModal('#modalReiniciarCheques', true, false);
    $('#modalReiniciarCheques').modal('show');
    return false;
});

$('#btnAddMovimiento').on('click', function () {
    fnOpenModal('#modalAddMovimientos', true, false);
    return false;
});


$('#btnPrintComp').on('click', function () {
    location.href = $('input[name="pathRoot"]').val() + "/Informes/TipoReportes/GenerarComp?tipoComprobanteId=" + $('#tipoComprobanteId').val() + "&comprobanteId=" + $('#comprobanteId').val();
});

$('#tipoComprobanteId').change(function () {

    if ($('#tipoComprobanteId option:selected').text() == "PAGO") {
        $('.chequeForm').show();
    } else
        $('.chequeForm').hide();


});

$('#btnGuardarComprobante').on('click', function () {
    var sms = "";
    sms = sms + $('#tipoComprobanteId').val().length <= 0 ? "Seleccione el tipo de comprobante \n" : "";
    sms = sms + ($('#concepto').val().length <= 0 ? "Ingrese la referencia \n" : "");

    if ($('#tipoComprobanteId option:selected').text() == "PAGO")
        sms = sms + ($('#beneficiario').val().length <= 0 ? "Ingrese el Beneficiario \n" : "");

    if (validarSaldosMovimientos() == true)
        return false;
    else if ($.trim(sms) != "")
        swal("", sms, "warning");
    else
        SaveComprobante();
});

function redondeo(numero, decimales) {
    var flotante = parseFloat(numero);
    var resultado = Math.round(flotante * Math.pow(10, decimales)) / Math.pow(10, decimales);
    return resultado;
}

function validarSaldosMovimientos() {
    var credito = 0;
    var debito = 0;

    var rowsMovimientos = $("#jqwgMovimientos").jqxGrid("getrows");

    var cuentaBancoReg = false;
    for (var i = 0; i < rowsMovimientos.length; i++) {
        credito = credito + parseFloat(rowsMovimientos[i].credito);
        debito = debito + parseFloat(rowsMovimientos[i].debito);

        credito = redondeo(credito, 2);
        debito = redondeo(debito, 2);

        //1-validar que sea un tipo comprobante de pago
        if ($("#tipoComprobanteId").val() == 2) {
            //2-validar si en el credito existe una cuenta de banco

            if (rowsMovimientos[i].Cuentabanco == 'true' || rowsMovimientos[i].Cuentabanco == true) {
                //Validar que en un comprobante de pago exista una cuenta bancaria seleccionada y registrada en el catalogo
                cuentaBancoReg = true;
                //if (!rowsMovimientos[i].Cuentabanco) {
                if (!(parseFloat(rowsMovimientos[i].credito) > 0)) {
                    swal("", "El comprobante de PAGO debe afectar una cuenta bancaria en el credito de los movimientos, Corrija!!!", "error");
                    return true;
                }
            }
        }
    }
    //Mesaje de validacion de cuentas bancarias en comprobantes de pagos
    if ($("#tipoComprobanteId").val() == 2 && cuentaBancoReg == false) {
        swal("", "El comprobante de PAGO debe afectar una cuenta bancaria en el credito de los movimientos y debe haber sido checkeada como cuenta bancara en el catalogo de cuenta, Corrija !!!", "error");
        return true;
    }

    console.log("credito: " + credito + " " + "Debito: " + debito);
    if (credito != debito) {
        swal('', 'Ocurrio un error, Los montos del credito y debito deben de ser iguales', 'error');
        return true;
    }

}

$("#btnSaveReiniciarNum").on("click", function () {
    $("#reinicio").val($("#reinicioTxt").val());

    if ($("#reinicio").val() <= $("#numCheque").val()) {
        swal("", "El numero de reinicio de cheque no puede ser igual o menor al consecutivo inmediato", "error");
        $("#reinicio").val("");
        return false;
    }


    if (!($("#reinicio").val().length > 0 && $('#justificacion').val().length > 4)) {
        swal("", "El N° Reinicio no puede esta vacio y la Justificantes debe conterner al menos una palabra coherente", "error");
        return false;

    }

    var info =
        { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), reinicio: $("#reinicio").val() };
    $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/GetLastReinicioCheque', info, function (result) {
        if (result != "") {
            swal("", result, "error");

            return false;
        } else {
            $("#numCheque").val($("#reinicio").val());
            $("#numero").val($("#reinicio").val());

        }

    }).fail(function (XMLHttpRequest, textStatus, errorThrown) {
        alert(textStatus + ": " + XMLHttpRequest.responseText);
    });

    $('#modalReiniciarCheques').modal('hide');
});

$('#btnEjecutar').on('click', function () {
    if (validarSaldosMovimientos() == true)
        return false
    else {
        $('#estadoId').val(3); //Definicion del estado del movimiento
        SaveComprobante();
    }

});



function validateDecimal(valor) {
    var RE = /^\d*(\.\d{1})?\d{0,1}$/;
    if (RE.test(valor)) {
        return true;
    } else {
        swal("", "Digite correctamente el monto. El monto tiene que tener dos decimales", "error");
        return false;
    }
}

$('#btnInsertarMov').on('click', function () {

    if ($('#codigo').val().split('-').length != 5) {
        swal('', 'La cuenta seleccionar no es detalle, favor seleccione corectamente la cuenta afectar', 'error');
        $("#cuentaId").val("");
        $("#descripcion").val("");
        $("#codigo").val("");

        return false;
    }

    if (!(validateDecimal(deleteComas($('#debito').val()))))
        return false;

    if (!(validateDecimal(deleteComas($('#credito').val()))))
        return false;

    if (!($("#cuentaId").val() > 0)) {
        swal('', 'El numero de cuenta digitado no existe, digitelo nuevamente', 'error');
        return false;
    }

    if (($('#debito').val().length == 0 && $('#credito').val().length == 0) || ($('#debito').val() == 0 && $('#credito').val() == 0)) {
        swal('', 'La información ingresada en el DEBITO o en el CREDITO no puede ser cero o vacia', 'warning');
        return false;
    } else if (((deleteComas($('#debito').val()) * deleteComas($('#credito').val())) != 0)) {
        swal('', 'No puede ingresar información en el debito y credito en el mismo registro', 'warning');
        return false;
    }

    AddMovimientos();
});

/**************************************************
*DEFINICION DE ELEMENTOS DEL JQWIDGET
**************************************************/
var source =
    {
        dataType: "json",
        dataFields: [
            { name: 'comprobanteCuentaId', type: 'number' },
            { name: 'comprobanteId', type: 'number' },
            { name: 'cuentaId', type: 'number' },
            { name: 'codigo', type: 'string' },
            { name: 'descripcion', type: 'string' },
            { name: 'Cuentabanco', type: 'bool' },
            { name: 'credito', type: 'float' },
            { name: 'debito', type: 'float' }
        ],
        comprobanteId: 'comprobanteId',
        url: $('input[name=pathRoot]').val() + '/ContabilidadMCA/Cuentas/GetListComprobantes?comprobanteId=' + $('#comprobanteId').val()
    };

var columns = [
       {
           text: ' ', dataField: 'comprobanteCuentaId', width: 50, editable: false, resizable: false, cellsalign: 'center', align: 'center', // hidden: (DESACTIVAR_CONTROLES != 0 ? true : false),
           cellsrenderer: function (row, column, value, defaultSettings, columnSettings, rowdata) {
               return '<div style="text-align: center;padding-top: 3px;"><button type="button" class="btn btn-danger btn-xs modificacion"  style="padding: 0 8px;"  onclick="return fnDeleteItem(' + row + ',' + rowdata.comprobanteCuentaId + ');" title="Eliminar item registro de recorrido"><i class="fa fa-trash" aria-hidden="true"></i></button></div>';
           }
       },
       { text: 'comprobanteId', dataField: 'comprobanteId', width: 250, hidden: true, cellsalign: 'left' },
       { text: 'cuentaId', dataField: 'cuentaId', width: 250, hidden: true, cellsalign: 'left' },
       { text: 'Cuentabanco', dataField: 'Cuentabanco', width: 250, hidden: true, cellsalign: 'left' },
       { text: 'Cuenta', dataField: 'codigo', width: 250, editable: false, cellsalign: 'center', align: 'center' },
       { text: 'Descripcion', dataField: 'descripcion', width: 500, editable: false, cellsalign: 'center', align: 'center' },
       { text: 'Debito', dataField: 'debito', width: 200, editable: false, cellsalign: 'center', cellsformat: 'f2', align: 'center', aggregates: ["sum"] },
       { text: 'Credito', dataField: 'credito', width: 200, editable: false, cellsalign: 'center', cellsformat: 'f2', align: 'center', aggregates: ["sum"] },

];

/*****************************************************
*Funcion de carga inicial del Grid, funcion que se encuentra en JQWGGENERAL.JS
*****************************************************/
CargarGrid(source, columns);
validarEdicion();
/*****************************************************
*Funcion agrega movimientos al comprobante
*****************************************************/
function AddMovimientos() {
    //if ($('#nombres').val() == 0 || $('#apellidos').val().length == 0 || $('#tipoUsuarioId').val().length == 0 || $('#tipoUsuarioId').val().length == 0) {
    //    swal('', 'Todos los campos son requeridos', 'error');
    //    return false;
    //}
    //condición ? valor1 : valor2

    $('#credito').val() == "" ? $('#credito').val(0) : $('#credito').val();
    $('#debito').val() == "" ? $('#debito').val(0) : $('#debito').val();

    CONTADOR = CONTADOR - 1;
    var rowNewData = {};
    rowNewData["comprobanteCuentaId"] = CONTADOR;
    rowNewData["comprobanteId"] = $('#comprobanteId').val();
    rowNewData["cuentaId"] = $('#cuentaId').val();
    rowNewData["Cuentabanco"] = $('#Cuentabanco').val();
    rowNewData["codigo"] = $('#codigo').val();
    rowNewData["descripcion"] = $('#descripcion').val();

    rowNewData["credito"] = parseFloat(deleteComas($('#credito').val()));
    rowNewData["debito"] = parseFloat(deleteComas($('#debito').val()));


    // rowNewData["usuarioId"] = $('#usuarioId').val() == "" ? CONTADOR : $('#usuarioId').val();

    $("#jqwgMovimientos").jqxGrid("addrow", null, rowNewData);
    fnCloseModal('#modalAddMovimientos');

    return false;
}


/*****************************************************
*Funcion que maneja el AutoCompletar
*****************************************************/
var options = {

    url: function (phrase) {
        return $('input[name=pathRoot]').val() + "/ContabilidadMCA/Cuentas/GetListCuentas";
    },

    getValue: function (element) {
        return element.codigo;
    },

    ajaxSettings: {
        dataType: "json",
        method: "POST",
        data: {
            dataType: "json"
        }
    },
    preparePostData: function (data) {
        data.phrase = $("#codigo").val();
        return data;
    },
    list: {
        onSelectItemEvent: function () {
            $("#cuentaId").val($("#codigo").getSelectedItemData().cuentaId);
            $("#Cuentabanco").val($("#codigo").getSelectedItemData().Cuentabanco);
            var valor = $("#codigo").getSelectedItemData().descripcion;
            $("#descripcion").val(valor).trigger("change");
        }
    },

    requestDelay: 400
};

$("#codigo").easyAutocomplete(options);

/*****************************************************************************
 * Función para Guardar los datos de la Cuenta
 *****************************************************************************/
function SaveComprobante() {

    swal({ title: "", text: "¿ Esta Seguro que desea Guardar los datos de este comprobante? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
        function () {

            /***************************/
            var rowsMovimientos = $("#jqwgMovimientos").jqxGrid("getrows");
            if (rowsMovimientos.length <= 1) {
                swal('', 'La cantidad de movimientos ingresada no corresponde', 'warning'); return false;
            }

            var toComprobante = {
                comprobanteId: $('#comprobanteId').val(),
                estadoId: $('#estadoId').val(),
                numero: $('#numero').val(),
                concepto: $('#concepto').val(),
                beneficiario: $('#beneficiario').val(),
                numCheque: $('#numCheque').val(),
                fechaComprobante: $('#fechaComprobante').val(),
                tipoComprobanteId: $('#tipoComprobanteId').val(),

            }

            var info =
                {
                    __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                    movimientos: rowsMovimientos, toComprobante: toComprobante, reinicio: $('#reinicio').val(),
                    justificacion: $('#justificacion').val()


                };
            $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/SaveComprobante', info, function (result) {
                if (result.toComprobante != null && result.msj == "") {
                    $('#comprobanteId').val(result.toComprobante.comprobanteId);
                    $('#numero').val(result.toComprobante.numero);
                    validarEdicion();
                    swal("", "Los datos del comprobante fueron guardados exitosamente", "success");
                    setTimeout(function () {
                        location.href = $('input[name="pathRoot"]').val() + "/ContabilidadMCA/Cuentas/Comprobantes/Index"
                    }, 1500);


                } else {
                    swal("", result.msj, "error");
                }
                setTimeout(function () { }, 1000);

            }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
            return false;
        });
}

/*****************************************************/
/*** Eliminar Item o fila del JQWIDGWTS   ***/
function fnDeleteItem(row, movimiento) {
    swal({ title: "", text: "¿ Seguro desea borrar el Movimiento?", type: "warning", cancelButtonText: "NO", showCancelButton: true, confirmButtonText: "SI", closeOnConfirm: true, },
        function () {

            var id = $("#jqwgMovimientos").jqxGrid('getrowid', row);
            //   var rowDistanciaProgramada = $("#jqxgridRecorridos").jqxGrid('getrowdata', row).distanciaProgramada;

            if (movimiento > 0) {
                $.post($('input[name=pathRoot]').val() + '/ContabilidadMCA/Cuentas/EliminarMovimiento?movimientoId=' + movimiento, function (result) {
                    if (result == 1) {
                        swal('', 'Registro borrado exitosamente', 'success');
                        $("#jqwgMovimientos").jqxGrid('deleterow', id);
                    }
                }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
            } else {
                $("#jqwgMovimientos").jqxGrid('deleterow', id);
            }
            //  fnOperarCantidadKilometros(rowDistanciaProgramada); /** Resta la distancia programada al input totalKms, para calcular el rendimiento **/

        });
    return false;
}
