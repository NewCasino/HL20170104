$(function(){
    var $winH = $(window).height();//获取窗口高度
    var $img = $("img.img_hc");
    var $imgH = parseInt($img.height()/2);
    var $srcDef = "images/img_hc.png";
	runing();//页面刚载入时判断要显示的图片
    $(window).scroll(function(){
        runing();//滚动刷新
    })
    function runing(){
        $img.each(function(i){//遍历img
            var $src = $img.eq(i).attr("original");//获取当前img URL地址
            var $scroTop = $img.eq(i).offset();//获取图片位置
            if($scroTop.top + $imgH >= $(window).scrollTop() && $(window).scrollTop() + $winH >= $scroTop.top+ $imgH&&$scroTop.top!=$(window).scrollTop()){//判断窗口至上往下的位置
			if($img.eq(i).attr("src") == $srcDef){
				$img.eq(i).attr("src",function(){return $src}).fadeIn(300);//元素属性交换
			}
            }
			
			
        })
    }
})