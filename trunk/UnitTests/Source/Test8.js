
// This function returns another function
function GetFunction(type)
{
	switch (type)
	{
		case 0:
			var t = function()
			{
			};
			return t;
		case 1:
			t1 = function()
			{
			};
			return t1;
		case 2:
			return function(){};
		case 3:
			return { exec: function(){} };
	}
}
