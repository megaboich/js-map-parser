var d = Date.prototype;
Object.defineProperty(d, "year", {
    get: function () { return this.getFullYear() },
    set: function (y) { this.setFullYear(y) }
});