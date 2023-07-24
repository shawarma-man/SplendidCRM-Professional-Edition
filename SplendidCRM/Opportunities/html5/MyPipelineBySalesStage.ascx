<%@ Control CodeBehind="MyPipelineBySalesStage.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Opportunities.html5.MyPipelineBySalesStage" %>
<script runat="server">
/**********************************************************************************************************************
 * SplendidCRM is a Customer Relationship Management program created by SplendidCRM Software, Inc. 
 * Copyright (C) 2005-2023 SplendidCRM Software, Inc. All rights reserved.
 *
 * Any use of the contents of this file are subject to the SplendidCRM Professional Source Code License 
 * Agreement, or other written agreement between you and SplendidCRM ("License"). By installing or 
 * using this file, you have unconditionally agreed to the terms and conditions of the License, 
 * including but not limited to restrictions on the number of users therein, and you may not use this 
 * file except in compliance with the License. 
 * 
 * SplendidCRM owns all proprietary rights, including all copyrights, patents, trade secrets, and 
 * trademarks, in and to the contents of this file.  You will not link to or in any way combine the 
 * contents of this file or any derivatives with any Open Source Code in any manner that would require 
 * the contents of this file to be made available to any third party. 
 * 
 * IN NO EVENT SHALL SPLENDIDCRM BE RESPONSIBLE FOR ANY DAMAGES OF ANY KIND, INCLUDING ANY DIRECT, 
 * SPECIAL, PUNITIVE, INDIRECT, INCIDENTAL OR CONSEQUENTIAL DAMAGES.  Other limitations of liability 
 * and disclaimers set forth in the License. 
 * 
 *********************************************************************************************************************/
