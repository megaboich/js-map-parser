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
