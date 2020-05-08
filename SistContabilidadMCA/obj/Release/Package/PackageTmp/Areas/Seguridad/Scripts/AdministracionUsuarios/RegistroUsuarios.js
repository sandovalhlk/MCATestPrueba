/**********************************************************************************************************************************************
 * Función General
 **********************************************************************************************************************************************/
$(document).ready(function () {
    fnRegistrarUsuarios();

  
});

//$('select#RolId').on('change', function (e) { fnSetUCR($(this).val()); });
$('select#RolId').on('change', function (e) { fnSetUCR($("select#RolId option:selected").text()); });


function fnSetUCR(rol) {
    if (rol == "ADMINISTRADOR")
    {
        $("#ucrId").val(4);
        $("#ucrId").change();
    }else
    {
        $("#ucrId").val("");
        $("#ucrId").change();
    }
    
}

/**********************************************************************************************************************************************
 * Función para guardar nuevo registro
 **********************************************************************************************************************************************/
function fnRegistrarUsuarios() {

    $("#formRegistroUsuario").submit(function () {
        var IdRol = $('input[name="Rol"]:checked').val();
        
        swal({
            title: "",
            text: "¿Esta seguro que desea Registrar el Usuario?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Si, Continuar",
            cancelButtonText: "No, Cancelar",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {

                if (!(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test($.trim($('#Email').val())))) {
                    swal("", "EL correo eletronico ingresado no es correcto", "error");
                    return false;
                }

            var url = $('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/SaveUser';
            var parameter = {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                usuarioMCAId: $.trim($('#usuarioId').val()),
                UserName: $.trim($('#nombreUser').val()),
                Password: $.trim($('#contraseña').val()),
                Email: $.trim($('#Email').val()),
                RolId: $('#RolId').val(),
                ucrId: $('#ucrId').val(),
            };
            $.post(url, parameter, function (data) {
                if ($.trim(data.mensaje) != "") {
                    swal("", data.mensaje, "error");
                    setTimeout(function () {
                        console.log(data.mensaje)
                      //swal("", data.mensaje, "success")
                    }, 1500);
                    return false;
                }
                swal("", "El usuario, fue registrado exitosamente.", "success");
                setTimeout(function () {
                    //console.log(data.mensaje)
                    location.href = $('input[name="pathRoot"]').val() + "/Seguridad/AdministracionUsuarios/RegistroUsuarios"
                }, 1500);



            return false;
        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
        return false;
            }
            else {
                swal("Cancelado", "", "error");
            }
            return false;
        });
        return false;

    });
}


/**********************************************************************************************************************************************
 * Función para verificar si la Persona de la Directiva ya esta regitrado.
 **********************************************************************************************************************************************/
$('#btnCargarCedula').on('click', function () {
    if ($('#cedula').val().length == 16) {
        var url = $('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/GetUsuario';
        var parameter = {
            __RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
            cedula: $.trim($("#cedula").val())
        };

        $.post(url, parameter, function (data) {
            if ((data.toPersona == 0)) {
                swal("", "El número de cedula digitado no se encuentra registrado, por favor continúe el proceso.", "info");
                $("#nombres").prop("disabled", false);
                $("#apellidos").prop("disabled", false);
                $("#btnGuardarPersonalDirectiva").prop("disabled", false);
                return false;
            }
            /***Persona Directiva ya registrado***/
            fnOpenModal('#ModalPersonalDirectiva', false, false)
            swal("", "La Persona que desea agregar, ya se encuentra registrada", "warning");
            $("#nombreUser").val(data.toPersona[0].nombres + ' ' + data.toPersona[0].apellidos);
            $('#usuarioId').val(data.toPersona[0].usuarioId).trigger("change.select2");

        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
        return false;
    }
    else {
        swal("", "Verifique el número de Cédula digitado.", "warning");
    }
});


/**********************************************************************************************************************************************
 * Función para guardar Personal Directiva
 **********************************************************************************************************************************************/
$("#btnGuardarPersonalDirectiva").on('click', function () {

    if ($('#nombres').val() == 0 || $('#apellidos').val().length == 0) {
        swal('', 'Todos los campos son requeridos', 'error');
        return false;
    }

    nombres = $('#nombres').val().toUpperCase();
    apellidos = $('#apellidos').val().toUpperCase();
    cedula = $('#cedula').val();
    var url = $('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/SavePersonalDirectiva';
    var parameter = {
        nombres: nombres,
        apellidos: apellidos,
        cedula: cedula,
    };
    $.post(url, parameter, function (data) {
        if ($.trim(data.mensaje) == "") {
            swal("", "La persona fue registrado exitosamente.", "success");
            GetComboPersonalDirectiva();
            fnOpenModal('#ModalPersonalDirectiva', false, false);
        }
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
    return false;
});


/**********************************************************************************************************************************************
 * Función refresca combo Personal Directiva
 **********************************************************************************************************************************************/
function GetComboPersonalDirectiva() {
    //borrar combo antiguo
    var modeloId = $('select[name=usuarioId]');
    modeloId.html('').text('').append('<option value="">Seleccione el Usuario</option>').select2('destroy').select2();
    //Carga de Datos
    var url = $('input[name="pathRoot"]').val() + '/Seguridad/AdministracionUsuarios/GetComboPersonalDirectiva';
    $.getJSON(url, function (data) {
        //Crear Nuevo Combo
        $.each(data, function (i, jsondata) {
            modeloId.append('<option value="' + jsondata.usuarioId + '">' + jsondata.nombres + '</option>').trigger("change.select2");
        });
    });
}

function validarEmail(valor) {
    if (!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3,4})+$/.test(valor))) 
        {

        }
        //alert("La dirección de email " + valor + " es correcta.");
    //} else {
    //    alert("La dirección de email es incorrecta.");
    //}
}

