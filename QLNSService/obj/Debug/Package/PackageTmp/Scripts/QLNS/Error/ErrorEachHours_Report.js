/// <reference path="../../ShowLCD.html" />
/// <reference path="../../ShowLCD.html" />
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
GPRO.namespace('LCDErrorInDay');
GPRO.LCDErrorInDay = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',
            GetErrors: '/Error/GetErrorsForDrawChart'
        },
        Data: {
            TableType: 1,
            LineId: 0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {

        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.LineId = listLineId;// listLineId[0];
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
        GetErrors();
          setInterval(function () { GetErrors() }, 2000);
    }


    function GetErrors() {
        $.ajax({
            url: Global.UrlAction.GetErrors,
            type: 'POST',
            data: JSON.stringify({ 'lineIds': Global.Data.LineId, 'date': new Date(), 'isOneLine': true }),
            contentType: 'application/json charset=utf-8', 
            success: function (data) { 
                if (data.Data != null) {
                    BuildChart(data.Data[0]);
                }
            }
        });
    }

    function BuildChart(datas) {
        var min = 0;
        var max = 10;
        var data = [];
        $.each(datas.SubErrors, function (i, item) {
            var obj = {
                'ErrorName': item.ErrorName,
                'Quantites': item.Quantites, // chieu cao cua cot  
            }
            data.push(obj);
            min = item.Quantites < min ? item.Quantites : min;
            max = item.Quantites > max ? item.Quantites : max; 
        });

        // prepare jqxChart settings
        var settings = {
            title: "Lỗi của Chuyền : " + datas.LineName + ' - Ngày : ' + parseJsonDateToDate(datas.Date).toLocaleDateString(),
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
                    dataField: 'ErrorName',
                    displayText: 'Lỗi',
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
                        color: '#888888'
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
                        //title: { text: 'Năng Suất' }, //ten 
                        minValue: min,  // gtri nho nhat
                        maxValue: max, // gtri > nhất
                        gridLines: {
                            visible: true,
                            step: 1, // bước kẽ line ngang
                            color: '#888888'
                        }
                    },
                    series: [{
                        dataField: 'Quantites',
                        displayText: 'Số lượng',
                        showLabels: true,
                    }]
                }]
        };
        // setup the chart
        $('#jqxChartHours_Error').css('height', parseInt($('[height]').html()));
        $('#jqxChartHours_Error').jqxChart(settings);
    }
}


$(document).ready(function () {
    var LCDErrorInDay = new GPRO.LCDErrorInDay();
    LCDErrorInDay.Init();
})