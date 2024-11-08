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
GPRO.namespace('Authenticate');
GPRO.Authenticate = function () {
    var Global = {
        UrlAction: {
            AuthenticateLogin: '/api/authenticate/login',
            
        },
        Data: {            
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () { 
        RegisterEvent();
            }   

    var RegisterEvent = function () {
        $('#btnLogin').click(function () {
            if (checkValidate()) {

                Login();               
            }            
        });
        
        $('body').keypress(function (event) {
            if (event.which == 13) {
                $('#btnLogin').click();
                return false;
            }
        });
    }   


    function Login() {
        var modelLogin = { UserName: $('#txtUsername').val(), PassWord: $('#txtPassword').val() };
        $.ajax({
            url: Global.UrlAction.AuthenticateLogin,
            type: 'POST',
            data: modelLogin,
            dataType: 'json',
            beforeSend: function () { $('#loading').show(); },
            success: function (data, textStatus, xhr) {
                $('#loading').hide();
                if (data.Result == "OK") {
                    setCookie('strLineIds', data.Data, 2, '/')
                    location.href = "/Home/Index";
                }
                else {
                    $('#lblMessage').html(data.ErrorMessages[0]);
                  
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                $('#lblMessage').html("Lỗi xử lý.")
            }
        });
    }
    function checkValidate() {
        if ($('#txtUsername').val().trim() == '')
        {
            GlobalCommon.ShowMessageDialog("Tên đăng nhập không được trống.", function () { }, "Lỗi Nhập Liệu.");
            $('#txtUsername').focus();
            return false;
        }
        else if ($('#txtPassword').val().trim() == '') {
            GlobalCommon.ShowMessageDialog("Mật khẩu không được trống.", function () { }, "Lỗi Nhập Liệu.");
            $('#txtPassword').focus();
            return false;

        }
        else {            
            return true;
        }
       
        
    }
}

$(document).ready(function () {
    var Authenticate = new GPRO.Authenticate();
    Authenticate.Init();
})