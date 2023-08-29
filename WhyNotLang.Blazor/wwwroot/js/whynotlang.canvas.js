var WhyNotLang = WhyNotLang || {};

WhyNotLang.Canvas = WhyNotLang.Canvas || (function () {
    var canvas;
    var ctx;

    function initGraphics(canvasId) {
        canvas = document.getElementById(canvasId);
        ctx = canvas.getContext("2d");
    }

    function clearScreen() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
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

    function handleKeyDown(event) {
        DotNet.invokeMethodAsync('WhyNotLang.Blazor', 'OnKeyDown', event.key)
            .then(message => {
            });
    }

    function handleKeyUp(event) {
        DotNet.invokeMethodAsync('WhyNotLang.Blazor', 'OnKeyUp', event.key)
            .then(message => {
            });
    }

    window.onkeydown = function (args) {
        handleKeyDown(args);
    }

    window.onkeyup = function (args) {
        handleKeyUp(args);
    }

    return {
        initGraphics,
        clearScreen,
        drawRectangle,
        drawText
    }
})();