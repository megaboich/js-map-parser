var MainFunction = function(input, options)
{
	// only opera doesn't trigger keydown multiple times while pressed, others don't work with keypress at all
	$input.bind("keydown.autocomplete", function(event)
	{
		//some code to process key events
	}).focus(function()
	{
		// track whether the field has focus, we shouldn't process any
		// results if the field no longer has focus
		hasFocus++;
	}).blur(function()
	{
		hasFocus = 0;
		if (!config.mouseDownOnSelect)
		{
			hideResults();
		}
	}).click(function()
	{
		// show select when clicking in a focused field
		if (hasFocus++ > 1 && !select.visible())
		{
			onChange(0, true);
		}
	});
}