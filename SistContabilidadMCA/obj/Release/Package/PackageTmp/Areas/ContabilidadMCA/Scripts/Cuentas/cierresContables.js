var tipoCierreId = 0;
$(document).ready(function () {

});

$('#btnCierreMensual').on('click', function () {
    tipoCierreId = 1;
    fnOpenModal('#modalCierre', true, false);   
});

$('#btnCierreAnual').on('click', function () {
    tipoCierreId = 2;
    fnOpenModal('#modalCierre', true, false);
});

$('#btnCierreContable').on('click', function () {
    tipoCierreId = 3;
    fnOpenModal('#modalCierre', true, false);
});

$('#btnEjecutarCierre').on('click', function () {
    if ($('#fechaTransaccion').val().length > 0)
    {
        fnOpenModal('#modalCierre', false, false);
        EjecutarCierre();
    }
    else
        swal("","Defina una fecha valida","error");    
});

$('#btnCalcularSaldos').on('click', function () {

    //swal({ title: "", text: "¿ Esta Seguro que desea Ejecutar el Calculo de Saldos? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
    //    function () {

    //        var info =
    //           {
    //               __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
    //               tipoCierreId: 1, fechaTransaccion: $('#ultimoCierre').val(), cierreOficial: 3 

    //           };
    //        $.blockUI({
    //            css: {
    //                border: 'none',
    //                padding: '15px',
    //                backgroundColor: '#000',
    //                '-webkit-border-radius': '10px',
    //                '-moz-border-radius': '10px',
    //                opacity: .5,
    //                color: '#fff'
    //            }
    //        });
    //        $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/EjecutarCierre', info, function (result) {
    //            $.unblockUI();
    //            if (result == "") {
    //                setTimeout(function () {
    //                    swal("", "El cierre se ejecuto satisfactoriamente", "success")
    //                }, 1200);
    //                fnOpenModal('#modalCierre', false, false);
    //            } else {
    //                setTimeout(function () {
    //                    swal("", result, "error")
    //                }, 1200);
    //                fnOpenModal('#modalCierre', false, false);
    //            }
    
    //        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    //        return false;
    //        $.unblockUI();
    //    });

    CalcularSaldos(1);

});

$('#btnCalculoAnual').on('click', function () {
CalcularSaldos(2);
});

$('#btnCalculoTotal').on('click', function () {
    CalcularSaldos(3);
});

function EjecutarCierre() {

    swal({ title: "", text: "¿ Esta Seguro que desea Ejecutar el Cierre del Mes? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
        function () {

            var meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
            var mesAnio = meses[getFecha($('#fechaTransaccion').val()).getMonth()+1] + " " + getFecha($('#fechaTransaccion').val()).getFullYear();
            var info =
               {
                   __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                   tipoCierreId: tipoCierreId, fechaTransaccion: $('#fechaTransaccion').val(), cierreOficial: 4

               };
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });
            $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/EjecutarCierre', info, function (result) {
                $.unblockUI();
                if(result=="")
                {
                    $("#mesCierreView").text(mesAnio);
                    setTimeout(function () {
                        swal("", "El cierre se ejecuto satisfactoriamente", "success")
                    }, 1200);
                    fnOpenModal('#modalCierre', false, false);
                } else {
                    setTimeout(function () {
                        swal("", result, "error")
                    }, 1200);
                    fnOpenModal('#modalCierre', false, false);
                }
                    

            }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
            return false;
            $.unblockUI();
        });

}

function CalcularSaldos(tipoCierreId) {
    swal({ title: "", text: "¿ Esta Seguro que desea Ejecutar el Calculo de Saldos? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
           function () {

               if (tipoCierreId == 2) {
                   $('#ultimoCierre').val($('#anioCierre').val())
               }
               var info =
                  {
                      __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                      tipoCierreId: tipoCierreId, fechaTransaccion: $('#ultimoCierre').val(), cierreOficial: 3

                  };
               $.blockUI({
                   css: {
                       border: 'none',
                       padding: '15px',
                       backgroundColor: '#000',
                       '-webkit-border-radius': '10px',
                       '-moz-border-radius': '10px',
                       opacity: .5,
                       color: '#fff'
                   }
               });
               $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/EjecutarCierre', info, function (result) {
                   $.unblockUI();
                   if (result == "") {
                       setTimeout(function () {
                           swal("", "El cierre se ejecuto satisfactoriamente", "success")
                       }, 1200);
                       fnOpenModal('#modalCierre', false, false);
                   } else {
                       setTimeout(function () {
                           swal("", result, "error")
                       }, 1200);
                       fnOpenModal('#modalCierre', false, false);
                   }


               }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
               return false;
               $.unblockUI();
           });
}