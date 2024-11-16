var _rhdivId = "rhist";
var _recdivId = "recommend";
var _mostOrdereddivId = "mostordered";

var href = $(location).attr('href');

var id = href.substring(href.lastIndexOf('?') + 1);

var arr = id.split('=');
var cid = arr[1];

//var pid = GetQueryString("pid");
var pid = cid;


function DisplayHistory() {

    $("#" + _rhdivId).html("<div style='margin:50px'><img src='images/ajax-loader2.gif'/></div>");
    AddRecenyHistory();
    PopulateRecentHistory();    
}
function DisplayRecommend() { 
    var objDiv = $("#" + _recdivId);
    objDiv.html("<div style='margin:50px'><img src='images/ajax-loader2.gif'/></div>");
    var P_id = ConvertToInt($(".h_productid").val());
    var products = $.cookie("rh");
    if (P_id == 0) {
        objDiv.hide();
        return;
    }
    var html = RequestData("getrecommend.ashx?pid="+pid);
    objDiv.html("<div class='container'><div class='carousel recscroll'>" + html + "</div></div>");
    $("div.recscroll").carousel();      
}
function DisplayMostOrdered() {
    var objDiv = $("#" + _mostOrdereddivId);
    objDiv.html("<div style='margin:50px'><img src='images/ajax-loader2.gif'/></div>");    
    var html = RequestData('getmostordered.ashx');
    objDiv.html("<div class='container'><div class='carousel morderscroll'>" + html + "</div></div>");
    $("div.morderscroll").carousel();
}
function AddRecenyHistory() {

    var recentHistory = $.cookie("rh"); //rh-Receny History
    var newhistory = "";
    if (recentHistory == null) {
        recentHistory = pid;
    }
    else {
        recentHistory = pid + "," + recentHistory;
    }
    newhistory = pid;
    var arr = recentHistory.split(',');
    var c = 0;
    for (i = 1; i < arr.length; i++) {
        if (arr[i] != "") {
            c++;
            if (c > 20) {
                break;
            }
            if (parseInt(pid) != parseInt(arr[i])) {
                newhistory = newhistory + "," + arr[i];
            }
        }
    }
    $.cookie("rh", newhistory, { expires: 365 });
}
function PopulateRecentHistory() {
    var objDiv = $("#" + _rhdivId);
    objDiv.html("<div style='margin:50px'><img src='images/ajax-loader2.gif'/></div>");    
    var products = $.cookie("rh");
    if (products == null) {
        objDiv.html("<div class='error'>No history found.</div>");
        return;
    }
    var arrProducts = products.split(',');
    if (arrProducts.length > 0) {
        var html = RequestData('gethistory.ashx'); 
        objDiv.html("<div class='container'><div class='carousel histscroll'>" + html + "</div></div>");
        $("div.histscroll").carousel();
    }

}
function RequestData(URL) {
    var p;
    var isAsc = false;
    //if(window.location.toLower().index("mobile/")>0)
    {
    }
    $.ajax({
        url: URL,
        type: 'GET',
        async: isAsc,
        success: function(jsonObj) {
            if ((jsonObj + "").indexOf("session expired") > 0) {
                window.location = "../userlogin.aspx";
                return;
            }
            else if ((jsonObj + "").indexOf("Error") >= 0) {
                alert("Error : \n\n" + jsonObj);
                jsonObj = "Error occurred!";
            }
            p = jsonObj;
        },
        complete: function() {
        },
        error: function(err, status, jqXHR) {
        }
    });
    return p;
}


