// JavaScript Document
window.onload = function() {chkcookies();};
// 判断cookie是否存在
function chkcookies()
{
  var NameOfCookie="first_request_cookie";
  var c = document.cookie.indexOf(NameOfCookie+"="); 
  if (c != -1)
  {
	  $(".addToDeskTipWrap").hide();
  }
  else
  {
  // alert("第一次请求");
     $(".addToDeskTipWrap").show();
	 setTimeout(function(){$(".addToDeskTipWrap").fadeOut();},3000);
     SetCookie('first_request_cookie','1');
  }
}

// 设置cookie
function SetCookie(sName, sValue){
   date = new Date();
   date.setDate(date.getDate()+30);
   document.cookie = sName+'='+escape(sValue)+'; expires='+date.toGMTString();
}
$(function(){
	$(".addToDeskTip .close").click(function(){
		$(".addToDeskTipWrap").fadeOut();
	});
});