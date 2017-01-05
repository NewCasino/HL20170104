// JavaScript Document

function login_form(thisform) //验证登录
{
with (thisform)
  {
  if(username.value.length < 2 || username.value.length > 20 || username.value=="用户名"){
   alert("请正确输入您的用户名!");
   username.focus();return false} 
  }
with (thisform)
  {
  if(password.value.length < 6 || password.value.length > 20){
  alert("请正确输入您的密码!密码不小于6个字符,不大于20个字符");
   password.focus();return false}
  }
}

function reg_form(thisform) //验证登录
{
with (thisform)
  {
  if(hy_username.value.length < 2 || hy_username.value.length > 20 || hy_username.value=="用户名" ){
   alert("请正确输入您的用户名!");
   hy_username.focus();return false} 
  }
with (thisform)
  {
  if(hy_password.value.length < 6 || hy_password.value.length > 20){
  alert("请正确输入您的密码!密码不小于6个字符,不大于20个字符");
   hy_password.focus();return false}
  }
with (thisform)
{
  if(hy_password.value!=hy_password1.value){
  alert("两次密码不一样");
   hy_password1.focus();return false}
}


}

function user_up()  //验证修改
{
	  var frm      = document.forms['form_user_up'];
	  var hy_passwordj     = frm.elements['hy_passwordj'].value;
	  var hy_password     = frm.elements['hy_password'].value;
	  var hy_password1    = frm.elements['hy_password1'].value;
	  
	   var tel             = frm.elements['tel'].value;
	  var qq              = frm.elements['qq'].value;
	  
	  var email           = frm.elements['email'].value;
	  
    if(hy_passwordj.length < 6 || hy_passwordj.length > 20){
   document.getElementById('hy_passwordjs').innerHTML="旧密码不小于6个字符,不大于20个字符";
   frm.elements['hy_passwordj'].focus();return false}
   else
   {document.getElementById('hy_passwordjs').innerHTML="";}
   
    if(hy_password.length < 6 || hy_password.length > 20){
   document.getElementById('hy_passwords').innerHTML="新密码不小于6个字符,不大于20个字符";
   frm.elements['hy_password'].focus();return false}
   else
   {document.getElementById('hy_passwords').innerHTML="";}
   
   if(hy_password!=hy_password1){
   document.getElementById('hy_password1s').innerHTML="两次密码不一致";
   frm.elements['hy_password1'].focus();return false}
   else
   {document.getElementById('hy_password1s').innerHTML="";}
   
      if(tel.length < 6 || tel.length > 20 || isNumber(tel)==0){
   document.getElementById('tels').innerHTML="请正确填写您的联系电话";
   frm.elements['tel'].focus();return false}
   else
   {document.getElementById('tels').innerHTML="";}
   
    if(qq.length < 6 || qq.length > 11 || isNumber(qq)==0){
   document.getElementById('qqs').innerHTML="请正确填写您的联系 Q Q ";
   frm.elements['qq'].focus();return false}
   else
   {document.getElementById('qqs').innerHTML="";}
   
   
   
  if(email.length < 6 || isEmail(email)==0 ){
    document.getElementById('emails').innerHTML="请填写正确的邮箱格式";
    frm.elements['email'].focus();return false}
   else
   {document.getElementById('emails').innerHTML="";}
   return true;
}

function validate_form()  //验证注册
{
	  var frm             = document.forms['form_nav'];
	  var hy_username     = frm.elements['hy_username'].value;
	  var hy_password     = frm.elements['hy_password'].value;

	   var xingming    = frm.elements['xingming'].value;
	
	  var qq              = frm.elements['qq'].value;
	 
	  
   if(hy_username.length < 2 || hy_username.length > 20){
   document.getElementById('hy_usernames').innerHTML="用户名必须在2-20个字符之间";
   frm.elements['hy_username'].focus();return false}
   else
   {document.getElementById('hy_usernames').innerHTML="";}
   
    if(hy_password.length < 6 || hy_password.length > 20){
   document.getElementById('hy_passwords').innerHTML="密码不小于6个字符,不大于20个字符";
   frm.elements['hy_password'].focus();return false}
   else
   {document.getElementById('hy_passwords').innerHTML="";}
 
   
     if(xingming.length < 1 || xingming.length > 100){
   document.getElementById('xingmings').innerHTML="请正确填写您的姓名";
   frm.elements['xingming'].focus();return false}
   else
   {document.getElementById('xingmings').innerHTML="";}

   
    if(qq.length < 6 || qq.length > 11 || isNumber(qq)==0){
   document.getElementById('qqs').innerHTML="请正确填写您的联系 Q Q ";
   frm.elements['qq'].focus();return false}
   else
   {document.getElementById('qqs').innerHTML="";}
   return true;
}


function validate_form_cj()  //验证注册
{
	  var frm             = document.forms['form_nav'];
	  var hy_username     = frm.elements['hy_username'].value;
	  var hy_password     = frm.elements['hy_password'].value;
	  var qq              = frm.elements['qq'].value;
	  
	  
   if( hy_username.length !=11 || isNumber(hy_username)==0){
   document.getElementById('hy_usernames').innerHTML="用户名必须为11位手机号码";
   frm.elements['hy_username'].focus();return false}
   else
   {document.getElementById('hy_usernames').innerHTML="";}
   
    if(hy_password.length < 6 || hy_password.length > 20){
   document.getElementById('hy_passwords').innerHTML="密码不小于6个字符,不大于20个字符";
   frm.elements['hy_password'].focus();return false}
   else
   {document.getElementById('hy_passwords').innerHTML="";}
   
    if(qq.length < 6 || qq.length > 11 || isNumber(qq)==0){
   document.getElementById('qqs').innerHTML="请正确填写您的联系 Q Q ";
   frm.elements['qq'].focus();return false}
   else
   {document.getElementById('qqs').innerHTML="";}
   return true;
}

