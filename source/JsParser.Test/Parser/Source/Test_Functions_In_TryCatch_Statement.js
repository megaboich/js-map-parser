
try {
    function f1() {
        alert('f1');
    }

    var f2 = function () {
        alert('f2');

        try {
            var f21 = function () {
                alert('f21');
            };
        }
        catch (e) {
            var f22 = function () {
                alert('f22');
            }

            function f23() {
                alert('f23');
            }
        }
    };

}
catch (e) {
    function f3() {
        alert('f3');
    }

    var f4 = function () {
        alert('f4');
    }
}