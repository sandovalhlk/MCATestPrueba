var CONTADOR = 0;
/********************************
*Funcion principal
*****************************/
$(document).ready(function () {
    fnCargaGrid();
    // SaveMCA();
    $(".Modificacion").each(function (i, k) {
        $(this).prop('disabled', false);//.addClass('disabled');
        $(this).prop('readonly', false);
    });

});

$('#btnAddFirmantes').on('click', function () { return fnOpenModalNewRecorrido(); }); /** Evento para abrir modal de registro de firmantes **/
$('#btnInsertarFirmantes').on('click', function () { return AgregarItemFirmantes(); }); /** Evento para guardar en el modal de registro de firmantes **/


/********************************
*Funcion para llamar los datos de los usuarios registrados
*****************************/
$('#btnCargarCedula').on('click', function () {
    if ($('#cedula').val().length == 16) {
        var url = $('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/GetUsuario';
        var parameter = {
            __RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
            cedula: $.trim($("#cedula").val())
        };

        $.post(url, parameter, function (data) {
            //     if ($.trim(data.sms) != "") { swal('', data.toModelo, 'warning'); return false; }
            /*** Resetear y Rellenar el ComboBox de la pantalla ***/
            $("#nombres").val(data.toPersona[0].nombres);
            $("#apellidos").val(data.toPersona[0].apellidos);


        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
        return false;
    }
});





/**********************************
*Funciones de abrir Modales 
**********************************/
function fnOpenModalNewRecorrido() {
    //if ($('#vehiculo').val() == '') {
    //    swal('', 'Favor seleccione primero el vehículo para poder agregar un recorrido', 'info');
    //} else {
    fnOpenModal('#modalAddFirmantes', true, false);
    //}
    return false;
}

/********************************
*Funcion de carga inicial del Grid
*****************************/
function fnCargaGrid() {
    var url = $('input[name=pathRoot]').val() + '/AdministracionMCA/ModuloMCA/GetListaFirmantes?moduloMCAId=' + ($('#moduloMCAId').length <= 0 ? 0 : $('#moduloMCAId').val());
    /*** LLenado de la tabla de documentos para seleccion del usuario ***/
    var source =
    {
        dataType: "json",
        dataFields: [
            { name: 'usuarioModuloMCAId', type: 'number' },
            { name: 'nombres', type: 'string' },
            { name: 'apellidos', type: 'string' },
            { name: 'cedula', type: 'string' },
            { name: 'tipoUsuario', type: 'string' },
            { name: 'usuarioId', type: 'number' },
            { name: 'moduloMCAId', type: 'number' }
        ],
        id: 'usuarioModuloMCAId',
        url: url
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#jqxgridFirmantes").jqxGrid(
    {
        width: '75%',
        height: '200px',
        sortable: false,
        source: dataAdapter,
        editable: true,
        showfilterrow: false,
        filterable: false,
        theme: 'darkblue',
        // autoshowfiltericon: false,
        selectionmode: 'multiplerowsextended',
        pageable: false,
        columnsresize: true,
        showstatusbar: true,
        statusbarheight: 25,
        showaggregates: true,

        ready: function () {
            $("#jqxgridFirmantes").jqxGrid('localizestrings', localizationobj);
        },
        columns: [
          {
              text: ' ', dataField: 'usuarioModuloMCAId', width: 50, editable: false, resizable: false, cellsalign: 'center', align: 'center', // hidden: (DESACTIVAR_CONTROLES != 0 ? true : false),
              cellsrenderer: function (row, column, value, defaultSettings, columnSettings, rowdata) {
                  return '<div style="text-align: center;padding-top: 3px;"><button type="button" class="btn btn-danger btn-xs"  style="padding: 0 8px;"  onclick="return fnDeleteItem(' + row + ',' + rowdata.usuarioModuloMCAId + ');" title="Eliminar item registro de recorrido"><i class="fa fa-trash" aria-hidden="true"></i></button></div>';
              }
          },
          //{ text: 'usuarioModuloMCAId', dataField: 'usuarioModuloMCAId', width: 300, hidden: true, cellsalign: 'right' },
          { text: 'Nombre', dataField: 'nombres', width: 300, editable: false, cellsalign: 'right' },
          { text: 'Nombre', dataField: 'apellidos', width: 300, editable: false, cellsalign: 'right' },
          { text: 'Tipo Usuario', dataField: 'tipoUsuario', width: 250, editable: false, cellsalign: 'left' },

        ]
    });
}


/********************************************************
*Agregar firmante al MCA
********************************************************/
function AgregarItemFirmantes() {
    if ($('#nombres').val() == 0 || $('#apellidos').val().length == 0 || $('#tipoUsuarioId').val().length == 0 || $('#tipoUsuarioId').val().length == 0) {
        swal('', 'Todos los campos son requeridos', 'error');
        return false;
    }

    CONTADOR = CONTADOR - 1;
    var rowNewData = {};
    rowNewData["usuarioModuloMCAId"] = CONTADOR;
    rowNewData["nombres"] = $('#nombres').val().toUpperCase();
    rowNewData["apellidos"] = $('#apellidos').val().toUpperCase();
    rowNewData["tipoUsuario"] = $('select#tipoUsuarioId option:selected').text().toUpperCase();
    rowNewData["cedula"] = $('#cedula').val();
    rowNewData["tipoUsuarioId"] = $('#tipoUsuarioId').val();

    rowNewData["usuarioId"] = $('#usuarioId').val() == "" ? CONTADOR : $('#usuarioId').val();

    $("#jqxgridFirmantes").jqxGrid("addrow", null, rowNewData);
    fnCloseModal('#modalAddFirmantes');
    //}
    return false;
}