function isEmail(str) //验证邮箱格式
	{ var myReg = /^[-_A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/; 
	if (myReg.test(str)) return true; return false; } 

function yzyh(obj,value){                                          //验证用户名
	 if(value.length < 2 || value.length > 10){
   document.getElementById('hy_usernames').innerHTML="用户名2-10个字符之间";
   obj.focus();return false}
   else
   {document.getElementById('hy_usernames').innerHTML="";}
}

function yzmm(obj,value){                                          //验证密码
	 if(value.length < 6 || value.length > 20){
   document.getElementById('hy_passwords').innerHTML="密码不小于6个字符,不大于20个字符";
   obj.focus();return false}
   else
   {document.getElementById('hy_passwords').innerHTML="";}
}

function yzqrmm(obj,value){    //验证确认密码
	if(value.length < 6 || value.length > 20)
   {document.getElementById('hy_password1s').innerHTML="密码不小于6个字符";
   obj.focus();return false}
   else if(document.getElementById('hy_password').value != value ){
   document.getElementById('hy_password1s').innerHTML="两次密码不一致";
   obj.focus();return false}
   else
   {document.getElementById('hy_password1s').innerHTML="";}
}

function yzyx(obj,value){                                          //验证邮箱
   if(value.length < 6 || value.length > 40 || isEmail(value)==0 ){
   document.getElementById('emails').innerHTML="请填写正确的邮箱格式";
   email.focus();return false}
   else
   {document.getElementById('emails').innerHTML="";}
}

function yzmm_jiu(obj,value){                                          //验证旧密码
	 if(value.length < 6 || value.length > 20){
   document.getElementById('hy_passwordjs').innerHTML="密码不小于6个字符,不大于20个字符";
   obj.focus();return false}
   else
   {document.getElementById('hy_passwordjs').innerHTML="";}
}

function isNumber(str)  { var reg = /^\d+$/;    if (reg.test(str)) return true; return false;} //验证数字格式
function isZh(str){  var reg = /^[\u4e00-\u9fa5]+$/; if (reg.test(str)) return true; return false;} //验证汉字格式


function app()  //验证简历
{
	  var frm      = document.forms['form_app'];
	  var post     = frm.elements['postname'].value;
	  var name     = frm.elements['name'].value;
	  var degree   = frm.elements['degree'].value;
	  var school   = frm.elements['school'].value;
	  var tel      = frm.elements['tel'].value;
	  var qq       = frm.elements['qq'].value;
	  var remark   = frm.elements['remark'].value;
	  var email    = frm.elements['email'].value;
	  
    if(post.length < 1 || post.length > 100){
   document.getElementById('posts').innerHTML="请选择应聘职位";
   return false;}
   else
   {document.getElementById('posts').innerHTML="";}
   
    if(name.length < 2 || name.length > 20 || isZh(name)==0){
   document.getElementById('names').innerHTML="请正确填写您的姓名,姓名只能为汉字";
   frm.elements['name'].focus();return false;}
   else
   {document.getElementById('names').innerHTML="";}
   
   if(degree.length < 2 || degree.length > 20 || isZh(degree)==0){
   document.getElementById('degrees').innerHTML="请正确填写您的最高学历";
   frm.elements['degree'].focus();return false;}
   else
   {document.getElementById('degrees').innerHTML="";}
   
   
   if(school.length < 2 || school.length > 100){
   document.getElementById('schools').innerHTML="请正确填写您的毕业学校";
   frm.elements['school'].focus();return false}
   else
   {document.getElementById('schools').innerHTML="";}
   
    if(tel.length < 6 || tel.length > 20 || isNumber(tel)==0){
   document.getElementById('tels').innerHTML="请正确填写您的联系电话";
   frm.elements['tel'].focus();return false}
   else
   {document.getElementById('tels').innerHTML="";}
   
    if(qq.length < 6 || qq.length > 11 || isNumber(qq)==0){
   document.getElementById('qqs').innerHTML="请正确填写您的联系 Q Q ";
   frm.elements['qq'].focus();return false}
   else
   {document.getElementById('qqs').innerHTML="";}
   
     if(remark.length < 1 ){
   document.getElementById('remarks').innerHTML="请填写您的其它信息 ";
   frm.elements['remark'].focus();return false}
   else
   {document.getElementById('remarks').innerHTML="";}
   
   
  if(email.length < 6 || email.length > 100 || isEmail(email)==0 ){
    document.getElementById('emails').innerHTML="请填写正确的邮箱格式";
    frm.elements['email'].focus();return false}
   else
   {document.getElementById('emails').innerHTML="";} 
   
   return true;
}


function order()  //验证订单
{
	  var frm      = document.forms['form_order'];
	  var name     = frm.elements['name'].value;
	  var tel      = frm.elements['tel'].value;
	  var qq       = frm.elements['qq'].value;
	  var email    = frm.elements['email'].value;
   
    if(name.length < 2 || name.length > 20 || isZh(name)==0){
   document.getElementById('names').innerHTML="请正确填写您的姓名,姓名只能为汉字";
   frm.elements['name'].focus();return false;}
   else
   {document.getElementById('names').innerHTML="";}
   
    if(tel.length < 6 || tel.length > 20 || isNumber(tel)==0){
   document.getElementById('tels').innerHTML="请正确填写您的联系电话";
   frm.elements['tel'].focus();return false}
   else
   {document.getElementById('tels').innerHTML="";}
   
    if(qq.length < 6 || qq.length > 11 || isNumber(qq)==0){
   document.getElementById('qqs').innerHTML="请正确填写您的联系 Q Q ";
   frm.elements['qq'].focus();return false}
   else
   {document.getElementById('qqs').innerHTML="";}
      
  if(email.length < 6 || email.length > 100 || isEmail(email)==0 ){
    document.getElementById('emails').innerHTML="请填写正确的邮箱格式";
    frm.elements['email'].focus();return false}
   else
   {document.getElementById('emails').innerHTML="";} 
   
   return true;
}

function user_up()  //验证修改
{
	  var frm      = document.forms['form_user_up'];
	  var hy_passwordj     = frm.elements['hy_passwordj'].value;
	  var hy_password     = frm.elements['hy_password'].value;
	  var hy_password1    = frm.elements['hy_password1'].value;
	  
	  
	  
    if(hy_passwordj.length < 6 || hy_passwordj.length > 16){
   alert("旧密码不小于6个字符,不大于16个字符");
   frm.elements['hy_passwordj'].focus();return false}
   
   
    if(hy_password.length < 6 || hy_password.length > 16){
   alert("新密码不小于6个字符,不大于20个字符");
   frm.elements['hy_password'].focus();return false}
   
   
   if(hy_password!=hy_password1){
   alert("两次密码不一致");
   frm.elements['hy_password1'].focus();return false}
   return true;
}

function hytx_yz()  //验证修改
{
	  var frm      = document.forms['hytx'];
	  var xingming     = frm.elements['xingming'].value;
	   var sheng     = frm.elements['sheng'].value;
	 // var tel     = frm.elements['tel'].value;
	 
	 if(xingming.length < 2 || xingming.length > 100){
   alert("请填写姓名");
   frm.elements['xingming'].focus();return false}
   
  
   if(sheng.length < 2 ){
   alert("请选择省");
   frm.elements['sheng'].focus();return false}
   
   
  //  if(tel.length < 6 || tel.length > 20 || isNumber(tel)==0){
  //  alert("请填写手机号码");
  // frm.elements['tel'].focus();return false
 //  }
   else
	{
		$("#hytx").submit();
	}
		
}

function sxfs_yz()  //验证发消息
{
	
	  var frm      = document.forms['sxfs'];
	  var email    = frm.elements['email'].value;
	  var title    = frm.elements['title'].value;
	 
	  if(  email.length>100 || email.length<1 ){
			  alert("请正确填写收件人");
			  return false;
		}
		
		 if(  title.length<1   ){
			  alert("请填写消息内容");
			  frm.elements['title'].focus();
			  return false;
		}
		
   return true;
}

function fwpj_yz()  //验证发消息
{
	
	  var frm      = document.forms['fwpj'];
	  var pinfen    = frm.elements['pinfen'].value;
	  var content    = frm.elements['content'].value;
	 
	  if(  pinfen.length<1 ){
	  
			  alert("请选择评分");
			  return false;
		}
		
		 if(  content.length<1 || content.length>250  ){
			  alert("请填写评价内容，不多于250个字");
			  frm.elements['content'].focus();
			  return false;
		}
		
   return true;
}

function hyss_yz()  //验证发消息
{
	
	  var frm      = document.forms['form_hyss'];
	  var nicheng    = frm.elements['nicheng'].value;
	  var content    = frm.elements['content'].value;
	 
	  if(  nicheng.length>100 || nicheng.length<1 ){
			  alert("请填写昵称");
			  return false;
		}
		
		 if(  content.length<1 || content.length>250  ){
			  alert("请填写评价内容，不多于250个字");
			  frm.elements['content'].focus();
			  return false;
		}
		
   return true;
}


function hy_wyyy_yz()  //验证发消息
{
	
	  var frm      = document.forms['form_hy_wyyy'];
	  var xingming    = frm.elements['xingming'].value;
	  var tel    = frm.elements['tel'].value;
	  var yysj    = frm.elements['yysj'].value;
	  var content    = frm.elements['content'].value;
	 
	  if(  xingming.length>100 || xingming.length<1 ){
			  alert("请填写姓名");
			  return false;
		}
		
		
    if(tel.length < 6 || tel.length > 20 || isNumber(tel)==0){
    alert("请填写联系电话");
   frm.elements['tel'].focus();return false}
   
     if(  yysj.length>100 || yysj.length<1 ){
			  alert("请填写预约时间");
			  return false;
		}
   
		 if(  content.length<1 || content.length>250  ){
			  alert("请填写预约内容，不多于250个字");
			  frm.elements['content'].focus();
			  return false;
		}
		
   return true;
}


function fbxq_yz()  //验证发消息
{
	
	  var frm      = document.forms['fbxq'];
	  var xming    = frm.elements['xming'].value;
	  var dhua    = frm.elements['dhua'].value;
	 
	  if(  xming.length>100 || xming.length<1 ){
			  alert("请填写姓名");
			  frm.elements['xming'].focus();
			  return false;
		}
    if(dhua.length < 6 || dhua.length > 20 || isNumber(dhua)==0){
    alert("请填写联系电话");
   frm.elements['dhua'].focus();return false}
   
    
		
   return true;
}


function zxyy_yz()  //验证发消息
{
	
	  var frm      = document.forms['form_zxyy'];
	  var yy_xm    = frm.elements['yy_xm'].value;
	  var yy_dq    = frm.elements['yy_dq'].value;
	  var yy_xb    = frm.elements['yy_xb'].value;
	  var yy_dh    = frm.elements['yy_dh'].value;
	  var yy_sj    = frm.elements['yy_sj'].value;
	  
	  if(  yy_xm.length>100 || yy_xm.length<1 ){
			  alert("请填写姓名");
			  frm.elements['yy_xm'].focus();
			  return false;
		}
      if(  yy_dq.length>100 || yy_dq.length<1 ){
			  alert("请填写地区");
			  frm.elements['yy_dq'].focus();
			  return false;
		}
		
	if(  yy_xb.length>100 || yy_xb.length<1 ){
			  alert("请填写性别");
			  frm.elements['yy_xb'].focus();
			  return false;
		}
		
		
    if(yy_dh.length < 6 || yy_dh.length > 20 || isNumber(yy_dh)==0){
    alert("请填写联系电话");
   frm.elements['yy_dh'].focus();return false}
   		
	if(  yy_sj.length>100 || yy_sj.length<1 ){
			  alert("请填写预约时间");
			  frm.elements['yy_sj'].focus();
			  return false;
		}
		
   
    
		
   return true;
}


function jrgwc(cpid,tz,sxgs)  //验证发消息
{
	
	 if( cpid.length <1 || isNumber(cpid)==0 ){
		  alert("产品ID有误");
		  return false;
	}
	if(document.getElementById("cpys")!=undefined){
		cpys=document.getElementById("cpys").value;
		if(cpys==""){
			alert("请选择颜色");
			return false;
		}
	}
	else
	{
	  cpys="";
	}
	var sxzh="";
	if(sxgs>0)
	{
		sxz=document.getElementById("cp_sxz_"+cpid+"_1").value;
		sxbt=document.getElementById("cp_sxbt_"+cpid+"_1").value;
		if(sxz==""){
			alert("请选择"+sxbt);
			return false;
		}
		sxzh=sxbt+":"+sxz;
		for(var j=2; j<=sxgs; j++){
			sxzh=sxzh+","+document.getElementById("cp_sxbt_"+cpid+"_"+j).value+":"+document.getElementById("cp_sxz_"+cpid+"_"+j).value;
			if(document.getElementById("cp_sxz_"+cpid+"_"+j).value==""){
			alert("请选择"+document.getElementById("cp_sxbt_"+cpid+"_"+j).value);
			return false;
		}
		}
	}
	if(document.getElementById('sl_'+cpid)!=undefined){
	sl=parseInt(document.getElementById('sl_'+cpid).value);
	if(isNaN(sl) ||sl==0)
	{
		sl=1;
	}
	}
	else
	{
		sl=1;
	}
	if(document.getElementById('jj_sxz_'+cpid)!=undefined){
		jjid=document.getElementById('jj_sxz_'+cpid).value;
		if(jjid==""){
			alert("请选择加价属性");
			return false;
		}
	}
	else
	{
	  jjid="";
	}
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {cpid:cpid,tjnc:"gwc",sxz:sxzh,sl:sl,jjid:jjid,cpys:cpys},   
     success: function(data){
     if (data==1)//产品ID不存在 
     {
		 alert("产品ID不存在");	
	 }
	 else if (data==4)//库存不足 
     {
		 alert("库存不足");	
	 }
	  else if (data==40)//库存不足 
     {
		 alert("零元购产品,微信端专属");	
	 }
	 else if(data==2)//已经加入购物车 
	 {
		  alert("已经加入购物车了");	
		   if(tz==1)
		  {window.location.href='ckgwc.asp';}
	 }
	 else if(data==3)//加入购物车 
	 {
		  	
		  jrgwc_sl();
		  alert("恭喜您加入购物车成功");
		  if(tz==1)
		  {window.location.href='ckgwc.asp';}
	 }
	 
     }});
}

