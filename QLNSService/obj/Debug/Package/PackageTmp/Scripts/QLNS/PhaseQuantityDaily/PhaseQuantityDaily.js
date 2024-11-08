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
GPRO.namespace('InsertProductivity');
GPRO.InsertProductivity = function () {
    var Global = {
        UrlAction: {
            GetLineInfo: '/InsertProductivity/GetLineInfo',
            GetProductByLine: '/InsertProductivity/GetNSNgayCuaChuyen',
            GetError: '/InsertProductivity/GetErrors',
            Save: '/InsertProductivity/SavePhaseQuantity',
            getLK: '/InsertProductivity/GetLKPhase',
            GetPhaseDayInfo: '/InsertProductivity/GetPhaseDayInfo',
            GetPhases: '/InsertProductivity/GetPhases'
        },
        Data: {
            TableType: 1,
            LineId: 0,
            dongthungid: parseInt($('#Phase').attr('dongthungid'))
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        var listLineId = getCookie('strLineIds').split(',');
        if (listLineId != null && listLineId.length > 0)
            Global.Data.LineId = listLineId;

        RegisterEvent();
        GetLines();
    }

    var RegisterEvent = function () {
        $('#line').change(function () {
            if ($('#line') != '')
                GetProducts($(this).val());
        });

        $('#type').change(function () {
            if ($(this).val() == '6')
                $('#errorType').prop('disabled', false);
            else
                $('#errorType').prop('disabled', true);
        });

        $('[save]').click(function () {
            Save();
        })

        $('#Phase').change(function () {
            if ($('#product').val() != '') {
                $.ajax({
                    url: Global.UrlAction.getLK,
                    type: 'POST',
                    data: JSON.stringify({ 'phaseId': $('#Phase').val(), 'cspId': parseInt($('#product option:selected').attr('csp')) }),
                    contentType: 'application/json charset=utf-8',
                    beforeSend: function () { $('.progress').show(); },
                    success: function (data) {
                        $('.progress').hide();
                        $('#lk').val(data.Id);
                        $('#lkkcs').val(data.Value);
                    }
                });
                GetPhaseDayInfo();
            }
        });

        $('#product').change(function () {
            if ($('#product').val() != '')
                GetPhases();
        });

        $('#phaseType').change(function () {
            if ($('#product').val() != '')
                GetPhases();
        });
    }


    function GetLines() {
        $.ajax({
            url: Global.UrlAction.GetLineInfo,
            type: 'POST',
            data: JSON.stringify({ 'Ids': Global.Data.LineId }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                var str = "<option value='0'>Không có dữ liệu chuyền</option>";
                if (data.Data.length > 0) {
                    str = '';
                    $.each(data.Data, function (i, item) {
                        str += '<option value="' + item.Value + '" cluster="' + item.Data + '">' + item.Name + '</option>';
                    });
                }
                $('#line').html(str);
                $('#line').change();
                $('select').formSelect();
            }
        });
    }

    function GetProducts(id) {
        $.ajax({
            url: Global.UrlAction.GetProductByLine,
            type: 'POST',
            data: JSON.stringify({ 'Id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                var str = "<option csp='0' value='0'>Không có dữ liệu Mã Hàng</option>";
                if (data.Data != null && data.Data.length > 0) {
                    str = '';
                    $.each(data.Data, function (i, item) {
                        str += '<option csp="' + item.Data + '" value="' + item.Value + '">' + item.Name + '</option>';
                    });
                }
                $('#product').html(str);
                $('#product').change();
                $('select').formSelect();
            }
        });
    }

    function Save() {
        var lkCD = parseInt($('#lk').val());
        var lkKCS = parseInt($('#lkkcs').val());
        var sl = parseInt($('#quantity').val());
        if ($('#product').val() == '' || $('#product').val() == '0')
            alert('Vui lòng chọn sản phẩm cần nhập sản lượng.');
        else if ($('#ht').val() == '4' && (lkCD + sl) > lkKCS && $('#Phase').val() != Global.Data.dongthungid)
            alert('Sản lượng bạn vừa nhập và lũy kế sản lượng công đoạn đã vượt lũy kế kiểm đạt. Xin vui lòng nhập sản lượng trong phạm vi lũy kế kiễm đạt.');
        else {
            var obj = {
                Id: 0,
                PhaseId: $('#Phase').val(),
                NangSuatId: $('#product').val(),
                Quantity: $('#quantity').val(),
                CommandTypeId: $('#ht').val()
            }
            $.ajax({
                url: Global.UrlAction.Save,
                type: 'POST',
                data: JSON.stringify({ 'product': obj, 'csp': parseInt($('#product option:selected').attr('csp')) }),
                contentType: 'application/json charset=utf-8',
                beforeSend: function () { $('.progress').show(); },
                success: function (data) {
                    $('.progress').hide();
                    $('#Phase').change();
                }
            });
        }
    }

    function GetPhaseDayInfo() {
        $.ajax({
            url: Global.UrlAction.GetPhaseDayInfo,
            type: 'POST',
            data: JSON.stringify({ 'phaseId': $('#Phase').val() }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('.progress').show(); },
            success: function (data) {
                $('.progress').hide();
                var tb = $('#tbsl tbody');
                tb.empty();
                var total = 0;
                if (data.length > 0) {
                    $.each(data, function (i, item) {
                        var tr = $('<tr></tr>');
                        tr.append('<td>' + item.strDate + '</td>');
                        tr.append('<td>' + item.Quantity + '</td>');
                        tr.append('<td>' + item.strCommandType + '</td>');
                        tb.append(tr);
                        if (item.CommandTypeId == 4)
                            total += item.Quantity;
                        else
                            total -= item.Quantity;
                    });
                }
                else
                    tb.append('<tr><td colspan="3">Không có dữ liệu.</td></tr>')

                $('#total').html(total);
            }
        });
    }

    function GetPhases() {
        $.ajax({
            url: Global.UrlAction.GetPhases,
            type: 'POST',
            data: JSON.stringify({ 'type': $('#phaseType').val() }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('.progress').show(); },
            success: function (data) {
                $('.progress').hide();
                var select = $('#Phase');
                select.empty();
                if (data != null && data.length > 0) {
                    $.each(data, function (i, item) {
                        select.append('<option value ="' + item.Value + '">' + item.Name + '</option>')
                    });
                }
                $('select').formSelect();
                $('#Phase').change();
            }
        });
    }
}


$(document).ready(function () {
    var InsertProductivity = new GPRO.InsertProductivity();
    InsertProductivity.Init();

})

