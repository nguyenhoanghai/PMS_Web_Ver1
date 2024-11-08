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
            GetCheckList: '/Productivity/GetLCD_HT_Data',
            GetData: '/Productivity/GetLCDInfo',
            GetCheckListConfig: '/Productivity/GetCheckListConfig'
        },
        Data: {
            rowHeight: 0,
            TableType: 11,
            LineId: 0,
            paging: 0,
            TimeToChangeRow: 0,
            IsDraw: false,

            col: 0,
            row: 0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {


        RegisterEvent();
    }

    var RegisterEvent = function () {
        GetCheckListConfig();
        var productivity = setInterval(function () { GetProductivity() }, 1000);
    }

    function GetCheckListConfig() {
        $.ajax({
            url: Global.UrlAction.GetCheckListConfig,
            type: 'POST',
            data: JSON.stringify({ 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    Global.Data.paging = parseInt(data.Data.paging_Detail);
                    Global.Data.TimeToChangeRow = parseInt(data.Data.Timer_Detail);
                    Global.Data.rowHeight = data.Data.RowHeight;
                    if (data.Data.LCDConfigs.length > 0) {
                        $.each(data.Data.LCDConfigs, function (i, item) {
                            switch (item.RowType) {
                                case 1: // title
                                    $('style').append('#LCD_Complete #cl-title .t_' + item.Column + '{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px; line-height:' + item.FontSize + 'px;  font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ; text-transform: uppercase; }');
                                    break;
                                case 2: // head
                                    if (item.Column == null)
                                        $('style').append('#LCD_Complete #ul-head ul li{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + ' ; background:' + item.Background + '; font-family:' + item.FontName + ' !important;font-size:' + item.FontSize + 'px !important;line-height:' + item.FontSize + 'px !important; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' !important;  }');
                                    else
                                        $('style').append('#LCD_Complete #ul-head .h_' + item.Column + '{height:' + item.Height + '%  !important;width:' + item.Width + '% !important; color:' + item.Color + '  !important; background:' + item.Background + '  !important; font-family:' + item.FontName + '  !important;font-size:' + item.FontSize + 'px  !important; line-height:' + item.FontSize + 'px;  font-weight:' + (item.FontWeight ? 'bold' : 'normal') + '  !important;  }');
                                    break;
                                case 3: // content
                                    if (item.Column == null)
                                        $('style').append('#LCD_Complete #ul-content{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px; line-height:' + item.FontSize + 'px; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ;   }');
                                    else
                                        $('style').append('#LCD_Complete #ul-content .c_' + item.Column + '{height:' + item.Height + '%  !important;width:' + item.Width + '%  !important; color:' + item.Color + '  !important; background:' + item.Background + '  !important; font-family:' + item.FontName + '  !important;font-size:' + item.FontSize + 'px  !important; line-height:' + item.FontSize + 'px !important;  font-weight:' + (item.FontWeight ? 'bold' : 'normal') + '  !important;   }');

                                    break;
                            }
                        });
                    }
                    GetProductivity();
                }
            }
        });
    }

    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetCheckList,
            type: 'POST',
            data: '',// JSON.stringify({ 'Id': Global.Data.LineId, 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    if (!Global.Data.IsDraw || (Global.Data.col != data.Data.Head.length || Global.Data.row != data.Data.Body.length))
                        DrawHeader(data.Data);
                    else
                        BindDataBody(data.Data);
                }
            }
        });
    }

    function DrawHeader(data) {
        var width = (100 / (data.Head.length + 1));
        var str = '<li style="width:' + width + '%" class="h_0">Mã Hàng</li>';
        $.each(data.Head, function (i, item) {
            if (i == data.Head.length - 1)
                str += '<li style="width:' + (width - 1) + '%" class="h_' + (i + 1) + '"> ' + item.Name + '</li>';
            else
                str += '<li style="width:' + width + '%" class="h_' + (i + 1) + '">' + item.Name + '</li>';
        });
        $('#ul-head ul').html(str);
        Global.Data.col = data.Head.length;
        Global.Data.row = data.Body.length;
        DrawBody(data);
    }

    function DrawBody(data) {
        $('#LCD_Complete_ticker').html("");
        var height = Global.Data.rowHeight;
        var width = 100 / (data.Head.length + 1.05);

        if (Global.Data.paging < data.Body.length) {
            $.each(data.Body, function (ii, body) {
                if (body.Items.length > 0) {
                    var $tr = $('<li style="height:' + height + 'px"></li>');
                    var $ul = $('<ul style="float:left" id="r_' + ii + '"></ul>');

                    $.each(data.Head, function (i, head) {
                        if (i == 0)
                            $ul.append('<li style="width:' + width + '%;" class=" value c_0" ><span id="c_' + ii + '_00">' + (body.Items[0].CommoName.length > 8 ? ('...' + body.Items[0].CommoName.substring(body.Items[0].CommoName.length - 5, body.Items[0].CommoName.length)) : body.Items[0].CommoName) + '</span></li>');
                        var de = null;
                        $.each(body.Items, function (iii, detail) {
                            if (detail.CompletionPhaseId == head.Id)
                                de = detail;
                        });

                        if (de == null)
                            $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">0</span></li>');
                        else
                            $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.Quantity + '</span></li>');
                    });
                    $tr.append($ul);
                    $tr.append('<div style="clear:left"></div>');
                    $('#LCD_Complete_ticker').append($tr);
                }
            });

            $('#LCD_Complete_ticker li ul li').css('height', $('#LCD_Complete_ticker li').css('height'));
            SetAutoScroll(data.Body.length, Global.Data.paging, Global.Data.TimeToChangeRow, 6);
        }
        else {
            $('#6_stop').click();
            $.each(data.Body, function (ii, body) {
                if (body.Items.length > 0) {
                    var $tr = $('<li style="height:' + height + 'px"></li>');
                    //  var $ul = $('<table  id="r_' + ii + '"><tr></tr></table>');
                    var $ul = $('<ul style="float:left" id="r_' + ii + '"></ul>');

                    $.each(data.Head, function (i, head) {
                        if (i == 0)
                            //    $ul.append('<td style="width:' + width + '%" class="c_0 value" id="c_' + ii + '_00">' + body.Items[0].CommoName + '</td>');
                            //var de = null;
                            //$.each(body.Items, function (iii, detail) {
                            //    if (detail.CompletionPhaseId == head.Id)
                            //        de = detail;
                            //});

                            //if (de == null)
                            //    $ul.append('<td style="width:' + width + '%" class="c_' + (i + 1) + ' value font_dien_tu" ><span id="c_' + ii + '_' + i + '"></span></td>');
                            //else
                            //    $ul.append('<td style="width:' + width + '%" class="c_' + (i + 1) + ' value font_dien_tu"><span id="c_' + ii + '_' + i + '">' + de.Quantity + '</span></td>');
                            $ul.append('<li style="width:' + width + '%;" class=" value c_0" ><span id="c_' + ii + '_00">' + (body.Items[0].CommoName.length > 8 ? ('...' + body.Items[0].CommoName.substring(body.Items[0].CommoName.length - 5, body.Items[0].CommoName.length)) : body.Items[0].CommoName) + '</span></li>');
                        var de = null;
                        $.each(body.Items, function (iii, detail) {
                            if (detail.CompletionPhaseId == head.Id)
                                de = detail;
                        });

                        if (de == null)
                            $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">0</span></li>');
                        else
                            $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.Quantity + '</span></li>');

                    });
                    $tr.append($ul);
                    $('#LCD_Complete_ticker').append($tr);
                }
            });
            $('#LCD_Complete_ticker li table td').css('height', $('#LCD_Complete_ticker li').css('height'));
        }
        Global.Data.IsDraw = true;
    }

    function BindDataBody(data) {
        $.each(data.Body, function (ii, body) {
            if (body.Items.length > 0) {
                $.each(data.Head, function (i, head) {
                    if (i == 0)
                        $('#c_' + ii + '_00').html('<span>' + (body.Items[0].CommoName.length > 8 ? ('...' + (body.Items[0].CommoName.substring(body.Items[0].CommoName.length - 5, body.Items[0].CommoName.length))) : body.Items[0].CommoName) + '</span>');
                    var de = null;
                    $.each(body.Items, function (iii, detail) {
                        if (detail.CompletionPhaseId == head.Id)
                            de = detail;
                    });

                    if (de == null)
                        $('#c_' + ii + '_' + i).html('0');
                    else
                        $('#c_' + ii + '_' + i).html(de.Quantity);

                    $($('#c_' + ii + '_' + i).parent()).removeClass('size_110');
                    if (de == null && de.Quantity > 9999)
                        $($('#c_' + ii + '_' + i).parent()).addClass('size_110');
                });
            }
        });
    }

}


$(document).ready(function () {
    var LCDKanBan = new GPRO.LCDKanBan();
    LCDKanBan.Init();
})