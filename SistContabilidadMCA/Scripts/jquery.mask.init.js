$(function () {
    mascaras();
});

/*********************************************************************************************************************
 Definir las mascaras para los input
*********************************************************************************************************************/
function mascaras() {

	//$('.date').mask('00/00/0000');
	$('.time').mask('00:00:00');
	$('.date_time').mask('00/00/0000 00:00:00');
	$('.cep').mask('00000-000');
	$('.phone').mask('0000-0000');
	$('.phone_with_ddd').mask('(00) 0000-0000');
	$('.phone_us').mask('(000) 000-0000');
	$('.mixed').mask('AAA 000-S0S');
	$('.cpf').mask('000.000.000-00', { reverse: true });
	$('.money').mask('000,000,000,000,000.00', { reverse: true });
	$('.money2').mask("#,##0.00", { reverse: true });
	$('.ip_address').mask('0ZZ.0ZZ.0ZZ.0ZZ', {
		translation: {
			'Z': {
				pattern: /[0-9]/, optional: true
			}
		}
	});
	$('.ip_address').mask('099.099.099.099');
	$('.percent').mask('##0,00%', { reverse: true });
	$('.clear-if-not-match').mask("00/00/0000", { clearIfNotMatch: true });
	$('.placeholder').mask("00/00/0000", { placeholder: "__/__/____" });
	$('.fallback').mask("00r00r0000", {
		translation: {
			'r': {
				pattern: /[\/]/,
				fallback: '/'
			},
			placeholder: "__/__/____"
		}
	});

	$('.selectonfocus').mask("00/00/0000", { selectOnFocus: true });
	$('.cedula').mask("999-999999-9999S", { selectOnFocus: true, placeholder: "____-______-_____" });
	$('.cedulaResidencia').mask("S99999999", { selectOnFocus: true, placeholder: "_________" });
    //$('.ruc').mask("9999-999999-9999", { selectOnFocus: true, placeholder: "____-______-____" });
	$('.ruc').mask("rrrrrrrrrrrrrrrrrr", {
	    placeholder: "__________________",
	    translation: {
	        'r': {
	            pattern: /^[a-zA-Z0-9\-]*$/  /*/[a-zA-Z]/	*/
	        },
	       
	    }
	});

	$('.only_number').mask("#");
	$('.phone2').mask('0000-0000,0000-0000,0000-0000');

    	$('.decimalSincoma').keypress(function (key) {
	    var keycode = (key.which) ? key.which : key.keyCode;

	    if (keycode == 9){
	        return true;
	    }	        
	    else if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
	        return false;
	    }	    
	    else {

	        var parts = $(this).val().split('.');
	        if (parts.length > 1 && keycode == 46)
	            return false;
	        return true;
	    }
	}).blur(function () {
	    if ($(this).val() != null && isNaN($(this).val()) == false) {

	        if ($.trim($(this).val()) != '') {
	            var num = parseFloat($(this).val());
	            var cleanNum = num.toFixed(2);

	            if (!isNaN(cleanNum)) {
	                $(this).val(cleanNum);
	            }
	        }
	    }
	});

	$('.decimalComma').keypress(function (key) {
	    var keycode = (key.which) ? key.which : key.keyCode;

	    if (keycode == 9) {
	        return true;
	    }
	    else if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
	        return false;
	    }	    
	    else {
	        var parts = $(this).val().split('.');
	        if (parts.length > 1 && keycode == 46)
	            return false;
	        return true;
	    }
	}).blur(function () {
	    if ($(this).val() != null && isNaN($(this).val()) == false) {
	        if ($.trim($(this).val()) != '') {
	            var num = $.trim($(this).val());
	            var cleanNum = num.toString().replace('%', '').replace(new RegExp(",", "g"), "");

	            if (!isNaN(cleanNum)) {
	                var cleanNum2 = parseFloat(cleanNum).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
	                $(this).val(cleanNum2);
	            }
	        }
	    }
	    return true;
	});
}