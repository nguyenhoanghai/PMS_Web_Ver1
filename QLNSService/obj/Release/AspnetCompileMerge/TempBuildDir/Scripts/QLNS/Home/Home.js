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
GPRO.namespace('Home');
GPRO.Home = function () {
    var Global = {
        UrlAction: {
            
        },
        Data: {
            ListBox: [],
            ActiveBox:""
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();       
    }

    var RegisterEvent = function () {
       
        //$('[cmd="lineProductivity"]').click(function () {
        //    var windowLineProductivity = window.open("/ShowLCD/Index", "MsgLineProductivity", "fullscreen=yes, height=" + screen.height + ", width=" + screen.width);
        //    Global.Data.ListBox.push(windowLineProductivity);            
        //});
        //$('[cmd="error"]').click(function () {
        //    var windowError = window.open("/ShowLCD/LCDError", "MsgError", "fullscreen=yes, height=" + screen.height + ", width=" + screen.width);
        //    Global.Data.ListBox.push(windowError);            
        //});

        $('[action="check"]').click(function () {
            var $check = $($(this).find('.infoi'));
            if ($check.css('display') == 'none')
                $check.css('display', 'block');
            else
                $check.css('display', 'none');
        });

        $('#butLCDShow').click(function () {
            var listTableTypeId = [];
            var listLi = $('#home li');
            $.each(listLi, function (i, li) {
                if($($(this).find('.infoi')).css('display') == 'block')
                {
                    listTableTypeId.push($(this).attr('tableTypeId'));
                }
            });            
            window.open("/ShowLCD/LCDShow?listTableTypeId=" + listTableTypeId, '_blank');
        });
    }

   
}


$(document).ready(function () {
    var Home = new GPRO.Home();
    Home.Init();
})