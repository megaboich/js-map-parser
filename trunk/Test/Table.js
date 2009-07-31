//-------------------------------------------------------------------------
// wwwroot\summary\Controls\Table\Table.js
//--------------------------------------------------------------------------

//This is debug helper - should not to use in production.
function Debugger(message)
{
	if (confirm("debug " + message + "?"))
	{
		debugger;
	}
}

//------------------------------------------------------------------------------------
// This object used to pass parameters from JavaScript to asp.net Server.
// It will deserialize as Dictionary<string, string>
function JSParameters()
{
	this.Add = function(param, value)
	{
		this[param] = value;
	}

	this.GetJSON = function()
	{
		return Object.toJSON(this);
	}
}

//------------------------------------------------------------------------------------
// This object used to organize queue of requests to server.
// Each function that pass to Run method should organize callback.
// And this callback must to call RunQueuedItems method to run other requests from queue.
function QueueRunner(limit)
{
	this.queue = new Array();
	this.param = new Array();
	this.limit = limit + 1;
	this.count = 0;

	// Add the function to queue
	// If queue is empty will direct call and increase the counter
	// fn - function to add to the queue.
	this.Run = function(fn, param)
	{
		++ this.count;
		if (this.count < this.limit)
		{
			fn(param);
		}
		else
		{
			this.queue.push(fn);
			this.param.push(param);
		}
	};

	//Runs next function from queue
	this.RunQueuedItems = function()
	{
		-- this.count
		if (this.queue.length > 0)
		{
			var fn = this.queue.shift();
			var param = this.param.shift();
			fn(param);
		}
	};
}

