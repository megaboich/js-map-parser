
// This function show alert if browser is IE
var alertIsIEFunc = null;

if (jQuery.browser.msie)
{
	// ie- specific implementation
	alertIsIEFunc = function()
	{
		alert('This is IE');
	}
}
else
{
	// html standart implementation
	alertIsIEFunc = function()
	{
		alert('This is not IE');
	}
}