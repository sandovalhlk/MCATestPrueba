$(document).ready(function () {
    localStorage.setItem("AnualInput", $("#cierreAnualInput").val());

    $("#cierreAnualCk").change(function () {
        if ($('#cierreAnualCk').is(':checked')) {
            $('#cierreAnualInput').val(1);
            console.log($('#cierreAnualInput').val());
        } else {
            //$('#cierreAnualInput').val(1);
            $("#cierreAnualInput").val(localStorage.getItem("AnualInput"));
        }
    });
});

function validarCierresExistentes() {
    var validar = "";
    var info =
              {
                  __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
                  fechaTrans: $('input[name=fecha1]').val(), moduloMCAId: $("#moduloMCAId").val()

              };
    $.post($('input[name=pathRoot]').val() + '/ContabilidadMCA/Cuentas/validarCierresExistentes', info, function (result) {

        if (result != "") {
            swal('', result, 'error');
        } else {
            $('form').submit();
        }
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
}


