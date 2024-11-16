<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="mysubscription.aspx.cs" 
Inherits="mysubscription" Title="My Subscription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">My Subscription</td>
            </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="6">
                        <tr><td class="bold"><u>Service Description : </u></td></tr>
                        <tr>
                            <td><asp:Label ID="lblserviceplan" runat="server" CssClass="bold"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" cellspacing="5">
                                    <tr>
                                        <td width="100" style="vertical-align:top;">Service : </td>
                                        <td class="list"><asp:Literal ID="ltservices" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align:top;">Software : </td>
                                        <td class="list"><asp:Literal ID="ltsoftwares" runat="server"></asp:Literal></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">FX Software User Id</td>
                        </tr>
                        <tr>
                            <td><asp:Literal ID="ltusers" runat="server"></asp:Literal></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>
</asp:Content>

