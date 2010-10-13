/*
The sample of JQuery plugin code style
*/

; (function($)
{
	$.fn.extend({
		autocomplete: function(urlOrData, options)
		{
			// if highlight is set to false, replace it with a do-nothing function
			options.highlight = options.highlight || function(value) { return value; };

			return this.each(function()
			{
				new $.Autocompleter(this, options);
			});
		},
		result: function(handler)
		{
			return this.bind("result", handler);
		},
		unautocomplete: function()
		{
			return this.trigger("unautocomplete");
		},
		test1Function: function()
		{
		},
		TestFunctions: function()
		{
		},
		VeryCreepyNameFunction :function()
		{
		}
	});

	$.Autocompleter = function(input, options)
	{
		// prevent form submit in opera when selecting with return key
		$.browser.opera && $(input.form).bind("submit.autocomplete", function()
		{
			if (blockSubmit)
			{
				blockSubmit = false;
				return false;
			}
		});

		// only opera doesn't trigger keydown multiple times while pressed, others don't work with keypress at all
		$input.bind(($.browser.opera ? "keypress" : "keydown") + ".autocomplete", function(event)
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


		function selectCurrent()
		{
			//blah blah
		};

		function stopLoading()
		{
			$input.removeClass(options.loadingClass);
			$input.addClass(options.defaultClass);
		};
	};

	$.Autocompleter.defaults = {
		inputClass: "ac_input",
		formatItem: function(row) { return row[0]; },
		formatMatch: function(row) { return row[0]; },
		highlight: function(value, term)
		{
			return value.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + term.replace(/([\^\$\(\)\[\]\{\}\*\.\+\?\|\\])/gi, "\\$1") + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
		},
		scroll: true,
		scrollHeight: 180
	};

	$.Autocompleter.Cache = function(options)
	{
		var data = {};
		var length = 0;
		var populated = false;

		function matchSubset(s, sub)
		{
			//blah blah
		};

		function populate(excludeDataSet)
		{
			var stMatchSets = {};
			
			// add the data items to the cache
			$.each(stMatchSets, function(i, value)
			{
				// increase the cache size
				options.cacheLength++;
				// add to the cache
				add(i, value);
			});
		}

		function flush()
		{
			data = {};
			length = 0;
		}

		return {
			flush: flush,
			add: add,
			populate: populate,
			load: function(q, excludeDataSet)
			{
				return null;
			}
		};
	};
})(jQuery);