<%@ Page Title="" Language="C#" MasterPageFile="~/CommodityMasterPage.master" AutoEventWireup="true" CodeFile="commodity-historicaldata.aspx.cs" Inherits="commodity_historicaldata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='pagetitle'>Historical Data</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>
                <table cellspacing=10>
                    <tr>
                        <td width="100" class="bold">Metal <span class="error">*</span> : </td>
                        <td><asp:DropDownList ID="ddlmetal" runat="server" style="width:120px;height:25px;">
                        </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="bold">Date : </td>
                        <td>
                            From <asp:TextBox ID="txtfromdate" runat="server" CssClass="datepicker"></asp:TextBox>
                            &nbsp;To <asp:TextBox ID="txttodate" runat="server" CssClass="datepicker"></asp:TextBox>
                        </td>
                    </tr>
                    <tr><td></td><td><asp:Button ID="btnSubmit" CssClass="button-ui" runat="server" Text="Search" OnClick="btnSubmit_Click" /></td></tr>
                </table>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td></tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

