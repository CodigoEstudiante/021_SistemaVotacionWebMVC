// based on prepared DOM, initialize echarts instance
var myChart;

var option = {
    toolbox: {
        show: true,
        feature: {
            saveAsImage: {}
        }
    },
    tooltip: {
        trigger: 'axis',
        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
    },
    title: {
        text: 'ELECCIONES 2021'
    },
    legend: {
        show: true
    },
    xAxis: {
        type: 'category',
        data: ['Mon ', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        axisTick: {
            alignWithLabel: true
        }

    },
    yAxis: {
        type: 'value'
    },
    series: [
        {
            name: "Cantidad de Votos",
            data: [120, 200, 150, 80, 70, 110, 130],
            type: 'bar',
            barWidth: '60%',
            label: {
                show: true,
                position: 'top'
            }
        }
    ]
};

$(document).ready(function () {

    myChart = echarts.init(document.getElementById('main'));
   

    activarMenu("resultados");



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

})




function buscarResultados() {

    if ($("#cboelecciones").val() == 0) {
        swal("Mensaje", "Seleccione una elección", "warning")
        return;
    }

    var dataMenbers = [];
    var dataValues = [];

    //OBTENER ELECCIONES
    jQuery.ajax({
        url: $.MisUrls.url.Url_obtenerResultados + "?ideleccion=" + $("#cboelecciones").val(),
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            dataMenbers = [];
            dataValues = [];


            //$(".card-resultados").LoadingOverlay("hide");

            myChart.hideLoading();

            if (data != undefined) {

                if (data.length) {
                    $.each(data, function (i,item) {
                        dataMenbers.push(item.NombresCompleto);
                        dataValues.push(item.Votos);

                    })

                }
            }
           

            option.title.text = $("#cboelecciones option:selected").text();
            option.xAxis.data = dataMenbers;
            option.series[0].data = dataValues;


            // use configuration item and data specified to show chart
            myChart.setOption(option);
        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {
            myChart.showLoading();
            //$(".card-resultados").LoadingOverlay("show");
        },
    });

}