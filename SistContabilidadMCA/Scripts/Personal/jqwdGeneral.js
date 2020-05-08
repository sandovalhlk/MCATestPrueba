var CONTADOR = 0;
/********************************
*Funcion principal
*****************************/
$(document).ready(function () {
   // GetListComprobante();
    //// SaveMCA();
    //$(".Modificacion").each(function (i, k) {
    //    $(this).prop('disabled', false);//.addClass('disabled');
    //    $(this).prop('readonly', false);
    //});

});

/********************************
*Funcion de carga inicial del Grid
*****************************/
function CargarGrid(source, columns) {
    // var url = $('input[name=pathRoot]').val() + '/AdministracionMCA/ModuloMCA/GetListaFirmantes?moduloMCAId=' + ($('#moduloMCAId').length <= 0 ? 0 : $('#moduloMCAId').val());
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#jqwgMovimientos").jqxGrid(
    {
        width: '100%',
        height: '200px',
        sortable: false,
        source: dataAdapter,
        editable: true,
        showfilterrow: false,
        filterable: false,
        theme: 'bootstrap',
        // autoshowfiltericon: false,
        selectionmode: 'multiplerowsextended',
        pageable: false,
        columnsresize: true,
        showstatusbar: true,
        statusbarheight: 25,
        showaggregates: true,

        ready: function () {
            $("#jqwgMovimientos").jqxGrid('localizestrings', localizationobj);
        },
        columns
    });
}
