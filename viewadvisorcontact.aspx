<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="viewadvisorcontact.aspx.cs" Inherits="viewadvisorcontact" Title="View Advisor Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">View Advisor Contact</td>
            </tr>
            <tr>
                <td style="padding-left:100px;padding-top:20px;">
                    <table width="100%" cellpadding="10">
                        <tr>
                            <td class="label">Advisor Name</td>
                            <td><asp:Label ID="lbladvisorname" runat="server" CssClass="val"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label">Advisor Email Id</td>
                            <td><asp:Literal ID="ltadvisoremailid" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td class="label">Advisor Mobile Number</td>
                            <td><asp:Label ID="lbladvisormobileno" runat="server" CssClass="val"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>

</asp:Content>

