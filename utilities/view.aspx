<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="view.aspx.cs" Inherits="utilities_view" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%" cellpadding="0" cellspacing="5">
    <tr>
        <td width="100%" style="vertical-align:top" class="tddatapanel">
            <table width="100%">
                <tr>
                    <td class="title">
                        <asp:Label ID="lblPageTitle" runat="server"/>
                    </td>
                 </tr>
                 <tr>
                    <td colspan="2">
                        <uc:Grid id="grid" runat="server"/>
                    </td>
                 </tr>
            </table>
        </td>
    </tr>
     
</table>
<!--DESIGN_END-->
</asp:Content>