$(function () {

  //  Eventos();
});




$("#btnGenerarRep").on('click', function () {
    var sms = "";
    sms = sms + ($('input[name=fecha1]').val().length <= 0 ? "Seleccione la fecha de inicio \n" : "") + ($('input[name=fecha2]').val().length <= 0 ? "Seleccione la fecha de Fin \n" : "");

            if (!$("#ckTodas").prop('checked')) {
                sms = sms + ($('input[name=codigo1]').val().length <= 0 ? "Seleccione el rango de cuenta \n" : "");
                sms = sms + ($('input[name=codigo2]').val().length <= 0 ? "Seleccione el rango de cuenta \n" : "");
            }
            if ($.trim(sms) != "") {
                swal("", sms, "warning"); return false;
            }


    if ($.trim(sms) != "") {
        swal("", sms, "warning"); return false;
    }


    validarCierresExistentes();
});

//VALIDAR CODIGOS DE CUENTAS
$(".codigoC").on("focusout", function () {
    var idCodigo = $(this).attr('id');// $(this).na
    if (!$("#ckTodas").prop('checked')) {
        //  alert($(this).val());

        var info = {
            __RequestVerificationToken: $('[name= "__RequestVerificationToken"]').val(),
            codigo: $(this).val(),
            nivel: 5
        };
            $.post($('input[name="pathRoot"]').val() + '/ContabilidadMCA/Cuentas/ValidarCodCuenta', info, function (result) {
                if(result!="")
                {
                    swal("", result, "error");
                    $("#"+ idCodigo).val("");
                }
                    
                return false;
            }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
            return false;        
    }

});



$("#ckTodas").on("click", function () {
    if ($("#ckTodas").prop('checked'))
    {
        $("#codigo1").prop('readonly', true);
        $("#codigo2").prop('readonly', true);
        $("#todas").val(1);
    } else {
        $("#codigo1").prop('readonly', false);
        $("#codigo2").prop('readonly', false);
        $("#todas").val(0);
    }
});


var options = {

    url: function (phrase) {
        return $('input[name=pathRoot]').val() + "/ContabilidadMCA/Cuentas/GetListCuentas";
    },

    getValue: function (element) {
        return element.codigo;
    },

    ajaxSettings: {
        dataType: "json",
        method: "POST",
        data: {
            dataType: "json"
        }
    },
    preparePostData: function (data) {
        data.phrase = $("#codigo1").val();
        return data;
    },
    list: {
        onSelectItemEvent: function () {
          
        }
    },

    requestDelay: 400
};
$("#codigo1").easyAutocomplete(options);


var options2 = {

    url: function (phrase) {
        return $('input[name=pathRoot]').val() + "/ContabilidadMCA/Cuentas/GetListCuentas";
    },

    getValue: function (element) {
        return element.codigo;
    },

    ajaxSettings: {
        dataType: "json",
        method: "POST",
        data: {
            dataType: "json"
        }
    },
    preparePostData: function (data) {
        data.phrase = $("#codigo2").val();
        return data;
    },
    list: {
        onSelectItemEvent: function () {

        }
    },

    requestDelay: 400
};
$("#codigo2").easyAutocomplete(options2);


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

