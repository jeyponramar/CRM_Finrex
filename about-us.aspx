<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="about-us.aspx.cs" Inherits="about_us" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".i-mainmenu").removeClass("i-mainmenu-active");
        $(".menu-aboutus").addClass("i-mainmenu-active");
    });
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td style="font-size:14px;text-align:justify;padding-top:25px;">
            <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Finrex Treasury Advisors </b>is a treasury and foreign exchange consulting company. We recognise your business risks while dealing with foreign currencies and provide you with optimum results. We constantly strive to provide you with a phenomenal strategy, understanding your company’s risk appetite, and provide you the perfect solution for the identified need-gaps in your company's financial and foreign exchange model.
        </td>
    </tr>
    <tr>
        <td style="font-size:14px;text-align:justify;padding-top:15px;padding-bottom:15px;">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;At Finrex, we follow a step-by-step methodology to ensure hedging at minimum risks for your company. Our USP lies in process based consulting which starts with Treasury Audit that intends to understand your existing banking structure and various associated charges that your business pays for. We add tremendous value by providing a comprehensive risk management policy which ultimately aims to meet business objectives efficiently.
        </td>
    </tr>
    <tr>
        <td style="font-size:14px;text-align:justify;">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Finrex has adopted eminent international concepts in the Indian context. Our experts are well versed with the fluctuating foreign exchange and interest rate markets and can assist you in your day to day operations. With Finrex, you can look forward to receiving unbiased and independent view that benefits your organisation.
        </td>
    </tr>
</table>
</asp:Content>

