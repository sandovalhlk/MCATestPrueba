$(function () {

    Eventos();
});



function Eventos() {
    $('form').submit(function () {

        var sms = "";
        sms = sms + ($('input[name=fecha1]').val().length <= 0 ? "Seleccione la fecha de inicio \n" : "");

        if ($.trim(sms) != "") {
            swal("", sms, "warning"); return false;
        }
    });
}