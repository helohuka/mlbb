$(function(){
	var dauarr=[];
	var dnuarr=[];
	var account_dauarr=[];
	var account_dnuarr=[];
	var id=[]
	function content(url,docId,doc){
		$.ajax({
		type:"Post",
		url:url,
		success:function(data){
			var content=JSON.parse(data)
			var line = echarts.init(document.getElementById(docId));
			var date=[];
			var num=[];
			var id=[]
			var arr=[] 
			var name=[]
			for(var key in content){
			var tim=content[key]
			for(var i=0;i<tim.length;i++){
			date.push(tim[i].Date)
			num.push(tim[i].Num)
			id.push(tim[i].ServId)
			name.push(tim[i].Name)
			}
			}
			doc.html(date[0])
			for(var j=0;j<id.length;j++){
			arr[j]={
			value:num[j],
			name:name[j],
			data:date[j]
			}
			}
			line.setOption({
				tooltip: {
				trigger: 'item',
				},
				legend: {
				orient: 'vertical',
				x: 'left',
				data:name,
				},
				series: [
				{
				name:'访问来源',
				type:'pie',
				selectedMode: 'single',
				radius: [0, '30%'],
				label: {
				normal: {
				position: 'inner'
				}
				},
				labelLine: {
				normal: {
				show: false
				}
				}
				},
				{
				name:'访问来源',
				type:'pie',
				radius: ['50%', '65%'],
				data:arr
				}
				]
				})						
			}	
		})
	}
	content("/player/dau",'ring',$("p"))
	content("/player/dnu",'dnu',$("p"))
	content("/account/dau",'account_ring',$("p"))
	content("/account/dnu",'account_dnu',$("p"))
	function dataCon(docu,open_time,close_time,con,url,tal){
		docu.click(function(){
		var stropen=open_time.val()
		var strclose=close_time.val()
		$.ajax({
			type:"Post",
			url:url,
			data:{open:stropen,close:strclose},
			success:function(data){
				var content=JSON.parse(data)
				var data = [];
				for(var key in content){
				var key=content[key]
				for(var i=0;i<key.length;i++)
				{
				data[i] =[key[i].Name,key[i].Num,key[i].Date];
				con[i]={
				id:key[i].ServId,
				name:key[i].Name,
				num:key[i].Num
				}
				}
				}
				var cs = new table({
				"tableId":tal,    //必须 表格id
				"headers":["服务器","人数","日期"],   //必须 thead表头
				"data":data,         //必须 tbody 数据展示
				"displayNum": 20,    //必须   默认 10  每页显示行数
				"groupDataNum":1,     //可选    默认 10  组数
				"display_tfoot":true, // true/false  是否显示tfoot --- 默认false
				"bindContentTr":function(){ //可选 给tbody 每行绑定事件回调
				this.tableObj.find("tbody").on("click",'tr',function(e){
				return false;
				var tr_index = $(this).data("tr_index");        // tr行号  从0开始
				var data_index = $(this).data("data_index");   //数据行号  从0开始
				})
				},
				sort:true,    // 点击表头是否排序 true/false  --- 默认false
				search:true   // 默认为false 没有搜索
				});
			}	
		})
	})
	}
	dataCon($("#btn"),$("#opendate"),$("#closedate"),dauarr,"/player/dau_date","dau_table")
	dataCon($("#newbtn"),$("#newdate"),$("#olddate"),dnuarr,"/player/dnu_date","dnu_table")
	dataCon($("#account_btn"),$("#account_opendate"),$("#account_closedate"),account_dauarr,"/account/dau_date","account_dau_table")
	dataCon($("#account_newbtn"),$("#account_newdate"),$("#account_olddate"),account_dnuarr,"/account/dnu_date","account_dnu_table")
	$(document).on("click","#dau_btn",function(){
		var id=$("#dau_sev_id").val()
		var str=0
		for(var i=0;i<dauarr.length;i++){
			if(dauarr[i].id==id||dauarr[i].name==id){
				str+=parseInt(dauarr[i].num);
			}
		}
			$("#dau_data").val(str)
	})
	$(document).on("click","#old_btn",function(){
		var id=$("#dnu_sev_id").val()
		var str=0
		for(var i=0;i<dnuarr.length;i++){
			if(dnuarr[i].id==id||dnuarr[i].name==id){
				str+=parseInt(dnuarr[i].num);
			}
		}
		$("#dnu_data").val(str)
	})
	$(document).on("click","#account_dau_btn",function(){
		var id=$("#account_dau_sev_id").val()
		var str=0
		for(var i=0;i<account_dauarr.length;i++){
			if(account_dauarr[i].id==id||account_dauarr[i].name==id){
				str+=parseInt(account_dauarr[i].num);
			}
		}
			$("#account_dau_data").val(str)
	})
	$(document).on("click","#account_old_btn",function(){
		var id=$("#account_dnu_sev_id").val()
		var str=0
		for(var i=0;i<account_dnuarr.length;i++){
			if(account_dnuarr[i].id==id||account_dnuarr[i].name==id){
				str+=parseInt(account_dnuarr[i].num);
			}
		}
		$("#account_dnu_data").val(str)
	})
	$.ajax({
		type:"post",
		url:"../player/rrs",
		dataType:"json",
		success:function(data){
			var text = [];
			for(var key in data){
				var content=data[key]
				for(var i=0;i<content.length;i++){
					var sev_id=content[i].ServerId
					id[i]={
						Id:sev_id,
						PFID:content[i].PFID,
						PFName:content[i].PFName,
						AccountName:content[i].AccountName,
						RoleId:content[i].RoleId,
						RoleLevel:content[i].RoleLevel,
						Payment:content[i].Payment,
						Name:content[i].Name
						}
						text[i] =[content[i].PFID,content[i].PFName,content[i].AccountName,content[i].RoleId,content[i].RoleLevel,content[i].Payment,content[i].Name];
				}
			}
			var cs = new table({
				"tableId":"player_rss_table",    //必须 表格id
				"headers":["PFID","PFName","AccountName","角色ID","角色等级","充值金额","服务器"],   //必须 thead表头
				"data":text,         //必须 tbody 数据展示
				"displayNum": 20,    //必须   默认 10  每页显示行数
				"groupDataNum":1,     //可选    默认 10  组数
				"display_tfoot":true, // true/false  是否显示tfoot --- 默认false
				"bindContentTr":function(){ //可选 给tbody 每行绑定事件回调
					this.tableObj.find("tbody").on("click",'tr',function(e){
						return false;
						var tr_index = $(this).data("tr_index");        // tr行号  从0开始
						 var data_index = $(this).data("data_index");   //数据行号  从0开始
					})
				},
				sort:true,    // 点击表头是否排序 true/false  --- 默认false
				search:true   // 默认为false 没有搜索
			});
		}
	})
	$(".player_input").click(function(){
			$("#player_severs").removeClass("hide").addClass("show")
		})
	$(document).on("click",".player_rss",function(){
			$("#player_rss_table").hide()
			$(".player_input").val($(this).html())
			$(this).parent().removeClass("show").addClass("hide")
			var  sever_id=$(this).attr('id')
			var data=[]
			for(var i=0;i<id.length;i++){
				if(id[i].Id == sever_id){
					data.push([id[i].PFID,id[i].PFName,id[i].AccountName,id[i].RoleId,id[i].RoleLevel,id[i].Payment,id[i].Name])
				}else if(sever_id==0){
					data[i]=[id[i].PFID,id[i].PFName,id[i].AccountName,id[i].RoleId,id[i].RoleLevel,id[i].Payment,id[i].Name]
				}
			}
			var cs = new table({
				"tableId":"player_old_table",    //必须 表格id
				"headers":["PFID","PFName","AccountName","角色ID","角色等级","充值金额","服务器Id"],   //必须 thead表头
				"data":data,         //必须 tbody 数据展示
				"displayNum": 20,    //必须   默认 10  每页显示行数
				"groupDataNum":1,     //可选    默认 10  组数
				"display_tfoot":true, // true/false  是否显示tfoot --- 默认false
				"bindContentTr":function(){ //可选 给tbody 每行绑定事件回调
				this.tableObj.find("tbody").on("click",'tr',function(e){
					return false;
					var tr_index = $(this).data("tr_index");        // tr行号  从0开始
					var data_index = $(this).data("data_index");   //数据行号  从0开始
					            })
					},
					sort:true,    // 点击表头是否排序 true/false  --- 默认false
					 search:true   // 默认为false 没有搜索
		});
			
	})	
	
})