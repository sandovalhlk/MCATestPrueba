/**********************************************************************************************************************************************
 * Función General
 **********************************************************************************************************************************************/
$(document).ready(function () {
    fnGuardarConvenio();
   // fnDataTables();
});


/**********************************************************************************************************************************************
 * Función para guardar nuevo Convenio
 **********************************************************************************************************************************************/
function fnGuardarConvenio() {
    $("#formNuevoConvenio").submit(function () {
        swal({
            title: "",
            text: "¿Desea Guardar el Convenio digitado?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Si, Continuar",
            cancelButtonText: "No, Cancelar",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                if ($('#convenio').val() == 0) {
                    swal('', 'Todos los Datos son requeridos', 'error');
                    return false;
                }
                var tipoFormulario = $('#tipoFormulario').val();

        var url = $('input[name="pathRoot"]').val() + '/Catalogos/Convenio/SaveConvenios';
        var parameter = {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
            convenioId: ($('input[name="convenioId"]').val() == '' ? 0 : $('input[name="convenioId"]').val()),
            convenio: $.trim($('#convenio').val()),
        };

        $.post(url, parameter, function (data) {

            if ($.trim(data.mensaje) != "") {
                swal("", data.mensaje, "warning");
                fnOpenModal('#ModalConvenio', false, false)
                return false;
            }

            swal("", "El Convenio digitado, fue registrada exitosamente.", "success");
            if (tipoFormulario == 0) {
                fnRefrescarListaConvenios();
                fncloseModalA();            
                 
               
            }

            if (tipoFormulario == 1) {
                GetComboConvenio();
                //document.getElementById("divBaja").style.display = "none";
                //document.getElementById("divEdicion").style.display = "none";
                //document.getElementById("divNuevo").style.display = "block";
                                           

            }

            fnOpenModal('#ModalConvenio', false, false)
        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
        return false;
            }
            else {
                swal("Cancelado", "", "error");
                fnOpenModal('#ModalConvenio', false, false)
            }
            return false;
        });
        return false;

    });
    }


/**********************************************************************************************************************************************
 * Función para editar Registros
 **********************************************************************************************************************************************/

function fnSetEditData(convenioId) {



    var url = $('input[name="pathRoot"]').val() + '/Catalogos/Convenio/GetConvenio';

    $.getJSON(url, { convenioId: convenioId }, function (data) {
        if ($.trim(data) == "") {
            swal("", "El Convenio seleccionado, no se puede editar porque está asociado a un proyecto.", "warning");
            return false;
        }
        else {
            $('#convenioId').val(data.convenioId);
            $('#convenio').val(data.convenio);
            document.getElementById("divBaja").style.display = "block";
            document.getElementById("divEdicion").style.display = "block";
            document.getElementById("divNuevo").style.display = "none";
            fnOpenModal('#ModalConvenio', true, true)
            return false;
        }
    });

    return false;

}



/**********************************************************************************************************************************************
 * Función para anulacion de registro
 **********************************************************************************************************************************************/
function fnBajaData() {

    var convenioId = $('#convenioId').val();

    swal({
        title: "",
        text: "¿Desea anular el Convenio seleccionado?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {

            var url = $('input[name="pathRoot"]').val() + '/Catalogos/Convenio/BajaConvenios';

            $.getJSON(url, { convenioId: convenioId }, function (data) {
                if ($.trim(data) == "") {
                    swal("", "El Convenio no se puede anular porque está asociado a un Proyecto.", "warning");
                    return false;
                }
                else {

                    swal("", "El Convenio seleccionado, fue anulado exitosamente.", "success");
                    fnRefrescarListaConvenios();
                    document.getElementById("divBaja").style.display = "none";
                    document.getElementById("divEdicion").style.display = "none";
                    document.getElementById("divNuevo").style.display = "block";
                    fnOpenModal('#ModalConvenio', false, false)
                    
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
 * Función actualizar DataTable GetListaConvenios
 **********************************************************************************************************************************************/
function fnRefrescarListaConvenios() {
    $.post($('input[name="pathRoot"]').val() + '/Catalogos/Convenio/GetListaConvenios', {}, function (result) {
        $('#divconvenio').html('');
        $('#divconvenio').html(result);
        fnDataTables();
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
}



/**********************************************************************************************************************************************
 * Función para  cerrar modal Proeyecto
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
                document.getElementById("divBaja").style.display = "none";
                document.getElementById("divEdicion").style.display = "none";
                document.getElementById("divNuevo").style.display = "block";
                fnOpenModal('#ModalConvenio', false, false)
            
            }

            if (tipoFormulario == 1) {
               
                fnOpenModal('#ModalConvenio', false, false)
                
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
 * Función para  cerrar modal Convenio desde el guardar
 **********************************************************************************************************************************************/
function fncloseModalA() {    
            document.getElementById("divBaja").style.display = "none";
            document.getElementById("divEdicion").style.display = "none";
            document.getElementById("divNuevo").style.display = "block";
            fnOpenModal('#ModalConvenio', false, false)
            return false;
       
}


/**********************************************************************************************************************************************
 * Función refresca combo Convenio
 **********************************************************************************************************************************************/
function GetComboConvenio() {
    //borrar combo antiguo
   var convenioId = $('select[name=convenioId]');
   //convenioId.html('').text('').append('<option value="">Seleccione el Convenio</option>').select2('destroy').select2();
   convenioId.html('').text('').append('<option value="">Seleccione el Convenio</option>').select2('destroy').select2();
    //Carga de Datos
    var url = $('input[name="pathRoot"]').val() + '/Catalogos/Convenio/GetComboConvenios';
    $.getJSON(url, function (data) {
        //Crear Nuevo Combo
        $.each(data, function (i, jsondata) {
           convenioId.append('<option value="' + jsondata.convenioId + '">' + jsondata.convenio + '</option>').trigger("change.select2");
        });
    });
}






















