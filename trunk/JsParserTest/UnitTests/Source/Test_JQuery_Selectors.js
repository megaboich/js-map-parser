
//Attach on document ready
$(document).ready(function () {
    AttachEvents();
});

// Comment to funtion 1
// This function attaches events to various elements on the page.
function AttachEvents() {
    $('#btnSubmit').click(function () {
        alert('Submit button click');
    });

    $('#btnSearchWithLongSelector div a .RealyLongSelectorIsHere').click(function () {
        alert('Submit button click');
    });

    $('#btnBack').click(function () {
        alert('Back button click');
    });

    $('.btnBack').each(function (index, elt) {
        alert("yo!");
    });

    $('.btnBack').eq(0).bind('mousedown', function () {
        alert("mousedown");
    });
}