<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="SplendidCRM.Emails.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
	<SplendidCRM:HeaderLeft ID="ctlHeaderLeft" Title="Emails.LBL_NEW_FORM_TITLE" Width=<%# uWidth %> Visible="<%# ShowHeader %>" Runat="Server" />

	<asp:Panel ID="pnlMain" Width="100%" CssClass="leftColumnModuleS3" runat="server">
		<%@ Register TagPrefix="SplendidCRM" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
		<SplendidCRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# ShowTopButtons && !PrintView %>" Runat="server" />

		<asp:Panel ID="pnlEdit" CssClass="" style="margin-bottom: 4px;" Width=<%# uWidth %> runat="server">
			<asp:Literal Text='<%# "<h4>" + L10n.Term("Emails.LBL_NEW_FORM_TITLE") + "</h4>" %>' Visible="<%# ShowInlineHeader %>" runat="server" />

			<asp:HiddenField ID="txtTO_ADDRS_IDS"     runat="server" />
			<asp:HiddenField ID="txtTO_ADDRS_NAMES"   runat="server" />
			<asp:HiddenField ID="txtTO_ADDRS_EMAILS"  runat="server" />
			<asp:HiddenField ID="txtCC_ADDRS_IDS"     runat="server" />
			<asp:HiddenField ID="txtCC_ADDRS_NAMES"   runat="server" />
			<asp:HiddenField ID="txtCC_ADDRS_EMAILS"  runat="server" />
			<asp:HiddenField ID="txtBCC_ADDRS_IDS"    runat="server" />
			<asp:HiddenField ID="txtBCC_ADDRS_NAMES"  runat="server" />
			<asp:HiddenField ID="txtBCC_ADDRS_EMAILS" runat="server" />
			<asp:HiddenField ID="hidREMOVE_LABEL"     Value='<%# L10n.Term(".LBL_REMOVE") %>' runat="server" />
			<asp:HiddenField ID="hidATTACHMENT_COUNT" Value="0" runat="server" />

			<asp:TableCell>
				<asp:Table SkinID="tabEditView" runat="server">
					<asp:TableRow id="trDATE_START" Runat="server">
						<asp:TableCell Width="20%" CssClass="dataLabel" VerticalAlign="top">
							<%= L10n.Term("Emails.LBL_DATE_AND_TIME") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" />
						</asp:TableCell>
						<asp:TableCell Width="35%" CssClass="dataField">
							<%@ Register TagPrefix="SplendidCRM" Tagname="DateTimeEdit" Src="~/_controls/DateTimeEdit.ascx" %>
							<SplendidCRM:DateTimeEdit ID="ctlDATE_START" EnableNone="false" Runat="Server" />
						</asp:TableCell>
						<asp:TableCell Width="15%" CssClass="dataLabel">&nbsp;</asp:TableCell>
						<asp:TableCell Width="30%" CssClass="dataLabel">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel" VerticalAlign="top"><div id="divTEAM_LABEL" style="DISPLAY: <%= SplendidCRM.Crm.Config.enable_team_management() ? "INLINE" : "NONE" %>"><%= L10n.Term("Teams.LBL_TEAM") %></div></asp:TableCell>
						<asp:TableCell CssClass="dataField" VerticalAlign="top">
							<asp:Panel Visible="<%# SplendidCRM.Crm.Config.enable_team_management() && !SplendidCRM.Crm.Config.enable_dynamic_teams() %>" runat="server">
								<asp:TextBox     ID="TEAM_NAME"     ReadOnly="True" Runat="server" />
								<asp:HiddenField ID="TEAM_ID"       runat="server" />&nbsp;
								<asp:Button      ID="btnChangeTeam" UseSubmitBehavior="false" OnClientClick=<%# "return ModulePopup('Teams', '" + TEAM_ID.ClientID + "', '" + TEAM_NAME.ClientID + "', null, false, null);" %> Text='<%# L10n.Term(".LBL_CHANGE_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_CHANGE_BUTTON_TITLE") %>' CssClass="button" runat="server" />
							</asp:Panel>
							<%@ Register TagPrefix="SplendidCRM" Tagname="TeamSelect" Src="~/_controls/TeamSelect.ascx" %>
							<SplendidCRM:TeamSelect ID="ctlTeamSelect" Visible="<%# SplendidCRM.Crm.Config.enable_team_management() && SplendidCRM.Crm.Config.enable_dynamic_teams() %>" Runat="Server" />
						</asp:TableCell>
						<asp:TableCell CssClass="dataLabel" VerticalAlign="top"><asp:DropDownList ID="lstPARENT_TYPE" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="3" onChange=<%# "ClearModuleType('', '" + txtPARENT_ID.ClientID + "', '" + txtPARENT_NAME.ClientID + "', false);" %> runat="server" /></asp:TableCell>
						<asp:TableCell CssClass="dataField" VerticalAlign="top" Wrap="false">
							<asp:TextBox     ID="txtPARENT_NAME" ReadOnly="True" Runat="server" />
							<asp:HiddenField ID="txtPARENT_ID" runat="server" />&nbsp;
							<asp:Button      ID="btnChangeParent" UseSubmitBehavior="false" OnClientClick=<%# "return ModulePopup(document.getElementById('" + lstPARENT_TYPE.ClientID + "').options[document.getElementById('" + lstPARENT_TYPE.ClientID + "').options.selectedIndex].value, '" + txtPARENT_ID.ClientID + "', '" + txtPARENT_NAME.ClientID + "', null, false, null);" %> Text='<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' CssClass="button" runat="server" />&nbsp;
							<asp:Button      ID="btnClearParent"  UseSubmitBehavior="false" OnClientClick=<%# "return ClearModuleType('', '" + txtPARENT_ID.ClientID + "', '" + txtPARENT_NAME.ClientID + "', false);" %> Text='<%# L10n.Term(".LBL_CLEAR_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_CLEAR_BUTTON_TITLE") %>' CssClass="button" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term(".LBL_ASSIGNED_TO") %></asp:TableCell>
						<asp:TableCell CssClass="dataField">
							<asp:Panel Visible="<%# !SplendidCRM.Crm.Config.enable_dynamic_assignment() %>" runat="server">
								<asp:TextBox     ID="ASSIGNED_TO" ReadOnly="True" Runat="server" />
								<asp:HiddenField ID="ASSIGNED_USER_ID" runat="server" />&nbsp;
								<asp:Button      ID="btnChangeAssigned" UseSubmitBehavior="false" OnClientClick=<%# "return ModulePopup('Users', '" + ASSIGNED_USER_ID.ClientID + "', '" + ASSIGNED_TO.ClientID + "', null, false, null);" %> Text='<%# L10n.Term(".LBL_CHANGE_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_CHANGE_BUTTON_TITLE") %>' CssClass="button" Runat="server" />
							</asp:Panel>
							<%@ Register TagPrefix="SplendidCRM" Tagname="UserSelect" Src="~/_controls/UserSelect.ascx" %>
							<SplendidCRM:UserSelect ID="ctlUserSelect" Visible="<%# SplendidCRM.Crm.Config.enable_dynamic_assignment() %>" Runat="Server" />
						</asp:TableCell>
						<asp:TableCell CssClass="dataLabel">
							<span ID="spnTEMPLATE_LABEL" runat="server">
							<%= L10n.Term("Emails.LBL_USE_TEMPLATE") %>
							</span>
						</asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<asp:DropDownList ID="lstEMAIL_TEMPLATE" DataValueField="ID" DataTextField="NAME" TabIndex="0" OnSelectedIndexChanged="lstEMAIL_TEMPLATE_Changed" AutoPostBack="true" Runat="server" />
							<asp:CheckBox ID="chkPREPEND_TEMPLATE" Text='<%# L10n.Term("Emails.LBL_PREPEND_TEMPLATE") %>' CssClass="checkbox" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"></asp:TableCell>
						<asp:TableCell CssClass="dataField"></asp:TableCell>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term("Emails.LBL_SIGNATURE") %></asp:TableCell>
						<asp:TableCell CssClass="dataField">
							<asp:DropDownList ID="lstSIGNATURE" DataValueField="ID" DataTextField="NAME" TabIndex="0" OnSelectedIndexChanged="lstSIGNATURE_Changed" AutoPostBack="true" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="4">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow id="trNOTE_SEMICOLON" runat="server">
						<asp:TableCell CssClass="dataLabel">&nbsp;</asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3"><%= L10n.Term("Emails.LBL_NOTE_SEMICOLON") %></asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term("Emails.LBL_TO") %></asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<asp:TextBox ID="txtTO_ADDRS" TabIndex="0" TextMode="MultiLine" Columns="80" Rows="1" style="overflow-y:auto;" Runat="server" />&nbsp;
							<asp:Button ID="btnChangeTO"  UseSubmitBehavior="false" OnClientClick=<%# "sChangeContactEmailADDRS='" + txtTO_ADDRS.ClientID  + "'; sChangeContactEmailADDRS_IDS='" + txtTO_ADDRS_IDS.ClientID  + "'; sChangeContactEmailADDRS_NAMES='" + txtTO_ADDRS_NAMES.ClientID  + "'; sChangeContactEmailADDRS_EMAILS='" + txtTO_ADDRS_EMAILS.ClientID  + "'; window.open('" + Application["rootURL"] + "Emails/PopupEmailAddresses.aspx', 'EmailAddressesPopup', '" + SplendidCRM.Crm.Config.PopupWindowOptions() + "'); return false;" %> Text='<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' CssClass="button" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term("Emails.LBL_CC") %></asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<asp:TextBox ID="txtCC_ADDRS" TabIndex="0" TextMode="MultiLine" Columns="80" Rows="1" style="overflow-y:auto;" Runat="server" />&nbsp;
							<asp:Button ID="btnChangeCC"  UseSubmitBehavior="false" OnClientClick=<%# "sChangeContactEmailADDRS='" + txtCC_ADDRS.ClientID  + "'; sChangeContactEmailADDRS_IDS='" + txtCC_ADDRS_IDS.ClientID  + "'; sChangeContactEmailADDRS_NAMES='" + txtCC_ADDRS_NAMES.ClientID  + "'; sChangeContactEmailADDRS_EMAILS='" + txtCC_ADDRS_EMAILS.ClientID  + "'; window.open('" + Application["rootURL"] + "Emails/PopupEmailAddresses.aspx', 'EmailAddressesPopup', '" + SplendidCRM.Crm.Config.PopupWindowOptions() + "'); return false;" %> Text='<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' CssClass="button" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term("Emails.LBL_BCC") %></asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<asp:TextBox ID="txtBCC_ADDRS" TabIndex="0" TextMode="MultiLine" Columns="80" Rows="1" style="overflow-y:auto;" Runat="server" />&nbsp;
							<asp:Button ID="btnChangeBCC" UseSubmitBehavior="false" OnClientClick=<%# "sChangeContactEmailADDRS='" + txtBCC_ADDRS.ClientID + "'; sChangeContactEmailADDRS_IDS='" + txtBCC_ADDRS_IDS.ClientID + "'; sChangeContactEmailADDRS_NAMES='" + txtBCC_ADDRS_NAMES.ClientID + "'; sChangeContactEmailADDRS_EMAILS='" + txtBCC_ADDRS_EMAILS.ClientID + "'; window.open('" + Application["rootURL"] + "Emails/PopupEmailAddresses.aspx', 'EmailAddressesPopup', '" + SplendidCRM.Crm.Config.PopupWindowOptions() + "'); return false;" %> Text='<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>' ToolTip='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' CssClass="button" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term("Emails.LBL_FROM") %></asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<asp:DropDownList ID="MAILBOX_ID" DataValueField="ID" DataTextField="DISPLAY_NAME" TabIndex="0" Runat="server" />&nbsp;
							<SplendidCRM:RequiredFieldValidatorForDropDownList ID="reqMAILBOX_ID" ControlToValidate="MAILBOX_ID" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="4">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term("Emails.LBL_SUBJECT") %></asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<asp:TextBox ID="txtNAME" TabIndex="0" TextMode="MultiLine" Columns="100" Rows="1" style="overflow-y:auto;" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= L10n.Term("Emails.LBL_BODY") %></asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<%@ Register TagPrefix="CKEditor" Namespace="CKEditor.NET" Assembly="CKEditor.NET" %>
							<CKEditor:CKEditorControl id="txtDESCRIPTION" Toolbar="SplendidCRM" Language='<%# Session["USER_SETTINGS/CULTURE"] %>' BasePath="~/ckeditor/" FilebrowserUploadUrl="../ckeditor/upload.aspx" FilebrowserBrowseUrl="../Images/Popup.aspx" FilebrowserWindowWidth="640" FilebrowserWindowHeight="480" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel" VerticalAlign="Top"><%= L10n.Term("Emails.LBL_ATTACHMENTS") %></asp:TableCell>
						<asp:TableCell CssClass="dataField" VerticalAlign="Top" ColumnSpan="3">
							<asp:Repeater id="ctlTemplateAttachments" runat="server">
								<HeaderTemplate />
								<ItemTemplate>
									<asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "FILENAME") %>' NavigateUrl='<%# "~/Notes/Attachment.aspx?ID=" + DataBinder.Eval(Container.DataItem, "NOTE_ATTACHMENT_ID") %>' Target="_blank" Runat="server" /><br />
								</ItemTemplate>
								<FooterTemplate />
							</asp:Repeater>
							<asp:Repeater id="ctlKBAttachments" runat="server">
								<HeaderTemplate />
								<ItemTemplate>
									<asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "FILENAME") %>' NavigateUrl='<%# "~/KBDocuments/Attachment.aspx?ID=" + DataBinder.Eval(Container.DataItem, "ID") %>' Target="_blank" Runat="server" /><br />
								</ItemTemplate>
								<FooterTemplate />
							</asp:Repeater>
							<asp:Repeater id="ctlAttachments" runat="server">
								<HeaderTemplate />
								<ItemTemplate>
									<asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "FILENAME") %>' NavigateUrl='<%# "~/Notes/Attachment.aspx?ID=" + DataBinder.Eval(Container.DataItem, "NOTE_ATTACHMENT_ID") %>' Target="_blank" Runat="server" /><br />
								</ItemTemplate>
								<FooterTemplate />
							</asp:Repeater>
							<div id="<%= this.ClientID %>_attachments_div"></div>
							<div style="display: none">
								<input id="dummy_email_attachment" type="file" tabindex="0" size="40" runat="server" />
							</div>
							<asp:Button UseSubmitBehavior="false" OnClientClick=<%# "AddFile(\'" + this.ClientID + "\', \'" + hidREMOVE_LABEL.ClientID + "\', \'" + hidATTACHMENT_COUNT.ClientID + "\'); return false;" %> Text='<%# L10n.Term("Emails.LBL_ADD_FILE") %>' CssClass="button" runat="server" />
							<br />
							<asp:HyperLink ID="lnkReportAttachment" Target="_blank" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:Panel>

		<SplendidCRM:DynamicButtons ID="ctlFooterButtons" Visible="<%# ShowBottomButtons && !PrintView %>" Runat="server" />
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
</div>

