$(function () {

   // Eventos();
});



//function Eventos() {
//    $('form').submit(function () {

//        var sms = "";
//        sms = sms + ($('input[name=fecha1]').val().length <= 0 ? "Seleccione la fecha de inicio \n" : "");

//        if ($.trim(sms) != "") {
//            swal("", sms, "warning"); return false;
//        }
//    });
//}


$("#btnGenerarRep").on('click', function () {
    var sms = "";
    sms = sms + ($('input[name=fecha1]').val().length <= 0 ? "Seleccione la fecha de inicio \n" : "");

    if ($.trim(sms) != "") {
        swal("", sms, "warning"); return false;
    }
    validarCierresExistentes();

});


$("#fecha1").on("change", function () {
    if ($("#fecha1").val() != "") {
        if ($("#fecha1").val().substring(5, 7) == 12) {
            $('.cierreAnual').css('display', 'block');
        
            $('.checkbox input').prop('checked', false);
        }
        else {
        
            $('.checkbox input').prop('checked', false);
            $('.cierreAnual').css('display', 'none');
        }
    }
});