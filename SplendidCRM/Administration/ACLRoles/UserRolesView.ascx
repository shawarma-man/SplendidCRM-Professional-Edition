<%@ Control CodeBehind="UserRolesView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Administration.ACLRoles.UserRolesView" %>
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
<script type="text/javascript">
function ChangeRole(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= txtROLE_ID.ClientID   %>').value = sPARENT_ID  ;
	document.forms[0].submit();
}
function RoleMultiSelect()
{
	return window.open('PopupMultiSelect.aspx', 'RolePopup', '<%= SplendidCRM.Crm.Config.PopupWindowOptions() %>');
}
</script>
<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
<SplendidCRM:ListHeader Visible="<%# !PrintView %>" Title="ACLRoles.LBL_SEARCH_FORM_TITLE" Runat="Server" />
<div id="divSearch">
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Table SkinID="tabSearchView" runat="server">
					<asp:TableRow>
						<asp:TableCell width="20%" CssClass="dataLabel"><%= L10n.Term(".LBL_ASSIGNED_TO") %></asp:TableCell>
						<asp:TableCell width="70%" CssClass="dataField"><asp:DropDownList ID="lstUSERS" DataValueField="ID" DataTextField="USER_NAME" TabIndex="1" AutoPostBack="True" Runat="server" /></asp:TableCell>
						<asp:TableCell width="10%" CssClass="dataLabel" HorizontalAlign="Right" Wrap="false">
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_SEARCH_BUTTON_TITLE") %>' AccessKey='<%# L10n.AccessKey(".LBL_SEARCH_BUTTON_KEY") %>' Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</div>
<br />

<div id="divAccessRights">
	<%@ Register TagPrefix="SplendidCRM" Tagname="AccessView" Src="AccessView.ascx" %>
	<SplendidCRM:AccessView ID="ctlAccessView" EnableACLEditing="false" Runat="Server" />
</div>

<div id="divUsersRoles">
	<input ID="txtROLE_ID" type="hidden" Runat="server" />
	<br />
	
	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Button ID="btnSelectRole" UseSubmitBehavior="false" OnClientClick="RoleMultiSelect(); return false;" CssClass="button" Text='<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' AccessKey='<%# L10n.AccessKey(".LBL_SELECT_BUTTON_KEY") %>' Runat="server" />
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>

	<SplendidCRM:SplendidGrid id="grdMain" AllowPaging="false" AllowSorting="false" EnableViewState="true" runat="server">
		<Columns>
			<asp:HyperLinkColumn HeaderText="ACLRoles.LBL_NAME"        DataTextField="ROLE_NAME" ItemStyle-Width="60%" ItemStyle-CssClass="listViewTdLinkS1" DataNavigateUrlField="ROLE_ID" DataNavigateUrlFormatString="~/Administration/ACLRoles/view.aspx?id={0}" />
			<asp:BoundColumn     HeaderText="ACLRoles.LBL_DESCRIPTION" DataField="DESCRIPTION"   ItemStyle-Width="30%" />
			<asp:TemplateColumn  HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton CommandName="Roles.Edit"   CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ROLE_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" SkinID="edit_inline" AlternateText='<%# L10n.Term(".LNK_EDIT") %>' Runat="server" />
					<asp:LinkButton  CommandName="Roles.Edit"   CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ROLE_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Runat="server"><%# L10n.Term(".LNK_EDIT") %></asp:LinkButton>
					&nbsp;
					<asp:ImageButton CommandName="Roles.Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ROLE_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_REMOVE") %>' SkinID="delete_inline" Runat="server" />
					<asp:LinkButton  CommandName="Roles.Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ROLE_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term(".LNK_REMOVE") %>' Runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</SplendidCRM:SplendidGrid>

	<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>

