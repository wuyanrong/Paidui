// JavaScript Document
$(function(){
	$(".menubtnbox").click(function(){
		//$(".mask").toggle();
		$(".menucon").toggle();
		$(".menubtnbox div").toggleClass("click");
	});
	//
	var index=0;
	var speed = 1000;
	var lis = $(".indfocusbox .imgs li");
	var thumbimgs = $(".indfocusbox .thumbbox span");
	function changeimg(index){
		lis.eq(index).show().siblings().hide();
		thumbimgs.eq(index).addClass("cur").siblings().removeClass("cur");
	};
	thumbimgs.click(function(){
		index = thumbimgs.index(this);	
		lis.eq(index).show().siblings().hide();
		$(this).addClass("cur").siblings().removeClass("cur");
	});
	var myTime = setInterval(function(){
	   changeimg(index);
	   index++;
	   if(index==5){index=0;}
	} , speed);
})