var _rpluschat_ParentURL = "http://finstation.in/";
var rpluschat_body = document.getElementsByTagName("body")[0];
var rpluschat_divLogin = document.createElement("div");
rpluschat_divLogin.innerHTML = '<div id="rpluschat_login" style="display:none;background-image:url(' + _rpluschat_ParentURL + 'chat/images/chat-bg1.jpg); background-repeat:no-repeat;width:258px;height:215px;position:fixed;bottom:0px;right:0px;z-index:999999;">' +
                                    '<div style="padding:10px 0 10px 220px;"><img style="cursor:pointer" onclick="rpluschat_close()" src="' + _rpluschat_ParentURL + 'chat/images/chat-close.jpg" /></div>' +
                                    '<div style="padding:53px 0 10px 20px;">'+
                                        '<input type="text" id="rpluschat_txtusername" onclick="ph_click_name(this)" keypress="ph_click_name(this)" ph="Your Name *" value="Your Name *" style="color:#444;height:25px;width:220px;background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #393d3e), color-stop(1, #3d3e40));background:-moz-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:-webkit-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:-o-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:-ms-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:linear-gradient(to bottom, #3d3e40 5%, #393d3e 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#393d3e, endColorstr=#3d3e40,GradientType=0);background-color:#768d87;-moz-border-radius:5px;-webkit-border-radius:5px;border-radius:5px;border:1px solid #566963;display:inline-block;cursor:pointer;color:#ffffff;font-family:Arial;font-size:12px;text-decoration:none;padding-left:5px;text-shadow:0px -1px 0px #2b665e;"></input>' +
                                    '</div>'+
                                    '<div style="padding:0px 0 8px 20px;">'+
                                        '<input type="text" id="rpluschat_txtemailid" onclick="ph_click_emailid(this)" keypress="ph_click_emailid(this)" ph="Email Id *" value="Email Id *" style="color:#444;height:25px;width:220px;background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #393d3e), color-stop(1, #3d3e40));background:-moz-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:-webkit-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:-o-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:-ms-linear-gradient(top, #3d3e40 5%, #393d3e 100%);background:linear-gradient(to bottom, #3d3e40 5%, #393d3e 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#393d3e, endColorstr=#3d3e40,GradientType=0);background-color:#768d87;-moz-border-radius:5px;-webkit-border-radius:5px;border-radius:5px;border:1px solid #566963;display:inline-block;cursor:pointer;color:#ffffff;font-family:Arial;font-size:12px;text-decoration:none;padding-left:5px;text-shadow:0px -1px 0px #2b665e;"></input>' +
                                     '</div>'+
                                    '<div style="padding:3px 20px 10px 20px;"><img src="' + _rpluschat_ParentURL + 'chat/images/chat-submit.jpg" style="cursor:pointer" onclick="rpluschat_start()"/></div>' +
                                '</div>'+
                                '<div onclick="rpluschat_open()" id="rpluschat_start" style="background-image:url(' + _rpluschat_ParentURL + 'chat/images/chat-start-bg.png); background-repeat:no-repeat;width:152px;height:39px;position:fixed;bottom:0px;right:0px;z-index:999999;cursor:pointer;"></div>';
rpluschat_body.appendChild(rpluschat_divLogin);
var rpluschat_loginDiv = document.getElementById("rpluschat_login");
var rpluschat_startDiv = document.getElementById("rpluschat_start");

function rpluschat_close() {
    rpluschat_loginDiv.style.display = "none";
    rpluschat_startDiv.style.display = "block";
}
function rpluschat_open() {
    var url = _rpluschat_ParentURL + "chat/chat.aspx";
    window.open(url, "rpluschat", "width=400px,height=550px");
    //rpluschat_loginDiv.style.display = "block";
    //rpluschat_startDiv.style.display = "none";
}
function rpluschat_start() {
    var txtusername=document.getElementById("rpluschat_txtusername");
    var txtemailid=document.getElementById("rpluschat_txtemailid");
    var name = txtusername.value;
    var emailId = txtemailid.value;
    if (name == "" || name=="Your Name *") {
        alert("Please enter your name");
        txtusername.focus();
        return;
    }
    if (emailId == "" || emailId=="Email Id *") {
        alert("Please enter your email id");
        txtemailid.focus();
        return;
    }
    var filter = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (!filter.test(emailId)) {
        alert("Please enter valid email id");
        txtemailid.focus();
        return;
    }
    rpluschat_close();
    var url = _rpluschat_ParentURL + "chat/chat.aspx?name=" + name + "&emailid=" + emailId;
    window.open(url, "rpluschat", "width=400px,height=550px");
}
function ph_click_name(obj) {
    if (obj.value == "Your Name *") {
        obj.value = "";
    }
    else if (obj.value == "") {
        obj.value = "Your Name *";
    }
    obj = document.getElementById("rpluschat_txtemailid");
    if (obj.value == "") {
        obj.value = "Email Id *";
    }
}
function ph_click_emailid(obj) {
    if (obj.value == "Email Id *") {
        obj.value = "";
    }
    else if (obj.value == "") {
        obj.value = "Email Id *";
    }
    obj = document.getElementById("rpluschat_txtusername");
    if (obj.value == "") {
        obj.value = "Your Name *";
    }
}