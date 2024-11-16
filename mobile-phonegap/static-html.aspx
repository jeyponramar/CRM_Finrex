<%@ Page Language="C#" AutoEventWireup="true" CodeFile="static-html.aspx.cs" Inherits="mobile_phonegap_static_html" 
EnableViewState="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <page-start></page-start>
    
    <svg class="progress-circular" style="top:50%;left:45%;position:absolute;">
	  <circle class="progress-circular__background"/>  
	  <circle class="progress-circular__secondary" style="stroke-dasharray: 140%, 251.32%"/>
	  <circle class="progress-circular__primary" style="stroke-dasharray: 100%, 251.32%"/>
	</svg>
        <div id="jq-menu_html" style="display:none;">
            <ons-page id="jq-finicon-menu">
                <div style="margin-left:0px;padding-top:10px;padding-bottom:10px;background-color:#fff;height:20px;">
                    <div style="float:left;color:#32347e;font-size:20px;padding-left:10px;">FinIcon</div>
                </div>
                <div style="clear:both;"></div>
                <ons-list>
                    <ons-list-item tappable class="lnkmenu-metstation">MetStation</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="dashboard.html">Spot Rate</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="forward.html">Forward Rate</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="indices.html">Indices</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="brokendatecalc.html">Broken Date Calculation</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="essentialreading.html">Essential Reading</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="setalert.html">Set Alert</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="logout.html">Logout</ons-list-item>
                </ons-list>
            </ons-page>
            <ons-page id="jq-metstation-menu" style="display:none;">
                <div style="margin-left:0px;padding-top:10px;padding-bottom:10px;background-color:#fff;height:20px;">
                    <div style="float:left;color:#32347e;font-size:20px;padding-left:10px;">MetStation</div>
                </div>
                <div style="clear:both;"></div>
                <ons-list>
                    <ons-list-item tappable class="lnkmenu-finstation">FinIcon</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="metal-dashboard.html">LME - 3M Forward</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="metal-3m-cashspread.html">LME Cash-3M spread rates</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="metal-settlementrate.html">LME - SETTLEMENT RATE US$/TONNE</ons-list-item>
                    <%--<ons-list-item tappable class="lnkmenu" url="metal-stock.html">LME Warehouse Stock</ons-list-item>--%>
                    <ons-list-item tappable class="lnkmenu" url="metal-currencies.html">Currencies</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="metal-indices-commodities.html">INDICES AND COMMODITIES</ons-list-item>
                    <ons-list-item tappable class="lnkmenu" url="logout.html">Logout</ons-list-item>
                </ons-list>
            </ons-page>
          </div>
          
<ons-template id="homepage.html">
  <ons-page id="homepage">
    <ons-navigator id="myNavigator" page="login.html"></ons-navigator>
  </ons-page>
</ons-template>

