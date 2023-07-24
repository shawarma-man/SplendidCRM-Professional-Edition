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

namespace SplendidCRM.Contacts
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

		// 05/06/2010 Paul.  We need a common way to attach a command from the Toolbar. 

		public Guid ACCOUNT_ID
		{
			get
			{
				// 02/21/2010 Paul.  An EditView Inline will use the ViewState, and a NewRecord Inline will use the Request. 
				Guid gACCOUNT_ID = Sql.ToGuid(ViewState["ACCOUNT_ID"]);
				if ( Sql.IsEmptyGuid(gACCOUNT_ID) )
					gACCOUNT_ID = Sql.ToGuid(Request["ACCOUNT_ID"]);
				return gACCOUNT_ID;
			}
			set
			{
				ViewState["ACCOUNT_ID"] = value;
			}
		}

		// 08/07/2015 Paul.  Add Leads/Contacts relationship. 
		public Guid LEAD_ID
		{
			get
			{
				// 02/21/2010 Paul.  An EditView Inline will use the ViewState, and a NewRecord Inline will use the Request. 
				Guid gLEAD_ID = Sql.ToGuid(ViewState["LEAD_ID"]);
				if ( Sql.IsEmptyGuid(gLEAD_ID) )
					gLEAD_ID = Sql.ToGuid(Request["LEAD_ID"]);
				return gLEAD_ID;
			}
			set
			{
				ViewState["LEAD_ID"] = value;
			}
		}

		// 03/13/2014 Paul.  Make sure to pass Reports To from Direct Reports subpanel. 
		public Guid REPORTS_TO_ID
		{
			get
			{
				Guid gREPORTS_TO_ID = Sql.ToGuid(ViewState["REPORTS_TO_ID"]);
				if ( Sql.IsEmptyGuid(gREPORTS_TO_ID) )
					gREPORTS_TO_ID = Sql.ToGuid(Request["REPORTS_TO_ID"]);
				return gREPORTS_TO_ID;
			}
			set
			{
				ViewState["REPORTS_TO_ID"] = value;
			}
		}

		// 05/05/2010 Paul.  We need a common way to access the parent from the Toolbar. 

		public Guid CALL_ID
		{
			get { return Sql.ToGuid(ViewState["CALL_ID"]); }
			set { ViewState["CALL_ID"] = value; }
		}

		public Guid MEETING_ID
		{
			get { return Sql.ToGuid(ViewState["MEETING_ID"]); }
			set { ViewState["MEETING_ID"] = value; }
		}

		// 04/20/2010 Paul.  Add functions to allow this control to be used as part of an InlineEdit operation. 
		public override bool IsEmpty()
		{
			string sNAME = new DynamicControl(this, "LAST_NAME").Text;
			return Sql.IsEmptyString(sNAME);
		}

		public override void ValidateEditViewFields()
		{
			if ( !IsEmpty() )
			{
				this.ValidateEditViewFields(m_sMODULE + "." + sEditView);
				// 10/20/2011 Paul.  Apply Business Rules to NewRecord. 
				this.ApplyEditViewValidationEventRules(m_sMODULE + "." + sEditView);
			}
		}

		public override void Save(Guid gPARENT_ID, string sPARENT_TYPE, IDbTransaction trn)
		{
			if ( IsEmpty() )
				return;
			
			string    sTABLE_NAME    = Crm.Modules.TableName(m_sMODULE);
			DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sTABLE_NAME);
			
			Guid gASSIGNED_USER_ID = new DynamicControl(this, "ASSIGNED_USER_ID").ID;
			Guid gTEAM_ID          = new DynamicControl(this, "TEAM_ID"         ).ID;
			Guid gACCOUNT_ID       = new DynamicControl(this, "ACCOUNT_ID"      ).ID;
			Guid gCALL_ID          = this.CALL_ID   ;
			Guid gMEETING_ID       = this.MEETING_ID;
			if ( Sql.IsEmptyGuid(gASSIGNED_USER_ID) )
				gASSIGNED_USER_ID = Security.USER_ID;
			if ( Sql.IsEmptyGuid(gTEAM_ID) )
				gTEAM_ID = Security.TEAM_ID;
			if ( Sql.IsEmptyGuid(gACCOUNT_ID) )
				gACCOUNT_ID = this.ACCOUNT_ID;
			if ( sPARENT_TYPE == "Accounts" && !Sql.IsEmptyGuid(gPARENT_ID) )
				gACCOUNT_ID = gPARENT_ID;
			// 03/13/2014 Paul.  Make sure to pass Reports To from Direct Reports subpanel. 
			Guid gREPORTS_TO_ID    = new DynamicControl(this, "REPORTS_TO_ID"   ).ID;
			if ( Sql.IsEmptyGuid(gREPORTS_TO_ID) )
				gREPORTS_TO_ID = this.REPORTS_TO_ID;

			// 08/07/2015 Paul.  Add Leads/Contacts relationship. 
			Guid gLEAD_ID          = new DynamicControl(this, "LEAD_ID"         ).ID;
			if ( Sql.IsEmptyGuid(gLEAD_ID) )
				gLEAD_ID = this.LEAD_ID;
			if ( sPARENT_TYPE == "Leads" && !Sql.IsEmptyGuid(gPARENT_ID) )
				gLEAD_ID = gPARENT_ID;

			SqlProcs.spCONTACTS_Update
				( ref gID
				, gASSIGNED_USER_ID
				, new DynamicControl(this, "SALUTATION"                ).SelectedValue
				, new DynamicControl(this, "FIRST_NAME"                ).Text
				, new DynamicControl(this, "LAST_NAME"                 ).Text
				, gACCOUNT_ID
				, new DynamicControl(this, "LEAD_SOURCE"               ).SelectedValue
				, new DynamicControl(this, "TITLE"                     ).Text
				, new DynamicControl(this, "DEPARTMENT"                ).Text
				, gREPORTS_TO_ID
				, new DynamicControl(this, "BIRTHDATE"                 ).DateValue
				, new DynamicControl(this, "DO_NOT_CALL"               ).Checked
				, new DynamicControl(this, "PHONE_HOME"                ).Text
				, new DynamicControl(this, "PHONE_MOBILE"              ).Text
				, new DynamicControl(this, "PHONE_WORK"                ).Text
				, new DynamicControl(this, "PHONE_OTHER"               ).Text
				, new DynamicControl(this, "PHONE_FAX"                 ).Text
				, new DynamicControl(this, "EMAIL1"                    ).Text
				, new DynamicControl(this, "EMAIL2"                    ).Text
				, new DynamicControl(this, "ASSISTANT"                 ).Text
				, new DynamicControl(this, "ASSISTANT_PHONE"           ).Text
				, new DynamicControl(this, "EMAIL_OPT_OUT"             ).Checked
				, new DynamicControl(this, "INVALID_EMAIL"             ).Checked
				, new DynamicControl(this, "PRIMARY_ADDRESS_STREET"    ).Text
				, new DynamicControl(this, "PRIMARY_ADDRESS_CITY"      ).Text
				, new DynamicControl(this, "PRIMARY_ADDRESS_STATE"     ).Text
				, new DynamicControl(this, "PRIMARY_ADDRESS_POSTALCODE").Text
				, new DynamicControl(this, "PRIMARY_ADDRESS_COUNTRY"   ).Text
				, new DynamicControl(this, "ALT_ADDRESS_STREET"        ).Text
				, new DynamicControl(this, "ALT_ADDRESS_CITY"          ).Text
				, new DynamicControl(this, "ALT_ADDRESS_STATE"         ).Text
				, new DynamicControl(this, "ALT_ADDRESS_POSTALCODE"    ).Text
				, new DynamicControl(this, "ALT_ADDRESS_COUNTRY"       ).Text
				, new DynamicControl(this, "DESCRIPTION"               ).Text
				, sPARENT_TYPE
				, gPARENT_ID
				, new DynamicControl(this, "SYNC_CONTACT"              ).Checked
				, gTEAM_ID
				, new DynamicControl(this, "TEAM_SET_LIST"             ).Text
				// 09/27/2013 Paul.  SMS messages need to be opt-in. 
				, new DynamicControl(this, "SMS_OPT_IN"                ).SelectedValue
				// 10/22/2013 Paul.  Provide a way to map Tweets to a parent. 
				, new DynamicControl(this, "TWITTER_SCREEN_NAME"       ).Text
				// 08/07/2015 Paul.  Add picture. 
				, new DynamicControl(this, "PICTURE"                   ).Text
				// 08/07/2015 Paul.  Add Leads/Contacts relationship. 
				, gLEAD_ID
				// 09/27/2015 Paul.  Separate SYNC_CONTACT and EXCHANGE_FOLDER. 
				, new DynamicControl(this, "EXCHANGE_FOLDER"           ).Checked
				// 05/12/2016 Paul.  Add Tags module. 
				, new DynamicControl(this, "TAG_SET_NAME"              ).Text
				// 06/20/2017 Paul.  Add number fields to Contacts, Leads, Prospects, Opportunities and Campaigns. 
				, new DynamicControl(this, "CONTACT_NUMBER"            ).Text
				// 11/30/2017 Paul.  Add ASSIGNED_SET_ID for Dynamic User Assignment. 
				, new DynamicControl(this, "ASSIGNED_SET_LIST"         ).Text
				// 06/23/2018 Paul.  Add DP_BUSINESS_PURPOSE and DP_CONSENT_LAST_UPDATED for data privacy. 
				, new DynamicControl(this, "DP_BUSINESS_PURPOSE"       ).Text
				, new DynamicControl(this, "DP_CONSENT_LAST_UPDATED"   ).DateValue
				, trn
				);
			SplendidDynamic.UpdateCustomFields(this, trn, gID, sTABLE_NAME, dtCustomFields);
			
			// 02/21/2010 Paul.  Use separate request fields when creating a contact from a call or a meeting. 
			if ( !Sql.IsEmptyGuid(gCALL_ID) )
				SqlProcs.spCALLS_CONTACTS_Update(gCALL_ID, gID, false, String.Empty, trn);
			if ( !Sql.IsEmptyGuid(gMEETING_ID) )
				SqlProcs.spMEETINGS_CONTACTS_Update(gMEETING_ID, gID, false, String.Empty, trn);
			// 04/20/2010 Paul.  For those procedures that do not include a PARENT_TYPE, 
			// we need a new relationship procedure. 
			SqlProcs.spCONTACTS_InsRelated(gID, sPARENT_TYPE, gPARENT_ID, trn);
		}

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "NewRecord" )
				{
					// 06/20/2009 Paul.  Use a Dynamic View that is nearly idential to the EditView version. 
					this.ValidateEditViewFields(m_sMODULE + "." + sEditView);
					// 10/20/2011 Paul.  Apply Business Rules to NewRecord. 
					this.ApplyEditViewValidationEventRules(m_sMODULE + "." + sEditView);
					if ( Page.IsValid )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							con.Open();
							// 10/20/2011 Paul.  Apply Business Rules to NewRecord. 
							this.ApplyEditViewPreSaveEventRules(m_sMODULE + "." + sEditView, null);
							
							// 10/07/2009 Paul.  We need to create our own global transaction ID to support auditing and workflow on SQL Azure, PostgreSQL, Oracle, DB2 and MySQL. 
							using ( IDbTransaction trn = Sql.BeginTransaction(con) )
							{
								try
								{
									Guid   gPARENT_ID   = new DynamicControl(this, "PARENT_ID"  ).ID;
									// 02/04/2011 Paul.  We gave the PARENT_TYPE a unique name, but we need to update all EditViews and NewRecords. 
									string sPARENT_TYPE = new DynamicControl(this, "PARENT_ID_PARENT_TYPE").SelectedValue;
									if ( Sql.IsEmptyGuid(gPARENT_ID) )
										gPARENT_ID = this.PARENT_ID;
									// 07/14/2010 Paul.  We should be checking the sPARENT_TYPE value and not the ViewState value. 
									if ( Sql.IsEmptyString(sPARENT_TYPE) )
										sPARENT_TYPE = this.PARENT_TYPE;
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
							// 10/20/2011 Paul.  Apply Business Rules to NewRecord. 
							// 12/10/2012 Paul.  Provide access to the item data. 
							DataRow rowCurrent = Crm.Modules.ItemEdit(m_sMODULE, gID);
							this.ApplyEditViewPostSaveEventRules(m_sMODULE + "." + sEditView, rowCurrent);
						}
						if ( !Sql.IsEmptyString(RulesRedirectURL) )
							Response.Redirect(RulesRedirectURL);
						// 02/21/2010 Paul.  An error should not forward the command so that the error remains. 
						// In case of success, send the command so that the page can be rebuilt. 
						// 06/02/2010 Paul.  We need a way to pass the ID up the command chain. 
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
			// 06/04/2006 Paul.  NewRecord should not be displayed if the user does not have edit rights. 
			// 01/02/2020 Paul.  Allow the NewRecord to be disabled per module using config table. 
			this.Visible = (!Sql.ToBoolean(Application["CONFIG." + m_sMODULE + ".DisableNewRecord"]) || sEditView != "NewRecord") && (SplendidCRM.Security.GetUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				// 05/06/2010 Paul.  Use a special Page flag to override the default IsPostBack behavior. 
				bool bIsPostBack = this.IsPostBack && !NotPostBack;
				if ( !bIsPostBack )
				{
					// 05/06/2010 Paul.  When the control is created out-of-band, we need to manually bind the controls. 
					if ( NotPostBack )
						this.DataBind();
					// 02/21/2010 Paul.  When used in a SubPanel, this line does not get executed because 
					// the Page_Load happens after the user as clicked Create, which is a PostBack event. 
					this.AppendEditViewFields(m_sMODULE + "." + sEditView, tblMain, null, ctlFooterButtons.ButtonClientID("NewRecord"));
					// 07/02/2018 Paul.  Allow defaults to display as checked for Opt Out and Do Not Call. 
					new DynamicControl(this, "EMAIL_OPT_OUT").Checked = Sql.ToBoolean(Application["CONFIG.default_email_opt_out"]);
					new DynamicControl(this, "DO_NOT_CALL"  ).Checked = Sql.ToBoolean(Application["CONFIG.default_do_not_call"  ]);
					// 06/04/2010 Paul.  Notify the parent that the fields have been loaded. 
					if ( EditViewLoad != null )
						EditViewLoad(this, null);
					
					// 02/21/2010 Paul.  When the Full Form buttons are used, we don't want the panel to have margins. 
					if ( bShowFullForm || bShowCancel || sEditView != "NewRecord" )
					{
						pnlMain.CssClass = "";
						pnlEdit.CssClass = "tabForm";
						
						Guid gPARENT_ID = this.ACCOUNT_ID;
						// 05/05/2010 Paul.  The Toolbar will only set the Parent, so we need to populate with this value. 
						// 04/03/2013 Paul.  Simplify the logic so that there is only one code path to initialize the account data. 
						if ( Sql.IsEmptyGuid(gPARENT_ID) )
							gPARENT_ID = this.PARENT_ID;
						if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						{
							// 04/14/2016 Paul.  New spPARENT_GetWithTeam procedure so that we can inherit Assigned To and Team values. 
							string sMODULE           = String.Empty;
							string sPARENT_TYPE      = String.Empty;
							string sPARENT_NAME      = String.Empty;
							Guid   gASSIGNED_USER_ID = Guid.Empty;
							string sASSIGNED_TO      = String.Empty;
							string sASSIGNED_TO_NAME = String.Empty;
							Guid   gTEAM_ID          = Guid.Empty;
							string sTEAM_NAME        = String.Empty;
							Guid   gTEAM_SET_ID      = Guid.Empty;
							// 11/30/2017 Paul.  Add ASSIGNED_SET_ID for Dynamic User Assignment. 
							Guid   gASSIGNED_SET_ID  = Guid.Empty;
							SqlProcs.spPARENT_GetWithTeam(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME, ref gASSIGNED_USER_ID, ref sASSIGNED_TO, ref sASSIGNED_TO_NAME, ref gTEAM_ID, ref sTEAM_NAME, ref gTEAM_SET_ID, ref gASSIGNED_SET_ID);
							if ( !Sql.IsEmptyGuid(gPARENT_ID) )
							{
								if ( sPARENT_TYPE == "Accounts" )
								{
									new DynamicControl(this, "ACCOUNT_ID"  ).ID   = gPARENT_ID;
									new DynamicControl(this, "ACCOUNT_NAME").Text = sPARENT_NAME;
									// 04/14/2016 Paul.  New spPARENT_GetWithTeam procedure so that we can inherit Assigned To and Team values. 
									if ( Sql.ToBoolean(Application["CONFIG.inherit_assigned_user"]) )
									{
										new DynamicControl(this, "ASSIGNED_USER_ID").ID   = gASSIGNED_USER_ID;
										new DynamicControl(this, "ASSIGNED_TO"     ).Text = sASSIGNED_TO     ;
										new DynamicControl(this, "ASSIGNED_TO_NAME").Text = sASSIGNED_TO_NAME;
										// 11/30/2017 Paul.  Add ASSIGNED_SET_ID for Dynamic User Assignment. 
										if ( Crm.Config.enable_dynamic_assignment() )
										{
											SplendidCRM._controls.UserSelect ctlUserSelect = FindControl("ASSIGNED_SET_NAME") as SplendidCRM._controls.UserSelect;
											if ( ctlUserSelect != null )
												ctlUserSelect.LoadLineItems(gASSIGNED_SET_ID, true, true);
										}
									}
									if ( Sql.ToBoolean(Application["CONFIG.inherit_team"]) )
									{
										new DynamicControl(this, "TEAM_ID"  ).ID   = gTEAM_ID  ;
										new DynamicControl(this, "TEAM_NAME").Text = sTEAM_NAME;
										SplendidCRM._controls.TeamSelect ctlTeamSelect = FindControl("TEAM_SET_NAME") as SplendidCRM._controls.TeamSelect;
										if ( ctlTeamSelect != null )
											ctlTeamSelect.LoadLineItems(gTEAM_SET_ID, true, true);
									}
									// 10/07/2010 Paul.  Populate with full address information. 
									DbProviderFactory dbf = DbProviderFactories.GetFactory();
									using ( IDbConnection con = dbf.CreateConnection() )
									{
										string sSQL ;
										sSQL = "select *              " + ControlChars.CrLf
										     + "  from vwACCOUNTS_Edit" + ControlChars.CrLf;
										using ( IDbCommand cmd = con.CreateCommand() )
										{
											cmd.CommandText = sSQL;
											Security.Filter(cmd, "Accounts", "view");
											Sql.AppendParameter(cmd, gPARENT_ID, "ID", false);
											con.Open();

											if ( bDebug )
												RegisterClientScriptBlock("vwACCOUNTS_Edit", Sql.ClientScriptBlock(cmd));

											using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
											{
												if ( rdr.Read() )
												{
													new DynamicControl(this, "PRIMARY_ADDRESS_STREET"     ).Text = Sql.ToString(rdr["BILLING_ADDRESS_STREET"     ]);
													new DynamicControl(this, "PRIMARY_ADDRESS_CITY"       ).Text = Sql.ToString(rdr["BILLING_ADDRESS_CITY"       ]);
													new DynamicControl(this, "PRIMARY_ADDRESS_STATE"      ).Text = Sql.ToString(rdr["BILLING_ADDRESS_STATE"      ]);
													new DynamicControl(this, "PRIMARY_ADDRESS_POSTALCODE" ).Text = Sql.ToString(rdr["BILLING_ADDRESS_POSTALCODE" ]);
													new DynamicControl(this, "PRIMARY_ADDRESS_COUNTRY"    ).Text = Sql.ToString(rdr["BILLING_ADDRESS_COUNTRY"    ]);
													// 10/26/2010 Paul.  Fix spelling of SHIPPING. 
													new DynamicControl(this, "ALT_ADDRESS_STREET"     ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_STREET"     ]);
													new DynamicControl(this, "ALT_ADDRESS_CITY"       ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_CITY"       ]);
													new DynamicControl(this, "ALT_ADDRESS_STATE"      ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_STATE"      ]);
													new DynamicControl(this, "ALT_ADDRESS_POSTALCODE" ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_POSTALCODE" ]);
													new DynamicControl(this, "ALT_ADDRESS_COUNTRY"    ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_COUNTRY"    ]);
													// 04/03/2013 Paul.  A customer suggested that we copy phone numbers. 
													new DynamicControl(this, "PHONE_WORK"             ).Text = Sql.ToString(rdr["PHONE_OFFICE"                ]);
													new DynamicControl(this, "PHONE_FAX"              ).Text = Sql.ToString(rdr["PHONE_FAX"                   ]);
													new DynamicControl(this, "PHONE_OTHER"            ).Text = Sql.ToString(rdr["PHONE_ALTERNATE"             ]);
												}
											}
										}
									}
								}
							}
						}
					}
					// 10/20/2011 Paul.  Apply Business Rules to NewRecord. 
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
			m_sMODULE = "Contacts";
			// 05/06/2010 Paul.  Use a special Page flag to override the default IsPostBack behavior. 
			bool bIsPostBack = this.IsPostBack && !NotPostBack;
			if ( bIsPostBack )
			{
				this.AppendEditViewFields(m_sMODULE + "." + sEditView, tblMain, null, ctlFooterButtons.ButtonClientID("NewRecord"));
				// 06/04/2010 Paul.  Notify the parent that the fields have been loaded. 
				if ( EditViewLoad != null )
					EditViewLoad(this, null);
				// 10/20/2011 Paul.  Apply Business Rules to NewRecord. 
				Page.Validators.Add(new RulesValidator(this));
			}
		}
		#endregion
	}
}

