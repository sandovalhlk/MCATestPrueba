$(document).ready(function () {
    //  ListComprobantes();
});

$("#btnAnulacion").on("click", function () {

    var sms = "";
    sms = sms + ($("#razonAnulacion").val().length <= 5 ? "Ingrese una razon de anulacion valida \n" : "");

    if ($.trim(sms) != "") {
        swal("", sms, "warning"); return false;
    } else
    {
        swal({ title: "", text: "¿ Esta Seguro que desea anular este comprobante? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
      function () {
          fnOpenModal(modalAnulacion, false, false);
          AnularComprobante();
      });

    }
        
});

function fnAnularComprobante(comprobanteId) {

    swal({ title: "", text: "¿ Esta Seguro que desea anular este comprobante? ", type: "warning", cancelButtonText: "Cancelar", showCancelButton: true, confirmButtonText: "Si", closeOnConfirm: true, },
        function () {
            $("#razonAnulacion").val("")
            $("#varComprobanteId").val(comprobanteId);
            fnOpenModal(modalAnulacion, true, false);
        });
}



function AnularComprobante() {
    var info = {
        __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(), comprobanteId: $("#varComprobanteId").val(),
        razonAnulacion: $("#razonAnulacion").val()
    }
    var url = $('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/AnularComprobante';
    $.post(url, info, function (result) {
        setTimeout(function () {
            if (result == "") {
                ListComprobantes.Refresh();
                swal("", "El proceso de anulación se ejecuto satisfactoriamente", "success");
            } else
                swal("", result, "error")
        }, 1000);
       
    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
    //    return true;
}