function jrgwc_sl()  //验证发消息
{
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:"gwcsl"},   
     success: function(data){
		 $("#gwc_sl").html(data);
	  }});
	 $.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:"gwcnr"},   
     success: function(data){
		 $("#gcd_nr").html(data);
	  }});
}

function jrgwc_sl_sj()  //验证发消息
{
	$.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {tjnc:"gwcsl"},   
     success: function(data){
		 $("#gwc_sl").html(data);
	  }});
	
}

function jr_sc(cpid)  //验证发消息
{
	
	 if( cpid.length <1 || isNumber(cpid)==0 ){
		  alert("产品ID有误");
		  return false;
	}
	$.ajax({
     type: "get",     
     url: "/gwysc.asp",   
     data: {cpid:cpid,tjnc:"sc"},   
     success: function(data){
     if (data==1)//请先登录再关注 
     {
		 alert("产品ID不存在");	
	 }
	  else if(data==2)//已经关注 
	 {
		  alert("请先登录才能收藏");	
	 }
	 else if(data==3)//已经关注 
	 {
		  alert("已经收藏该产品了");	
	 }
	  else if(data==4)//已经关注 
	 {
		  alert("恭喜您，该产品收藏成功");	
	 }
	 
     }});
}

function cp_xz_sx(a,b,c,d,f){
	var tli=document.getElementById("cp_sx_"+a+"_"+b).getElementsByTagName("li");
	document.getElementById("cp_sxz_"+a+"_"+b).value=d;
	for(var j=0; j<=f; j++){
		tli[j].className=j==c?"selected":"";
	}
}

