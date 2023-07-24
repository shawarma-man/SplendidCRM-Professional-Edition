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

function ACCOUNTS_ACCOUNT_NAME_Changed(fldACCOUNT_NAME)
{
	// 02/04/2007 Paul.  We need to have an easy way to locate the correct text fields, 
	// so use the current field to determine the label prefix and send that in the userContact field. 
	// 08/24/2009 Paul.  One of the base controls can contain NAME in the text, so just get the length minus 4. 
	var userContext = fldACCOUNT_NAME.id.substring(0, fldACCOUNT_NAME.id.length - 'ACCOUNT_NAME'.length)
	var fldAjaxErrors = document.getElementById(userContext + 'ACCOUNT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';
	
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_ACCOUNT_NAME');
	if ( fldPREV_ACCOUNT_NAME == null )
	{
		//alert('Could not find ' + userContext + 'PREV_ACCOUNT_NAME');
	}
	else if ( fldPREV_ACCOUNT_NAME.value != fldACCOUNT_NAME.value )
	{
		if ( fldACCOUNT_NAME.value.length > 0 )
		{
			try
			{
				SplendidCRM.Accounts.AutoComplete.ACCOUNTS_ACCOUNT_NAME_Get(fldACCOUNT_NAME.value, ACCOUNTS_ACCOUNT_NAME_Changed_OnSucceededWithContext, ACCOUNTS_ACCOUNT_NAME_Changed_OnFailed, userContext);
			}
			catch(e)
			{
				alert('ACCOUNTS_ACCOUNT_NAME_Changed: ' + e.message);
			}
		}
		else
		{
			// 08/30/2010 Paul.  If the name was cleared, then we must also clear the hidden ID field. 
			var result = { 'ID' : '', 'NAME' : '' };
			ACCOUNTS_ACCOUNT_NAME_Changed_OnSucceededWithContext(result, userContext, null);
		}
	}
}

function ACCOUNTS_ACCOUNT_NAME_Changed_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID   = result.ID  ;
		var sNAME = result.NAME;
		
		var fldAjaxErrors        = document.getElementById(userContext + 'ACCOUNT_NAME_AjaxErrors');
		var fldACCOUNT_ID        = document.getElementById(userContext + 'ACCOUNT_ID'       );
		var fldACCOUNT_NAME      = document.getElementById(userContext + 'ACCOUNT_NAME'     );
		var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_ACCOUNT_NAME');
		if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = sID  ;
		if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = sNAME;
		if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = sNAME;
		// 03/24/2011 Paul.  If an Update button is available, then click it. 
		var fldACCOUNT_UPDATE = document.getElementById(userContext + 'ACCOUNT_UPDATE');
		if ( fldACCOUNT_UPDATE != null )
			fldACCOUNT_UPDATE.click();
	}
	else
	{
		alert('result from Accounts.AutoComplete service is null');
	}
}

