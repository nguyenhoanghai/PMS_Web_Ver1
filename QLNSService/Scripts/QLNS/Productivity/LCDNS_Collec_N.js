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
GPRO.namespace('LCDNS_Coll_N');
GPRO.LCDNS_Coll_N = function () {
    var Global = {
        UrlAction: {
            GetProductivity: '/Productivity/GetTotalProductivity',
            GetData: '/Productivity/GetLCDInfo'
        },
        Data: {
            TableType: 3,
            LineId: [],
            isDrawContent: false,
            paging: 0,
            TimeToChangeRow: 0,
            check: false, 
            IsKanBanAcc: false
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        GetTime();
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.LineId = listLineId;

        var LoginUser = getCookie('LoginUser');
        Global.Data.IsKanBanAcc = JSON.parse(LoginUser).IsKanbanAccount;

        BuildTableLayOut();
        RegisterEvent();
    }

    var RegisterEvent = function () {
        GetProductivity_Collec();
   //   setInterval(function () { GetProductivity_Collec() }, 2000);
    }

    function GetTableData(configTypeId, callback) {
        $.ajax({
            url: Global.UrlAction.GetData,
            type: 'POST',
            data: JSON.stringify({ 'Id': 1, 'tableId': Global.Data.TableType, 'configTypeId': configTypeId }),
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
                $('#tb_pro_collec').css('height', data.PageHeight + 'px');
                Global.Data.paging = data.paging;
                Global.Data.TimeToChangeRow = data.TimeToChangeRow;
                veKhung(data.LayoutPanelConfig);
                DrawBackgroundColor(data.Panel_Background);
                SetFontStyle(data.FontStyle);
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
                            style.append('#tb_pro_collec .header{ height:' + v.SizePercent + '%  }');
                            break;
                        case 2:
                            style.append('#tb_pro_collec .title1{ height:' + v.SizePercent + '%  }');
                             break;
                        case 3:
                            style.append('#tb_pro_collec .title2{ height:' + v.SizePercent + '%  }');
                            break;
                        case 4:
                            style.append('#tb_pro_collec .content{ height:' + v.SizePercent + '%  }');
                            break;
                        case 5:
                            style.append('#tb_pro_collec .footer{ height:' + v.SizePercent + '%  }');
                            break;
                    }
                    break;
                case 'tblpanelHeader':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_pro_collec #header .logo { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_pro_collec #header .title { width:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_pro_collec #header .time { width:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelTitle1':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_pro_collec #title1 .label1 { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_pro_collec #title1 .label2 { width:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_pro_collec #title1 .label3 { width:' + v.SizePercent + '%}');
                            break;
                        case 4:
                            style.append('#tb_pro_collec #title1 .label4 { width:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelTitle2':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_pro_collec #title2 .label1 { width:' + v.SizePercent + '%}');
                            style.append('#tb_pro_collec #LCDTH_ticker .value1 { width:' + (v.SizePercent - 0.2) + '%}');
                            break;
                        case 2:
                            style.append('#tb_pro_collec #title2 .label2 { width:' + v.SizePercent + '%}');
                            style.append('#tb_pro_collec #LCDTH_ticker .value2 { width:' + (v.SizePercent - 0.2) + '%}');
                            break;
                        case 3:
                            style.append('#tb_pro_collec #title2 .label3 { width:' + v.SizePercent + '%}');
                            style.append('#tb_pro_collec #LCDTH_ticker .value3 { width:' + (v.SizePercent - 0.2) + '%}');
                            break;
                        case 4:
                            style.append('#tb_pro_collec #title2 .label4 { width:' + v.SizePercent + '%}');
                            style.append('#tb_pro_collec #LCDTH_ticker .value4 { width:' + (v.SizePercent - 0.2) + '%}');
                            break;
                        case 5:
                            style.append('#tb_pro_collec #title2 .label5 { width:' + v.SizePercent + '%}');
                            style.append('#tb_pro_collec #LCDTH_ticker .value5 { width:' + (v.SizePercent - 1) + '%}');
                            break;
                        case 6:
                            style.append('#tb_pro_collec #title2 .label6 { width:' + v.SizePercent + '%}');
                            style.append('#tb_pro_collec #LCDTH_ticker .value6 { width:' + (v.SizePercent - 1) + '%}');
                             break;
                    }
                    break;
                    //case 'tblpanelContent':
                    //    switch (parseInt(v.ColumnInt)) {
                    //        case 1: style.append('#tb_pro_collec #LCDTH_ticker .value1 { width:' + v.SizePercent + '%}'); break;
                    //        case 2: style.append('#tb_pro_collec #LCDTH_ticker .value2 { width:' + v.SizePercent + '%}'); break;
                    //        case 3: style.append('#tb_pro_collec #LCDTH_ticker .value3 { width:' + v.SizePercent + '%}'); break;
                    //        case 4: style.append('#tb_pro_collec #LCDTH_ticker .value4 { width:' + v.SizePercent + '%}'); break;
                    //        case 5: style.append('#tb_pro_collec #LCDTH_ticker .value5 { width:' + v.SizePercent + '%}'); break;
                    //        case 6: style.append('#tb_pro_collec #LCDTH_ticker .value6 { width:' + v.SizePercent + '%}'); break;
                    //        case 7: style.append('#tb_pro_collec #LCDTH_ticker .value7 { width:' + v.SizePercent + '%}'); break;
                    //        case 8: style.append('#tb_pro_collec #LCDTH_ticker .value8 { width:' + v.SizePercent + '%}'); break;
                    //    }
                    //    break;
            }
        });

        $('head').append(style);
        if (!Global.Data.check)
            $('#tb_pro_collec tr.title1').remove();
    }

    function DrawBackgroundColor(data) {
        var style = $('<style></style>');
        $.each(data, function (i, item) {
            switch (item.Name.trim()) {
                case 'panelHeader': style.append('#tb_pro_collec #header {background-color :' + item.BackColor + '}'); break;
                case 'panelContent': style.append('#tb_pro_collec #LCDTH_ticker{background-color :' + item.BackColor + '}'); break;
                case 'panelTitle1': style.append('#tb_pro_collec #title1{background-color :' + item.BackColor + '}'); break;
                case 'panelTitle2': style.append('#tb_pro_collec #title2{background-color :' + item.BackColor + '}'); break;
                case 'panelFooter': style.append('#tb_pro_collec .footer{background-color :' + item.BackColor + '}'); break;
            }
        });
        $('head').append(style);
    }

    function SetFontStyle(data) {
        var style = $('<style></style>');
        $.each(data, function (i, v) {
            switch (v.TableLayoutPanelName.trim()) {
                case 'tblPanelHeader':
                    switch (parseInt(v.Position)) {
                        case 1: style.append('#tb_pro_collec #header .title {font-size :' + v.Size + 'px ;line-height :' + (v.Size + 3) + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                        case 2: style.append('#tb_pro_collec #header .time {font-size :' + v.Size + 'px ;line-height :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                    }
                    break;
                case 'tblPanelContent':
                    switch (parseInt(v.Position)) {
                        case 1: style.append('#tb_pro_collec #LCDTH_ticker .title {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                        case 2: style.append('#tb_pro_collec #LCDTH_ticker .value {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                    }
                    break;
                case 'tblPanelTitle1':
                    switch (parseInt(v.Position)) {
                        case 1: style.append('#tb_pro_collec #title1 .lable {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                    }
                    break;
                case 'tblPanelTitle2':
                    switch (parseInt(v.Position)) {
                        case 1: style.append('#tb_pro_collec #title2 .lable {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                    }
                    break;
                case 'tblPanelFooter':
                    switch (parseInt(v.Position)) {
                        case 1: style.append('#tb_pro_collec .footer .title {font-size :' + v.Size + 'px ;line-height :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                        case 2: style.append('#tb_pro_collec .footer .value {font-size :' + v.Size + 'px ;line-height :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}'); break;
                    }
                    break;
            }
        });
        $('head').append(style);
    }

    function GetProductivity_Collec() {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'listLineId': Global.Data.LineId, 'tableTypeId': Global.Data.TableType, 'IsKanbanAcc': Global.Data.IsKanBanAcc }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    $('#tb_pro_collec #title1 .label1').html(data.Data.Morth);
                    $('#tb_pro_collec #monthStr').html(data.Data.Morth);
                    if (!Global.Data.IsKanBanAcc) {
                        $('#tb_pro_collec #title1 .label2').html("SẢN LƯỢNG (TH/KH)<br/> " + accounting.formatNumber(data.Data.THThang) + " / " + accounting.formatNumber(data.Data.KHThang));
                        $('#tb_pro_collec #title1 .label3').html("DOANH THU (TH/KH)<br/> " + accounting.formatNumber(data.Data.DoanhThuTHThang) + " / " + accounting.formatNumber(data.Data.DoanhThuKHThang));
                        $('#tb_pro_collec #title1 .label4').html("NHỊP SẢN XUẤT (TH/KH)<br/> " + data.Data.NhipThucTeKH + "/" + data.Data.NhipSanXuatTH);
                        $('#tb_pro_collec #title2 .label6').remove();
                    }
                    else
                        $('#tb_pro_collec #title2 .label4').remove();

                    if (data.Data.ListLineMorthProductivity != null && data.Data.ListLineMorthProductivity.length > 0) {
                        if (Global.Data.isDrawContent)
                            SetDataWithoutDrawContent(data.Data.ListLineMorthProductivity);
                        else
                            DrawContentAndSetData(data.Data.ListLineMorthProductivity);
                    }

                    var tr1 = '', tr2 = '';
                    $.each(data.Data.ListHoursProductivity, function (i, item) {
                        tr1 += '<td class="title">' + item.IntHours + 'H</td>';
                        tr2 += '<td class="value font-dt">' + item.KCS+'<span style="color:blue">|</span>' +item.NormsHour+ '</td>';
                    })
                    $('#tb_pro_collec .footer table thead tr').html(tr1);
                    $('#tb_pro_collec .footer table tbody tr').html(tr2);
                }
            }
        });
    }

    function DrawContentAndSetData(datas) {
        $('#tb_pro_collec #LCDTH_ticker').html("");
        var height = 100 / parseInt(Global.Data.paging);
        $.each(datas, function (i, obj) {
            var $tr = $('<li style="height:' + height + '%"></li>');
            var $ul = $('<ul id="r_' + i + '"></ul>');
            $ul.append('<li class="title value1" >' + obj.LineName + '</li>');
            if (Global.Data.IsKanBanAcc) {
                $ul.append('<li class="value value6">' + (obj.CommoName == null ? "" : obj.CommoName) + '</li>');
            }
            $ul.append('<li class="value value2 font-dt"  >' + accounting.formatNumber(obj.THNgay) + '|' + accounting.formatNumber(obj.KHNgay) + '</li>');
            $ul.append('<li class="value value3 font-dt"  >' + accounting.formatNumber(obj.THThang) + '|' + accounting.formatNumber(obj.KHThang) + '</li>');
            if (!Global.Data.IsKanBanAcc)
                $ul.append('<li class="value value4 font-dt">' + accounting.formatNumber(obj.DoanhThuTH) + '|' + accounting.formatNumber(obj.DoanhThuKH) + '</li>');
            $ul.append('<li class="value value5 font-dt" >' + obj.TiLeTH + '</li>');
            $tr.append($ul);
            $tr.append('<div style="clear:left"></div>');
            $('#tb_pro_collec #LCDTH_ticker').append($tr);
        });

        $('#tb_pro_collec #LCDTH_ticker ul li').css('line-height', $('#tb_pro_collec #LCDTH_ticker ul li').css('height'));

        Global.Data.isDrawContent = true;
        SetAutoScroll(datas.length, Global.Data.paging, Global.Data.TimeToChangeRow, 1);
    }

    function SetDataWithoutDrawContent(datas) {
        $.each(datas, function (i, obj) {
            $('#tb_pro_collec #r_' + i + ' .value2').html(accounting.formatNumber(obj.THNgay) + '<span style="color:blue">|</span>' + accounting.formatNumber(obj.KHNgay));
            $('#tb_pro_collec #r_' + i + ' .value3').html(accounting.formatNumber(obj.THThang) + '<span style="color:blue">|</span>' + accounting.formatNumber(obj.KHThang));
            if (!Global.Data.IsKanBan)
                $('#tb_pro_collec #r_' + i + ' .value4').html(accounting.formatNumber(obj.DoanhThuTH) + '<span style="color:blue">|</span>' + accounting.formatNumber(obj.DoanhThuKH));
            else
                $('#tb_pro_collec #r_' + i + ' .value6').html((obj.CommoName == null ? "" : obj.CommoName));
            $('#tb_pro_collec #r_' + i + ' .value5').html(obj.TiLeTH);
        });
    }
}


$(document).ready(function () {
    var LCDNS_Coll_N = new GPRO.LCDNS_Coll_N();
    LCDNS_Coll_N.Init();
});
