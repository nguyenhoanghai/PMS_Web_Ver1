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
GPRO.namespace('Assignment');
GPRO.Assignment = function () {
    var Global = {
        UrlAction: {
            Gets: '/Assignment/Gets',
            Save: '/Assignment/Save',
            Delete: '/Assignment/Delete'
        },
        Data: {
            Assignments: [],
            selectedAssignment: null,
            table:null,
            Id:0
        },
        Element :{
            popupId:'modal1'
        }
    }

    this.GetGlobal = function () {
        return Global;
    }

    this.Edit = function(Id){ 
        Edit(Id);
    }

    this.Delete = function(Id){
        if(confirm('Những dữ liệu liên quan đến phân công này sẽ bị xóa. Bạn có muốn xóa phân công này ?'))
            Delete(Id);
    }

    this.Init = function () {
        RegisterEvent();
        InitPopup();
        Gets();
    }
 

    var RegisterEvent = function () {
        $('#txtLine').change(function(){
            Gets();
        });
    }

    function Gets() {
        $.ajax({
            url: Global.UrlAction.Gets,
            type: 'POST',
            data: JSON.stringify({ 'lineId': $('#txtLine').val()}),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('.progress').removeClass('hide'); },
            success: function (objs) {
                $('.progress').addClass('hide');
                if (Global.Data.table != null) {
                    Global.Data.table.destroy();
                    $('#tbAssignments').empty();
                    Global.Data.table = null;
                }
                Global.Data.Assignments = null;
                Global.Data.Assignments = objs;
                DrawTable(objs);
                ReDrawFilterAndLengthBoxForGrid(Global.Data.table);
                $('.table-responsive').show();
            }
        });
    }

    function DrawTable(objs) {
        Global.Data.table=  $("#tbAssignments").DataTable({
            "filter": true, // this is for disable filter (search box)
            "orderMulti": false, // for disable multiple column at once
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "data": objs,
            "responsive": true,
            "oLanguage": {
                "sSearch": "Bộ lọc"
            },
            "columnDefs":
                [ 
                {
                    "targets": [5],
                    "visible": true,
                    "searchable": false,
                    "orderable": false,
                    "width": "150px"
                }],

            "columns": [
                { "data": "STT_TH", "title": "STT" },
                { "data": "CommoName", "title": "Sản phẩm" },
                { "data": "ProductionPlans", "title": "Kế hoạch"  },
                { "data": "LK_TH", "title": "LK thực hiện" },
                { "data": "IsFinishStr", "title": "Trạng thái"  },
                {
                    "render": function (data, type, full, meta) { 
                        return `<i class="material-icons icon-edit  cursor" onClick="Edit(${full.STT})">edit</i> 
                    <i class="material-icons cursor  icon-delete " onClick="Delete(${full.STT})">delete</i>`;
                    }
                }]
        });

        var btnAdd = $('<button class="btn-floating btn-small waves-effect waves-light red" style="margin: 25px 0  0  15px; float:right"><i class="material-icons">add</i></button>');
        btnAdd.click(function () {
            $('#' + Global.Element.popupId).modal()[0].M_Modal.options.dismissible = false;
            $('#' + Global.Element.popupId).modal('open');
            M.updateTextFields(); 
        });
        btnAdd.insertBefore('#tbAssignments_filter');
    }     
 
    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.Delete,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('.progress').removeClass('hide'); },
            success: function (objs) {
                $('.progress').addClass('hide');
                Gets();
            }
        });
    }

    function Edit(Id){ 
        var product = $.map(Global.Data.Assignments ,function(item,i){ 
            if(item.STT == Id)
                return item;
        })[0];
        Global.Data.selectedAssignment = product;
        Global.Data.Id = product.STT;
        $('#txtProduct').val(product.ProductId);
        $('#txtstt').val(product.STT_TH);
        $('#txtSLKH').val(product.ProductionPlans); 
        $('#' + Global.Element.popupId).modal()[0].M_Modal.options.dismissible = false;
        $('#' + Global.Element.popupId).modal('open');
        M.updateTextFields(); 
        $('select').formSelect();
    }

    function InitPopup(){
        $("#modal1" ).modal({
            dismissible: false
        });

        $('#modal1 #btnSave').click(function(){
            if(CheckValidate()){ 
                var obj = {
                    STT : Global.Data.Id,
                    MaChuyen :$('#txtLine').val(),
                    MaSanPham :$('#txtProduct').val(),
                    Thang : new Date().getMonth() +1,
                    Nam :new Date().getFullYear(),
                    STTThucHien :parseInt($('#txtstt').val()),
                    SanLuongKeHoach :parseInt($('#txtSLKH').val())                    
                }
 

                $.ajax({
                    url: Global.UrlAction.Save,
                    type: 'POST',
                    data: JSON.stringify(obj),
                    contentType: 'application/json charset=utf-8',
                    beforeSend: function () { $('.progress').removeClass('hide'); },
                    success: function (response) {
                        $('.progress').addClass('hide');
                        if (response.IsSuccess) {
                            $('#modal1 #btnCancel').click(); 
                            Gets();                           
                        }
                        else
                            alert(response.Messages[0].msg); 
                    }
                });
            }
        });

        $('#modal1 #btnCancel').click(function(){ 
            $('#txtstt').val(1);
            $('#txtSLKH').val(1); 
            Global.Data.Id = 0;
            Global.Data.selectedAssignment = null;
            $("#" + Global.Element.popupId).modal("close");
        });
    }

    function CheckValidate() {
        if ($('#txtProduct').val().trim() == "") {
            alert("Vui lòng chọn sản phẩm.");
            $('#txtProduct').focus();
            return false;
        }
        else if ($('#txtstt').val().trim() == "") {
            alert("Nhập số thứ tự sản xuất.");
            $('#txtstt').focus();
            return false;
        }
        else if (parseFloat($('#txtstt').val()) <= 0) {
            alert("số thứ tự sản xuất phải lớn hơn 0.");
            $('#txtstt').focus();
            return false;
        }
        else if ($('#txtSLKH').val().trim() == "") {
            alert("Nhập sản lượng kế hoạch.");
            $('#txtSLKH').focus();
            return false;
        }
        else if (parseFloat($('#txtSLKH').val()) <= 0) {
            alert("sản lượng kế hoạch phải lớn hơn 0.");
            $('#txtSLKH').focus();
            return false;
        } 
        return true;
    }
}
