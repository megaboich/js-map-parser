/*
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Working_with_Objects

When defining getters and setters using object initializers 
all you need to do is to prefix a getter method with get and a setter
method with set. Of course, the getter method must not expect a parameter,
while the setter method expects exactly one parameter (the new value to set).
For instance:
*/
var o = {
    a: 7,
    get b() {
        return this.a + 1;
    },
    set b(x) {
        this.a = x / 2
    }
};

console.log(o.a); // 7
console.log(o.b); // 8
o.c = 50;
console.log(o.a); // 25
