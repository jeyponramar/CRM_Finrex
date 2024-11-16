<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chat-client.aspx.cs" Inherits="chat_client1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finchat</title>
    <link href="chat.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.min.js" type="text/javascript"></script>  
    <script>
        var _noNessageCounter = 0;
        var _heartBeatCounter = 0;
        function receiveClientChat() {
            _heartBeatCounter++;
            debugger;
            var isheartbeat = false;
            if (_heartBeatCounter >= 10) {
                _heartBeatCounter = 0;
                isheartbeat = true;
            }
            var URL = "chat.ashx?a=rc&id=" + $(".hdnchatid").val() + "&cid=" + $(".hdnchatclientid").val() + "&hb=" + isheartbeat;
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                success: function(response) {
                    if (response == "" || response == "Error") {
                        _noNessageCounter++;
                    }
                    else {
                        var json = jQuery.parseJSON(response);
                        //bind chat messages
                        var messages = json.chat;
                        if (messages.length == 0) {
                            _noNessageCounter++;
                            if (_noNessageCounter >= 60) {
                                _noNessageCounter = 0;
                                $(".chat-wait-msg").remove();
                                $(".chathistory").append("<tr><td class='chat-wait-msg'>You are very important for us, please wait for some time!</td></tr>");
                                $(".chat-wait-msg").show("slow");
                                scrollToBottom();
                            }
                        }
                        else {
                            _noNessageCounter = 0;
                        }
                        for (i = 0; i < messages.length; i++) {
                            var html = "<tr><td><b>" + messages[i].name + "</b> : " + messages[i].message + "</td></tr>";
                            $(".chathistory").append(html);
                        }
                    }
                    setTimeout("receiveClientChat()", 1000);
                },
                complete: function() {

                },
                error: function(err, status, jqXHR) {
                    //alert("Error occurred while processing your request!");
                    setTimeout("receiveClientChat()", 1000);
                }
            });
        }
        function sessionExpired() {
            alert("Your session is expired, please start chat again!");
            window.close();
        }
        $(document).ready(function() {
            $("#txtmessage").focus();
            scrollToBottom();
            setTimeout("receiveClientChat()", 1000);
            setTimeout("updateHeartBeat()", 10000);

            $("#txtmessage").keypress(function(e) {
                if (e.keyCode == 13) {
                    send();
                    return false;
                }
            });
            $("#btnsendchat").click(function(){
                send();
                $("#txtmessage").focus();
            });
            $(".close-chat").click(function() {
                if (!confirm("Are you sure you want to close this chat?")) return;
                //window.location = "chatfeedback.aspx";
                window.location = "close-chat.aspx";
            });
            
        });
        function send()
        {
            if ($("#txtmessage").val() == "") return false;
            var URL = "chat.ashx?a=s&ut=c";
            $.ajax({
                url: URL,
                type: 'POST',
                async: true,
                data: $("form").serialize(),
                success: function(response) {
                    if (response == "" || response == "") {
                        alert("Error occurred while processing your request!");
                    }
                    else {
                        var html = "<tr><td><b>" + $(".hdnname").val() + "</b> : " + $("#txtmessage").val() + "</td></tr>";
                        $(".chathistory").append(html);
                        $("#txtmessage").val("");
                        scrollToBottom();
                    }
                },
                complete: function() {
                },
                error: function(err, status, jqXHR) {
                    alert("Error occurred while processing your request!");
                }
            });
        }
        function updateHeartBeat() {
            return;
            var URL = "chat.ashx?a=heartbeat&id=" + $(".hdnchatid").val();
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                success: function(response) {
                    setTimeout("updateHeartBeat()", 10000);

                },
                error: function(err, status, jqXHR) {
                    setTimeout("updateHeartBeat()", 10000);
                }
            });
        }
        function scrollToBottom() {
            $(".chathistorydiv").scrollTop($(".chathistorydiv")[0].scrollHeight);
        }
    </script>
</head>
<body style="background-color:#000;">
    <form id="form1" runat="server">
    
    <table cellpadding="0" cellspacing="0"  width="100%">
    <tr>
        <td style="padding:7px;background-color:#fff;">
            <table width="100%">
                <tr>
                    <td style="color:#32347e;font-size:25px;font-weight:bold;">Finchat</td>
                    <td align="right">
                        <div class="close-chat">
                            Close Chat
                        </div>
                    </td>
                </tr>
            </table>
        
        </td>
    </tr>
       <tr>
            <td>
                <table cellpadding="0" cellspacing="0" width="100%"> 
                    <tr>
                        <td style="background-color:#201f1f;height:35px; 
                            width:100%;text-align:center;color:#f5f5f5; font-size:14px;">
                            Welcome to Finchat
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                        <div style="width:99%;height:350px;overflow:auto;border:solid 1px #cccccc;color:#fff;" class="chathistorydiv">
                            <table class="chathistory">
                                <asp:Literal ID="ltChatHistory" runat="server"></asp:Literal>
                            </table>
                        </div>
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="hdnchatid" runat="server" CssClass="hidden hdnchatid"/>
                            <asp:TextBox ID="hdnagentid" runat="server" CssClass="hidden hdnagentid"/>
                            <asp:TextBox ID="hdnchatclientid" runat="server"  CssClass="hidden hdnchatclientid"></asp:TextBox>
                            <asp:TextBox ID="hdnname" runat="server" CssClass="hdnname hidden"/>
                            <table>
                                <tr>
                                    <td><textarea id="txtmessage" name="txtmessage" style="width:280px;height:70px;" placeholder="Type your message here..."></textarea></td>
                                    <td><input type="button" style="background-color:#000;color:#fff;border:solid 1px #ccc;border-radius:3px;padding:10px 20px 10px 20px;" value="Send" id="btnsendchat"/></td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
                </table>
            </td>
       </tr>
    </table>
    </form>
</body>
</html>
