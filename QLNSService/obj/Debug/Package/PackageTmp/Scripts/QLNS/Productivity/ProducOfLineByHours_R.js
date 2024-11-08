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
GPRO.namespace('LCDNSOfLineByHours');
GPRO.LCDNSOfLineByHours = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',
            GetProductivity: '/Productivity/GetProductivitiesForDrawChart'
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
        GetProductivity();
        var productivity = setInterval(function () { GetProductivity() }, 3000);
    }


    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'lineIds': Global.Data.LineId, 'date': new Date(), 'isOneLine': true, 'IsProductOutput': false }),
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
        $.each(datas.Productivities, function (i, item) {
            var obj = {
                'Hour': item.HourName,
                'Value': item.Value, // chieu cao cua cot 
                'NormsHour': item.NormsHour
            }
            data.push(obj);
            min = item.Value < min ? item.Value : min;
            max = item.Value > max ? item.Value : max;
            max = item.NormsHour > max ? item.NormsHour : max;
        });

        // prepare jqxChart settings
        var settings = {
             title: "Năng suất của Chuyền : " + datas.LineName + ' - Ngày : ' + parseJsonDateToDate(datas.Date).toLocaleDateString(),
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
                    dataField: 'Hour',
                    displayText: 'Giờ',
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
                        dataField: 'Value',
                        displayText: 'Năng suất giờ',
                        showLabels: true,
                    },
                    {
                        dataField: 'NormsHour',
                        displayText: 'Định mức giờ',
                        showLabels: true,
                    }]
                }]
        };
        // setup the chart
        $('#jqxChartHours').css('height', parseInt($('[height]').html()))
        $('#jqxChartHours').jqxChart(settings);
        ////var str='<table id="header"><tbody><tr><td class="logo"><img src="~/Content/img/logocompany.png" /></td>';
        ////str += '<td class="title" id="lineName">Năng suất của Chuyền : ' + datas.LineName + ' - Ngày : ' + parseJsonDateToDate(datas.Date).toLocaleDateString() + '</td><td class="ledalert"></td>';
        ////str+='<td class="time"><span id="date" date></span> <br /><span id="timeofday" time></span>';
        ////str += '</td></tr></tbody></table>';
        ////$('#jqxChartHours #tdTop').html(str);
    }
}


$(document).ready(function () {
    var LCDNSOfLineByHours = new GPRO.LCDNSOfLineByHours();
    LCDNSOfLineByHours.Init();
})