function cp_xz_sx1(a,b,c){
	var tli=document.getElementById("cpys_ul").getElementsByTagName("li");
	if(document.getElementById("cpys")!=undefined){
	document.getElementById("cpys").value=b;
	}
	for(var j=0; j<c; j++){
		tli[j].className=j==a?"selected":"";
	}
}
function xzgbtp1(bb){
	$("#xzgbtp").attr("src","../"+bb);
}

function cp_jj_sx(a,c,d,f){
	var tli=document.getElementById("jj_sx_"+a).getElementsByTagName("li");
	document.getElementById("jj_sxz_"+a).value=d;
	
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {cpid:a,tjnc:"cpxx_zxjg",jgid:d},   
     success: function(data){
		 if(data==1){
			 alert("产品不存在");
		}
		else{
			$("#projiage").html(data);
		}
	 
     }});
	 for(var j=0; j<f; j++){
		tli[j].className=j==c?"selected":"";
	}
	
}

function cp_jj_sx_sj(a,c,d,f){
	var tli=document.getElementById("jj_sx_"+a).getElementsByTagName("li");
	document.getElementById("jj_sxz_"+a).value=d;
	
	$.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {cpid:a,tjnc:"cpxx_zxjg_sj",jgid:d},   
     success: function(data){
		 if(data==1){
			 alert("产品不存在");
		}
		else{
			$("#projiage").html(data);
		}
	 
     }});
	 for(var j=0; j<f; j++){
		tli[j].className=j==c?"selected":"";
	}
	
}

function cp_jj_sx_ks(a,c,d,f){
	var tli=document.getElementById("jj_sx_"+a).getElementsByTagName("li");
	document.getElementById("jj_sxz_"+a).value=d;
	
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {cpid:a,tjnc:"cpxx_zxjg_ks",jgid:d},   
     success: function(data){
		 if(data==1){
			 alert("产品不存在");
		}
		else{
			$("#projiage_").html(data);
		}
	 
     }});
	 for(var j=0; j<f; j++){
		tli[j].className=j==c?"selected":"";
	}
	
}


function jrgwc_jf(cpid,tz,sxgs)  //验证发消息
{
	
	 if( cpid.length <1 || isNumber(cpid)==0 ){
		  alert("产品ID有误");
		  return false;
	}
	if(document.getElementById("cpys")!=undefined){
		cpys=document.getElementById("cpys").value;
		if(cpys==""){
			alert("请选择颜色");
			return false;
		}
	}
	else
	{
	  cpys="";
	}
	var sxzh="";
	if(sxgs>0)
	{
		sxz=document.getElementById("cp_sxz_"+cpid+"_1").value;
		sxbt=document.getElementById("cp_sxbt_"+cpid+"_1").value;
		if(sxz==""){
			alert("请选择"+sxbt);
			return false;
		}
		sxzh=sxbt+":"+sxz;
		for(var j=2; j<=sxgs; j++){
			sxzh=sxzh+","+document.getElementById("cp_sxbt_"+cpid+"_"+j).value+":"+document.getElementById("cp_sxz_"+cpid+"_"+j).value;
			if(document.getElementById("cp_sxz_"+cpid+"_"+j).value==""){
			alert("请选择"+document.getElementById("cp_sxbt_"+cpid+"_"+j).value);
			return false;
		}
		}
	}
	if(document.getElementById('sl_'+cpid)!=undefined){
	sl=parseInt(document.getElementById('sl_'+cpid).value);
	if(isNaN(sl) ||sl==0)
	{
		sl=1;
	}
	}
	else
	{
		sl=1;
	}
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {cpid:cpid,tjnc:"gwc_jf",sxz:sxzh,sl:sl,cpys:cpys},   
     success: function(data){
     if (data==0)//请先登录再关注 
     {
		 alert("请先登录再兑换");	
	 }
	 else if (data==5)//请先登录再关注 
     {
		 alert("库存不足，无法兑换");	
	 }
	 else if (data==1)//请先登录再关注 
     {
		 alert("积分兑换产品不存在");	
	 }
	 else if(data==2)//已经关注 
	 {
		  alert("已经加入购物车了");	
		 
	 }
	 else if(data==4)//已经关注 
	 {
		  alert("积分不足，无法兑换");	
		  
	 }
	  else if(data==3)//已经关注 
	 {
		  	
		  jrgwc_sl();
		  alert("恭喜您加入购物车成功");
		 
	 }
	 
     }});
}




function wycz_yz()  //验证充值
{
	
	  var frm      = document.forms['wycz'];
	  var total_fee    = frm.elements['total_fee'].value;
	 
	  if( total_fee<1 || isNumber(total_fee)==0 || total_fee.length<1 ){
			  alert("最低充值金额为1元");
			  frm.elements['total_fee'].focus();
			  return false;
		}
	
   return true;
}


