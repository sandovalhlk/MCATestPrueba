var AreaActual;
/**********************************************************************************************************************************************
 * Función General
 **********************************************************************************************************************************************/
$(document).ready(function () {

});


$('#btnGuardarP').on('click', function () {
    fnGuardarProyecto();
});

$('#UcrBM').on('click', function () {
    $("#ucrId").val(1); //Identificador de la UCR BM   
    GetListaProyectos();
});

$('#UcrBCIE').on('click', function () {
    $("#ucrId").val(2); //Identificador de la UCR BCIE    
    GetListaProyectos();

});


$('#UcRV').on('click', function () {
    $("#ucrId").val(3); //Identificador de la UCR RV    
    GetListaProyectos();
});

function GetListaProyectos() {   
    //$('#ucrName').text(ucrName);
    
   
    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), ucrId: $("#ucrId").val() };
    $.post($('input[name="pathRoot"]').val() + '/Catalogos/Catalogos/GetListaProyectos', info, function (result) {
        $('#ViewGeneral').toggle();
        $('#ViewListaMCA').html('');
        $('#ViewListaMCA').html(result);   
        document.getElementById("divucr").style.display = "block";


    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });

}


/**********************************************************************************************************************************************
 * Función para guardar nuevo Proyecto.....
 **********************************************************************************************************************************************/
function fnGuardarProyecto() {
    if ($('#nombreProyecto').val() == 0 || $('#longuitud').val().length == 0 || $('#monto').val().length == 0 || $('#fechaInicio').val().length == 0 || $('#fechaFin').val().length == 0 || $('#convenioId').val().length == 0 || $('#ucrIds').val().length == 0) {
        swal('', 'Todos los campos son requeridos', 'error');
        return false;
    }

    //var vMonto = deleteComas($('#monto').val()).replace('.', ',');
    //var vlong = deleteComas($('#longuitud').val()).replace('.', ',');
    swal({
        title: "",
        text: "¿Desea Guardar el Proyecto digitado?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            var url = $('input[name="pathRoot"]').val() + '/Catalogos/Catalogos/SaveProyecto';
            var parameter = {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                proyectoId: ($('input[name="proyectoId"]').val() == '' ? 0 : $('input[name="proyectoId"]').val()),
                nombreProyecto: $.trim($('#nombreProyecto').val()),
                longuitud: deleteComas($('#longuitud').val()),
                monto: deleteComas($('#monto').val()),
                fechaInicio: $.trim($('#fechaInicio').val()),
                fechaFin: $.trim($('#fechaFin').val()), 
                convenioId: $.trim($('#convenioId').val()),
                ucrId: $.trim($('#ucrIds').val())
            };
            $.post(url, parameter, function (result) {
                if ($.trim(result.mensaje) != "") {
                    swal("", result.mensaje, "warning");
                    fnOpenModal('#ModalProyecto', false, false)
                    document.getElementById("divEdicion").style.display = "none";
                    document.getElementById("divNuevo").style.display = "block";
                    return false;
                }
                swal("", "El Proyecto fue registrado exitosamente.", "success");
                FnRefrescarProyectos();
                fnOpenModal('#ModalProyecto', false, false);
                document.getElementById("divEdicion").style.display = "none";
                document.getElementById("divNuevo").style.display = "block";
                document.getElementById("divBaja").style.display = "none";
                //fncloseModal();
            }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
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
 * Función actualizar Grid GetListaProyecto
 **********************************************************************************************************************************************/
function FnRefrescarProyectos() {
    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), ucrId: $.trim($('#ucrIds').val()) };
    $.post($('input[name="pathRoot"]').val() + '/Catalogos/Catalogos/GetListaProyectos', info, function (result) {
        $('#ViewGeneral').fadeOut(1);
        //$('#ViewGeneral').toggle();
        $('#ViewListaMCA').html('');
        $('#ViewListaMCA').html(result);
        $("#btnnuevo").css("display", "block")


    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });

}


/**********************************************************************************************************************************************
 * Función para  cerrar modal
 **********************************************************************************************************************************************/
//function fncloseModal() {
//    document.getElementById("divBaja").style.display = "none";
//    document.getElementById("divEdicion").style.display = "none";
//    document.getElementById("divNuevo").style.display = "block";
//    fnOpenModal('#ModalProyecto', false, false)
//}


/**********************************************************************************************************************************************
 * Función para editar Registros
 **********************************************************************************************************************************************/
function fnEditProyecto(proyectoId) {
    var url = $('input[name="pathRoot"]').val() + '/Catalogos/Catalogos/GetProyecto';
    $.getJSON(url, { proyectoId: proyectoId }, function (data) {
        if ($.trim(data) == "") {
            swal("", "El Proyecto seleccionado, no se puede editar porque está asociado a un Módulo MCA.", "warning");
            return false;
        }
        else {                  

            $('#proyectoId').val(data.proyectoId);
            $('#nombreProyecto').val(data.nombreProyecto);
            $('#longuitud').val(data.longuitud);
            $('#monto').val(data.monto);
            $('#fechaInicio').val(data.fechaInicio);
            $('#fechaFin').val(data.fechaFin);
            $('#convenioId').val(data.convenioId).trigger("change.select2");           
            $('#ucrIds').val(data.ucrId).trigger("change.select2");
            document.getElementById("divBaja").style.display = "block";
            document.getElementById("divEdicion").style.display = "block";
            document.getElementById("divNuevo").style.display = "none";
            fnOpenModal('#ModalProyecto', true, true)
            return false;
        }
    });
    return false;
}


/**********************************************************************************************************************************************
 * Función para anulacion de registro
 **********************************************************************************************************************************************/
function fnBajaDataP() {
    var proyectoId = $('#proyectoId').val();
    swal({
        title: "",
        text: "¿Desea anular el Proyecto seleccionado?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            var url = $('input[name="pathRoot"]').val() + '/Catalogos/Catalogos/BajaProyecto';
            $.getJSON(url, { proyectoId: proyectoId }, function (data) {
                if ($.trim(data) == "") {
                    swal("", "El Proyecto seleccionado, no se puede anular porque está asociado a un Módulo MCA.", "warning");
                    return false;
                }
                else {
                    swal("", "El Proyecto seleccionado fue anulado exitosamente.", "success");
                    fnOpenModal('#ModalProyecto', false, false)
                    FnRefrescarProyectos();
                    document.getElementById("divEdicion").style.display = "none";
                    document.getElementById("divBaja").style.display = "none";
                    document.getElementById("divNuevo").style.display = "block";
                   
                    return false;
                }
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
 * Función para  cerrar Panel
 **********************************************************************************************************************************************/

function fncloseModalP() {
    swal({
        title: "",
        text: "¿Esta seguro que desea Salir ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: true,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            //swal("", "", "success");
            document.getElementById("divBaja").style.display = "none";
            document.getElementById("divEdicion").style.display = "none";
            document.getElementById("divNuevo").style.display = "block";
            fnOpenModal('#ModalProyecto', false, false);
            return false;
        }
        else {
            swal("", "", "error");
        }
        return false;
    });
    return false;
}
