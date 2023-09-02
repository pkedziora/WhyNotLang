var WhyNotLang = WhyNotLang || {};

WhyNotLang.Text = WhyNotLang.Text || (function () {
    function scrollOutput() {
        document.getElementById("output").scrollTop = document.getElementById("output").scrollHeight;
    }

    function setFocus(id) {
        document.getElementById(id).focus();
        return true;
    }

    function allowTextAreaTabs(textAreaId) {
        var textarea = document.getElementById(textAreaId);

        textarea.onkeydown = function (e) {
            if (e.keyCode === 9 || e.which === 9) {
                e.preventDefault();
                var start = this.selectionStart;
                this.value = this.value.substring(0, this.selectionStart) + "    " + this.value.substring(this.selectionEnd);
                this.selectionEnd = start + 4;
            }
        };
    }
    
    return {
        scrollOutput,
        setFocus,
        allowTextAreaTabs
    };
})();

