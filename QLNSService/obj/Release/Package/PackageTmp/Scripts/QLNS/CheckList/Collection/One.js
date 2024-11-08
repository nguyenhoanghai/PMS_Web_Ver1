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
GPRO.namespace('CL_One');
GPRO.CL_One = function () {
    var Global = {
        UrlAction: {
            GetCheckList: '/CheckList/GetByJGroupId',
            GetData: '/Productivity/GetLCDInfo',

            GetCheckListConfig: '/Productivity/GetCheckListConfig'
        },
        Data: {
            TableType: 8,
            LineId: 0,
            user: null,
            IsDraw: false,
            paging: 0,
            TimeToChangeRow:0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        //GetTime();
        Global.Data.user = JSON.parse(getCookie('LoginUser'));
        //if (listLineId != null && listLineId.length > 0)
        //    Global.Data.LineId = listLineId[0];
        //BuildTableLayOut();
        if ($('#jgroup').val() == '' || $('#jgroup').val() == '0')
            alert('Bạn chưa chọn Nhóm Công việc cần hiển thị cho CheckList. Vui lòng quay lại trang chọn màn hình Hiển thị để chọn lại.');
        else
            RegisterEvent();
    }

    var RegisterEvent = function () {
        GetCheckListConfig();
        var productivity = setInterval(function () { GetProductivity() }, 2000);
    }

    function GetCheckListConfig() {
        $.ajax({
            url: Global.UrlAction.GetCheckListConfig,
            type: 'POST',
            data: JSON.stringify({ 'tableTypeId': Global.Data.TableType }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null) {
                    Global.Data.paging = parseInt(data.Data.paging_Collection);
                    Global.Data.TimeToChangeRow = data.Data.Timer_Collection;
                    if (data.Data.LCDConfigs.length > 0) {
                        $.each(data.Data.LCDConfigs, function (i, item) {
                            switch (item.RowType) {
                                case 1: // title
                                    $('style').append('#tb-nguyenlieu #cl-title{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ;   }');
                                    break;
                                case 2: // head
                                    $('style').append('#tb-nguyenlieu #ul-head{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ;  }');
                                    break;
                                case 3: // content
                                   $('style').append('#LCDCT_CL_ticker li table td { line-height:' + item.FontSize + 'px !important }');

                                    if (item.Column == null)
                                        $('style').append('#tb-nguyenlieu #ul-content{height:' + item.Height + '% ;width:' + item.Width + '% ; color:' + item.Color + '; background:' + item.Background + '; font-family:' + item.FontName + ' ;font-size:' + item.FontSize + 'px; font-weight:' + (item.FontWeight ? 'bold' : 'normal') + ' ;   }');
                                    else
                                        $('style').append('#tb-nguyenlieu #ul-content .c_' + item.Column + '{  color:' + item.Color + '  !important; background:' + item.Background + '  !important; font-family:' + item.FontName + '  !important;font-size:' + item.FontSize + 'px  !important; line-height:' + item.FontSize + 'px !important;  font-weight:' + (item.FontWeight ? 'bold' : 'normal') + '  !important;   }');

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
            data: JSON.stringify({ 'JGroupId': $('#jgroup').val(), 'userId': Global.Data.user.Id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Data != null)
                    DrawHeader(data.Data);
            }
        });
    }

    function DrawHeader(data) {
        $('#tb-nguyenlieu #cl-title').html('CheckList - ' + data.Title);
   
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
        var height = Global.Data.paging < data.Body.length ? 100 / parseInt(Global.Data.paging) : (data.Body.length == 0 ? 100 : 100 / data.Body.length);
            var width = 100 / (data.Head.length + 1);
        if (data.Body.length > 0) {
            $('#LCDCT_CL_ticker').html("");  
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
                                if (detail.JobId == head.Id)
                                    de = detail;
                            });

                            if (de == null)
                                $ul.append('<li style="width:' + width + '%" class="value  font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '"></span></li>');
                            else {
                                switch (de.StatusId) {
                                    case 0:
                                    case 1:
                                    case 5:
                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                    case 2:
                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + ' col-process"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                    case 3:
                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + ' col-error"><span id="c_' + ii + '_' + i + '">Lỗi</span></li>'); break;
                                    case 4:
                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + ' col-ok"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                  }
                            }
                        });
                        $tr.append($ul);
                        $tr.append('<div style="clear:left"></div>');
                        $('#LCDCT_CL_ticker').append($tr);
                    }
                });

                $('#LCDCT_CL_ticker li ul li').css('height', $('#LCDCT_CL_ticker li').css('height'));
                SetAutoScroll(data.Body.length, Global.Data.paging, Global.Data.TimeToChangeRow, 4);
            }
            else {
                $.each(data.Body, function (ii, body) {
                    if (body.Items.length > 0) {
                        var $tr = $('<li style="height:' + height + '%"></li>');
                        //  var $ul = $('<table  id="r_' + ii + '"><tr></tr></table>');
                        var $ul = $('<ul style="float:left" id="r_' + ii + '"></ul>');

                        $.each(data.Head, function (i, head) {
                            if (i == 0)
                              //  $ul.append('<td style="width:' + width + '%" class="title value" id="c_' + ii + '_00">' + body.Items[i].CustomerName + '</td>');
                            $ul.append('<li style="width:' + width + '%" class="title value c_0" id="c_' + ii + '_00">' + body.Items[i].CustomerName + '</li>');
                            var de = null;
                            $.each(body.Items, function (iii, detail) {
                                if (detail.JobId == head.Id)
                                    de = detail;
                            });

                            if (de == null)
                               // $ul.append('<td style="width:' + width + '%" class="value" ><span id="c_' + ii + '_' + i + '"></span></td>');
                                $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '" ><span id="c_' + ii + '_' + i + '"></span></li>');
                            else {
                                switch (de.StatusId) {
                                    case 0:
                                    case 1:
                                    case 5:
                                   //     $ul.append('<td style="width:' + width + '%" class="value "><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '%</span></td>'); break;
                                   // case 2:
                                   //     $ul.append('<td style="width:' + width + '%" class="value col-process"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '%</span></td>'); break;
                                   // case 3:
                                   //     $ul.append('<td style="width:' + width + '%" class="value col-error"><span id="c_' + ii + '_' + i + '">Lỗi</span></td>'); break;
                                   // case 4:
                                   //     $ul.append('<td style="width:' + width + '%" class="value col-ok"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '%</span></td>'); break;

                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + '"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                    case 2:
                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + ' col-process"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;
                                    case 3:
                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + ' col-error"><span id="c_' + ii + '_' + i + '">Lỗi</span></li>'); break;
                                    case 4:
                                        $ul.append('<li style="width:' + width + '%" class="value font_dien_tu c_' + (i + 1) + ' col-ok"><span id="c_' + ii + '_' + i + '">' + de.PercentComplete + '</span></li>'); break;

                                }
                            }
                        });
                        $tr.append($ul);
                        $tr.append('<div style="clear:left"></div>');
                        $('#LCDCT_CL_ticker').append($tr);
                    }
                });
                $('#LCDCT_CL_ticker li table td').css('height', $('#LCDCT_CL_ticker li').css('height'));
            }
            Global.Data.IsDraw = true;
        }
        else {
            $('#LCDCT_CL_ticker').append('<li style="height:' + height + '%"><ul><li style="width:100%"><span>Không có Dữ Liệu</span></li></ul><div style="clear:left"></div></li>');
            Global.Data.IsDraw = false;
        } 
    }


}


$(document).ready(function () {
    var collec = new GPRO.CL_One();
    collec.Init();
})