function jrgwc_sj(cpid,tz,sxgs)  //验证发消息
{
	 if( cpid.length <1 || isNumber(cpid)==0 ){
		  alert("产品ID有误");
		  return false;
	}
	if(document.getElementById("cpys")!=undefined){
		cpys=document.getElementById("cpys").value;
		if(cpys==""){
			alert("请选择颜色");
			return false;
		}
	}
	else
	{
	  cpys="";
	}
	var sxzh="";
	if(sxgs>0)
	{
		sxz=document.getElementById("cp_sxz_"+cpid+"_1").value;
		sxbt=document.getElementById("cp_sxbt_"+cpid+"_1").value;
		if(sxz==""){
			alert("请选择"+sxbt);
			return false;
		}
		sxzh=sxbt+":"+sxz;
		for(var j=2; j<=sxgs; j++){
			sxzh=sxzh+","+document.getElementById("cp_sxbt_"+cpid+"_"+j).value+":"+document.getElementById("cp_sxz_"+cpid+"_"+j).value;
			if(document.getElementById("cp_sxz_"+cpid+"_"+j).value==""){
			alert("请选择"+document.getElementById("cp_sxbt_"+cpid+"_"+j).value);
			return false;
		}
		}
	}
	if(document.getElementById('sl_'+cpid)!=undefined){
	sl=parseInt(document.getElementById('sl_'+cpid).value);
	if(isNaN(sl) ||sl==0)
	{
		sl=1;
	}
	}
	else
	{
		sl=1;
	}
	if(document.getElementById('jj_sxz_'+cpid)!=undefined){
		jjid=document.getElementById('jj_sxz_'+cpid).value;
		if(jjid==""){
			alert("请选择加价属性");
			return false;
		}
	}
	else
	{
	  jjid="";
	}
	
	if(tz==4){
		window.location.href='lyg_flow.asp?id='+cpid+'&sxz='+sxzh+'&cpys='+cpys+'&jjid='+jjid;
	}
	else if(tz==3){
		window.location.href='bargain_flow.asp?id='+cpid+'&sxz='+sxzh+'&cpys='+cpys;
	}else{
		$.ajax({
		 type: "get",     
		 url: "../gwysc.asp",   
		 data: {cpid:cpid,tjnc:"gwc",sxz:sxzh,sl:sl,jjid:jjid,cpys:cpys},   
		 success: function(data){
		 if (data==1)//请先登录再关注 
		 {
			 alert("产品ID不存在");	
		 }
		  else if (data==4)//库存不足 
		 {
			 alert("库存不足");	
		 }
		 else if (data==40)//库存不足 
		 {
			 alert("零元购产品,微信端专属");
		 }
		 else if(data==2)//已经关注 
		 {
			  alert("已经加入购物车了");	
			   if(tz==1)
			  {window.location.href='ckgwc.asp';}
		 }
		  else if(data==3)//已经关注 
		 {
				
			  jrgwc_sl_sj();
			  alert("恭喜您加入购物车成功");
			  if(tz==1)
			  {window.location.href='ckgwc.asp';}
		 }
		 $(".cart-btns1").hide();
		 
		 }});
	 }
}


function jrgwc_sj_jf(cpid,tz,sxgs)  //验证发消息
{
	
	 if( cpid.length <1 || isNumber(cpid)==0 ){
		  alert("产品ID有误");
		  return false;
	}
	if(document.getElementById("cpys")!=undefined){
		cpys=document.getElementById("cpys").value;
		if(cpys==""){
			alert("请选择颜色");
			return false;
		}
	}
	else
	{
	  cpys="";
	}
	var sxzh="";
	if(sxgs>0)
	{
		sxz=document.getElementById("cp_sxz_"+cpid+"_1").value;
		sxbt=document.getElementById("cp_sxbt_"+cpid+"_1").value;
		if(sxz==""){
			alert("请选择"+sxbt);
			return false;
		}
		sxzh=sxbt+":"+sxz;
		for(var j=2; j<=sxgs; j++){
			sxzh=sxzh+","+document.getElementById("cp_sxbt_"+cpid+"_"+j).value+":"+document.getElementById("cp_sxz_"+cpid+"_"+j).value;
			if(document.getElementById("cp_sxz_"+cpid+"_"+j).value==""){
			alert("请选择"+document.getElementById("cp_sxbt_"+cpid+"_"+j).value);
			return false;
		}
		}
	}
	if(document.getElementById('sl_'+cpid)!=undefined){
	sl=parseInt(document.getElementById('sl_'+cpid).value);
	if(isNaN(sl) ||sl==0)
	{
		sl=1;
	}
	}
	else
	{
		sl=1;
	}
	$.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {cpid:cpid,tjnc:"gwc_jf",sxz:sxzh,sl:sl,cpys:cpys},   
     success: function(data){
     if (data==0)//请先登录再关注 
     {
		 alert("请先登录再兑换");	
	 }
	 else if (data==1)//请先登录再关注 
     {
		 alert("积分兑换产品不存在");	
	 }
	  else if (data==5)//请先登录再关注 
     {
		 alert("库存不足，无法兑换");	
	 }
	 else if(data==2)//已经关注 
	 {
		  alert("已经加入购物车了");	
		  window.location.href='ckgwc.asp';
		 
	 }
	 else if(data==4)//已经关注 
	 {
		  alert("积分不足，无法兑换");	
		  
	 }
	  else if(data==3)//已经关注 
	 {
		  	
		  jrgwc_sl_sj();
		  alert("恭喜您加入购物车成功");
		 window.location.href='ckgwc.asp';
	 }
	 
     }});
}


function gwc_sc(cpid)  //验证发消息
{
	if( cpid.length <1 || isNumber(cpid)==0 ){
		  alert("产品ID有误");
		  return false;
	}
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {cpid:cpid,tjnc:"gwc_sc"},   
     success: function(data){
     jrgwc_sl();
	 
     }});
}


function zf_ps(a,b,c,d){
	var tli=document.getElementById(a+"_ul").getElementsByTagName("li");
	document.getElementById(a).value=c;
	for(var j=0; j<d; j++){
		tli[j].className=j==b?"selected":"";
	}
}


function wytx_yz()  //验证充值
{
	
	  var frm      = document.forms['wytx'];
	  var total_fee    = frm.elements['total_fee'].value;
	  var yhzh    = frm.elements['yhzh'].value;
	  var khm    = frm.elements['khm'].value;
	  var khh    = frm.elements['khh'].value;
	  var yhwd    = frm.elements['yhwd'].value;
	 
	  if( total_fee<1 || isNumber(total_fee)==0 || total_fee.length<1 ){
			  alert("最低提现金额为1元");
			  frm.elements['total_fee'].focus();
			  return false;
		}
		
		if( isNumber(yhzh)==0 || yhzh.length<1 ){
			  alert("请输入您的银行账号");
			  frm.elements['yhzh'].focus();
			  return false;
		}
		
		if( khm.length<1 ){
			  alert("请输入您的开户名");
			  frm.elements['khm'].focus();
			  return false;
		}
		
		if( khh.length<1 ){
			  alert("请输入您的开户行");
			  frm.elements['khh'].focus();
			  return false;
		}
		
		if( yhwd.length<1 ){
			  alert("请输入您的开户网点");
			  frm.elements['yhwd'].focus();
			  return false;
		}
	 var yzpassword    = frm.elements['yzpassword'].value;
		if(  yzpassword.length<1 ){
			  alert("请填写您的密码");
			  frm.elements['yzpassword'].focus();
			  return false;
		}
	
   return true;
}


function wytx_yz2()  //验证充值
{
	
	  var frm      = document.forms['wytx2'];
	  var total_fee    = frm.elements['total_fee'].value;
	  var yhzh    = frm.elements['yhzh'].value;
	 
	 
	  if( total_fee<1 || isNumber(total_fee)==0 || total_fee.length<1 ){
			  alert("最低提现金额为1元");
			  frm.elements['total_fee'].focus();
			  return false;
		}
		
		if(  yhzh.length<1 ){
			  alert("请输入您的支付宝");
			  frm.elements['yhzh'].focus();
			  return false;
		}
		 var yzpassword    = frm.elements['yzpassword'].value;
		if(  yzpassword.length<1 ){
			  alert("请填写您的密码");
			  frm.elements['yzpassword'].focus();
			  return false;
		}
		
	
   return true;
}

