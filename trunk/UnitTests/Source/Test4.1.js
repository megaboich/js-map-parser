// Comment to funtion 1
// This function does nothing
function Function1()
{
}

//------------------------------------------------------------------------------------
// This is sample of JS class factory.
function GetNewItem(param1, param2)
{
	return {
		field1: param1,
		field2: param2,
		/*method1*/method1: function() { },
		/*method2*/method2: function(p) {
		//This function contains a lot of comments before and after declaration
		},
		/*method3*/method3: function(p1, p2)
		{
			/*Delegate method
			This method contains multilines comment*/
			var delegate = function()
			{
			};
		}
	};
}

//This is another sample of JS class
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
