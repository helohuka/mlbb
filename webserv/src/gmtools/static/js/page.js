// JavaScript Document
/*****************数据改变  ul 不变*************/
//根据数据写入 li
clipInit=function (){
	 pageCon=1000;   //可更改
	 liTab=7;    //可更改
	 medCur=Math.ceil(liTab/2);
	var str="";
	str+="<ul>";
	str+="<li class='disbled' id='firstPage' onclick='FirstPage()'>首页</li>";
	str+="<li class='disbled' id='lastPage' onclick='LastPage()'>上一页</li>";
	str+="<div id='pageU' class='fl'>";
	if(liTab<=pageCon){
		for(var i=1;i<=liTab;i++){
			str+="<li id='clip"+i+"' onclick='pageInt(&#039;clip"+i+"&#039;,&#039;"+liTab+"&#039;,&#039;"+medCur+"&#039;)'>"+i+"</li>";
		}
	}else{
		for(var i=1;i<=pageCon;i++){
			str+="<li id='clip"+i+"' onclick='pageInt(&#039;clip"+i+"&#039;,&#039;"+pageCon+"&#039;,&#039;"+medCur+"&#039;)'>"+i+"</li>";
		}
	}
	str+="<li class='clear'></li>";
	str+="</div>";
	str+="<li class='BORDER' id='nextPage' onclick='NextPage()'>下一页</li>";
	str+="<li class='clear'></li>";
	str+="</ul>";
	$("#clipDIV").html(str);
	pageInt('clip1',pageCon,medCur);
}
//设置当点击的值小于预设固定值
//单击事件  选择页数
clipPage=function (cur,page){
	var str="";
	for(var i=1;i<=page;i++){
		var liId="clip"+i;
		if(cur==i){
			$("#"+liId).attr("class","curPage");
		}else{
			$("#"+liId).attr("class","BORDER");
		}
		$("#"+liId).text(i);
	}
	pageControl(cur);
}
//设置的中转站，根据获取的值更改操作
pageInt=function (obj,page,medCur){
	var value=parseInt($("#"+obj).text());
	if(value < medCur){
		clipPage(value,page);
	}else if(value >= medCur){
		clipPageMax(value,page,medCur);
	}
}
//设置当获取的值大于预设固定值
clipPageMax=function (cur,page,medCur){
	var str="";
	var startNum=cur-medCur+1;
	var maxPage=startNum+parseInt(page)-1;
	if(maxPage<pageCon){
		for(var i=1;i<=page;i++){
			var liId="clip"+i;
			if(medCur==i){
				$("#"+liId).attr("class","curPage");
			}else{
				$("#"+liId).attr("class","BORDER");
			}
			$("#clip"+i).text(startNum);
			startNum++;
		}
	}else{
		var end = new RegExp(/\d+$/);
		var page=parseInt(end.exec(page));
		var curT=cur-pageCon+page;
		var maxP=pageCon;
		for(var i=page;i>=1;i--){
			var liId="clip"+i;
			if(curT==i){
				$("#"+liId).attr("class","curPage");
			}else{
				$("#"+liId).attr("class","BORDER");
			}
			$("#"+liId).text(maxP);
			maxP--;
		}
		
	}
	pageControl(cur);
}
//首页，尾页，上一页，下一页 的样式
pageControl=function (cur){
	if(cur==1){
		$("#firstPage").attr("class","disbled");
		$("#lastPage").attr("class","disbled");
		$("#nextPage").attr("class","BORDER");
		$("#endPage").attr("class","BORDER");
	}else if(cur==pageCon){
		$("#firstPage").attr("class","BORDER");
		$("#lastPage").attr("class","BORDER");
		$("#nextPage").attr("class","disbled");
		$("#endPage").attr("class","disbled");
	}else{
		$("#firstPage").attr("class","BORDER");
		$("#lastPage").attr("class","BORDER");
		$("#nextPage").attr("class","BORDER");
		$("#endPage").attr("class","BORDER");
	}
}
//第一页 显示
FirstPage=function (){
	var forNum=parseInt(liTab);
	clipPage(1,forNum);
}
//尾页 显示
EndPage=function (){
	var maxV=parseInt(pageCon);
	clipPageMax(maxV,liTab,medCur);
}
//上一页 显示
LastPage=function (){
	var choice=$(".curPage").attr('id');
	var obj=$("#"+choice).prev().attr('id');
	pageInt(obj,liTab,medCur);
}
//下一页 显示
NextPage=function (){
	var choice=$(".curPage").attr('id');
	var obj=$("#"+choice).next().attr('id');
	pageInt(obj,liTab,medCur);
}











/******************************************************************************************************************************
														表单判断
********************************************************************************************************************************/

function checkForm(formID){
	var form1=document.getElementById(formID);
	if(form1.patientName.value.length==0)
	{
		alert("姓名不能为空");
		form1.patientName.focus();
		return false;
	}else if(form1.type.value.length==0)
	{
		alert("渠道来源不能为空");
		form1.type.focus();
		return false;
	}else if(form1.patientTel.value.length==0)
	{
		alert("电话号码不能为空");
		form1.patientTel.focus();
		return false;
	}else if(form1.age.value.length==0)
	{
		alert("年龄不能为空");
		form1.age.focus();
		return false;
	}
	if(!check_the_tel()){
		return false;	
	}
	if(!check_age()){
		return false;	
	}
	if(!check_the_email('patientEmail')){
		return false;	
	}else
	{
		return true;
	}
}
function check_the_tel(){
	var obj=document.getElementById("tel_new");
	if(obj.value==""){
		return true;	
	}
	if(obj.value.length!=11){
		alert("电话号码长度不规范，请重新输入");
		return false;
	}else if(!obj.value.match(/^(\d+)$/)){
		alert("电话号码格式不规范，请重新输入");
		return false;
	}else{
		return true;
	}
	
}
function check_age(){
	var obj=$("#age").val();
	//console.log(obj);
	if(obj==""){
		return true;	
	}
	var check_age=/^\d+$/;
	if(!check_age.test(obj)){
		alert("请注意“年龄”格式的书写");
		return false;
	}else{
		return true;	
	}
}
function check_the_email(obj){
	//console.log(obj);
	if($("#"+obj).val()==""){
		return true;
	}else{
		var check_email=/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
		 if(!check_email.test($("#"+obj).val())){ 
 			alert("请输入正确的邮箱地址!"); 
			return false;
 		 }else{
			return true;	 
		}
	}
}
/****************以上对电话进行判断***********************/




/**********************************************************************************************************************************
													测试 表情秀
***********************************************************************************************************************************/
showTime=function (){
	var width=$("#faceContent").width();
//	console.log(width);
	var row=parseInt(width/55);
//	console.log(row);
	var count=38;
	//var row=7;
	var len=Math.ceil(count/row);
	//console.log(len);
	var str="";
	for(var j=0;j<len;j++){
		str+="<div>";
		for(var i=1;i<=row;i++){
			var imgId=j*row+i;
			//console.log(imgId);
			if(imgId<=count){
				str+="<a><img src='images/"+imgId+".png' style='width:45px; height:55px; margin:5px;' onclick='imgIN(this)'></a>";
			}else{
				break;	
			}
			
		}
		str+="</div>";
	}
	$("#faceContent").html(str);	
}
imgIN=function (obj){
//	console.log(obj);
	var src=$(obj).attr("src");
	var styleCon=$(obj).attr("style");
//	console.log(src);
	
	var str="<img src="+src+" style='"+styleCon+"'>";
	$("#ImageCont").append(str); 
}