function wytx_yz3()  //验证充值
{
	
	  var frm      = document.forms['wytx3'];
	  var total_fee    = frm.elements['total_fee'].value;
	  var yhzh    = frm.elements['yhzh'].value;
	 
	 
	  if( total_fee<1 ||  total_fee>2000 || isNumber(total_fee)==0 || total_fee.length<1 ){
			  alert("最低提现金额为1元，最高为2000元");
			  frm.elements['total_fee'].focus();
			  return false;
	   }
		
	    var yzpassword    = frm.elements['yzpassword'].value;
		if(  yzpassword.length<1 ){
			  alert("请填写您的密码");
			  frm.elements['yzpassword'].focus();
			  return false;
		}
   return true;
}


function gbzfss(obj) 
{
   $("#zffs").val(obj);
}
function tj_wszf() 
{
   tjzf_id=$("#zffs").val();
   document.getElementById("tjdd_"+tjzf_id).submit();
}


function closedivshow() {
        $("#div_detail").html("");
		$("#lightshow").hide();
		$("#div_overlay").hide();
}

function divshow(cpid) {
	if( cpid.length <1 || isNumber(cpid)==0 ){
		  alert("产品ID有误");
		  return false;
	}
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {cpid:cpid,tjnc:"sp_gwc"},   
     success: function(data){
     if (data==1)//请先登录再关注 
     {
		 alert("产品ID不存在");	
	 }
	  else
	 {
		 $("#div_detail").html("");
		 $("#div_detail").html(data);
		 $("#lightshow").show();
		 $("#div_overlay").hide();
	 }
	 
     }});
}

function jr_redu(obj){
	dqs_l=parseInt(document.getElementById('sl_'+obj).value);
	if(isNaN(dqs_l) ||dqs_l==0)
	{
		dqs_l=1;
	}
	if(dqs_l<2)
	{
		dqs_l=1;
	}
	else
	{
		dqs_l=dqs_l-1;
	}
	document.getElementById('sl_'+obj).value=dqs_l;
}

function jr_add(obj,kc){
	dqs_l=parseInt(document.getElementById('sl_'+obj).value);
	if(isNaN(dqs_l) ||dqs_l==0)
	{
		dqs_l=1;
	}
	else if(dqs_l>=kc)
	{
		dqs_l=kc;
	}
	else
	{
		dqs_l=dqs_l+1;
	}
	document.getElementById('sl_'+obj).value=dqs_l;
}


function lingyhj_sj(yhjid)
{
	
	 if( yhjid.length <1 || isNumber(yhjid)==0 ){
		  alert("优惠劵不存在");
		  return false;
	}
	$.ajax({
     type: "get",
     url: "../gwysc.asp",   
     data: {yhjid:yhjid,tjnc:"lqyhj"},   
     success: function(data){
     if (data==1)
     {
		 alert("请登录再领劵");	
	 }
	 else if (data==2)
     {
		 alert("优惠劵不存在");	
	 }
	 else if(data==3)
	 {
		  alert("您已经领过该优惠劵了");	
		   $("#yhj_"+yhjid).addClass("yjl");
		  $("#yhj_"+yhjid).attr("onclick","");
	 }
	 else if(data==4)
	 {
		  alert("恭喜您领取该优惠劵成功");
		 $("#yhj_"+yhjid).addClass("yjl");
		 $("#yhj_"+yhjid).attr("onclick","");
	 }
	 
     }});
}

function lingyhj(yhjid)
{
	
	 if( yhjid.length <1 || isNumber(yhjid)==0 ){
		  alert("优惠劵不存在");
		  return false;
	}
	$.ajax({
     type: "get",
     url: "gwysc.asp",   
     data: {yhjid:yhjid,tjnc:"lqyhj"},   
     success: function(data){
     if (data==1)
     {
		 alert("请登录再领劵");	
	 }
	 else if (data==2)
     {
		 alert("优惠劵不存在");	
	 }
	 else if(data==3)
	 {
		  alert("您已经领过该优惠劵了");	
		  
	 }
	 else if(data==4)
	 {
		  alert("恭喜您领取该优惠劵成功");
	 }
	 
     }});
}
function lingyhj_gb()
{
	$(".yhj_div").hide();
	$(".yhj_div_bj").hide();
}
function lingyhj_zk()
{
	$(".yhj_div").show();
	$(".yhj_div_bj").show();
}

function productSubmit(id)                                             //产品验证
{
 
	 var frm      = document.forms['sccp'];
	 var title     = frm.elements['title'].value;
	 var parent     = frm.elements['parent1'].value;
  
  
  if(title==""){
	  alert("产品名称不能为空");
	  return false;
   }
   if(parent==""){
	  alert("产品分类不能为空");
	  return false;
   }
   
   
    var ptj     = parseFloat(frm.elements['ptj'].value);
	var arj     = parseFloat(frm.elements['arj'].value);
	var xxcb     = parseFloat(frm.elements['xxcb'].value);
	var ghcb     = parseFloat(frm.elements['ghcb'].value);

	
	if(ptj<arj){
	  alert("市场价不能低于本站价");
	  return false;
	}
	
	if(arj<xxcb){
	  alert("本站价不能低于销售成本");
	  return false;
	}
	
	if(xxcb<ghcb){
	  alert("销售成本不能低于供货成本");
	  return false;
	}
	
	
   if(id=="xiugai")
   {
	   if(confirm("确定修改该产品的信息?"))
	   {
		   return true;
		}
		else
		{ return false;
		}
   }
}


//产品分类添加_新定义
function addImg2_xdy(obj)
{
var sum  = document.getElementById('pronumber');
var sum1 = Number(sum.value)+1;
tjnr=$("#cpfl_0").html();
tjnr=tjnr.replace(/(.*)(lmxzwk_1)(.*)/i, "$1lmxzwk_"+sum1+"$3");
tjnr=tjnr.replace(/(.*)(addImg2_xdy)(.*)(\[)(\+)/i, "$1removeImg2_xdys$3$4-");
tjnr=tjnr.replace(/(.*)(parent1)(.*)/i, "$1parent"+sum1+"$3");
$("#cpfl_dkj").append(tjnr);
sum.value=sum1;
}
function removeImg2_xdys(obj)
{
	var row = obj.parentNode.parentNode;
	row.innerHTML="";
	row.style.display="none";
}


//产品配图_新定义
function addcppt_xdy(obj)
{
var sum  = document.getElementById('number');
var sum1 = Number(sum.value)+1;
tjnr=$("#cppt_0").html();
tjnr=tjnr.replace(/(.*)(lmptwk_1)(.*)/i, "$1lmptwk_"+sum1+"$3");
tjnr=tjnr.replace(/(.*)(addcppt_xdy)(.*)(\[)(\+)/i, "$1removecppt_xdy$3$4-");
tjnr=tjnr.replace(/(.*)(id="picture1")(.*)/i, "$1id='picture"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="picture1")(.*)/i, "$1name='picture"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(al=pic1)(.*)/i, "$1al=pic"+sum1+"$3");
tjnr=tjnr.replace(/(.*)(id="pic1")(.*)/i, "$1id='pic"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="pic1")(.*)/i, "$1name='pic"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(al=nst1)(.*)/i, "$1al=nst"+sum1+"$3");
tjnr=tjnr.replace(/(.*)(id="nst1")(.*)/i, "$1id='nst"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="nst1")(.*)/i, "$1name='nst"+sum1+"'$3");

