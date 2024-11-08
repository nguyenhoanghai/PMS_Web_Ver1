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
GPRO.namespace('CL_One');
GPRO.CL_One = function () {
    var Global = {
        UrlAction: {
            GetData: '/Productivity/GetLCDInfo',
            GetCheckListConfig: '/Productivity/GetCheckListConfig',
            GetProductivity: '/Productivity/GetProductivityByLineId_New',
        },
        Data: {
            TableType: 10,
            LineId: 0,
            user: null,
            IsDraw: false,
            paging: 4,
            TimeToChangeRow: 3000,
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
            if (listLineId != null && listLineId.length > 0) {
                $.each(listLineId, function (i, item) {
                    Global.Data.ListLines.push(item);
                });
            }
        }
        Global.Data.LineId = listLineId[Global.Data.currentIndex];
        $('#box_video').attr('line', Global.Data.LineId);

        setInterval(function () {
            Global.Data.currentIndex = ((Global.Data.currentIndex + 1) > (Global.Data.ListLines.length-1)) ? 0 : (Global.Data.currentIndex + 1);
            Global.Data.LineId = listLineId[Global.Data.currentIndex];
            $('#box_video').attr('line', Global.Data.LineId);
        }, Global.Data.IntervalChange);


        BuildTableLayOut();
        RegisterEvent();
    }

    var RegisterEvent = function () {

        GetProductivity()
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
                $('#tb_ns_new').css('height', data.PageHeight + 'px');
                $('#tb_ns_new #lineName').html(data.LineName);
                Global.Data.ShowType = data.ShowNSType;
                Global.Data.TimesGetNS = data.TimesGetNS;
                Global.Data.KhoangCachGetNSOnDay = data.KhoangCachGetNSOnDay;
                Global.Data.paging = parseInt(data.paging);
                Global.Data.TimeToChangeRow = data.TimeToChangeRow;
                Global.Data.NSGio_Formula = data.NSG_Formula;
                veKhung(data.LayoutPanelConfig);
                DrawBackgroundColor(data.Panel_Background);
                SetFontStyle(data.FontStyle);
                DrawBody(data.BodyTitle, data.rowHeight, data.FooterTitle, data.ShowNSType, data.KhoangCachGetNSOnDay);
                // SetFooterTitle(data.FooterTitle, data.ShowNSType, data.KhoangCachGetNSOnDay);
            }
        })
    }

    function DrawBody(data, rowHeight, footers, showNSType, KhoangCachGetNSOnDay) {
        var height = Global.Data.paging < data.length ? (100 / parseInt(Global.Data.paging)) : (data.length == 0 ? 100 : 100 / data.length);

        if (data.length > 0 || footers.length > 0) {
            $('#LCD_NS_New_ticker').html("");

            for (var i = 0; i < data.length; i += 2) {
                var li = $('<li style="height:' + rowHeight + 'px"></li>');
                var ul = $('<ul style="float:left" id="r_' + i + '"></ul>');
                var r = i;
                ul.append('<li  class="title value col_1" ><span class="Pos_1" id="c_' + r + '_00" tt="' + data[i].SystemValueName.trim() + '">' + data[i].LabelName + '</span></li>');
                ul.append('<li style="background:black; " class="value col_2" ><span class="Pos_2" id="c_' + r + '_' + i + '" action="cmd" cmd="' + data[i].SystemValueName.trim() + '">' + i + '</span></li>');

                //
                r++;
                if (data[r] != null && typeof (data[r]) != 'undefined') {
                    ul.append('<li class="title value col_3" ><span class="Pos_3" id="c_' + r + '_00" tt="' + data[r].SystemValueName.trim() + '">' + data[r].LabelName + '</span></li>');
                    ul.append('<li style="background:black; " class="value col_4"  ><span class="Pos_4" id="c_' + r + '_' + i + '" action="cmd" cmd="' + data[r].SystemValueName.trim() + '">' + r + '</span></li>');
                }
                else {
                    if (footers.length > 0) {
                        ul.append('<li  class="title value col_3" ><span class="Pos_3" id="c_' + r + '_00" tt="0000">' + footers[0].Name + '</span></li>');
                        ul.append('<li style="background:black; " class="value col_4"  ><span id="NSG_0" class="Pos_4" cmd="NSG">0</span></li>');
                    }
                }

                li.append(ul);
                li.append('<div style="clear:left"></div>');
                $('#LCD_NS_New_ticker').append(li);
            }

            for (var i = ((data.length % 2) == 0 ? 0 : 1) ; i < footers.length; i += 2) {
                if (1 == 1) {
                    if (footers[i].IsShow || footers[i + 1].IsShow) {
                        var li = $('<li style="height:' + rowHeight + 'px"></li>');
                        var ul = $('<ul style="float:left" id="r_' + i + '"></ul>');

                        ul.append('<li  class="title value col_1" ><span class="Pos_1" id="NSG_TT_0"  >' + (i == 0 ? footers[i].Name : footers[i - 1].Name) + '</span></li>');
                        ul.append('<li style="background:black; " class="value col_2" ><span class="Pos_2" id="NSG_0"  cmd="NSG">0</span></li>');

                        //  
                        ul.append('<li class="title value col_3" ><span class="Pos_3" id="NSG_TT_1">' + (i == 0 ? footers[i + 1].Name : footers[i].Name) + '</span></li>');
                        ul.append('<li style="background:black; " class="value col_4"  ><span class="Pos_4" id="NSG_1"  cmd="NSG">0</span></li>');

                        li.append(ul);
                        li.append('<div style="clear:left"></div>');
                        $('#LCD_NS_New_ticker').append(li);
                        return false;
                    }
                }
                else {
                    var li = $('<li style="height:' + rowHeight + 'px"></li>');
                    var ul = $('<ul style="float:left" id="r_' + i + '"></ul>');
                    var r = i;
                    ul.append('<li  class="title value col_1" ><span class="Pos_1"   >' + footers[i].Name + '</span></li>');
                    ul.append('<li style="background:black; " class="value col_2" ><span class="Pos_2" id="NSG_' + i + '"  cmd="NSG">0</span></li>');

                    //
                    r++;
                    if (footers[r] != null && typeof (footers[r]) != 'undefined') {
                        ul.append('<li class="title value col_3" ><span class="Pos_3" >' + footers[r].Name + '</span></li>');
                        ul.append('<li style="background:black; " class="value col_4"  ><span class="Pos_4" id="NSG_' + r + '"  cmd="NSG">0</span></li>');
                    }
                    else {
                        ul.append('<li  class="title value col_3" ><span id="c_' + r + '_00" tt="0000"></span></li>');
                        ul.append('<li style="background:black; " class="value col_4"  ><span class="Pos_4" cmd="NSG"></span></li>');
                    }

                    li.append(ul);
                    li.append('<div style="clear:left"></div>');
                    $('#LCD_NS_New_ticker').append(li);
                }
            }
            $('#tb_ns_new li ul li').css('height', $('#tb_ns_new li').css('height'));

            var tong = data.length > 0 ? data.length / 2 : 0;
            tong += (1 == 1 ? 1 : (footers.length > 0 ? footers.length / 2 : 0));

            if (Global.Data.paging < tong)
                SetAutoScroll(tong, Global.Data.paging, Global.Data.TimeToChangeRow, 5);
        }
        else {
            $('#LCDCT_CL_ticker').append('<li style="height:' + height + '%"><ul><li style="width:100%"><span>Không có Dữ Liệu</span></li></ul><div style="clear:left"></div></li>');
            Global.Data.IsDraw = false;
        }
    }

    function SetFontStyle(data) {
        var style = $('<style></style>');
        $.each(data, function (i, v) {
            switch (v.TableLayoutPanelName.trim()) {
                case 'tblpanelHeader':
                    switch (parseInt(v.Position)) {
                        case 2:
                            style.append('#tb_ns_new #header .title {line-height :' + v.Size + 'px ;font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 3:
                            style.append('#tb_ns_new #header .time {font-size :' + v.Size + 'px ;line-height :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
                case 'tblpanelContent':
                    switch (parseInt(v.Position)) {
                        case 1:
                            style.append('#tb_ns_new #LCD_NS_New_ticker .Pos_1 {font-size :' + v.Size + 'px ;line-height:' + v.Size + 'px ; font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 2:
                            style.append('#tb_ns_new #LCD_NS_New_ticker .Pos_2 {font-family:DS-DIGIB;font-size :' + v.Size + 'px ;line-height:' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 3:
                            style.append('#tb_ns_new #LCD_NS_New_ticker .Pos_3 {font-size :' + v.Size + 'px ;line-height:' + v.Size + 'px ; font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 4:
                            style.append('#tb_ns_new #LCD_NS_New_ticker .Pos_4 {font-family:DS-DIGIB;font-size :' + v.Size + 'px ;line-height:' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
            }
        });
        $('head').append(style);
    }

    function veKhung(data) {
        var style = $('<style></style>');
        $.each(data, function (i, v) {
            switch (v.TableLayoutPanelName.trim()) {
                case 'tblpanelBody':
                    switch (parseInt(v.RowInt)) {
                        case 1:
                            style.append('#tb_ns_new #cl-title{ height:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_ns_new #ul-content{ height:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelHeader':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_ns_new #header .logo { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_ns_new #header .title { width:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_ns_new #header .ledalert { width:' + v.SizePercent + '%}');
                            break;
                        case 4:
                            style.append('#tb_ns_new #header .time { width:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelContent':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_ns_new #ul-content .col_1 { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_ns_new #ul-content .col_2 { width:' + v.SizePercent + '%  }');
                            break;
                        case 3:
                            style.append('#tb_ns_new #ul-content .col_3 { width:' + v.SizePercent + '%}');
                            break;
                        case 4:
                            style.append('#tb_ns_new #ul-content .col_4 { width:' + v.SizePercent + '%  }');
                            break;
                    }
                    break;
            }
        });

        $('head').append(style);
    }

    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.LineId, 'tableTypeId': 1, 'hienThiNSGio': Global.Data.ShowType, 'TimesGetNS': Global.Data.TimesGetNS, 'KhoangCachGetNSOnDay': Global.Data.KhoangCachGetNSOnDay }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    $('#tb_ns_new #lineName').html(data.Data.chuyen + ' - Mặt Hàng: ' + data.Data.maHang);
                    switch (data.Data["mauDen"]) {
                        case "ĐỎ": $('#tb_ns_new #lightAlert').attr('src', '../Content/ShowLCD/circle_red.png'); break;
                        case "VÀNG": $('#tb_ns_new #lightAlert').attr('src', '../Content/ShowLCD/circle_yellow.png'); break;
                        case "XANH": $('#tb_ns_new #lightAlert').attr('src', '../Content/ShowLCD/circle_green.png'); break;
                    }
                    var listValue = $('#tb_ns_new #LCD_NS_New_ticker [action="cmd"]');
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
                    }

                    var NSG = $('#tb_ns_new #LCD_NS_New_ticker [cmd="NSG"]');
                    if (data.Data.listWorkHours != null && data.Data.listWorkHours.length > 0 && Global.Data.NSGio_Formula != null && Global.Data.NSGio_Formula.length > 0) {
                        $.each(data.Data.listWorkHours, function (i, item) {
                            if (1 == 1) {
                                if (item.IsShow) {
                                    var value = '';

                                    var first = i == 0 ? item : data.Data.listWorkHours[i - 1];

                                    $.each(Global.Data.NSGio_Formula, function (ii, cf) {
                                        switch (cf) {
                                            case 'TC': value += value == '' ? first.TC : '|' + first.TC; break;
                                            case 'TH': value += value == '' ? first.KCS : '|' + first.KCS; break;
                                            case 'DM': value += value == '' ? first.NormsHour : '|' + first.NormsHour; break;
                                            case 'Error': value += value == '' ? first.Error : '|' + first.Error; break;
                                            case 'BTP': value += value == '' ? first.BTP : '|' + first.BTP; break;
                                        }
                                    });
                                    $('#NSG_0').html(value);
                                    $('#NSG_TT_0').html(i == 0 ? item.Name : data.Data.listWorkHours[i - 1].Name);

                                    //
                                    var second = i == 0 ? data.Data.listWorkHours[i + 1] : item;
                                    value = '';
                                    $.each(Global.Data.NSGio_Formula, function (ii, cf) {
                                        switch (cf) {
                                            case 'TC': value += value == '' ? second.TC : '|' + second.TC; break;
                                            case 'TH': value += value == '' ? second.KCS : '|' + second.KCS; break;
                                            case 'DM': value += value == '' ? second.NormsHour : '|' + second.NormsHour; break;
                                            case 'Error': value += value == '' ? second.Error : '|' + second.Error; break;
                                            case 'BTP': value += value == '' ? second.BTP : '|' + second.BTP; break;
                                        }
                                    });
                                    $('#NSG_1').html(value);
                                    $('#NSG_TT_1').html(i == 0 ? data.Data.listWorkHours[i + 1].Name : item.Name);
                                }
                            }
                            else {
                                var value = '';
                                $.each(Global.Data.NSGio_Formula, function (ii, cf) {
                                    switch (cf) {
                                        case 'TC': value += value == '' ? item.TC : '|' + item.TC; break;
                                        case 'TH': value += value == '' ? item.KCS : '|' + item.KCS; break;
                                        case 'DM': value += value == '' ? item.NormsHour : '|' + item.NormsHour; break;
                                        case 'Error': value += value == '' ? item.Error : '|' + item.Error; break;
                                        case 'BTP': value += value == '' ? item.BTP : '|' + item.BTP; break;
                                    }
                                });
                                $('#NSG_' + i).html(value)
                            }

                        });
                    }
                }
            }
        });
    }

    function DrawBackgroundColor(data) {
        var style = $('<style></style>');
        $.each(data, function (i, item) {
            switch (item.Name.trim()) {
                case 'panelHeader':
                    style.append('#tb_ns_new #cl-title {background-color :' + item.BackColor + '}');
                    break;
                case 'panelContent':
                    style.append('#tb_ns_new #ul-content{background-color :' + item.BackColor + '}');
                    break;
            }
        });
        $('head').append(style);
    }

}


$(document).ready(function () {
    var collec = new GPRO.CL_One();
    collec.Init();
})