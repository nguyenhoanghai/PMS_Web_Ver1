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
GPRO.namespace('LCDError');
GPRO.LCDError = function () {
    var Global = {
        UrlAction: {
            GetLCDConfig: '/api/lcdconfig',           
            GetError: '/Error/GetErrorByLineId'
        },
        Data: {
            TableType: 4,
            LineId:0
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
                            $('#tableError').css('height', value + 'px');
                            break;
                    }
                });
            }
        });
         BuildTableLayOut();      
      }

    var RegisterEvent = function () {
        var productivity = setInterval(function () { GetError() }, 1000);
        $(window).resize(function () {
            $('#tableError').css("height", window.innerHeight);
        });
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
                                    $('#tableError .header').css('height', v.SizePercent + "%");
                                    break;
                                case 2:
                                    $('#tableError .title').css('height', v.SizePercent + "%");
                                    break;
                                case 3:
                                    $('#tableError .content').css('height', v.SizePercent + "%");
                                    break;
                            }
                            break;
                        case 'tblpanelHeader':
                            switch (parseInt(v.ColumnInt)) {
                                case 1: $('#tableError #header .logo').css('width', v.SizePercent + "%"); break;
                                case 2: $('#tableError #header .title').css('width', v.SizePercent + "%"); break;
                                case 3: $('#tableError #header .time').css('width', v.SizePercent + "%"); break;
                            }
                            break;
                        case 'tblpanelContent':
                            switch (parseInt(v.ColumnInt)) {
                                case 1:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tableError #content .title {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);                                    
                                    break;
                                case 2:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tableError #content .value1 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    $('#tableError #value1').css('width', v.SizePercent + "%");
                                    break;
                                case 3:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tableError #content .value2 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);
                                    $('#tableError #value1').css('width', v.SizePercent + "%");
                                    break;
                                case 4:
                                    var sheet = document.createElement('style')
                                    sheet.innerHTML = "#tableError #content .value3 {width:" + v.SizePercent + "%}";
                                    document.body.appendChild(sheet);                                    
                                    break;
                            }
                            break;
                        case 'tblpanelTitle1':
                            switch (parseInt(v.ColumnInt)) {
                                case 1:
                                    $('#tableError #lable1').css('width', v.SizePercent + "%");
                                    break;
                                case 2:
                                    $('#tableError #lable2').css('width', v.SizePercent + "%");
                                    break;
                                case 3:
                                    $('#tableError#lable3').css('width', v.SizePercent + "%");
                                    break;
                                case 4:
                                    $('#tableError #lable4').css('width', v.SizePercent + "%");
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
                            $('#tableError #header').css('background-color', v.BackColor);
                            break;
                        case 'panelContent':
                            var sheet = document.createElement('style')
                            sheet.innerHTML = "#tableError #content .error {background-color:" + v.BackColor + "}";
                            document.body.appendChild(sheet);                           
                            break;
                        case 'panelContent2':
                            var sheet = document.createElement('style')
                            sheet.innerHTML = "#tableError #content .grouperror {background-color:" + v.BackColor + "}";
                            document.body.appendChild(sheet);
                            break;
                        case 'panelTitle1':
                            $('#tableError #title').css('background-color', v.BackColor);
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
                                    $('#tableError #header .title').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableError #header .title').css('font-weight', "bold");
                                    else
                                        $('#tableError #header .title').css('font-weight', "");
                                    $('#tableError #header .title').css('color', v.Color);
                                    break;
                                case 2:
                                    $('#tableError #header .time').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableError #header .time').css('font-weight', "bold");
                                    else
                                        $('#tableError #header .time').css('font-weight', "");
                                    $('#tableError #header .time').css('color', v.Color);
                                    break;
                            }
                            break;
                        case 'tblPanelContent':
                            switch (parseInt(v.Position)) {
                                case 1:
                                    var sheet = document.createElement('style')
                                    var css = "#tableError #content .title {background-color:" + v.BackColor + ";";
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
                                    var css = "#tableError #content .value {background-color:" + v.BackColor + ";";
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
                                    $('#tableError #title .lable').css('font-size', v.Size + "px");
                                    if (v.Bold)
                                        $('#tableError #title .lable').css('font-weight', "bold");
                                    else
                                        $('#tableError #title .lable').css('font-weight', "");
                                    $('#tableError #title .lable').css('color', v.Color);
                                    ; break;                                
                            }
                            break;

                    }
                });
                BuildLabelForPanelTitle1();
            }
        })
    }

    function BuildLabelForPanelTitle1() {
        GetTableLayoutPanel(4, function (data) {
            var listTd = $('#tableError #title td');
            if (data != null && listTd != null) {
                $.each(data, function (i, v) {
                    $.each(listTd, function (j, va) {
                        if (parseInt($(va).attr("columnint")) == v.IntRowTBLPanelContent) {                            
                            $(va).html(v.LabelName);                            
                        }
                    });
                });
            }
        })
    }

    

    function GetError()
    {
        $.ajax({
            url: Global.UrlAction.GetError,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.LineId}),
            contentType: 'application/json charset=utf-8', 
            success: function (data) {
                if (data.Data != null && data.Data.length>0) {
                    $('#tableError #lineName').html('THÔNG TIN CHI TIẾT LỖI ' + data.Data[0].LineName);
                    var listGroupError = data.Data;
                    $.each(listGroupError, function (i, gError) {
                        var listError = gError.ListError;
                        if (listError != null && listError.length > 0)
                        {
                            var $content = $('#tableError #content tbody');
                            $content.html('');
                            $.each(listError, function (j, err) {
                                var $tr = $('<tr class="error"></tr>');
                                $tr.append('<td class="title">' + err.ErrorName + '</td>');
                                $tr.append('<td class="value">' + err.CountErrorLastHours + '</td>')
                                $tr.append('<td class="value">' + err.CountErrorCurrentHours + '</td>')
                                $tr.append('<td class="value">' + err.TotalError + '</td>')
                                $content.append($tr);
                            });
                            var $tr = $('<tr class="grouperror"></tr>');
                            $tr.append('<td class="title">' + gError.GroupErrorName + '</td>');
                            $tr.append('<td class="value">' + gError.CountErrorLastHours + '</td>')
                            $tr.append('<td class="value">' + gError.CountErrorCurrentHours + '</td>')
                            $tr.append('<td class="value">' + gError.TotalError + '</td>')
                            $content.append($tr);
                        }
                    });
                }
            }
        });
    }

   
}


$(document).ready(function () {
    var LCDError = new GPRO.LCDError();
    LCDError.Init();
})