

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

	//#region Public Interface
	
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

	_this.GetItemsCaptionsStr = function()
	{
		var str = "";
		_this._element.find(".il_itemBox").each(function() { str = str + ", " + this.outerText; });
		str = str.length > 0 ? str.substring(2) : "";

		return str;
	}

	_this.RemoveSelected = function()
	{
		if (_this._selectedItems.length < 100) {
			_this._element.find(".il_itemBoxSelected").fadeOut(300, function()
			{
				$(this).remove();
			});
		}
		else {
			_this._element.find(".il_itemBoxSelected").remove();
		}

		_this._selectedItems.length = 0;
	};

	_this.InsertItems = function(value)
	{
		var items = $.makeArray(value);
		var itemTemplate = _this._element.find(".il_itemBoxTemplate");
		var bigScope = items.length > 30;
		var lastIndex = items.length - 1;
		$.each(items, function(index, item)
		{
			if (document.getElementById(item.Id) == null) {
				var template = itemTemplate.clone();
				template.attr("id", item.Id).attr("index", (++_this._lastItemIndex).toString());
				template.find(".il_name").text(item.Name).attr("title", (item.Description.length > 0 ? "("+item.Description+")" : ""));
				template.addClass("il_itemBox").removeClass("il_itemBoxTemplate");
				_this._element.append(template);
				if (bigScope)
				{
					template[0].style.display = "";
				}
				else
				{
					template.fadeIn(300, index == lastIndex ? _this.ScrollDown : null);
				}
			}
		});
		
		if (bigScope)
		{
			setTimeout(_this.ScrollDown, 50);
		}
	};

	_this.ClearAll = function()
	{
		_this.ClearSelected();
		_this._element.fadeOut(300, function()
		{
			$(this).find(".il_itemBox").remove();
			$(this).fadeIn(0);
		});
	};

	_this.FastClearAll = function()
	{
		_this.ClearSelected();
		_this._element.find(".il_itemBox").remove();
	};

	//#endregion Public Interface

	_this.OnLoad = function()
	{
		_this._element = $("#" + _this._clientID);
		_this._lastItemIndex = parseInt(_this._element.find(".il_itemBox:last").attr("index")) || -1;
		$(document.body).click(_this.OnBodyClick);
		$(document.body).mousemove(_this.OnMouseMove);
		$(_this._element).dblclick(_this.OnDblClick);
	};

	_this.OnBodyClick = function(ev)
	{
		var el = $(ev.target);
		if (el.hasClass("il_name")) {
			_this.OnItemClick(ev);
		} else if (!el.is(":submit, :button") && !ev.ctrlKey && !ev.shiftKey) {
			_this.ClearSelected();
		}
		if (_this._onNodeSelected)
		{
			_this._onNodeSelected();
		}
	};

	_this.OnItemClick = function(ev)
	{
		var el = $(ev.target).closest(".il_itemBox");

		if (ev.ctrlKey) {
			if (el.hasClass("il_itemBoxSelected")) {
				_this.UnselectItems(el);
			} else {
				_this.PrependSelected(el);
			}
		}
		else if (ev.shiftKey && _this._selectedItems.length > 0 && _this._selectedItems[0] !== el[0])
		{
			var isInvertSelection = parseInt(_this._selectedItems[0].index) > parseInt(el[0].index);
			var startEl = isInvertSelection ? el[0] : _this._selectedItems[0];
			var endEl = isInvertSelection ? _this._selectedItems[0] : el[0];
			_this.ClearSelected();
			while (startEl != endEl.nextSibling && startEl != null)
			{
				var item = $(startEl).addClass("il_itemBoxSelected")[0];
				if (isInvertSelection)
				{
					_this._selectedItems.unshift(item);
				}
				else
				{
					_this._selectedItems.push(item);
				}
				startEl = startEl.nextSibling;
			}
		}
		else {
			_this.ClearSelected();
			_this.AppendSelected(el);
		}
	};

	_this.OnMouseMove = function(ev)
	{
		var jboxEl = $(ev.target).closest(".il_itemBox");
		var boxEl = null;
		if (jboxEl.length > 0)
			boxEl = jboxEl[0];

		if (_this._hoverItem != null && _this._hoverItem != boxEl)
		{
			$(_this._hoverItem).removeClass("il_itemBoxHover");
			_this._hoverItem = null;
		}

		if (boxEl != null && _this._hoverItem == null && jboxEl.hasClass("il_itemBox"))
		{
			jboxEl.addClass("il_itemBoxHover");
			_this._hoverItem = boxEl;
		}
	};

	_this.OnDblClick = function(ev)
	{
		var boxEl = $(ev.target).closest(".il_itemBox");
		if (boxEl.length > 0)
		{
			_this.OnItemDblClick(boxEl);
		}
	};

	_this.ClearSelected = function(invertOrder)
	{
		_this._element.find(".il_itemBoxSelected").removeClass("il_itemBoxSelected");
		_this._selectedItems.length = 0;
	};

	_this.AppendSelected = function(elements)
	{
		_this.SelectItems(elements, true);
	};

	_this.PrependSelected = function(elements)
	{
		_this.SelectItems(elements, false);
	};

	_this.SelectItems = function(elements, isAppending)
	{
		elements.each(function()
		{
			if (!$(this).hasClass("il_itemBoxSelected")) {
				if (isAppending) {
					_this._selectedItems.push(this);
				} else {
					_this._selectedItems.unshift(this);
				}
			}
		});
		elements.addClass("il_itemBoxSelected");
	};

	_this.UnselectItems = function(elements)
	{
		elements.each(function()
		{
			if ($(this).hasClass("il_itemBoxSelected")) {
				_this._selectedItems.splice($.inArray(this, _this._selectedItems), 1);
			}
		});
		elements.removeClass("il_itemBoxSelected");
	};
	
	_this.ScrollDown = function()
	{
		_this._element.parent().scrollTop(_this._element.attr("scrollHeight"));
	};

	_this.OnItemDblClick = function(item)
	{
		_this.ClearSelected();
		item.fadeOut(200, function() { $(this).remove(); });
		if (_this._onNodeDblClick)
		{
			_this._onNodeDblClick();
		}
	};

	$(_this.OnLoad);
};


ItemList_ascx.DataItem = function(id, name, description)
{
	this.Id = id;
	this.Name = name;
	this.Description = description;
	
	var sabaka = function()
	{
	};
	
	this.kabka = function()
	{
	};
};