$("#cppt_dkj").append(tjnr);
sum.value=sum1;
}
function removecppt_xdy(obj)
{
	var row = obj.parentNode.parentNode;
	row.innerHTML="";
	row.style.display="none";
}

//产品参数_新定义
function addcpcs_xdy(obj)
{
var sum  = document.getElementById('sxnumber');
var sum1 = Number(sum.value)+1;
tjnr=$("#cpcs_0").html();
tjnr=tjnr.replace(/(.*)(lmcswk_1)(.*)/i, "$1lmcswk_"+sum1+"$3");
tjnr=tjnr.replace(/(.*)(addcpcs_xdy)(.*)(\[)(\+)/i, "$1removecpcs_xdy$3$4-");
tjnr=tjnr.replace(/(.*)(id="property1")(.*)/i, "$1id='property"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="property1")(.*)/i, "$1name='property"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(id="values1")(.*)/i, "$1id='values"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="values1")(.*)/i, "$1name='values"+sum1+"'$3");
$("#cpcs_dkj").append(tjnr);
sum.value=sum1;
}
function removecpcs_xdy(obj)
{
	var row = obj.parentNode.parentNode;
	row.innerHTML="";
	row.style.display="none";
}



//产品加价属性_新定义
function addjjcs_xdy(obj)
{
var sum  = document.getElementById('jjnumber');
var sum1 = Number(sum.value)+1;
tjnr=$("#jjcs_0").html();
tjnr=tjnr.replace(/(.*)(lmcswk1_1)(.*)/i, "$1lmcswk1_"+sum1+"$3");
tjnr=tjnr.replace(/(.*)(addjjcs_xdy)(.*)(\[)(\+)/i, "$1removejjcs_xdy$3$4-");
tjnr=tjnr.replace(/(.*)(id="sxmc1")(.*)/i, "$1id='sxmc"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="sxmc1")(.*)/i, "$1name='sxmc"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(id="sxjg1")(.*)/i, "$1id='sxjg"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="sxjg1")(.*)/i, "$1name='sxjg"+sum1+"'$3");
$("#jjcs_dkj").append(tjnr);
sum.value=sum1;
}
function removejjcs_xdy(obj)
{
	var row = obj.parentNode.parentNode;
	row.innerHTML="";
	row.style.display="none";
}
function yz_znsrsz(obj) //验证只能输入数字
{
	obj.value=obj.value.replace(/\D/g,'');
}
function yz_znsrxs(obj){
 obj.value = obj.value.replace(/[^\d.]/g,"");  //清除“数字”和“.”以外的字符  
 obj.value = obj.value.replace(/^\./g,"");  //验证第一个字符是数字而不是.
 obj.value = obj.value.replace(/\.{2,}/g,"."); //只保留第一个. 清除多余的  
 obj.value = obj.value.replace(".","$#$").replace(/\./g,"").replace("$#$",".");
 obj.value=obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/,'$1$2.$3');//只能输入两个小数
}
function yzsz_kzfhl(obj){
	 var value = obj.value;
	 if(value==""){
		 obj.value=0;
	 }
}
function del_gys(obj,id,url,action){                                                 //删除
	 var ljie="'"+url+"'";
	 $.ajax({
     type: "get",     
     url: url,   
     data: {id:id,action:action,tjnc:"gys_cp"},   
     success: function(data){
	 $(obj).parent().parent().html('');
	 }});
}

function xztx(a)
{
	for(var j=1; j<=3; j++){
		if(document.getElementById("xztx_a_"+j)!=undefined){
		document.getElementById("xztx_a_"+j).className=j==a?"no":"";
		}
		document.getElementById("xztx_div_"+j).style.display=j==a?"block":"none";
	}
}


function addcppj_xdy(obj)
{
var sum  = document.getElementById('number');
var sum1 = Number(sum.value)+1;
tjnr=$("#cp_pj_div0").html();
tjnr=tjnr.replace(/(.*)(addcppt_xdy)(.*)(\[)(\+)/i, "$1removecppt_xdy$3$4-");
tjnr=tjnr.replace(/(.*)(al=pic1)(.*)/i, "$1al=pic"+sum1+"$3");
tjnr=tjnr.replace(/(.*)(id="pic1")(.*)/i, "$1id='pic"+sum1+"'$3");
tjnr=tjnr.replace(/(.*)(name="pic1")(.*)/i, "$1name='pic"+sum1+"'$3");
$("#cp_pj_div").append(tjnr);
sum.value=sum1;
}
function removecppj_xdy(obj)
{
	var row = obj.parentNode.parentNode;
	row.innerHTML="";
	row.style.display="none";
}


function gjzdh_dycs(obj){
 iden=$(obj).val();
 $.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:'dycs',iden:iden},   
     success: function(data){
		 $("#cp_cs").html(data);
	  }
 });
 
}

function gjzdh_dycs1(obj,id){
 iden=$(obj).val();
 $.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:'dycs1',iden:iden,id:id},   
     success: function(data){
		 $("#cp_cs").html(data);
	  }
 });
 
}



function wyfx(id){
 $.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {tjnc:'wyfx',id:id},   
     success: function(data){
		 if(data=="1"){
			 alert("您的分销产品已达上限100个,\n\r请取消其它分销产品再分销这个产品")
		 }
		 else{
		 $("#wyfx_"+id).html(data);
		 }
	  }
 });
 
}

function wyfx1(id){
 $.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:'wyfx',id:id},   
     success: function(data){
		 if(data=="1"){
			 alert("您的分销产品已达上限20个,\n\r请取消其它分销产品再分销这个产品")
		 }
		 else{
		 $("#wyfx_"+id).html(data);
		 }
	  }
 });
 
}

function sjz_qd(){
	$.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {tjnc:'sjz_qd'},   
     success: function(data){
		 $("#sjz_qd").html(data);
	  }
 });
}

function sjz_qd_pc(){
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:'sjz_qd'},   
     success: function(data){
		 $("#sjz_qd").html(data);
	  }
 });
}


function yz_znsrzm_sz(obj) //验证只能输入字母数字下划线
{
	obj.value=obj.value.replace(/[^\a-\z\A-\Z0-9_]/g,'');
}


