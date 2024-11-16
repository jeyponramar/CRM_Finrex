<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewfpiinvestment.aspx.cs" 
Inherits="viewfpiinvestment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'><asp:Label ID="lbltitle" runat="server" Text="FPI Net Investments"></asp:Label></td></tr>
        <tr>
            <td>
                <table cellspacing=10>
                    <tr>
                        <td width="100" class="bold">Date Range <span class="error">*</span>: </td>
                        <td><asp:DropDownList ID="ddlreporttype" runat="server" style="width:120px;height:25px;" OnSelectedIndexChanged="ddlreporttype_changed" AutoPostBack="true">
                            <asp:ListItem Text="USD Million Datewise" Value="0"></asp:ListItem>
                            <asp:ListItem Text="USD Million Monthly" Value="1"></asp:ListItem>
                        </asp:DropDownList></td>
                    </tr>
                    <%--<tr>
                        <td width="100" class="bold">Currency <span class="error">*</span>: </td>
                        <td><asp:DropDownList ID="ddlcurrency" runat="server" style="width:120px;height:25px;">
                        </asp:DropDownList></td>
                    </tr>--%>
                    <tr id="tryear" runat="server" visible="false">
                        <td class="bold">Year : </td>
                        <td>
                            <asp:DropDownList ID="ddlyear" runat="server" style="width:120px;height:25px;"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trdate" runat="server">
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
        
        <tr><td align="right" style="padding-right:20px;font-weight:bold;">US($) million</td></tr>
        <tr><td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td></tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

