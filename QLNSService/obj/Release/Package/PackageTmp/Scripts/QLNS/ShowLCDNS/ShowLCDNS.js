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
GPRO.namespace('ShowLCDNS');
GPRO.ShowLCDNS = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',
            // GetTotalWorkTime: '/Shift/CountWorkHoursInDayOfLine',
            GetTotalWorkTime: '/Shift/GetWorkHoursInDayOfLine',
            GetProductivity: '/Productivity/GetProductivityByLineId'
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
        RegisterEvent();
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.LineId = listLineId[0];
        GetLCDConfigInfo(function (data) {
            if (data != null) {
                $.each(data, function (i, obj) {
                    switch (obj.Name) {
                        case "WebPageHeight":
                            var value = obj.Value;
                            $('#tableproductivity').css('height', value + 'px');
                            break;
                    }
                });
            }
        });
        BuildTableLayOut(); 
    }

    var RegisterEvent = function () {
        var productivity = setInterval(function () { GetProductivity() }, 1000);
    }

    function GetTableLayoutPanel(configTypeId, callback) {
        var modelGetLCDConfig = { TableTypeId: Global.Data.TableType, ConfigTypeId: configTypeId };
        $.ajax({
            url: Global.UrlAction.GetLCDConfig,
            type: 'POST',
            data: modelGetLCDConfig,
            dataType: 'json',
            success: function (data, textStatus, xhr) {
                if (typeof (callback) == "function") {
                    callback(data);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }

    function BuildTableLayOut() {
        GetTableLayoutPanel(1, function (data) {
            if (data != null) {
                $.each(data, function (i, v) {
                    switch (v.TableLayoutPanelName.trim()) {
                        case 'tblpanelBody':
                            switch (parseInt(v.RowInt)) {
                                case 1:
                                    $('#tableproductivity .header').css('height', v.SizePercent + "%");
                                    break;
                                case 2:
                                    $('#tableproductivity .content').css('height', v.SizePercent + "%");
                                    break;
                                case 3:
                                    $('#tableproductivity .forter').css('height', v.SizePercent + "%");
                                    break;
                            }
                            break;
                        case 'tblpanelHeader':
                            switch (parseInt(v.ColumnInt)) {
                                case 1: $('#tableproductivity #header .logo').css('width', v.SizePercent + "%"); break;
                                case 2: $('#tableproductivity #header .title').css('width', v.SizePercent + "%"); break;
                                case 3: $('#tableproductivity #header .ledalert').css('width', v.SizePercent + "%"); break;
                                case 4: $('#tableproductivity #header .time').css('width', v.SizePercent + "%"); break;
                            }
                            break;
                        case 'tblpanelContent':
                            if (v.RowInt != null)
                                $('#tableproductivity #content tbody').append('<tr RowInt =' + v.RowInt + '><td class="title"  ></td><td class="value">0</td></tr>');
                            else {
                                switch (parseInt(v.ColumnInt)) {
                                    case 1:
                                        var sheet = document.createElement('style')
                                        sheet.innerHTML = "#tableproductivity #content .title {width:" + v.SizePercent + "%}";
                                        document.body.appendChild(sheet);
                                        break;
                                    case 2:
                                        var sheet = document.createElement('style')
                                        sheet.innerHTML = "#tableproductivity #content .value {width:" + v.SizePercent + "%}";
                                        document.body.appendChild(sheet);
                                        break;
                                }
                            }
                            break;
                        case 'tblpanelFooter':
                            break;
                    }
                });
                BuildPanel();
            }
        })
    }

    function BuildPanel() {
        GetTableLayoutPanel(2, function (data) {
            if (data != null) {
                $.each(data, function (i, v) {
                    switch (v.Name.trim()) {
                        case 'panelHeader':
                            $('#tableproductivity #header').css('background-color', v.BackColor);
                            break;
                        case 'panelContent':
                            $('#tableproductivity #content tbody').css('background-color', v.BackColor);
                            break;
                        case 'panelFooter':
                            $('#tableproductivity #footer').css('background-color', v.BackColor);
                            break;
                    }
                });
                BuildLabelArea();
            }
        })
    }

    function BuildLabelArea() {
        GetTableLayoutPanel(3, function (data) {
            if (data != null) {
                $.each(data, function (i, v) {
                    switch (v.TableLayoutPanelName.trim()) {
                        case 'tblpanelHeader':
                            switch (parseInt(v.Position)) {
                                case 2:
                                    $('#tableproductivity #header .title').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableproductivity #header .title').css('font-weight', "bold");
                                    else
                                        $('#tableproductivity #header .title').css('font-weight', "");
                                    $('#tableproductivity #header .title').css('color', v.Color);
                                    break;
                                case 3:
                                    $('#tableproductivity #header .time').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableproductivity #header .time').css('font-weight', "bold");
                                    else
                                        $('#tableproductivity #header .time').css('font-weight', "");
                                    $('#tableproductivity #header .time').css('color', v.Color);
                                    break;
                            }
                            break;
                        case 'tblpanelContent':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    $('#tableproductivity #content .title').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableproductivity #content .title').css('font-weight', "bold");
                                    else
                                        $('#tableproductivity #content .title').css('font-weight', "");
                                    $('#tableproductivity #content .title').css('color', v.Color);
                                    break;
                                case 2:
                                    $('#tableproductivity #content .value').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableproductivity #content .value').css('font-weight', "bold");
                                    else
                                        $('#tableproductivity #content .value').css('font-weight', "");
                                    $('#tableproductivity #content .value').css('color', v.Color);
                                    break;
                            }
                            break;
                        case 'tblpanelFooter':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    $('#tableproductivity #footer .titleFooter').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableproductivity #footer .titleFooter').css('font-weight', "bold");
                                    else
                                        $('#tableproductivity #footer .titleFooter').css('font-weight', "");
                                    $('#tableproductivity #footer .titleFooter').css('color', v.Color);
                                    ; break;
                                case 2:
                                    $('#tableproductivity #footer .valueFooter').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableproductivity #footer .valueFooter').css('font-weight', "bold");
                                    else
                                        $('#tableproductivity #footer .valueFooter').css('font-weight', "");
                                    $('#tableproductivity #footer .valueFooter').css('color', v.Color);
                                    ; break;
                            }
                            break;

                    }
                });
                BuildLabelForPanelContent();
            }
        })
    }

    function BuildLabelForPanelContent() {
        GetTableLayoutPanel(4, function (data) {
            var listTr = $('#tableproductivity #content tr');
            if (data != null && listTr != null) {
                $.each(data, function (i, v) {
                    $.each(listTr, function (j, va) {
                        if (parseInt($(va).attr("rowint")) == v.IntRowTBLPanelContent) {
                            var listTd = $(va).find('td');
                            $(listTd[0]).html(v.LabelName);
                            $(listTd[1]).attr("action", "cmd");
                            $(listTd[1]).attr("cmd", v.SystemValueName.trim());
                        }
                    });
                });
                BuildFooterContent();
            }
        })
    }

    function GetTotalWorkTime(callback) {
        $.ajax({
            url: Global.UrlAction.GetTotalWorkTime,
            type: 'POST',
            data: lineId,
            dataType: 'json',
            success: function (data, textStatus, xhr) {
                if (typeof (callback) == "function") {
                    callback(data);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }

    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.LineId, 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8', 
            success: function (data) {
                if (data.Data != null) {
                    var listValue = $('#tableproductivity #content [action="cmd"]');
                    if (listValue != null && listValue.length > 0) {
                        $('#tableproductivity #lineName').html("BẢNG NĂNG SUẤT " + data.Data["chuyen"]);
                        switch (data.Data["mauDen"]) {
                            case "ĐỎ": $('#tableproductivity #lightAlert').attr('src', '../Content/ShowLCD/circle_red.png'); break;
                            case "VÀNG": $('#tableproductivity #lightAlert').attr('src', '../Content/ShowLCD/circle_yellow.png'); break;
                            case "XANH": $('#tableproductivity #lightAlert').attr('src', '../Content/ShowLCD/circle_green.png'); break;
                        }
                        var listHTMLHoursProductivity = $('[hours]');
                        if (data.Data.listWorkHours != null && data.Data.listWorkHours.length > 0) {
                            $.each(data.Data.listWorkHours, function (i, v) {
                                $.each(listHTMLHoursProductivity, function (j, item) {
                                    if (v.IntHours == parseInt($(item).attr("hours"))) {
                                        var val = data.Data.KieuHienThiNangSuatGio == '1' ? v.HoursProductivity : v.HoursProductivity_1;
                                        $(item).html((val == null || val == '') ? "0/0" : val);
                                        return false;
                                    }
                                });
                            });
                        }

                        $.each(listValue, function (i, v) {
                            for (var propertyName in data.Data) {
                                if ($(v).attr("cmd") == propertyName) {
                                    var value = data.Data[propertyName];
                                    $(v).html(((value == null || value == '') ? "0" : value));
                                }
                            }
                        });
                    }
                }
            }
        });
    }

    function BuildFooterContent() {
        $.ajax({
            url: Global.UrlAction.GetTotalWorkTime,
            type: 'POST',
            data: JSON.stringify({ 'lineId': Global.Data.LineId }),
            contentType: 'application/json charset=utf-8', 
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (i, item) {
                        $('#tableproductivity #footer .titleFooter').append('<td class="title">' + item.TimeEnd.Hours + ':' + item.TimeEnd.Minutes + 'H</td>');
                        $('#tableproductivity #footer .valueFooter').append('<td class="value" hours="' + (i + 1) + '">0/0</td>');
                    });
                }
            }
        });
    }

}


$(document).ready(function () {
    var ShowLCDNS = new GPRO.ShowLCDNS();
    ShowLCDNS.Init();
})