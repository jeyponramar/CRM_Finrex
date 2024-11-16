<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="historical-data.aspx.cs" Inherits="historical_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style>
    .ui-datepicker-calendar1 {
        display: none;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'><asp:Label ID="lbltitle" runat="server" Text="Historical Data - Spot Rate"></asp:Label></td></tr>
        <tr>
            <td>
                <table cellspacing=10>
                    <tr id="tdcurrency" runat="server" visible="false">
                        <td width="100" class="bold">Currency <span class="error">*</span> : </td>
                        <td><asp:DropDownList ID="ddlcurrency" runat="server" style="width:120px;height:25px;">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="USDINR" Value="2"></asp:ListItem>
                            <asp:ListItem Text="EURINR" Value="3"></asp:ListItem>
                            <asp:ListItem Text="GBPINR" Value="4"></asp:ListItem>
                            <asp:ListItem Text="JPYINR" Value="5"></asp:ListItem>
                        </asp:DropDownList></td>
                    </tr>
                    <tr id="trdate" runat="server">
                        <td class="bold">Date : </td>
                        <td>
                            From <asp:TextBox ID="txtfromdate" runat="server" CssClass="datepicker"></asp:TextBox>
                            &nbsp;To <asp:TextBox ID="txttodate" runat="server" CssClass="datepicker"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trmonth" runat="server" visible="false">
                        <td class="bold">Month :</td>
                        <td>
                            From <asp:TextBox ID="txtfrommonth" runat="server" CssClass="monthyearpicker"></asp:TextBox>
                            &nbsp; To <asp:TextBox ID="txttomonth" runat="server" CssClass="monthyearpicker"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display:none;"><td class="bold">Spot Rate : </td><td><asp:Label  class="bold" ID="lblspotrate" runat="server"></asp:Label></td></tr>
                    <tr><td></td><td><asp:Button ID="btnSubmit" CssClass="button-ui" runat="server" Text="Search" OnClick="btnSubmit_Click" /></td></tr>
                </table>
            </td>
        </tr>
        
        <tr><td>&nbsp;</td></tr>
        <%--<tr><td class="bold">Spot Rate :--%>
        <tr><td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td></tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

