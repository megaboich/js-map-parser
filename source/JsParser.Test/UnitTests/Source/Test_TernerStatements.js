function F1(what) {
	var c = (function () {
		        return what;
	        })()
		    ? (function () {
			    function InnerF() {
				    return 1;
			    };
			    return InnerF();
		    })()
		    : (function () {
			    function InnerF() {
				    return 2;
			    };
			    return InnerF();
		    })();
	return c;
}

function Show1() {
	alert(F1(true));
}

function Show2() {
	alert(F1(false));
}

Show1();
Show2();