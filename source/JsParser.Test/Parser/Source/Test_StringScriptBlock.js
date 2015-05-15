function test() {
    var s1 = "<script type='text/javascript'> </script>";

    var s2 = '<script type="text/javascript"> </script>';

    var s3 = '<script/>';

    var s4 = "<script/>";

    function someInnerFunc() {
        var t = 123;
    }
}