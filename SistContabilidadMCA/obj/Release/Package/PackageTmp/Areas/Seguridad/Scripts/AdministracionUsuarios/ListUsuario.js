$(document).ready(function(){

});

$('#btnUserAct').on('click', function () {
    ListUsuarios(1);
    $("#estadoIdConsult").val(1);
});

$('#btnUserInact').on('click', function () {
    ListUsuarios(2);
    $("#estadoIdConsult").val(2);
});


$('#btnModUser').on('click', function () {
    SaveUsuario();
});

$('#desbloquear').on('click', function () {
    Desbloquear();
});


function EditUser(usuarioId) {
    $('#usuarioId').val(usuarioId);
    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), usuarioId: usuarioId };
    $.post($('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/EditUser', info, function (result) {
        if (jQuery.isEmptyObject(result)) {
            $("#listUsuarios").hide();
            swal("", "Ocurrio un Error", "error");
            return false;

        }
        fnOpenModal(modalModificarUser, true, false);
        $('#login').val(result.login);
        $('#estadoId').val(result.estadoId);
        $('#Id').val(result.rolId);
        $('#bloqueado').val(result.bloqueado);
        FnRefrescarUsuarios()
       
        return false;
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    return false;
}

function SaveUsuario() {
    var info = {
        __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
        usuarioId: $('#usuarioId').val(),
        rolId: $('#Id').val(),
        estadoId:$('#estadoId').val()
    };
    $.post($('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/SaveUsuario', info, function (result) {
        
        if (result!="")
        {
            swal("", result, "error");
            return false;
        }
        setTimeout(function () { "", 1500
        });
        swal("", "El usuario se actualizo satisfactoriamente", "success");
        fnOpenModal(modalModificarUser, false, false);
        FnRefrescarUsuarios()
       

        return false;
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    return false;
}

function ListUsuarios(estadoId) {
    //var url = $('input[name="pathRoot"]').val() + "/Vehiculos/Vehiculos/BuscarVehiculo";
    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), estadoId: estadoId };
    $.post($('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/ListUsuarios', info, function (result) {
        if (jQuery.isEmptyObject(result)) {
            $("#listUsuarios").hide();
            swal("", "No se encontraron usuarios", "warning"); return false;

        }
        $('#listUsuarios').html('');
        $('#listUsuarios').html(result);
        //$("select").select2();

        ////PermisoModificacion
        $('#btnSeleccion').hide();

    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
}


/**********************************************************************************************************************************************
 * Función actualizar Grid GetListUsuarios
 **********************************************************************************************************************************************/
function FnRefrescarUsuarios() {
    var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), estadoId: $('#estadoIdConsult').val() };
    $.post($('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/ListUsuarios', info, function (result) {
       
        $('#listUsuarios').html('');
        $('#listUsuarios').html(result);
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });

}

function Desbloquear() {
    var info = "";
    info = $('#bloqueado').val() == "true" ? "Bloquear" : "Desbloquear";
    
    swal({ title: "", text: "¿ Esta Seguro que desea " + info +" este usuario ? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
      function () {

          var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), login: $('#login').val(), bloqueado: $('#bloqueado').val() };
          $.post($('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/Desbloquear', info, function (result) {
        if (jQuery.isEmptyObject(result)) {
            swal("", "Ocurrio un error en el proceso", "error");
           // return false;
        } else {
            setTimeout(function () {
                swal("", result, "info")
            }, 1200);
            
           
        }
        fnOpenModal(modalModificarUser, false, false);
        ListUsuarios.Refresh()();
        return false;
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    return false;
      });
}