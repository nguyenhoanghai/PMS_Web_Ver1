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
GPRO.namespace('CL_Collection');
GPRO.CL_Collection = function () {
    var Global = {
        UrlAction: {
            GetCheckList: '/CheckList/GetData',
            GetData: '/Productivity/GetLCDInfo',
            GetCheckListConfig: '/Productivity/GetCheckListConfig'
        },
        Data: {
            TableType: 7,
            LineId: 0,
            paging: 0,
            TimeToChangeRow: 0,
            IsDraw: false
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
       var productivity = setInterval(function () { GetProductivity() }, 2000);
    }

    function GetProductivity() {
        $.ajax({
            url: Global.UrlAction.GetCheckList,
            type: 'POST',
            data: '',// JSON.stringify({ 'Id': Global.Data.LineId, 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    if (!Global.Data.IsDraw)
                        DrawHeader(data.Data);
                    else
                        BindDataBody(data.Data);
                }
            }
        });
    }

    function DrawHeader(data) {
        var width = (100 / (data.Head.length + 1));
        var str = '<li style="width:' + width + '%">Mã Hàng</li>';
        $.each(data.Head, function (i, item) {
            if (i == data.Head.length - 1)
                str += '<li style="width:' + (width - 1) + '%">' + item.Name + ' (%)</li>';
            else
                str += '<li style="width:' + width + '%">' + item.Name + ' (%)</li>';
        });
        $('#ul-head ul').html(str);
        DrawBody(data);
    }

    function DrawBody(data) {
        $('#LCDTH_CL_ticker').html("");
        var height = Global.Data.paging < data.Body.length ? 100 / parseInt(Global.Data.paging) : 100 / data.Body.length;
        var width = 100 / (data.Head.length + 1.2);

        if (Global.Data.paging < data.Body.length) {
            $.each(data.Body, function (ii, body) {
                if (body.Items.length > 0) {
                    var $tr = $('<li style="height:' + height + '%"></li>');
                    var $ul = $('<ul style="float:left" id="r_' + ii + '"></ul>');

                    $.each(data.Head, function (i, head) {
                        if (i == 0)
                            $ul.append('<li style="width:' + width + '%" class="title value c_0" ><span id="c_' + ii + '_00">' + body.Items[i].CustomerName + '</span></li>');
                        var de = null;
                        $.each(body.Items, function (iii, detail) {
                            if (detail.JobGroupId == head.Id)
                                de = detail;
                        });

                        if (de == null)
                            $ul.append('<li style="width:' + width + '%" class="value  c_' + (i+1) + '"><span id="c_' + ii + '_' + i + '"></span></li>');
                        else {
                            switch (de.StatusId) {
                                case 0:
                                case 1:
                                case 5:
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                case 2:
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu col-process c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                case 3:
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu col-error c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">Lỗi</span></li>'); break;
                                case 4:
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu col-ok c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                    //case 6:
                                    //    $ul.append('<li style="width:' + width + '%" class="value value6 col-ok">' + de.PercentComplete + '%</li>'); break;
                                    //case 7:
                                    //    $ul.append('<li style="width:' + width + '%" class="value value6 col-ok">' + de.PercentComplete + '%</li>'); break;
                                    //case 8:
                                    //    $ul.append('<li style="width:' + width + '%" class="value value6 col-ok">' + de.PercentComplete + '%</li>'); break;
                            }
                        }
                    });
                    $tr.append($ul);
                    $tr.append('<div style="clear:left"></div>');
                    $('#LCDTH_CL_ticker').append($tr);
                }
            });

            $('#LCDTH_CL_ticker li ul li').css('height', $('#LCDTH_CL_ticker li').css('height'));
            SetAutoScroll(data.Body.length, Global.Data.paging, Global.Data.TimeToChangeRow, 3);
        }
        else {
            $.each(data.Body, function (ii, body) {
                if (body.Items.length > 0) {
                    var $tr = $('<li style="height:' + height + '%"></li>');
               //     var $ul = $('<table  id="r_' + ii + '"><tr></tr></table>');
                    var $ul = $('<ul style="float:left" id="r_' + ii + '"></ul>');

                    $.each(data.Head, function (i, head) {
                        if (i == 0)
                          //  $ul.append('<td style="width:' + width + '%" class="title value c_' + i + '" id="c_' + ii + '_00">' + body.Items[i].CustomerName + '</td>');
                            $ul.append('<li style="width:' + width + '%" class="title value c_0" id="c_' + ii + '_00">' + body.Items[i].CustomerName + '</li>');

                        var de = null;
                        $.each(body.Items, function (iii, detail) {
                            if (detail.JobGroupId == head.Id)
                                de = detail;
                        });
                         
                        if (de == null)
                         //   $ul.append('<td style="width:' + width + '%" class="value c_' + i + '" ><span id="c_' + ii + '_' + i + '"></span></td>');
                            $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '" ><span id="c_' + ii + '_' + i + '"></span></li>');
                        else {
                            switch (de.StatusId) {
                                case 0:
                                case 1:
                                case 5:
                                //    $ul.append('<td style="width:' + width + '%" class="value c_' + i + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '%</span></td>'); break;
                                //case 2:
                                //    $ul.append('<td style="width:' + width + '%" class="value col-process c_' + i + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '%</span></td>'); break;
                                //case 3:
                                //    $ul.append('<td style="width:' + width + '%" class="value col-error c_' + i + '"><span id="c_' + ii + '_' + i + '">Lỗi</span></td>'); break;
                                //case 4:
                                    //    $ul.append('<td style="width:' + width + '%" class="value col-ok c_' + i + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '%</span></td>'); break;
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                case 2:
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu col-process c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                case 3:
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu col-error c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">Lỗi</span></li>'); break;
                                case 4:
                                    $ul.append('<li style="width:' + width + '%" class="value font_dien_tu col-ok c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                            }
                        }
                    });
                    $tr.append($ul);
                    $tr.append('<div style="clear:left"></div>');
                    $('#LCDTH_CL_ticker').append($tr);
                }
            });
            $('#LCDTH_CL_ticker li table td').css('height', $('#LCDTH_CL_ticker li').css('height'));
        }
        Global.Data.IsDraw = true;
    }

    function BindDataBody(data) {
        $.each(data.Body, function (ii, body) {
            if (body.Items.length > 0) {
                $.each(data.Head, function (i, head) {
                    if (i == 0)
                        $('#c_' + ii + '_00').html(body.Items[i].CustomerName);
                    var de = null;
                    $.each(body.Items, function (iii, detail) {
                        if (detail.JobGroupId == head.Id)
                            de = detail;
                    });

                    if (de == null)
                        $('#c_' + ii + '_' + i).html('');
                    else {
                        switch (de.StatusId) {
                            case 0:
                            case 1:
                            case 5:
                                $('#c_' + ii + '_' + i).html(de.PercentComplete);
                                $($('#c_' + ii + '_' + i).parent()).removeClass();
                                $($('#c_' + ii + '_' + i).parent()).addClass('value'); break;
                            case 2:
                                $('#c_' + ii + '_' + i).html(de.PercentComplete);
                                $($('#c_' + ii + '_' + i).parent()).removeClass();
                                $($('#c_' + ii + '_' + i).parent()).addClass('value col-process'); break;
                            case 3:
                                $('#c_' + ii + '_' + i).html('Lỗi');
                                $($('#c_' + ii + '_' + i).parent()).removeClass();
                                $($('#c_' + ii + '_' + i).parent()).addClass('value col-error'); break;
                            case 4:
                                $('#c_' + ii + '_' + i).html(de.PercentComplete);
                                $($('#c_' + ii + '_' + i).parent()).removeClass();
                                $($('#c_' + ii + '_' + i).parent()).addClass('value col-ok'); break;
                                //case 6:
                                //    $ul.append('<li style="width:' + width + '%" class="value value6 col-ok">' + de.PercentComplete + '%</li>'); break;
                                //case 7:
                                //    $ul.append('<li style="width:' + width + '%" class="value value6 col-ok">' + de.PercentComplete + '%</li>'); break;
                                //case 8:
                                //    $ul.append('<li style="width:' + width + '%" class="value value6 col-ok">' + de.PercentComplete + '%</li>'); break;
                        }
                    }
                });
            }
        });
    }

    function GetCheckListConfig() {
        $.ajax({
            url: Global.UrlAction.GetCheckListConfig,
            type: 'POST',
            data: JSON.stringify({ 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    Global.Data.paging = data.Data.paging_Collection;
                    Global.Data.TimeToChangeRow = data.Data.Timer_Collection;
                    if (data.Data.LCDConfigs.length > 0) {
                        $.each(data.Data.LCDConfigs, function (i, item) {
                            switch (item.RowType) {
                                case 1: // title
                                    $('style').append('#tb_cl_th #cl-title{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px;line-height:' + item.FontSize + 'px !important; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ;   }');
                                    break;
                                case 2: // head
                                    $('style').append('#tb_cl_th #ul-head{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px;line-height:' + item.FontSize + 'px !important; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ;  }');
                                    break;
                                case 3: // content
                                    $('style').append('#LCDTH_CL_ticker li table td { line-height:' + item.FontSize + 'px !important }');

                                    if (item.Column == null)
                                        $('style').append('#tb_cl_th #ul-content{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px;line-height:' + item.FontSize + 'px !important; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ;   }');
                                    else
                                        $('style').append('#tb_cl_th #ul-content .c_' + item.Column + '{  color:' + item.Color + '  !important; background:' + item.Background + '  !important; font-family:' + item.FontName + '  !important;font-size:' + item.FontSize + 'px  !important; line-height:' + item.FontSize + 'px !important;  font-weight:' + (item.FontWeight ? 'bold' : 'normal') + '  !important;   }');
                                    break;
                            }
                        });
                    }
                    GetProductivity();
                }
            }
        });
    }
}


$(document).ready(function () {
    var collec = new GPRO.CL_Collection();
    collec.Init();
})