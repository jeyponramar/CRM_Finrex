<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="testbondyield.aspx.cs" Inherits="testbondyield" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!-- TradingView Widget BEGIN -->

<div class="tradingview-widget-container">

  <div class="tradingview-widget-container__widget"></div>

  <div class="tradingview-widget-copyright"><a href="https://www.tradingview.com/" rel="noopener nofollow" target="_blank"><span class="blue-text">Track all markets on TradingView</span></a></div>

  <script type="text/javascript" src="https://s3.tradingview.com/external-embedding/embed-widget-market-overview.js" async>

  {

  "colorTheme": "dark",

  "dateRange": "12M",

  "showChart": true,

  "locale": "en",

  "largeChartUrl": "",

  "isTransparent": false,

  "showSymbolLogo": false,

  "showFloatingTooltip": true,

  "width": "400",

  "height": "550",

  "plotLineColorGrowing": "rgba(41, 98, 255, 1)",

"plotLineColorFalling": "rgba(41, 98, 255, 1)",

  "gridLineColor": "rgba(240, 243, 250, 0)",

  "scaleFontColor": "rgba(209, 212, 220, 1)",

  "belowLineFillColorGrowing": "rgba(41, 98, 255, 0.12)",

  "belowLineFillColorFalling": "rgba(41, 98, 255, 0.12)",

  "belowLineFillColorGrowingBottom": "rgba(41, 98, 255, 0)",

  "belowLineFillColorFallingBottom": "rgba(41, 98, 255, 0)",

  "symbolActiveColor": "rgba(41, 98, 255, 0.12)",

  "tabs": [

    {

      "title": "Bonds",

      "symbols": [

        {

          "s": "TVC:US10Y"

        },

        {

          "s": "TVC:GB10Y"

        },

        {

          "s": "TVC:DE10Y"

        },

        {

          "s": "TVC:JP10Y"

        },

        {

          "s": "TVC:AU10Y"

        },

        {

          "s": "TVC:CA10Y"

        },

        {

          "s": "TVC:IT10"

        },

        {

          "s": "TVC:EU10"

        },

        {

          "s": "TVC:IN10Y"

        },

        {

          "s": "TVC:CN10Y"

       },

        {

          "s": "TVC:RU10Y"

        },

        {

          "s": "TVC:FR10Y"

        },

        {

          "s": "TVC:ZA10Y"

        },

        {

          "s": "TVC:BR10Y"

        },

        {

          "s": "TVC:SG10Y"

        },

        {

          "s": "TVC:HK10Y"

        },

        {

          "s": "TVC:ID10Y"

        },

        {

          "s": "TVC:KR10Y"

        },

        {

          "s": "TVC:TH10Y"

        },

        {

          "s": "TVC:TW10Y"

        }

      ],

      "originalTitle": "Bonds"

    }

  ]

}

  </script>

</div>

<!-- TradingView Widget END -->
</asp:Content>

