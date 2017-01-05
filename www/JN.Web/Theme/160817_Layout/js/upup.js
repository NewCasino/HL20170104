$(function(){
    if (!$.fn.marquee) {
        var obj_script=document.createElement("script"); 
            obj_script.type="text/javascript"; 
            obj_script.src="/cl/js/tools/marquee.js"; 
        $("body").append(obj_script);
    }
});

//進維護畫面倒數
var _upupinit = true,$upup,$upupWin,_upupHeight;

function figLeaf(H){
    if(!top.upupMsg){
        return false;
    }
    if(_upupinit){
        $('body').append(H.html());
        $upup = $('#upupMessage'), $upupC = $('#upupMessage > #upupContent');
        $upupWin = $(window), _upupMoveSpeed = 100 ,_upupDiffY = 20 , _upupDiffX = 20;
        _upupHeight = $upup.height() , _upupWidth = $upup.width();
        
            // 控制 #upupMessage 的移動
        $upupWin.bind('scroll resize', function(){
            $upup.stop().animate({
                top: $upupWin.scrollTop() + $upupWin.height() - _upupHeight - _upupDiffY,
                left: $upupWin.scrollLeft() + $upupWin.width() - _upupWidth - _upupDiffX
            }, _upupMoveSpeed);
        });
        //關閉訊息
        $('#upupMessage .close_ad').click(function(){
            $upup.hide();
        });
        _upupinit = false;
    }
    
    //距離維護開始倒數秒數
    if(top.upupMsg._UNDER['UNDER_MAINTAIN_SEQUENCE'] == true && top.upupMsg._UNDER['COUNTDOWN1'] > 0){
        var str = top.upupMsg.millisecondsStrToDate(top.upupMsg._UNDER['COUNTDOWN1']);
        if(str){
            $upup.show();
            $upupC.html(str);
        }
    }else{
        $upup.hide();
    }
    
    //廣播訊息
    if(top.upupMsg._UNDER['MARQUEE'] != "" && top.upupMsg._UNDER['MARQUEE'] != undefined){
        $upup.show();
    
        $upupC.html("<div id='marquee' direction='up' height='46'>" + top.upupMsg._UNDER['MARQUEE'] + "</div>");

        $("#marquee").marquee({speed: 120})
        .hover(function() {
            $(this).trigger("stop");
        }, function() {
            $(this).trigger("start");
        });

        top.upupMsg._UNDER['MARQUEE'] = "";
    }else if(top.upupMsg._UNDER['MARQUEE'] == ""){//如果送空字串就隱藏訊息
        return;
    }
}

