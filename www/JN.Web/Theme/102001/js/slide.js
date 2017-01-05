

//图片按比例缩放 
function DrawImage_zdy(ImgD,iwidth,iheight) //ImgD ID//iwidth 定义允许图片宽度  //iheight 定义允许图片高度
{  
   var image=new Image();
   image.src=ImgD.src;
   if(image.width>0 && image.height>0)
   {
	   flag=true;
	   if(image.width/image.height>= iwidth/iheight)
	   {
	       if(image.width>iwidth)
		   {
		      ImgD.width=iwidth;
			  ImgD.height=(image.height*iwidth)/image.width;
		   }
		   else
		   {
		      ImgD.width=image.width;
			  ImgD.height=image.height;
		   }
		   ImgD.alt=image.width+"×"+image.height;
	   }  
	   else
	   {
	       if(image.height>iheight)
		   {
		      ImgD.height=iheight;
			  ImgD.width=(image.width*iheight)/image.height;
		   }
		   else
		   {
		      ImgD.width=image.width;
			  ImgD.height=image.height;
		   }
	   }
    }
}
//验证手机号码 
function phones(){
	  var frm             = document.forms['phoneform'];
	  var phone           = frm.elements['phone'].value;
	  if(phone == ""){
			alert("请输入手机号码");
			frm.elements['phone'].focus();
			return false;
		}
		if(phone.length != 11){
			alert("请输入正确的手机号码");
			frm.elements['phone'].focus();
			return false;
		}
	 var regNum =/^\d*$/;
        if(!regNum.test(phone)){
			alert("请输入正确的手机号码");
			frm.elements['phone'].focus();
			return false;
		}
		return true; 	 
}

function checkAllaction(obj)
{
	for(var i=0;i<document.getElementsByTagName("input").length;i++)
	{
		if (document.getElementsByTagName("input")[i].type=="checkbox")
		{
			switch(obj)
			{
				case 1:document.getElementsByTagName("input")[i].checked=true;
				break; //全选
				case 2:document.getElementsByTagName("input")[i].checked=false;
				break; //不选
				case 3:
				{
					if(document.getElementsByTagName("input")[i].checked==true)
					{
						document.getElementsByTagName("input")[i].checked=false;
					}
					else
					{
						document.getElementsByTagName("input")[i].checked=true;
					}
				}
				break; //反选
			} 
		}
	}
}




function del_qb (dsfds) //用于判断记录有没有选中的函数 
{
	var m=0;
	flag=false;
	var str="";
	for(i=0;i<document.getElementsByTagName("input").length;i++)
	{
		if(document.getElementsByTagName("input")[i].type=="checkbox" && document.getElementsByTagName("input")[i].checked==true)
		{
			str+=document.getElementsByTagName("input")[i].value+",";
			flag=true;
			m++;
		}
	}
	if(!flag)
	{
		alert("你没有选中任何数据");
		return false ;
	}
	if(confirm('您确定要删除以上'+m+'条信息吗？'))
	{
		location.href="?msg=del_all&lx="+dsfds+"&idlist="+str;
	}
	else
	{
		return false;
	}
}


var interval = 1000; 
function ShowCountDown(leftsecond,divname) 
{ 
var day1=Math.floor(leftsecond/(60*60*24)); 
var hour=Math.floor((leftsecond-day1*24*60*60)/3600); 
var minute=Math.floor((leftsecond-day1*24*60*60-hour*3600)/60); 
var second=Math.floor(leftsecond-day1*24*60*60-hour*3600-minute*60); 
var cc = document.getElementById(divname); 
cc.innerHTML =day1+"天"+hour+"小时"+minute+"分"+second+"秒"; 
leftsecond-=1;
if (leftsecond>0){
}
window.setTimeout(function(){ShowCountDown(leftsecond,divname);}, interval); 
} 




