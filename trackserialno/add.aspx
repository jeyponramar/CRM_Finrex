<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="add.aspx.cs" Inherits="trackserialno_add" Title="Untitled Page" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server" Text="Track Serial No"/>
            </td>
            <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
         </tr>
         <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td class="label">Serial No <span class="error">*</span></td>
                        <td><asp:TextBox ID="SerialNo" runat="server" CssClass="textbox ac" m="serialno" cn="serialno_serialno" autocomplete="off"></asp:TextBox>
                            <asp:TextBox id="txtserialno"  dcn="serialno_serialnoid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
                            <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtSerialNo" ErrorMessage="Enter Serial No" 
                            ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr><td colspan="2"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Track" ValidationGroup="vg" CssClass="button"/></td>
                    </tr>

                    <tr><td class="subtitle" colspan="2">AMC / Warranty Detail</td></tr>
                    <tr><td colspan="2"><asp:Literal ID="ltWarrantyDetail" runat="server"></asp:Literal></td></tr>
                    
                    <tr><td class="subtitle" colspan="2">Product Tracking</td></tr>
                    <tr><td colspan="2"><asp:Literal ID="ltSerialNo" runat="server"></asp:Literal></td></tr>
                </table>
            </td>
         </tr>
     </table>
</asp:PlaceHolder>         
</asp:Content>

