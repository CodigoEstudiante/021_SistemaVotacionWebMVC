
var tabladata;

$(document).ready(function () {

    activarMenu("candidatos");

    ////validamos el formulario
    $("#formNivel").validate({
        rules: {
            nombres: "required"
        },
        messages: {
            nombres: "(*)"
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
            "url": $.MisUrls.url.Url_ObtenerCandidatos +"?ideleccion=0",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "ImagenBase64", render: function (data, type, row, meta) {
                    return '<img src="data:image/' + row.Extension + ';base64,' + row.ImagenBase64 + '" alt="Foto" width="35" height="40">'
                },
                "orderable": false,
                "searchable": false,
                "width": "90px"
            },
            { "data": "NombresCompleto" },
            { "data": "Mensaje" },
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


function buscarCandidatos() {

    if ($("#cboelecciones").val() == 0) {
        swal("Mensaje", "Seleccione una elección", "warning")
        return;
    }
  
    tabladata.ajax.url($.MisUrls.url.Url_ObtenerCandidatos + "?ideleccion=" + $("#cboelecciones").val()).load();


}



function abrirPopUpForm(json) {

    $("#fileImagen").val(null);
    $("#txtid").val(0);

    if (json != null) {

        $("#txtid").val(json.IdCandidato);

        $("#txtnombres").val(json.NombresCompleto);
        $("#txtmensaje").val(json.Mensaje);

        var valor = 0;
        valor = json.Activo == true ? 1 : 0
        $("#cboEstado").val(valor);

    } else {
        $("#txtnombres").val("");
        $("#txtmensaje").val("");

        $("#cboEstado").val(1);
    }

    $('#FormModal').modal('show');

}

function Guardar() {

    if ($("#formNivel").valid()) {

        var ImagenSeleccionada = ($("#fileImagen"))[0].files[0];
      
        var objeto = {
            IdCandidato: $("#txtid").val(),
            NombresCompleto: $("#txtnombres").val(),
            Mensaje: $("#txtmensaje").val(),
            oEleccion: {
                IdEleccion: $("#cboelecciones").val(),
            },
            Activo: parseInt($("#cboEstado").val()) == 1 ? true : false
        }

        var request = new FormData();

        request.append("imagenArchivo", ImagenSeleccionada);
        request.append("objeto", JSON.stringify(objeto));


        jQuery.ajax({
            url: $.MisUrls.url.Url_GuardarCandidato,
            type: "POST",
            data: request,
            processData: false,
            contentType: false,
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
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imagenpreview')
                .attr('src', e.target.result)
                .width(100)
                .height(150);
        };

        reader.readAsDataURL(input.files[0]);
    }
}