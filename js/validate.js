function IsNumber(id, IsInt) {
    var n = $("#" + id).val();
    var result = !isNaN(parseFloat(n)) && isFinite(n);
    if (result == true) {
        if (n.indexOf("-") >= 0) {
            result = false;
        }
        if (IsInt) {
            if (n.indexOf(".") >= 0) {
                result = false;
            }

        }
    }
    if (result == false) {
        alert("Invalid Number Format");
        $("#" + id).focus();
    }
    return result;
}
function validatePage() {
    var isvalid = true;
    //if (!isValidForm()) return false;
    $(".val-mobile").each(function(event) {
        if (isvalid) {
            var mobNo = $(this).val();
            if (mobNo == "") return;
            if (mobNo.length != 10) {
                alert("Invalid mobile number!");
                $(this).focus();
                isvalid = false;
            }
            for(var i=0;i<mobNo.length;i++)
            {
                if(isNaN(parseInt(mobNo[i]))){
                    alert("Invalid mobile number!");
                    isvalid=false;
                    $(this).focus();
                    break;
                }
            } 
        }
    });
    if (isvalid) {
        $(".val-email").each(function(event) {
            if (isvalid) {
                if ($(this).val() == "") return;
                var arr = $(this).val().split(',');
                for (i = 0; i < arr.length; i++) {
                    if (isValidEmailAddress(arr[i]) == false) {
                        isvalid = false;
                        alert("Invalid email id!");
                        $(this).focus();
                    }
                }
            }
        });
    }
    $(".mchk").each(function() {
        //isrequired
        if ($(this).closest("table").closest("td").prev().find(".error").length > 0) {
            if ($(this).find("input:checked").length == 0) {
                alert("Please select atleast one option in " + $(this).closest("table").closest("td").prev().text().replace("*",""));
                isvalid = false;
            }
        }
    });
    return isvalid;
}
$(document).ready(function() {
    $("form").submit(function() {
        var isvalid = true;
        try {
            isvalid = Page_IsValid;
        } catch (e) { }
        if (!validatePage()) return false;
        if (isvalid) {
            var isprocessing = true;
            $(":submit").each(function() {
                var processing = ConvertToString($(this).attr("processing"));
                if (processing == "false") {
                    isprocessing = false;
                }
            });
            if (isprocessing) {
                $(":submit").each(function() {
                    $(this).attr("caption", $(this).val());
                    $(this).val("Processing...");
                });
            }
        }
    });
    $(".noprocessing").click(function() {
        setTimeout(function() {
            $(":submit").each(function() {
                if ($(this).attr("caption") != undefined) {
                    $(this).val($(this).attr("caption"));
                }
            });
        }, 1000);
    });
    $(":submit").click(function() {
        if (!validatePage()) return false;
        if ($(this).val() == "Processing...") {
            alert("Processing your request, please wait!");
            return false;
        }
    });
    $(":submit").click(function(e) {
        if (!validatePage()) return false;
        if (event.keyCode == undefined) return;
        if (event.keyCode == 13) {
            if ($(this).val() == "Processing...") {
                alert("Processing your request, please wait!");
                return false;
            }
        }
    });
    $(".btnexport").live("click", function() {
        setTimeout(function() {
            $(":submit").each(function() {
                if ($(this).attr("caption") != undefined) {
                    $(this).val($(this).attr("caption"));
                }
            });
        }, 1000);
    });
    $(".val-i").live("keydown", function(event) {
        var isvalid = false;
        if (event.keyCode == 17 || event.keyCode == 86 || event.keyCode == 67) return true; //copy paste
        //allow down and up arrow
        if (event.keyCode == 38 || event.keyCode == 40) return true;

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

            //allow 0-9 and minus sign
            if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 189 || event.keyCode == 109) {
                isvalid = true;
            }
        }
        return isvalid;
    });
    $(".val-mobile").live("keydown", function(event) {
        var isvalid = false;
        //allow down and up arrow
        if (event.keyCode == 38 || event.keyCode == 40) return true;

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
    $(".val-time").live("keydown", function(event) {
        var isvalid = false;
        //allow down and up arrow
        //if(event.keyCode != 16)alert(event.keyCode);
        if (event.keyCode == 59 || event.keyCode == 16 || event.keyCode == 186) return true;
        if (event.keyCode == 38 || event.keyCode == 40) return true;

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

    $(".val-dt").live("keydown", function(event) {
        var isvalid = false;
        //for key -
        if (event.keyCode == 189) return true;
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
    $(".val-dt").live("blur", function(event) {
        var dt = $(this).val();
        if (dt == "") return true;
        dt = dt.replace("/", '-').replace("/", '-');
        var arr = dt.split('-');
        if (arr.length == 3) {
            try {
                if (arr[2].length != 4) {
                    $(this).focus();
                    return false;
                }

                var m = ConvertToInt(arr[1]) - 1;
                var y = ConvertToInt(arr[2]);
                var d = ConvertToInt(arr[0]);
                var date = new Date(y, m, d);
                if (y == date.getFullYear() && m == date.getMonth() && d == date.getDate()) {
                    return true;
                }
            }
            catch (ex) {
            }
        }
        $(this).focus();
        return false;
    });
    $(".val-dbl").live("keydown", function(event) {
        var isvalid = false;
        if (event.keyCode == 17 || event.keyCode == 86 || event.keyCode == 67) return true; //copy paste
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

            //allow 0-9 and .(dot) and -
            if (event.keyCode >= 189 || (event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 190 || event.keyCode == 110 || (event.keyCode >= 96 && event.keyCode <= 105)) {
                isvalid = true;
            }
        }
        return isvalid;
    });
    $(".val-ph").live("keydown", function(event) {
        var isvalid = false;
        // Allow: backspace, delete, tab, escape, shift, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 16 || event.keyCode == 13 ||
        // Allow: Ctrl+A                                            CTRL+P
            (event.keyCode == 65 && event.ctrlKey === true) || (event.keyCode == 86) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            isvalid = true;
        }
        else {
            //allow 0-9,-,+
            if ((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 109 || event.keyCode == 107) {
                //|| (event.keyCode >= 96 && event.keyCode <= 105)) {
                isvalid = true;
            }
        }
        return isvalid;
    });
    $(".val-nm").live("keydown", function(event) {
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
    $(".val-i").live("blur", function(event) {
        return isValidInt($(this));
    });
    $(".val-dbl").live("blur", function(event) {
        return isValidDbl($(this));
    });
});
function isValidInt(obj) {
    var n = obj.val();
    if (n.trim() == "") return true;
    n = n.replace("-", "");
    var result = !isNaN(parseInt(n)) && isFinite(n);
    if (!result) {
        obj.focus();
        return false;
    }
    return true;
}
function isValidDbl(obj) {
    var n = obj.val();
    if (n.trim() == "") return true;
    n = n.replace("-", "").replace(",","");
    var result = !isNaN(parseFloat(n)) && isFinite(n);
    if (!result) {
        obj.focus();
        return false;
    }
    return true;
}
function isValidForm() {
    var isvalid = true;
    $(".val-i,.val-dbl").each(function() {
        if (isvalid) {
            var n = $(this).val();
            if (n.trim() == "") return;
            n = n.replace("-", "");
            var result = !isNaN(parseFloat(n)) && isFinite(n);
            if (result == true) {
                if (n.indexOf("-") >= 0) {
                    result = false;
                }
                if ($(this).hasClass("val-i")) {
                    if (n.indexOf(".") >= 0) {
                        result = false;
                    }
                }
            }
            if (!result) {
                $(this).focus();
                isvalid = false;
            }
        }
    });
    return isvalid;
}
function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
    return pattern.test(emailAddress);
};
function validate(table) {
    var isval = true;
    var errmsg = "Please fillup the following fields!\n\n";
    var ddl;
    var txt;
    var isfirst = true;
    table.find("select").each(function() {
        if ($(this).parent().parent().attr("class") != "newrow") {
            var ir = $(this).attr("ir"); //required fileds
            var id = $(this).attr("id");
            if (ir == "1") {

                if ($(this).val() == "0") {
                    if (isfirst) {
                        isfirst = false;
                        ddl = $(this);
                    }
                    if ($(this).attr("msg") == undefined) {
                        errmsg += "Select " + id.replace("Id", "") + "\n";
                    }
                    else {
                        errmsg += $(this).attr("msg") + "\n";
                    }
                    isval = false;
                }
            }
        }
    });
    isfirst = true;
    table.find("input").each(function() {
        if ($(this).parent().parent().attr("class") != "newrow") {
            if ($(this).attr("type") == "text" || $(this).attr("type") == "password") {
                var ir = $(this).attr("ir"); //required fileds
                var id = $(this).attr("id");
                if (ir == "1") {
                    if ($.trim($(this).val()) == "") {
                        if (isfirst) {
                            isfirst = false;
                            txt = $(this);
                        }
                        if ($(this).attr("msg") == undefined) {
                            errmsg += id + " is required\n";
                        }
                        else {
                            errmsg += $(this).attr("msg") + "\n";
                        }
                        isval = false;
                    }
                }
            }
        }
    });
    if (ddl == undefined) {
        if (txt != undefined) {
            txt.focus();
        }
    }
    else {
        if (txt == undefined) {
            ddl.focus();
        }
        else {
            if (parseFloat(ddl.position().top) > parseFloat(txt.position().top)) {
                txt.focus();
            }
            else {
                ddl.focus();
            }
        }
    }
    if (isval) {
        return true;
    }
    else {
        alert(errmsg);
        return false;
    }
}
