
Pen = function (tag) {
    this.dir = -90;
    this.x = 0;
    this.y = 0;

    this.tag = document.getElementById(tag) || tag;

    this.canvas = this.tag.getContext("2d");
    this.strokeStyle = this.canvas.strokeStyle = "#000";
    this.lineWidth = this.canvas.lineWidth = 1;
    this.fillStyle = this.canvas.fillStyle = "";

    this.ox = 0;
    this.oy = 0;

    this.pen = true;

    this.canvas.clearRect(0, 0, this.tag.width, this.tag.height);
    var w = this.tag.width;
    this.tag.width = 1;
    this.tag.width = w;

    this.canvas.beginPath();
};
Pen.prototype = {
    height: function () {
        return this.tag.height;
    },

    goto: function (x, y) {
        this.x = x;
        this.y = y;

        if (!this.pen)
            this.canvas.moveTo(x, y);
        else
            this.canvas.lineTo(x, y);

        return this;
    },

    jump: function (x, y) {
        this.canvas.beginPath();

        var p = this.pen;
        this.pen = true;
        this.goto(x, y);
        this.pen = p;

        return this;
    },

    height: function () {
        return this.tag.height;
    },

    center: function () {
        return this.goto(this.width() / 2, this.height() / 2);
    }
};