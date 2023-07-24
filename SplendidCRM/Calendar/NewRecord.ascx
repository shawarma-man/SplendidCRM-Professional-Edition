<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="SplendidCRM.Calendar.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divNewRecord">
	<%@ Register TagPrefix="SplendidCRM" Tagname="HeaderLeft" Src="~/_controls/HeaderLeft.ascx" %>
	<SplendidCRM:HeaderLeft ID="ctlHeaderLeft" Title="Calendar.LNK_NEW_APPOINTMENT" Runat="Server" />

	<asp:Panel Width="100%" CssClass="leftColumnModuleS3" runat="server">
		<asp:RadioButton ID="radScheduleCall"    GroupName="grpSchedule" class="radio" Checked="true" Runat="server" /><asp:Label Text='<%# L10n.Term("Calls.LNK_NEW_CALL"   ) %>' runat="server" /><br />
		<asp:RadioButton ID="radScheduleMeeting" GroupName="grpSchedule" class="radio"                Runat="server" /><asp:Label Text='<%# L10n.Term("Calls.LNK_NEW_MEETING") %>' runat="server" /><br />
		<asp:Label Text='<%# L10n.Term("Meetings.LBL_SUBJECT") %>' runat="server" /><asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /><br />
		<asp:TextBox ID="txtNAME" size="25" MaxLength="255" Runat="server" /><br />
		<asp:Label Text='<%# L10n.Term("Meetings.LBL_DATE") %>' runat="server" />&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" />&nbsp;<asp:Label ID="lblDATEFORMAT" CssClass="dateFormat" Runat="server" /><br />
		<%@ Register TagPrefix="SplendidCRM" Tagname="DatePicker" Src="~/_controls/DatePicker.ascx" %>
		<SplendidCRM:DatePicker ID="ctlDATE_START" Runat="Server" />
		<asp:Label Text='<%# L10n.Term("Meetings.LBL_TIME") %>' runat="server" />&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" />&nbsp;<asp:Label ID="lblTIMEFORMAT" CssClass="dateFormat" Runat="server" /><br />
		<asp:TextBox ID="txtTIME_START" size="15" MaxLength="10" Runat="server" /><br />
		
		<asp:Button ID="btnSave" CommandName="NewRecord" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) + "  " %>' ToolTip='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE") %>' AccessKey='<%# L10n.AccessKey(".LBL_SAVE_BUTTON_KEY") %>' Runat="server" /><br />
		<asp:RequiredFieldValidator ID="reqNAME"       ControlToValidate="txtNAME"       ErrorMessage="(required)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
		<asp:RequiredFieldValidator ID="reqTIME_START" ControlToValidate="txtTIME_START" ErrorMessage="(required)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
	<%= Utils.RegisterEnterKeyPress(txtNAME.ClientID          , btnSave.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(ctlDATE_START.DateClientID, btnSave.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtTIME_START.ClientID    , btnSave.ClientID) %>
</div>

