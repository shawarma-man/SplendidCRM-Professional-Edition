/*
 * Copyright (C) 2005-2023 SplendidCRM Software, Inc. All rights reserved.
 *
 * Any use of the contents of this file are subject to the SplendidCRM Professional Source Code License 
 * Agreement, or other written agreement between you and SplendidCRM ("License"). By installing or 
 * using this file, you have unconditionally agreed to the terms and conditions of the License, 
 * including but not limited to restrictions on the number of users therein, and you may not use this 
 * file except in compliance with the License. 
 * 
 */

function DynamicButtons_LoadAllLayouts(callback, context)
{
	var xhr = SystemCacheRequestAll('GetAllDynamicButtons');
	xhr.onreadystatechange = function()
	{
		if ( xhr.readyState == 4 )
		{
			GetSplendidResult(xhr, function(result)
			{
				try
				{
					if ( result.status == 200 )
					{
						if ( result.d !== undefined )
						{
							//alert(dumpObj(result.d, 'd'));
							DYNAMIC_BUTTONS = result.d.results;
							callback.call(context||this, 1, null);
						}
						else
						{
							callback.call(context||this, -1, xhr.responseText);
						}
					}
					else
					{
						if ( result.ExceptionDetail !== undefined )
							callback.call(context||this, -1, result.ExceptionDetail.Message);
						else
							callback.call(context||this, -1, xhr.responseText);
					}
				}
				catch(e)
				{
					callback.call(context||this, -1, SplendidError.FormatError(e, 'DynamicButtons_LoadAllLayouts'));
				}
			});
		}
	}
	try
	{
		xhr.send();
	}
	catch(e)
	{
		// 03/28/2012 Paul.  IE9 is returning -2146697208 when working offline. 
		if ( e.number != -2146697208 )
			callback.call(context||this, -1, SplendidError.FormatError(e, 'DynamicButtons_LoadAllLayouts'));
	}
}

function DynamicButtons_LoadLayout(sVIEW_NAME, callback, context)
{
	// 06/11/2012 Paul.  Wrap System Cache requests for Cordova. 
	var xhr = SystemCacheRequest('DYNAMIC_BUTTONS', 'CONTROL_INDEX asc', null, 'VIEW_NAME', sVIEW_NAME, true);
	//var xhr = CreateSplendidRequest('Rest.svc/GetModuleTable?TableName=DYNAMIC_BUTTONS&$orderby=CONTROL_INDEX asc&$filter=' + encodeURIComponent('(VIEW_NAME eq \'' + sVIEW_NAME + '\' and DEFAULT_VIEW eq 0)'), 'GET');
	xhr.onreadystatechange = function()
	{
		if ( xhr.readyState == 4 )
		{
			GetSplendidResult(xhr, function(result)
			{
				try
				{
					if ( result.status == 200 )
					{
						if ( result.d !== undefined )
						{
							SplendidCache.SetDynamicButtons(sVIEW_NAME, result.d.results);
							// 10/03/2011 Paul.  DynamicButtons_LoadLayout returns the layout. 
							var layout = SplendidCache.DynamicButtons(sVIEW_NAME);
							callback.call(context||this, 1, layout);
						}
						else
						{
							callback.call(context||this, -1, xhr.responseText);
						}
					}
					else
					{
						if ( result.ExceptionDetail !== undefined )
							callback.call(context||this, -1, result.ExceptionDetail.Message);
						else
							callback.call(context||this, -1, xhr.responseText);
					}
				}
				catch(e)
				{
					callback.call(context||this, -1, SplendidError.FormatError(e, 'DynamicButtons_LoadLayout'));
				}
			});
		}
	}
	try
	{
		// 10/03/2011 Paul.  DynamicButtons_LoadLayout returns the layout. 
		var layout = SplendidCache.DynamicButtons(sVIEW_NAME);
		if ( layout == null )
		{
			xhr.send();
		}
		else
		{
			callback.call(context||this, 1, layout);
		}
	}
	catch(e)
	{
		// 03/28/2012 Paul.  IE9 is returning -2146697208 when working offline. 
		if ( e.number != -2146697208 )
			callback.call(context||this, -1, SplendidError.FormatError(e, 'DynamicButtons_LoadLayout'));
	}
}

