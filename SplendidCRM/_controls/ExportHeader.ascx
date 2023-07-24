<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ExportHeader.ascx.cs" Inherits="SplendidCRM._controls.ExportHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<SplendidCRM:InlineScript runat="server">
<script type="text/javascript">
function OAuthTokenUpdate(code)
{
	var AUTHORIZATION_CODE = document.getElementById('<%= AUTHORIZATION_CODE.ClientID %>');
	AUTHORIZATION_CODE.value = code;
	document.getElementById('<%= btnPhoneBurnerAuthorized.ClientID %>').click();
}

function PhoneBurnerAuthorize()
{
	var OAUTH_CLIENT_ID = '<%= Application["CONFIG.PhoneBurner.ClientID"] %>';
	var REDIRECT_URL    = '<%= Request.Url.Scheme + "://" + Request.Url.Host + Sql.ToString(Application["rootURL"]) + "Administration/PhoneBurner/OAuthLanding.aspx" %>';
	// 08/16/2020 Paul.  Update PhoneBurner OAuth. 
	// https://www.phoneburner.com/developer/authentication
	// Optional. If you are acting on behalf of a vendor account, then you must include and set to vendor.
	var authenticateUrl = 'https://www.phoneburner.com/oauth/index?client_id=' + OAUTH_CLIENT_ID + '&redirect_uri=' + REDIRECT_URL + '&response_type=code'; // + '&owner_type=';
	window.open(authenticateUrl, 'PhoneBurnerPopup', 'width=830,height=830,status=1,toolbar=0,location=0,resizable=1');
	return false;
}
</script>
</SplendidCRM:InlineScript>
	<div style="display: none;">
		<asp:TextBox ID="AUTHORIZATION_CODE" style="display: none;" runat="server" />
		<asp:Button  ID="btnPhoneBurnerAuthorized" CommandName="PhoneBurner.Authorize" OnCommand="Page_Command" Text="PhoneBurner Authorized" style="display: none;" runat="server" />
	</div>
	<asp:Table SkinID="tabFrame" CssClass="h3Row" runat="server">
		<asp:TableRow>
			<asp:TableCell Wrap="false">
				<h3><asp:Image SkinID="h3Arrow" Runat="server" />&nbsp;<asp:Label Text='<%# L10n.Term(sTitle) %>' runat="server" /></h3>
			</asp:TableCell>
			<asp:TableCell HorizontalAlign="Right">
				<div id="divExport" Visible='<%# !IsMobile && !PrintView %>' Runat="server">
					<asp:Label ID="lblPhoneBurnerAuthorizedStatus" CssClass="error" runat="server" />
					<asp:Button       ID="btnPhoneBurnerDialSession" Visible='<%# (Sql.ToBoolean(Application["CONFIG.PhoneBurner.Enabled"]) && Module == Sql.ToString(Application["CONFIG.PhoneBurner.SyncModules"])) && !Sql.IsEmptyString(Application["CONFIG.PhoneBurner." + Security.USER_ID.ToString() + ".OAuthAccessToken"]) %>' CommandName="PhoneBurner.BeginDialSession" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term("PhoneBurner.LBL_BEGIN_DIAL_SESSION"    ) %>' Runat="server" />
					<asp:Button       ID="btnPhoneBurnerAuthorize"   Visible='<%# (Sql.ToBoolean(Application["CONFIG.PhoneBurner.Enabled"]) && Module == Sql.ToString(Application["CONFIG.PhoneBurner.SyncModules"])) &&  Sql.IsEmptyString(Application["CONFIG.PhoneBurner." + Security.USER_ID.ToString() + ".OAuthAccessToken"]) %>' OnClientClick="return PhoneBurnerAuthorize();"                      CssClass="button" Text='<%# L10n.Term("PhoneBurner.LBL_AUTHORIZE_BUTTON_LABEL") %>' Runat="server" />

					<asp:DropDownList ID="lstEXPORT_RANGE"  Visible='<%# (Sql.ToBoolean(Application["CONFIG.PhoneBurner.Enabled"]) && Module == Sql.ToString(Application["CONFIG.PhoneBurner.SyncModules"])) || SplendidCRM.Security.GetUserAccess(sModule, "export") >= 0 %>' Runat="server" />
					<asp:DropDownList ID="lstEXPORT_FORMAT" Visible='<%# SplendidCRM.Security.GetUserAccess(sModule, "export") >= 0 %>' Runat="server" />
					<asp:Button       ID="btnExport"        Visible='<%# SplendidCRM.Security.GetUserAccess(sModule, "export") >= 0 %>' CommandName="Export" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_EXPORT_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_EXPORT_BUTTON_TITLE") %>' Runat="server" />
				</div>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>

