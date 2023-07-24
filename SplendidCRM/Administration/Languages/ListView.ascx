<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Administration.Languages.ListView" %>
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
<div id="divListView">
	<%-- 05/31/2015 Paul.  Combine ModuleHeader and DynamicButtons. --%>
	<%@ Register TagPrefix="SplendidCRM" Tagname="HeaderButtons" Src="~/_controls/HeaderButtons.ascx" %>
	<SplendidCRM:HeaderButtons ID="ctlModuleHeader" Module="Administration" Title="Administration.LBL_MANAGE_LANGUAGES" EnablePrint="true" HelpName="index" EnableHelp="true" Runat="Server" />
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>

	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Button ID="btnAdd"    UseSubmitBehavior="false" OnClientClick="window.location.href='edit.aspx'; return false;"       CssClass="button" Text='<%# "  " + L10n.Term(".LBL_ADD_BUTTON_LABEL"   ) + "  " %>' ToolTip='<%# L10n.Term(".LBL_ADD_BUTTON_TITLE"   ) %>' Runat="server" />
		<asp:Button ID="btnCancel" UseSubmitBehavior="false" OnClientClick="window.location.href='../default.aspx'; return false;" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_CANCEL_BUTTON_LABEL") + "  " %>' ToolTip='<%# L10n.Term(".LBL_CANCEL_BUTTON_TITLE") %>' Runat="server" />
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
	
	<SplendidCRM:SplendidGrid id="grdMain" AllowPaging="false" AllowSorting="false" EnableViewState="true" runat="server">
		<Columns>
			<asp:BoundColumn    HeaderText="Terminology.LBL_LIST_LANG"         DataField="NAME"         ItemStyle-Width="20%" />
			<asp:BoundColumn    HeaderText="Terminology.LBL_LIST_NAME_NAME"    DataField="DISPLAY_NAME" ItemStyle-Width="30%" />
			<asp:BoundColumn    HeaderText="Terminology.LBL_LIST_DISPLAY_NAME" DataField="NATIVE_NAME"  ItemStyle-Width="30%" />
			<asp:TemplateColumn HeaderText="Administration.LNK_ENABLED" ItemStyle-Width="5%" ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:Label Visible='<%#  Sql.ToBoolean(Eval("ACTIVE")) %>' Text='<%# L10n.Term(".LBL_YES") %>' Runat="server" />
					<asp:Label Visible='<%# !Sql.ToBoolean(Eval("ACTIVE")) %>' Text='<%# L10n.Term(".LBL_NO" ) %>' Runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="10%" ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton Visible='<%# SplendidCRM.Security.AdminUserAccess(m_sMODULE, "edit") >= 0 && (Sql.ToString(Eval("NAME")) != "en-US") &&  Sql.ToBoolean(Eval("ACTIVE")) %>' CommandName="Languages.Disable" CommandArgument='<%# Eval("NAME") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term("Administration.LNK_DISABLE") %>' SkinID="minus_inline" Runat="server" />
					<asp:LinkButton  Visible='<%# SplendidCRM.Security.AdminUserAccess(m_sMODULE, "edit") >= 0 && (Sql.ToString(Eval("NAME")) != "en-US") &&  Sql.ToBoolean(Eval("ACTIVE")) %>' CommandName="Languages.Disable" CommandArgument='<%# Eval("NAME") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term("Administration.LNK_DISABLE"         ) %>' Runat="server" />
					<asp:ImageButton Visible='<%# SplendidCRM.Security.AdminUserAccess(m_sMODULE, "edit") >= 0 && (Sql.ToString(Eval("NAME")) != "en-US") && !Sql.ToBoolean(Eval("ACTIVE")) %>' CommandName="Languages.Enable"  CommandArgument='<%# Eval("NAME") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term("Administration.LNK_ENABLE" ) %>' SkinID="plus_inline" Runat="server" />
					<asp:LinkButton  Visible='<%# SplendidCRM.Security.AdminUserAccess(m_sMODULE, "edit") >= 0 && (Sql.ToString(Eval("NAME")) != "en-US") && !Sql.ToBoolean(Eval("ACTIVE")) %>' CommandName="Languages.Enable"  CommandArgument='<%# Eval("NAME") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term("Administration.LNK_ENABLE"          ) %>' Runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
				<ItemTemplate>
					<span onclick="return confirm('<%= L10n.TermJavaScript(".NTC_DELETE_CONFIRMATION") %>')">
						<asp:ImageButton Visible='<%# SplendidCRM.Security.AdminUserAccess(m_sMODULE, "delete") >= 0 && (Sql.ToString(Eval("NAME")) != "en-US") %>' CommandName="Languages.Delete" CommandArgument='<%# Eval("NAME") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_DELETE") %>' SkinID="delete_inline" Runat="server" />
						<asp:LinkButton  Visible='<%# SplendidCRM.Security.AdminUserAccess(m_sMODULE, "delete") >= 0 && (Sql.ToString(Eval("NAME")) != "en-US") %>' CommandName="Languages.Delete" CommandArgument='<%# Eval("NAME") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term(".LNK_DELETE") %>' Runat="server" />
					</span>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</SplendidCRM:SplendidGrid>

	<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>

