<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="tradingview-chart-widget.aspx.cs" Inherits="tradingview_chart_widget" Title="Charts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'>Charts</td></tr>
        <tr>
            <td>
<!-- TradingView Widget BEGIN -->

<div class="tradingview-widget-container">

  <div id="tradingview_854fb" style="height:550px;"></div>

  <div class="tradingview-widget-copyright"><a href="https://in.tradingview.com/symbols/USDINR/?exchange=FX_IDC" rel="noopener" target="_blank"><span class="blue-text">USD INR chart</span></a> by TradingView</div>

  <script type="text/javascript" src="https://s3.tradingview.com/tv.js"></script>

  <script type="text/javascript">

  new TradingView.widget(

  {

  "autosize": true,

  "symbol": "FX_IDC:USDINR",

  "interval": "D",

  "timezone": "Asia/Kolkata",

  "theme": "light",

  "style": "2",

  "locale": "in",

  "toolbar_bg": "#f1f3f6",

  "enable_publishing": false,

  "withdateranges": true,

  "hide_side_toolbar": false,

  "save_image": false,

  "watchlist": [

    "FX_IDC:USDINR",

    "OANDA:EURUSD",

    "FX_IDC:EURINR",

    "FX:GBPUSD",

    "FX_IDC:GBPINR",

    "FX:AUDUSD",

    "SAXO:CADUSD",

    "FX_IDC:USDCNY",

    "FX:USDJPY",

    "FX_IDC:AUDINR",

    "FX_IDC:BRLINR",

    "FX_IDC:CADINR",

    "FX:USDCNH",

    "FX_IDC:CNYINR",

    "FX_IDC:CNHINR",

    "FX_IDC:JPYINR",

    "BSE:SENSEX",

    "CAPITALCOM:DXY",

    "TVC:UKOIL",

    "TVC:USOIL",

    "TVC:GOLD",

    "TVC:SILVER",

    "BLACKBULL:DJ30.F"

  ],

  "container_id": "tradingview_854fb"

}

  );

  </script>

</div>

<!-- TradingView Widget END -->
     </td></tr></table>
     </td></tr></table>
</asp:Content>

