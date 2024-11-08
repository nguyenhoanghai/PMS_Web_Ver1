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
GPRO.namespace('InsertProductivity');
GPRO.InsertProductivity = function () {
    var Global = {
        UrlAction: {
            // GetLCDConfig: '/api/lcdconfig',
            //  GetProductivity: '/Productivity/GetProductivitiesForDrawChart',
            GetLineInfo: '/InsertProductivity/GetLineInfo',
            GetProductByLine: '/InsertProductivity/GetProductInfoByLine',
            GetError: '/InsertProductivity/GetErrors',
            Save: '/Productivity/InsertProductivity'
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
            Global.Data.LineId = listLineId;

        RegisterEvent();
        GetLines();
        GetErrors();
    }

    var RegisterEvent = function () {
        $('#line').change(function () {
            GetProducts($(this).val());
        });

        $('#type').change(function () {
            if ($(this).val() == '6')
                $('#errorType').prop('disabled', false);
            else
                $('#errorType').prop('disabled', true);
        });

        $('[save]').click(function () {
            Save();
        })
    }


    function GetLines() {
        $.ajax({
            url: Global.UrlAction.GetLineInfo,
            type: 'POST',
            data: JSON.stringify({ 'Ids': Global.Data.LineId }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                var str = "<option value='0'>Không có dữ liệu chuyền</option>";
                if (data.Data.length > 0) {
                    str = '';
                    $.each(data.Data, function (i, item) {
                        str += '<option value="' + item.Value + '" cluster="' + item.Data + '">' + item.Name + '</option>';
                    });
                }
                $('#line').html(str);
                $('#line').change();
            }
        });
    }

    function GetProducts(id) {
        $.ajax({
            url: Global.UrlAction.GetProductByLine,
            type: 'POST',
            data: JSON.stringify({ 'Id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                var str = "<option value='0'>Không có dữ liệu Mã Hàng</option>";
                if (data.Data.length > 0) {
                    str = '';
                    $.each(data.Data, function (i, item) {
                        str += '<option value="' + item.Value + '">' + item.Name + '</option>';
                    });
                }
                $('#product').html(str);
                $('#product').change();
            }
        });
    }

    function GetErrors() {
        $.ajax({
            url: Global.UrlAction.GetError,
            type: 'POST',
            data: '',
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                var str = "<option value='0'>Không có dữ liệu lỗi</option>";
                if (data.Data.length > 0) {
                    str = '';
                    $.each(data.Data, function (i, item) {
                        str += '<option value="' + item.Value + '">' + item.Name + '</option>';
                    });
                }
                $('#errorType').html(str);
                $('#errorType').change();
            }
        });
    }

    function Save() {
        var obj = {
            Id: 0,
            LineId: $('#line').val(),
            ClusterId: $('#line option:selected').attr('cluster'),
            ProductId: $('#product').val(),
            TypeOfProductivity: $('#type').val(),
            ErrorId: $('#errorType').val(),
            Quantity: $('#quantity').val(),
            IsIncrease: $('input[name="ht"]:checked').val()
        }
        $.ajax({
            url: Global.UrlAction.Save,
            type: 'POST',
            data: JSON.stringify({ 'product': obj }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
            }
        });
    }

    //function GetProductivity() {
    //    $.ajax({
    //        url: Global.UrlAction.GetProductivity,
    //        type: 'POST',
    //        data: JSON.stringify({ 'lineIds': Global.Data.LineId, 'date': new Date(), 'isOneLine': true, 'IsProductOutput': false }),
    //        contentType: 'application/json charset=utf-8',
    //        beforeSend: function () { $('#loading').show(); },
    //        success: function (data) {
    //            if (data.Data != null) {
    //                BuildChart(data.Data[0]);
    //            }
    //        }
    //    });
    //}

    //function BuildChart(datas) {
    //    var min = 0;
    //    var max = 10;
    //    var data = [];
    //    $.each(datas.Productivities, function (i, item) {
    //        var obj = {
    //            'Hour': item.HourName,
    //            'Value': item.Value, // chieu cao cua cot 
    //            'NormsHour': item.NormsHour
    //        }
    //        data.push(obj);
    //        min = item.Value < min ? item.Value : min;
    //        max = item.Value > max ? item.Value : max;
    //        max = item.NormsHour > max ? item.NormsHour : max;
    //    });

    //    // prepare jqxChart settings
    //    var settings = {
    //        title: "Năng suất của Chuyền : " + datas.LineName + ' - Ngày : ' + parseJsonDateToDate(datas.Date).toLocaleDateString(),
    //        description: '',
    //        showLegend: true,
    //        source: data,
    //        padding: {
    //            left: 20, top: 20, right: 20, bottom: 20
    //        },
    //        titlePadding: {
    //            left: 0, top: 0, right: 0, bottom: 20
    //        },
    //        categoryAxis:
    //            {
    //                dataField: 'Hour',
    //                displayText: 'Giờ',
    //                flip: false,
    //                tickMarks: {
    //                    visible: true,
    //                    step: 1,
    //                    color: '#888888',

    //                },
    //                unitInterval: 1,
    //                gridLines: {
    //                    visible: true,
    //                    step: 10,
    //                    color: '#888888'
    //                }
    //            },
    //        colorScheme: 'scheme02',
    //        seriesGroups:
    //            [{
    //                type: 'column',
    //                valueAxis:
    //                {
    //                    displayValueAxis: true,
    //                    axisSize: 'auto',
    //                    tickMarksColor: '#888888',
    //                    size: 10,
    //                    unitInterval: Math.round(max / 10), // bước nhảy
    //                    //title: { text: 'Năng Suất' }, //ten 
    //                    minValue: min,  // gtri nho nhat
    //                    maxValue: max, // gtri > nhất
    //                    gridLines: {
    //                        visible: true,
    //                        step: 1, // bước kẽ line ngang
    //                        color: '#888888'
    //                    }
    //                },
    //                series: [{
    //                    dataField: 'Value',
    //                    displayText: 'Năng suất giờ',
    //                    showLabels: true,
    //                },
    //                {
    //                    dataField: 'NormsHour',
    //                    displayText: 'Định mức giờ',
    //                    showLabels: true,
    //                }]
    //            }]
    //    };
    //    // setup the chart
    //    $('#jqxChartHours').jqxChart(settings);
    //}
}


$(document).ready(function () {
    var InsertProductivity = new GPRO.InsertProductivity();
    InsertProductivity.Init();

    
    var myVar = setInterval(function () { GetTime() }, 1000);
})
function GetTime() {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1;
        var yyyy = today.getFullYear();

        var date = (dd < 10 ? '0' + dd : dd) + '/' + (mm < 10 ? '0' + mm : mm) + '/' + yyyy;

        var hours = today.getHours();
        var minutes = today.getMinutes();
        var second = today.getSeconds();
        var time = (hours < 10 ? '0' + hours : hours) + ':' + (minutes < 10 ? '0' + minutes : minutes) + ':' + (second < 10 ? '0' + second : second);

        $('[date]').html(date);
        $('[time]').html(time);
    }
