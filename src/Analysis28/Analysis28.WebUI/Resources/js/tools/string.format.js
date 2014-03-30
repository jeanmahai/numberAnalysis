if (!String.format) {
    String.format = function (str, params) {
        var i;
        var result = str;
        if (params instanceof Array) {
            for(i=0;i<params.length;i++) {
                result = result.replace(new RegExp("\\{" + (i) + "\\}", "g"), params[i]);
            }
        }
        else {
            for (i = 1; i < arguments.length; i++) {
                result = result.replace(new RegExp("\\{" + (i - 1) + "\\}", "g"), arguments[i]);
            }
        }

        return result;
    };
}
