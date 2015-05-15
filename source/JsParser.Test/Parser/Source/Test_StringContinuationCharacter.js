function function1() {
    var hugeString1 = "blabla\
blabla";

    var hugeString2 = 'blabla\
blabla';

    var hugeString3 = 'bla "bla"\
"bla" bla';

    var hugeString4 = 'bla "bla\
bla" bla';

    var hugeString5 = "bla 'bla\
bla' bla";

    var hugeString6 = "bla 'bla'\
'bla' bla"; var hugeString7 = "bla 'bla'\
'bla' bla"; var hugeString8 = "bla 'bla'\
'bla' bla";

    var theveryHUGEMultiline = "This is probably the biggest leap\
 in functionality for JScript since the 1996 introduction of JScript version 1.0 with Internet Explorer 3.0.\
 JScript has traditionally been used to develop client-side scripts due to its ubiquitous,\
 cross-platform support on the Internet, but we've been seeing a steady increase in the\
 usage of JScript on the server—particularly in Active Server Pages (ASP).\
 For example, your favorite Web site (MSDN) uses a large amount of server-side\
 JScript, as do many other sites on the Internet.";

    function innerFunction() {
        //Not visible if previous hugeString declaration was throw error
    }

    var f = {
        a: "adffsdf\
dffdf",
        b: "adsdadasdas\
erreer",
        testFunction: function () {
        }
    };
}