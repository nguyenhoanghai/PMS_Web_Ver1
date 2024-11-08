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
GPRO.namespace('LCDGeneral');
GPRO.LCDGeneral = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',           
            GetProductivity: '/Productivity/GetTotalProductivity',
            //GetTotalWorkTime: '/Shift/CountWorkHoursMaxOfLines',
            GetTotalWorkTime: '/Shift/GetWorkHoursInDayOfLine',
        },
        Data: {
            TableType: 3,
            ListLineId: [],
            isDrawContent:false
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0) 
            Global.Data.ListLineId = listLineId;
        GetLCDConfigInfo(function (data) {
            if (data != null) {
                $.each(data, function (i, obj) {
                    switch (obj.Name) {
                        case "WebPageHeight":
                            var value = obj.Value;
                            $('#tb_pro_collec').css('height', value + 'px');
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
                                    $('#tb_pro_collec .header').css('height', v.SizePercent + "%");
                                    break;
                                case 2:
                                    $('#tb_pro_collec .title1').css('height', v.SizePercent + "%");
                                    break;
                                case 3:
                                    $('#tb_pro_collec .title2').css('height', v.SizePercent + "%");
                                    break;
                                case 4:
                                    $('#tb_pro_collec .content').css('height', v.SizePercent + "%");
                                    break;
                                case 5:
                                    $('#tb_pro_collec .footer').css('height', v.SizePercent + "%");
                                    break;
                            }
                            break;
                        case 'tblpanelHeader':
                            switch (parseInt(v.ColumnInt)) {
                                case 1: $('#tb_pro_collec #header .logo').css('width', v.SizePercent + "%"); break;
                                case 2: $('#tb_pro_collec #header .title').css('width', v.SizePercent + "%"); break;
                                case 3: $('#tb_pro_collec #header .time').css('width', v.SizePercent + "%"); break;
                            }
                            break;
                        case 'tblpanelTitle1':
                            switch (parseInt(v.ColumnInt)) {
                                case 1: $('#tb_pro_collec #title1 .label1').css('width', v.SizePercent + "%"); break;
                                case 2: $('#tb_pro_collec #title1 .label2').css('width', v.SizePercent + "%"); break;
                                case 3: $('#tb_pro_collec #title1 .label3').css('width', v.SizePercent + "%"); break;
                                case 4: $('#tb_pro_collec #title1 .label4').css('width', v.SizePercent + "%"); break;
                            }
                            break;
                        case 'tblpanelTitle2':
                            switch (parseInt(v.ColumnInt)) {
                                case 1: $('#tb_pro_collec #title2 .label1').css('width', v.SizePercent + "%"); break;
                                case 2: $('#tb_pro_collec #title2 .label2').css('width', v.SizePercent + "%"); break;
                                case 3: $('#tb_pro_collec #title2 .label3').css('width', v.SizePercent + "%"); break;
                                case 4: $('#tb_pro_collec #title2 .label4').css('width', v.SizePercent + "%"); break;
                                case 4: $('#tb_pro_collec #title2 .label5').css('width', v.SizePercent + "%"); break;
                            }
                            break;
                        case 'tblpanelContent':
                            switch (parseInt(v.ColumnInt)) {
                                case 1:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value1 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);                                    
                                    break;
                                case 2:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value2 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    $('#tb_pro_collec #value1').css('width', v.SizePercent + "%");
                                    break;
                                case 3:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value3 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    $('#tb_pro_collec #value1').css('width', v.SizePercent + "%");
                                    break;
                                case 4:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value4 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);                                    
                                    break;
                                case 5:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value5 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    break;
                                case 6:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value6 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    break;
                                case 7:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value7 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    break;
                                case 8:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tb_pro_collec #content .value8 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    break;
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
                            $('#tb_pro_collec #header').css('background-color', v.BackColor);
                            break;
                        case 'panelContent':
                            $('#tb_pro_collec #content').css('background-color', v.BackColor);
                            break;                       
                        case 'panelTitle1':
                            $('#tb_pro_collec #title1').css('background-color', v.BackColor);
                            break;
                        case 'panelTitle2':
                            $('#tb_pro_collec #title2').css('background-color', v.BackColor);
                            break;
                        case 'panelFooter':
                            $('#tb_pro_collec #footer').css('background-color', v.BackColor);
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
                                    $('#tb_pro_collec #header .title').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tb_pro_collec #header .title').css('font-weight', "bold");
                                    else
                                        $('#tb_pro_collec #header .title').css('font-weight', "");
                                    $('#tb_pro_collec #header .title').css('color', v.Color);
                                    break;
                                case 2:
                                    $('#tb_pro_collec #header .time').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tb_pro_collec #header .time').css('font-weight', "bold");
                                    else
                                        $('#tb_pro_collec #header .time').css('font-weight', "");
                                    $('#tb_pro_collec #header .time').css('color', v.Color);
                                    break;
                            }
                            break;
                        case 'tblPanelContent':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    var sheet = document.createElement('style')
                                    var css = "#tb_pro_collec #content .title {background-color:" + v.BackColor + ";";
                                    css += "font-size:" + v.Size + "px;";
                                    if (v.Bold)
                                        css += "font-weight:bold;";
                                    else
                                        css += "font-weight:;";
                                    css += "color:" + v.Color+";";
                                    css += "}";
                                    sheet.innerHTML = css;
                                    document.body.appendChild(sheet);                                    
                                    break;
                                case 2:
                                    var sheet = document.createElement('style')
                                    var css = "#tb_pro_collec #content .value {background-color:" + v.BackColor + ";";
                                    css += "font-size:" + v.Size + "px;";
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
                        case 'tblPanelTitle1':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    $('#tb_pro_collec #title1 .lable').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tb_pro_collec #title1 .lable').css('font-weight', "bold");
                                    else
                                        $('#tb_pro_collec #title1 .lable').css('font-weight', "");
                                    $('#tb_pro_collec #title1 .lable').css('color', v.Color);
                                    break;                                
                            }
                            break;
                        case 'tblPanelTitle2':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    $('#tb_pro_collec #title2 .lable').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tb_pro_collec #title2 .lable').css('font-weight', "bold");
                                    else
                                        $('#tb_pro_collec #title2 .lable').css('font-weight', "");
                                    $('#tb_pro_collec #title2 .lable').css('color', v.Color);
                                    break;
                            }
                            break;
                        case 'tblPanelFooter':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    $('#tb_pro_collec #footer .title').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tb_pro_collec #footer .title').css('font-weight', "bold");
                                    else
                                        $('#tb_pro_collec #footer .title').css('font-weight', "");
                                    $('#tb_pro_collec #footer .title').css('color', v.Color);
                                    ; break;
                                case 2:
                                    $('#tb_pro_collec #footer .value').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tb_pro_collec #footer .value').css('font-weight', "bold");
                                    else
                                        $('#tb_pro_collec #footer .value').css('font-weight', "");
                                    $('#tb_pro_collec #footer .value').css('color', v.Color);
                                    ; break;
                            }
                            break;

                    }
                });
                BuildFooterContent();
            }
        })
    }

    function BuildFooterContent() {
        $.ajax({
            url: Global.UrlAction.GetTotalWorkTime,
            type: 'POST',
            data: JSON.stringify({ 'listLineId': Global.Data.ListLineId }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (i, item) {
                        $('#tb_pro_collec #footer .titleFooter').append('<td class="title">' + item.TimeEnd.Hours + ':' + item.TimeEnd.Minutes + 'H</td>');
                        $('#tb_pro_collec #footer .valueFooter').append('<td class="value" hours="' + (i + 1) + '">0</td>');
                    });
                }
            }
        });
    }
  
    function GetProductivity()
    {
        $.ajax({
            url: Global.UrlAction.GetProductivity,
            type: 'POST',
            data: JSON.stringify({ 'listLineId': Global.Data.ListLineId, 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8', 
            success: function (data) {
                if (data.Data != null)
                {
                    $('#tb_pro_collec #title1 .label1').html(data.Data.Morth);
                    $('#tb_pro_collec #title1 .label2').html("SẢN LƯỢNG (TH/KH): " + accounting.formatNumber(data.Data.THThang) + " / " + accounting.formatNumber(data.Data.KHThang));
                    $('#tb_pro_collec #title1 .label3').html("DOANH THU (TH/KH): " + accounting.formatNumber(data.Data.DoanhThuTHThang) + " / " + accounting.formatNumber(data.Data.DoanhThuKHThang));
                    $('#tb_pro_collec #title1 .label4').html("NHỊP SẢN XUẤT (TH/KH): " + data.Data.NhipThucTeKH + "/" + data.Data.NhipSanXuatTH);
                    if (data.Data.ListLineMorthProductivity != null && data.Data.ListLineMorthProductivity.length > 0)
                    {                        
                        if (Global.Data.isDrawContent)
                            SetDataWithoutDrawContent(data.Data.ListLineMorthProductivity);
                        else
                            DrawContentAndSetData(data.Data.ListLineMorthProductivity);                       
                    }
                    if (data.Data.ListHoursProductivity != null && data.Data.ListHoursProductivity.length > 0) {
                        var listHTMLHoursProductivity = $('[hours]');
                        $.each(data.Data.ListHoursProductivity, function (i, v) {
                            $.each(listHTMLHoursProductivity, function (j, item) {
                                if (v.IntHours == parseInt($(item).attr("hours")))
                                    $(item).html(data.Data.KieuHienThiNangSuatGio == '1' ? v.HoursProductivity : v.HoursProductivity_1);
                            });
                        });                       
                    }
                }
            }
        });
    }

    function DrawContentAndSetData(datas) {
        $('#tb_pro_collec #content tbody').html("");
        $.each(data.Data.ListLineMorthProductivity, function (i, obj) {
            var $tr = $("<tr></tr>");
            $tr.append('<td class="title value1">' + obj.LineName + '</td>');
            $tr.append('<td class="value value2">' + accounting.formatNumber(obj.THNgay) + '</td>');
            $tr.append('<td class="value value3">' + accounting.formatNumber(obj.KHNgay) + '</td>');
            $tr.append('<td class="value value4">' + accounting.formatNumber(obj.THThang) + '</td>');
            $tr.append('<td class="value value5">' + accounting.formatNumber(obj.KHThang) + '</td>');
            $tr.append('<td class="value value6">' + accounting.formatNumber(obj.DoanhThuTH) + '</td>');
            $tr.append('<td class="value value7">' + accounting.formatNumber(obj.DoanhThuKH) + '</td>');
            $tr.append('<td class="value value8">' + obj.TiLeTH + '</td>');
            $('#tb_pro_collec #content tbody').append($tr);
        });
        Global.Data.isDrawContent = true;
    }

    function SetDataWithoutDrawContent(datas)
    {
        alert('da ve roi');
    }

}


$(document).ready(function () {
    var LCDGeneral = new GPRO.LCDGeneral();
    LCDGeneral.Init();
})