//------------------------------------------------------------------------------------
// PivotTableControl
// This object is responsible for dynamic operations with Table - ajax handling and rendering
function PivotTableControl()
{
	this.controlId = null;
	this.selectedDimansion = null;
	this.IsManuallyUpdateView = null;
	this.LoadNextNodeRunner = new QueueRunner(3);
	this.ExpandCounter = 0;
	this.Request = null;
	this.IsRequestAborted = false;
	this.OldStateId = null;
	this.NewStateId = null;
	this.NewItemPerPageIndex = null;
	this.RequestGuid = null;
	this.RequestStartedTime = null;
	this.RequestParams = null;
	
	//Storage for collect information about report changes during manually update
	this.ActionInfoStorage = new Array();

	//Initializes the object with values
	//Used in TBD...
	this.Init = function(controlId, selectedDimansion, hideExpand, isManuallyUpdateView) {
	    this.controlId = controlId;
	    this.selectedDimansion = selectedDimansion;
	    this.IsManuallyUpdateView = isManuallyUpdateView;

	    if (typeof (SCaptionHeader) != "undefined" && SCaptionHeader)
	        SCaptionHeader.style.display = "";

	    var xmlTableDataContentObj = document.getElementById("xmlTableData" + controlId);
	    var reportXml = xmlTableDataContentObj.documentElement;
	    if (reportXml == null) {
	        this.InitTableWithControl(xmlTableDataContentObj, controlId, this);
	    }
	    else {
	        initFullHTML(reportXml, false, controlId, this);
	    }
	    this.OldStateId = GetStoredStateId(this.controlId);
	    var conatTableObj = document.getElementById("contentTable" + controlId);
	    conatTableObj.style.visibility = "";
	};

	//Initializes the object with values
	//Used in TBD...
	this.InitTableWithControl = function(mainElement, controlID, thisValue) {
		this.context.IsRequestAborted = false;
		var reportXml = null;
		if (mainElement.XMLDocument != null)
		{
			reportXml = mainElement.XMLDocument.documentElement;
			if (reportXml == null)
			{
				var xmlObj = new ActiveXObject("Microsoft.XMLDOM");
				xmlObj.loadXML(mainElement.XMLDocument.xml);
				reportXml = xmlObj.firstChild;
			}
		}
		else
		{
			var xmlObj = new ActiveXObject("Microsoft.XMLDOM");
			xmlObj.loadXML(mainElement.innerHTML);
			reportXml = xmlObj.firstChild;
		}
		if (reportXml != null)
		{
			initFullHTML(reportXml, false, controlID, thisValue);
		}
	};

	// generates guide string
	this.GetRequestGuid = function() {
		var result = "";
		for (var i = 0; i < 32; i++)
			result += parseInt(Math.random() * 16).toString(16);
		return result;
	}		
	

	// Create and run a request for new data for summary table.
	// PerformAction_Callback will be launched when data is ready.
	this.RunAction = function(params) {

		if (params.storedStateId != null) //used for sub report states: expanding and paging
		{
			var stateId = params.storedStateId;
		}
		else // gets main storedStateId
		{
			var stateId = reportsXML[this.controlId]["storedStateId"];
		}

		switch (params.action) {
			case "ExpandAll":
				var inTQ = true;
				break;
			default:
				var inTQ = false;
				break;
		}

		var contextcontainer = { context: this, clientargs: params };

		this.RequestStartedTime = new Date();
		if (inTQ == true) {
			var tblAjaxManager = new AjaxDataLoader();
			tblAjaxManager.ControlId = this.controlId;
			tblAjaxManager.StoredStateId = stateId;
			tblAjaxManager.CallBackFunction = this.PerformAction_Callback.bind(contextcontainer);
			tblAjaxManager.OnErrorFunction = this.PerformAction_CallbackError.bind(contextcontainer);
			tblAjaxManager.StatusChangedFunction = this.UpdateProgressBar.bind(this);
			tblAjaxManager.CustomErrorFunction = this.ShowExpandAllMaximumRowsEceeded.bind(this);
			tblAjaxManager.AddTask(params);
		}
		else {
			this.RequestGuid = this.GetRequestGuid();
			this.IsRequestAborted = false;
			if (params.action == null) {
				var tableProxy = new Mede.Summary.Controls.Table.TableService();
				this.Request = tableProxy.PerformAction(
					this.controlId,
					stateId,
					Object.toJSON(params),
					this.RequestGuid,
					null,
					this.PerformAction_Callback.bind(contextcontainer),
					this.PerformAction_CallbackError.bind(contextcontainer),
					this.PerformAction_CallbackError.bind(contextcontainer)
				);
				if (window.SetAjaxRequestForCancel) {
					SetAjaxRequestForCancel(this, params);
				}
			} else {
				this.RequestParams = params;
				Mede.Services.ReportState.ReportStateService.PerformStateAction(
					stateId,
					Object.toJSON(params),
					this.PerformActionStateChain_Callback.bind(contextcontainer),
					this.PerformAction_CallbackError.bind(contextcontainer),
					this.PerformAction_CallbackError.bind(contextcontainer)
				);
			}
		}
	};

	this.PerformActionStateChain_Callback = function(result, params) {
		this.context.NewStateId = result.NewStateId;
		if(typeof(AttachExportAction) == "function")
		{
			AttachExportAction(function() { CancelFunc(); clickBtn('export', null, null, result.NewStateId, true); });
		}
		this.context.RequestParams.Add("NewStateId", result.NewStateId);
		var tableProxy = new Mede.Summary.Controls.Table.TableService();
		var contextcontainer = { context: this.context, clientargs: this.context.RequestParams };
		this.context.Request = tableProxy.PerformAction(
					this.context.controlId,
					this.context.OldStateId,
					Object.toJSON(this.context.RequestParams),
					this.context.RequestGuid,
					result,
					this.context.PerformAction_Callback.bind(contextcontainer),
					this.context.PerformAction_CallbackError.bind(contextcontainer),
					this.context.PerformAction_CallbackError.bind(contextcontainer)
				);
		if (window.SetAjaxRequestForCancel) {
			SetAjaxRequestForCancel(this, params);
		}
	};

	this.Abort = function() {
		if (this.Request != null) {
			var executor = this.Request.get_executor();
			var params = new JSParameters();
			if (executor.get_started() && !executor.get_aborted()) {

				executor.abort();
				this.IsRequestAborted = true;
				this.Request = null;
				if (clckMngr) {
					clckMngr.cancel();
				}
			}
			if (this.OldStateId != null) {
				SaveStoredStateId(this.controlId, this.OldStateId);
			}
			this.NewStateId = null;
			this.RequestParams = null;
			if (this.NewItemPerPageIndex != null) {
				if (this.NewItemPerPageIndex != defaultItemPerPageIndex) {
					params.Add("ChangePageSize", "" + defaultItemPerPageIndex);
				}
				this.NewItemPerPageIndex = null;
				$(".sipp").get(0).selectedIndex = defaultItemPerPageIndex;
			}

			var tableProxy = new Mede.Summary.Controls.Table.TableService();
			if (params.storedStateId != null) //used for sub report states: expanding and paging
			{
				var stateId = params.storedStateId;
			}
			else // gets main storedStateId
			{
				var stateId = reportsXML[this.controlId]["storedStateId"];
			}
			this.Request = tableProxy.PerformCancelAction(
				this.controlId,
				stateId,
				Object.toJSON(params)
			);
		}
	}

	// Performs an action on summary report
	// used in automatically update mode
	this.PerformAction = function(params) {
	    switch (params.action) {
	        case "ExpandNode":
	            break;
	        case "NextPage":
	        case "ExpandAll":
	            this.UpdateProgressBar("Reading");
	            this.showLoadingClock(true, params);
	            break;
	        default:
	            this.UpdateProgressBar("Reading");
	            this.showLoadingClock(!this.IsManuallyUpdateView, params);
	            break;
	    }

	    var reportTable = document.getElementById("reportTable" + this.controlId, params);
	    reportTable.isSorting = 0;

	    if (this.IsManuallyUpdateView && disableUpdateViewBtn) {
	        switch (params.action) {
	            case "ExpandNode":
	            case "NextPage":
	                break;
	            default:
	                disableUpdateViewBtn();
	                break;
	        }
	    }

	    var stateMan = GetReportStateManager(this.controlId);
	    stateMan.PerformSynchronizedAction(this.RunAction.bind(this), params);
	};
	
	// Performs an action on summary report state
	// used in manually update mode
	this.PerformActionState = function(params) {
		if (params.ippIndex) {
			this.NewItemPerPageIndex = params.ippIndex;
		}
		var reportTable = document.getElementById("reportTable" + this.controlId);
		reportTable.isSorting = 0;

		if (params.storedStateId != null) //used for sub report states: expanding and paging
		{
			var stateId = params.storedStateId;
		}
		else // gets main storedStateId
		{
			var stateId = reportsXML[this.controlId]["storedStateId"];
		}

		//call external function to update view
		if (typeof (OnSummaryActionBegin) != "undefined") {
			OnSummaryActionBegin(params);
		}

		var stateMan = GetReportStateManager(this.controlId);
		stateMan.PerformStateAction(stateId, Object.toJSON(params), this.PerformActionState_Callback.bind(this), null);
	};

	// Callback - raised when new summary table content received from server
	// used in automatically update mode
	// result is the instance of ActionResult class
	this.PerformAction_Callback = function(result) {
		if (window.DisableCancel) {
			DisableCancel();
		}
		this.context.NewStateId = null;
		this.context.RequestParams = null;
		
		if (this.context.NewItemPerPageIndex != null) {
			defaultItemPerPageIndex = this.context.NewItemPerPageIndex;
			this.context.NewItemPerPageIndex = null;
		}

		if (this.context.IsRequestAborted) {
			return;
		}


		//open container with context and parameters
		var _this = this.context;
		var params = this.clientargs;

		//Debugger("DoAction_Callback");

		var reportTable = document.getElementById("reportTable" + _this.controlId);
		var reportsObj = new ActiveXObject("Microsoft.XMLDOM");
		reportsObj.loadXML(result.TableXml);

		var newStoredId = reportsObj.documentElement.getAttribute("storedStateId");
		var newStoredHash = reportsObj.documentElement.getAttribute("storedStateHash");
		
		var action = params.action;
		if ("CreateVariance" == action) {
			newStoredId = this.context.OldStateId;
		}
		var flatData = reportsObj.documentElement.getAttribute("flatData");
		if (flatData == "True") //only part of table is updated - expand and paging cases
		{
			var parentRowID = params.parentRowID;

			switch (action) {
				case "ExpandNode":
					insertTableContent(_this.controlId, reportsObj.documentElement, parentRowID, true);
					_this.ExpandCounter--;
					break;
				case "NextPage":
					insertTableContent(_this.controlId, reportsObj.documentElement, parentRowID, false);
					//suspicious code
					reportsXML[_this.controlId]["page"][parentRowID] = reportsObj.documentElement.getAttribute("page");
					_this.hideLoadingClock();
					break;
			}
		}
		else //all table is updated
		{
			//update stateId for all table
			SaveStoredStateId(_this.controlId, newStoredId);
			SaveStoredStateHash(_this.controlId, newStoredHash);
						
			switch (action) {
				case "Sort":
					reportTable.isSorting = 0;
					break;
				case "ExpandAll":
					_this.scrollUp();
					if (_this.IsManuallyUpdateView && UpdateDimensions) {
						UpdateDimensions();
					}
					break;
				case "CreateVariance":
					if (_this.IsManuallyUpdateView && UpdateDimensions) {
						//UpdateDimensions();
					}
					break;
				case "Axis":
				case "ChangeView":
				case "ResetFilters":
				case "":
				case null:
				case undefined:
					if (UpdateDimensions) {
						UpdateDimensions();
					}
					break;
			}
			initFullHTML(reportsObj.documentElement, true, _this.controlId, _this, _isWebPart[controlId]);
			_this.hideLoadingClock();
			if (ShowProcessingTime) {
				var currentTime = new Date();
				var clientProcessingSec = (currentTime.getTime() - _this.RequestStartedTime.getTime()) / 1000;
				ShowProcessingTime(result.ProcessingTime, false);
				ShowProcessingTime(Math.round(clientProcessingSec * 100) / 100, true);
				_this.RequestStartedTime = null;
			}
			if (AddStatisticsEvent && typeof(summaryUpdatedEvent) != "undefined" && summaryUpdatedEvent)
			{
				AddStatisticsEvent(summaryUpdatedEvent, result.LrGuid, result.ProcessingTime, false);
			}

			_this.OnActionFinished(result.ActionInfo);
		}
		this.context.OldStateId = newStoredId;
		_this.LoadNextNodeRunner.RunQueuedItems();
	};

	// Callback - raised when new summary table state received from server
	// used in manual update mode
	// result is the instance of StateActionInfo class
	// params is the original client params passed to PerformActionState method
	this.PerformActionState_Callback = function(result, params) {
		//Debugger("PerformActionState_Callback");
		if (result.IsSuccess) {
			SaveStoredStateId(this.controlId, result.NewStateId);
			document.focus();

			this.OnActionFinished(result);
		}
	};

	// Error handler for callbacks
	this.PerformAction_CallbackError = function(result) {
		if (result.get_exceptionType() == "") return;
		if (window.DisableCancel) {
			DisableCancel();
		}
		this.context.NewStateId = null;
		this.context.RequestParams = null;
		if (!this.context.IsRequestAborted) {
			if (this.Abort != null) {
				this.Abort();
			}
			else if (this.context.Abort != null) {
				this.context.Abort();
			}
		}
		var _this = this.context;
		var params = this.clientargs;
		_this.RequestStartedTime = null;

		//var res = result.get_message() + "\r\n" + result.get_exceptionType() + "\r\n" + result.get_stackTrace();
		//Debugger("DoAction_CallbackError\r\n" + res);
		if (result.get_exceptionType().indexOf("CustomAjaxException") < 0)
			Notifier.ExpandError("An error has occurred that has forced us to cancel this request.");

		_this.hideLoadingClock();
	};

	// Adds action info to navigation bar
	// actionInfo - instance of of StateActionInfo class comes from webservice
	this.OnActionFinished = function(actionInfo) {
		//Debugger('OnActionFinished');
		//collect action information
		this.ActionInfoStorage.push(actionInfo);

		//call external function to update view
		if (typeof (OnSummaryActionFinished) != "undefined") {
			OnSummaryActionFinished(this.ActionInfoStorage);
		}

		if ((!this.IsManuallyUpdateView
	            && actionInfo.Operation != "Filter"
	            && actionInfo.Operation != "Advanced Filter") //because filters in automatical mode works same as in manual
		    || actionInfo.Operation == "ExpandAll"
		    || actionInfo.Operation == "ResetFilters"
	        || actionInfo.Operation == null) {
			//clear storage in automatically mode
			//and dont forget about ExpandAll, because ExpandAll always works in automatically mode
			this.ActionInfoStorage = new Array();
			SetActionStorageHidden("");
		}
	};

	//--------------------------------------------------------------------------------
	this.RunSorting = function(sortColumn, sortDir, orderAxis)
	{
		var params = new JSParameters();
		params.Add("action", "Sort");
		params.Add("sortDir", sortDir);
		params.Add("sortColumn", sortColumn);
		params.Add("orderAxis", orderAxis);
		params.Add("MemberCaption", getHeaderCaptionByName(this.controlId, sortColumn));

		if (this.IsManuallyUpdateView)
		{
			this.PerformActionState(params);
		}
		else
		{
			this.PerformAction(params);
		}
	};

	//--------------------------------------------------------------------------------
	this.RunResetFilters = function() {
	    var params = new JSParameters();
	    params.Add("action", "ResetFilters");
	   
	    this.PerformAction(params);
	};

	//--------------------------------------------------------------------------------
	this.RunSwitchAxes = function() {
	    var params = new JSParameters();
	    params.Add("action", "Axis");
	    params.Add("swapAxes", "yes");
	    if (this.IsManuallyUpdateView) {
	        this.PerformActionState(params);
	    }
	    else {
	        this.PerformAction(params);
	    }
	};

	this.RunChangePageSize = function(ippIndex) {
		var params = new JSParameters();
		params.Add("action", "ChangePageSize");
		params.Add("ippIndex", ippIndex);
		params.Add("view", "table");
		this.NewItemPerPageIndex = ippIndex;
		this.PerformAction(params);
	};

	//--------------------------------------------------------------------------------
	this.RunNextPage = function(currentPage, parentRowID, action) {
	    var params = new JSParameters();
	    params.Add("action", "NextPage");
	    params.Add("currentPage", currentPage);
	    params.Add("navDirection", action);
	    params.Add("headerMemberNameList", headerMemberNameList[this.controlId]);
	    params.Add("parentRowID", parentRowID);
	    params.Add("freezeIndex", typeof (FreezeIndex) != "undefined" ? FreezeIndex : -1);

	    var tStoredStateId = reportsXML[controlId]["storedStateIds"][parentRowID];
	    params.Add("storedStateId", tStoredStateId);
	    this.PerformAction(params);
	};

	//--------------------------------------------------------------------------------
	this.RunExpand = function(groupName, parentRowID, isLeaf)
	{
		//Debugger('RunExpand');

		var reportTable = document.getElementById("reportTable" + this.controlId);
		if (reportTable.isSorting == 1 || this.HasLoadingChilds(parentRowID))
		{
			return false;
		}

		var expandOperation = "True";
		var rObj = document.getElementById(parentRowID);
		if (document.getElementById(parentRowID + "_0"))
		{
			var isShown = changeShowHideState(parentRowID);
			ResizeUnfreezeDiv(controlId);

			if (!isShown)
			{
				expandOperation = "False";
			}
		}
		else
		{
			// Show 'Loading...' label while server call is in progress
			this.ShowLoadingRow(parentRowID);

			//send request for table data
			var tReportStateId = reportsXML[this.controlId]["storedStateIds"][parentRowID.substr(0, parentRowID.lastIndexOf("_"))];
			var params = new JSParameters();
			params.Add("action", "ExpandNode");
			params.Add("member", getUnquotedStr(groupName));
			params.Add("isLeaf", isLeaf ? "True" : "False");
			params.Add("headerMemberNameList", headerMemberNameList[controlId]);
			params.Add("parentRowID", parentRowID);
			params.Add("storedStateId", tReportStateId);
			params.Add("freezeIndex", typeof(FreezeIndex) != "undefined" ? FreezeIndex : -1);
			this.LoadNextNodeRunner.Run(this.PerformAction.bind(this), params);
			this.ExpandCounter++;
		}

		//send change main state request
		var membersList = GetHierarchicalMembersXml(parentRowID);
		var params = new JSParameters();
		params.Add("action", "Expand");
		params.Add("members", membersList);
		params.Add("expand", expandOperation);
		this.PerformActionState(params);
	};

	//--------------------------------------------------------------------------------
	this.RunExpandAll = function(size, expand)
	{
		var params = new JSParameters();
		params.Add("action", "ExpandAll");
		params.Add("type", expand);
		params.Add("expand-axes", reportsXML[controlId]["RowAxes"]);
		params.Add("page", reportsXML[controlId]["page"]["tr" + controlId + "_0"]);
		params.Add("page-size", size);
		this.PerformAction(params);
	}

	//--------------------------------------------------------------------------------
	this.RunFreeze = function(freezeIndex)
	{
		this.PerformActionState({ action: "Freeze", freezeIndex: freezeIndex });
    };

    //--------------------------------------------------------------------------------
    this.RunUpdate = function(storedStateId) {
    	var params = new JSParameters();
    	AttachExportAction(function() { CancelFunc(); clickBtn('export', null, null, storedStateId, true); });
    	this.PerformAction(params);
    }

	//--------------------------------------------------------------------------------
	this.ShowContextMenu = function(tdObj)
	{
		//Debugger('ShowContextMenu');
		var menu = new ContextMenu();
		this.CreateSortContextMenu(menu, tdObj);
		var reportTable = document.getElementById("reportTable" + this.controlId);
		if (this.ExpandCounter == 0) {
			this.CreateFreezeColumnsContextMenu(menu, tdObj);
		}
		menu.Show(event.screenX, event.screenY, null);
		return false;
	};

	//--------------------------------------------------------------------------------
	this.scrollUp = function()
	{
		if (CanFreeze)
			document.body.scrollTop = 0;
	};
	
	//--------------------------------------------------------------------------------
	this.CreateSortContextMenu = function(menu, tdObj) {
		var paragraph = "&nbsp&nbsp";
		var scrup = "window.pivotTableControl_" + this.controlId + ".scrollUp();";
		for (var i = 0; i < this.selectedDimansion.length; i++) {
			var imgAsc = "";
			var imgDesc = "";
			var imgNoSort = "";
			var ascEnabled = true;
			var descEnabled = true;
			var nosortEnabled = true;
			var fontColorAsc = "";
			var fontColorDesc = "";
			var fontColorNoSort = "";
			var selectItemMenuAsc = "";
			var selectItemMenuDesc = "";
			var selectItemMenuNoSort = "";
			var onClick = scrup + "window.pivotTableControl_" + this.controlId + ".SubSort(" + i + ", \"" + tdObj.id + "\"";
			var headers = getHeaders(this.controlId);
			var indexHeader = parseInt(tdObj.id.toString().substr(tdObj.id.toString().indexOf("|") + 1));
			
			if (!this.AllowSorting(headers(indexHeader))) {
				continue;
			}

			var sortMember = headers(indexHeader).getAttribute("name");
			var tAlternativeMemberHeader = headers(indexHeader).getAttribute("alternative-name");
			var tMember = getUnquotedStr(this.selectedDimansion[i]["Member"]);
			var isCurrentMember = (sortMember == tMember || tAlternativeMemberHeader == tMember);

			if (isCurrentMember && this.selectedDimansion[i]["Order"].toLowerCase() == "asc") {
				ascEnabled = false;
//				if (this.selectedDimansion[i]["isSetManualUpdate"] == true) {
//					fontColorAsc = " color:#6BA56B; ";
//					imgAsc = '<%= WebResource("MedeFinance.images.sort.sortAscSelectGreen.png")%>'; //"sortAscSelectGreen.png";
//				}
//				else {
//					fontColorAsc = " color:#666666; ";
//					imgAsc = '<%= WebResource("MedeFinance.images.sort.sortAscSelect.png")%>'; //"sortAscSelect.png";
//				}
			}
			else {
				//imgAsc = '<%= WebResource("MedeFinance.images.sort.sortAsc.png")%>'; //"sortAsc.png";
				//fontColorAsc = "";
			}

			if (isCurrentMember && this.selectedDimansion[i]["Order"].toLowerCase() == "desc") {
				descEnabled = false;
//				if (this.selectedDimansion[i]["isSetManualUpdate"] == true) {
//					fontColorDesc = " color:#6BA56B; ";
//					imgDesc = '<%= WebResource("MedeFinance.images.sort.sortDescSelectGreen.png")%>'; //"sortDescSelectGreen.png";
//				}
//				else {
//					fontColorDesc = " color:666666; ";
//					imgDesc = '<%= WebResource("MedeFinance.images.sort.sortDescSelect.png")%>'; //"sortDescSelect.png";
//				}
			}
			else {
				imgDesc = '<%= WebResource("MedeFinance.images.sort.sortDesc.png")%>'; // "sortDesc.png";
				fontColorDesc = "";
			}

			if (!isCurrentMember || this.selectedDimansion[i]["Order"].toLowerCase() == "none") {
				nosortEnabled = false;
//				if (this.selectedDimansion[i]["isSetManualUpdate"] == true && isCurrentMember) {
//					fontColorNoSort = " color:#6BA56B; ";
//					imgNoSort = '<%= WebResource("MedeFinance.images.sort.sortNoSelectGreen.png")%>'; //"sortNoSelectGreen.png";
//				}
//				else {
//					fontColorNoSort = " color:666666; ";
//					imgNoSort = '<%= WebResource("MedeFinance.images.sort.sortNoSelect.png")%>'; //"sortNoSelect.png";
//				}
			}
			else {
				imgNoSort = '<%= WebResource("MedeFinance.images.sort.sortNo.png")%>'; //"sortNo.png";
				fontColorNoSort = "";
			}

			var dimName = this.selectedDimansion[i]["Name"];
			var subMenu = new ContextMenu('SubMenu'+i);
			var menuItem = new ContextMenuItemWithSubmenu(dimName, subMenu);
			//menuItem.enabled = false;
			menu.items.push(menuItem);

			var menuItem = new ContextMenuItem('Sort Ascending');
			menuItem.onClick = onClick + ", \"asc\")";
			menuItem.imgSrc = '../images/ContextMenu/sortAsc.png';
			menuItem.enabled = ascEnabled;
			subMenu.items.push(menuItem);

			var menuItem = new ContextMenuItem('Sort Descending');
			menuItem.onClick = onClick + ", \"desc\")";
			menuItem.imgSrc = '../images/ContextMenu/sortDesc.png';
			menuItem.enabled = descEnabled;
			subMenu.items.push(menuItem);

			var menuItem = new ContextMenuItem('No Sort');
			menuItem.onClick = onClick + ", \"none\")";
			menuItem.imgSrc = '../images/ContextMenu/noSort.png';
			menuItem.enabled = nosortEnabled;
			subMenu.items.push(menuItem);
		}
	};

	//--------------------------------------------------------------------------------
	this.SubSort = function(indexArray, tdId, order)
	{
		var reportTable = document.getElementById("reportTable" + this.controlId);
		if (reportTable.isSorting == 1)
		{
			return;
		}
		reportTable.isSorting = 1;

		if (tdId.charAt(0) == "f")
		{
			tdId = tdId.substr(1);
		}

		var tdObj = document.getElementById(tdId);
		var orderAxis = this.selectedDimansion[indexArray]["Id"];
		var headers = getHeaders(this.controlId);
		var indexHeaders = parseInt(tdId.toString().substr(tdId.toString().indexOf("|") + 1));
		var sortMember = headers(indexHeaders).getAttribute("name");
		var tAlternativeMemberHeader = headers(indexHeaders).getAttribute("alternative-name");

		if (headers(indexHeaders).getAttribute("allow-sorting") == "no")
			return;

		this.selectedDimansion[indexArray]["Member"] = sortMember;
		this.selectedDimansion[indexArray]["Order"] = order;
		this.selectedDimansion[indexArray]["isSetManualUpdate"] = this.IsManuallyUpdateView;
		this.selectedDimansion[indexArray]["HeaderObj"] = tdObj;

		this.RefreshHeaderManuallyUpdate(headers);

		this.RunSorting(sortMember, order, orderAxis);
	};

	//--------------------------------------------------------------------------------
	this.Sort = function(tdObj) {
	    
	    var headerId = "";
	    if (tdObj.id.toString().charAt(0) != "f") {
	        headerId = tdObj.id.toString();
	    }
	    else {
	        headerId = tdObj.id.toString().substr(1);
	    }

	    var reportTable = document.getElementById("reportTable" + this.controlId);
	    if (reportTable.isSorting == 1) {
	        return;
	    }

	    var headers = getHeaders(this.controlId);
	    var indexHeaders = parseInt(headerId.substr(headerId.indexOf("|") + 1));
	    var sortMember = headers(indexHeaders).getAttribute("name");
	    var sortDir = headers(indexHeaders).getAttribute("sort");

	    if (headers(indexHeaders).getAttribute("allow-sorting") == "no")
	        return;

	    reportTable.isSorting = 1;

	    if (sortDir == null)
	        sortDir = "none";

	    switch (sortDir) {
	        case "":
	        case "none":
	            sortDir = "asc";
	            break;
	        case "asc":
	            sortDir = "desc";
	            break;
	        case "desc":
	            sortDir = "none";
	            break;
	        default:
	            sortDir = "none";
	            break;
	    }

	    headers(indexHeaders).setAttribute("sort", sortDir);

	    for (var i = 0; i < this.selectedDimansion.length; i++) {
	        this.selectedDimansion[i]["Order"] = sortDir;
	        this.selectedDimansion[i]["Member"] = sortMember;
	        this.selectedDimansion[i]["isSetManualUpdate"] = this.IsManuallyUpdateView;
	        this.selectedDimansion[i]["HeaderObj"] = document.getElementById(headerId); // tdObj;
	    }

	    this.RefreshHeaderManuallyUpdate(headers);

	    this.RunSorting(sortMember, sortDir, '');
	};
	
	//--------------------------------------------------------------------------------
	this.RefreshHeaderManuallyUpdate = function(headers)
	{
		this.SetHeaderClass("cssHeader", headers);

		for (var iHeader = 0; iHeader < this.selectedDimansion.length; iHeader++)
		{
			var headerObj = this.selectedDimansion[iHeader]["HeaderObj"];
			if (headerObj != null)
			{
				headerObj.className = "cssHeaderGreen";
				if (headerObj.parentNode.parentNode.parentNode.id < 2)
				{
					headerObj.parentNode.parentNode.parentNode.parentNode.className = "cssHeaderGreen";
					headerObj.style.borderBottomStyle = "none";
					headerObj.style.borderTopStyle = "none";
				}
			}
		}
	};

	//--------------------------------------------------------------------------------
	this.CreateFreezeColumnsContextMenu = function(menu, tdObj)
	{
		var scrup = "window.pivotTableControl_" + this.controlId + ".scrollUp();";

		if (CanFreeze)
		{
			var reportIndex = getReportIndexByRowID(tdObj.parentElement.id);
			var freezeTable = document.getElementById("reportTable" + reportIndex);
			var unfreezeTable = document.getElementById("unfreeze_reportTable" + reportIndex);
			var colposition = getColumnPosition(freezeTable, unfreezeTable, tdObj);

			if (colposition < (getColumnsArray(freezeTable).length + getColumnsArray(unfreezeTable).length) - 1)
			{
				var menuCaption = "Freeze Columns";
				var operation = "freeze";

				if (FreezeIndex == colposition)
				{
					menuCaption = "Unfreeze Columns";
					operation = "unfreeze";
				}
				menu.addSeparator();
				var menuItem = new ContextMenuItem(menuCaption);
				menuItem.onClick = scrup + "window.pivotTableControl_" + this.controlId + ".FreezeColumns(\"" + tdObj.id + "\", \"" + operation + "\");";
				menu.items.push(menuItem);
			}
		}
	};

	//--------------------------------------------------------------------------------
	this.FreezeColumns = function(tdId, operation)
	{
		applyFreeze(tdId, operation);
	};

	//--------------------------------------------------------------------------------
	this.SetHeaderClass = function(cssClassName, headers)
	{
		var indexHeader = 0;
		var freeze_TrObj = document.getElementById("tr" + this.controlId + "_0");
		var unreeze_TrObj = document.getElementById("unfreeze_tr" + this.controlId + "_0");

		if (freeze_TrObj)
		{
			for (var i = 0; i < freeze_TrObj.cells.length; i++)
			{
				this.ClearHeaderImg(freeze_TrObj.cells[i], cssClassName, headers, indexHeader);
				indexHeader++;
			}
		}

		if (unreeze_TrObj)
		{
			for (var i = 0; i < unreeze_TrObj.cells.length; i++)
			{
				this.ClearHeaderImg(unreeze_TrObj.cells[i], cssClassName, headers, indexHeader);
				indexHeader++;
			}
		}
	};

	//--------------------------------------------------------------------------------
	this.ClearHeaderImg = function(cellObj, cssClassName, headers, indexHeader)
	{
		if (cellObj.id.toString().charAt(0) == "f")
		{
			var headerId = cellObj.id.toString().substr(1);
			cellObj = document.getElementById(headerID);
		}
		cellObj.className = cssClassName;
		if (cellObj.id.length < 2)
		{
			cellObj.children[0].rows[0].cells[1].className = cssClassName;
		}
		//var tImgSpanObj = document.getElementById("imgSpanHeader" + this.controlId + "|" + indexHeader);
		var tHeaderObj = document.getElementById("headerid" + this.controlId + "|" + indexHeader);
		var tImgSpanObj = this.GetSortingConteiner(tHeaderObj);
		var tHeader = headers(indexHeader);
		tImgSpanObj.innerHTML = this.GetImgSortOrder(tHeader);
	};

	this.GetSortingConteiner = function(tSpanObj)
	{
		return tSpanObj.getElementsByTagName("span")[0];
	};

	//--------------------------------------------------------------------------------
	this.AllowSorting = function(header)
	{
		var allowSorting = header.getAttribute("allow-sorting");
		if (allowSorting == "no") return false;
		return true;
	};

	//--------------------------------------------------------------------------------
	this.GetImgSortOrder = function(header)
	{
		if (!this.AllowSorting(header)) return "";

		var strResult = "";
		var tImgSort = "";
		var headerSortAscCounter = 0;
		var headerSortDescCounter = 0;
		var tMemberHeader = header.getAttribute("name");

		var tAlternativeMemberHeader = header.getAttribute("alternative-name");

		for (var iHeader = 0; iHeader < this.selectedDimansion.length; iHeader++)
		{
			var tMember = this.selectedDimansion[iHeader]["Member"];
			if (tMemberHeader == tMember || tAlternativeMemberHeader == tMember)
			{
				if (this.selectedDimansion[iHeader]["Order"].toLowerCase() == "asc")
				{
					headerSortAscCounter++;
				}

				if (this.selectedDimansion[iHeader]["Order"].toLowerCase() == "desc")
				{
					headerSortDescCounter++;
				}
			}
		}

		if (headerSortAscCounter == 1)
		{
			tImgSort = '<%= WebResource("MedeFinance.images.sort.HeaderSortAsc.gif")%>'; // "HeaderSortAsc.gif";
		}

		if (headerSortDescCounter == 1)
		{
			tImgSort = '<%= WebResource("MedeFinance.images.sort.HeaderSortDesc.gif")%>'; // "HeaderSortDesc.gif";
		}

		if (headerSortAscCounter > 1)
		{
			tImgSort = '<%= WebResource("MedeFinance.images.sort.HeaderSortAscDouble.gif")%>'; // "HeaderSortAscDouble.gif";
		}

		if (headerSortDescCounter > 1)
		{
			tImgSort = '<%= WebResource("MedeFinance.images.sort.HeaderSortDescDouble.gif")%>'; // "HeaderSortDescDouble.gif";
		}

		if (headerSortAscCounter != 0 && headerSortDescCounter != 0)
		{
			tImgSort = '<%= WebResource("MedeFinance.images.sort.HeaderSortAscDesc.gif")%>'; // "HeaderSortAscDesc.gif";
		}

		if (tImgSort != "")
		{
			strResult = "&nbsp<img width=\"10px\" height=\"12px\" align=\"absMiddle\" src=\"" + tImgSort + "\"/>";
		}

		return strResult;
	};


	// Inserts 'Loading...' row in the table.
	// parentRowID - is ID of TR element under which will be inserted Loading row
	this.ShowLoadingRow = function(parentRowID)
	{
		var reportTable = document.getElementById("reportTable" + this.controlId);
		var unfreeze_reportTable = document.getElementById("unfreeze_reportTable" + this.controlId);
		var num = getRowNumberByRowID(reportTable, parentRowID);

		var freezeColumnCount;

		var rObj = reportTable.insertRow(num + 1);
		var unfreeze_rObj = unfreeze_reportTable.insertRow(num + 1);

		rObj.isLoading = 1;
		rObj.bgColor = GroupBgColor;
		rObj.id = "loading_level_progress_" + parentRowID;
		rObj.parentID = parentRowID;

		unfreeze_rObj.bgColor = GroupBgColor;
		unfreeze_rObj.id = "loading_level_progress_unfreeze_" + parentRowID;
		unfreeze_rObj.parentID = "unfreeze_" + parentRowID;

		cObj = rObj.insertCell(0);
		cObj.noWrap = true;
		cObj.vAlign = "bottom";
		cObj.style.color = "gray";

		for (var len = 1; len < parentRowID.split('_').length; len++)
		{
			cObj.innerHTML += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
		}
		cObj.innerHTML += "Loading..."

		var cellsCount = getHeaders(this.controlId).length;
		freezeColumnCount = cellsCount;
		if (unfreeze_rObj)
		{
			if (FreezeIndex > -1 && FreezeIndex < cellsCount)
				freezeColumnCount = FreezeIndex + 1;
		}

		for (var j = 0; j < cellsCount - 1; j++)
		{
			if (j + 1 >= freezeColumnCount)
				cObj = unfreeze_rObj.insertCell(0);
			else
				cObj = rObj.insertCell(1);

			cObj.noWrap = true;
			cObj.innerHTML = "&nbsp;";
		}
		if (unfreeze_rObj && unfreeze_rObj.lastChild != null && unfreeze_reportTable.rows[num].lastChild)
		{
			unfreeze_rObj.lastChild.bgColor = unfreeze_reportTable.rows[num].lastChild.bgColor;
		}
		else
		{
			if (reportTable.rows[num].lastChild)
				rObj.lastChild.bgColor = reportTable.rows[num].lastChild.bgColor;
		}
	};

	// Check if group in the table has running request for data
	// Used in expanding and paging
	this.HasLoadingChilds = function(parentRowID)
	{
		if (this.IsRowLoading(parentRowID))
			return true;
		var reportTable = document.getElementById("reportTable" + this.controlId);
		var num = getRowNumberByRowID(reportTable, parentRowID);
		var row = reportTable.rows[num];
		for (var i = num; (i < reportTable.rows.length) && (reportTable.rows(i).id.indexOf(parentRowID) != -1); i++)
		{
			if (reportTable.rows(i).id.indexOf("loading_level_progress_") == 0)
				return true;
		}
		return false;
	};

	//--------------------------------------------------------------------------------
	this.IsRowLoading = function(rowID)
	{
		var row = document.getElementById(rowID);
		return (row.isLoading == 1);
	};

	// Shows a local layer with progress bar on summary table.
	// if tempShowClock is false - shows only layer witout progressbar
	this.showLoadingClock = function(tempShowClock, params) {
		if (window.DisableFunc) {
			DisableFunc(params);
			return;
		}
		if (typeof (isMultiTable) == "undefined")
			isMultiTable = false;

		var tObj = document.getElementById(getTableTag(this.controlId));
		var unfreeze_tObj = document.getElementById("unfreeze_" + getTableTag(this.controlId));
		if (unfreeze_tObj)
			unfreeze_tObj.disabled = true;
		tObj.disabled = true;
		var clockObj = document.getElementById("loadingclock" + this.controlId);
		var o = clockObj;
		var y = 0;
		var x = 0;
		var o = tObj;
		var _body;

		if (isMultiTable) {
			_body = o.parentElement;
			while (o.id.indexOf("tableDiv") == -1 && o.id.indexOf("attachedTable") == -1) {
				_body = o = o.parentElement;
			}
		}
		else {
			_body = document.body
		}

		y = getOffTop(_body);

		var allTableVisible = false;
		if (y > _body.scrollTop && tObj.offsetHeight <= (_body.clientHeight + _body.scrollTop - _body.offsetTop)) allTableVisible = true;

		var clockWidth = clockObj.style.pixelWidth;
		var clockHeight = clockObj.style.pixelHeight;

		if (allTableVisible) {
			y = tObj.offsetHeight / 2 - clockHeight / 2;
		}
		else {
			if (_body.scrollTop > y + tObj.offsetTop) {
				var tTopInvisible = _body.scrollTop - y - tObj.offsetTop;
			} else {
				var tTopInvisible = 0;
			}

			if (!isMultiTable) {
				var tBottomInvisible = (y + tObj.offsetHeight + tObj.offsetTop) - (_body.scrollTop + _body.clientHeight);
				var resHeight = tObj.offsetHeight - tTopInvisible;
				if (tBottomInvisible > 0) resHeight -= tBottomInvisible;
				y = resHeight / 2 - clockHeight / 2 + tTopInvisible - _body.scrollTop;
			}
			else {
				var resHeight = (tObj.offsetHeight < _body.offsetHeight) ? tObj.offsetHeight : _body.offsetHeight;
				y = resHeight / 2 - clockObj.style.pixelHeight / 2 + tTopInvisible;
			}
		}
		if (y < 0) y = 0;
		clockObj.style.posTop = y;
		if (isMultiTable) {
			clockObj.style.posLeft = (_body.offsetWidth) / 2 - clockWidth / 2 + _body.scrollLeft;
		}
		else {
			clockObj.style.posLeft = (_body.offsetWidth - 20) / 2 - clockWidth / 2 + _body.scrollLeft;
		}

		if (tempShowClock == true) {
			if (_showClock)
				clockObj.style.display = "";
			else
				clockObj.style.display = "none";
		}
	};

	// Hides a local layer with progress bar on summary table.
	this.hideLoadingClock = function() {
		if (window.UndisableFunc) {
			UndisableFunc();
			return;
		}
		var tObj = document.getElementById(getTableTag(this.controlId));
		var unfreeze_tObj = document.getElementById("unfreeze_" + getTableTag(this.controlId));
		if (unfreeze_tObj)
			unfreeze_tObj.disabled = false;
		if (tObj)
			tObj.disabled = false;
		var clockObj = document.getElementById("loadingclock" + this.controlId);
		if (clockObj)
			clockObj.style.display = "none";
	};
	
	//--------------------------------------------------------------------------------
	this.ShowExpandAllMaximumRowsEceeded = function(exception) {
		window.showModalDialog("Controls/Table/MaximumRowsExceededStub.aspx", window, "dialogHeight:300px; dialogWidth:500px; center:yes; resizable:no; scroll:no; status:no");
	};

	// Set step on a local layer with progress bar on summary table.
	// status can be "Reading" or "Rendering". Other value will be reset step.
	this.UpdateProgressBar = function(status) {
		//try {
			if (window.UpdateDisableMessages) {
				switch (status) {
					case "Reading":
						UpdateDisableMessages("Reading...", "Step 1 of 2");
						break;
					case "Rendering":
						UpdateDisableMessages("Rendering...", "Step 2 of 2");
						break;
					default:
						UpdateDisableMessages("Rendering...", "Step 1 of 1");
						break;
				}
			} else {
				var loadingWheel = eval("LoadingWheel" + this.controlId + "_LoadingWheel");
				switch (status) {
					case "Reading":
						loadingWheel.SetStatus("Reading...", "Step 1 of 2");
						break;
					case "Rendering":
						loadingWheel.SetStatus("Rendering...", "Step 2 of 2");
						break;
					default:
						loadingWheel.SetStatus("Rendering...", "Step 1 of 1");
						break;
				}
			}
		//} catch (e) { }
};

this.ShowViewSource = function() {
    var tableProxy = new Mede.Summary.Controls.Table.TableService();
    tableProxy.GetViewSource(
				this.OldStateId,
				this.GetViewSource_Callback,
				this.ShowViewSource_CallbackError,
				this.ShowViewSource_CallbackError
			);
}

this.ShowViewSource_CallbackError = function() {
    Notifier.ExpandError("An error has occurred that has forced us to cancel this request.");
}

this.GetViewSource_Callback = function(result) {
    var resultString = "<title>View Source</title>"
        + "<span style='font-family:Arial;font-size:10pt;'>"
        + "Organization Unit = " + result.OrgUnit
        + "<br/>Datetime = " + (new Date()).toLocaleString()
        + "<br/>Server Name = " + result.ServerName
        + "<br/>" + result.State
        + "</span><br/><br/>";
    var win = window.open('', 'ViewSource', 'top=400, left=400, width=500, height=200, scrollbars=yes, resizable=yes');
    win.document.write(resultString);
}
}
