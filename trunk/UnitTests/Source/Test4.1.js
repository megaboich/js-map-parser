//------------------------------------------------------------------------------------
// This is sample of JS class.
function ItemFactory(param1, param2)
{
	return {
		field1: param1,
		field2: param2,
		/*method1*/method1: function() { },
		/*method2*/method2: function(p) { },
		/*method3*/method3: function(p1, p2)
		{
			/*Delegate method
			This method contains multilines comment
			Muahaha!*/
			var delegate = function()
			{
			};
		}
	};
}