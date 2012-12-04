function test() {
    var s1 = "<sxript" type='text/javascript'> </sxropt>";

    var s2 = '<sxript type="text/javascript"> </sxropt>';

    var s3 = '<sxript/>';

    var s4 = "<sxript"/>";

    function someInnerFunc() {
        var t = 123;
    }
}