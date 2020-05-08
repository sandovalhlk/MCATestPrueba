var CONTADOR = 0; //variable que maneja los id del Jqwg cuando son insertados pero no guardados
//var validaFirmante = [];
var vf = 0;
/********************************
*Funcion principal
*****************************/
$(document).ready(function () {
    fnCargaGrid();
   // saveMCASubmit();
   //SaveMCA();
    $(".Modificacion").each(function (i, k) {
        $(this).prop('disabled', false);//.addClass('disabled');
        $(this).prop('readonly', false);
    });

    $('select#departamentoId').on('change', function (e) { fnGetMunicipio($(this).val()); });

});

$('#btnCargarCedula').on('click', function () {
    if ($('#cedula').val().length == 16) {
        var url = $('input[name="pathRoot"]').val() + '/AdministracionMCA/ModuloMCA/GetUsuario';
        var parameter = {
            __RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
            cedula: $.trim($("#cedula").val())
        };

        $.post(url, parameter, function (data) {          
            if ((data.toPersona == 0)) {
                swal("", "El usuario no se encuentra registrado", "warning");
                $("#nombres").attr("disabled", false);
                $("#apellidos").attr("disabled", false);
                
                return false;
            }
                //     if ($.trim(data.sms) != "") { swal('', data.toModelo, 'warning'); return false; }
                /*** Resetear y Rellenar el ComboBox de la pantalla ***/
                $("#nombres").val(data.toPersona[0].nombres);
                $("#apellidos").val(data.toPersona[0].apellidos);
                $('#usuarioId').val(data.toPersona[0].usuarioId);
                $("#nombres").attr("disabled", true);
                $("#apellidos").attr("disabled", true);
        }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
        return false;
    }
    else
    {
        swal("", "Verifique el número de Cédula digitado.", "warning");
    }
});

$("#btnGuardar").on("click",function(){
    saveMCASubmit();
});


/**********************************
*Funciones de abrir Modales 
**********************************/
function fnOpenModalFirmantes() {
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
        width: '100%',
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
              text: ' ', dataField: 'usuarioModuloMCAId', width: 50, editable: false, resizable: false, cellsalign: 'center', align: 'center', hidden: ($("#Eval").val() == "True" ? true : false),
              cellsrenderer: function (row, column, value, defaultSettings, columnSettings, rowdata) {
                  return '<div style="text-align: center;padding-top: 3px;"><button type="button" class="btn btn-danger btn-xs"  style="padding: 0 8px;"  onclick="return fnDeleteItem(' + row + ',' + rowdata.usuarioModuloMCAId + ');" title="Eliminar item registro de Firmantes"><i class="fa fa-trash" aria-hidden="true"></i></button></div>';
              }
          },
          { text: 'Nombre', dataField: 'nombres', width: 300, editable: false, cellsalign: 'center' , align: 'center'},
          { text: 'Apellido', dataField: 'apellidos', width: 300, editable: false, cellsalign: 'center', align: 'center' },
          { text: 'Tipo Firmante', dataField: 'tipoUsuario', width: 250, editable: false, cellsalign: 'center', align: 'center' },
      
        ]
    });

   
}


function AgregarItemFirmantes() {

    var firmantesG="";
    var validaFirmante = [];

    firmantesG = $("#jqxgridFirmantes").jqxGrid("getrows");

    for(let firmante of firmantesG) {
        validaFirmante[vf] = firmante.tipoUsuario;
        vf = vf + 1;
    }

    if($('#nombres').val()==0 || $('#apellidos').val().length==0 ||  $('#tipoUsuarioId').val().length==0 || $('#tipoUsuarioId').val().length==0){
        swal('', 'Todos los campos son requeridos', 'error');
        return false;
    }
    if ($('#cedula').val().length < 16) {
        swal("", "Verifique el número de Cédula digitado.", "warning");
        return false;
    } 
    if ($('#nombres').val() == 0 || $('#apellidos').val().length == 0 || $('#tipoUsuarioId').val().length == 0 || $('#tipoUsuarioId').val().length == 0) {
        swal('', 'Todos los campos son requeridos', 'error');
        return false;
    }
    //validaFirmante
    for (let firmante of validaFirmante) {
        if (firmante == $('select#tipoUsuarioId option:selected').text()) {
            swal("", "El tipo de firmante ya fue ingresado.", "warning");
            return false;
        }

    }
    validaFirmante[vf] = $('#tipoUsuarioId').val();
    vf=vf+1;

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
    swal("", "El firmante se agrego Satisfactoriamente", "success");
    //}
    return false;
}

/*****************************************************/
/*** Eliminar Item o fila del JQWIDGWTS   ***/
function fnDeleteItem(row, firmante) {
    swal({ title: "", text: "¿ Seguro desea borrar la fila del registro del Firmante?", type: "warning", cancelButtonText: "NO", showCancelButton: true, confirmButtonText: "SI", closeOnConfirm: true, },
        function () {

            var id = $("#jqxgridFirmantes").jqxGrid('getrowid', row);
         //   var rowDistanciaProgramada = $("#jqxgridRecorridos").jqxGrid('getrowdata', row).distanciaProgramada;

            if (firmante > 0) {
                $.post($('input[name=pathRoot]').val() + '/AdministracionMCA/ModuloMCA/EliminarFirmantes?UsuarioModuloId=' + id, function (result) {
                    if (result == 1) {
                        swal('', 'Registro borrado exitosamente', 'success');
                        $("#jqxgridFirmantes").jqxGrid('deleterow', id);
                      //  validaFirmante[vf] = validaFirmante.splice()
                    }
                }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
            } else {
                $("#jqxgridFirmantes").jqxGrid('deleterow', id);
            }
          //  fnOperarCantidadKilometros(rowDistanciaProgramada); /** Resta la distancia programada al input totalKms, para calcular el rendimiento **/

        });
    return false;
}

/**********************************************************************************************************************************************
 * Función para Cargar los municipios de un determinado departamento
 **********************************************************************************************************************************************/
function fnGetMunicipio(munic) {
    var departamento = $('select[name=municipioId]');
    departamento.html('').text('').append('<option value="">Seleccione el Municipio</option>').select2('destroy').select2();
    if ($.trim(munic).length <= 0)
        return false;
    var url = $('input[name="pathRoot"]').val() + "/AdministracionMCA/ModuloMCA/GetMunicipioDep";

    $.post(url, { departamentoId: munic }, function (result) {
        console.log(result.Municipios);
        if (result.Municipios == null) {
            swal("", "Este Departamento no tiene Municipios asociados", "warning");
        }
        $.each(result.Municipios, function (i, jsondata) {
            departamento.append('<option value="' + jsondata.municipioId + '">' + '-- ' + jsondata.Municipio + '</option>').trigger("change.select2");
        });

    }).fail(function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus + ": " + XMLHttpRequest.responseText); });
}

function saveMCASubmit() {

//    $('#formCreateMCA').on('submit', function () {
         
        swal({
            title: "",
            text: "¿Esta seguro que desea agregar el Modulo MCA?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Si, Continuar",
            cancelButtonText: "No, Cancelar",
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                $('#formCreateMCA').submit();
         
            }

            return false;


    });
    

}