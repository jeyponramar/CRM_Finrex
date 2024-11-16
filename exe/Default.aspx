<%@ Page Language="C#" MasterPageFile="~/exe/ExeMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="exe_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script>
        var _liverateIds = "";
        $(document).ready(function() {
            setLiveRateIds();
            if(_liverateIds!="")
            {
                liverate_exe();
            }
            setInterval(function(){updateHeartBeat();},10000);
        });
        function setLiveRateIds()
        {
            $(".lr-rate").each(function(){
                var lid = $(this).attr("lid");
                if(_liverateIds=="")
                {
                    _liverateIds=lid;
                }
                else
                {
                    _liverateIds+=","+lid;
                }
            });
        }
        function updateHeartBeat(){
            var URL = "../liverate.ashx?a=exe-heartbeat&sid="+$(".hdnsessionid").val();
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                cache:false,
                success: function(response) {
                    if(response == "Session Expired")
                    {
                        window.location = "login.aspx";
                    }
                }
            });
        }
        function liverate_exe() {
            var URL = "../liverate.ashx?a=exe&lids="+_liverateIds;
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                cache:false,
                success: function(json) {
                    if (json != "") {
                        var data = jQuery.parseJSON(json);
                        for (i = 0; i < data.length; i++) {
                            var rid = data[i].rid;
                            var rate = data[i].cr;
                            //$("#lr" + rid).text(rate);
                            setUpDownStatus($("#lr" + rid), rate);
                        }
                    }
                },
                complete: function() {
                    setTimeout("liverate_exe()", 2000);
                }
            });
        }
        function setUpDownStatus(obj, rate) {
            if (!isNaN(rate)) {
                rate = parseFloat(rate);
                var prevrate = parseFloat(obj.text());
                if (rate < prevrate) {
                    obj.removeClass("rate-down").addClass("rate-mid");
                    setTimeout(function() {
                        obj.removeClass("rate-mid").addClass("rate-up");
                    }, 500);
                }
                else if (rate > prevrate) {
                    obj.removeClass("rate-up").addClass("rate-mid");
                    setTimeout(function() {
                    obj.removeClass("rate-mid").addClass("rate-down");
                    }, 500);
                }
            }
            obj.text(rate);

        }

    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:TextBox ID="hdnsessionid" runat="server" CssClass="hdnsessionid hidden"></asp:TextBox>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table width="100%">
                <tr>
                    <td>
                        <div style='float:left;position: relative;' class='jq-push-notify-panel'>
                             <i class='icon ion-ios-bell header-bell jq-header-bell'></i>
                             <div class='push-notify-msg-count'></div>
                             <div class='push-notify-msg-list'></div>
                         </div>
                     </td>
                     <td align="right" style="padding-right:10px;">
                        <asp:HyperLink ID="lnksetalert" runat="server" Text="Set Alert"></asp:HyperLink>
                     </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="border:solid 1px #d76709;">
            <asp:Literal ID="ltliverate" runat="server"></asp:Literal>
        </td>
    </tr>
</table>

</asp:Content>

