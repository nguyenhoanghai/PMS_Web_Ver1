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
GPRO.namespace('LCDTH');
GPRO.LCDTH = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',
            GetProductivity: '/Productivity/GetInforLCDTH_New',
        },
        Data: {
            TableType: 7,
            ListLineId: [],
            isDrawContent: false,
            paging: 0,
            TimeToChangeRow: 0,
            rowHeight: 0,
            oldRow: 0,
            rowPerPage: 8
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.ListLineId = listLineId;
        GetLCDConfigInfo(function (data) {
            if (data != null) {
                $.each(data, function (i, obj) {
                    switch (obj.Name) {
                        case "WebPageHeight":
                            var value = obj.Value;
                            $('#tbKB').css('height', value + 'px');
                            break;
                        case "SoDongHienThiLCDKanBan":
                            Global.Data.paging = obj.Value; break;
                        case "ThoiGianNhayChuyen":
                            Global.Data.TimeToChangeRow = obj.Value; break;
                        case 'ChieuCaoDong_LCDKanBan':
                            Global.Data.rowHeight = parseInt(obj.Value); break;
                    }
                });
            }
        });
        BuildTableLayOut();
        RegisterEvent();
    }

    var RegisterEvent = function () {
        GetProductivity();
        //  var productivity = setInterval(function () { GetProductivity() }, 2000);
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
                                    $('#tbKB .header').css('height', v.SizePercent + "%");
                                    break;
                                case 2:
                                    $('#tbKB .content').css('height', v.SizePercent + "%");
                                    break;

                            }
                            break;
                        case 'tblpanelHeader':
                            switch (parseInt(v.ColumnInt)) {
                                case 1: $('#tbKB #header .label1').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .value1 {width:" + (v.SizePercent - 0.2) + "%;";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 2: $('#tbKB #header .label2').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .value2 {width:" + (v.SizePercent - 0.2) + "%;";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 3: $('#tbKB #header .label3').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .value3 {width:" + (v.SizePercent - 0.2) + "%;";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 4: $('#tbKB #header .label4').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .value4 {width:" + (v.SizePercent - 0.2) + "%;";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 5: $('#tbKB #header .label5').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .value5 {width:" + (v.SizePercent - 0.2) + "%;";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 6: $('#tbKB #header .label6').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .value6 {width:" + (v.SizePercent) + "%;";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                            }
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
                            $('#tbKB #header').css('background-color', v.BackColor);
                            break;
                        case 'panelContent':
                            $('#tbKB #KanBan_Ticker').css('background-color', v.BackColor);
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
                        case 'tblPanelHeader':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    $('#tbKB #header .title').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tbKB #header .title').css('font-weight', "bold");
                                    else
                                        $('#tbKB #header .title').css('font-weight', "");
                                    $('#tbKB #header .title').css('color', v.Color);
                                    $('#tbKB #header .title').css('background-color', 'transparent');
                                    break;
                            }
                            break;
                        case 'tblPanelContent':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .title {background-color:" + v.BackColor + ";";
                                    css += "font-size:" + v.Size + "px;";
                                    css += "line-height:" + v.Size + "px !important;";
                                    if (v.Bold)
                                        css += "font-weight:bold;";
                                    else
                                        css += "font-weight:;";
                                    css += "color:" + v.Color + ";";
                                    css += "}";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 2:
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB #KanBan_Ticker .value {background-color:" + v.BackColor + ";";
                                    css += "font-size:" + v.Size + "px;";
                                    // css += "line-height:" + v.Size + "px !important;";
                                    if (v.Bold)
                                        css += "font-weight:bold;";
                                    else
                                        css += "font-weight:;";
                                    css += "color:" + v.Color + ";";
                                    css += "}";
                                    //  css += "#tbKB #KanBan_Ticker .title {line-height:" + v.Size + "px !important;}"
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
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
            var listTr = $('#tbKB #header td');
            if (data != null && listTr != null) {
                $.each(data, function (i, v) {
                    $.each(listTr, function (j, va) {
                        if (parseInt($(va).attr("rowint")) == v.IntRowTBLPanelContent) {
                            $(va).html(v.LabelName);
                            $(va).attr("action", "cmd");
                            $(va).attr("cmd", v.SystemValueName.trim());
                        }
                    });
                });
            }
        })
    }

    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'listLineId': Global.Data.ListLineId, 'tableTypeId': 1 }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                $('#tbKB #KanBan_Ticker tbody').html('<tr id="notfound"><td colspan="6">Không tìm thấy dữ liệu phân công cho chuyền.</td></tr>');
                if (data.Data != null && data.Data.length > 0) {
                    if (data.Data != null && data.Data.length > 0) {
                        if (Global.Data.oldRow != data.Data.length) {
                            DrawContentAndSetData_Slide(data.Data);
                            //   DrawContentAndSetData(data.Data);
                            Global.Data.oldRow = data.Data.length;
                        }
                        else
                            SetDataWithoutDrawContent(data.Data);
                    }
                }
            }
        });
    }

    // dang cuon song
    function DrawContentAndSetData(datas) {
        $('#kb_stop').click();
        $('#tbKB #KanBan_Ticker').empty();
        var height = 100 / parseInt(Global.Data.paging);

        $.each(datas, function (i, obj) {
            //if (i < Global.Data.paging) {
            var $tr = $('<li style="height:' + Global.Data.rowHeight + 'px"></li>');
            var $ul = $('<ul class="r_' + i + '"></ul>');
            $ul.append('<li class="title value1 "><span>' + (obj.LineName + '|' + (obj.ProductName.length > 8 ? (obj.ProductName.substring(0, 8) + '...') : obj.ProductName)) + '</span></li>');
            $ul.append('<li class="title value2  font-dt"><span>' + obj.KCS + '<span class="blue">|</span>' + obj.DMN + '</span> </li>');
            $ul.append('<li class="value value3 font-dt"> <span>' + obj.NhipTT + '<span class="blue">|</span>' + obj.NhiSX + '</span></li>');
            $ul.append('<li class="value value4 font-dt"><span>' + obj.LK_KCS + '<span class="blue">|</span>' + obj.SLCL + '</span></li>');
            $ul.append('<li class="value value5 font-dt"><span>' + obj.Lean + '<span class="blue">|</span>' + (obj.BTPInLine < 0 ? 0 : obj.BTPInLine) + '</span></li>');
            $ul.append('<li class="value value6  "><span >' + obj.NSHienTai + '</span></li>');//
            $ul.append('<li class="value value7 "><span >' + obj.NSHienTai + '</span></li>');//
            $ul.append('<li class="value value8  "><span >' + obj.NSHienTai + '</span></li>');//
            $tr.append($ul);
            $tr.append('<div style="clear:left"></div>');
            $('#tbKB #KanBan_Ticker').append($tr);
            //  }
        });

        $('#KanBan_Ticker ul li').css('height', $('#KanBan_Ticker li').css('height'));
        $('#KanBan_Ticker ul li').css('line-height', $('#KanBan_Ticker ul li').css('height'));
        Global.Data.isDrawContent = true;
        SetAutoScroll(datas.length, Global.Data.paging, Global.Data.TimeToChangeRow, 2)
    }

    //dang cuon song 
    function SetDataWithoutDrawContent(datas) {
        $.each(datas, function (i, obj) {
            $('#tbKB .r_' + i + ' .value1').html('<span>' + getValue(obj, 0) + '</span>');
            $('#tbKB .r_' + i + ' .value2').html('<span>' + getValue(obj, 1) + '</span>  ');
            $('#tbKB .r_' + i + ' .value3').html('<span>' + getValue(obj, 2) + '</span>');
            $('#tbKB .r_' + i + ' .value4').html('<span>' + getValue(obj, 3) + '</span>');
            $('#tbKB .r_' + i + ' .value5').html(' <span>' + getValue(obj, 4) + '</span> ');
            $('#tbKB .r_' + i + ' .value6').html('<span >' + getValue(obj, 5) + '</span>');
            $('#tbKB .r_' + i + ' .value7').html('<span >' + getValue(obj, 6) + '</span>');
            $('#tbKB .r_' + i + ' .value8').html('<span >' + getValue(obj, 7) + '</span>');
            $('#tbKB .r_' + i + ' .value6').removeClass('ĐỎ,XANH,VÀNG');
            $('#tbKB .r_' + i + ' .value6').addClass(obj.mauDen);
        });
    }


    //dang slide
    function DrawContentAndSetData_Slide(datas) {
        $('#kb_stop').click();
        $('#tbKB #KanBan_Ticker').empty();
        var height = 100 / parseInt(Global.Data.rowPerPage);
         
        var z = 0;
        var str = '';
        for (var i = 0; i < Math.ceil(datas.length / Global.Data.rowPerPage) ; i++) {
            str += '<div class="slide-image"><ul id="KanBan_Ticker">';
            for (var y = 0; y < Global.Data.rowPerPage; y++) {
                var obj = datas[z];

                if (obj != null) {                     
                    str += ('<li style="height:' + Global.Data.rowHeight + 'px"><ul class="r_' + z + '">');
                    str += ('<li class="title value1 "><span>' +  getValue(obj,0) + '</span></li>');
                    str += ('<li class="title value2  ">' + getValue(obj, 1) + ' </li>');
                    str += ('<li class="value value3 font-dt">' + getValue(obj, 2) + ' </li>');
                    str += ('<li class="value value4 font-dt">' + getValue(obj, 3) + ' </li>');
                    str += ('<li class="value value5 font-dt">' + getValue(obj, 4) + ' </li>');
                    str += ('<li class="value value6 font-dt "><span >' + getValue(obj, 5) + ' </span></li>');
                    str += ('<li class="value value7 font-dt "><span >' + getValue(obj, 6) + '</span></li>');
                    str += ('<li class="value value8 font-dt "><span >' + getValue(obj, 7) + '</span></li>');
                    str += ('<div style="clear:left"></div>');
                    str += ('</ul></li>');
                }
                else
                    break;
                z++;
            }

            str += '</ul></div>';
        }
        $('#vtemslideshow1').empty().append(str);
        $('#KanBan_Ticker ul li').css('height', $('#KanBan_Ticker li').css('height'));
        $('#KanBan_Ticker ul li').css('line-height', $('#KanBan_Ticker ul li').css('height'));

        //  Global.Data.isDrawContent = true;
        //   SetAutoScroll(datas.length, Global.Data.paging, Global.Data.TimeToChangeRow, 2)
    }


    function getValue(obj, columIndex) {
        var listValue = $('#tbKB #header [action="cmd"]');

        var arr = ($(listValue[columIndex]).attr("cmd")).split('|');
        var str = '';
        for (var i = 0; i < arr.length; i++) {
            if (str != '')
                str += '<span style="color:blue">|</span> ';
            switch (arr[i]) {
                case 'ProductName': str +=  (obj.ProductName.length > 8 ? (obj.ProductName.substring(0, 8) + '...') : obj.ProductName); break;
                case 'LineName': str += obj.LineName; break;
                case 'LDDB': str += obj.LDDB; break;
                case 'LDTT': str += obj.LDTT; break;
                case 'SLKH': str += obj.SLKH; break;
                case 'SLCL': str += obj.SLCL; break;
                case 'DMN': str += obj.DMN; break;
                case 'KCS': str += obj.KCS; break;
                case 'TC': str += obj.TC; break;
                case 'Error': str += obj.Error; break;
                case 'BTP': str += obj.BTP; break;
                case 'BTPInLine': str += obj.BTPInLine; break;
                case 'BTPInLine_BQ': str += obj.BTPInLine_BQ; break;
                case 'DoanhThuBQ': str += obj.DoanhThuBQ; break;
                case 'DoanhThuBQ_T': str += obj.DoanhThuBQ_T; break;
                case 'DoanhThu': str += obj.DoanhThu; break;
                case 'DoanhThu_T': str += obj.DoanhThu_T; break;
                case 'DoanhThuKH_T': str += obj.DoanhThuKH_T; break;
                case 'KCS_KH_T': str += obj.KCS_KH_T; break;
                case 'DoanhThuDM': str += obj.DoanhThuDM; break;
                case 'DoanhThuDM_T': str += obj.DoanhThuDM_T; break;
                case 'KCSKH_T': str += obj.KCSKH_T; break;
                case 'ThuNhapBQ': str += obj.ThuNhapBQ; break;
                case 'ThuNhapBQ_T': str += obj.ThuNhapBQ_T; break;
                case 'SLKHToNow': str += obj.SLKHToNow; break;
                case 'NhipSX': str += obj.NhipSX; break;
                case 'NhipTT': str += obj.NhipTT; break;
                case 'NhipTC': str += obj.NhipTC; break;
                case 'tiLeThucHien': str += obj.tiLeThucHien; break;
                case 'nangSuatGioTruoc': str += obj.nangSuatGioTruoc; break;
                case 'nangSuatGioHienTai': str += obj.nangSuatGioHienTai; break;
                case 'Hour_ChenhLech_Day': str += obj.Hour_ChenhLech_Day; break;
                case 'Hour_ChenhLech': str += obj.Hour_ChenhLech; break;
                case 'KCS_QuaTay': str += obj.KCS_QuaTay; break;
                case 'LK_KCS_QuaTay': str += obj.LK_KCS_QuaTay; break;
                case 'LK_TC': str += obj.LK_TC; break;
                case 'LK_KCS': str += obj.LK_KCS; break;
                case 'LK_BTP': str += obj.LK_BTP; break;
                case 'Lean': str += obj.Lean; break;
                case 'HieuSuat': str += obj.HieuSuat; break;
                case 'TiLeLoi_D': str += obj.TiLeLoi_D; break;
                case 'DMHour': str += obj.DMHour; break;
                case 'KCSHour': str += obj.KCSHour; break;
            }
        }
        return str;
        //$(v).html(str);

        //$.each(listValue, function (i, v) {

        //});
    }
}


$(document).ready(function () {
    var LCDTH = new GPRO.LCDTH();
    LCDTH.Init();
})