$.fn.extend({
    //className:mask  mask,mask-login
    showByMask: function (className, container) {
        var mask = $("<div></div>");
        var classNames = className.split(",");
        for (var i = 0; i < classNames.length; i += 1)
            mask.addClass(classNames[i]);
        
        if (container) mask.appendTo(container);
        else mask.appendTo(document.body);
        
        if ($(this).data("__mask__") && $(this).data("__mask__").remove) $(this).data("__mask__").remove();
        
        $(this).data("__mask__", mask);
        $(this).show();
    },
    hideByMask: function () {
        var mask = $(this).data("__mask__");
        $(this).removeData("__mask__");
        if (mask) mask.remove();
        $(this).hide();
    }
});