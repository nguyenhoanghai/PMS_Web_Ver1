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
GPRO.namespace('LCDHoanTat');
GPRO.LCDHoanTat = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',
            GetProductivity: '/api/apipms'///HoanTatLCD',
          
        },
        Data: {
            TableType: 2,
            ListLineId: [],
            isDrawContent: false,
            paging: 9,
            TimeToChangeRow: 5000,
            rowHeight: 72,
            oldRow: 0,
            lineIdStr: '',
  tableId:''
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        Global.Data.lineIdStr = getCookie('strLineIds');
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.ListLineId = listLineId;


        $('#' + Global.Data.tableId + '').css('height', '600px');

        GetLCDConfigInfo(function (data) {
            if (data != null) {
                $.each(data, function (i, obj) {
                    switch (obj.Name) {
                        case "WebPageHeight":
                            var value = obj.Value;
                            $('#'+Global.Data.tableId+'').css('height', value + 'px');
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
        var build = setInterval(function () {
            BuildTableLayOut();
            clearInterval(build);
              var productivity = setInterval(function () { GetProductivity() }, 2000);
        }, 2000);
    }

    var RegisterEvent = function () {
        GetProductivity();
        //  var productivity = setInterval(function () { GetProductivity() }, 1000);
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
                                    $('#'+Global.Data.tableId+' .header').css('height', v.SizePercent + "%");
                                    break;
                                case 2:
                                    $('#'+Global.Data.tableId+' .content').css('height', v.SizePercent + "%");
                                    break;

                            }
                            break;
                        case 'tblpanelHeader':
                            switch (parseInt(v.ColumnInt)) {
                                case 1:
                                    //$('#' + Global.Data.tableId + ' #header .label1').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#"+Global.Data.tableId+" #KanBan_Ticker .value1 {width:" + (v.SizePercent - 0.2) + "%;}";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 2:
                                    //$('#' + Global.Data.tableId + ' #header .label2').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#"+Global.Data.tableId+" #KanBan_Ticker .value2 {width:" + (v.SizePercent - 0.2) + "%;}";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 3:
                                    //$('#' + Global.Data.tableId + ' #header .label3').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#"+Global.Data.tableId+" #KanBan_Ticker .value3 {width:" + (v.SizePercent - 0.2) + "%;}";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 4:
                                    //$('#' + Global.Data.tableId + ' #header .label4').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#" + Global.Data.tableId + " #KanBan_Ticker .value4 {width:" + (v.SizePercent - 0.2) + "%;}";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 5:
                                    //$('#' + Global.Data.tableId + ' #header .label5').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#" + Global.Data.tableId + " #KanBan_Ticker .value5 {width:" + (v.SizePercent - 0.2) + "%;}";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                                case 6:
                                    //$('#' + Global.Data.tableId + ' #header .label6').css('width', v.SizePercent + "%");
                                    var sheet = document.createElement('style')
                                    var css = "#" + Global.Data.tableId + " #KanBan_Ticker .value6 {width:" + (v.SizePercent) + "%;}";
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
                            $('#'+Global.Data.tableId+' #header').css('background-color', v.BackColor);
                            break;
                        case 'panelContent':
                            $('#'+Global.Data.tableId+' #KanBan_Ticker').css('background-color', v.BackColor);
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
                                    var sheet = document.createElement('style')
                                    var css = "#" + Global.Data.tableId + " #header .title {background-color:transparent ;";
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
                            }
                            break;
                        case 'tblPanelContent':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    var sheet = document.createElement('style')
                                    var css = "#"+Global.Data.tableId+" #KanBan_Ticker .title {background-color:" + v.BackColor + ";";
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
                                    var css = "#"+Global.Data.tableId+" #KanBan_Ticker .value {background-color:" + v.BackColor + ";";
                                    css += "font-size:" + v.Size + "px;";
                                    // css += "line-height:" + v.Size + "px !important;";
                                    if (v.Bold)
                                        css += "font-weight:bold;";
                                    else
                                        css += "font-weight:;";
                                    css += "color:" + v.Color + ";";
                                    css += "}";
                                    //  css += "#'+Global.Data.tableId+' #KanBan_Ticker .title {line-height:" + v.Size + "px !important;}"
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);
                                    break;
                            }
                            break;
                    }
                });
                // BuildLabelForPanelContent();
            }
        })
    }

    function BuildLabelForPanelContent() {
        GetTableLayoutPanel(4, function (data) {
            var listTr = $('#' + Global.Data.tableId + ' #header td');
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
            url: Global.UrlAction.GetProductivity + "?lineId=" + Global.Data.lineIdStr,
            type: 'POST', 
            dataType: 'json', 
            success: function (data) {
                $('#' + Global.Data.tableId + ' #KanBan_Ticker tbody').html('<tr id="notfound"><td colspan="6">Không tìm thấy dữ liệu phân công cho chuyền.</td></tr>');

                if (data != null && data.length > 0) {
                    if (Global.Data.oldRow != data.length) {
                        DrawContentAndSetData(data);
                        Global.Data.oldRow = data.length;
                    }
                    else
                        SetDataWithoutDrawContent(data);
                }
            }
        });
    }



    function DrawContentAndSetData(datas) {
        $('#kb_stop').click();
        var body = $('#'+Global.Data.tableId+' #KanBan_Ticker');
        body.empty();
        var head = $('#'+Global.Data.tableId+' #header tbody');
        head.empty();
        var height = 100 / parseInt(Global.Data.paging);

        $.each(datas, function (i, obj) {
            if (i == 0) {
                head.append()
                var tr = $('<tr class="title"></tr>');
                tr.append('<td rowint="1" class="label1">CH</td>');
                tr.append('<td rowint="2" class="label2">MÃ HÀNG</td>');
                tr.append('<td rowint="3" class="label3">SL Đơn hàng</td>');
                tr.append('<td rowint="4" class="label4">LK MAY RA</td>');
                tr.append('<td rowint="5" class="label5">LK KIỂM ĐẠT</td>');
                $.each(obj.Phases, function (ii, phase) {
                    tr.append('<td rowint="' + (ii + 6) + '" class="label' + (ii + 6) + '">' + phase.PhaseName + '</td>');
                });
                head.append(tr);
            }

            var $tr = $('<li style="height:' + Global.Data.rowHeight + 'px"></li>');
            var $ul = $('<ul class="r_' + i + '"></ul>');
            $ul.append('<li class="title value1 "><span>' + obj.LineName + '</span></li>');
            $ul.append('<li class="title value2 "><span>' + (obj.ProName.length > 8 ? (obj.ProName.substring(0, 8) + '...') : obj.ProName) + '</span></li>');
            $ul.append('<li class="value value3 font-dt"><span>' + obj.SLKH + '</span></li>');
            $ul.append('<li class="value value4 font-dt"><span>' + obj.LK_TC + ' </span></li>');
            $ul.append('<li class="value value5 font-dt"><span>' + obj.LK_KCS + ' </span></li>');
            $.each(obj.Phases, function (ii, phase) { 
                $ul.append('<li class="value value' + (ii + 6) + '  font-dt"><span>' + phase.LK  + ' </span></li>');
            });
            $tr.append($ul);
            $tr.append('<div style="clear:left"></div>');
           body.append($tr); 
        });

        $('#KanBan_Ticker ul li').css('height', $('#KanBan_Ticker li').css('height'));
        $('#KanBan_Ticker ul li').css('line-height', $('#KanBan_Ticker ul li').css('height'));
        Global.Data.isDrawContent = true;
        SetAutoScroll(datas.length, Global.Data.paging, Global.Data.TimeToChangeRow, 2)

    }

    function SetDataWithoutDrawContent(datas) {
        $.each(datas, function (i, obj) {
            $('#' + Global.Data.tableId + ' .r_' + i + ' .value1').html('<span>' + obj.LineName + '</span>');
            $('#' + Global.Data.tableId + ' .r_' + i + ' .value2').html('<span>' + (obj.ProName.length > 8 ? (obj.ProName.substring(0, 8) + '...') : obj.ProName) + '</span>');
            $('#' + Global.Data.tableId + ' .r_' + i + ' .value3').html('<span>' + obj.SLKH + '</span>');
            $('#' + Global.Data.tableId + ' .r_' + i + ' .value4').html('<span>' + obj.LK_TC + '</span>');
            $('#' + Global.Data.tableId + ' .r_' + i + ' .value5').html('<span>' + obj.LK_KCS + '</span>');
            $.each(obj.Phases, function (ii, phase) {
                $('#' + Global.Data.tableId + ' .r_' + i + ' .value' + (ii + 6)).html('<span>' + phase.LK + '</span>');
            });
        });
    }
}