function ACCOUNTS_ACCOUNT_NAME_Changed_OnFailed(error, userContext)
{
	// Display the error.
	var fldAjaxErrors = document.getElementById(userContext + 'ACCOUNT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '<br />' + error.get_message();

	var fldACCOUNT_ID        = document.getElementById(userContext + 'ACCOUNT_ID'       );
	var fldACCOUNT_NAME      = document.getElementById(userContext + 'ACCOUNT_NAME'     );
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_ACCOUNT_NAME');
	if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = '';
	if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = '';
	if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = '';
}

// 07/27/2010 Paul.  Allow Account lookup from Quotes, Orders or Invoices. 
function ACCOUNTS_BILLING_ACCOUNT_NAME_Changed(fldACCOUNT_NAME)
{
	var userContext = fldACCOUNT_NAME.id.substring(0, fldACCOUNT_NAME.id.length - 'BILLING_ACCOUNT_NAME'.length)
	var fldAjaxErrors = document.getElementById(userContext + 'BILLING_ACCOUNT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';
	
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_BILLING_ACCOUNT_NAME');
	if ( fldPREV_ACCOUNT_NAME == null )
	{
		//alert('Could not find ' + userContext + 'PREV_ACCOUNT_NAME');
	}
	else if ( fldPREV_ACCOUNT_NAME.value != fldACCOUNT_NAME.value )
	{
		if ( fldACCOUNT_NAME.value.length > 0 )
		{
			try
			{
				SplendidCRM.Accounts.AutoComplete.ACCOUNTS_ACCOUNT_NAME_Get(fldACCOUNT_NAME.value, ACCOUNTS_BILLING_ACCOUNT_NAME_Changed_OnSucceededWithContext, ACCOUNTS_BILLING_ACCOUNT_NAME_Changed_OnFailed, userContext);
			}
			catch(e)
			{
				alert('ACCOUNTS_BILLING_ACCOUNT_NAME_Changed: ' + e.message);
			}
		}
		else
		{
			// 08/30/2010 Paul.  If the name was cleared, then we must also clear the hidden ID field. 
			var result = { 'ID' : '', 'NAME' : '' };
			ACCOUNTS_BILLING_ACCOUNT_NAME_Changed_OnSucceededWithContext(result, userContext, null);
		}
	}
}

function ACCOUNTS_BILLING_ACCOUNT_NAME_Changed_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID   = result.ID  ;
		var sNAME = result.NAME;
		
		var fldAjaxErrors        = document.getElementById(userContext + 'BILLING_ACCOUNT_NAME_AjaxErrors');
		var fldACCOUNT_ID        = document.getElementById(userContext + 'BILLING_ACCOUNT_ID'  );
		var fldACCOUNT_NAME      = document.getElementById(userContext + 'BILLING_ACCOUNT_NAME');
		var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_BILLING_ACCOUNT');
		if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = sID  ;
		if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = sNAME;
		if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = sNAME;
		// 07/27/2010 Paul.  We typically submit the form when the account changes so that we can load the address. 
		// 08/21/2010 Paul.  If an Update button is available, then click it. 
		var fldBILLING_ACCOUNT_UPDATE = document.getElementById(userContext + 'BILLING_ACCOUNT_UPDATE');
		if ( fldBILLING_ACCOUNT_UPDATE != null )
			fldBILLING_ACCOUNT_UPDATE.click();
	}
	else
	{
		alert('result from Accounts.AutoComplete service is null');
	}
}

function ACCOUNTS_BILLING_ACCOUNT_NAME_Changed_OnFailed(error, userContext)
{
	// Display the error.
	var fldAjaxErrors = document.getElementById(userContext + 'BILLING_ACCOUNT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '<br />' + error.get_message();

	var fldACCOUNT_ID        = document.getElementById(userContext + 'BILLING_ACCOUNT_ID'       );
	var fldACCOUNT_NAME      = document.getElementById(userContext + 'BILLING_ACCOUNT_NAME'     );
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_BILLING_ACCOUNT_NAME');
	if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = '';
	if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = '';
	if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = '';
}

function ACCOUNTS_SHIPPING_ACCOUNT_NAME_Changed(fldACCOUNT_NAME)
{
	var userContext = fldACCOUNT_NAME.id.substring(0, fldACCOUNT_NAME.id.length - 'SHIPPING_ACCOUNT_NAME'.length)
	var fldAjaxErrors = document.getElementById(userContext + 'SHIPPING_ACCOUNT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';
	
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_SHIPPING_ACCOUNT_NAME');
	if ( fldPREV_ACCOUNT_NAME == null )
	{
		//alert('Could not find ' + userContext + 'PREV_ACCOUNT_NAME');
	}
	else if ( fldPREV_ACCOUNT_NAME.value != fldACCOUNT_NAME.value )
	{
		if ( fldACCOUNT_NAME.value.length > 0 )
		{
			try
			{
				SplendidCRM.Accounts.AutoComplete.ACCOUNTS_ACCOUNT_NAME_Get(fldACCOUNT_NAME.value, ACCOUNTS_SHIPPING_ACCOUNT_NAME_Changed_OnSucceededWithContext, ACCOUNTS_SHIPPING_ACCOUNT_NAME_Changed_OnFailed, userContext);
			}
			catch(e)
			{
				alert('ACCOUNTS_SHIPPING_ACCOUNT_NAME_Changed: ' + e.message);
			}
		}
		else
		{
			// 08/30/2010 Paul.  If the name was cleared, then we must also clear the hidden ID field. 
			var result = { 'ID' : '', 'NAME' : '' };
			ACCOUNTS_SHIPPING_ACCOUNT_NAME_Changed_OnSucceededWithContext(result, userContext, null);
		}
	}
}

function ACCOUNTS_SHIPPING_ACCOUNT_NAME_Changed_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID   = result.ID  ;
		var sNAME = result.NAME;
		
		var fldAjaxErrors        = document.getElementById(userContext + 'SHIPPING_ACCOUNT_NAME_AjaxErrors');
		var fldACCOUNT_ID        = document.getElementById(userContext + 'SHIPPING_ACCOUNT_ID'  );
		var fldACCOUNT_NAME      = document.getElementById(userContext + 'SHIPPING_ACCOUNT_NAME');
		var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_SHIPPING_ACCOUNT');
		if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = sID  ;
		if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = sNAME;
		if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = sNAME;
		// 07/27/2010 Paul.  We typically submit the form when the account changes so that we can load the address. 
		// 08/21/2010 Paul.  If an Update button is available, then click it. 
		var fldSHIPPING_ACCOUNT_UPDATE = document.getElementById(userContext + 'SHIPPING_ACCOUNT_UPDATE');
		if ( fldSHIPPING_ACCOUNT_UPDATE != null )
			fldSHIPPING_ACCOUNT_UPDATE.click();
	}
	else
	{
		alert('result from Accounts.AutoComplete service is null');
	}
}

