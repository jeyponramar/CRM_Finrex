$(document).ready(function() {
    openDemoReg();
});
function openDemoReg() {
    $("#register-freetrail").dialog({
        autoOpen: true,
        modal: true,
        width: 350,
        height: 150,
        open: function(event, ui) {
            $(".ui-dialog-titlebar-close").hide();
        },
        show: {
            effect: "blind",
            duration: 500
        },
        hide: {
            effect: "blind",
            duration: 500
        }
    });
}
function demoExpired() {
    window.location = "../logout.aspx?url=demo/sign-up.aspx";
}