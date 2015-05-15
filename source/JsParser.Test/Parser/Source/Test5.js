// Comment to funtion 1
// This function run a delegate
function Function1(delegate)
{
	delegate();
}

// Comment to funtion 2
// This function has declaration of other function
function Function2()
{
	var variable1 = 1;

	//delegate method
	var delegate = function()
	{
	};
	
	//Anonimous function
	Function1(function() {alert(1);});
}