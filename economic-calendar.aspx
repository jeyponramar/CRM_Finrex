<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="economic-calendar.aspx.cs" 
Inherits="economic_calendar" Title="Economic Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'>Economic Calendar</td></tr>
        <tr>
            <td>
                <div class="tradingview-widget-container">

  <div class="tradingview-widget-container__widget"></div>

  <div class="tradingview-widget-copyright"><a href="https://in.tradingview.com/markets/currencies/economic-calendar/" rel="noopener" target="_blank"><span class="blue-text"></span></a> </div>

  <script type="text/javascript" src="https://s3.tradingview.com/external-embedding/embed-widget-events.js" async>

  {

  "width": "100%",

  "height": "500",

  "colorTheme": "dark",

  "isTransparent": false,

  "locale": "in",

  "importanceFilter": "0,1",

  "currencyFilter": "INR,DEM,FRF,CNY,JPY,USD,CHF,SGD,GBP,EUR,AUD,CAD"

}

  </script>

</div>
            </td>
        </tr>
    </table>
  </td>
 </tr>
</table> 
</asp:Content>

