//------------------------------------------------------------------------------------
// This is sample of JS class.
function ItemFactory(param1, param2)
{
	var item = null;
	item = {
		field1: param1,
		field2: param2,
		/*method1*/method1: function() { },
		/*method2*/method2: function(p) { },
		/*method3*/method3: function(p1, p2)
		{
			//delegate method
			var delegate = function()
			{
			};
		}
	};
	return item;
}