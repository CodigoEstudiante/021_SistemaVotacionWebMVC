
$(document).ready(function () {


    //OBTENER ELECCIONES
    jQuery.ajax({
        url: $.MisUrls.url.obtenerElecciones,
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#elecciones").html("");
            if (data != undefined) {
                if (data.length > 0) {

                    $.each(data, function (i,row) {
                        $("<div>").addClass("card text-center border-dark mb-2").append(
                            $("<div>").addClass("card-body p-2").append(
                                
                                $("<h5>").addClass("card-title").text(row.Descripcion),
                                $("<p>").addClass("card-text").text(row.Cargo),
                                $("<button>").attr({ "type": "button", "onclick": "continuar(" + row.IdEleccion +")" }).addClass("btn btn-primary").append(
                                    $("<i>").addClass("fas fa-check"), " Continuar"
                                )
                            )
                        ).appendTo("#elecciones")
                    });
                }
            }

            console.log(data);
        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {
        },
    });
})


function continuar(ideleccion) {

    window.location.href = $.MisUrls.url.urlCandidatos + "?ideleccion=" + ideleccion;

}