$(function(){
	// 侧导航tab切换
	function tab(aa,bb){
		for(var i=0;i<aa.length;i++){
			aa.eq(i).click(function(){
				var index=$(this).index();
				$(this).addClass("active").siblings().removeClass("active");
				bb.eq(index).addClass("show").removeClass("hide").siblings().removeClass("show").addClass("hide")
				$(this).find("input[type='button']").addClass("bg").siblings().removeClass("bg")

			})
		}
	}
	// tab功能
	tab($(".ul li"),$(".tab>div"));
	
// 刘龙勤大区ajax复选框
	function checkbox(aa,bb){
		var arr_str=[];
		var sev_arr=[];
		aa.html("");
		$.ajax({
			type:"post",
			url:"../query_area_servers.php",
			dataType:"json",
			success:function(e){
				$('.legend').html("");
				for(var key in e){
					var len='<legend class="legend">'+key+'</legend>';
					var legend='<legend class="legend1">'+key+'</legend>';
					var tim=e[key];
					var lable="";
					var data="";
					for(var i=0; i<tim.length;i++){
						$(".order_input").val(tim[0].name);
						$(".pay_input").val(tim[0].name);
						var id=tim[i].id;
						lable+= '<li><input type="checkbox" name="servs"  value="'+id+'"   class="ui-checkboxradio ui-helper-hidden-accessible"/><span>'+tim[i].name+'</span></li>'
						data+= '<li><input type="radio" name="servs"  value="'+id+'"   class="ui-checkboxradio ui-helper-hidden-accessible"/><span>'+tim[i].name+'</span></li>'
						
						oli= '<li id="'+id+'"  class="option" style="cursor:pointer;list-style: none;height: 30px;width: 173px;margin-left: -35px;" >'+tim[i].name+'</li>'
						uli= '<li id="'+id+'"  class="pay_option " style="cursor:pointer;list-style: none;height: 30px;width: 173px;margin-left: -35px;" >'+tim[i].name+'</li>'
						pli= '<li id="'+id+'"  class="player_rss " style="cursor:pointer;list-style: none;height: 30px;width: 173px;margin-left: -35px;" >'+tim[i].name+'</li>'

						sev_arr.push({
                            id:id, name:tim[i].name, areaname:tim[i].areaname, capa:tim[i].capa,isonline:tim[i].isonline
                        })

						arr_str.push(tim[i].Channels);
						$("#sever_types").append(oli)//订单
						$("#pay_types").append(uli)//付费
						$("#player_severs").append(pli)//角色充值
					}
					var div='<div id="roll_notice_刘龙勤大区" role="toolbar" class="ui-controlgroup ui-controlgroup-horizontal ui-helper-clearfix leb "><div type="text" style="min-width:100px;height: 34px;cursor:pointer;border:1px solid #999" class="sevs_input"><p>All</p></div><ul class="sevs_types" name="roll_types" style="background:#eaff3d;border:1px solid #999;position:relative;z-index:999;" class="show">'+lable+'</ul></div>'
					var text='<div id="roll_notice_刘龙勤大区" role="toolbar" class="ui-controlgroup ui-controlgroup-horizontal ui-helper-clearfix leb "><div type="text" style="min-width:100px;height: 34px;cursor:pointer;border:1px solid #999" class="sevs_inputs"><p>All</p></div><ul class="sevs_type" name="roll_types" style="background:#fff;border:1px solid #999;position:relative;z-index:999;" class="show">'+data+'</ul></div>'
					
					
					aa.append(len,div)
					bb.append(legend,text)
					
				}
				//console.log(arr_str)
				var new_arr=[];
				for(var n=0;n<arr_str.length;n++) {
					for(var i in arr_str[n]){
						new_arr.push(arr_str[n][i])
					}
				}
				//console.log(new_arr)
				var channelId=[]
				for(var j=0;j<new_arr.length;j++){
                    if(channelId.indexOf(new_arr[j]) == -1){
                        channelId.push(new_arr[j])
                    }
                }
                //console.log(channelId)
				for(var m=0;m<channelId.length;m++){
					$("#order_id").append('<option value="" select="selected" title="'+channelId[m]+'">'+channelId[m]+'</option>')
				}


				//服务器
                function JsonSort(json,key){
                    //console.log(json);
                    for(var j=1,jl=json.length;j < jl;j++){
                        var temp = json[j],
                            val  = temp[key],
                            i    = j-1;
                        while(i >=0 && json[i][key]>val){
                            json[i+1] = json[i];
                            i = i-1;
                        }
                        json[i+1] = temp;

                    }
                    //console.log(json);
                    return json;
                }
                var json = JsonSort(sev_arr,'id');
				for(var i=0;i<json.length;i++){
					//console.log(json[i])
					var inline;
				   if(json[i].isonline==true){
					  inline="在线"
					 }else{
					   inline="不在线"
					 }
					sev_lable='<tr ><td>'+json[i].id+'</td><td>'+json[i].name+'</td> <td>'+json[i].areaname+'</td><td>'+json[i].capa+'</td><td>'+inline+'</td></tr>'
					$(".s_table").append(sev_lable)
                }
			}
			
		})
		
	}
	checkbox($(".fieldset"),$(".fieldset1"))
	
	
	//复选框
	$(document).on("click",".sevs_input",function(){
		var types=$(this).siblings($(".sevs_types"))
		if(types.css("display")=="block"){
			types.css("display","none")
		}else(
			types.css("display","block")
		)
		
	})
	$(document).on("click",".sevs_types li",function(){
		var check=$(this).find("input").prop("checked")

		if(check==true){
			$(this).find("input").prop("checked","")
			if($(".sevs_input").find("span").length==1){
				$(".sevs_input").find("span").remove()
				$(".sevs_input").html('<p>ALL</p>')
			}else{
				for(var i=0;i<$(".sevs_input").find("span").length;i++){
					var sp=$(".sevs_input").find("span").eq(i).html()
					if(sp == $(this).find("span").html()){
                        $(".sevs_input").find("span").eq(i).remove()
					}
				}
			}
		}else{
			$(this).parent().siblings(".sevs_input").find("p").remove()
			$(this).find("input").prop("checked","checked")
             $(this).parent().siblings(".sevs_input").append('<span>'+$(this).find("input").siblings("span").html()+'</span>')

		}
	})
	$(document).on("mouseover",".sevs_types",function(){
		$(this).show()
	})
	$(document).on("mouseout",".sevs_types",function(){
		$(this).hide()
	})
	
	//单选框
	$(document).on("click",".sevs_inputs",function(){
		var types=$(this).siblings($(".sevs_type"))
		if(types.css("display")=="block"){
			types.css("display","none")
		}else(
			types.css("display","block")
		)
	})
	$(document).on("click",".sevs_type li",function(){
		var check=$(this).find("input").prop("checked")
		if(check==true){
			$(this).find("input").prop("checked","")
			if($(".sevs_inputs").find("span").length==1){
				$(".sevs_inputs").find("span").remove()
				$(".sevs_inputs").html('<p>ALL</p>')
			}else{
				for(var i=0;i<$(".sevs_inputs").find("span").length;i++){
					if($(this).find("span").html()==$(".sevs_inputs").find("span").eq(i).html()){
						$(".sevs_inputs").find("span").eq(i).remove()
					}
				}
			}
		}else{
			$(this).parent().siblings(".sevs_inputs").find("p").remove()
			$(this).find("input").prop("checked","checked")
			$(this).parent().siblings(".sevs_inputs").html('<span>'+$(this).find("input").siblings("span").html()+'</span>')
            $(this).parent().parent().siblings("div").find(".sevs_inputs").html('<p>ALL</p>')
		}
	})
	$(document).on("mouseover",".sevs_type",function(){
		$(this).show()
	})
	$(document).on("mouseout",".sevs_type",function(){
		$(this).hide()
	})
	//订单页面选框
	//订单统计
	
	$(".order_input").click(function(){
		$("#sever_types").removeClass("hide").addClass("show")
	})
	$(document).on("click",".option",function(){
			var val=$("#order_id").find('option:checked').text()
			$(".order_input").val($(this).html())
			$(this).parent().removeClass("show").addClass("hide")
			var  id=$(this).attr('id')
			$.ajax({
				type:"post",
				url:"../order_inquiry.php",
				dataType:"json",
				data:{id:id,val:val},
				success:function(e){
				var data = [];
				for(var i=0;i<e.length;i++)
				{
					data[i] =[e[i].Game,e[i].Paytime,e[i].Pfid,e[i].Pfname,e[i].Orderid,e[i].Roleid,e[i].Accountid,e[i].Payment];
				}
				var cs = new table({
				 "tableId":"gs_table",    //必须 表格id
				"headers":["游戏名","支付时间","渠道id","渠道名","订单id","角色id","账户名","支付金额"],   //必须 thead表头
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
	//付费统计
	$(".pay_input").click(function(){
		$("#pay_types").removeClass("hide").addClass("show")
	})
	$(document).on("click",".pay_option",function(){
			$(".pay_input").val($(this).html())
			$(this).parent().removeClass("show").addClass("hide")
			var  id=$(this).attr('id')
			$.ajax({
				type:"post",
				url:"../pay_statistics.php",
				dataType:"json",
				data:{id:id},
				success:function(e){
				var data = [];
				for(var i=0;i<e.length;i++)
				{
					data[i] =[e[i].Paytime,e[i].Orderid,e[i].Roleid,e[i].Accountid,e[i].Payment];
				}
				var cs = new table({
				 "tableId":"fs_table",    //必须 表格id
				"headers":["支付时间","订单id","角色id","账户名","支付金额"],   //必须 thead表头
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
	
	
//	历史记录查询

//历史查询<导航>
	function navtab(aa){
		for(var i=0;i<aa.length;i++){
			aa.eq(i).click(function(){
				$(this).addClass("bg").siblings().removeClass("bg")
				$(this).find("input[type='button']").addClass("bg")
				$(this).siblings().find("input[type='button']").removeClass("bg")
			})
		}
	}
	navtab($(".nav_ul li"))
//	历史查询循环点击着个出现
	for(var i=0;i<$(".nav_ul li input[type='button']").length;i++){
		$(".nav_ul li input[type='button']").eq(i).click(function(){
			var val=$(this).val()
			var parent=$(this).parent()
			$.ajax({
			type:"post",
			url:"../history.php",
			dataType:"json",
			data:parent.serialize(),
			success:function(e){
				var arr=[]
				 var data = [];
				if(val=="历史系统公告查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Title,e[i].Content,e[i].Color,e[i].Time];

					}
					arr=["公告标题","公告内容","公告颜色","时间"]
				}
				if(val=="历史区服公告查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Title,e[i].Content,e[i].ContentColor,e[i].Time];
					}
					arr=["公告标题","公告内容","公告颜色","时间"]
				}
				if(val=="历史滚动公告查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].SendType,e[i].Content,e[i].Timestr,e[i].Time];
					}
					arr=["发送类型","公告内容","时间参数","时间"]
				}
				if(val=="历史发送邮件查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Title,e[i].Sender,e[i].Content,e[i].Recvers,e[i].Stritemids,e[i].Stritemsks,e[i].Lowlevel,e[i].Highlevel,e[i].Time0,e[i].Time1,e[i].SendType,e[i].Time];
					}
					arr=["标题","发件人","邮件内容","收件人","道具","堆叠","最低等级","最高等级","最早注册","最晚注册","类型","时间"]
				}
				if(val=="登录活动状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="累积充值状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="打折商店状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="单笔充值状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="热点伙伴状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="顶级招募状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="小额礼包状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="抽奖活动状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="积分商店状态查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].OpenTime,e[i].CloseTime,e[i].Status,e[i].Time];
					}
					arr=["活动名称","开始时间","结束时间","活动状态","时间"]
				}
				if(val=="模拟充值查询"){
					for(var i=0;i<e.length;i++){
						data[i] =[e[i].Name,e[i].ShopId,e[i].OrderId,e[i].Payment,e[i].RoleId,e[i].Time];
					}
					arr=["名称","商品Id","订单编号","充值金额","角色Id","时间"]
				}
				 var cs = new table({
				        "tableId":"ss_table",    //必须 表格id
				        "headers":arr,   //必须 thead表头
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
				},
				error:function(e){
				alert("执行错误")
				}
			})
		})
	}
	// 提交表单
	function pushback(a,urls){
			//alert(a.serialize())
			$.ajax({
			type:"post",
			url:urls,
			dataType:"json",
			data:a.serialize(),
			success:function(e){
				for(var i in e){
					if(e[i].error != 0){
						alert(e[i].desc)
					}else{
						alert("成功")
					}
				}
				
			}
		})
	}
	// 区域公告模快ajax
	$("#server_btn").click(function(){
		pushback($("#server"),"../change_server_notice.php")
	})
	// 系统公告模块ajax
	$("#system_btn").click(	function(){
			pushback($("#system"),"../change_system_notice.php")
			
		})
	// 滚动公告
	$(window).click(function(){
		if($("#types").val()=="NST_Immediately"){
			$('#form_datetime1').datetimepicker('remove');
			$('#form_datetime2').datetimepicker('remove');
			$(".roll_param").attr("disabled","disabled");
		}else if($("#types").val()=="NST_Timming"){
				$('#form_datetime2').datetimepicker('remove');
				$(".roll_param").attr("disabled","disabled");
				$('#form_datetime1').datetimepicker({
			        //language:  'fr',
			        weekStart: 1,
					todayBtn:  1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 2,
					forceParse: 0,
				    showMeridian: 1,
				  	format: "yyyy-MM-dd hh:ii"
			    });
				$('.form_date').datetimepicker({
			        language:  'fr',
			        weekStart: 1,
			        todayBtn:  1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 2,
					minView: 2,
					forceParse: 0
			    });
				$('.form_time').datetimepicker({
			        language:  'fr',
			        weekStart: 1,
			        todayBtn:  1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 1,
					minView: 0,
					maxView: 1,
					forceParse: 0
			    });
			}
			else if($("#types").val()=="NST_Loop"){
				$('#form_datetime1').datetimepicker('remove');
				$(".roll_param").removeAttr("disabled");
				$('#form_datetime2').datetimepicker({
			        //language:  'fr',
			        weekStart: 1,
					todayBtn:  1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 2,
					forceParse: 0,
				    showMeridian: 1,
				  	format: "yyyy-MM-dd hh:ii"
			    });
				$('.form_date').datetimepicker({
			        language:  'fr',
			        weekStart: 1,
			        todayBtn:  1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 2,
					minView: 2,
					forceParse: 0
			    });
				$('.form_time').datetimepicker({
			        language:  'fr',
			        weekStart: 1,
			        todayBtn:  1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 1,
					minView: 0,
					maxView: 1,
					forceParse: 0
			    });
			}
			
	})
	// 滚动公告ajax
	$("#roll_btn").click(function(){
		pushback($("#roll"),"../chang_roll_notice.php")
	})
	// 邮件发送ajax
	$("#mail_btn").click(function(){
			pushback($("#mail"),"../insert_mail.php")
		})
	// 角色操作ajax
	$("#player_btn").click(function(){
		//push($("#player"),"../contro_player.php")
		var param=$(".param").val();
		var  len=$(".param").val().length;
		if($("#player_type").val()=="GMCT_NoTalk"||$("#player_type").val()=="GMCT_AddMoney"||$("#player_type").val()=="GMCT_AddDiamond"||$("#player_type").val()=="GMCT_AddExp"){
			
			if(len>8){
				alert("参数长度超出范围")
			}else{
				$.ajax({
					type:"post",
					url:"../contro_player.php?param="+param,
					dataType:"json",
					data:$("#player").serialize(),
					success:function(e){
						if(e.error != 0){
						alert(e.desc)
						}else{
							alert("成功")
						}
					}
				})
			}
		}
		else{
			$.ajax({
				type:"post",
				url:"../contro_player.php?param="+param,
				dataType:"json",
				data:$("#player").serialize(),
				success:function(e){
					if(e.error != 0){
						alert(e.desc)
						}else{
							alert("成功")
					}
				}
			})
		}
	})
	// 登录活动ajax
	$("#login_btn").click(function(){

		pushback($("#activity"),"../login_activity.php")

         var txt = $('.login_table').find(':text');
         console.log(txt.eq(0).val())
         login_table=[]
         for(var i=0;i<txt.length;i++){
             login_table.push(txt.eq(i).val())

         }
         console.log(login_table)
        localStorage.setItem("login_table",login_table)//给你的第一个页面添加这句话，把type存进sessioStorage里面

	})
//	导入
        function addtab(id,tab,input){
            id.click(function(){
                    var num=localStorage.getItem(tab)
                    num=num.split(",")
                    console.log(num)
                     for (var i=0;i<input.length;i++){
                          input.eq(i).val("")
                         input.eq(i).val(num[i])
                     }
            })
        }
        addtab( $("#log_btn"),"login_table",$('.login_table input[type="text"]'))
        addtab( $("#sed_btn"),"accumulate_table",$('.accumulate_table input[type="text"]'))
        addtab( $("#third_btn"),"discount_table",$('.discount_table input[type="text"]'))
        addtab( $("#fourth_btn"),"single_table",$('.single_table input[type="text"]'))
        addtab( $("#fifth_btn"),"extract_table",$('.extract_table input[type="text"]'))
        addtab( $("#sixth_btn"),"zhuanpan_table",$('.zhuanpan_table input[type="text"]'))
        addtab( $("#seventh_btn"),"Integral_table",$('.Integral_table input[type="text"]'))
        addtab( $("#eight_btn"),"money_int",$('.money_int input[type="text"]'))
	//累计充值ajax
	$("#accumulate_btn").click(function(){
		pushback($("#accumulate_recharge"),"../accumulate_recharge.php")
		var txt = $('.accumulate_table').find(':text');
        console.log(txt.eq(0).val())
        accumulate_table=[]
         for(var i=0;i<txt.length;i++){
                accumulate_table.push(txt.eq(i).val())
        }
         console.log(accumulate_table)
         localStorage.setItem("accumulate_table",accumulate_table)//给你的第一个页面添加这句话，把type存进sessioStorage里面
	})
	//打折商店ajax
	$("#discount_btn").click(function(){
		pushback($("#discount_store"),"../discount_store.php")
		var txt = $('.discount_table').find(':text');
                console.log(txt.eq(0).val())
                discount_table=[]
                 for(var i=0;i<txt.length;i++){
                        discount_table.push(txt.eq(i).val())
                }
                 console.log(discount_table)
                 localStorage.setItem("discount_table",discount_table)//给你的第一个页面添加这句话，把type存进sessioStorage里面

	})
	//单笔充值ajax
	$("#single_btn").click(function(){
		pushback($("#single_recharge"),"../single_recharge.php")
		var txt = $('.single_table').find(':text');
                console.log(txt.eq(0).val())
                single_table=[]
                 for(var i=0;i<txt.length;i++){
                        single_table.push(txt.eq(i).val())
                }
                 console.log(single_table)
                 localStorage.setItem("single_table",single_table)//给你的第一个页面添加这句话，把type存进sessioStorage里面
	})
	//热点伙伴ajax
	$("#hotspot_btn").click(function(){
		pushback($("#hotspot_partner"),"../hotspot_partner.php")
	})
	//顶级招募伙伴ajax
	$("#extract_btn").click(function(){
		pushback($("#extract_partner"),"../extract_partner.php")
		var txt = $('.extract_table').find(':text');
             console.log(txt.eq(0).val())
            extract_table=[]
             for(var i=0;i<txt.length;i++){
                  extract_table.push(txt.eq(i).val())
             }
        console.log(extract_table)
        localStorage.setItem("extract_table",extract_table)//给你的第一个页面添加这句话，把type存进sessioStorage里面
	})
	//小额礼包
	$("#money_btn").click(function(){
		pushback($("#Money_partner"),"../money.php")
        var txt = $('.money_int').find(':text');
                     console.log(txt.eq(0).val())
                    money_int=[]
                     for(var i=0;i<txt.length;i++){
                          money_int.push(txt.eq(i).val())
                     }
                console.log(money_int)
                localStorage.setItem("money_int",money_int)//给你的第一个页面添加这句话，把type存进sessioStorage里面
	})
	//抽奖
	$("#cj_btn").click(function(){
		pushback($("#zhuanpan"),"../do_turntable.php")
		var txt = $('.zhuanpan_table').find(':text');
                             console.log(txt.eq(0).val())
                            zhuanpan_table=[]
                             for(var i=0;i<txt.length;i++){
                                  zhuanpan_table.push(txt.eq(i).val())
                             }
                        console.log(zhuanpan_table)
                        localStorage.setItem("zhuanpan_table",zhuanpan_table)//给你的第一个页面添加这句话，把type存进sessioStorage里面
	})
	//积分商城
	$("#shop_btn").click(function(){
		pushback($("#Integral_store"),"../do_integral.php")
		var txt = $('.Integral_table').find(':text');
                                     console.log(txt.eq(0).val())
                                    Integral_table=[]
                                     for(var i=0;i<txt.length;i++){
                                          Integral_table.push(txt.eq(i).val())
                                     }
                                console.log(Integral_table)
                                localStorage.setItem("Integral_table",Integral_table)//给你的第一个页面添加这句话，把type存进sessioStorage里面
	})
	//九游礼包
	$("#nine_btn").click(function(){
		//pushback($("#Nine_partner"),"../queryNineMoney")
		$.ajax({
			type:"post",
			url:"../queryNineMoney",
			dataType:"json",
			data:$("#Nine_partner").serialize(),
			success:function(e){
				alert("ok")
			},error:function(){
				alert("失败")
			}
		})
	})
	//渠道映射
	$("#Channel_btn").click(function(){
		//pushback($("#Channel_mapping"),"../do_channelsmapping.php")
		$.ajax({
			type:"post",
			url:"../do_channelsmapping.php",
			data:$("#Channel_mapping").serialize(),
			success:function(e){
					alert(e)
			}
		})
	})
	// 游服操作提交表单
	function push(a,urls){
			//alert(a.serialize())
			$.ajax({
			type:"post",
			url:urls,
			dataType:"json",
			data:a.serialize(),
			success:function(e){
				if(e.error != 0){
						alert(e.desc)
				}else{
						alert("成功")
				}
			}
		})
	}
	//模拟
	// 模拟充值模块ajax
	$("#mn_but").click(	function(){
			push($("#simulator"),"../do_simulator.php")
			
		})
	// 执行命令模块ajax
	$("#zx_but").click(	function(){
			push($("#execute"),"../do_execute.php")
		})
		

//	CDK生成
		
	    var time = 3000;//每次点击三秒后才能再次点击
	    var fun = function bClick(){
	        //pushback($("#cdkey_generate"),"../cdkey_generate.php")
			$.ajax({
				type:"post",
				url:"../cdkey_generate.php",
				data:$("#cdkey_generate").serialize(),
				success:function(e){
					alert(e)
				}
			})     
	        $("#w_btn").unbind("click");
	        setTimeout(function(){
				
	            $("#w_btn").click(fun);
	        },time);
	    };
		$("#w_btn").click(fun)
	
//	CDK查询
	$("#q_btn").click(function(){
		$.ajax({
			type:"post",
			url:"../cdkey_select.php",
			dataType:"json",
			data:$("#cdkey_select").serialize(),
			success:function(content){
						var data = [];
					   	for(var key in content){
							var text=content[key]
							for(var i=0;i<text.length;i++){
								 data[i] =[text[i].CDKey,text[i].UsedTime,text[i].PlayerName];
							}
						}
					    var cs = new table({
					        "tableId":"example",    //必须 表格id
					        "headers":["激活码","时间","礼包用户"],   //必须 thead表头
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

	//	CDK删除
	$("#delete_btn").click(function(){
		//pushback($("#cdkey_delete"),"../cdkey_delete.php")
		$.ajax({
				type:"post",
				url:"../cdkey_delete.php",
				
				data:$("#cdkey_delete").serialize(),
				success:function(e){
					alert(e)
				}
			})    
	})
//	聊天查询(带分页）
	//聊天在线查询ajax
	$("#now_btn").click(function(){
		$.ajax({
			type:"post",
			url:"../query_now_chat.php",
			dataType:"json",
			data:$("#query_now_chat").serialize(),
			success:function(e){
				 var data = [];
				    for(var i=0;i<e.length;i++)
				    {
				        data[i] =[e[i].SortId,e[i].Timestamp,e[i].PlayerGuid,e[i].PlayerName,e[i].ChannelId,e[i].Content];
					}
				    var cs = new table({
				        "tableId":"cs_table",    //必须 表格id
				        "headers":["序列id","时间","PlayerId","玩家名字","频道","内容"],   //必须 thead表头
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
				},
			error:function(e){
				alert("执行错误")
			}
		})
	})
	
	//聊天历史查询ajax
	$("#history_btn").click(function(){
		$.ajax({
			type:"post",
			url:"../query_history_chat.php",
			dataType:"json",
			data:$("#history_chat").serialize(),
			success:function(e){

				 var data = [];
				    for(var i=0;i<e.length;i++)
				    {
				        data[i] =[e[i].SortId,e[i].Timestamp,e[i].PlayerGuid,e[i].PlayerName,e[i].ChannelId,e[i].Content];
					}
				    var cs = new table({
				        "tableId":"ds_table",    //必须 表格id
				        "headers":["序列id","时间","id","玩家名字","频道","内容"],   //必须 thead表头
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
				},
				error:function(e){
					alert("执行错误")
				}
		})
	})
	//角色查询ajax
	$("#role_btn").click(function(){
		$.ajax({
			type:"post",
			url:"../query_player.php",
			dataType:"html",
			data:$("#query_player").serialize(),
			success:function(e){
				$(".w_text").html(e)
			},
			error:function(e){
				alert("错误")
			}
		})
	})
	//CDKEY导出
	$("#cdkey_export").click(function(){
		var input=$(".cdkey_select").val()
		window.location.href="../gift/"+input+".csv";
	})
	//ORDER导出
	$("#orders_button").click(function(){
		$(".orders_table").tableExport({formats:["xlsx"]});
	})
	$("#pay_button").click(function(){
		$(".pay_table").tableExport({formats:["xlsx"]});
	})

	
	//管理员
	$("#Admin_btn").click(function(){
		window.location.href="../home/regist.html"

	})

	//运营活动删除
	function remove(aa,bb){
		$(aa).click(function(){
			for(var i=0;i<$(".w_check").length;i++){
				if($(".w_check").eq(i).prop("checked")==true){
					var len=$("input[type=checkbox]:checked()")
					$(len).parent().parent().remove()
				}
			}
			var tr=bb.find("tr")
			
			if(tr.length==1){
				$(".w_add").css({"display":"block","float":"left",'marginRight':"5px"})
			}
			else{
				$(".w_add").css({"display":"none"})
			}
			
			
		})
	}
	//登录
	remove($('#activity-1 .w_remove'),$('#activity-1 .btable tbody'))
	//累积充值
	remove($('#activity-2 .w_remove'),$('#activity-2 .btable tbody'))
	//打折商店
	remove($('#activity-3 .w_remove'),$('#activity-3 .btable tbody'))
	//单笔充值
	remove($('#activity-4 .w_remove'),$('#activity-4 .btable tbody'))
	//顶级招募
	remove($('#activity-6 .w_remove'),$('#activity-6 .btable tbody'))
	//抽奖
	remove($('#activity-8 .w_remove'),$('#activity-8 .btable tbody'))
	//积分商店
	remove($('#activity-9 .w_remove'),$('#activity-9 .btable tbody'))
	
	
	
	function QueryDia(bb,url,box){
		$.ajax({
			type:"post",
			url:url,
			data:bb.serialize(),
			success:function(data){
				var content=JSON.parse(data)
				if(content.error==0){
					var e=JSON.parse(content.desc)
					var data = [];
				for(var i=0;i<e.length;i++)
				{
					data[i] =[e[i].id,e[i].name,e[i].value];
				}
				var cs = new table({
				 "tableId":box,    //必须 表格id
				"headers":["人物ID","人物名","数量"],   //必须 thead表头
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
				}else{
					alert("错误")
				}
			}
		})
	}
	
	$("#QueryDia_btn").click(function(){
			QueryDia($("#QueryDia"),"../queryDia","QueryDia_table")
	})
	$("#QueryRMB_btn").click(function(){
		QueryDia($("#QueryRMB"),"../queryRMB","QueryRMB_table")
	})
	$("#QueryMoney_btn").click(function(){
		QueryDia($("#QueryMoney"),"../queryMoney","QueryMoney_table")
	})
	
	
})


