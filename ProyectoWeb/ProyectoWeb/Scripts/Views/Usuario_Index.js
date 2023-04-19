
var tabladata;

$(document).ready(function () {
    activarMenu("usuarios");

    ////validamos el formulario
    $("#formNivel").validate({
        rules: {
            documentoidentidad: "required",
            nombres: "required",
            apellidos: "required",
            correo: "required",
            clave: "required",

        },
        messages: {
            documentoidentidad: "(*)",
            nombres: "(*)",
            apellidos: "(*)",
            correo: "(*)",
            clave: "(*)"
        },
        errorElement: 'span'
    });


    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url.Url_ObtenerUsuarios,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "DocumentoIdentidad" },
            { "data": "Nombres" },
            { "data": "Apellidos" },
            { "data": "Correo" },
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
                "data": "IdUsuario", "render": function (data, type, row, meta) {
                    return "<button class='btn btn-primary btn-sm' type='button' onclick='abrirPopUpForm(" + JSON.stringify(row) + ")'><i class='fas fa-pen'></i></button>" +
                        "<button class='btn btn-danger btn-sm ml-2' type='button' onclick='eliminar(" + data + ")'><i class='fa fa-trash'></i></button>"
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


function abrirPopUpForm(json) {

    $("#txtid").val(0);

    if (json != null) {

        $("#txtid").val(json.IdUsuario);

        $("#txtdocumentoidentidad").val(json.DocumentoIdentidad);
        $("#txtnombres").val(json.Nombres);
        $("#txtapellidos").val(json.Apellidos);
        $("#txtcorreo").val(json.Correo);
        $("#txtclave").val(json.Clave);
        $("#txtclave").prop("disabled",true);

        var valor = 0;
        valor = json.Activo == true ? 1 : 0
        $("#cboEstado").val(valor);

    } else {
        $("#txtdocumentoidentidad").val("");
        $("#txtnombres").val("");
        $("#txtapellidos").val("");
        $("#txtcorreo").val("");
        $("#txtclave").val("");
        $("#txtclave").prop("disabled", false);

        $("#cboEstado").val(1);
    }

    $('#FormModal').modal('show');

}



function Guardar() {

    if ($("#formNivel").valid()) {

        var request = {
            objeto: {
                IdUsuario: $("#txtid").val(),
                DocumentoIdentidad: $("#txtdocumentoidentidad").val(),
                Nombres: $("#txtnombres").val(),
                Apellidos: $("#txtapellidos").val(),
                Correo: $("#txtcorreo").val(),
                Clave: $("#txtclave").val(),
                Activo: parseInt($("#cboEstado").val()) == 1 ? true : false
            }
        }

        jQuery.ajax({
            url: $.MisUrls.url.Url_GuardarUsuario,
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



function eliminar($id) {


    swal({
        title: "Mensaje",
        text: "¿Desea eliminar el usuario seleccionado?",
        type: "warning",
        showCancelButton: true,
        
        confirmButtonText: "Si",
        confirmButtonColor: "#DD6B55",

        cancelButtonText: "No",
        
        closeOnConfirm: true
    },

        function () {
            jQuery.ajax({
                url: $.MisUrls.url.Url_EliminarUsuario + "?id=" + $id,
                type: "GET",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (data.resultado) {
                        tabladata.ajax.reload();
                    } else {
                        swal("Mensaje", "No se pudo eliminar el usuario", "warning")
                    }
                },
                error: function (error) {
                    console.log(error)
                },
                beforeSend: function () {

                },
            });
        });

}


