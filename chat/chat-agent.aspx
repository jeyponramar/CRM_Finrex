<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chat-agent.aspx.cs" Inherits="chat_agent" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FinStation</title>
    <link href="chat.css" rel="stylesheet" type="text/css" />
    <link href="../css/common.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />    
    <link href="../js/upload/jquery.fileupload-ui.css" rel="stylesheet" type="text/css" />    
    <script>
        var _isMobile = false;
        var _rpluschat_ParentURL = "http://localhost:58077/CRM_Finrex/";
    </script>
    <script src="../js/jquery.min.js" type="text/javascript"></script>  
    <script src="../js/common.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/jquery.autocomplete.css" />    
	<script type="text/javascript" src="../js/jquery.autocomplete.js"></script>
	<script src="../js/upload/jquery.fileupload.js"></script>
    <script src="../js/upload/jquery.fileupload-ui.js"></script>
    <script>
        var _lastMessage = "";
        var _defaultTitle = "FinStation";
        var _isWindowFocus = true;
        var audioElement = null;
        $(function() {

            animateTitle(true);
            $(window).focus(function() {
                _lastMessage = "";
                _isWindowFocus = true;
                audioElement.pause();
            }).blur(function() {
                _isWindowFocus = false;
            });
            $(".filediv,.imgfilediv").click(function() {
                _selectedFileDiv = $(this);
            });
            $('form').fileUploadUI({
                method: 'POST',
                buildUploadRow: function(files, index, handler) {
                    return $('<tr><td>' + files[index].name + '<\/td>' +
                    '<td class="file_upload_progress"><div><\/div><\/td>' +
                    '<td class="file_upload_cancel">' +
                    '<button class="ui-state-default ui-corner-all" title="Cancel">' +
                    '<span class="ui-icon ui-icon-cancel">Cancel<\/span>' +
                    '<\/button><\/td><\/tr>');
                },
                buildDownloadRow: function(file) {
                    var fileName = file.guidfilename;
                    var arr = fileName.split('_');
                    var actualFileName = fileName;
                    if (arr.length > 1) {
                        actualFileName = arr[arr.length - 1];

                        for (i = 0; i < arr.length - 1; i++) {
                            if (i == 0) {
                                //actualFileName = arr[0];
                            }
                            else {
                                //actualFileName += "_" + arr[0];
                            }
                        }
                    }
                    var url = _rpluschat_ParentURL + "upload/chat/temp/" + fileName;
                    var msg = "";
                    if (fileName.indexOf(".jpg") > 0 || fileName.indexOf(".png") > 0 || fileName.indexOf(".bmp") > 0 || fileName.indexOf(".gif") > 0) {
                        msg = "<a href='" + url + "' target='_blank'><img src='" + url + "' height='200'/></a>";
                    }
                    else {
                        msg = "<a href='" + url + "' target='_blank'>" + actualFileName + "</a>";
                    }
                    $("#txtmessage").val(msg);
                    sendMessage();
                }
            });
        });

    </script>
    <script>
        function animateTitle(istitle) {
            istitle = !istitle;
            if (!_isWindowFocus) {
                if (istitle) {
                    $(document).attr("title", _defaultTitle);
                }
                else {
                    if (_lastMessage == "") {
                        $(document).attr("title", _defaultTitle);
                    }
                    else {
                        $(document).attr("title", _lastMessage);
                    }
                }
            }
            setTimeout("animateTitle(" + istitle + ")", 500);
        }
        function receiveAgentChat() {
            var URL = "chat.ashx?a=ra&id=" + $(".hdnchatid").val() + "&cid=" + $(".hdnchatclientid").val() + "&aid=" + $(".hdnagentid").val() + "&tid=" + $(".hdnagenttypeid").val();
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                success: function(response) {
                    if (isValidJSON(response)) {
                        var json = jQuery.parseJSON(response);
                        
                        //bind new clients
                        var newchats = json.newchat;
                        var trNewClients = "";
                        var isAnyNewChat = false;
                        for (i = 0; i < newchats.length; i++) {
                            var chatId = newchats[i].chatid;
                            var chatClientId = newchats[i].chatclientid;
                            var exists = false;
                            $(".tblnewclients").find(".newchatclient").each(function() {
                                if ($(this).attr("cid") == chatId || $(this).attr("ccid") == chatClientId) {
                                    exists = true;
                                }
                            });
                            if (exists == false) {
                                $(".tblchatlist").find(".row").each(function() {
                                    if ($(this).attr("cid") == chatId || $(this).attr("ccid") == chatClientId) {
                                        exists = true;
                                    }
                                });
                            }
                            if (!exists) {
                                isAnyNewChat = true;
                            }
                            //if (!exists) {
                            var clientName = newchats[i].clientname;
                            //var emailId = newchats[i].emailid;
                            var city = newchats[i].city;
                            var country = newchats[i].country;
                            var chat_longitude = newchats[i].longitude;
                            var chat_latitude = newchats[i].latitude;
                            var ip = newchats[i].ip;
                            var chatclientid = newchats[i].chatclientid;

                            var tr = "<tr class='row'><td class='newchatclient' cid='" + chatId + "' ccid='" + chatclientid + "'>" +
                                         "<table><tr><td class='bold clientname'>" + clientName + "</td></tr>" +
                            //"<tr><td class='emailid'>" + emailId + "</tr>" +
                                         "<tr><td class='city'>" + city + "</tr>" +
                                         "<tr><td class='ip'>" + ip + "</tr>" +
                                         "</table>" +
                                         "</td></tr>";
                            trNewClients += tr;

                        }
                        $(".tblnewclients").html("");
                        $(".tblnewclients").append($(trNewClients));
                        if (isAnyNewChat) {
                            if (!_isWindowFocus) {
                                audioElement.play();
                                setTimeout("audioElement.pause();", 5000);
                            }
                        }
                        //}
                        //bind chat messages
                        var messages = json.chat;
                        var msgtemp = "";
                        for (i = 0; i < messages.length; i++) {
                            msgtemp = messages[i].message;
                            var html = "<tr><td><b>" + messages[i].name + "</b> : " + messages[i].message + "</td></tr>";
                            $(".chathistory").append(html);
                        }
                        if (msgtemp != "") {
                            if (msgtemp.length > 20) {
                                msgtemp = msgtemp.substring();
                            }
                            _lastMessage = msgtemp;
                            if (!_isWindowFocus) {
                                audioElement.play();
                                setTimeout("audioElement.pause();", 5000);
                            }
                        }
                        var status = ConvertToInt(json.status);
                        if (status == 3) {
                            showMessage("Client has left the chat!");
                            $(".chathistory").html("");
                            $(".chatlist-row-hover").closest(".row").remove();
                        }
                        //bind chat count
                        var chatcounts = json.chatcount;
                        if (chatcounts.length == 0) {
                            $(".countdiv").remove();
                        }
                        for (i = 0; i < chatcounts.length; i++) {
                            var chatId = chatcounts[i].chatid;
                            var msgcount = chatcounts[i].msgcount;
                            var diff = ConvertToInt(chatcounts[i].diff);
                            var chatDiv = null;
                            var clientName = chatcounts[i].clientname;
                            var city = chatcounts[i].city;
                            var ip = chatcounts[i].ip;

                            $(".chatingclient").each(function() {
                                if ($(this).attr("cid") == chatId) {
                                    chatDiv = $(this);
                                }
                            });
                            if (chatDiv == null) {
                                //forwarded client
                                if (diff < 20) {
                                    var clientId = chatcounts[i].chatclientid;
                                    var tr = "<tr cid='" + chatId + "' ccid='" + clientId + "' class='chatingclient row'><td class='chatlist-row'>" +
                                             "<table><tr><td class='bold'>" + clientName + "</td>" +
                                                          "<td class='tdcount'><div class='countdiv'>" + msgcount + "</div></td></tr>" +
                                                    "<tr><td class='city'>" + city + "</td></tr>" +
                                                    "<tr><td class='ip'>" + ip + "</td></tr>" +
                                             "</table>" +
                                             "</td></tr>";
                                    $(".tblchatlist").append(tr);
                                }
                            }
                            else {

                                if (diff >= 20) {//no heartbeat from client
                                    if (parseInt(chatId) == parseInt($(".hdnchatid").val())) {
                                        showMessage("Client has left the chat!");
                                        $(".chathistory").html("");
                                    }
                                    chatDiv.remove();
                                }
                                else {
                                    if (msgcount == 0) {
                                        chatDiv.find(".countdiv").remove();
                                    }
                                    else {
                                        if (chatDiv.find(".countdiv").length == 0) {
                                            chatDiv.find(".tdcount").append("<div class='countdiv'>" + msgcount + "</div>");
                                        }
                                        else {
                                            chatDiv.find(".countdiv").text(msgcount);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    setTimeout("receiveAgentChat()", 4000);
                },
                complete: function() {

                },
                error: function(err, status, jqXHR) {
                    //alert("Error occurred while processing your request!");
                    setTimeout("receiveAgentChat()", 4000);
                }
            });
        }
        function showMessage(msg) {
            $("#lblmessage").text(msg);
        }
        $(document).ready(function() {
            setTimeout("receiveAgentChat()", 4000);
            setTimeout("updateHeartBeat()", 10000);

            $("#txtmessage").keypress(function(e) {
                if (e.keyCode == 13) {
                    sendMessage();
                    return false;
                }
            });
            $(".btnlogout").click(function() {
                return confirm("Are you sure you want to logout?");
            });
            $(".imgforward").click(function() {
                showForward();
            });
            $("#btncancelforward").click(function() {
                $(".trforward").hide();
            });
            $("#btnforward").click(function() {
                forwardChat();
            });
            $(".newchatclient").live("click", function() {
                if (!confirm("Are you sure you want to add this client to your chat list?")) return;
                var chatId = $(this).attr("cid");
                var agentId = $(".hdnagentid").val();
                var clientName = $(this).find(".clientname").text();
                var city = $(this).find(".city").text();
                var ip = $(this).find(".ip").text();
                var clientId = $(this).attr("ccid");
                var $this = $(this);

                var URL = "chat.ashx?a=addchat&id=" + chatId + "&aid=" + agentId;
                $(".chathistory").html("");
                $.ajax({
                    url: URL,
                    type: 'GET',
                    async: true,
                    success: function(response) {
                        var isvalid = false;
                        if (response == "ClientClosed") {
                            $(".hdnchatid").val("0")
                            $(".hdnchatclientid").val("0");
                            showMessage("Client has already left the chat!");
                        }
                        else if (response == "Chatting") {
                            $(".hdnchatid").val("0")
                            $(".hdnchatclientid").val("0");
                            alert("Client is chatting with some other agent!");
                        }
                        else {
                            isvalid = true;
                            $(".chatlist-row").removeClass("chatlist-row-hover");
                            var tr = "<tr cid='" + chatId + "' ccid='" + clientId + "' class='chatingclient row'><td class='chatlist-row chatlist-row-hover'>" +
                                     "<table><tr><td class='bold'>" + clientName + "</td><td class='tdcount'></td></tr>" +
                                             "<tr><td class='city'>" + city + "</td></tr>" +
                                             "<tr><td class='ip'>" + ip + "</td></tr>" +
                                     "</table>" +
                                     "</td></tr>";
                            $(".tblchatlist").append(tr);
                            $this.closest(".row").remove();
                            
                            
                            $(".hdnchatid").val(chatId)
                            $(".hdnchatclientid").val(clientId);
                            if (!isValidJSON(response)) return;

                            var json = jQuery.parseJSON(response);
                            var chat = json.chat;
                            for (i = 0; i < chat.length; i++) {
                                var name = chat[i].name;
                                var msg = chat[i].message;
                                var tr = "<tr><td><b>" + name + "</b> : " + msg + "</td></tr>";
                                $(".chathistory").append(tr);
                            }
                        }
                    }
                });
            });
            $(".chatingclient").live("click", function(e) {
                var chatId = $(this).attr("cid");
                var clientId = $(this).attr("scid");
                $(".hdnchatid").val(chatId);
                $(".hdnchatclientid").val(clientId);

                $(".chatingclient").each(function() {
                    if ($(this).attr("cid") == chatId) {
                        if ($(this).find(".countdiv").length > 0) {
                            $(this).find(".countdiv").remove();
                        }
                    }
                });
                var URL = "chat.ashx?a=getchat&id=" + chatId;
                $(".chathistory").html("");
                $(".chatlist-row").removeClass("chatlist-row-hover");
                $(this).find(".chatlist-row").addClass("chatlist-row-hover");
                $.ajax({
                    url: URL,
                    type: 'GET',
                    async: true,
                    success: function(response) {
                        if (!isValidJSON(response)) return;

                        var json = jQuery.parseJSON(response);
                        var chat = json.chat;
                        for (i = 0; i < chat.length; i++) {
                            var name = chat[i].name;
                            var msg = chat[i].message;
                            var tr = "<tr><td><b>" + name + "</b> : " + msg + "</td></tr>";
                            $(".chathistory").append(tr);
                        }
                        scrollToBottom();
                        $("#txtmessage").focus();
                    }
                });
            });
            $(".closechat").click(function() {
                if (!confirm("Are you sure you want to close this chat?")) {
                    return false;
                }
                var chatId = $(".hdnchatid").val();
                var URL = "chat.ashx?a=closechat&id=" + chatId;
                $.ajax({
                    url: URL,
                    type: 'GET',
                    async: true,
                    success: function(response) {
                        if (response == "1") {
                            $(".chathistory").html("");
                            $(".chatlist-row-hover").remove();
                        }
                        else {
                            alert("Error occurred while processing your request!");
                        }
                    }
                });
                return false;
            });
            $(".txtmessagetemplate").keypress(function(e) {
                if (e.keyCode == 13) {
                    $("#txtmessage").val($(this).val());
                    sendMessage();
                    $(this).val("");
                    return false;
                }
            });

            $(".txtproduct").keypress(function(e) {
                if (e.keyCode == 13) {
                    var productId = ConvertToInt($(".productid").val());
                    if (productId == 0) return false;
                    var URL = "chat.ashx?a=getproduct&id=" + productId;
                    $.ajax({
                        url: URL,
                        type: 'GET',
                        async: true,
                        success: function(response) {
                            if (isErrorResponse(response)) {
                                alert("Error occurred while processing your request!");
                            }
                            else {

                                $(".txtproduct").val("");
                                $("#txtmessage").val(response);
                                sendMessage();
                            }
                        }
                    });

                    return false;
                }
            });
        });
        function sendMessage() {
            if ($("#txtmessage").val() == "") return false;
            //$("#txtmessage").attr("disabled", "disabled");
            var URL = "chat.ashx?a=s&ut=a";
            $.ajax({
                url: URL,
                type: 'POST',
                async: true,
                data: $("form").serialize(),
                success: function(response) {
                    //$("#txtmessage").attr("disabled", "");
                    if (isErrorResponse(response)) {
                        alert("Error occurred while processing your request!");
                    }
                    else if (response == "ClientClosed") {
                        showMessage("Client has left the chat");
                        $("#txtmessage").val("");
                        $(".chathistory").html("");
                        $(".hdnchatid").val("0");
                        $(".hdnchatclientid").val("0");
                        $(".chatlist-row-hover").closest(".chatingclient").remove();
                    }
                    else {
                        var html = "<tr><td><b>" + $(".hdnagentname").val() + "</b> : " + $("#txtmessage").val() + "</td></tr>";
                        $(".chathistory").append(html);
                        scrollToBottom();
                        $("#txtmessage").val("");
                        //$(".hdnchatdetailid").val(response);
                    }
                },
                complete: function() {
                },
                error: function(err, status, jqXHR) {
                    //$("#txtmessage").attr("disabled", "");
                    alert("Error occurred while processing your request!");
                }
            });
        }
        function updateHeartBeat() {
            var URL = "chat.ashx?a=heartbeat-agent&aid="+$(".hdnagentid").val();
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                success: function(response) {
                    setTimeout("updateHeartBeat()", 10000);
                },
                complete: function() {
                },
                error: function(err, status, jqXHR) {
                    setTimeout("updateHeartBeat()", 10000);
                }
            });
        }
        function showForward() {
            var URL = "chat.ashx?a=online-agent&aid=" + $(".hdnagentid").val();
            $("#ddlagent").find("option").remove();
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                success: function(response) {
                    if (!isValidJSON(response)) return;
                    var json = jQuery.parseJSON(response);
                    for (i = 0; i < json.length; i++) {
                        $("#ddlagent").append("<option value=" + json[i].agentid + ">" + json[i].agentname + "</option>");
                    }
                    $(".trforward").show();
                },
                complete: function() {
                },
                error: function(err, status, jqXHR) {
                }
            });
        }
        function forwardChat() {
            var toAgent = ConvertToInt($("#ddlagent").val());
            if (toAgent == 0) {
                alert("Please select agent");
            }
            var URL = "chat.ashx?a=forward&aid=" + $(".hdnagentid").val() + "&taid=" + toAgent + "&id=" + $(".hdnchatid").val() + "&tid=" + $(".hdnagenttypeid").val();
            $.ajax({
                url: URL,
                type: 'GET',
                async: true,
                success: function(response) {
                    if (response == "1") {
                        $(".chathistory").html("");
                        $(".chatlist-row-hover").closest(".chatingclient").remove();
                        $(".trforward").hide();
                        alert("Your chat has been successfully forwarded!");
                    }
                    else {
                        alert("Error occurred while forwarding message");
                    }
                },
                complete: function() {
                },
                error: function(err, status, jqXHR) {
                }
            });
        }
        function scrollToBottom() {
            $(".chathistorydiv").scrollTop($(".chathistorydiv")[0].scrollHeight);
        }
        function isValidJSON(json) {
            if (json == null || json == undefined || json == "") return false;
            if (json.indexOf("DOCTYPE") >= 0) return false;
            return true;
        }
        function isErrorResponse(json) {
            if (json == null || json == undefined || json == "") return true;
            if (json.indexOf("DOCTYPE") >= 0) return true;
            if (json.indexOf("Error") >= 0) return true;
            return false;
        }
    </script>
    <script>
        $(document).ready(function() {
            audioElement = document.createElement('audio');
            audioElement.setAttribute('src', 'ring.mp3');
            //audioElement.setAttribute('autoplay', 'autoplay');
            $.get();
            
        });
