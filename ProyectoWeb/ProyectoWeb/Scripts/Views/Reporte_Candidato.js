
var tabladata;

var tablacarga;


$(document).ready(function () {

    activarMenu("rptcandidatos");

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



});



function buscarCandidatos() {

    if ($("#cboelecciones").val() == 0) {
        swal("Mensaje", "Seleccione una elección", "warning")
        return;
    }

    $("#tbdata tbody").html("");

    jQuery.ajax({
        url: $.MisUrls.url.Url_ObtenerCandidatos + "?ideleccion=" + $("#cboelecciones").val(),
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#inner-right").LoadingOverlay("hide");
            if (data.data.length != 0) {

                $.each(data.data, function (i,row) {
                    $("<tr>").append(
                        $("<td>").css({"text-align":"center","width":"75px"}).append(
                            $("<img>").attr({ "src": "data:image/" + row.Extension + ";base64," + row.ImagenBase64 ,"alt":"Foto","width":"70","height":"80" })
                         ),
                        $("<td>").text(row.NombresCompleto),
                        $("<td>").text(row.Mensaje)
                    ).appendTo("#tbdata tbody")

                })
            }
        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {
            $("#inner-right").LoadingOverlay("show");
        },
    });
    
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

function printData() {

    if ($('#tbdata tbody tr').length == 0) {
        swal("Mensaje", "No existen datos para imprimir", "warning")
        return;
    }



    var divToPrint = document.getElementById("tbdata");
    
    var style = "<style>";
    style = style + "table {width: 100%;font: 17px Calibri;}";
    style = style + "table, th, td {border: solid 1px #DDD; border-collapse: collapse;";
    style = style + "padding: 2px 3px;text-align: center;}";
    style = style + "</style>";

    newWin = window.open("");
    

    newWin.document.write(style);
    newWin.document.write("<center><h3>CANDIDATOS - " + $("#cboelecciones option:selected").text() + "</h3></center>");
    newWin.document.write(divToPrint.outerHTML);
    //newWin.print();
    //newWin.close();



    var images = newWin.document.getElementsByTagName("img");

    var imagesLoaded = 0;
    for (var i = 0; i < images.length; i++) {
        var image = images[i];
        var img = new Image();

        img.onload = function () {
            imagesLoaded++;
            if (imagesLoaded === images.length) {
                newWin.print();
                newWin.close();
            }
        }
        var oneSrc = image.getAttribute('src');

        img.src = oneSrc;
    };

}
