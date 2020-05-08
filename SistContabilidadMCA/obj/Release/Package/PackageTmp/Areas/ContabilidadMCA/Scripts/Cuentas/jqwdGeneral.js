
/********************************
*Funcion principal
*****************************/
$(document).ready(function () {
 

});

/********************************
*Funcion de carga inicial del Grid
*****************************/
function CargarGrid(source, columns) {
    
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