</script>      
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:relative;display:none;z-index:1000;" class="add-menu">
        <div class="addmenudiv">
            <ul>
                <li class="qa">Quick Add</li>
            </ul>
        </div>
    </div>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="border:1px solid #cccccc;">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-color:#3b3c3e;height:35px; width:100%;text-align:center;color:#fff;border-bottom:2px solid #d71921; font-size:17px;">
                            <table width="100%">
                                <tr>
                                    <td>FinStation</td>
                                    <td align="right">
                                        <table>
                                            <tr>
                                                <td><asp:Label ID="lblAgentName" runat="server"></asp:Label></td>
                                                <td><asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="button btnlogout" OnClick="btnLogout_Click"/></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-bottom:1px solid #cccccc;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="vertical-align:top;padding-top:5px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="width:500px;height:400px;overflow:scroll;border:solid 1px #cccccc" class="chathistorydiv">
                                                        <table class="chathistory" width="100%">
                                                            <asp:Literal ID="ltChatHistory" runat="server"></asp:Literal>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="hdnchatid" runat="server" CssClass="hidden hdnchatid"/>
                                                                <asp:TextBox ID="hdnagentid" runat="server" CssClass="hidden hdnagentid"/>
                                                                <asp:TextBox ID="hdnagenttypeid" runat="server"  CssClass="hidden hdnagenttypeid"></asp:TextBox>
                                                                <asp:TextBox ID="hdnchatclientid" runat="server"  CssClass="hidden hdnchatclientid"></asp:TextBox>
                                                                <asp:TextBox ID="hdnname" runat="server" CssClass="hdnagentname hidden"/>
                                                                <textarea id="txtmessage" name="txtmessage" rows="10" cols="20" style="width:300px;height:50px;"></textarea>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td style="vertical-align:top;">
                                                                <table>
                                                                    <tr>
                                                                        <td class="chat-forward">
                                                                            <div style="margin:auto;text-align:center;">
                                                                                <img src="images/chat-forward.png" class="imgforward hand" title="Forwad chat to other agent"/>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="chat-forward">
                                                                            <div style="margin:auto;text-align:center;">
                                                                                <img src="images/chat-close.png" class="closechat hand" title="Close this chat"/>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr class="trforward hidden">
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>Forward to Agent</td>
                                                            <td><select id="ddlagent"></select></td>
                                                            <td><input type="button" id="btnforward" class="button" value="Forward"/></td>
                                                            <td><input type="button" id="btncancelforward" value="Cancel" class="cancel"/></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table>
                                                    <tr><td>
                                                    <uc:MultiFileUpload ID="mfuattachment"  FolderPath="upload/chat" IsMutiple="false" FileType="Any" runat="server" CssClass="textbox "></uc:MultiFileUpload>
                                                    </td></tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="vertical-align:top;">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="vertical-align:top;width:200px;border-right:1px solid #cccccc;border-left:1px solid #cccccc;">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr><td class="chat-waiting">Online Clients</td></tr>
                                                    <tr>
                                                        <td style="height:375px;vertical-align:top;">
                                                            <table class="tblchatlist" width="100%">
                                                                <asp:Literal ID="ltChatList" runat="server"></asp:Literal>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align:top;width:200px;">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr><td class="chat-waiting">Waiting Clients</td></tr>
                                                    <tr>
                                                        <td style="vertical-align:top;border-right:1px solid #cccccc;height:375px;">
                                                            <table class="tblnewclients" width="100%">
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="border:1px solid #cccccc;">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr><td class="chat-waiting" colspan="2">Template</td></tr>
                                                                    <tr>
                                                                       <td>
                                                                            <table>
                                                                                <tr>
                                                                                     <td class="label" style="width:60px;">Message</td>
                                                                                     <td>
                                                                                        <asp:TextBox ID="messagetemplate" runat="server" m="messagetemplate" cn="template" CssClass="textbox ac txtqa txtmessagetemplate"></asp:TextBox>
                                                                                        <asp:TextBox id="txtmessagetemplateid" Text="0" runat="server" class=" hdnac hdnqa messagetemplateid"/>
                                                                                        <img src="../images/down-arr1.jpg" class="quick-menu  epage"/>
                                                                                     </td>
                                                                                </tr>
                                                                            </table>
                                                                       </td>
                                                                    </tr>
                                                                    <%--<tr>
                                                                       <td>
                                                                            <table>
                                                                                <tr>
                                                                                     <td class="label" style="width:60px;">Product</td>
                                                                                     <td>
                                                                                        <asp:TextBox ID="TextBox1" runat="server" m="product" cn="productname" CssClass="textbox ac txtqa txtproduct"></asp:TextBox>
                                                                                        <asp:TextBox id="txtproductid" Text="0" runat="server" class=" hdnac hdnqa productid"/>
                                                                                        <img src="../images/down-arr1.jpg" class="quick-menu  epage"/>
                                                                                     </td>
                                                                                     
                                                                                </tr>
                                                                            </table>
                                                                       </td>
                                                                    </tr>--%>
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="vertical-align:top;padding:10px">
                                        <div class="error" id="lblmessage" style="font-size:15px;font-weight:bold;"></div>
                                    </td>
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
