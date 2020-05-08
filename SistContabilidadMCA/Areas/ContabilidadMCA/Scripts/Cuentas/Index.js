/*****************************************************************************
 * Carga Inicial
 *****************************************************************************/
$(document).ready(function () {
    $('#btnModalGuardar').on('click', function () {
        SaveCuenta();
    });

    $('#btnGenerarCatalogo').on('click', function () {
        CrearAnularCatalogoCuentas(1);
    });
});


$("#btnCerrar").on("click", function () {
    swal({ title: "", text: "¿ Esta Seguro que desea cerrar esta pantalla? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
           function () {

               fnCloseModal("#ModalAddEditCuenta");
               return false;
           });
    return false;
});

/*****************************************************************************
     * Función para Crear o anular un catalgo de cuentas en base al usuario que ingreso y perteneciente a un determinado modulo
     *  Esta opcion solo sera visualizada por el usuario con el rol de contardor.
 *****************************************************************************/
function CrearAnularCatalogoCuentas(option) {

    var mensaje = "";

    swal({ title: "", text: "¿ Esta Seguro que desea Crear el Catalogo de cuentas? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: false, },
            function () {

                var info =
                    {
                        __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                        opcion: $.trim(option)
                    };
                 $.blockUI({ css: { 
            border: 'none', 
            padding: '15px', 
            backgroundColor: '#000', 
            '-webkit-border-radius': '10px', 
            '-moz-border-radius': '10px', 
            opacity: .5, 
            color: '#fff' 
        } }); 
                $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/CrearAnularCatalogo', info, function (result) {
                    $.unblockUI();
                    if (result.band == true) {
                        swal("", result.mensaje, "success");
                        console.log(result);
                        
                        setTimeout(function () {
                            location.href = $('input[name="pathRoot"]').val() + "/ContabilidadMCA/Cuentas/Index"
                        }, 1200);
                        return false;
                    }else
                        swal("Ocurrio un Problema: ", result.mensaje, "error");
                  
                }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
                return false;
                $.unblockUI();

            });
}

/*****************************************************************************
 * Función para Crear Editar las Cuentas
 *****************************************************************************/
//TIENE QUE SER AL DAR CLICK EN EL BOTON DE GUARDAR DE LA VISTA PARCIAL
function AddEditCuenta(cuentaId, jerarquia) {
    if ($('#btnCuenta').val() != "") {
        var cuentaId = cuentaId; 
        var jerarquia = jerarquia;

        var url = $('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/AddEditCuenta';
        var parameter = {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
            cuentaId: cuentaId,
            jerarquia: jerarquia
        };
        $.post(url, parameter, function (data) {
            if ($.trim(data.sms) != "") { swal('', data.sms, 'error'); return false; }
            if ($.trim(data.msj) != "") { swal('', data.msj, 'error'); return false; }

            fnOpenModal('#ModalAddEditCuenta', true, false);

            $('#moduloMCA').val(data.toCuenta[0].nombreModuloMCA);
            $('#naturaleza').val(data.toCuenta[0].naturaleza);
            $('#tipoCuenta').val("DETALLE"); //data.toCuenta[0].tipoCuenta -->LAS CUENTAS QUE SE AGREGARAN SON UNICAMENTE DETALLE

            if (jerarquia > 0) {//edicion
                $('#codigoPadre').val(data.toCuenta[0].codigoPadre);
                $('#cuentaPadre').val(data.toCuenta[0].descripcionPadre);
                $('#codigo').val(data.toCuenta[0].codigo);
                $("#descripcion").val(data.toCuenta[0].descripcion);
                $("#cuentaPadreId").val(data.toCuenta[0].cuentaPadreId);
                $("#cuentaId").val(data.toCuenta[0].cuentaId);
                $("#Cuentabanco").prop("checked",data.toCuenta[0].Cuentabanco);
            } else if (jerarquia == -1) {//insercion
                $("#Cuentabanco").prop("checked", false);
                
                $('#codigoPadre').val(data.toCuenta[0].codigo);
                $('#cuentaPadre').val(data.toCuenta[0].descripcion);
               
                var cod2 = 0;
                    if(data.toCuenta[0].codigoHijo == null){
                        cod2 = 1;
                    } else { 
                        cod2 = parseInt(data.toCuenta[0].codigoHijo.split('-')[data.toCuenta[0].codigoHijo.split('-').length - 1]) + 1
                    }

                    cod2 = cod2 > 9 ? cod2 : "0" + cod2;                   
                    var cod = data.toCuenta[0].codigo + ('-' + cod2);

                $('#codigo').val(cod); 
                $("#descripcion").val('');
                $("#cuentaPadreId").val(data.toCuenta[0].cuentaId);
            }

        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { console.log(textStatus + ": " + XMLHttpRequest.responseText); });
        return false;

    }
}


/*****************************************************************************
 * Función para Guardar los datos de la Cuenta
 *****************************************************************************/
function SaveCuenta() {

    if (!($("#descripcion").val().length >0)) {
        swal("", "El nombre de la cuenta no puede ir vacio", "warning");
        return false;
    }

    swal({ title: "", text: "¿ Esta Seguro que desea Guardar los datos de esta cuenta? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
        function () {

            var info =
                {
                    __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                    cuentaPadreId: $.trim($("#cuentaPadreId").val()),
                    cuentaId: $.trim($("#cuentaId").val()),
                    codigo: $.trim($("#codigo").val()),
                    descripcion: $.trim($("#descripcion").val()),
                    Cuentabanco: $('#Cuentabanco').is(':checked'),
                };
            $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/SaveCuenta', info, function (result) {
                if ($.trim(result.mensaje) != "") {
                    swal("", result.mensaje, "warning");
                    return false;
                }
                setTimeout(function () {
                    swal("", "El catalogo de cuentas fue Actualizado Exitosamente!!", "success");
                },1000);
                
                fnCloseModal('#ModalAddEditCuenta');
                TreeList.ExpandAll();
                return false;
            }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
            return false;
        });
}


/*****************************************************************************
 * Función para Anular las Cuentas
 *****************************************************************************/
function NullCuentas(cuentaId) {
    swal({ title: "Anulacion de Cuentas", text: "¿ Esta Seguro que desea Anular esta Cuenta?", type: "input", showCancelButton: true, closeOnConfirm: false, animation: "slide-from-top", inputPlaceholder: "Razon de Anulacion", cancelButtonText: "Cancelar", confirmButtonText: "Si" },
        function (inputValue) {
            if (inputValue === false) return false;

            if (inputValue === "") {
                swal("", "Se necesita una Razon de Anulacion!!", "error");
                return false;
            }

            var info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), cuentaId: cuentaId, razon: inputValue };
            $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/NullCuentas', info, function (result) {
                if ($.trim(result.mensaje) != "") {
                    swal("", result.mensaje, "warning");
                    return false;
                }
                swal("", "La cuenta fue Anulada Exitosamente!!", "success");
                TreeList.ExpandAll();
            }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
        });
}

$('#btnPrintCatalogo').on('click', function () {
    if (TreeList.rowCount > 0)
        location.href = $('input[name="pathRoot"]').val() + "/Informes/TipoReportes/GenerarCatalogoCuenta";
    else
        swal("","El catalogo de cuentas esta vacio","error");
});