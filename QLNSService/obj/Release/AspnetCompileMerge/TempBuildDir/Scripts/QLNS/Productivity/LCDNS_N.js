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
GPRO.namespace('LCDNS_N');
GPRO.LCDNS_N = function () {
    var Global = {
        UrlAction: { 
            GetProductivity: '/Productivity/GetProductivityByLineId',
            GetData: '/Productivity/GetLCDInfo'
        },
        Data: {
            TableType: 1,
            LineId: 0, 
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        GetTime();
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.LineId = listLineId[0];
        BuildTableLayOut();
        RegisterEvent();
    }

    var RegisterEvent = function () {
        GetProductivity();
        var productivity = setInterval(function () { GetProductivity() }, 2000);
    }

    function GetTableData(configTypeId, callback) {
        $.ajax({
            url: Global.UrlAction.GetData,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.LineId, 'tableId': Global.Data.TableType, 'configTypeId': configTypeId }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (typeof (callback) == "function") {
                    callback(data.Data);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }

    function BuildTableLayOut() {
        GetTableData(1, function (data) {
            if (data != null) {
                $('#tb_qlns').css('height', data.PageHeight + 'px');
                $('#tb_qlns #lineName').html("BẢNG NĂNG SUẤT " + data.LineName);
                veKhung(data.LayoutPanelConfig);
                DrawBackgroundColor(data.Panel_Background);
                SetFontStyle(data.FontStyle);
                SetBodyTitle(data.BodyTitle);
                SetFooterTitle(data.FooterTitle);
            }
        })
    }

    function veKhung(data) {
        var style = $('<style></style>');
        $.each(data, function (i, v) {
            switch (v.TableLayoutPanelName.trim()) {
                case 'tblpanelBody':
                    switch (parseInt(v.RowInt)) {
                        case 1:
                            style.append('#tb_qlns .header{ height:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_qlns .content{ height:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_qlns .forter{ height:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelHeader':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_qlns #header .logo { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_qlns #header .title { width:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_qlns #header .ledalert { width:' + v.SizePercent + '%}');
                            break;
                        case 4:
                            style.append('#tb_qlns #header .time { width:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelContent':
                    //if (v.RowInt != null)
                    //  $('#tb_qlns #tb_body').append('<tr RowInt =' + v.RowInt + '><td class="title"  ></td><td class="value">0</td></tr>');
                    // if(v.RowInt==null) {
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_qlns #content .title { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_qlns #content .value { width:' + v.SizePercent + '%  }');
                            break;
                    }
                    // }
                    break;
                case 'tblpanelFooter':
                    break;
            }
        });

        $('head').append(style);
    }

    function DrawBackgroundColor(data) {
        var style = $('<style></style>');
        $.each(data, function (i, item) {
            switch (item.Name.trim()) {
                case 'panelHeader':
                    style.append('#tb_qlns #header {background-color :' + item.BackColor + '}');
                    break;
                case 'panelContent':
                    style.append('#tb_qlns #content{background-color :' + item.BackColor + '}');
                    break;
                case 'panelFooter':
                    style.append('#tb_qlns #footer{background-color :' + item.BackColor + '}');
                    break;
            }
        });
        $('head').append(style);
    }

    function SetFontStyle(data) {
        var style = $('<style></style>');
        $.each(data, function (i, v) {
            switch (v.TableLayoutPanelName.trim()) {
                case 'tblpanelHeader':
                    switch (parseInt(v.Position)) {
                        case 2:
                            style.append('#tb_qlns #header .title {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 3:
                            style.append('#tb_qlns #header .time {font-size :' + v.Size + 'px ;line-height :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
                case 'tblpanelContent':
                    switch (parseInt(v.Position)) {
                        case 1:
                            style.append('#tb_qlns #content .title {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 2:
                            style.append('#tb_qlns #content .value {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
                case 'tblpanelFooter':
                    switch (parseInt(v.Position)) {
                        case 1:
                            style.append('#tb_qlns #footer .titleFooter {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 2:
                            style.append('#tb_qlns #footer .valueFooter {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
            }
        });
        $('head').append(style);
    }

    function SetBodyTitle(data) {
        var listTr = $('#tb_qlns #content');
        if (data != null && listTr != null) {
            var str = '';
            $.each(data, function (i, v) {
                str += '<tr><td class="title">' + v.LabelName + '</td><td class="value" action="cmd" cmd="' + v.SystemValueName.trim() + '">0</td></tr>';
            });
            $('#tb_qlns #content tbody').append(str);
        }
    }

    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.LineId, 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    switch (data.Data["mauDen"]) {
                        case "ĐỎ": $('#tb_qlns #lightAlert').attr('src', '../Content/ShowLCD/circle_red.png'); break;
                        case "VÀNG": $('#tb_qlns #lightAlert').attr('src', '../Content/ShowLCD/circle_yellow.png'); break;
                        case "XANH": $('#tb_qlns #lightAlert').attr('src', '../Content/ShowLCD/circle_green.png'); break;
                    }
                    var listValue = $('#tb_qlns #content [action="cmd"]');
                    if (listValue != null && listValue.length > 0) {
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

    function SetFooterTitle(data) {
        var title = $('<tr class="titleFooter"></tr>');
        var value = $('<tr class="valueFooter"></tr>');
        $.each(data, function (i, item) {
            title.append('<td class="title">' + item.TimeEnd.Hours + ':' + item.TimeEnd.Minutes + 'H</td>');
            value.append('<td class="value" hours="' + (i + 1) + '">0/0</td>');
        });
        $('#tb_qlns #footer').append(title);
        $('#tb_qlns #footer').append(value);
    }
}


$(document).ready(function () {
    var LCDNS_N = new GPRO.LCDNS_N();
    LCDNS_N.Init();
})