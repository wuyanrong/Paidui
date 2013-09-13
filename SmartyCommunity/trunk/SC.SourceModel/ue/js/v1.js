function windowBack()
{
	if(window.history.length > 1)
	{
		window.history.go(-1);
	}
	else
	{
		window.location = "index.html";
	}
}
//弹出框显示
var tit1;
var tit2;
var okTit;
var cancelTit;
var popType;
var popHtml;
var outstr;
function showMsg(type,tit1,tit2,okTit,cancelTit)
{
	//var pageHeight = $(window).height();
	//var pageWidth = document.body.scrollWidth;
	//var popWidth = $(".zpopWrap").width();
	//var ml = popWidth/2;
	$(".zpopWrap:last").show();	
	if(type == '1')
	{
		outstr = "<ul class=\"twobtn onebtn\"><li><div class=\"ok btn btnbg\">" + okTit + "</div></li></ul>";
	}
	else
	{
		outstr = "<ul class=\"twobtn\"><li><div class=\"ok btn btnbg\">" + okTit + "</div></li><li><div class=\"cancel btn btnbg2\">" + cancelTit+ "</div></li></ul>";

	}
	popHtml = "<div class=\"zmask\"></div><div class=\"zpopWrap\"><div class=\"zpop\"><div class=\"tit1\">" + tit1 + " </div><div class=\"tit2\">" + tit2 + "</div>" + outstr + "</div></div>";
	$("body:first").append(popHtml);
	$(".zmask").show();
	var popHeight = $(".zpopWrap").height();
	var mt = popHeight/2;
	$(".zpopWrap").css("margin-top",-mt);
}
//
$(function(){
	//弹出框确定取消按钮点击
		$(".zpop .twobtn li .btn.ok").live("click",function(){
			$(".zmask").hide();
			$(".zpopWrap").hide();
		});
		$(".zpop .twobtn li .btn.cancel").live("click",function(){
			$(".zmask").hide();
			$(".zpopWrap").hide();
		});
	//
	$(".fwRadiusWrap .mainWrap li").click(function(){
		var index  = $(this).index();
		$(".fwRadiusWrap .mainWrap li").eq(index).addClass("cur").siblings("li").removeClass("cur");
	});
	
 	//fwPrice
	if($(".fwPriceList .fwDl dl").length)
	{
		$(".fwPriceBtn .del").removeAttr("disabled");
	}
	else
	{
		$(".fwPriceBtn .del").attr("disabled","disadled");
	}
	//点击电话图标弹出电话框
	$(".sellerList li .telWrap,.telWrapOut .btnbg").click(function(){
		$(".zmask").show();
		$(".zTelPop").show();
	});
	$(".zTelPop .btnbg2").click(function(){
		$(".zmask").hide();
		$(".zTelPop").hide();
	});
	
});