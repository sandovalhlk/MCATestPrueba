/**********************************************************************************************************************************************
 * Función para abrir modales
 * 
 * @param > modalhtml, nombre del modal
 * @param > isopen , booleado para abrir o cerrar modal
 * @param > iseditar , booleado para saber si es edicion o nuevo registro
 * @param > idHTMLLlava , referencia al html del hiden para capturar el valor del hiden
 * @param > valorIdLlavaPrimaria , valor del hiden de llave primaria
 * 
 **********************************************************************************************************************************************/
function fnOpenModal(modalhtml, isopen, iseditar, idHTMLLlava, valorIdLlavaPrimaria) {

    if (isopen) /** abrir modal **/ {
        $(modalhtml).find('.text-danger').each(function (i, e) { $(this).html(''); });

        $(modalhtml).find('form').each(function (i,e) {
            $(this).jqxValidator('hide');
        })

        $('.jqx-validator-hint').each(function () { $(this).remove() })
        if (iseditar == false) {

            $(modalhtml).find('input[type=hidden]').each(function (i, e) { $(this).val('0'); });
            $(modalhtml).find('select').each(function (i, e) { $(this).val('').trigger("change.select2"); });
            $(modalhtml).find('.selectvaluegrid').each(function (i, e) { $(this).val('').remove(); }); /* remover los elementos del selectcomboxgrid */
            $(modalhtml).find('input[type=text],textarea').each(function (i, e) { $(this).val(''); });
        }
        else {
            if (idHTMLLlava != null) {
                $(idHTMLLlava).val(valorIdLlavaPrimaria);
            }
        }
        if (valorIdLlavaPrimaria == null) {
            $(idHTMLLlava).val('0');
        }
        $(modalhtml).modal('show');
      //  LimpiarInputClass();
    }
    else
        $(modalhtml).modal('hide');

    return false;
}
/**********************************************************************************************************************************************
 * Función para Cerrar modales
 * 
 **********************************************************************************************************************************************/

function fnCloseModal(modalhtml)
{
    $(modalhtml).modal('hide');
   // LimpiarInputClass();
}
