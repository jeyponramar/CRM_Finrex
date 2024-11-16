<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chat-client-login.aspx.cs" Inherits="chat_chat_client" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chat</title>
    <style>
    .chat-textbox
    {
	    background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #393d3e), color-stop(1, #3d3e40));
	    background:-moz-linear-gradient(top, #3d3e40 5%, #393d3e 100%);
	    background:-webkit-linear-gradient(top, #3d3e40 5%, #393d3e 100%);
	    background:-o-linear-gradient(top, #3d3e40 5%, #393d3e 100%);
	    background:-ms-linear-gradient(top, #3d3e40 5%, #393d3e 100%);
	    background:linear-gradient(to bottom, #3d3e40 5%, #393d3e 100%);
	    filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#393d3e', endColorstr='#3d3e40',GradientType=0);
	    background-color:#768d87;
	    -moz-border-radius:5px;
	    -webkit-border-radius:5px;
	    border-radius:5px;
	    border:1px solid #566963;
	    display:inline-block;
	    cursor:pointer;
	    color:#ffffff;
	    font-family:Arial;
	    font-size:12px;
	    text-decoration:none;
	    padding-left:5px;
	    text-shadow:0px -1px 0px #2b665e;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="background-image:url(images/chat-bg.jpg); background-repeat:no-repeat;width:258px;height:215px;">
        <div style="padding:10px 0 10px 220px;"><img src="images/chat-close.jpg" /></div>
        <div style="padding:53px 0 10px 20px;">
            <asp:TextBox runat="server" id="txtName" placeholder="Your Name *" CssClass="chat-textbox" Width="220px" Height="25"></asp:TextBox>
        </div>
        <div style="padding:0px 0 8px 20px;">
            <asp:TextBox runat="server" id="txtemailid" placeholder="Email Id *" CssClass="chat-textbox" Width="220px" Height="25"></asp:TextBox>
         </div>
        <div style="padding:3px 20px 10px 20px;"><img src="images/chat-submit.jpg" /></div>
    </div>
    </form>
</body>
</html>
