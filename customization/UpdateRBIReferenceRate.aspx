<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="UpdateRBIReferenceRate.aspx.cs" 
Inherits="customization_UpdateRBIReferenceRate" Title="Update RBI Reference Rate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server" Text="Update RBI Reference Rate"/>
            </td>
         </tr>
         <tr>
            <td align="center" colspan="2"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td>
        </tr>
         <tr>
            <td class="form">
                <table cellpadding="10" cellspacing="0" border="1" style="border-collapse:collapse;border-color:#ddd;">
                <tr class="bold"><td>Currency</td><td>Rate</td><td>Date</td></tr>
                <tr>
                    <td class="label">USDINR</td>
                    <td><asp:TextBox ID="txtrate1" runat="server" CssClass="val-dbl sbox"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtdate1" runat="server" CssClass="datepicker"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="label">EURINR</td>
                    <td><asp:TextBox ID="txtrate2" runat="server" CssClass="val-dbl sbox"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtdate2" runat="server" CssClass="datepicker"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="label">GBPINR</td>
                    <td><asp:TextBox ID="txtrate3" runat="server" CssClass="val-dbl sbox"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtdate3" runat="server" CssClass="datepicker"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="label">JPYINR</td>
                    <td><asp:TextBox ID="txtrate4" runat="server" CssClass="val-dbl sbox"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtdate4" runat="server" CssClass="datepicker"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2"><asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CssClass="button" /></td>
                </tr>
               </table>
             </td>
          </tr>
       </table>
    </asp:PlaceHolder>
</asp:Content>