<template id="login.html">
  <ons-page id="loginpage" style="display:none;">
        <div style="background-color:#fff;width:100%;height:30px;padding:10px;">
            <div style="float:left;color:#32347e;font-size:20px;">FinIcon</div>
        </div>
        <br />
      <div class="login-form form">
        
        <input type="email" class="text-input--underbar" placeholder="User Name" value="" name="username" id="username"/>
        <br/>
        <input type="password" class="text-input--underbar" placeholder="Password" value="" name="password" id="password"/>
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
    <template id="dashboard.html">
      <ons-page id="dashboard-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>FinIcon</div>
          <div class="right">
            <div style='position: relative;margin-right: 40px;margin-top: 15px;float:right;' class='jq-push-notify-panel'>
                 <i class='icon ion-ios-bell header-bell jq-header-bell' style="font-size:20px;"></i>
                 <div class='push-notify-msg-count'></div>
                 <div class='push-notify-msg-list'></div>
             </div>
          </div>
        </ons-toolbar>
	    <div class="addpage-content">
	        <div style="background-color:#222;width:100%;height:32px;">
	            <table width="100%">
	                <tr>
	                    <td style="font-size:14px;padding:10px;width:90px;">SPOT RATE</td>
	                    
	                    <td style="font-size:12px;padding:10px;" align="right">
	                        Spot Date: 
	                            <span class="lr-rate date" lrid="1048" style="background-color:transparent;" id="lblspotdate" runat="server"></span>
	                    </td>
	                    <td style="padding-top:5px;padding-bottom:5px;padding-right:10px;"><img src="https://finstation.in/images/plus-add-white.png" class="jq-img lnkmenu" width="20" href="config-currency.html"/></td>
	                </tr>
	            </table>
	            
	        </div>
		    <div class="jq-liverate" style="overflow:scroll;clear:both;">
		        <div class="jq-spotrate"><asp:Literal ID="ltspotrate" runat="server"></asp:Literal></div>
		        <asp:Literal ID="ltoffshore" runat="server"></asp:Literal>
		    </div>
	    </div>
        <div style="display:none;">
            <asp:Literal ID="lthiddenliverates" runat="server"></asp:Literal>
        </div>
        <%--hidden liverates for calculation--%>
        <div class="hidden test">
            <asp:Literal ID="ltUSDINR" runat="server"></asp:Literal>
            <asp:Literal ID="ltEURINR" runat="server"></asp:Literal>
            <asp:Literal ID="ltGBPINR" runat="server"></asp:Literal>
            <asp:Literal ID="ltJPYINR" runat="server"></asp:Literal>
            <asp:Literal ID="ltEURUSD" runat="server"></asp:Literal>
            <asp:Literal ID="ltGBPUSD" runat="server"></asp:Literal>
            <asp:Literal ID="ltUSDJPY" runat="server"></asp:Literal>
        </div>
      </ons-page>
    </template>
    <template id="forward.html">
      <ons-page id="forward-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>FORWARD RATE</div>
        </ons-toolbar>
        <ons-tabbar swipeable position="auto">
            <ons-tab page="forwardrate-usdinr.html" label="USDINR" active></ons-tab>
            <ons-tab page="forwardrate-eurinr.html" label="EURINR"></ons-tab>
            <ons-tab page="forwardrate-gbpinr.html" label="GBPINR"></ons-tab>
            <ons-tab page="forwardrate-eurusd.html" label="EURUSD"></ons-tab>
            <ons-tab page="forwardrate-gbpusd.html" label="GBPUSD"></ons-tab>
         </ons-tabbar>
      </ons-page>
    </template>
    <template id="forwardrate-usdinr.html">
      <ons-page id="forwardrate-usdinr-page">
	    <div class="addpage-content" type="usdinr">
		    <div class="jq-liverate">
                <asp:Literal ID="ltforwardrate_usdinr" runat="server"></asp:Literal>
		    </div>
	    </div>
      </ons-page>
    </template>
    <template id="forwardrate-eurinr.html">
      <ons-page id="forwardrate-eurinr-page">
	    <div class="addpage-content" type="eurinr">
		    <div class="jq-liverate">
                <asp:Literal ID="ltforwardrate_eurinr" runat="server"></asp:Literal>
                
		    </div>
	    </div>
      </ons-page>
    </template>
    <template id="forwardrate-gbpinr.html">
      <ons-page id="forwardrate-gbpinr-page">
	    <div class="addpage-content">
		    <div class="jq-liverate">
               <asp:Literal ID="ltforwardrate_gbpinr" runat="server"></asp:Literal>
		    </div>
	    </div>
      </ons-page>
    </template>
    <template id="forwardrate-eurusd.html">
      <ons-page id="forwardrate-eurusd-page">
	    <div class="addpage-content">
		    <div class="jq-liverate">
                <asp:Literal ID="ltforwardrate_eurusd" runat="server"></asp:Literal>
		    </div>
	    </div>
      </ons-page>
    </template>
    <template id="forwardrate-gbpusd.html">
      <ons-page id="forwardrate-gbpusd-page">
	    <div class="addpage-content">
		    <div class="jq-liverate">
                <asp:Literal ID="ltforwardrate_gbpusd" runat="server"></asp:Literal>
		    </div>
	    </div>
      </ons-page>
    </template>
    <template id="indices.html">
      <ons-page id="indices-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>Indices</div>
        </ons-toolbar>
	    <div class="addpage-content">
		    <div class="jq-liverate">
                <table width='100%' cellpadding='0' cellspacing='0'>
		            <tr class='lr-header'><td>Indices</td><td>LTP</td><td>Change</td><td>% Change</td></tr>
		            <tr>
		                <td><div>Dollar Index</div></td>
                        <td><div class='lr-rate' lrid='1193'></div></td>
                        <td><div class='lr-rate' lrid='1194'></div></td>
                        <td><div class='lr-rate' lrid='1195'></div></td>
                    </tr>
                    <tr>
		                <td><div>SENSEX</div></td>
                        <td><div class='lr-rate' lrid='163'></div></td>
                        <td><div class='lr-rate' lrid='164'></div></td>
                        <td><div class='lr-rate' lrid='1106'></div></td>
                    </tr>
                    <tr>
		                <td><div>NIFTY</div></td>
                        <td><div class='lr-rate' lrid='1107'></div></td>
                        <td><div class='lr-rate' lrid='1108'></div></td>
                        <td><div class='lr-rate' lrid='1109'></div></td>
                    </tr>
                    <tr>
		                <td><div>NIKKEI</div></td>
                        <td><div class='lr-rate' lrid='1110'></div></td>
                        <td><div class='lr-rate' lrid='1111'></div></td>
                        <td><div class='lr-rate' lrid='1112'></div></td>
                    </tr>
                    <tr>
		                <td><div>HANGSENG</div></td>
                        <td><div class='lr-rate' lrid='1113'></div></td>
                        <td><div class='lr-rate' lrid='1114'></div></td>
                        <td><div class='lr-rate' lrid='1115'></div></td>
                    </tr>
                    <tr>
		                <td><div>DAX</div></td>
                        <td><div class='lr-rate' lrid='1116'></div></td>
                        <td><div class='lr-rate' lrid='1117'></div></td>
                        <td><div class='lr-rate' lrid='1118'></div></td>
                    </tr>
                    <tr>
		                <td><div>FTSE</div></td>
                        <td><div class='lr-rate' lrid='1119'></div></td>
                        <td><div class='lr-rate' lrid='1120'></div></td>
                        <td><div class='lr-rate' lrid='1121'></div></td>
                    </tr>
		        </table>
		    </div>
	    </div>
      </ons-page>
    </template>
    <template id="essentialreading.html">
      <ons-page id="essentialreading-page">
         <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>Essential Reading</div>
        </ons-toolbar>
	    <div class="addpage-content white-page" style="background-color:#fff;">
            <asp:Literal ID="ltessentialreading" runat="server"></asp:Literal>
	    </div>
      </ons-page>
    </template>
    <template id="brokendatecalc.html">
      <ons-page id="brokendatecalc-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>Broken Date Calculation</div>
        </ons-toolbar>
	    <div class="addpage-content">
	       <table cellspacing="5" width="100%">
            <tr>
                <td width="120">Spot Date</td>
                <td id="broken-sportdate"></td>
            </tr>
            <tr>
                <td>Conver Type</td>
                <td>
                    <asp:RadioButtonList ID="rbtnConverType" runat="server" RepeatDirection="Horizontal" CssClass="broken-covertype">
                        <asp:ListItem Text="Exports" Selected="True" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Imports" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>Select Date</td>
                <td><input type="text" class="datepicker" id="broken-txtbdate" style="width:100px;"/>
                <ons-input type="date" class="ons-datepicker" style="position:absolute;left1:-1000px;width:0px;"></ons-input>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td>Select Currency</td>
                <td>
                    <select id="broken-ddlcurrency" style="width:104px;height:25px;">
                        <option value="1" selected="selected" targetrateid="1029">USDINR</option>
                        <option value="2" targetrateid="1032">EURINR</option>
                        <option value="3" targetrateid="1035">GBPINR</option>
                        <option value="4" targetrateid="1038">JPYINR</option>
                        <option value="5" targetrateid="1041">EURUSD</option>
                        <option value="6" targetrateid="1044">GBPUSD</option>
                        <option value="7" targetrateid="1054">USDJPY</option>
                    </select>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td></td>
                <td><input type="button" id="broken-btncalc" value="Calculate" style="padding:6px 10px 6px 10px;"/></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td colspan="2" style="font-size:14px;"><table width="100%"><tr><td id="broken-result"></td></tr></table></td></tr>
        </table> 
	    </div>
      </ons-page>
    </template>
    <template id="setalert.html">
      <ons-page id="setalert-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>Set Alert</div>
        </ons-toolbar>
	    <div class="addpage-content">
	         <table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
				<tr>
					<td class="label" style="width:100px;">Currency <span class="error">*</span></td>
					<td ti='1'><asp:DropDownList ID="ddlcurrencymasterid" runat="server" CssClass="ddl required"></asp:DropDownList></td>
				</tr>
				<tr>
					<td class="label">Cover Type <span class="error">*</span></td>
					<td ti='2'><asp:DropDownList ID="ddlcovertypeid"  runat="server" CssClass="ddl required"></asp:DropDownList></td>
				</tr>
				<tr>
					<td class="label">Target</td>
					<td ti='3'>
					    <ons-input type="number" class="txttarget">
                          <input type="number" class="text-input" name="txttarget">
                          <span class="text-input__label"></span>
                        </ons-input>
					</td>
				</tr>
				<tr>
					<td class="label">Stop Loss</td>
					<td ti='3'>
					    <ons-input type="number" class="txtstoploss">
                          <input type="number" class="text-input" name="txtstoploss">
                          <span class="text-input__label"></span>
                        </ons-input>
					</td>
				</tr>
				<tr>
					<td class="label">Expiry Date <span class="error">*</span></td>
					<td ti='5'><asp:TextBox ID="txtexpirydate"  runat="server" CssClass="textbox datepicker required"></asp:TextBox>
					<ons-input type="date" class="ons-datepicker" style="position:absolute;left1:-1000px;width:0px;"></ons-input>
					</td>
				</tr>
				<tr> 
					<td class="label">Email Id</td>
				</tr>
				<tr>
				    <td colspan="2">
				        <div id="ltemailids"></div>
					    <asp:TextBox ID="txtemailid"  runat="server" CssClass="txtemailid hdn"></asp:TextBox>
				    </td>
				</tr>
				<tr>
					<td class="label">Mobile No</td>
				</tr>
				<tr>
				    <td colspan="2">
				        <div id="ltmobilenos"></div>
					    <asp:TextBox ID="txtmobileno" runat="server"  CssClass="txtmobileno hdn"></asp:TextBox>
				    </td>
				</tr>
				 <tr>
				    <td></td>
                    <td>
                          <input type="button" value="Save" class="jq-savealert button btnsubmit" m="setalert" action="save"/>
                    </td>
                </tr>
				</table>
	    </div>
        
      </ons-page>
    </template>
    <template id="viewalert.html">
      <ons-page id="viewalert-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>View Alerts</div>
        </ons-toolbar>
	    <div class="addpage-content jq-viewalert">
	         
	    </div>
        
      </ons-page>
    </template>
    <div style="display:none;"><asp:TextBox ID="txtserversessionid" runat="server" CssClass="txtserversessionid"></asp:TextBox></div>
    <div style="display:none;"><asp:TextBox ID="txtisfinstationenabled" runat="server" CssClass="txtisfinstationenabled"></asp:TextBox></div>
    <div style="display:none;"><asp:TextBox ID="txtismetalcommodityenabled" runat="server" CssClass="txtismetalcommodityenabled"></asp:TextBox></div>
    <div class="fade"></div>
    <div class="alert-modal" style="display:none;">
        <div class="alert-modal-title">Alert</div>
        <div class="alert-modal-msg"></div>
        <div class="alert-modal-ok"><input type="button" id="btnalertmodalok" value="Ok"/></div>
    </div>
    <template id="pushnotification.html">
      <ons-page id="pushnotification-page">
         <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>Push Notifications</div>
        </ons-toolbar>
	    <div class="addpage-content jq-pushnotification-list">
            
	    </div>
      </ons-page>
    </template>
    <!--COMMODITY START-->
    
    <template id="metal-dashboard.html">
      <ons-page id="metal-dashboard-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>MetStation</div>
        </ons-toolbar>
	    <div class="addpage-content">
	        <div style="background-color:#222;width:100%;height:32px;">
	            <div style="float:left;font-size:20px;padding:10px;">LME - 3M Forward</div>
	        </div>
	        <div class="jq-liverate" style="overflow:scroll;clear:both;">
		    <asp:Literal ID="ltlmeforward" runat="server"></asp:Literal>
	    </div>
      </ons-page>
    </template>
    <template id="metal-settlementrate.html">
      <ons-page id="metal-settlementrate-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>MetStation</div>
        </ons-toolbar>
	    <div class="addpage-content">
	        <div style="background-color:#222;width:100%;height:32px;">
	            <div style="float:left;font-size:20px;padding:10px;">LME - SETTLEMENT RATE US$/TONNE</div>
	        </div>
	        <div class="jq-liverate" style="overflow:scroll;clear:both;">
		    <asp:Literal ID="ltmetallmesettlementrate" runat="server"></asp:Literal>
	    </div>
      </ons-page>
    </template>
    <template id="metal-stock.html">
      <ons-page id="metal-stock-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>MetStation</div>
        </ons-toolbar>
	    <div class="addpage-content">
	        <div style="background-color:#222;width:100%;height:32px;">
	            <div style="float:left;font-size:20px;padding:10px;">LME Warehouse Stock</div>
	        </div>
	        <div class="jq-liverate" style="overflow:scroll;clear:both;">
		    <asp:Literal ID="ltmetalstock" runat="server"></asp:Literal>
	    </div>
      </ons-page>
    </template>
    <template id="metal-3m-cashspread.html">
      <ons-page id="metal-3m-cashspread-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>MetStation</div>
        </ons-toolbar>
	    <div class="addpage-content">
	        <div style="background-color:#222;width:100%;height:32px;">
	            <div style="float:left;font-size:20px;padding:10px;">LME Cash-3M spread rates</div>
	        </div>
	        <div class="jq-liverate" style="overflow:scroll;clear:both;">
		    <asp:Literal ID="lt3mcashspread" runat="server"></asp:Literal>
	    </div>
      </ons-page>
    </template>
    <template id="metal-currencies.html">
      <ons-page id="metal-currencies-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>MetStation</div>
        </ons-toolbar>
	    <div class="addpage-content">
	        <div style="background-color:#222;width:100%;height:32px;">
	            <div style="float:left;font-size:20px;padding:10px;">Currencies</div>
	        </div>
	        <div class="jq-liverate" style="overflow:scroll;clear:both;">
		    <asp:Literal ID="ltmetalCurrencies" runat="server"></asp:Literal>
	    </div>
      </ons-page>
    </template>
    <template id="metal-indices-commodities.html">
      <ons-page id="metal-indices-commodities-page">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>MetStation</div>
        </ons-toolbar>
	    <div class="addpage-content">
	        <div style="background-color:#222;width:100%;height:32px;">
	            <div style="float:left;font-size:20px;padding:10px;">INDICES AND COMMODITIES</div>
	        </div>
	        <div class="jq-liverate" style="overflow:scroll;clear:both;">
		        <asp:Literal ID="ltmetalindicesandcommodities" runat="server"></asp:Literal>
		    </div>
	    </div>
      </ons-page>
    </template>
    <template id="config-currency.html">
      <ons-page id="config-currency">
        <ons-toolbar>
          <div class="left">
            <ons-toolbar-button onclick="document.querySelector('ons-splitter-side').open();">
              <ons-icon icon="md-menu"></ons-icon>
            </ons-toolbar-button>
          </div>
          <div class="center" ripple>Configure Currencies</div>
        </ons-toolbar>
        <div class="addpage-content">
	        <div class="jq-config-currencies" style="overflow-y:scroll;clear:both;"></div>
	        <div>
	            <table width="100%">
	                <tr><td align="center">
	                <input type="button" class="btn jq-saveconfig-currency" value="Save" />
	                <%--<ons-button modifier="large" class="login-button btnsubmit button button--large jq-saveconfig-currency">Save</ons-button>--%>
	                </td></tr>
	                <tr><td style="height:50px;">&nbsp;</td></tr>
	            </table>
	        </div>
	    </div>
      </ons-page>
    </template>
    
    </form></body></html>