function zxb(a,b,c,d){
	var tli=document.getElementById(b).getElementsByTagName("li");
	for(var j=0; j<=d; j++){
		document.getElementById(c+j).style.display=j==a?"block":"none";
		tli[j].className=j==a?"no":"";
		if(j==a){
			var $srcDef = "images/img_hc.png";
		    var $img = $("img.img_hc");
			var $winH = $(window).height();//获取窗口高度
			var $imgH = parseInt($img.height()/2);
			$img.each(function(i){
				var $src = $img.eq(i).attr("original");//获取当前img URL地址
				var $scroTop = $img.eq(i).offset();//获取图片位置
				if($scroTop.top + $imgH >= $(window).scrollTop() && $(window).scrollTop() + $winH >= $scroTop.top+ $imgH&&$scroTop.top!=$(window).scrollTop()){
					if($img.eq(i).attr("src") == $srcDef){
						$img.eq(i).attr("src",function(){return $src}).fadeIn(300);
					}
				}
			});
		}
	}
}

$(document).ready(function(){

  $(".sy_lc_tj_li").hover(function(){
    $(this).siblings().find(".sy_lc_tj_ul").hide();
    $(this).siblings().find("h2").show();
	$(this).find(".sy_lc_tj_ul").show();
    $(this).find("h2").hide();
	
	var $srcDef = "images/img_hc.png";
	var $img = $(this).find("img.img_hc");
	if($img.attr("src") == $srcDef){
		 var $src = $img.attr("original");
		 $img.attr("src",function(){return $src}).fadeIn(300);
	}
	
	
  });
  
  $(".fjd_gw").hover(function(){
	  $(this).addClass("hover");
   },
   function(){
	   $(this).removeClass("hover");
   }
   );
    $("#store-selector").hover(function(){
	  $(this).addClass("hover");
   },
   function(){
	    $(this).removeClass("hover");
   }
   );
   
   $(".area-list li a").click(function(){
		$('#store-selector').removeClass('hover');
		$('#store-prompt1').html($(this).attr("data-value"));
		var title=$(this).html();
		$('#store-selector .text').find("div").html(title);
		$('#store-selector .text').find("div").attr("title",title);
	});
	
	 $(".area-list1 li").click(function(){
		 $("#ps_div").hide();
		 $("#ps_div1").hide();
		
		$('#store-prompt1').html($(this).find("a").attr("data-value"));
		var title=$(this).find("a").html();
		$('#store-selecto').html(title);
		$('#store-selecto').attr("title",title);
	
	});
	
	$("#div_nav1 li").click(function(){
	    var dqdx_div=$(this).parent().find("li");
	    var dqdx_cd = dqdx_div.length;
		var indexs = dqdx_div.index(this);
		dqdx_div.removeClass("active");
		$(this).addClass("active");
		dqdx_cd=parseInt(1000/dqdx_cd)/10*indexs;
		$(this).parent().find(".activeborder").css({"left":dqdx_cd +"%"});
		$(".goodsinfo").css({"z-index":"1","opacity":"0"});
		$(".goodsinfo").hide();
		$(".goodsinfo").eq(indexs).show();
		$(".goodsinfo").eq(indexs).css({"z-index":"3","opacity":"1"});
  });
 
 
});

//跳转菜单
function MM_jumpMenu(targ,selObj,restore){ //v3.0
  eval(targ+".location='"+selObj.options[selObj.selectedIndex].value+"'");
  if (restore) selObj.selectedIndex=0;
}

function xdy_show(n)
{
document.getElementById(n).style.display="block";
}
function xdy_hide(n)
{
document.getElementById(n).style.display="none";
}

function xdy_psxz()
{
	$("#ps_div").show();
	$("#ps_div1").show();
	
}



function zxb_qxt(a,b,c,d,e){
	var tli=document.getElementById(b).getElementsByTagName("li");
	if(document.getElementById("cpys")!=undefined){
	document.getElementById("cpys").value=e;
	}
	for(var j=0; j<=d; j++){
		document.getElementById(c+j).style.display=j==a?"block":"none";
		tli[j].className=j==a?"no":"";
	}
	if (d>1){
	 var tli1=document.getElementById("cpys_ul").getElementsByTagName("li");
	for(var j=1; j<=d; j++){
		tli1[j-1].className=j==a?"selected":"";
	 }
	
	}
}