function qx_sc(obj){
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:'qx_sc',id:obj},   
     success: function(data){
		  $("#qx_sc_"+obj).remove();
	  }
 });
}

function qx_sc_sj(obj){
	$.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {tjnc:'qx_sc',id:obj},   
     success: function(data){
		 $(".qx_sc_"+obj).remove();
	  }
 });
}

function order_dcdd(){
  $("#lx").val("order_xz");
  $("#searchForm").submit();
}


function zkgmjm(){
  $(".cart-btns1").toggle();
}
function ziyuan_tj(){
  tel=document.getElementById('tel');
  if(tel.value.length!=11 || isNumber(tel.value)==0 || tel.value.substr(0,1)!="1" ){
   alert("请正确填写您的手机电话");
   tel.focus();
   return false;
   }
   else
   {
	   return true;
   }
}

function zx_wxdl(){
	
   weixindl=$("#weixindl").attr("src");
   weixindl_data=$("#weixindl").attr("data-src");
   if(weixindl==""){
	   
   $.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:'zx_wxdl'},   
     success: function(data){
		 if(data==1){
			alert("您已经登录了"); 
			location.reload();
		  }
		  else if(data==0){
			  $.ajax({
				  type: "get",
				  url: "weixingz/ewm.asp",
				  data: {},
				  success: function(data){
					  if(data==1){
						  alert("请先配置好微信"); 
						  $("#weixindl").attr("src","1");
						  $("#weixindl").attr("data-src","1");
						  $("#weixindl").parent().attr("onmouseover","xdy_show('weixindl');");
					  }
					  else{
						  $("#weixindl").attr("src","https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket="+data);
						  $("#weixindl").attr("data-src",data);
						  sTicket=data;
					      showHint(data);
					  }
				   }
				});
		  }
	  }
   });
   
   }
   else if(weixindl_data=="1"){
	    alert("请先配置好微信"); 
		$("#weixindl").parent().attr("onmouseover","xdy_show('weixindl');");
   }
   else
   {
	   sTicket=weixindl_data;
	   showHint(weixindl_data);
	}
}


function showHint() {
    if (window.XMLHttpRequest) { // 针对 IE7+, Firefox, Chrome, Opera, Safari 的代码
        xmlhttp = new XMLHttpRequest();
    } else { // 针对 IE6, IE5 的代码
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    
	xmlhttp.open("GET", "weixingz/gethint.asp?sTicket="+sTicket, true);
	xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
           if (xmlhttp.responseText == '0') {
              alert("登录失败");
              location.reload();
            }
           else if (xmlhttp.responseText == '1') {
			  alert("登录成功");
              location.reload();
            }
		   else if(xmlhttp.responseText == 'no') {
			  setTimeout("showHint()", 2000); // 定时调用 
		   }
		   else if(xmlhttp.responseText == 'no1') {
			  alert("二维码已失效");
		   }
		   
        }
    }    
    
    xmlhttp.send();	
	
} 



function product_del_order(id)     //是否确定删除
{
 if(confirm("确定退单？退单后订单将自动删除，如果已付款钱款将退到您的账户"))
 {
 window.open(id,'_self');
 }
}

function useryz_xx(objvalue){
	$.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {tjnc:'useryz_xx',objvalue:objvalue},   
     success: function(data){
		 $("#zchy_xx").html(data);
	  }
 });
	
}

function useryz_xx_pc(objvalue){
	$.ajax({
     type: "get",     
     url: "gwysc.asp",   
     data: {tjnc:'useryz_xx',objvalue:objvalue},   
     success: function(data){
		 $("#zchy_xx").html(data);
	  }
 });
	
}


function tjsjhm(){
	var frm      = document.forms['form_nav'];
	var hy_username     = frm.elements['hy_username'].value;
	if(hy_username.length!=11 || isNumber(hy_username)==0 || hy_username.substr(0,1)!="1" ){
		 $("#hy_usernames").html("请正确填写您的联系电话");
		 frm.elements['hy_username'].focus();
		 return false
	}
	else
	{
		$("#hy_usernames").html("");
	}
	
	if(document.getElementById("hy_password")!=undefined){
		var hy_password     = frm.elements['hy_password'].value;
		if(hy_password.length<6 || hy_password.length>20 ){
			 $("#hy_passwords").html("请输入6-20位密码");
			 frm.elements['hy_password'].focus();
			 return false
		}
		else{
			$("#hy_passwords").html("");
		}
		var hy_password1     = frm.elements['hy_password1'].value;
		if(hy_password!=hy_password1 ){
			$("#hy_password1s").html("两次密码不一样");
			 frm.elements['hy_password1'].focus();
			 return false
		}
		else{
			$("#hy_password1s").html("");
		}
		
	}
	
	$.ajax({
     type: "get",     
     url: "../gwysc.asp",   
     data: {tjnc:'useryz_xgtel',hy_username:hy_username,hy_password:hy_password},   
     success: function(data){
		 if(data==1){
			 alert("手机和密码修改成功");
			 window.location.href='../weixingz/test3.asp'
		 }
		 else
		 {
			 alert(data);
		 }
	  }
	 });
	
}


function order_fh(obj){
  var frm      = document.forms['form_link'];
  var kdgs    = frm.elements['kdgs'].value;
  var kdid    = frm.elements['kdid'].value;
 
  if(kdgs==""){
	  alert("请选择物流公司");
	  return false;
   }
   else if (kdid==""){
	   alert("请填写快递单号");
	  return false;
    }
   else{
$.ajax({
	  cache: true,
	  type: "POST",
	  url:"gwysc.asp?tjnc=gys_fh&id="+obj,
	  data:$('#yourformid').serialize(),// 你的formid
	  async: false,
	  error: function(request) {
		  alert(request);
	  },
	  success: function(data) {
			  alert(data);
			  CloseMsg();
			  location.reload();
		 
      }
	  });
 }

}

//晒单点赞
function single(id){
$.ajax({
	  cache: true,
	  type: "POST",
	  url:"/gwysc.asp?tjnc=single&id="+id,
	  success: function(data) {
			 $("#links"+id).html(data);
      }
	  });
 }


function yz_jbhz()  //验证充值
{
	
	  var frm      = document.forms['wytx_jbhz'];
	  var total_fee    = frm.elements['total_fee'].value;
	  var username_zc    = frm.elements['username_zc'].value;
	  var yzpassword    = frm.elements['yzpassword'].value;
	 
	 
	  if( total_fee<1 || isNumber(total_fee)==0 || total_fee.length<1 ){
			  alert("请填写转出金额");
			  frm.elements['total_fee'].focus();
			  return false;
		}
		
		if(  username_zc.length<1 ){
			  alert("请填写转出账号");
			  frm.elements['username_zc'].focus();
			  return false;
		}
		
		if(  yzpassword.length<1 ){
			  alert("请填写您的密码");
			  frm.elements['yzpassword'].focus();
			  return false;
		}
		else
		{
		$("#wytx_jbhz").submit();
		}
		
	
   return true;
}