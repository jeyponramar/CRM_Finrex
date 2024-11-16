<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reminder.ascx.cs" Inherits="reminder" %>
<script>
    $(document).ready(function() {
        setTimeout(checkReminder, 1000);
        function checkReminder() {
            $(".reminder").find("tr").each(function() {
                var time = $(this).find(".r-time").text();
                var id = $(this).find(".r-id").text();
                var title = "Reminder - " + $(this).find(".r-subject").text();
                var dt = new Date();
                var m = parseInt(dt.getMonth()) + 1;
                var yy = dt.getFullYear();
                var day = dt.getDate();
                var rd = m + "/" + day + "/" + yy + " " + time;
                var reminderDate = new Date(rd);
                var cd = new Date();
                var diff = (cd - reminderDate) / 60000;
                if (parseInt(diff) > 0) {
                    var tabExists = false;
                    $(".tab-bar", window.top.document).find(".tabpage").each(function() {
                        var t = $(this).attr("title");
                        var page = parseInt($(this).attr("id").replace("tab-", ""));
                        if (t == title) {
                            tabExists = true;
                        }
                    });
                    if (!tabExists) {
                        loadPage("#reminder/reminder.aspx?id="+id, title);
                    }
                }
                setTimeout(checkReminder, 3000);
            });
        }

    });
</script>
<div id="reminder" title="Reminder">
<asp:Literal ID="ltReminder" runat="server"></asp:Literal>
</div>