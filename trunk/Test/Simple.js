
$.Autocompleter = function(input, options) {
	var timeout;
	var previousValue = "";
	var cache = $.Autocompleter.Cache(options);

	$input.bind(".autocomplete",
		 function(event) {})
	.bind("search", function() {})
	.bind("flushCache", function() {
		cache.flush();
	}).bind("setOptions", function() {
		$.extend(options, arguments[1]);
		// if we've updated the data, repopulate
		if ( "data" in arguments[1] )
			cache.populate();
	}).bind("unautocomplete", function() {
		select.unbind();
		$input.unbind();
		$(input.form).unbind(".autocomplete");
	});
}

function AnonimousRunner(fun)
{
	fun();
}

AnonimousRunner(
	function() 
	{
		alert("ahha") 
	}
);

var obj = {
	method1: function()
	{
		AnonimousRunner(function(){alert("ahha")});
	},

	methid2: function()
	{
	}
}

function F()
{
	var subF = function()
	{
		var subF2 = function()
		{
		}

		var subF3 = function()
		{
		}

		var contains = document.compareDocumentPosition 
		? function(a, b)
		{
			return a.compareDocumentPosition(b) & 16;
		}
		: function(a, b)
		{
			return a !== b && (a.contains ? a.contains(b) : true);
		};
	};
}

//------------------------------------------------------------------------------------
// This object used to pass parameters from JavaScript to asp.net Server.
// It will deserialize as Dictionary<string, string>
function Class1()
{
	this.Fun1 = function(param, value)
	{
		this[param] = value;
		
		
		var f = function()
		{
		};
	}

	this.Fun2 = function()
	{
		return Object.toJSON(this);
	}
}

//------------------------------------------------------------------------------------
// Big comment block
var ItemList_ascx = function(clientID)
{
	var _this = this;
	_this._clientID = clientID;
	_this._element = null;
	_this._selectedItems = new Array();
	_this._hoverItem = null;
	_this._lastItemIndex = -1;

	_this._onNodeSelected = null;
	_this._onNodeDblClick = null;
	
	_this.GetAllItems = function()
	{
		var allElems = _this._element.find(".il_itemBox");
		var allNames = allElems.find(".il_name");
		var data = [];
		
		for (var i=0; i < allElems.length; ++i)
		{
			data.push( {
				Id: allElems[i].id,
				Name: allNames[i].innerText
			});
		}
		return data;
	};

	_this.GetItemsJsonStr = function()
	{
		var str = "";
		_this._element.find(".il_itemBox").each(function()
		{
			str = str + ", '" + this.id.replace(/\\/, "\\\\").replace(/'/, "\\'") + "':'"
			+ this.outerText.replace(/\\/, "\\\\").replace(/'/, "\\'") + "'";
		});

		str = str.length > 0 ? "{" + str.substring(2) + "}" : "{}";


		return str;
	};
}



