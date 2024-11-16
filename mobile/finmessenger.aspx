<%@ Page Language="C#" AutoEventWireup="true" CodeFile="finmessenger.aspx.cs" Inherits="mobile_finmessenger" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="msapplication-tap-highlight" content="no" />
    <meta name="viewport" content="user-scalable=no, initial-scale=1, maximum-scale=1, minimum-scale=1, width=device-width" />
    <!-- This is a wide open CSP declaration. To lock this down for production, see below. -->
     <meta http-equiv="Content-Security-Policy" content="default-src * 'unsafe-inline' gap:; style-src * 'unsafe-inline'; media-src *" />  
    <!--<meta http-equiv="Content-Security-Policy" content="*"/>-->

	<link rel="stylesheet" type="text/css" href="onsenui/css/onsenui.min.css" />
    <link rel="stylesheet" type="text/css" href="onsenui/css/onsen-css-components.min.css" />
    <link rel="stylesheet" type="text/css" href="css/custom.css?v=1.6" />
	 
    <title>FinPulse</title>  
    <script type="text/javascript" src="onsenui/js/onsenui.min.js"></script> 
    <script type="text/javascript" src="../js/jquery.min.js"></script> 
    <script type="text/javascript" src="js/mobile.js?v=2.0.0.4"></script> 
</head>
<body>
    <svg class="progress-circular" style="top:50%;left:45%;position:absolute;">
	  <circle class="progress-circular__background"/>  
	  <circle class="progress-circular__secondary" style="stroke-dasharray: 140%, 251.32%"/>
	  <circle class="progress-circular__primary" style="stroke-dasharray: 100%, 251.32%"/>
	</svg>
    <ons-page>
    <ons-splitter>
        <ons-splitter-side id="menu" side="left" width="220px" swipe-target-width="150px" collapse swipeable>
            <ons-page>
                <div style="margin-left:0px;padding-top:10px;padding-bottom:10px;background-color:#fff;height:20px;">
                    <div style="float:left;color:#32347e;font-size:20px;padding-left:10px;">FinPulse</div>
                </div>
                <div style="clear:both;"></div>
                <ons-list>
                    <%--<ons-list-item tappable class="lnkmenu" url="latestnotification.html">Last Message</ons-list-item>--%>
                    <ons-list-item tappable class="lnkmenu" url="viewmessages.html">Messages</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="logout.html">Logout</ons-list-item>
                </ons-list>
            </ons-page>
        </ons-splitter-side>
        <ons-splitter-content id="content" page="homepage.html"></ons-splitter-content>
    </ons-splitter>
</ons-page>
<ons-template id="homepage.html">
  <ons-page id="homepage">
    <ons-navigator id="myNavigator" page="latestnotification.html"></ons-navigator>
  </ons-page>
</ons-template>

<template id="login.html">
  <ons-page id="loginpage">
        <%--<div style1="background: bottom no-repeat #181818;font-weight: 400;background-image: linear-gradient(0deg,#242424,#242424 100%);width:100%;height:30px;padding:10px;" class="toolbar">
            <div style="float:left;color:#fff;font-size:20px;">FinMessenger</div>
        </div>--%>
        
        <ons-toolbar class="toolbar">
            <div class="center toolbar__center toolbar__title" ripple="">FinPulse</div><div class="right toolbar__right"></div>
        </ons-toolbar>
      <div class="login-form form">
        
        <input type="email" class="text-input--underbar" placeholder="User Name" name="username" id="username" value=""/>
        <br/>
        <input type="password" class="text-input--underbar" placeholder="Password" name="password" id="password" value=""/>
        <br/><br/>
        <ons-button modifier="large" class="login-button btnsubmit" action="login" m="login" id="btnlogin">Log In</ons-button>
        <br/><br/>
        
      </div>

    </ons-page>
</template>

<template id="logout.html">
  <ons-page id="logout-page">
  </ons-page>
</template>

<template id="latestnotification.html">
  <ons-page id="latestnotification-page">
    <ons-toolbar>
      <div class="left">
        <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
          <ons-icon icon="md-menu"></ons-icon>
        </ons-toolbar-button>
      </div>
      <div class="center" ripple>FinPulse</div>
    </ons-toolbar>
    <div class="addpage-content">
         
    </div>
    
  </ons-page>
</template>
<template id="viewmessages.html">
  <ons-page id="viewmessages-page">
    <ons-toolbar>
      <div class="left">
        <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
          <ons-icon icon="md-menu"></ons-icon>
        </ons-toolbar-button>
      </div>
      <div class="center" ripple>View Messages</div>
    </ons-toolbar>
    <div class="addpage-content">
         
    </div>
  </ons-page>
</template>
<div style="display:none;"><input type="text" id="txtserversessionid" class="txtserversessionid" /></div>
<div class="fade"></div>
<div class="alert-modal" style="display:none;">
    <div class="alert-modal-title">Alert</div>
    <div class="alert-modal-msg"></div>
    <div class="alert-modal-ok"><input type="button" id="btnalertmodalok" value="Ok"/></div>
</div>

<div class="jq-iframe hidden"></div>
    
</body>
</html>
