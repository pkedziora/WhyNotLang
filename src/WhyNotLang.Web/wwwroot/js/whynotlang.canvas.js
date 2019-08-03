var WhyNotLang = WhyNotLang || {};

WhyNotLang.Canvas = WhyNotLang.Canvas || (function () {
    var canvas;
    var ctx;

    function initGraphics(canvasId) {
        canvas = document.getElementById(canvasId);
        ctx = canvas.getContext("2d");
    }

    function clearScreen(color) {
        ctx.fillStyle = color;
        ctx.fillRect(0, 0, canvas.width, canvas.height);
    }

    function drawRectangle(x, y, width, height, color) {
        var oldFillStyle = ctx.fillStyle;
        ctx.fillStyle = color;
        ctx.fillRect(x, y, width, height);
        ctx.fillStyle = oldFillStyle;
    }

    function drawText(text, x, y, color, font) {
        var oldFillStyle = ctx.fillStyle;
        ctx.fillStyle = color;
        ctx.font = font;
        ctx.fillText(text, x, y);
        ctx.fillStyle = oldFillStyle;
    }

    function HandleKeyDown(event) {
        DotNet.invokeMethodAsync('WhyNotLang.Web', 'OnKeyDown', event.key)
            .then(message => {
            });
    }

    function HandleKeyUp(event) {
        DotNet.invokeMethodAsync('WhyNotLang.Web', 'OnKeyUp', event.key)
            .then(message => {
            });
    }

    window.onkeydown = function (args) {
        HandleKeyDown(args);
    }

    window.onkeyup = function (args) {
        HandleKeyUp(args);
    }

    return {
        initGraphics,
        clearScreen,
        drawRectangle,
        drawText
    }
})();