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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace SplendidCRM.ChatChannels
{
	/// <summary>
	///		Summary description for New.
	/// </summary>
	public class NewRecord : NewRecordControl
	{
		protected _controls.DynamicButtons ctlDynamicButtons;
		protected _controls.DynamicButtons ctlFooterButtons ;
		protected _controls.HeaderLeft     ctlHeaderLeft    ;

		protected Guid            gID                             ;
		protected HtmlTable       tblMain                         ;
		protected Label           lblError                        ;
		protected Panel           pnlMain                         ;
		protected Panel           pnlEdit                         ;

		public override bool IsEmpty()
		{
			string sNAME = new DynamicControl(this, "NAME").Text;
			return Sql.IsEmptyString(sNAME);
		}

		public override void ValidateEditViewFields()
		{
			if ( !IsEmpty() )
			{
				this.ValidateEditViewFields(m_sMODULE + "." + sEditView);
				this.ApplyEditViewValidationEventRules(m_sMODULE + "." + sEditView);
			}
		}

		public override void Save(Guid gPARENT_ID, string sPARENT_TYPE, IDbTransaction trn)
		{
			if ( IsEmpty() )
				return;
			
			string    sTABLE_NAME    = Crm.Modules.TableName(m_sMODULE);
			DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sTABLE_NAME);
			
			Guid gASSIGNED_USER_ID  = new DynamicControl(this, "ASSIGNED_USER_ID").ID;
			Guid gTEAM_ID           = new DynamicControl(this, "TEAM_ID"         ).ID;
			if ( Sql.IsEmptyGuid(gASSIGNED_USER_ID) )
				gASSIGNED_USER_ID = Security.USER_ID;
			if ( Sql.IsEmptyGuid(gTEAM_ID) )
				gTEAM_ID = Security.TEAM_ID;
			SqlProcs.spCHAT_CHANNELS_Update
				( ref gID
				, gASSIGNED_USER_ID
				, new DynamicControl(this, "NAME"         ).Text
				, gPARENT_ID
				, sPARENT_TYPE
				, gTEAM_ID
				, new DynamicControl(this, "TEAM_SET_LIST").Text
				// 05/17/2017 Paul.  Add Tags module. 
				, new DynamicControl(this, "TAG_SET_NAME" ).Text
				// 11/30/2017 Paul.  Add ASSIGNED_SET_ID for Dynamic User Assignment. 
				, new DynamicControl(this, "ASSIGNED_SET_LIST").Text
				, trn
				);
		}

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "NewRecord" )
				{
					this.ValidateEditViewFields(m_sMODULE + "." + sEditView);
					this.ApplyEditViewValidationEventRules(m_sMODULE + "." + sEditView);
					if ( Page.IsValid )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							con.Open();
							this.ApplyEditViewPreSaveEventRules(m_sMODULE + "." + sEditView, null);
							
							using ( IDbTransaction trn = Sql.BeginTransaction(con) )
							{
								try
								{
									Guid   gPARENT_ID   = this.PARENT_ID;
									string sPARENT_TYPE = this.PARENT_TYPE;
									Save(gPARENT_ID, sPARENT_TYPE, trn);
									trn.Commit();
								}
								catch(Exception ex)
								{
									trn.Rollback();
									SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
									if ( bShowFullForm || bShowCancel )
										ctlFooterButtons.ErrorText = ex.Message;
									else
										lblError.Text = ex.Message;
									return;
								}
							}
							DataRow rowCurrent = Crm.Modules.ItemEdit(m_sMODULE, gID);
							this.ApplyEditViewPostSaveEventRules(m_sMODULE + "." + sEditView, rowCurrent);
						}
						if ( !Sql.IsEmptyString(RulesRedirectURL) )
							Response.Redirect(RulesRedirectURL);
						else if ( Command != null )
							Command(sender, new CommandEventArgs(e.CommandName, gID.ToString()));
						else if ( !Sql.IsEmptyGuid(gID) )
							Response.Redirect("~/" + m_sMODULE + "/view.aspx?ID=" + gID.ToString());
					}
				}
				else if ( Command != null )
				{
					Command(sender, e);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				if ( bShowFullForm || bShowCancel )
					ctlFooterButtons.ErrorText = ex.Message;
				else
					lblError.Text = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 01/02/2020 Paul.  Allow the NewRecord to be disabled per module using config table. 
			this.Visible = (!Sql.ToBoolean(Application["CONFIG." + m_sMODULE + ".DisableNewRecord"]) || sEditView != "NewRecord") && (SplendidCRM.Security.GetUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				bool bIsPostBack = this.IsPostBack && !NotPostBack;
				if ( !bIsPostBack )
				{
					if ( NotPostBack )
						this.DataBind();
					this.AppendEditViewFields(m_sMODULE + "." + sEditView, tblMain, null, ctlFooterButtons.ButtonClientID("NewRecord"));
					if ( EditViewLoad != null )
						EditViewLoad(this, null);
					
					if ( bShowFullForm || bShowCancel || sEditView != "NewRecord" )
					{
						pnlMain.CssClass = "";
						pnlEdit.CssClass = "tabForm";
						
						Guid gPARENT_ID = this.PARENT_ID;
						if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						{
							string sMODULE      = String.Empty;
							string sPARENT_TYPE = String.Empty;
							string sPARENT_NAME = String.Empty;
							SqlProcs.spPARENT_Get( ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
							if ( !Sql.IsEmptyGuid(gPARENT_ID) && sPARENT_TYPE == "Accounts" )
							{
								new DynamicControl(this, "PARENT_ID"  ).ID   = gPARENT_ID  ;
								new DynamicControl(this, "PARENT_NAME").Text = sPARENT_NAME;
							}
						}
					}
					this.ApplyEditViewNewEventRules(m_sMODULE + "." + sEditView);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				if ( bShowFullForm || bShowCancel )
					ctlFooterButtons.ErrorText = ex.Message;
				else
					lblError.Text = ex.Message;
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

			ctlDynamicButtons.AppendButtons("NewRecord." + (bShowFullForm ? "FullForm" : (bShowCancel ? "WithCancel" : "SaveOnly")), Guid.Empty, Guid.Empty);
			ctlFooterButtons .AppendButtons("NewRecord." + (bShowFullForm ? "FullForm" : (bShowCancel ? "WithCancel" : "SaveOnly")), Guid.Empty, Guid.Empty);
			m_sMODULE = "Accounts";
			bool bIsPostBack = this.IsPostBack && !NotPostBack;
			if ( bIsPostBack )
			{
				this.AppendEditViewFields(m_sMODULE + "." + sEditView, tblMain, null, ctlFooterButtons.ButtonClientID("NewRecord"));
				if ( EditViewLoad != null )
					EditViewLoad(this, null);
				Page.Validators.Add(new RulesValidator(this));
			}
		}
		#endregion
	}
}

