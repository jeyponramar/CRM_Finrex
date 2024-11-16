function IsNumber(id,IsInt)
{
    var n = $("#"+id).val();
    var result = !isNaN(parseFloat(n)) && isFinite(n);
    if(result==true)
    {
        if(n.indexOf("-")>=0)
        {
            result = false;
        }
        if(IsInt)
        {
            if(n.indexOf(".")>=0)
            {
                result = false;
            }
            
        }   
    }
    if(result==false)
    {
        alert("Invalid Number Format");
        $("#"+id).focus();
    }
    return result;
}
function GetDate(d) {
    if (d.indexOf("1900") > 0) {
        return "";
    }
    else {
        return d;
    }

}
function IsValidNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
$(document).ready(function() {
    $(".validate-int").keydown(function(event) {        
        var isvalid = false;
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            isvalid = true;
        }
        else {

            //allow 0-9
            if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) {
                isvalid = true;
            }
        }
        return isvalid;
    });

    $(".validate-date").keydown(function(event) {
        var isvalid = false;
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            isvalid = true;
        }
        else {

            //allow 0-9
            if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode == 191) || (event.keyCode == 111) || (event.keyCode >= 96 && event.keyCode <= 105)) {
                isvalid = true;
            }
        }
        return isvalid;
    });

    $(".validate-double").keydown(function(event) {
        var isvalid = false;
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            isvalid = true;
        }
        else {

            //allow 0-9 and .(dot)
            if ((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 190 || event.keyCode==110 || (event.keyCode >= 96 && event.keyCode <= 105)) {
                isvalid = true;
            }
        }
        return isvalid;
    });
    $(".validate-phone").keydown(function(event) {
        var isvalid = false;
        // Allow: backspace, delete, tab, escape, shift, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 16 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            isvalid = true;
        }
        else {
            //allow 0-9,-,+
            if ((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 109 || event.keyCode == 107 || (event.keyCode >= 96 && event.keyCode <= 105)) {
                isvalid = true;
            }
        }
        return isvalid;
    });
    $(".validate-name").keydown(function(event) {
        var isvalid = false;
        // Allow: backspace, delete, tab, escape, shift, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 16 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            isvalid = true;
        }
        else {
            //allow a-z,A-Z,SPACE,dot(.)
            if ((event.keyCode >= 65 && event.keyCode <= 90) || event.keyCode == 32 || event.keyCode == 190) {
                isvalid = true;
            }
        }
        return isvalid;
    });

});

(function($) {
    $.cookie = function(key, value, options) {

        // key and at least value given, set cookie...
        if (arguments.length > 1 && (!/Object/.test(Object.prototype.toString.call(value)) || value === null || value === undefined)) {
            options = $.extend({}, options);

            if (value === null || value === undefined) {
                options.expires = -1;
            }

            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setDate(t.getDate() + days);
            }
            value = String(value);
            return (document.cookie = [
                encodeURIComponent(key), '=', options.raw ? value : encodeURIComponent(value),
                options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
                options.path ? '; path=' + options.path : '',
                options.domain ? '; domain=' + options.domain : '',
                options.secure ? '; secure' : ''
            ].join(''));
        }

        // key and possibly options given, get cookie...
        options = value || {};
        var decode = options.raw ? function(s) { return s; } : decodeURIComponent;

        var pairs = document.cookie.split('; ');
        for (var i = 0, pair; pair = pairs[i] && pairs[i].split('='); i++) {
            if (decode(pair[0]) === key) return decode(pair[1] || ''); // IE saves cookies with empty string as "c; ", e.g. without "=" as opposed to EOMB, thus pair[1] may be undefined
        }
        return null;
    };
})(jQuery);