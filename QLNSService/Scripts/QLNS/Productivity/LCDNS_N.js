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
            ShowType: 0,
            TimesGetNS: 0,
            KhoangCachGetNSOnDay: 1,
            NSGio_Formula: null,

            IntervalChange: parseInt($('#jgroup').attr('interval')),
            ListLines: [],
            currentIndex: 0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        GetTime();
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0) {
            $.each(listLineId, function (i, item) {
                Global.Data.ListLines.push(item);
            });
        }

        Global.Data.LineId = listLineId[Global.Data.currentIndex];
        $('#box_video').attr('line', Global.Data.LineId);

        setInterval(function () {
            Global.Data.currentIndex = ((Global.Data.currentIndex + 1) > (Global.Data.ListLines.length - 1)) ? 0 : (Global.Data.currentIndex + 1);
            Global.Data.LineId = listLineId[Global.Data.currentIndex];
            $('#box_video').attr('line', Global.Data.LineId);
        }, Global.Data.IntervalChange);

        //  GetVideoSchedule();

        BuildTableLayOut();
        RegisterEvent();

    }

    var RegisterEvent = function () {
        GetProductivity();
    //    var productivity = setInterval(function () { GetProductivity() }, 2000);
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
                Global.Data.ShowType = data.ShowNSType;
                Global.Data.TimesGetNS = data.TimesGetNS;
                Global.Data.KhoangCachGetNSOnDay = data.KhoangCachGetNSOnDay;
                Global.Data.NSGio_Formula = data.NSG_Formula;
                veKhung(data.LayoutPanelConfig);
                DrawBackgroundColor(data.Panel_Background);
                SetFontStyle(data.FontStyle);
                SetBodyTitle(data.BodyTitle);
                SetFooterTitle(data.FooterTitle, data.ShowNSType, data.KhoangCachGetNSOnDay);
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
                            style.append('#tb_qlns .footer{ height:' + v.SizePercent + '%}');
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
                            style.append('#tb_qlns #header .title {text-transform: uppercase;font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 3:
                            style.append('#tb_qlns #header .time { font-size :' + v.Size + 'px ;line-height :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
                case 'tblpanelContent':
                    switch (parseInt(v.Position)) {
                        case 1:
                            style.append('#tb_qlns #content .title {text-transform: uppercase;font-size :' + v.Size + 'px ;line-height:' + v.Size + 'px ; font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 2:
                            style.append('#tb_qlns #content .value {font-size :' + v.Size + 'px ;line-height:' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
                case 'tblpanelFooter':
                    switch (parseInt(v.Position)) {
                        case 1:
                            style.append('#tb_qlns #footer .titleFooter {text-transform: uppercase;font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
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
        var height = 100 / data.length;
        if (data != null && listTr != null) {
            var str = '';
            $.each(data, function (i, v) {
                str += '<tr><td class="title" tt="' + v.SystemValueName.trim() + '">' + v.LabelName + '</td><td class="value ' + (v.SystemValueName.trim() == 'maHang' ? '' : 'font-dt') + '" action="cmd" cmd="' + v.SystemValueName.trim() + '">0</td></tr>';
                //  str += '<div style="height:'+height+'%; border:1px solid green"><div class="left_ title"  tt="' + v.SystemValueName.trim() + '">'++'</div><div class="right_">0/0</div><div style="clear:left"></div></div>';
            });
            $('#tb_qlns #content tbody').append(str);
        }
    }

    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.LineId, 'tableTypeId': Global.Data.TableType, 'hienThiNSGio': Global.Data.ShowType, 'TimesGetNS': Global.Data.TimesGetNS, 'KhoangCachGetNSOnDay': Global.Data.KhoangCachGetNSOnDay }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    $('#tb_qlns #lineName').html("BẢNG NĂNG SUẤT " + data.Data.LineName);
                    switch (data.Data["mauDen"]) {
                        case "ĐỎ": $('#tb_qlns #lightAlert').attr('src', '../Content/ShowLCD/circle_red.png'); break;
                        case "VÀNG": $('#tb_qlns #lightAlert').attr('src', '../Content/ShowLCD/circle_yellow.png'); break;
                        case "XANH": $('#tb_qlns #lightAlert').attr('src', '../Content/ShowLCD/circle_green.png'); break;
                    }
                    var listHTMLHoursProductivity = $('[hours]');
                    if (Global.Data.ShowType == 2)
                    {
                        var abc = data.Data.KCS + "/" + data.Data.SLKHToNow + '(' + (data.Data.SLKHToNow > 0 ? Math.round((data.Data.KCS / data.Data.SLKHToNow) * 100) : 0) + '%)';
                        $(listHTMLHoursProductivity[0]).html(abc);
                    }
                    else {
                        if (data.Data.listWorkHours != null && data.Data.listWorkHours.length > 0) {
                            $.each(data.Data.listWorkHours, function (i, v) {
                                $.each(listHTMLHoursProductivity, function (j, item) {
                                    if (v.IntHours == parseInt($(item).attr("hours"))) {

                                        var value = '';
                                        $.each(Global.Data.NSGio_Formula, function (ii, cf) {
                                            switch (cf) {
                                                case 'TC': value += value == '' ? v.TC : '|' + v.TC; break;
                                                case 'TH': value += value == '' ? v.KCS : '|' + v.KCS; break;
                                                case 'DM': value += value == '' ? v.NormsHour : '|' + v.NormsHour; break;
                                                case 'Error': value += value == '' ? v.Error : '|' + v.Error; break;
                                                case 'BTP': value += value == '' ? v.BTP : '|' + v.BTP; break;
                                            }
                                        });
                                        $(item).html(value)

                                        return false;
                                    }
                                });
                            });
                        }
                    }

                    var listValue = $('#tb_qlns #content [action="cmd"]');
                    if (listValue != null && listValue.length > 0) {
                        $.each(listValue, function (i, v) {
                            for (var propertyName in data.Data) {
                                if ($(v).attr("cmd") == propertyName) {
                                    if (propertyName == 'thucHienVaDinhMuc') {
                                        switch (Global.Data.ShowType) {
                                            case 2:
                                            case 0:
                                            case 10:
                                            case 1:
                                            case 11:
                                            case 15:
                                                //TH-DM
                                                var value = data.Data[propertyName];
                                                $(v).html(((value == null || value == '') ? "0" : value));
                                                break;
                                            case 3:
                                            case 13: //TH-TC
                                            case 16: //TH-TC
                                                $(v).html(data.Data.ThucHienNgay + '/' + data.Data.ThoatChuyenNgay);
                                                break;
                                            case 4:
                                            case 14: //TH-Error
                                            case 17: //TH-Error
                                                $(v).html(data.Data.ThucHienNgay + '/' + data.Data.ErrorNgay);
                                                break;
                                        }
                                    }

                                    else {
                                        var value = data.Data[propertyName];
                                        $(v).html(((value == null || value == '') ? "0" : value));
                                    }
                                }
                            }
                        });

                        $.each(listValue, function (i, v) {
                            var arr = ($(v).attr("cmd")).split('|');
                            var str = '';
                            for (var i = 0; i < arr.length; i++) {
                                if (str != '')
                                    str += '<span style="color:blue">|</span>';
                                switch (arr[i]) {
                                    case 'ProductName': str += data.Data.ProductName; break;
                                    case 'LineName': str += data.Data.LineName; break;
                                    case 'LDDB': str += data.Data.LDDB; break;
                                    case 'LDTT': str += data.Data.LDTT; break;
                                    case 'SLKH': str += data.Data.SLKH; break;
                                    case 'SLCL': str += data.Data.SLCL; break;
                                    case 'DMN': str += data.Data.DMN; break;
                                    case 'KCS': str += data.Data.KCS; break;
                                    case 'TC': str += data.Data.TC; break;
                                    case 'Error': str += data.Data.Error; break;
                                    case 'BTP': str += data.Data.BTP; break;
                                    case 'BTPInLine': str += data.Data.BTPInLine; break;
                                    case 'BTPInLine_BQ': str += data.Data.BTPInLine_BQ; break;
                                    case 'DoanhThuBQ': str += data.Data.DoanhThuBQ; break;
                                    case 'DoanhThuBQ_T': str += data.Data.DoanhThuBQ_T; break;
                                    case 'DoanhThu': str += data.Data.DoanhThu; break;
                                    case 'DoanhThu_T': str += data.Data.DoanhThu_T; break;
                                    case 'DoanhThuKH_T': str += data.Data.DoanhThuKH_T; break;
                                    case 'KCS_KH_T': str += data.Data.KCS_KH_T; break;
                                    case 'DoanhThuDM': str += data.Data.DoanhThuDM; break;
                                    case 'DoanhThuDM_T': str += data.Data.DoanhThuDM_T; break;
                                    case 'KCSKH_T': str += data.Data.KCSKH_T; break;
                                    case 'ThuNhapBQ': str += data.Data.ThuNhapBQ; break;
                                    case 'ThuNhapBQ_T': str += data.Data.ThuNhapBQ_T; break;
                                    case 'SLKHToNow': str += data.Data.SLKHToNow; break;
                                    case 'NhipSX': str += data.Data.NhipSX; break;
                                    case 'NhipTT': str += data.Data.NhipTT; break;
                                    case 'NhipTC': str += data.Data.NhipTC; break;
                                    case 'tiLeThucHien': str += data.Data.tiLeThucHien; break;
                                    case 'nangSuatGioTruoc': str += data.Data.nangSuatGioTruoc; break;
                                    case 'nangSuatGioHienTai': str += data.Data.nangSuatGioHienTai; break;
                                    case 'Hour_ChenhLech_Day': str += data.Data.Hour_ChenhLech_Day; break;
                                    case 'Hour_ChenhLech': str += data.Data.Hour_ChenhLech; break;
                                    case 'KCS_QuaTay': str += data.Data.KCS_QuaTay; break;
                                    case 'LK_KCS_QuaTay': str += data.Data.LK_KCS_QuaTay; break;
                                    case 'LK_TC': str += data.Data.LK_TC; break;
                                    case 'LK_KCS': str += data.Data.LK_KCS; break;
                                    case 'LK_BTP': str += data.Data.LK_BTP; break;
                                    case 'Lean': str += data.Data.Lean; break;
                                }
                            }
                            $(v).html(str);
                        });
                    }
                }
            }
        });
    }

    function SetFooterTitle(data, showNSType, hour) {
        $('#tb_qlns #footer tbody').empty();
        var title = $('<tr class="titleFooter"></tr>');
        var value = $('<tr class="valueFooter"></tr>');
        if (showNSType == 2) {
            title.append('<td class="title" style="width:60%">Năng suất hiện tại</td>');
            title.append('<td class="value" hours="1">0/0 (0%)</td>');
            $('#tb_qlns #footer').append(title);
        }
        else {
            $.each(data, function (i, item) {
                switch (showNSType) {
                    case 0:
                    case 1:
                    case 3:
                    case 4:
                        title.append('<td class="title">' + item.TimeEnd.Hours + ':' + (item.TimeEnd.Minutes.length == 1 ? '0' + item.TimeEnd.Minutes : item.TimeEnd.Minutes) + 'H</td>');
                        value.append('<td class="value" hours="' + (i + 1) + '">0/0</td>');
                        break;
                    case 10:
                    case 11:
                    case 13:
                    case 14:
                        title.append('<td class="title">' + item.TimeStart.Hours + ':' + item.TimeStart.Minutes + '-' + item.TimeEnd.Hours + ':' + item.TimeEnd.Minutes + '</td>');
                        value.append('<td class="value" hours="' + (i + 1) + '">0/0</td>');
                        break;
                    case 15:
                    case 16:
                    case 17:
                        var name = i == 0 ? 'NS ' + hour + 'H Hiện tại' : 'NS ' + hour + 'H trước';
                        title.append('<td class="title">' + name + '</td>');
                        value.append('<td class="value" hours="' + (i + 1) + '">0/0</td>');
                        break;
                }

            });
            $('#tb_qlns #footer').append(title);
            $('#tb_qlns #footer').append(value);
        }
    }
}


$(document).ready(function () {
    var LCDNS_N = new GPRO.LCDNS_N();
    LCDNS_N.Init();
})