</script>
<div id="divOpportunitiesMyPipeline">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ChartDatePicker" Src="~/_controls/ChartDatePicker.ascx" %>
	<%@ Register TagPrefix="SplendidCRM" Tagname="DashletHeader" Src="~/_controls/DashletHeader.ascx" %>
	<SplendidCRM:DashletHeader ID="ctlDashletHeader" Title="Home.LBL_PIPELINE_FORM_TITLE" DivEditName="my_pipeline_edit_html5" Runat="Server" />
	<p></p>
	<SplendidCRM:DatePickerValidator ID="valDATE_START" ControlToValidate="ctlDATE_START" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Display="Dynamic" Runat="server" />
	<SplendidCRM:DatePickerValidator ID="valDATE_END"   ControlToValidate="ctlDATE_END"   CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Display="Dynamic" Runat="server" />
	<div ID="my_pipeline_edit_html5" style="DISPLAY: <%= bShowEditDialog ? "inline" : "none" %>">
		<asp:Table BorderWidth="0" CellPadding="0" CellSpacing="0" CssClass="chartForm" HorizontalAlign="Center" runat="server">
			<asp:TableRow>
				<asp:TableCell VerticalAlign="top" Wrap="false">
					<b><%# L10n.Term("Dashboard.LBL_DATE_START") %> </b><br />
					<asp:Label CssClass="dateFormat" Text='<%# System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() %>' runat="server" />
				</asp:TableCell>
				<asp:TableCell VerticalAlign="top">
					<SplendidCRM:ChartDatePicker ID="ctlDATE_START" Runat="Server" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell VerticalAlign="top" Wrap="false">
					<b><%# L10n.Term("Dashboard.LBL_DATE_END") %></b><br />
					<asp:Label CssClass="dateFormat" Text='<%# System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() %>' runat="server" />
				</asp:TableCell>
				<asp:TableCell VerticalAlign="top">
					<SplendidCRM:ChartDatePicker ID="ctlDATE_END" Runat="Server" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell VerticalAlign="top" Wrap="false"><b><%# L10n.Term("Dashboard.LBL_SALES_STAGES") %></b></asp:TableCell>
				<asp:TableCell VerticalAlign="top">
					<asp:ListBox ID="lstSALES_STAGE" DataValueField="NAME" DataTextField="DISPLAY_NAME" SelectionMode="Multiple" Rows="3" Runat="server" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell HorizontalAlign="Right" ColumnSpan="2">
					<asp:Button ID="btnSubmit" CommandName="Submit" OnCommand="Page_Command"                    CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SELECT_BUTTON_LABEL") + "  " %>' ToolTip='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' AccessKey='<%# L10n.AccessKey(".LBL_SELECT_BUTTON_KEY") %>' runat="server" />
					<asp:Button ID="btnEdit"   UseSubmitBehavior="false" OnClientClick="toggleDisplay('my_pipeline_edit_html5'); return false;" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_CANCEL_BUTTON_LABEL") + "  " %>' ToolTip='<%# L10n.Term(".LBL_CANCEL_BUTTON_TITLE") %>' AccessKey='<%# L10n.AccessKey(".LBL_CANCEL_BUTTON_KEY") %>' runat="server" />
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
	</div>
	<p></p>
	<div align="center">
		<asp:HiddenField ID="hidSERIES_DATA" Value="{}" runat="server" />
		<asp:HiddenField ID="hidACTIVE_USERS" runat="server" />
		<asp:HiddenField ID="hidPIPELINE_TOTAL" runat="server" />
		<%@ Register TagPrefix="SplendidCRM" Tagname="FormatDateJavaScript" Src="~/_controls/FormatDateJavaScript.ascx" %>
		<SplendidCRM:FormatDateJavaScript ID="FormatDateJavaScript1" Runat="Server" />

		<SplendidCRM:InlineScript runat="server">
		<script type="text/javascript">
		$(document).ready(function()
		{
			var arrSalesStage = new Array();
			var lstSALES_STAGE = document.getElementById('<%# lstSALES_STAGE.ClientID %>');
			for ( var i = 0; i < lstSALES_STAGE.options.length; i++ )
			{
				if ( lstSALES_STAGE.options[i].selected )
					arrSalesStage.unshift(lstSALES_STAGE.options[i].text);
			}

			var sCurrencyPrefix = '<%# GetCurrencyPrefix() %>';
			var sCurrencySuffix = '<%# GetCurrencySuffix() %>';
			var data    = $.parseJSON(document.getElementById('<%# hidSERIES_DATA.ClientID %>').value);
			var users   = $.parseJSON(document.getElementById('<%# hidACTIVE_USERS.ClientID %>').value);
			var arrSeriesUsers = new Array();
			if ( $.isArray(users) )
			{
				for ( var i = 0; i < users.length; i++ )
				{
					var user = new Object();
					user.label = users[i];
					arrSeriesUsers.push(user);
				}
			}

			var options = 
			{ stackSeries: true
			, width: 600
			, height: 600
			, title: 
				{ show: true
				}
			, cursor: 
				{ show: true
				, zoom: true
				}
			, seriesDefaults: 
				{ renderer: $.jqplot.BarRenderer
				, rendererOptions: 
					{ barDirection: 'horizontal'
					, fillToZero: true
					}
				}
			, series: arrSeriesUsers
			, axes: 
				{ yaxis: 
					{ show: true
					, tickRenderer: $.jqplot.CanvasAxisTickRenderer
					, label: ''
					, renderer: $.jqplot.CategoryAxisRenderer
					, ticks: arrSalesStage
					}
				, xaxis: 
					{ show: false
					, label: ''
					, tickOptions: 
						{ formatString: '%.1f'
						, prefix: sCurrencyPrefix
						, suffix: sCurrencySuffix
						}
					}
				}
			};
	
			try
			{
				var sStartDate = document.getElementById('<%# ctlDATE_START.DateClientID %>').value;
				var sEndDate   = document.getElementById('<%# ctlDATE_END.DateClientID   %>').value;
				options.axes.xaxis.label  = '<%# Sql.EscapeJavaScript(L10n.Term("Dashboard.LBL_DATE_RANGE")) %>';
				options.axes.xaxis.label += ' ' + sStartDate + ' ';
				options.axes.xaxis.label += '<%# Sql.EscapeJavaScript(L10n.Term("Dashboard.LBL_DATE_RANGE_TO")) %>';
				options.axes.xaxis.label += ' ' + sEndDate + ' ';
				options.axes.xaxis.label += '<br/><%# Sql.EscapeJavaScript(L10n.Term("Dashboard.LBL_OPP_SIZE") + " " + 1.ToString("c0") + L10n.Term("Dashboard.LBL_OPP_THOUSANDS")) %>';
				
				options.title.text  = '<%# Sql.EscapeJavaScript(L10n.Term("Dashboard.LBL_TOTAL_PIPELINE")) %>';
				options.title.text += ' ' + document.getElementById('<%# hidPIPELINE_TOTAL.ClientID %>').value;
				options.title.text += '<%# Sql.EscapeJavaScript(L10n.Term("Dashboard.LBL_OPP_THOUSANDS")) %>';
				var plot1 = $.jqplot('html5MyPipelineBySalesStage', data, options);
				
				$('#html5MyPipelineBySalesStage').bind('jqplotDataClick', function(ev, seriesIndex, pointIndex, data)
				{
					var sSALES_STAGE        = '';
					var sASSIGNED_USER_ID   = '<%# Security.USER_ID %>';
					var lstSALES_STAGE      = document.getElementById('<%# lstSALES_STAGE.ClientID      %>');
					for ( var i = 0; i < lstSALES_STAGE.options.length; i++ )
					{
						if ( lstSALES_STAGE.options[i].text == arrSalesStage[pointIndex] )
						{
							sSALES_STAGE = lstSALES_STAGE.options[i].value;
						}
					}
					window.location.href = sREMOTE_SERVER + 'Opportunities/default.aspx?SALES_STAGE=' + escape(sSALES_STAGE) + '&ASSIGNED_USER_ID=' + escape(sASSIGNED_USER_ID);
				});
				$("#html5MyPipelineBySalesStage").bind('jqplotDataHighlight', function(ev, seriesIndex, pointIndex, data)
				{
					var $this = $(this);
					var sValue = options.axes.xaxis.tickOptions.prefix + $.jqplot.DefaultTickFormatter(options.axes.xaxis.tickOptions.formatString, data[0]) + options.axes.xaxis.tickOptions.suffix;
					$this.attr('title', sValue + '\n' + options.series[seriesIndex].label + '\n' + arrSalesStage[pointIndex]);
				}); 
				$("#html5MyPipelineBySalesStage").bind('jqplotDataUnhighlight', function(ev, seriesIndex, pointIndex, data)
				{
					var $this = $(this);
					$this.attr('title', '');
				});
			}
			catch(e)
			{
				var divChartError = document.getElementById('divChartError_MyPipelineBySalesStage');
				divChartError.innerHTML = 'Chart error: ' + e.message;
			}
		});
		</script>
		</SplendidCRM:InlineScript>
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
		<div id="divChartError_MyPipelineBySalesStage" class="error"></div>
		<div id="html5MyPipelineBySalesStage" style="width: 500px; height: 400px; margin-top:20px; margin-left:20px;"></div>
	</div>
	<span class="chartFootnote">
		<p align="center"><%# L10n.Term("Dashboard.LBL_PIPELINE_FORM_TITLE_DESC") %></p>
		<p aligh="right"><i><%# L10n.Term("Dashboard.LBL_CREATED_ON") + T10n.FromServerTime(DateTime.Now).ToString() %></i></p>
	</span>
</div>

