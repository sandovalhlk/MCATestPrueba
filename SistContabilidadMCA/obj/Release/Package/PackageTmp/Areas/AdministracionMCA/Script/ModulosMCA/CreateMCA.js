///********************************
//*Funcion principal
//*****************************/
$(document).ready(function () {
   
    var tipoFormulario = $('#tipoFormulario').val();
   
    if (tipoFormulario == 1 || tipoFormulario == 3) {
        $('#Solicitud').on(setTimeout(function () {
            //Code
            location.href = $('input[name="pathRoot"]').val() + "/AdministracionMCA/ModuloMCA/Index";
        }, 1500));
    }  

});

$('#btnAddFirmantes').on('click', function () {
    $("#nombres").attr("disabled", true);
    $("#apellidos").attr("disabled", true);
    return fnOpenModalFirmantes();
    
}); /** Evento para abrir modal de registro de firmantes **/
$('#btnInsertarFirmantes').on('click', function () {
    swal({
        title: "",
        text: "¿Esta seguro que desea agregar el firmante ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            AgregarItemFirmantes(); //=> Generales.js
            return false;

        }
      
        return false;

    });

}); /** Evento para guardar en el modal de registro de firmantes **/
$('#btnCancel').on('click', function () {
    swal({
        title: "",
        text: "¿Esta seguro que desea Cancelar ?",
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
            location.href = $('input[name="pathRoot"]').val() + "/AdministracionMCA/ModuloMCA";
             return false;
           
        }
        else {
            swal("", "", "error");
        }
        return false;
       
    });
   
    return false;  
});



function fncloseModalP() {
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

        $('#formCreateMCA').submit(function () {

            if ($('#nombreModuloMCA').val() == 0 || $('#ruc').val() == 0 || $('#proyectos').val() == 0 || $('#departamentoId').val().length == 0 || $('#municipioId').val().length == 0 || $('#numContrato').val() == 0
       || $('#longuitud').val() == 0 || $('#monto').val() == 0 || $('#fechaConstitucion').val() == 0 || $('#fechaInicio').val() == 0 || $('#fechaFin').val() == 0 || $('#pFiscalInicio').val() == 0 || $('#pFiscalFin').val() == 0) {
                swal('', 'Todos los Datos son requeridos', 'error');
                return false;
            }

            var monto = deleteComas($('#monto').val());


            var rowsFirmantes = $("#jqxgridFirmantes").jqxGrid("getrows");
            if (rowsFirmantes.length <= 0) {
                swal('', 'Ingrese al menos tres Firmantes.', 'info'); return false;
            }

            $('input[name*="listaFirmantes"]').remove(); var k = 0;
            /*** Recorrer el grid y pintar inputs con el HTML ***/
            for (var i = 0; i < rowsFirmantes.length; i++) {
                if (rowsFirmantes[i].usuarioModuloMCAId < 0) {

                    $('#formCreateMCA').append(
                          '<input type="hidden" name="listaFirmantes[' + k + '].usuarioModuloMCAId" value="' + rowsFirmantes[i].usuarioModuloMCAId + '" />'
                        + '<input type="hidden" name="listaFirmantes[' + k + '].usuarioId" value="' + rowsFirmantes[i].usuarioId + '" />'
                       // + '<input type="hidden" name="listaFirmantes[' + k + '].moduloMCAId" value="' + rowsFirmantes[i].apellidos + '" />'
                        + '<input type="hidden" name="listaFirmantes[' + k + '].tipoUsuarioId" value="' + rowsFirmantes[i].tipoUsuarioId + '" />'
                      //  + '<input type="hidden" name="listaFirmantes[' + k + '].cedula" value="' + rowsFirmantes[i].cedula + '" />'

                       // + '<input type="hidden" name="listaRecorridos[' + k + '].observaciones" value="' + rowsRecorridos[i].observaciones + '" />'
                           + '<input type="hidden" name="listaUsuarios[' + k + '].usuarioId" value="' + rowsFirmantes[i].usuarioId + '" />'
                          + '<input type="hidden" name="listaUsuarios[' + k + '].nombres" value="' + rowsFirmantes[i].nombres + '" />'
                          + '<input type="hidden" name="listaUsuarios[' + k + '].apellidos" value="' + rowsFirmantes[i].apellidos + '" />'
                          + '<input type="hidden" name="listaUsuarios[' + k + '].cedula" value="' + rowsFirmantes[i].cedula + '" />'
                        //+ '<input type="hidden" name="listaFirmantes[' + k + '].cedula" value="' + rowsFirmantes[i].cedula + '" />'                         
                    );
                    k = k + 1;
                }
            }

            $('#formCreateMCA').append('<input type="hidden" name="montoTotal" value="' + monto + '" />');
           
            return true;

        });
   
    