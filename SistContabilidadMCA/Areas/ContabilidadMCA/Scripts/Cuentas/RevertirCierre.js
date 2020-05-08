$(document).ready(function () {

});

$('select#moduloMCAId').on('change', function (e) {
    if ($('select#moduloMCAId').val() > 0)
    {
        info = { __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),  moduloMCAId: $("#moduloMCAId").val() };
        $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/GetCierres', info, function (result) {
            if (result.length >0 )
            {
                
                $("#cierreId").html('').text('').append('<option value="">Seleccione el cierre </option>').trigger("change.select2");
                $.each(result, function (i, item) {
                    $("#cierreId").append('<option value="' + item.cierreId + '">' + item.cierre + '</option>').trigger("change.select2");
                });
            }else
            {
                $("#cierreId").html('');
                swal("", "El MCA Seleccionado no posee cierres", "warning");
            }

        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    }

});

$("#btnReversionCierre").on("click",function () {

    swal({ title: "", text: "¿ Esta Seguro que desea Ejecutar la reversion de Asentamiento? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
        function () {

            if (!($("#cierreId").val() > 0) || $("#razonReversion").val().length<5)
            {
                setTimeout(function () {
                    swal("", "SELECCIONE UN CIERRE E INGRESE LA RAZON DE LA REVERSION", "error")
                }, 500);
                
                return false;
            }
            var info =
               {
                   __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                   cierreInicio: $("#cierreId").val(), razonReversion: $("#razonReversion").val(),
                   moduloMCAId: $('#moduloMCAId').val()

               };
            //$.blockUI({
            //    css: {
            //        border: 'none',
            //        padding: '15px',
            //        backgroundColor: '#000',
            //        '-webkit-border-radius': '10px',
            //        '-moz-border-radius': '10px',
            //        opacity: .5,
            //        color: '#fff'
            //    }
            //});
            $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/RevertirCierre', info, function (result) {
              //  $.unblockUI();
                if (result == "") {
                    setTimeout(function () {
                        swal({
                            title: "",
                            text: "La reversion de cierre se ejecuto satisfactoriamente",
                            type: "success"
                        }, function () {
                            location.href = $('input[name="pathRoot"]').val() + "/Home/Index";
                        });
                    }, 1000);
                    
                } else {
                    setTimeout(function () {
                        swal("", result, "error")
                    }, 1200);
                }

            }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
            return false;
           // $.unblockUI();
        });

});

$('#btnCancel').on('click', function () {
    swal({
        title: "",
        text: "¿Esta seguro que desea Cancelar ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Continuar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
           // swal("", "", "success");
            location.href = $('input[name="pathRoot"]').val() + "/Home/Index";
            return false;

        }
        else {
            swal("", "", "error");
        }
        return false;

    });

    return false;
});