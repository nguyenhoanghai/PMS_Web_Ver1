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
GPRO.namespace('LCDERROR_N');
GPRO.LCDERROR_N = function () {
    var Global = {
        UrlAction: {
            GetProductivity: '/Productivity/GetProductivityByLineId',
            GetData: '/Productivity/GetLCDInfo',
            GetError: '/Error/GetErrorByLineId'
        },
        Data: {
            TableType: 4,
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
        GetError();
         setInterval(function () { GetError() }, 2000);
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
                $('#tb_error').css('height', data.PageHeight + 'px');
                $('#tb_error #lineName').html("THÔNG TIN CHI TIẾT LỖI " + data.LineName);
                veKhung(data.LayoutPanelConfig);
                DrawBackgroundColor(data.Panel_Background);
                SetFontStyle(data.FontStyle);
                SetBodyTitle(data.BodyTitle);
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
                            style.append('#tb_error .header{ height:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_error .title{ height:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_error .content{ height:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelHeader':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_error #header .logo { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_error #header .title { width:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_error #header .ledalert { width:' + v.SizePercent + '%}');
                            break;
                        case 4:
                            style.append('#tb_error #header .time { width:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelContent':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_error #content .title { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_error #content .value1 { width:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_error #content .value2 { width:' + v.SizePercent + '%}');
                            break;
                        case 4:
                            style.append('#tb_error #content .value3 { width:' + v.SizePercent + '%}');
                            break;
                    }
                    break;
                case 'tblpanelTitle1':
                    switch (parseInt(v.ColumnInt)) {
                        case 1:
                            style.append('#tb_error #lable1  { width:' + v.SizePercent + '%}');
                            break;
                        case 2:
                            style.append('#tb_error #lable2  { width:' + v.SizePercent + '%}');
                            break;
                        case 3:
                            style.append('#tb_error #lable3  { width:' + v.SizePercent + '%}');
                            break;
                        case 4:
                            style.append('#tb_error #lable4  { width:' + v.SizePercent + '%}');
                            break;
                    }
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
                    style.append('#tb_error #header {background-color :' + item.BackColor + '}');
                    break;
                case 'panelContent':
                    style.append('#tb_error #content .error{background-color :' + item.BackColor + '}');
                    break;
                case 'panelContent2':
                    style.append('#tb_error #content .grouperror{background-color :' + item.BackColor + '}');
                    break;
                case 'panelTitle1':
                    style.append('#tb_error #title{background-color :' + item.BackColor + '}');
                    break;
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
                        case 1:
                            style.append('#tb_error #header .title {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 2:
                            style.append('#tb_error #header .time {font-size :' + v.Size + 'px ;line-height :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
                case 'tblPanelContent':
                    switch (parseInt(v.Position)) {
                        case 1:
                            style.append('#tb_error #content .title {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                        case 2:
                            style.append('#tb_error #content .value {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
                case 'tblPanelTitle1':
                    switch (parseInt(v.Position)) {
                        case 1:
                            style.append('#tb_error #title .lable {font-size :' + v.Size + 'px ;font-weight:' + (v.Bold ? 'bold' : 'normal') + '; color:' + v.Color + '}');
                            break;
                    }
                    break;
            }
        });
        $('head').append(style);
    }

    function SetBodyTitle(data) {
        if (data != null) {
            var str = '<tr class="lable">';
            $.each(data, function (i, v) {
                str += '<td columnint="' + (i + 1) + '" id="lable' + (i + 1) + '" class="title">' + v.LabelName + '</td> ';
            });
            str += '</tr>';
            $('#tb_error #title tbody').append(str);
        }
    }

    function GetError() {
        $.ajax({
            url: Global.UrlAction.GetError,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.LineId }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null && data.Data.length > 0) {
                    var listGroupError = data.Data;
                    var $content = $('#tb_error #content tbody');
                    $content.html('');
                    $.each(listGroupError, function (i, gError) {
                        var listError = gError.ListError;
                        if (listError != null && listError.length > 0) {
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
    var LCDERROR_N = new GPRO.LCDERROR_N();
    LCDERROR_N.Init();
})