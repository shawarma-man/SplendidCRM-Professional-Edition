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
using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace SplendidCRM.SmsMessages
{
	/// <summary>
	/// Summary description for InboundView.
	/// </summary>
	public class InboundView : SplendidControl
	{
		// 05/31/2015 Paul.  Combine ModuleHeader and DynamicButtons. 
		protected _controls.HeaderButtons  ctlDynamicButtons;

		protected Guid        gID              ;
		protected HtmlTable   tblMain          ;
		protected PlaceHolder plcSubPanel      ;
		protected Repeater    ctlAttachments   ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Forward" )
				{
					Response.Redirect("edit.aspx?type=forward&DuplicateID=" + gID.ToString());
				}
				else if ( e.CommandName == "Reply" )
				{
					Response.Redirect("edit.aspx?type=reply&DuplicateID=" + gID.ToString());
				}
				else if ( e.CommandName == "Reply All" )
				{
					Response.Redirect("edit.aspx?type=replyall&DuplicateID=" + gID.ToString());
				}
				else if ( e.CommandName == "Delete" )
				{
					SqlProcs.spSMS_MESSAGES_Delete(gID);
					Response.Redirect("default.aspx");
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				ctlDynamicButtons.ErrorText = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term(".moduleList." + m_sMODULE));
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "view") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					if ( !Sql.IsEmptyGuid(gID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *                  " + ControlChars.CrLf
							     + "  from vwSMS_MESSAGES_Edit" + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Security.Filter(cmd, m_sMODULE, "view");
								Sql.AppendParameter(cmd, gID, "ID", false);
								con.Open();
								
								if ( bDebug )
									RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
								
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dtCurrent = new DataTable() )
									{
										da.Fill(dtCurrent);
										if ( dtCurrent.Rows.Count > 0 )
										{
											DataRow rdr = dtCurrent.Rows[0];
											this.ApplyDetailViewPreLoadEventRules(m_sMODULE + ".DetailView", rdr);
											
											// 05/31/2015 Paul.  Combine ModuleHeader and DynamicButtons. 
											ctlDynamicButtons.Title = Sql.ToString(rdr["NAME"]);
											SetPageTitle(L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlDynamicButtons.Title);
											Utils.UpdateTracker(Page, m_sMODULE, gID, ctlDynamicButtons.Title);
											
											this.AppendDetailViewRelationships(m_sMODULE + ".DetailView", plcSubPanel);
											this.AppendDetailViewFields(m_sMODULE + ".DetailView", tblMain, rdr);
											
											string sSUBJECT = Sql.ToString(rdr["NAME"]);
											new DynamicControl(this, "NAME").Text = sSUBJECT;
											
											string sEMAIL_TYPE = Sql.ToString(rdr["TYPE"]).ToLower();
											// 05/31/2015 Paul.  Combine ModuleHeader and DynamicButtons. 
											ctlDynamicButtons.EnableModuleLabel = false;
											switch ( sEMAIL_TYPE )
											{
												case "archived":
													ctlDynamicButtons.Title = "<a href=\"default.aspx\">" + L10n.Term("SmsMessages.LBL_ARCHIVED_MODULE_NAME") + "</a><span class=\"pointer\">&raquo;</span>" + ctlDynamicButtons.Title;
													break;
												case "inbound":
													ctlDynamicButtons.Title = "<a href=\"default.aspx\">" + L10n.Term("SmsMessages.LBL_INBOUND_TITLE") + "</a><span class=\"pointer\">&raquo;</span>" + ctlDynamicButtons.Title;
													string sEMAIL_STATUS = Sql.ToString(rdr["STATUS"]).ToLower();
													if ( sEMAIL_STATUS == "unread" )
													{
														SqlProcs.spSMS_MESSAGES_UpdateStatus(gID, "read", String.Empty);
													}
													break;
												case "out":
													ctlDynamicButtons.Title = "<a href=\"default.aspx\">" + L10n.Term("SmsMessages.LBL_LIST_FORM_SENT_TITLE") + "</a><span class=\"pointer\">&raquo;</span>" + ctlDynamicButtons.Title;
													Response.Redirect("view.aspx?ID=" + gID.ToString());
													break;
												case "sent":
													ctlDynamicButtons.Title = "<a href=\"default.aspx\">" + L10n.Term("SmsMessages.LBL_LIST_FORM_SENT_TITLE") + "</a><span class=\"pointer\">&raquo;</span>" + ctlDynamicButtons.Title;
													break;
												case "campaign":
													ctlDynamicButtons.Title = "<a href=\"default.aspx\">" + L10n.Term("SmsMessages.LBL_LIST_FORM_SENT_TITLE") + "</a><span class=\"pointer\">&raquo;</span>" + ctlDynamicButtons.Title;
													Response.Redirect("view.aspx?ID=" + gID.ToString());
													break;
												default:
													sEMAIL_TYPE = "draft";
													ctlDynamicButtons.Title = "<a href=\"default.aspx\">" + L10n.Term("SmsMessages.LBL_NEW_FORM_TITLE" ) + "</a><span class=\"pointer\">&raquo;</span>" + ctlDynamicButtons.Title;
													Response.Redirect("edit.aspx?ID=" + gID.ToString());
													break;
											}
											Page.Items["ASSIGNED_USER_ID"] = Sql.ToGuid(rdr["ASSIGNED_USER_ID"]);
											ctlDynamicButtons.AppendButtons(m_sMODULE + ".InboundView", Sql.ToGuid(rdr["ASSIGNED_USER_ID"]), rdr);
											this.ApplyDetailViewPostLoadEventRules(m_sMODULE + "." + LayoutDetailView, rdr);
										}
										else
										{
											plcSubPanel.Visible = false;
											ctlDynamicButtons.DisableAll();
											ctlDynamicButtons.ErrorText = L10n.Term("ACL.LBL_NO_ACCESS");
										}
									}
								}
							}
							sSQL = "select *                         " + ControlChars.CrLf
							     + "  from vwSMS_MESSAGES_Attachments" + ControlChars.CrLf
							     + " where PARENT_ID = @PARENT_ID    " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@PARENT_ID", gID);
								
								if ( bDebug )
									RegisterClientScriptBlock("vwSMS_MESSAGES_Attachments", Sql.ClientScriptBlock(cmd));
								
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dt = new DataTable() )
									{
										da.Fill(dt);
										ctlAttachments.DataSource = dt.DefaultView;
										ctlAttachments.DataBind();
									}
								}
							}
						}
					}
				}
				else
				{
					if ( !Sql.IsEmptyGuid(gID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *                         " + ControlChars.CrLf
							     + "  from vwSMS_MESSAGES_Attachments" + ControlChars.CrLf
							     + " where PARENT_ID = @PARENT_ID    " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@PARENT_ID", gID);
								
								if ( bDebug )
									RegisterClientScriptBlock("vwSMS_MESSAGES_Attachments", Sql.ClientScriptBlock(cmd));
								
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dt = new DataTable() )
									{
										da.Fill(dt);
										ctlAttachments.DataSource = dt.DefaultView;
									}
								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				ctlDynamicButtons.ErrorText = ex.Message;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
			ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
			m_sMODULE = "SmsMessages";
			SetMenu(m_sMODULE);
			if ( IsPostBack )
			{
				this.AppendDetailViewRelationships(m_sMODULE + ".DetailView", plcSubPanel);
				this.AppendDetailViewFields(m_sMODULE + ".DetailView", tblMain, null);
				ctlDynamicButtons.AppendButtons(m_sMODULE + ".InboundView", Guid.Empty, Guid.Empty);
			}
		}
		#endregion
	}
}

