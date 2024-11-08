$(document).ready(function () {
    $('#vtemslideshow1').cycle({
        // fx: 'scrollDown,custom,turnUp,turnDown,fade,turnLeft,turnRight,curtainX',blindX
        fx: 'blindX,blindY,blindZ,cover,fadeZoom,toss',
        //fx: 'all',
        easing: 'easeOutBack',
        timeout: 1000,
        speed: @ViewData["timeChangeLCD"].ToString(),
        next: '#cycle_next',
        prev: '#cycle_prev',
        pager: '#vtemnav',
        pagerEvent: 'click',
        pagerAnchorBuilder: pagerFactory,
        startingSlide: 0,
        fit: true,
        height: @ViewData["Height"].ToString() ,
        //width: true
    });
    function pagerFactory(idx, slide) {
        return '#vtemnav a:eq(' + idx + ') span';
    };
});

var myVar = setInterval(function () { GetTime(); GetVideoSchedule();  }, 1000);

function GetTime() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    var date =  (dd < 10? '0'+dd:dd) + '-' + (mm < 10 ? '0'+mm:mm) + '-' + yyyy;

    var hours = today.getHours();
    var minutes = today.getMinutes();
    var second = today.getSeconds();
    var time = (hours < 10 ? '0' + hours : hours) + ':' + (minutes < 10 ? '0' + minutes : minutes) + ':' + (second < 10 ? '0' + second : second);

    $('[date]').html(date);
    $('[time]').html(time);
}

function GetVideoSchedule() {
    $.ajax({
        url: '/VideoSchedule/GetVideoSchedule',
        type: 'POST',
        data: JSON.stringify({ 'lineId':  $('#box_video').attr('line') }),
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            var videoPlayer = document.getElementById("video_html5_api");
            if (data.Data != null ) {
                var today = new Date();
                var hours = today.getHours();
                var minutes = today.getMinutes();

                if ((minutes <= data.Data.TimeEnd.Minutes || data.Data.TimeEnd.Hours > hours ))
                {
                    $('#vtemslideshow_wapper').css('display','none');
                    $('#box_video').css('display','block');
                    if( data.Data.Id != parseInt($('#video_list').attr('currentId'))){
                        $('#video_list').attr('currentId',data.Data.Id);
                        if(data.Data.Detail.length > 0 ){
                            var str = '';
                            var src='';
                            $.each(data.Data.Detail, function(i, item){
                                str += '<option value="'+item.Path+'">'+item.Name+'</option>';
                                src+='<source src="/Media/'+item.Path+'" type="video/mp4">';
                            });
                            $('#video_list').empty().append(str);
                            $('#video_list option').eq(0).prop('selected', true);
                            //  $('#video').empty().append(src);

                            $('#video_html5_api').attr('src',"/Media/"+$('#video_list').val());
                            $('#video_html5_api').attr('autoplay','autoplay');

                            var myPlayer =  videojs('video_html5_api');

                            $('#btn').click();

                            videoPlayer.addEventListener("ended", function () {
                                var i= $('#video_list').prop('selectedIndex');
                                if($('#video_list option').length > (i+1))
                                {  $('#video_list option').eq((i+1)).prop('selected', true);
                                    videoPlayer.src = "/Media/"+$('#video_list').val() ;
                                    videoPlayer.autoplay = "autoplay";
                                }
                            });
                        }
                    }
                }
                else
                {
                    $('#box_video').css('display','none');
                    $('#vtemslideshow_wapper').css('display','block');
                    videoPlayer.pause();
                    $('#video_list').attr('currentId',0);
                }
            }
            else
            {
                $('#box_video').css('display','none');
                $('#vtemslideshow_wapper').css('display','block');
                videoPlayer.pause();
                $('#video_list').attr('currentId',0);
            }
        }
    });
}

