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

namespace SplendidCRM.Calls
{
	/// <summary>
	/// Summary description for DetailView.
	/// </summary>
	public class DetailView : SplendidControl
	{
		// 05/31/2015 Paul.  Combine ModuleHeader and DynamicButtons. 
		protected _controls.HeaderButtons ctlDynamicButtons;

		protected Guid          gID                   ;
		protected HtmlTable     tblMain               ;
		protected PlaceHolder   plcSubPanel           ;
		protected HtmlTable     tblEditAllRecurrences ;
		protected HtmlTableCell tdEditAllRecurrences  ;
		protected HtmlTableCell tdDeleteAllRecurrences;
		protected HyperLink     lnkRepeatParent       ;
		protected LinkButton    lnkDeleteRecurrences  ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Edit" )
				{
					Response.Redirect("edit.aspx?ID=" + gID.ToString());
				}
				else if ( e.CommandName == "Duplicate" )
				{
					Response.Redirect("edit.aspx?DuplicateID=" + gID.ToString());
				}
				else if ( e.CommandName == "Delete" )
				{
					SqlProcs.spCALLS_Delete(gID);
					Response.Redirect("default.aspx");
				}
				// 03/27/2013 Paul.  Allow deletion of all recurrences. 
				else if ( e.CommandName == "DeleteRecurrences" )
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						using ( IDbTransaction trn = Sql.BeginTransaction(con) )
						{
							try
							{
								SqlProcs.spCALLS_DeleteRecurrences(gID, true, trn);
								SqlProcs.spCALLS_Delete(gID, trn);
								trn.Commit();
							}
							catch(Exception ex)
							{
								trn.Rollback();
								SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
								ctlDynamicButtons.ErrorText = ex.Message;
								return;
							}
						}
					}
					Response.Redirect("default.aspx");
				}
				else if ( e.CommandName == "Cancel" )
				{
					Response.Redirect("default.aspx");
				}
				// 10/10/2017 Paul.  Add Archive access right. 
				else if ( e.CommandName == "Archive.MoveData" )
				{
					ArchiveUtils archive = new ArchiveUtils(Context);
					ctlDynamicButtons.ErrorText = archive.MoveData(m_sMODULE, gID);
					if ( Sql.IsEmptyString(ctlDynamicButtons.ErrorText) )
						Response.Redirect("view.aspx?ID=" + gID.ToString() + "&ArchiveView=1");
				}
				else if ( e.CommandName == "Archive.RecoverData" )
				{
					ArchiveUtils archive = new ArchiveUtils(Context);
					ctlDynamicButtons.ErrorText = archive.RecoverData(m_sMODULE, gID);
					if ( Sql.IsEmptyString(ctlDynamicButtons.ErrorText) )
						Response.Redirect("view.aspx?ID=" + gID.ToString());
				}
				else
				{
					throw(new Exception("Unknown command: " + e.CommandName));
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
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "view") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				gID = Sql.ToGuid(Request["ID"]);
				// 11/28/2005 Paul.  We must always populate the table, otherwise it will disappear during event processing. 
				// 03/19/2008 Paul.  Place AppendDetailViewFields inside OnInit to avoid having to re-populate the data. 
				if ( !IsPostBack )
				{
					if ( !Sql.IsEmptyGuid(gID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							// 10/08/2017 Paul.  Add Archive access right. 
							// 11/01/2017 Paul.  Use a module-based flag so that Record Level Security is only enabled when needed. 
							sSQL = "select *"               + ControlChars.CrLf
							     + Sql.AppendRecordLevelSecurityField(m_sMODULE, "view", m_sVIEW_NAME)
							     + "  from " + m_sVIEW_NAME + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								// 11/24/2006 Paul.  Use new Security.Filter() function to apply Team and ACL security rules.
								// 10/08/2017 Paul.  Add Archive access right. 
								Security.Filter(cmd, m_sMODULE, (ArchiveViewExists() ? "archive" : "view"));
								Sql.AppendParameter(cmd, gID, "ID", false);
								con.Open();

								if ( bDebug )
									RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

								// 11/22/2010 Paul.  Convert data reader to data table for Rules Wizard. 
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dtCurrent = new DataTable() )
									{
										da.Fill(dtCurrent);
										// 10/31/2017 Paul.  Provide a way to inject Record level ACL. 
										if ( dtCurrent.Rows.Count > 0 && (SplendidCRM.Security.GetRecordAccess(dtCurrent.Rows[0], m_sMODULE, "view", "ASSIGNED_USER_ID") >= 0) )
										{
											DataRow rdr = dtCurrent.Rows[0];
											// 11/11/2010 Paul.  Apply Business Rules. 
											this.ApplyDetailViewPreLoadEventRules(m_sMODULE + "." + LayoutDetailView, rdr);
											
											// 05/31/2015 Paul.  Combine ModuleHeader and DynamicButtons. 
											ctlDynamicButtons.Title = Sql.ToString(rdr["NAME"]);
											SetPageTitle(L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlDynamicButtons.Title);
											// 10/10/2017 Paul.  Don't update tracker in ArchiveView. 
											if ( !ArchiveView() )
												Utils.UpdateTracker(Page, m_sMODULE, gID, ctlDynamicButtons.Title);
											ViewState["ctlDynamicButtons.Title"] = ctlDynamicButtons.Title;
											
											// 03/24/2013 Paul.  Provide a link to the parent. 
											Guid gREPEAT_PARENT_ID = Sql.ToGuid(rdr["REPEAT_PARENT_ID"]);
											if ( !Sql.IsEmptyGuid(gREPEAT_PARENT_ID) )
											{
												lnkRepeatParent.Text        = Sql.ToString(rdr["REPEAT_PARENT_NAME"]);
												// 04/08/2013 Paul.  Point to Calls module not Meetings module. 
												lnkRepeatParent.NavigateUrl = "~/Calls/edit.aspx?ID=" + gREPEAT_PARENT_ID.ToString();
												tdEditAllRecurrences.Visible = true;
												tblEditAllRecurrences.Visible = true;
											}
											else if ( !Sql.IsEmptyString(rdr["REPEAT_TYPE"]) )
											{
												tdDeleteAllRecurrences.Visible = true;
												tblEditAllRecurrences.Visible = true;
											}
											
											// 02/13/2013 Paul.  Move relationship append so that it can be controlled by business rules. 
											this.AppendDetailViewRelationships(m_sMODULE + "." + LayoutDetailView, plcSubPanel);
											this.AppendDetailViewFields(m_sMODULE + "." + LayoutDetailView, tblMain, rdr);
											// 03/20/2008 Paul.  Dynamic buttons need to be recreated in order for events to fire. 
											// 04/28/2008 Paul.  We will need the ASSIGNED_USER_ID in the sub-panels. 
											Page.Items["ASSIGNED_USER_ID"] = Sql.ToGuid(rdr["ASSIGNED_USER_ID"]);
											ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Sql.ToGuid(rdr["ASSIGNED_USER_ID"]), rdr);
											// 10/10/2017 Paul.  Add Archive access right. 
											int nACLACCESS_Archive = Security.GetUserAccess(m_sMODULE, "archive");
											ctlDynamicButtons.ShowButton("Archive.MoveData"   , (nACLACCESS_Archive >= ACL_ACCESS.ARCHIVE || Security.IS_ADMIN) && !ArchiveView() && ArchiveEnabled());
											ctlDynamicButtons.ShowButton("Archive.RecoverData", (nACLACCESS_Archive >= ACL_ACCESS.ARCHIVE || Security.IS_ADMIN) &&  ArchiveView() && ArchiveEnabled());
											// 10/31/2017 Paul.  Provide a way to inject Record level ACL. 
											ctlDynamicButtons.ShowButton("Duplicate", (SplendidCRM.Security.GetRecordAccess(rdr, m_sMODULE, "edit"  , "ASSIGNED_USER_ID") >= 0) && ctlDynamicButtons.IsButtonVisible("Duplicate"));
											ctlDynamicButtons.ShowButton("Edit"     , (SplendidCRM.Security.GetRecordAccess(rdr, m_sMODULE, "edit"  , "ASSIGNED_USER_ID") >= 0) && ctlDynamicButtons.IsButtonVisible("Edit"     ));
											ctlDynamicButtons.ShowButton("Delete"   , (SplendidCRM.Security.GetRecordAccess(rdr, m_sMODULE, "delete", "ASSIGNED_USER_ID") >= 0) && ctlDynamicButtons.IsButtonVisible("Delete"   ));
											// 11/10/2010 Paul.  Apply Business Rules. 
											this.ApplyDetailViewPostLoadEventRules(m_sMODULE + "." + LayoutDetailView, rdr);
										}
										else
										{
											// 11/25/2006 Paul.  If item is not visible, then don't show its sub panel either. 
											plcSubPanel.Visible = false;
											
											// 03/20/2008 Paul.  Dynamic buttons need to be recreated in order for events to fire. 
											ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Guid.Empty, null);
											ctlDynamicButtons.DisableAll();
											ctlDynamicButtons.ErrorText = L10n.Term("ACL.LBL_NO_ACCESS");
										}
									}
								}
							}
						}
					}
					else
					{
						// 03/20/2008 Paul.  Dynamic buttons need to be recreated in order for events to fire. 
						ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Guid.Empty, null);
						ctlDynamicButtons.DisableAll();
						//ctlDynamicButtons.ErrorText = L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + "ID";
					}
				}
				else
				{
					// 06/07/2015 Paul.  Seven theme DetailView.master uses an UpdatePanel, so we need to recall the title. 
					ctlDynamicButtons.Title = Sql.ToString(ViewState["ctlDynamicButtons.Title"]);
					SetPageTitle(L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlDynamicButtons.Title);
				}
				// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
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
			m_sMODULE = "Calls";
			// 10/08/2017 Paul.  Add Archive access right. 
			string sTABLE_NAME = Crm.Modules.TableName(m_sMODULE);
			m_sVIEW_NAME = "vw" + sTABLE_NAME;
			if ( ArchiveViewExists() )
				m_sVIEW_NAME += "_ARCHIVE";
			else
				m_sVIEW_NAME += "_Edit";
			this.LayoutDetailView = (ArchiveViewExists() ? "ArchiveView" : "DetailView");
			// 02/13/2007 Paul.  Calls should highlight the Activities menu. 
			// 03/15/2011 Paul.  Change menu to use main module. 
			SetMenu(m_sMODULE);
			if ( IsPostBack )
			{
				// 02/13/2013 Paul.  Move relationship append so that it can be controlled by business rules. 
				this.AppendDetailViewRelationships(m_sMODULE + "." + LayoutDetailView, plcSubPanel);
				this.AppendDetailViewFields(m_sMODULE + "." + LayoutDetailView, tblMain, null);
				// 03/20/2008 Paul.  Dynamic buttons need to be recreated in order for events to fire. 
				ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Guid.Empty, null);
			}
		}
		#endregion
	}
}

