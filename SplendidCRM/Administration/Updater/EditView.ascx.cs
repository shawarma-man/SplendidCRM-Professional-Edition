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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace SplendidCRM.Administration.Updater
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		// 05/31/2015 Paul.  Combine ModuleHeader and DynamicButtons. 
		protected _controls.HeaderButtons  ctlDynamicButtons;
		// 01/13/2010 Paul.  Add footer buttons. 
		protected _controls.DynamicButtons ctlFooterButtons ;

		protected DataView     vwMain               ;
		protected CheckBox     SEND_USAGE_INFO      ;
		protected CheckBox     CHECK_UPDATES        ;
		protected Table        AVAILABLE_UPDATES    ;
		protected Label        NO_UPDATES           ;
		protected SplendidGrid grdMain              ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Save" )
			{
				if ( Page.IsValid )
				{
					try
					{
						Application["CONFIG.send_usage_info"] = SEND_USAGE_INFO.Checked ? "true" : "false";
						SqlProcs.spCONFIG_Update("system", "send_usage_info", Sql.ToString(Application["CONFIG.send_usage_info"]));
						
						SqlProcs.spSCHEDULERS_UpdateStatus("function::CheckVersion", CHECK_UPDATES.Checked ? "Active" : "Inactive");
						if ( CHECK_UPDATES.Checked )
						{
							Application.Remove("available_version"            );
							Application.Remove("available_version_description");
						}
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
						ctlDynamicButtons.ErrorText = ex.Message;
						return;
					}
					Response.Redirect("../default.aspx");
				}
			}
			else if ( e.CommandName == "CheckNow" )
			{
				try
				{
					DataTable dt = Utils.CheckVersion(Application);

					vwMain = dt.DefaultView;
					vwMain.RowFilter = "New = '1'";
					vwMain.Sort      = "Build desc";
					grdMain.DataSource = vwMain ;
					grdMain.DataBind();
					grdMain.Visible    = (vwMain.Count > 0);
					NO_UPDATES.Visible = (vwMain.Count == 0);

					vwMain.RowFilter = String.Empty;
					if ( CHECK_UPDATES.Checked && vwMain.Count > 0 )
					{
						Application["available_version"            ] = Sql.ToString(vwMain[0]["Build"      ]);
						Application["available_version_description"] = Sql.ToString(vwMain[0]["Description"]);
					}
					else
					{
						Application.Remove("available_version"            );
						Application.Remove("available_version_description");
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
					ctlDynamicButtons.ErrorText = ex.Message;
				}
			}
			else if ( e.CommandName == "Cancel" )
			{
				Response.Redirect("../default.aspx");
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term("Administration.LBL_CONFIGURE_UPDATER"));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = SplendidCRM.Security.IS_ADMIN;
			if ( !this.Visible )
			{
				// 03/17/2010 Paul.  We need to rebind the parent in order to get the error message to display. 
				Parent.DataBind();
				return;
			}

			try
			{
				if ( !IsPostBack )
				{
					// 03/20/2008 Paul.  Dynamic buttons need to be recreated in order for events to fire. 
					ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
					ctlFooterButtons .AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);

					SEND_USAGE_INFO.Checked = Sql.ToBoolean(Application["CONFIG.send_usage_info"]);
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL ;
						sSQL = "select STATUS      " + ControlChars.CrLf
						     + "  from vwSCHEDULERS" + ControlChars.CrLf
						     + " where JOB = @JOB  " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@JOB", "function::CheckVersion");
							CHECK_UPDATES.Checked = (Sql.ToString(cmd.ExecuteScalar()) == "Active");
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
			ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
			ctlFooterButtons .Command += new CommandEventHandler(Page_Command);
			// 05/20/2007 Paul.  The m_sMODULE field must be set in order to allow default export handling. 
			m_sMODULE = "Updater";
			// 05/06/2010 Paul.  The menu will show the admin Module Name in the Six theme. 
			SetMenu(m_sMODULE);
			if ( IsPostBack )
			{
				// 03/20/2008 Paul.  Dynamic buttons need to be recreated in order for events to fire. 
				ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
				ctlFooterButtons .AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
			}
		}
		#endregion
	}
}