function goFullscreen(id) {
    var element = document.getElementById(id);
    if (element.mozRequestFullScreen) {
        element.mozRequestFullScreen();
    } else if (element.webkitRequestFullScreen) {
        element.webkitRequestFullScreen();
    }
}

function SetAutoScroll(rows, paging, tick , type) {
    switch (type) {
        case 1:
            if (rows > paging) {
                $('#LCDTH_ticker').totemticker({
                    row_height: $('#tb_pro_collec #LCDTH_ticker ul li').css('height'),
                    mousestop: false,
                    speed: 800,
                    interval: tick,
                    max_items: paging,
                    row_move: 1,
                    type: 'ul'
                });
            }
            break;
        case 2:
            if (rows > paging) {
                $('#KanBan_Ticker').totemticker({
                    row_height: $('#KanBan_Ticker ul li').css('height'),
                    mousestop: false,
                    speed: 800,
                    interval: parseInt(tick),
                    max_items: parseInt(paging),
                    row_move: 1,
                    type: 'ul'
                });
            }
            break;
        case 3:
            if (rows > paging) {
                $('#LCDTH_CL_ticker').totemticker({
                    row_height: $('#LCDTH_CL_ticker ul li').css('height'),
                    mousestop: false,
                    speed: 800,
                    interval: parseInt(tick),
                    max_items: parseInt(paging),
                    row_move: 1,
                    type: 'ul'
                });
            }
            break;
        case 4:
            if (rows > paging) {
                $('#LCDCT_CL_ticker').totemticker({
                    row_height: $('#LCDCT_CL_ticker ul li').css('height'),
                    mousestop: false,
                    speed: 800,
                    interval: parseInt(tick),
                    max_items: parseInt(paging),
                    row_move: 1,
                    type: 'ul'
                });
            }
            break;
        case 5:
            if (rows > paging) {
                $('#LCD_NS_New_ticker').totemticker({
                    row_height: $('#LCD_NS_New_ticker li').css('height'),
                    mousestop: false,
                    speed: 800,
                    interval: parseInt(tick),
                    max_items: parseInt(paging),
                    row_move: 1,
                    type: 'ul'
                });
            }
            break;
        case 6:
            if (rows > paging) {
                $('#LCD_Complete_ticker').totemticker({
                    row_height: $('#LCD_Complete_ticker li').css('height'),
                    mousestop: false,
                    speed: 800,
                    interval: parseInt(tick),
                    max_items: parseInt(paging),
                    row_move: 1,
                    type: 'ul',
                    stop        :   '#6_stop',
                    start       :   '#6_start',
                });
            }
            break;
    }

}

$(function() {
    // check native support
    //    $('#support').text($.fullscreen.isNativelySupported() ? 'supports' : 'doesn\'t support');

    // open in fullscreen
    $('#full').click(function() {
        //$('#slide').fullscreen();
        //$('#full').hide();
        //$('#dis-full').show();
        //return false;


        var element = document.getElementById('fullscreen');

        // These function will not exist in the browsers that don't support fullscreen mode yet,
        // so we'll have to check to see if they're available before calling them.

        if (element.mozRequestFullScreen) {
            // This is how to go into fullscren mode in Firefox
            // Note the "moz" prefix, which is short for Mozilla.
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullScreen) {
            // This is how to go into fullscreen mode in Chrome and Safari
            // Both of those browsers are based on the Webkit project, hence the same prefix.
            element.webkitRequestFullScreen();
        }
    });

    // exit fullscreen
    $('#dis-full').click(function() {
        $.fullscreen.exit();
        $('#full').show();
        $('#dis-full').hide();
        return false;
    });

    // document's event
    $(document).bind('fscreenchange', function(e, state, elem) {
        // if we currently in fullscreen mode
        if ($.fullscreen.isFullScreen()) {
            $('#fullscreen .requestfullscreen').hide();
            $('#fullscreen .exitfullscreen').show();
        } else {
            $('#fullscreen .requestfullscreen').show();
            $('#fullscreen .exitfullscreen').hide();
        }

        $('#state').text($.fullscreen.isFullScreen() ? '' : 'not');
    });
});

