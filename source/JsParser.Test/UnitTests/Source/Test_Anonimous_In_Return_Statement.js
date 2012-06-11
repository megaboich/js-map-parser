// Define a local copy of jQuery
var jQuery = function (selector, context) {
    // The jQuery object is actually just the init constructor 'enhanced'
    return new jQuery.fn.init(selector, context, rootjQuery, function () { });
};

function GetNewItem(param1, param2)
{
    return this.each(function (subparam) {
    });
}


