<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="economic-calender.aspx.cs" 
Inherits="economic_calender" Title="Economic Calender" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="page-title">
            Economic Calender
        </td>
     </tr>
     <%--<tr>
        <td>
            <iframe src="http://ec.in.forexprostools.com?columns=exc_flags,exc_currency,exc_importance,exc_actual,exc_forecast,exc_previous&category=_employment,_economicActivity,_inflation,_centralBanks,_confidenceIndex,_balance,_Bonds&importance=1,2,3&features=datepicker,timezone,timeselector,filters&countries=25,37,72,22,17,39,14,35,36,110,12,4,5&calType=day&timeZone=23&lang=56" width="1000" height="500" frameborder="0" allowtransparency="true" marginwidth="0" marginheight="0"></iframe>
            <div class="poweredBy" style="font-family: Arial, Helvetica, sans-serif;"><span style="font-size: 11px;color: #333333;text-decoration: none;">Real Time Economic Calendar provided by <a href="http://in.Investing.com/" rel="nofollow" target="_blank" style="font-size: 11px;color: #06529D; font-weight: bold;" class="underline_link">Investing.com India</a>.</span></div>
        </td>
     </tr>--%>
</table>     
</asp:Content>

