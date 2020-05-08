//var CONTADOR = 0;
///********************************
//*Funcion principal
//*****************************/
$(document).ready(function () {
   // fnCargaGrid();
//    SaveMCA();
    $(".Modificacion").each(function (i, k) {
        if ($("#Eval").val() == "True") {
            $(this).prop('disabled', true);//.addClass('disabled');
            $(this).prop('readonly', true);

        } else {
            $(this).prop('disabled', false);//.addClass('disabled');
            $(this).prop('readonly', false);
        }

    });
    var tipoFormulario = $('#tipoFormulario').val();

    if (tipoFormulario == 2) {
        $('#Solicitud').on(setTimeout(function () {
            //Code
            location.href = $('input[name="pathRoot"]').val() + "/AdministracionMCA/ModuloMCA/Index";
        }, 1500));
    }

});

$('#btnAddFirmantes').on('click', function () { return fnOpenModalFirmantes(); }); /** Evento para abrir modal de registro de firmantes **/
$('#btnInsertarFirmantes').on('click', function () { return AgregarItemFirmantes(); }); /** Evento para guardar en el modal de registro de firmantes **/
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


function saveMCASubmitE() {
    //$('#formEditMCA').submit(function () {

        //var monto = deleteComas($('#monto').val());
        $('#monto').val(deleteComas($('#monto').val()));
        $('#longuitud').val(deleteComas($('#longuitud').val()));

        var rowsFirmantes = $("#jqxgridFirmantes").jqxGrid("getrows");
        if (rowsFirmantes.length <= 0) {
            swal('', 'Ingrese al menos tres Firmantes.', 'info'); return false;
        }

        $('input[name*="listaFirmantes"]').remove(); var k = 0;
        /*** Recorrer el grid y pintar inputs con el HTML ***/
        for (var i = 0; i < rowsFirmantes.length; i++) {
            if (rowsFirmantes[i].usuarioModuloMCAId < 0) {

                $('#formEditMCA').append(
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
        $('#formEditMCA').submit();
      //  return true;
        
    //});

    return false;
}



$("#btnGuardarE").on("click", function () {
    
    swal({
        title: "",
        text: "¿Esta seguro que desea guardar los datos Modulo MCA?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            //$('#formCreateMCA').submit();
            saveMCASubmitE();
        }
        return false;
    });
});