//節慶style-A 中間卡片
function FestivalTypeA(html, IsShow) {
    var userAgent = window.navigator.userAgent.toLowerCase(),
    isIE8 = $.browser.msie8 = $.browser.msie && /msie 8\.0/i.test(userAgent);
    if (IsShow) {
        $('body').append(html);
        var _win = $(window),
            _box = $('#FestivalTypeA'),
            _boxH = _box.height(),
            timeoutID;
        function hideThis(){
            if(isIE8){
                _box.hide();
            }else{
                _box.fadeOut(500, function() { $(this).hide();});
            }
            }
        //自動隱藏
        function AutoHide() {
            timeoutID = window.setTimeout(hideThis, 7000);
    }

        AutoHide();

        //關閉
        _box.on('click', '#FestivalTypeA-closeBTN', function(event) {
            window.clearTimeout(timeoutID);
            hideThis();
});

    }
}
if($('#upupjs').data('ltl') == 'Y'){/*取data-ltl值判斷彩票開關是否為Y*/
    //節慶style-B 左下隱藏式圖片
    function FestivalTypeB(html, IsShow) {
        var userAgent = window.navigator.userAgent.toLowerCase(),
        isIE8 = $.browser.msie8 = $.browser.msie && /msie 8\.0/i.test(userAgent);
        $('body').append(html);
        var self = this,
            _win = $(window),
            _box = $("#FestivalTypeB"),
            _title = $("#FestivalTypeB-title"),
            _content = $("#FestivalTypeB-content"),
            _btn = $("#FestivalTypeB-btn"),
            _close = $(".FestivalTypeB-btn-close"),
            boxSpd = 300,
            timer_is_on = 0,
            timeoutID;

        this.IsShow = IsShow;

        /*彩票*/
        this.slideUpX = function () {
            _box.css({'width':'343px','height':'222px'});
            _content.stop().animate({'top':0},boxSpd, function(){
                _btn.css({'width':'50px','height':'45px'});
            });
            _title.hide();
            _close.hide();
            _box.off('mouseenter mousemove');
        };
        this.slideDownX = function () {
            _title.hide();
            _content.stop().animate({'top':'222px'},boxSpd, function(){
                _box.css({'width':'285px','height':'67px'});
                _btn.css({'width':'285px','height':'67px'});
                _title.css({'top':'67px'});
                _title.show();
                _title.animate({'top':'0px'},function(){
                    _box.on({
                        mouseenter: function(event) {
                            _close.show();
                        },
                        mousemove: function(event) {
                            _close.show();
                        }
                    });
                    _box.on('mouseleave', function(event) {
                        _close.hide();
                    });
                });
            });
            _close.css({'top':'0','right':'0'});
        };
        _close.live('click', function(event) {
            _box.remove();
        });
        /*節慶*/
        /*
        this.slideUpX = function () {
            _box.css({'width':'367px','height':'126px'});
            _content.stop().animate({'top':0},boxSpd, function(){
                _btn.css({'width':'65px','height':'65px'});
            });
            if(isIE8){
                _title.hide();
            }else{
                _title.fadeOut();
            }
        };
        this.slideDownX = function () {
            _box.css({'width':'290px','height':'65px'});
            _content.stop().animate({'top':'65px'},boxSpd, function(){
                _btn.css({'width':'290px','height':'65px'});
            });
            if(isIE8){
                _title.show();
            }else{
                _title.fadeIn();
            }
        };
        */
        //視窗自動縮下
        function AutoDown() {
            if (!timer_is_on){
                timer_is_on=1;
                timeoutID = window.setTimeout( function(){self.slideDownX();self.IsShow = false;}, 7000 );
            }
        }
        //停止計時
        function stopCount(){
            clearTimeout(timeoutID);
            timer_is_on=0;
        }

        //點擊縮放視窗
        _btn.live('click', function(event) {
            event.cancelBubble = true;
            if (self.IsShow) {
                self.IsShow = false;
                self.slideDownX();
                stopCount();
            } else {
                stopCount();
                self.IsShow = true;
                self.slideUpX();
                AutoDown();
            }
        });

        // 初始化,預設開啟
        if (this.IsShow) {
            this.slideUpX();
            AutoDown();
        } else {
            this.slideDownX();
            stopCount();
        }
    }
    $(function() {
        if (top.upupMsg && 'object' == typeof(top.upupMsg.Festival) && 'function' == typeof(top.upupMsg.Festival.callFunc)) {
            top.upupMsg.Festival.callFunc();
        } else {
            setTimeout(function () {
                if (top.upupMsg && 'object' == typeof(top.upupMsg.Festival) && 'function' == typeof(top.upupMsg.Festival.callFunc)) {
                    top.upupMsg.Festival.callFunc();
                }
            }, 1000);
        }
    });
}
/*
function mobileAD(o) {
    var mObj = mObj || {};

    $('body').append(o.html);

    mObj = {
        adWrap: $("#mobilead"),
        adBtn: $("#mobilead-close"),
        adOpen: o.isShow(),
        adSwitch: function() {
            //若曾關閉過，則不再顯示
            var lang = (top.upupMsg.$.cookie('langx') == 'vi')? 'vi': 'gb';
            if(lang == 'vi') {
                mObj.adWrap.css("background", "url(/tpl/template/upupMsg/image/mobilead/vi/app.png) no-repeat");
            }
            if(top.upupMsg.$.cookie('mobilead_close') == 'close') {
                mObj.adWrap.css("display", "none");
            }else if(mObj.adOpen) {
                mObj.adWrap.stop().animate({height: "300px"}, 650);
                mObj.adOpen = false;
            } else {
                mObj.adWrap.stop().animate({height: "106px"}, 650);
                mObj.adOpen = true;
            }
        },
        adClose: function() {
            mObj.adWrap.stop().animate({height: "0"}, 650, function() {
                top.upupMsg.$.cookie('mobilead_close', 'close', {path:'/', expires: ''});
                mObj.adWrap.css("display", "none");
            });
        }
    };

    mObj.adWrap.on('click', function() {
        mObj.adSwitch();
    });

    mObj.adBtn.on('click', function(e) {
        mObj.adClose();
        e.stopPropagation();
    });

    //初始化
    mObj.adWrap.css("display", "block");
    mObj.adSwitch();
}
$(function() {
    //大球登入頁變數 window.bigBallAd
    var bigBallShow = window.bigBallAd? bigBallAd: false;

    if (top.upupMsg && typeof(top.upupMsg.mobile) == 'object' && typeof(top.upupMsg.$.cookie) == 'function') {
        if(bigBallShow == true || (top.upupMsg.$.cookie('page_site') && top.upupMsg.$.cookie('page_site').toLowerCase() == 'first')) {
            mobileAD(top.upupMsg.mobile);
        }
    } else {
        setTimeout(function () {
            if (top.upupMsg && typeof(top.upupMsg.mobile) == 'object' && typeof(top.upupMsg.$.cookie) == 'function') {
                if(bigBallShow == true || (top.upupMsg.$.cookie('page_site') && top.upupMsg.$.cookie('page_site').toLowerCase() == 'first')) {
                    mobileAD(top.upupMsg.mobile);
                }
            }
        }, 1000);
    }
});
*/