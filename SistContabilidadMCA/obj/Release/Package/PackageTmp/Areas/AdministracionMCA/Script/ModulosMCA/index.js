/**********************************************************************************************************************************************
 * Función General
 **********************************************************************************************************************************************/
$(document).ready(function () {
    //fnGuardarAcuerdo();
});


$('#UcrBM').on('click', function () {
    $("#ucrId").val(1); //Identificador de la UCR BM
    GetListMCA();
    return false;
});

$('#UcrBCIE').on('click', function () {
    $("#ucrId").val(2); //Identificador de la UCR BCIE
    GetListMCA();
    return false;
});

$('#UcrRV').on('click', function () {
    $("#ucrId").val(3); //Identificador de la UCR RV
    GetListMCA();
    return false;
});


/**********************************************************************************************************************************************
 * Función validar Edicion
 **********************************************************************************************************************************************/

function fnValidarEdicion(moduloMCAId) {


    var url = $('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/ValdiarEdicion';

    $.getJSON(url, { moduloMCAId: moduloMCAId }, function (data) {
        if ($.trim(data) != "") {
        
            setTimeout(function () {
                swal("", data, "warning");
            }, 1500)

            location.href = $('input[name="pathRoot"]').val() + "/AdministracionMCA/ModuloMCA/EditMCA?moduloMCAId=" + moduloMCAId+"&Eval=1";
            return false;
        }
        else {
            location.href = $('input[name="pathRoot"]').val() + "/AdministracionMCA/ModuloMCA/EditMCA?moduloMCAId=" + moduloMCAId;
        }
    });

    return false;
}





/**********************************************************************************************************************************************
 * Función de llamado a Vista parcial de los Listado de MCA
 **********************************************************************************************************************************************/
function GetListMCA() {
    
    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), ucrId: $("#ucrId").val() };
    $.post($('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/GetListMCA', info, function (result) {
        //$('#ViewGeneral').fadeOut(1);
        $('#ViewGeneral').toggle();
        $('#ViewListaMCA').html('');
        $('#ViewListaMCA').html(result);
        document.getElementById("divucr1").style.display = "block";
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    return true;
}

/**********************************************************************************************************************************************
 * Función  Pantalla Acuerdos Suplementarios
 **********************************************************************************************************************************************/
function fnDetalleProyecto(moduloMCAId) {
    location.href = $('input[name="pathRoot"]').val() + "/AdministracionMCA/ModuloMCA/DetalleMCA?moduloMCAId=" + moduloMCAId;
    return false;
}

/**********************************************************************************************************************************************
 * Función Aparecer Panel Nuevo Modulo
 **********************************************************************************************************************************************/
function AparecerPanel() {
    document.getElementById("FormNewAcuerdo").style.display = "block";
    document.getElementById("divNuevo").style.display = "block";
    document.getElementById("divEdicion").style.display = "none";
    document.getElementById("btnCancelar").style.display = "block";
    document.getElementById("divBaja").style.display = "none";
    LimpiarDatos()

    return false;
}



/**********************************************************************************************************************************************
 * Función Cerrar Panel Nuevo Modulo
 **********************************************************************************************************************************************/
function CerrarPanel() {
    swal({
        title: "",
        text: "¿Esta seguro que desea Salir ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            swal("", "", "success");
    document.getElementById("FormNewAcuerdo").style.display = "none";
    LimpiarDatos()
    //$('#descripcion').val(""),
    //$('#numero').val(""),    
    // $("#tipoExtencionId").val("").trigger("change.select2"),   
    //$('#monto').val(""),
    //$('#loguitud').val(""),
    //$('#fechaInicial').val(""),
    //$('#fechaFinal').val("")
    return false;
        }
        else {
            swal("", "", "error");
        }
        return false;
    });
    return false;
}


/**********************************************************************************************************************************************
 * Función para guardar Acuerdo Suplementarios
 **********************************************************************************************************************************************/
function fnGuardarAcuerdo() {
    swal({
        title: "",
        text: "¿Desea Guardar el Acuerdo Suplementario Digitado?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm){

            if ($('#numero').val() == 0 || $('#descripcion').val() == 0 || $('#tipoExtencionId').val().length == 0 || $('#monto').val() == 0
                || $('#loguitud').val() == 0 || $('#fechaInicial').val() == 0 || $('#fechaFinal').val() == 0) {
                swal('', 'Todos los Datos son requeridos', 'error');
                return false;
            }
            //$("#formAddAcuerdo").submit(function () {

                var url = $('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/SaveAcuerdo';
                var parameter = {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    acuerdoSupMCAId: ($('input[name="acuerdoSupMCAId"]').val() == '' ? 0 : $('input[name="acuerdoSupMCAId"]').val()),
                    moduloMACId: $.trim($('#moduloMCAId').val()),
                    numero: $.trim($('#numero').val()),
                    descripcion: $.trim($('#descripcion').val()),
                    tipoExtencionId: $.trim($('#tipoExtencionId').val()),
                    //tipoExtensionId: $.trim($('#tipoExtencionId').val().trigger("change.select2")),
                    //monto: $('#monto').val().replace(',', ''),
                    monto: deleteComas($.trim($('#monto').val())),
                    loguitud: $.trim($('#loguitud').val()),
                    fechaInicial: $.trim($('#fechaInicial').val()),
                    fechaFinal: $.trim($('#fechaFinal').val())
                };

                $.post(url, parameter, function (data) {
                    if ($.trim(data.mensaje) != "") {
                        swal("", data.mensaje, "warning");
                        return false;
                    }
                    swal("", "El Acuerdo suplementario fue Registrado con exito.", "success");
                    GetListaAcuerdos.Refresh();
                    document.getElementById("FormNewAcuerdo").style.display = "none";
        



                }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
            //});
                return false;
        }
            else {
            swal("Cancelado", "", "error");
            document.getElementById("FormNewAcuerdo").style.display = "none";
               
            }
        return false;
    });
        return false;

}

/**********************************************************************************************************************************************
 * Función para editar Acuerdos
 **********************************************************************************************************************************************/
function fnEditAcuerdo(acuerdoSupMCAId) {
    var url = $('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/GetAcuerdos';
    $.getJSON(url, { acuerdoSupMCAId: acuerdoSupMCAId }, function (data) {
        if ($.trim(data) == "") {
            swal("", "El Acuerdo Suplementario seleccionado, no se puede editar porque ya existe un nuevo Acuerdo.", "warning");
            return false;
        }
        else {

            $('#acuerdoSupMCAId').val(data.acuerdoSupMCAId);
            $('#descripcion').val(data.descripcion);
            $('#numero').val(data.numero);
            $('#tipoExtencionId').val(data.tipoExtencionId).trigger("change.select2");           
            //$('#tipoExtensionId').val(data.tipoExtencionId);
            $('#monto').val(data.monto);
            $('#loguitud').val(data.loguitud);
            $('#fechaInicial').val(data.fechaInicial);
            $('#fechaFinal').val(data.fechaFinal);
            document.getElementById("divEdicion").style.display = "block";
            document.getElementById("divBaja").style.display = "block";
            //document.getElementById("divEdicion").style.display = "block";
            document.getElementById("divNuevo").style.display = "none";
            document.getElementById("FormNewAcuerdo").style.display = "block";           
            return false;
            GetListaAcuerdos.Refresh();
        }
    });
    return false;
}


/**********************************************************************************************************************************************
 * Función para anulacion de registro
 **********************************************************************************************************************************************/
function fnBajaDataA() {
    var acuerdoSupMCAId = $('#acuerdoSupMCAId').val();
    swal({
        title: "",
        text: "¿Desea anular el Acuerdo Suplementario seleccionado?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            var url = $('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/BajaAcuerdo';
            $.getJSON(url, { acuerdoSupMCAId: acuerdoSupMCAId }, function (data) {
               
                swal("", "El Acuerdo seleccionado fue anulado exitosamente.", "success");
                GetListaAcuerdos.Refresh();
                document.getElementById("FormNewAcuerdo").style.display = "none";               
                return false;               
            });
            return false;
        }
        else {
            swal("Cancelado", "", "error");
        }
        return false;
    });
    return false;
}



/**********************************************************************************************************************************************
 * Función limpiar datos
 **********************************************************************************************************************************************/
function LimpiarDatos() {
    $('#descripcion').val(""),
    $('#numero').val(""),    
     $("#tipoExtencionId").val("").trigger("change.select2"),   
    $('#monto').val(""),
    $('#loguitud').val(""),
    $('#fechaInicial').val(""),
    $('#fechaFinal').val("")
}


/**********************************************************************************************************************************************
 * Función para aceptar solo numeros
 **********************************************************************************************************************************************/
function RemoveCharacters(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros
    //patron = /[A-Z-a-z-0-9-_ ]/;
    patron = /[0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}