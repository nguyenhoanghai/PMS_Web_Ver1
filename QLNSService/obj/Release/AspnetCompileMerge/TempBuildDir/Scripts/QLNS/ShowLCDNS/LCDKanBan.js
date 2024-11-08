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
GPRO.namespace('LCDKanBan');
GPRO.LCDKanBan = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',
            GetProductivity: '/Productivity/GetKanBanInfo',
        },
        Data: {
            TableType: 2,
            ListLineId: [],
            isDrawContent: false,
            paging: 0,
            TimeToChangeRow: 0
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
                    }
                });
            }
        });
        BuildTableLayOut();
        RegisterEvent();
    }

    var RegisterEvent = function () {
        GetProductivity();
        var productivity = setInterval(function () { GetProductivity() }, 1000);
        //$(window).resize(function () {
        //    $('#tbKB').css("height", window.innerHeight);
        //});
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
                                    //  css += "line-height:" + v.Size + "px !important;";
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
            data: JSON.stringify({ 'listLineId': Global.Data.ListLineId, 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                $('#tbKB #KanBan_Ticker tbody').html('<tr id="notfound"><td colspan="6">Không tìm thấy dữ liệu phân công cho chuyền.</td></tr>');
                if (data.Data != null && data.Data.length > 0) {
                    if (data.Data != null && data.Data.length > 0) {
                        if (Global.Data.isDrawContent)
                            SetDataWithoutDrawContent(data.Data);
                        else
                            DrawContentAndSetData(data.Data);
                    }
                }
            }
        });
    }

    function DrawContentAndSetData(datas) {
        $('#tbKB #KanBan_Ticker').empty();
        var height = 100 / parseInt(Global.Data.paging);

        $.each(datas, function (i, obj) {
            //if (i < Global.Data.paging) {
                var $tr = $('<li style="height:' + height + '%"></li>');
                var $ul = $('<ul class="r_' + i + '"></ul>');
                $ul.append('<li class="title value1 ">' + obj.LineName + '</li>');
                $ul.append('<li class="title value2 ">' + obj.ProductName + '</li>');
                $ul.append('<li class="value value3 font-dt">' + obj.BTPOnDay + '</li>');
                $ul.append('<li class="value value4 font-dt">' + obj.BTPTotal + '</li>');
                $ul.append('<li class="value value5 font-dt">' + obj.BTPBQ + '</li>');
                $ul.append('<li class="value value6"><div class="btp_status" style=" background-color: ' + obj.StatusColor + ';"></div></li>');//
                $tr.append($ul);
                $tr.append('<div style="clear:left"></div>');
                $('#tbKB #KanBan_Ticker').append($tr);
          //  }
        });

        $('#KanBan_Ticker ul li').css('height',  $('#KanBan_Ticker li').css('height'));
        $('#KanBan_Ticker ul li').css('line-height', $('#KanBan_Ticker ul li').css('height'));
        Global.Data.isDrawContent = true;
         SetAutoScroll(datas.length, Global.Data.paging, Global.Data.TimeToChangeRow,2)
        //var a = setInterval(function () { ; window.clearInterval(a); }, 3000);

    }

    function SetDataWithoutDrawContent(datas) {
        $.each(datas, function (i, obj) {
            $('#tbKB .r_' + i + ' .value1').html(obj.LineName);
            $('#tbKB .r_' + i + ' .value2').html(obj.ProductName);
            $('#tbKB .r_' + i + ' .value3').html(obj.BTPOnDay);
            $('#tbKB .r_' + i + ' .value4').html(obj.BTPTotal);
            $('#tbKB .r_' + i + ' .value5').html(obj.BTPBQ);
            $('#tbKB .r_' + i + ' .value6').html('<div class="btp_status" style=" background-color: ' + obj.StatusColor + ';"></div>');
        });
    }
}


$(document).ready(function () {
    var LCDKanBan = new GPRO.LCDKanBan();
    LCDKanBan.Init();
})