var idcandidato = 0;

$(document).ready(function () {
    $("#vista2").hide("fast");
    $("#vista1").show("fast");

    //OBTENER ELECCIONES
    jQuery.ajax({
        url: $.MisUrls.url.obtenerCandidatos + "?ideleccion=" + getParameter("ideleccion"),
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("div.card-candidatos").LoadingOverlay("hide");
    
            if (data != undefined) {
                if (data.length > 0) {

                    $.each(data, function (i, row) {
                        $("<div>").addClass("card text-center border-primary mb-2").append(
                            $("<div>").addClass("card-body p-2").append(

                                $("<div>").addClass("row").append(
                                    $("<div>").addClass("col-sm-3").append(
                                        $("<img>").attr({ "src": "data:image/" + row.Extension + ";base64," + row.ImagenBase64, "alt": "Foto", "width": "100", "height": "100" }).addClass("border border-primary")
                                    ),
                                    $("<div>").addClass("col-sm-6").append(
                                        $("<h5>").addClass("card-title").text(row.NombresCompleto),
                                        $("<p>").addClass("card-text").text(row.Mensaje)
                                    ),
                                    $("<div>").addClass("col-sm-3").append(
                                        $("<input>").attr({ "type": "checkbox", "name": "optionCandidato"}).data("idcandidato",row.IdCandidato)
                                    )
                                )
                            )
                        ).appendTo("#candidatos")
                    });
                } 
            }
           
        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {
            $("div.card-candidatos").LoadingOverlay("show");
        },
    });
})


function getParameter(nameparameter) {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get(nameparameter)
}



$(document).on('click', 'input:checkbox', function (event) {


    var $box = $(this);

    idcandidato = $box.data("idcandidato");

    if ($box.is(":checked")) {
        // the name of the box is retrieved using the .attr() method
        // as it is assumed and expected to be immutable
        var group = "input:checkbox[name='" + $box.attr("name") + "']";
        // the checked state of the group/box on the other hand will change
        // and the current value is retrieved using .prop() method
        $(group).prop("checked", false);
        $box.prop("checked", true);
    } else {
        $box.prop("checked", false);
    }

});

function terminarVotacion() {

    if (idcandidato == 0) {
        swal("Mensaje", "Debe seleccionar un candidato", "warning");
        return;
    }

    var contador = 3;
    

    var request = {
        ideleccion: parseInt(getParameter("ideleccion")),
        idcandidato: idcandidato
    }

    jQuery.ajax({
        url: $.MisUrls.url.terminarVotacion,
        type: "POST",
        data: JSON.stringify(request),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("div.card-candidatos").LoadingOverlay("hide");
            if (data.resultado) {

                $("#vista1").hide("fast");
                $("#vista2").show("slow");


                setInterval(function () {

                    $("#contador").text(contador.toString())

                    if (contador == 0)
                        window.location.href = $.MisUrls.url.UrlIngreso;


                    contador -= 1;

                }, 1000);

            } else {

                swal("Mensaje", "Vuelva a intentarlo", "warning")
            }
        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {
            $("div.card-candidatos").LoadingOverlay("show");
        },
    });

    
}