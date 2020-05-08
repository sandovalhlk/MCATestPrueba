/**********************************************************************************************************************************************
 * Función General
 **********************************************************************************************************************************************/
$(document).ready(function () {
    fnGuardarTipoC();
    //fnDataTables();
});


/**********************************************************************************************************************************************
 * Función para guardar nuevo Convenio
 **********************************************************************************************************************************************/
//function fnGuardarTipoC() {
//    $("#formNuevoTipoExtension").submit(function () {
//        var tipoFormulario = $('#tipoFormulario').val();
//        var url = $('input[name="pathRoot"]').val() + '/Catalogos/TipoExtension/SaveTipoExtension';
//        var parameter = {
//            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
//            tipoExtensionId: ($('input[name="tipoExtensionId"]').val() == '' ? 0 : $('input[name="tipoExtensionId"]').val()),
//            tipoExtencion: $.trim($('#tipoExtencion').val()),
//        };

//        $.post(url, parameter, function (data) {

//            if ($.trim(data.mensaje) != "") {
//                swal("", data.mensaje, "warning");
//                fnOpenModal('#ModalTipoExtension', false, false)
//                return false;
//            }

//            swal("", "El Tipo de Extensión digitado, fue registrada exitosamente.", "success");
//            if (tipoFormulario == 0) {
//                fnRefrescarListaTipoE();
//                fncloseModalA();
//            }

//            if (tipoFormulario == 1) {
//                GetComboTextensiones();
//                fncloseModalA();
//            }

//            fncloseModalA();
//        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
//        return false;
//    });
//}

function fnGuardarTipoC() {
    
    $("#formNuevoTipoExtension").submit(function () {
        swal({
            title: "",
            text: "¿Desea Guardar la Extensión digitada?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Si, Continuar",
            cancelButtonText: "No, Cancelar",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
        if ($('#tipoExtencion').val() == 0) {
            swal('', 'Todos los Datos son requeridos', 'error');
         return false;
        }


        var tipoFormulario = $('#tipoFormulario').val();    
            

        var url = $('input[name="pathRoot"]').val() + '/Catalogos/TipoExtension/SaveTipoExtension';
        var parameter = {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
            tipoExtensionId: ($('input[name="tipoExtensionId"]').val() == '' ? 0 : $('input[name="tipoExtensionId"]').val()),
            tipoExtencion: $.trim($('#tipoExtencion').val()),
        };

        $.post(url, parameter, function (data) {

            if ($.trim(data.mensaje) != "") {
                swal("", data.mensaje, "warning");
                fnOpenModal('#ModalTipoExtension', false, false)
                return false;
            }

            swal("", "El Tipo de Extensión digitado, fue registrada exitosamente.", "success");
            if (tipoFormulario == 0) {
                fnRefrescarListaTipoE();
                 fncloseModalA();
                
            }

            if (tipoFormulario == 1) {
                GetComboTextensiones();              
               
            }
           


            fnOpenModal('#ModalTipoExtension', false, false)
        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
        return false;
            }
            else {
                swal("Cancelado", "", "error");
                fnOpenModal('#ModalTipoExtension', false, false)
            }
            return false;
        });
        return false;
    });
}


/**********************************************************************************************************************************************
 * Función actualizar DataTable 
 **********************************************************************************************************************************************/
function fnRefrescarListaTipoE() {
    $.post($('input[name="pathRoot"]').val() + '/Catalogos/TipoExtension/GetListaTipoExtensiones', {}, function (result) {
        $('#divTipoE').html('');
        $('#divTipoE').html(result);
        //fnDataTables();
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
}

/**********************************************************************************************************************************************
 * Función para  cerrar modal desde el guardar
 **********************************************************************************************************************************************/
function fncloseModalA() {   
    document.getElementById("divBaja").style.display = "none";
    document.getElementById("divEdicion").style.display = "none";
    document.getElementById("divNuevo").style.display = "block";
    fnOpenModal('#ModalTipoExtension', false, false)
}

/**********************************************************************************************************************************************
 * Función para  cerrar modal
 **********************************************************************************************************************************************/
//function fncloseModal() {
//    document.getElementById("divBaja").style.display = "none";
//    document.getElementById("divEdicion").style.display = "none";
//    document.getElementById("divNuevo").style.display = "block";
//    fnOpenModal('#ModalConvenio', false, false)
//}
function fncloseModal() {
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
            var tipoFormulario = $('#tipoFormulario').val();           
            if (tipoFormulario == 0) {                
                fncloseModalA();
                fnOpenModal('#ModalTipoExtension', false, false)

            }

            if (tipoFormulario == 1)   {                
                fnOpenModal('#ModalTipoExtension', false, false)
               
            }
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
 * Función para editar Registros
 **********************************************************************************************************************************************/

function fnSetEditData(tipoExtensionId) {

    var url = $('input[name="pathRoot"]').val() + '/Catalogos/TipoExtension/GetTipoExtension';

    $.getJSON(url, { tipoExtensionId: tipoExtensionId }, function (data) {
        if ($.trim(data) == "") {
            swal("", "El Tipo de extensión seleccionado, no se puede editar porque está asociado a un Acuerdo.", "warning");
            return false;
        }
        else {
            $('#tipoExtensionId').val(data.tipoExtensionId);
            $('#tipoExtencion').val(data.tipoExtencion);
            document.getElementById("divBaja").style.display = "block";
            document.getElementById("divEdicion").style.display = "block";
            document.getElementById("divNuevo").style.display = "none";
            fnOpenModal('#ModalTipoExtension', true, true)
            return false;
        }
    });

    return false;

}



/**********************************************************************************************************************************************
 * Función para anulacion de registro
 **********************************************************************************************************************************************/
function fnBajaData() {

    var tipoExtensionId = $('#tipoExtensionId').val();

    swal({
        title: "",
        text: "¿Desea anular el tipo de Extensión seleccionado?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {

            var url = $('input[name="pathRoot"]').val() + '/Catalogos/TipoExtension/BajaTipoE';

            $.getJSON(url, { tipoExtensionId: tipoExtensionId }, function (data) {
                if ($.trim(data) == "") {
                    swal("", "El Tipo de Extensión no se puede anular porque está asociado a un Acuerdo.", "warning");
                    return false;
                }
                else {

                    swal("", "El Tipo de Extensión seleccionado, fue anulado exitosamente.", "success");
                    fnRefrescarListaTipoE();
                    document.getElementById("divBaja").style.display = "none";
                    document.getElementById("divEdicion").style.display = "none";
                    document.getElementById("divNuevo").style.display = "block";
                    fnOpenModal('#ModalTipoExtension', false, false)
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
 * Función refresca combo 
 **********************************************************************************************************************************************/
function GetComboTextensiones() {
    //borrar combo antiguo
    var tipoExtencionId = $('select[name=tipoExtencionId]');
    //convenioId.html('').text('').append('<option value="">Seleccione el Convenio</option>').select2('destroy').select2();
    tipoExtencionId.html('').text('').append('<option value="">Seleccione el Tipo de Extensión</option>').select2('destroy').select2();
    //Carga de Datos
    var url = $('input[name="pathRoot"]').val() + '/Catalogos/TipoExtension/GetComboTextensiones';
    $.getJSON(url, function (data) {
        //Crear Nuevo Combo
        $.each(data, function (i, jsondata) {
            tipoExtencionId.append('<option value="' + jsondata.tipoExtensionId + '">' + jsondata.tipoExtencion + '</option>').trigger("change.select2");
        });
    });
}