function ACCOUNTS_SHIPPING_ACCOUNT_NAME_Changed_OnFailed(error, userContext)
{
	// Display the error.
	var fldAjaxErrors = document.getElementById(userContext + 'SHIPPING_ACCOUNT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '<br />' + error.get_message();

	var fldACCOUNT_ID        = document.getElementById(userContext + 'SHIPPING_ACCOUNT_ID'       );
	var fldACCOUNT_NAME      = document.getElementById(userContext + 'SHIPPING_ACCOUNT_NAME'     );
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_SHIPPING_ACCOUNT_NAME');
	if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = '';
	if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = '';
	if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = '';
}

// 07/28/2010 Paul.  We need this function in order to allow AutoComplete in the Accounts Parent field. 
function ACCOUNTS_PARENT_NAME_Changed(fldACCOUNT_NAME)
{
	var userContext = fldACCOUNT_NAME.id.substring(0, fldACCOUNT_NAME.id.length - 'PARENT_NAME'.length)
	var fldAjaxErrors = document.getElementById(userContext + 'PARENT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';
	
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_PARENT_NAME');
	if ( fldPREV_ACCOUNT_NAME == null )
	{
		//alert('Could not find ' + userContext + 'PREV_ACCOUNT_NAME');
	}
	else if ( fldPREV_ACCOUNT_NAME.value != fldACCOUNT_NAME.value )
	{
		if ( fldACCOUNT_NAME.value.length > 0 )
		{
			try
			{
				SplendidCRM.Accounts.AutoComplete.ACCOUNTS_ACCOUNT_NAME_Get(fldACCOUNT_NAME.value, ACCOUNTS_PARENT_NAME_Changed_OnSucceededWithContext, ACCOUNTS_PARENT_NAME_Changed_OnFailed, userContext);
			}
			catch(e)
			{
				alert('ACCOUNTS_PARENT_NAME_Changed: ' + e.message);
			}
		}
		else
		{
			// 08/30/2010 Paul.  If the name was cleared, then we must also clear the hidden ID field. 
			var result = { 'ID' : '', 'NAME' : '' };
			ACCOUNTS_PARENT_NAME_Changed_OnSucceededWithContext(result, userContext, null);
		}
	}
}

function ACCOUNTS_PARENT_NAME_Changed_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID   = result.ID  ;
		var sNAME = result.NAME;
		
		var fldAjaxErrors        = document.getElementById(userContext + 'PARENT_NAME_AjaxErrors');
		var fldACCOUNT_ID        = document.getElementById(userContext + 'PARENT_ID'  );
		var fldACCOUNT_NAME      = document.getElementById(userContext + 'PARENT_NAME');
		var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_PARENT');
		if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = sID  ;
		if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = sNAME;
		if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = sNAME;
	}
	else
	{
		alert('result from Accounts.AutoComplete service is null');
	}
}

function ACCOUNTS_PARENT_NAME_Changed_OnFailed(error, userContext)
{
	// Display the error.
	var fldAjaxErrors = document.getElementById(userContext + 'PARENT_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '<br />' + error.get_message();

	var fldACCOUNT_ID        = document.getElementById(userContext + 'PARENT_ID'       );
	var fldACCOUNT_NAME      = document.getElementById(userContext + 'PARENT_NAME'     );
	var fldPREV_ACCOUNT_NAME = document.getElementById(userContext + 'PREV_PARENT_NAME');
	if ( fldACCOUNT_ID        != null ) fldACCOUNT_ID.value        = '';
	if ( fldACCOUNT_NAME      != null ) fldACCOUNT_NAME.value      = '';
	if ( fldPREV_ACCOUNT_NAME != null ) fldPREV_ACCOUNT_NAME.value = '';
}

if ( typeof(Sys) !== 'undefined' )
	Sys.Application.notifyScriptLoaded();


