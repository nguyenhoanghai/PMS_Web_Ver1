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
            GetProductivity: '/Productivity/GetKanBanInfo',
            GetBTPHC: '/Home/GetBTPHC_Struct',
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
        GetBTPHC_Struct();
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.ListLineId = listLineId;
        GetLCDConfigInfo(function (data) {
            if (data != null) {
                $.each(data, function (i, obj) {
                    switch (obj.Name) {
                        case "WebPageHeight":
                            var value = obj.Value;
                            $('#tbKB_sh').css('height', value + 'px');
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
     //   var productivity = setInterval(function () { GetProductivity() }, 2000);
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
                                    $('#tbKB_sh .header').css('height', v.SizePercent + "%");
                                    break;
                                case 2:
                                    $('#tbKB_sh .content').css('height', v.SizePercent + "%");
                                    break;

                            }
                            break;
                        case 'tblpanelHeader':
                            //switch (parseInt(v.ColumnInt)) {
                            //    case 1:
                            //         $('#tbKB_sh #header .label1').css('width', v.SizePercent + "%");
                            //        var sheet = document.createElement('style')
                            //        var css = "#tbKB_sh #KanBan_Ticker .value1 {width:" + (v.SizePercent - 0.2) + "%;";
                            //        sheet.innerHTML = css;
                            //        document.body.appendChild(sheet);
                            //        break;
                            //    case 2:
                            //        $('#tbKB_sh #header .label2').css('width', v.SizePercent + "%");
                            //        var sheet = document.createElement('style')
                            //        var css = "#tbKB_sh #KanBan_Ticker .value2 {width:" + (v.SizePercent - 0.2) + "%;";
                            //        sheet.innerHTML = css;
                            //        document.body.appendChild(sheet);
                            //        break;
                            //    case 3:
                            //        $('#tbKB_sh_sh #header .label3').css('width', v.SizePercent + "%");
                            //        var sheet = document.createElement('style')
                            //        var css = "#tbKB_sh #KanBan_Ticker .value3 {width:" + (v.SizePercent - 0.2) + "%;";
                            //        sheet.innerHTML = css;
                            //        document.body.appendChild(sheet);
                            //        break;
                            //    case 4:
                            //      $('#tbKB_sh #header .label4').css('width', v.SizePercent + "%");
                            //        var sheet = document.createElement('style')
                            //        var css = "#tbKB_sh #KanBan_Ticker .value4 {width:" + (v.SizePercent - 0.2) + "%;";
                            //        sheet.innerHTML = css;
                            //        document.body.appendChild(sheet);
                            //        break;
                            //    case 5:
                            //         $('#tbKB_sh #header .label5').css('width', v.SizePercent + "%");
                            //        var sheet = document.createElement('style')
                            //        var css = "#tbKB_sh #KanBan_Ticker .value5 {width:" + (v.SizePercent - 0.2) + "%;";
                            //        sheet.innerHTML = css;
                            //        document.body.appendChild(sheet);
                            //        break;
                            //    case 6:
                            //        $('#tbKB_sh #header .label6').css('width', v.SizePercent + "%");
                            //        var sheet = document.createElement('style')
                            //        var css = "#tbKB_sh #KanBan_Ticker .value6 {width:" + (v.SizePercent) + "%;";
                            //        sheet.innerHTML = css;
                            //        document.body.appendChild(sheet);
                            //        break;
                            //}
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
                            $('#tbKB_sh #header').css('background-color', v.BackColor);
                            break;
                        case 'panelContent':
                            $('#tbKB_sh #KanBan_Ticker').css('background-color', v.BackColor);
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
                                    $('#tbKB_sh #header .title').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tbKB_sh #header .title').css('font-weight', "bold");
                                    else
                                        $('#tbKB_sh #header .title').css('font-weight', "");
                                    $('#tbKB_sh #header .title').css('color', v.Color);
                                    $('#tbKB_sh #header .title').css('background-color', 'transparent');
                                    break;
                            }
                            break;
                        case 'tblPanelContent':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    var sheet = document.createElement('style')
                                    var css = "#tbKB_sh #KanBan_Ticker .title {background-color:" + v.BackColor + ";";
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
                                    var css = "#tbKB_sh #KanBan_Ticker .value {background-color:" + v.BackColor + ";";
                                    css += "font-size:" + v.Size + "px;";
                                    // css += "line-height:" + v.Size + "px !important;";
                                    if (v.Bold)
                                        css += "font-weight:bold;";
                                    else
                                        css += "font-weight:;";
                                    css += "color:" + v.Color + ";";
                                    css += "}";
                                    //  css += "#tbKB_sh #KanBan_Ticker .title {line-height:" + v.Size + "px !important;}"
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                            }
                            break;
                    }
                });
             //   BuildLabelForPanelContent();
            }
        })
    }

    function BuildLabelForPanelContent() {
        GetTableLayoutPanel(4, function (data) {
            var listTr = $('#tbKB_sh #header td');
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

    function GetBTPHC_Struct() {
        $.ajax({
            url: Global.UrlAction.GetBTPHC,
            type: 'POST',
            data: '',
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null && data.Data.length > 0) {
                    var root = $('#tbKB_sh #header .title');
                    root.empty();
                    root.append('<td rowint="1" class="label1">C</td>');
                    root.append('<td rowint="2" class="label2">Mã Hàng</td>');
                    root.append('<td rowint="3" class="label3">SL Đ.H</td>');
                    var j = 4;
                    $.each(data.Data, function (i, item) {
                        root.append('<td rowint="' + j + '" class="label' + j + '">' + item.Name + '</td>');
                        j++;
                    });
                    root.append('<td rowint="' + j + '" class="label' + j + '">GIAO CHUYỀN</td>');
                    j++;
                    root.append('<td rowint="' + j + '" class="label' + j + '">BTP TỒN</td>');
                    j++;
                    root.append('<td rowint="' + j + '" class="label' + j + '">BTP /CHUYỀN</td>');
                  //  j++;
                  //  root.append('<td rowint="' + j + '" class="label' + j + '">TT BTP</td>');
                }
            }
        });
    }


    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'listLineId': Global.Data.ListLineId, 'tableTypeId': 1, 'includeBTPHC': true }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                $('#tbKB_sh #KanBan_Ticker tbody').html('<tr id="notfound"><td colspan="6">Không tìm thấy dữ liệu phân công cho chuyền.</td></tr>');
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
        $('#tbKB_sh #KanBan_Ticker').empty();
        var height = 100 / parseInt(Global.Data.paging);

        $.each(datas, function (i, obj) {
            //if (i < Global.Data.paging) {
            var $tr = $('<li style="height:' + Global.Data.rowHeight + 'px"></li>');
            var $ul = $('<ul class="r_' + i + '"></ul>');
            $ul.append('<li class="title value1 "><span>' +  obj.LineName  + '</span></li>');
            $ul.append('<li class="title value2 "><span>' + (obj.ProductName.length > 8 ? (obj.ProductName.substring(0, 8) + '...') : obj.ProductName) + '</span></li>');
            $ul.append('<li class="title value3 "><span>' + obj.ProductionPlans  + '</span></li>');
            var j = 4;
            $.each(obj.BTPHC_Structs, function (ii, iObj) {
                $ul.append('<li class="title value'+j+' "><span>' + iobj.Id + '</span></li>');
            });
            $ul.append('<li class="value value' + j + ' font-dt"><span>' + obj.BTPOnDay + '</span></li>');
            j++;
            $ul.append('<li class="value value' + j + ' font-dt"><span>' + (obj.BTPInLine < 0 ? 0 : obj.BTPInLine) + '</span></li>');
            j++;
            $ul.append('<li class="value value' + j + ' font-dt"><span>' + 0 + '</span></li>');
            j++;
            $ul.append('<li class="value value' + j + '"><div class="btp_status" style=" background-color: ' + obj.StatusColor + ';"></div></li>');//
             
            $tr.append($ul);
            $tr.append('<div style="clear:left"></div>');
            $('#tbKB_sh #KanBan_Ticker').append($tr);
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
            $('#tbKB_sh .r_' + i + ' .value1').html('<span>' + (obj.LineName ) + '</span>');
            $('#tbKB_sh .r_' + i + ' .value2').html('<span>' + (obj.ProductName.length > 8 ? (obj.ProductName.substring(0, 8) + '...') : obj.ProductName) + '</span>  ');
            $('#tbKB_sh .r_' + i + ' .value3').html('<span>' + obj.ProductionPlans + '</span>');
            var j = 4;
            $.each(obj.BTPHC_Structs, function (ii, iObj) {
                $('#tbKB_sh .r_' + i + ' .value' + j).html('<span>' + iObj.Note + '</span>');
                j++;
            });
            $('#tbKB_sh .r_' + i + ' .value' + j).html('<span>' + obj.BTPOnDay + '</span>');
            $('#tbKB_sh .r_' + i + ' .value' + j).removeClass('ĐỎ XANH VÀNG');
            $('#tbKB_sh .r_' + i + ' .value' + j).addClass(obj.LightBTPConLai);
            j++;
            $('#tbKB_sh .r_' + i + ' .value' + j).html(' <span>' + (obj.BTPInLine < 0 ? 0 : obj.BTPInLine) + '</span> ');
            j++;
            $('#tbKB_sh .r_' + i + ' .value' + j).html('<span >' + 0 + '</span>');
            $('#tbKB_sh .r_' + i + ' .value' + j).removeClass('ĐỎ XANH VÀNG');
            $('#tbKB_sh .r_' + i + ' .value' + j).addClass(obj.StatusColor);
            
        });
    }


    //dang slide
    function DrawContentAndSetData_Slide(datas) {
        $('#kb_stop').click();
        $('#tbKB_sh #KanBan_Ticker').empty();
        var height = 100 / parseInt(Global.Data.rowPerPage);

        var z = 0;
        var str = '';
        for (var i = 0; i < Math.ceil(datas.length / Global.Data.rowPerPage) ; i++) {
            str += '<div class="slide-image"><ul id="KanBan_Ticker">';
            for (var y = 0; y < Global.Data.rowPerPage; y++) {
                var obj = datas[z];

                if (obj != null) {
                    str += ('<li style="height:' + Global.Data.rowHeight + 'px"><ul class="r_' + z + '">');
                    str += ('<li class="title value1 "><span>' + obj.LineName + '</span></li>');
                    str += ('<li class="title value2 "><span>' + (obj.ProductName.length > 8 ? (obj.ProductName.substring(0, 8) + '...') : obj.ProductName) + '</span></li>');
                    str += ('<li class="title value3 font-dt"><span>' + obj.ProductionPlans + '</span></li>');
                    var j = 4;
                    $.each(obj.BTPHC_Structs, function (ii, iObj) {
                        str += ('<li class="title value' + j + ' font-dt"><span>' + iObj.Note + '</span></li>');
                        j++;
                    });
                    str += ('<li class="value value' + j + ' font-dt"><span>' + obj.BTPOnDay + '</span></li>');
                    j++;
                    str += ('<li class="value value' + j + ' font-dt"><span>' + (obj.BTPInLine < 0 ? 0 : obj.BTPInLine) + '</span></li>');
                    j++;
                    str += ('<li class="value value' + j + ' font-dt"><span>' + 0 + '</span></li>');
                   // j++;
                   // str += ('<li class="value value' + j + '" style=" background-color: ' + obj.StatusColor + ';" > </li>');//

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
}


$(document).ready(function () {
    var LCDTH = new GPRO.LCDTH();
    LCDTH.Init();
})