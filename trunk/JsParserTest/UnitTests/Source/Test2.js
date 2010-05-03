//This is sample of JS class
var Class1 = function(param1, param2)
{
	var _this = this;
	_this._field1 = param1;
	_this._field2 = param2;

	//This is First method
	_this.Method1 = function()
	{
	};

	//This is Second method
	_this.Method2 = function(param1)
	{
	};

	//This is Third method
	_this.Method3 = function(param1, param2)
	{
		//delegate method
		var delegate = function()
		{
		};
	};
};