(function() {
    var
        fullScreenApi = {
            supportsFullScreen: false,
            nonNativeSupportsFullScreen: false,
            isFullScreen: function() { return false; },
            requestFullScreen: function() {},
            cancelFullScreen: function() {},
            fullScreenEventName: '',
            prefix: ''
        },
        browserPrefixes = 'webkit moz o ms khtml'.split(' ');

    // check for native support
    if (typeof document.cancelFullScreen != 'undefined') {
        fullScreenApi.supportsFullScreen = true;
    } else {
        // check for fullscreen support by vendor prefix
        for (var i = 0, il = browserPrefixes.length; i < il; i++ ) {
            fullScreenApi.prefix = browserPrefixes[i];

            if (typeof document[fullScreenApi.prefix + 'CancelFullScreen' ] != 'undefined' ) {
                fullScreenApi.supportsFullScreen = true;
                break;
            }
        }
    }

    // update methods to do something useful
    if (fullScreenApi.supportsFullScreen) {
        fullScreenApi.fullScreenEventName = fullScreenApi.prefix + 'fullscreenchange';

        fullScreenApi.isFullScreen = function() {
            switch (this.prefix) {
                case '':
                    return document.fullScreen;
                case 'webkit':
                    return document.webkitIsFullScreen;
                default:
                    return document[this.prefix + 'FullScreen'];
            }
        }
        fullScreenApi.requestFullScreen = function(el) {
            return (this.prefix === '') ? el.requestFullScreen() : el[this.prefix + 'RequestFullScreen']();
        }
        fullScreenApi.cancelFullScreen = function(el) {
            return (this.prefix === '') ? document.cancelFullScreen() : document[this.prefix + 'CancelFullScreen']();
        }
    }
    else if (typeof window.ActiveXObject !== "undefined") { // IE.
        fullScreenApi.nonNativeSupportsFullScreen = true;
        fullScreenApi.requestFullScreen = fullScreenApi.requestFullScreen = function (el) {
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }
        fullScreenApi.isFullScreen = function() {
            return document.body.clientHeight == screen.height && document.body.clientWidth == screen.width;
        }
    }

    // jQuery plugin
    if (typeof jQuery != 'undefined') {
        jQuery.fn.requestFullScreen = function() {

            return this.each(function() {
                if (fullScreenApi.supportsFullScreen) {
                    fullScreenApi.requestFullScreen(this);
                }
            });
        };
    }

    // export api
    window.fullScreenApi = fullScreenApi;
})();

function check(){
    if(!fullScreenApi.isFullScreen())
        fullScreenApi.requestFullScreen(document.documentElement);
    else
        fullScreenApi.cancelFullScreen(document.documentElement);
}

$('#request-full').click(function(){
    check();
});

function launchFullscreen(element) { 
    window.scrollTo(0,1);
}

function exitFullscreen() {
    if(document.exitFullscreen) {
        document.exitFullscreen();
    } else if(document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
    } else if(document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
    }
}

function dumpFullscreen() {
    console.log("document.fullscreenElement is: ", document.fullscreenElement || document.mozFullScreenElement || document.webkitFullscreenElement || document.msFullscreenElement);
    console.log("document.fullscreenEnabled is: ", document.fullscreenEnabled || document.mozFullScreenEnabled || document.webkitFullscreenEnabled || document.msFullscreenEnabled);
}

// Events
document.addEventListener("fullscreenchange", function(e) {
    console.log("fullscreenchange event! ", e);
});
document.addEventListener("mozfullscreenchange", function(e) {
    console.log("mozfullscreenchange event! ", e);
});
document.addEventListener("webkitfullscreenchange", function(e) {
    console.log("webkitfullscreenchange event! ", e);
});
document.addEventListener("msfullscreenchange", function(e) {
    console.log("msfullscreenchange event! ", e);
});
