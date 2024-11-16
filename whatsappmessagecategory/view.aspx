<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="view.aspx.cs" Inherits="WhatsAppMessageCategory_view" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%" cellpadding="0" cellspacing="5">
    <tr>
        <td width="80%" style="vertical-align:top" class="tddatapanel">
            <table width="100%">
                <tr>
                    <td class="title">
                        <asp:Label ID="lblPageTitle" runat="server"/>
                    </td>
                 </tr>
                 <tr>
                    <td colspan="2">
                        <uc:Grid id="grid" runat="server" Module="WhatsAppMessageCategory"/>
                    </td>
                 </tr>
            </table>
        </td>
        <td width="20%" class="tdrightpanel hidden valign">
            <table width="100%">
                <tr><td align="right"><img class="quickedit-close hand" title="Close this panel" src="../images/close.png" /></td></tr>
                <tr><td class="tdrightpanel-inner">
                    <iframe src="" id="ifrRightPanel" class="summary-right-panel-iframe"></iframe>
                </td></tr>
            </table>
        </td>
    </tr>
     
</table>
<!--DESIGN_END-->
</asp:Content>