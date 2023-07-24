<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="SplendidCRM.Administration.EditCustomFields.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divNewRecord" Visible="false" runat="server">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader ID="ctlListHeader" Title="EditCustomFields.LBL_ADD_FIELD" Runat="Server" />

	<table border="0" cellpadding="3" cellspacing="0" width="100%" class="tabForm">
		<tr>
			<td class="dataLabel"><asp:Label Text='<%# L10n.Term("EditCustomFields.COLUMN_TITLE_NAME") + ":" %>' runat="server" /><asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
			<td><asp:TextBox ID="txtNAME" Runat="server" /></td>
		</tr>
		<tr>
			<td class="dataLabel"><asp:Label Text='<%# L10n.Term("EditCustomFields.COLUMN_TITLE_LABEL") + ":" %>' runat="server" /><asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
			<td><asp:TextBox ID="txtLABEL" Runat="server" /></td>
		</tr>
		<tr>
			<td class="dataLabel"><asp:Label Text='<%# L10n.Term("EditCustomFields.COLUMN_TITLE_DATA_TYPE") + ":" %>' runat="server" /><asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
			<td>
				<asp:DropDownList ID="lstDATA_TYPE" OnSelectedIndexChanged="lstDATA_TYPE_Changed" AutoPostBack="true" Runat="server">
					<asp:ListItem Value="varchar">Text</asp:ListItem>
					<asp:ListItem Value="text"   >Text Area</asp:ListItem>
					<asp:ListItem Value="int"    >Integer</asp:ListItem>
					<asp:ListItem Value="float"  >Decimal</asp:ListItem>
					<asp:ListItem Value="bool"   >Checkbox</asp:ListItem>
					<asp:ListItem Value="date"   >Date</asp:ListItem>
					<asp:ListItem Value="enum"   >Dropdown</asp:ListItem>
					<asp:ListItem Value="guid"   >Guid</asp:ListItem>
					<asp:ListItem Value="guid"   >Image</asp:ListItem>
					<asp:ListItem Value="money"  >Money</asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
		<tr id="trMAX_SIZE" Runat="server">
			<td class="dataLabel"><asp:Label Text='<%# L10n.Term("EditCustomFields.COLUMN_TITLE_MAX_SIZE") + ":" %>' runat="server" /></td>
			<td><asp:TextBox ID="txtMAX_SIZE" Runat="server" /></td>
		</tr>
		<tr>
			<td class="dataLabel"><asp:Label Text='<%# L10n.Term("EditCustomFields.COLUMN_TITLE_REQUIRED_OPTION") + ":" %>' runat="server" /></td>
			<td><asp:CheckBox ID="chkREQUIRED" CssClass="checkbox" Runat="server" /></td>
		</tr>
		<tr>
			<td class="dataLabel"><asp:Label Text='<%# L10n.Term("EditCustomFields.COLUMN_TITLE_DEFAULT_VALUE") + ":" %>' runat="server" /></td>
			<td><asp:TextBox ID="txtDEFAULT_VALUE" Runat="server" /></td>
		</tr>
		<!--
		<tr>
			<td class="dataLabel"><asp:Label Text='<%# L10n.Term("EditCustomFields.COLUMN_TITLE_AUDIT") + ":" %>' runat="server" /></td>
			<td><asp:CheckBox ID="chkAUDITED" CssClass="checkbox" Runat="server" /></td>
		</tr>
		-->
		<tr id="trDROPDOWN_LIST" Visible="false" Runat="server">
			<td class="dataLabel">
				<asp:Label Text='<%# L10n.Term("EditCustomFields.LBL_DROPDOWN_LIST") %>' runat="server" />
			</td>
			<td>
				<asp:DropDownList ID="lstDROPDOWN_LIST" OnSelectedIndexChanged="lstDROPDOWN_LIST_Changed" DataTextField="LIST_NAME" DataValueField="LIST_NAME" AutoPostBack="true" Runat="server" />
			</td>
		</tr>
		<tr id="trDROPDOWN_LIST_PREVIEW" Visible="false" Runat="server">
			<td colspan="2">
				<table border="1" CellPadding="0" CellSpacing="0" width="100%">
					<tr>
						<td>
							<asp:DataGrid id="grdPICK_LIST_VALUES" Width="100%" CellPadding="3" CellSpacing="0" border="0" BackColor="white" 
								AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" EnableViewState="true" runat="server">
								<Columns>
									<asp:BoundColumn  DataField="NAME"         />
									<asp:BoundColumn  DataField="DISPLAY_NAME" />
								</Columns>
							</asp:DataGrid>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="2" align="left">
				<asp:Button ID="btnSave" CommandName="NewRecord" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) + "  " %>' ToolTip='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE") %>' AccessKey='<%# L10n.AccessKey(".LBL_SAVE_BUTTON_KEY") %>' Runat="server" /><br />
				<asp:RegularExpressionValidator ID="regNAME" ControlToValidate="txtNAME" ErrorMessage="(invalid field name)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" 
					ValidationExpression="^[A-Za-z_]\w*" />
				<asp:RequiredFieldValidator ID="reqNAME"  ControlToValidate="txtNAME"  ErrorMessage="(required)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
				<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
			</td>
		</tr>
	</table>
</div>

