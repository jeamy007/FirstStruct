<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Master/MasterManage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="AppleView.Manage.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    首页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadPart" runat="server">
    <script>var app_menu = "menu_home";</script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
 <div class="row-fluid">

						<div class="span3 responsive" data-tablet="span6" data-desktop="span3">
							<div class="dashboard-stat blue">
								<div class="visual">
									<i class="icon-user"></i>
								</div>
								<div class="details">
									<div class="number"><%=_userCount%>
									</div>
									<div class="desc">用户数量   
									</div>
								</div>
								<a class="more" href="SystemManage/AddUser.aspx">
								查看详情<i class="m-icon-swapright m-icon-white"></i>
								</a>
							</div>
						</div>

						<div class="span3 responsive" data-tablet="span6" data-desktop="span3">
							<div class="dashboard-stat green">
								<div class="visual">
									<i class="icon-group"></i>
								</div>
								<div class="details">
									<div class="number"><%=_companys%></div>
									<div class="desc">合作单位</div>
								</div>
								<a class="more" href="SystemManage/UnitManager.aspx">
								查看详情<i class="m-icon-swapright m-icon-white"></i>
								</a>
							</div>
						</div>

						<div class="span3 responsive" data-tablet="span6  fix-offset" data-desktop="span3">
							<div class="dashboard-stat purple">
								<div class="visual">
									<i class="icon-globe"></i>
								</div>
								<div class="details">
									<div class="number"><%=_YSProject%></div>
									<div class="desc">预算审计项目</div>
								</div>
								<a class="more" href="#">
								查看详情<i class="m-icon-swapright m-icon-white"></i>
								</a>
							</div>
						</div>

						<div class="span3 responsive" data-tablet="span6" data-desktop="span3">
							<div class="dashboard-stat yellow">
								<div class="visual">
									<i class="icon-bar-chart"></i>
								</div>
								<div class="details">
									<div class="number"><%=_JSProject%></div>
									<div class="desc">结算审计项目</div>
								</div>
								<a class="more" href="#">
								查看详情<i class="m-icon-swapright m-icon-white"></i>
								</a>
							</div>
						</div>
</div>
    <div class="row-fluid">
        <div class="span6">
            <div class ="portlet solid bordered light-grey">
                <div class="portlet-title"><div class="caption"><i class="icon-bar-chart"></i>每月审计项目金额对比图</div></div>
                <div class="portlet-body" id="container">
                </div>
            </div>            
        </div>
          <div class="span6">
            <div class ="portlet solid bordered light-grey">
              
                <div class="portlet-title"><div class="caption"><i class="icon-bar-chart"></i>图表</div></div>
                <div class="portlet-body" id="container1">
                      <%--<iframe name="weather_inc" src="http://i.tianqi.com/index.php?c=code&id=64" frameborder="0" marginwidth="0" marginheight="0" scrolling="no"></iframe>--%>
                </div> 
            </div>            
        </div>
       <div class="clearfix"></div>
    </div>
  


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyBottom" runat="server">
<script src="https://code.highcharts.com/highcharts.js"></script>
<script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#container').highcharts({
                title: {
                    text: '每月审计项目金额对比图',
                    x: -20 //center
                },
                subtitle: {
                    text: '对比对象: 预算审计和结算审计',
                    x: -20
                },
                xAxis: {
                    categories: ['一月', '二月', '三月', '四月', '五月', '六月',
                        '七月', '八月', '九月', '十月', '十一月', '十二月']
                },
                yAxis: {
                    title: {
                        text: '单位 (元)'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: '元'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: '结算审计送审金额',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }, {
                    name: '预算审计送审金额',
                    data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
                }]
            });


            $.getJSON('https://www.highcharts.com/samples/data/jsonp.php?filename=usdeur.json&callback=?', function (data) {

                $('#container1').highcharts({
                    chart: {
                        zoomType: 'x'
                    },
                    title: {
                        text: 'USD to EUR exchange rate over time'
                    },
                    subtitle: {
                        text: document.ontouchstart === undefined ?
                                'Click and drag in the plot area to zoom in' : 'Pinch the chart to zoom in'
                    },
                    xAxis: {
                        type: 'datetime'
                    },
                    yAxis: {
                        title: {
                            text: 'Exchange rate'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        area: {
                            fillColor: {
                                linearGradient: {
                                    x1: 0,
                                    y1: 0,
                                    x2: 0,
                                    y2: 1
                                },
                                stops: [
                                    [0, Highcharts.getOptions().colors[0]],
                                    [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                                ]
                            },
                            marker: {
                                radius: 2
                            },
                            lineWidth: 1,
                            states: {
                                hover: {
                                    lineWidth: 1
                                }
                            },
                            threshold: null
                        }
                    },

                    series: [{
                        type: 'area',
                        name: 'USD to EUR',
                        data: data
                    }]
                });
            });
        });
    </script>
</asp:Content>
