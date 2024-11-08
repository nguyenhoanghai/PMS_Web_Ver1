if (typeof GPRO == 'undefined' || !GPRO) {
    var GPRO = {};
}

GPRO.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ('' + a[i]).split('.');
        o = GPRO;
        for (j = (d[0] == 'GPRO') ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
}
GPRO.namespace('LCDProducOfLinesByDate_Output');
GPRO.LCDProducOfLinesByDate_Output = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',
            GetProductivity: '/Productivity/GetProductivitiesForDrawChart'
        },
        Data: {
            TableType: 1,
            LineIds: [],
            date: null
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {

        Global.Data.LineIds = getCookie('strLineIds').split(',');

        GetLCDConfigInfo(function (data) {
            if (data != null) {
                $.each(data, function (i, obj) {
                    switch (obj.Name) {
                        case "WebPageHeight":
                            var value = obj.Value;
                            $('#jqxChart').css('height', value + 'px');
                            break;
                    }
                });
            }
        });
        RegisterEvent();
    }

    var RegisterEvent = function () {
        GetProductivity();
        var productivity = setInterval(function () { GetProductivity() }, 3000);
    }


    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'lineIds': Global.Data.LineIds, 'date': new Date(), 'isOneLine': false, 'IsProductOutput': false }),
            contentType: 'application/json charset=utf-8', 
            success: function (data) { 
                if (data.Data != null) {
                    BuildChart(data.Data);
                }
            }
        });
    }

    function BuildChart(datas) {
        var min = 0;
        var max = 10;
        var data = [];
        $.each(datas, function (i, item) {
            var obj = {
                'LineName': item.LineName,
                'ProductivitiesOfLine': item.ProductivitiesOfLine, // chieu cao cua cot 
                'NormsDay': item.NormsDay
            }
            data.push(obj);
            min = item.ProductivitiesOfLine < min ? item.ProductivitiesOfLine : min;
            max = item.ProductivitiesOfLine > max ? item.ProductivitiesOfLine : max;
            max = item.NormsDay > max ? item.NormsDay : max;
        });

        // prepare jqxChart settings
        var settings = {
            title: 'Thành phẩm thoát chuyền các Chuyền - Ngày : ' + parseJsonDateToDate(datas[0].Date).toLocaleDateString(),
            description: '',
            showLegend: true,
            source: data,
            padding: {
                left: 20, top: 20, right: 20, bottom: 20
            },
            titlePadding: {
                left: 0, top: 0, right: 0, bottom: 20
            },
            categoryAxis:
                {
                    dataField: 'LineName',
                    displayText: 'Chuyền ',
                    flip: false,
                    tickMarks: {
                        visible: true,
                        step: 1,
                        color: '#888888',

                    },
                    unitInterval: 1,
                    gridLines: {
                        visible: true,
                        step: 10,
                        color: '#888888',
                    }
                },
            colorScheme: 'scheme02',
            seriesGroups:
                [{
                    type: 'column',
                    valueAxis:
                    {
                        displayValueAxis: true,
                        axisSize: 'auto',
                        tickMarksColor: '#888888',
                        size: 10,
                        unitInterval: Math.round(max / 10), // bước nhảy
                        // title: { text: 'Năng Suất' }, //ten 
                        minValue: min,  // gtri nho nhat
                        maxValue: max, // gtri > nhất
                        gridLines: {
                            visible: true,
                            step: 2, // bước kẽ line ngang
                            color: '#888888'
                        }
                    },
                    series: [{
                        dataField: 'ProductivitiesOfLine',
                        displayText: 'Thành phẩm',
                        showLabels: true,
                    },
                    {
                        dataField: 'NormsDay',
                        displayText: 'Định mức ngày',
                        showLabels: true,
                    }]
                }]
        };
        // setup the chart
        $('#jqxChartDay_Output').css('height', parseInt($('[height]').html()));
        $('#jqxChartDay_Output').jqxChart(settings);
    }
}


$(document).ready(function () {
    var LCDProducOfLinesByDate_Output = new GPRO.LCDProducOfLinesByDate_Output();
    LCDProducOfLinesByDate_Output.Init();
})