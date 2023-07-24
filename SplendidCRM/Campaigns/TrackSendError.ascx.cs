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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace SplendidCRM.Campaigns
{
	/// <summary>
	///		Summary description for TrackSendError.
	/// </summary>
	public class TrackSendError : SplendidControl
	{
		protected _controls.DynamicButtons ctlDynamicButtons;
		protected UniqueStringCollection arrSelectFields;
		protected Guid          gID            ;
		protected DataView      vwMain         ;
		protected SplendidGrid  grdMain        ;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				// 08/22/2012 Paul.  Allow prospect list to be created from the tracked users. 
				if ( e.CommandName == "ProspectLists.Create" )
				{
					string sSQL  = "select TARGET_ID from vwCAMPAIGN_LOG_TrackSendError where CAMPAIGN_ID = '" + gID.ToString() + "'";
					string sNAME = Sql.ToString(Page.Items["NAME"]) + " - " + L10n.Term("Campaigns.LBL_LOG_ENTRIES_SEND_ERROR_TITLE");
					if ( sNAME.Length > 50 )
						sNAME = Sql.ToString(Page.Items["NAME"]).Substring(0, Sql.ToString(Page.Items["NAME"]).Length - (sNAME.Length - 50)) + " - " + L10n.Term("Campaigns.LBL_LOG_ENTRIES_SEND_ERROR_TITLE");
					Guid gPROSPECT_LIST_ID = Guid.Empty;
					SqlProcs.spPROSPECT_LISTS_InsertCampaign(ref gPROSPECT_LIST_ID, gID, sNAME, sSQL);
					Response.Redirect("~/ProspectLists/view.aspx?ID=" + gPROSPECT_LIST_ID.ToString());
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
			gID = Sql.ToGuid(Request["ID"]);

			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL;
				// 04/26/2008 Paul.  Build the list of fields to use in the select clause.
				sSQL = "select " + Sql.FormatSelectFields(arrSelectFields)
				     + "  from vwCAMPAIGN_LOG_TrackSendError" + ControlChars.CrLf
				     + " where 1 = 1                        " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AppendParameter(cmd, gID, "CAMPAIGN_ID");
					// 04/26/2008 Paul.  Move Last Sort to the database.
					cmd.CommandText += grdMain.OrderByClause("ACTIVITY_DATE", "asc");

					if ( bDebug )
						RegisterClientScriptBlock("vwCAMPAIGN_LOG_TrackSendError", Sql.ClientScriptBlock(cmd));

					try
					{
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(dt);
								// 03/07/2013 Paul.  Apply business rules to subpanel. 
								this.ApplyGridViewRules(m_sMODULE + ".TrackSendError", dt);
								vwMain = dt.DefaultView;
								grdMain.DataSource = vwMain ;
								// 09/05/2005 Paul. LinkButton controls will not fire an event unless the the grid is bound. 
								// 04/25/2008 Paul.  Enable sorting of sub panel. 
								// 04/26/2008 Paul.  Move Last Sort to the database.
								grdMain.DataBind();
							}
						}
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
						ctlDynamicButtons.ErrorText = ex.Message;
					}
				}
			}
			if ( !IsPostBack )
			{
				// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
				// 08/22/2012 Paul.  Allow prospect list to be created from the tracked users. 
				Guid gASSIGNED_USER_ID = Sql.ToGuid(Page.Items["ASSIGNED_USER_ID"]);
				ctlDynamicButtons.AppendButtons(m_sMODULE + ".TrackSendError", gASSIGNED_USER_ID, gID);
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
			m_sMODULE = "Campaigns";
			// 04/26/2008 Paul.  We need to build a list of the fields used by the search clause. 
			arrSelectFields = new UniqueStringCollection();
			arrSelectFields.Add("CAMPAIGN_ID"  );
			arrSelectFields.Add("ACTIVITY_DATE");
			// 11/26/2005 Paul.  Add fields early so that sort events will get called. 
			// 03/18/2010 Paul.  Use separate views for each tracker panel. 
			this.AppendGridColumns(grdMain, m_sMODULE + ".TrackSendError", arrSelectFields);
			// 08/22/2012 Paul.  Allow prospect list to be created from the tracked users. 
			if ( IsPostBack )
				ctlDynamicButtons.AppendButtons(m_sMODULE + ".TrackSendError", Guid.Empty, Guid.Empty);
		}
		#endregion
	}
}

