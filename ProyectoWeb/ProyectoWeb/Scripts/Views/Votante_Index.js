
var tabladata;

var tablacarga;


$(document).ready(function () {
    activarMenu("votantes");

    ////validamos el formulario
    $("#formNivel").validate({
        rules: {
            documentoidentidad: "required",
            nombres: "required",
            apellidos: "required"
        },
        messages: {
            documentoidentidad: "(*)",
            nombres: "(*)",
            apellidos: "(*)"
        },
        errorElement: 'span'
    });


    //OBTENER ELECCIONES
    jQuery.ajax({
        url: $.MisUrls.url.Url_ObtenerElecciones,
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            $("#cboelecciones").html("");

            $("<option>").attr({ "value": 0 }).text("-- Seleccione --").appendTo("#cboelecciones");
            if (data.data != null)
                $.each(data.data, function (i, item) {

                    if (item.Activo == true) {
                        $("<option>").attr({ "value": item.IdEleccion }).text(item.Descripcion).appendTo("#cboelecciones");
                    }
                })
        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {
        },
    });


    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url.Url_ObtenerVotantes +"?ideleccion=0",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "DocumentoIdentidad" },
            { "data": "Nombres" },
            { "data": "Apellidos" },
            {
                "data": "FechaRegistro", render: function (data) {
                    return ObtenerFormatoFecha(data)
                }
            },
            {
                "data": "Activo", "render": function (data) {
                    if (data) {
                        return '<span class="badge badge-success">Activo</span>'
                    } else {
                        return '<span class="badge badge-danger">No Activo</span>'
                    }
                }
            },
            {
                "data": "oUsuario", render: function (data) {
                    return data.Correo
                }
            },
            {
                "data": "IdCandidato", "render": function (data, type, row, meta) {
                    return "<button class='btn btn-primary btn-sm' type='button' onclick='abrirPopUpForm(" + JSON.stringify(row) + ")'><i class='fas fa-pen'></i></button>"
                },
                "orderable": false,
                "searchable": false,
                "width": "90px"
            }

        ],
        "language": {
            "url": $.MisUrls.url.Url_datatable_spanish
        }
    });

});

function MostrarModalCarga() {

    if ($("#cboelecciones").val() == 0) {
        swal("Mensaje", "Seleccione una elección", "warning")
        return;
    }

    $('#ModalCarga').modal('show');
}



$('#ModalCarga').on('hidden.bs.modal', function (e) {
   
    $("#fileExcel").val(null);
    $("#divResultadoCarga").hide();
})

$('#ModalCarga').on('show.bs.modal', function (e) {

    $("#fileExcel").val(null);
    $("#divResultadoCarga").hide();
})


function CargarExcel() {

    if (document.getElementById("fileExcel").files.length == 0) {
        swal("Mensaje", "Seleccione un archivo para cargar", "warning");
        return;
    }

    if ($.fn.DataTable.isDataTable('#tablaresultado')) {
        $('#tablaresultado').DataTable().destroy();
    }

    var archivo = ($("#fileExcel"))[0].files[0];
    var request = new FormData();

    request.append("excelArchivo", archivo);
    request.append("ideleccion",  $("#cboelecciones").val().toString());

    jQuery.ajax({
        url: $.MisUrls.url.Url_CargarVotantes,
        type: "POST",
        data: request,
        processData: false,
        contentType: false,
        success: function (data) {

            $("#ModalCarga div.modal-body").LoadingOverlay("hide");

            $("#fileExcel").val(null);

            if (data.data.length != 0) {

                $('#tablaresultado').dataTable({
                    data: data.data,
                    columns: [
                        {
                            "data": "Estado", "render": function (data) {
                                if (data) {
                                    return '<button class="btn btn-success btn-sm"><i class="fas fa-check-circle"></i></button>'
                                } else {
                                    return '<button class="btn btn-danger btn-sm"><i class="fas fa-times-circle"></i></button>'
                                }
                            },
                            "orderable": false,
                            "searchable": false,
                            "width": "20px"
                        },
                        { "data": "Mensaje" },
                        { "data": "DocumentoIdentidad"},
                        { "data": "Nombres" },
                        { "data": "Apellidos" }
                    ],
                    "language": {
                        "url": $.MisUrls.url.Url_datatable_spanish
                    },
                    "scrollY": "250px",
                    "scrollCollapse": true,
                    "paging": false
                });
            }

            $("#divResultadoCarga").show();
           
        },
        error: function (error) {
        },
        beforeSend: function () {

            $("#ModalCarga div.modal-body").LoadingOverlay("show")
        },
    });
}



function buscarVotantes() {

    if ($("#cboelecciones").val() == 0) {
        swal("Mensaje", "Seleccione una elección", "warning")
        return;
    }
  
    tabladata.ajax.url($.MisUrls.url.Url_ObtenerVotantes + "?ideleccion=" + $("#cboelecciones").val()).load();
}



function abrirPopUpForm(json) {
    
    $("#txtid").val(0);

    if (json != null) {

        $("#txtid").val(json.IdVotante);

        $("#txtdocumentoidentidad").val(json.DocumentoIdentidad);
        $("#txtnombres").val(json.Nombres);
        $("#txtapellidos").val(json.Apellidos);

        var valor = 0;
        valor = json.Activo == true ? 1 : 0
        $("#cboEstado").val(valor);

    }

    $('#FormModal').modal('show');

}

function Guardar() {

    if ($("#formNivel").valid()) {

        var request = {
            objeto: {
                IdVotante: $("#txtid").val(),
                DocumentoIdentidad: $("#txtdocumentoidentidad").val(),
                Nombres: $("#txtnombres").val(),
                Apellidos: $("#txtapellidos").val(),
                Activo: parseInt($("#cboEstado").val()) == 1 ? true : false
            }
        }

        jQuery.ajax({
            url: $.MisUrls.url.Url_GuardarVotante,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.resultado) {
                    tabladata.ajax.reload();
                    $('#FormModal').modal('hide');
                } else {

                    swal("Mensaje", "No se pudo guardar los cambios", "warning")
                }
            },
            error: function (error) {
                console.log(error)
            },
            beforeSend: function () {

            },
        });

    }

}


function ObtenerFormatoFecha(datetime) {

    var re = /-?\d+/;
    var m = re.exec(datetime);
    var d = new Date(parseInt(m[0]))


    var month = d.getMonth() + 1;
    var day = d.getDate();
    var output = (('' + day).length < 2 ? '0' : '') + day + '-' + (('' + month).length < 2 ? '0' : '') + month + '-' + d.getFullYear();

    return output;
}

function readURL(input) {
    //if (input.files && input.files[0]) {
    //    var reader = new FileReader();

    //    reader.onload = function (e) {
    //        $('#imagenpreview')
    //            .attr('src', e.target.result)
    //            .width(100)
    //            .height(150);
    //    };

    //    reader.readAsDataURL(input.files[0]);
    //}
}