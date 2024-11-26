<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Addfincategory.aspx.cs" Inherits="Addfincategory" Title="Addfincategory" %>
<%@ Register Src="~/usercontrols/FinDocMenu.ascx" TagName="findocmenu" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
body{background-color:#fff !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server">
 <table width='100%' cellpadding='0' cellspacing='0' >
      <tr>
             <td class="finmenubg">
               <uc:findocmenu id="findocmenu" runat="server" IsAdminPage="false"></uc:findocmenu>
            </td>
           <td class='page-inner2'>
           <table width='100%'>
                  <tr>
                      <td class="page-title2">Add Findoc Category </td>
                           
                     
                  </tr>
                  <tr>
                       <td>
                            <asp:Label ID="lblmessage" runat="server" CssClass="error"></asp:Label>
                       </td>
                  </tr>
                  <tr>
                       <td>
                            <table>
                                   <tr>
                                       <td style="padding:15px;width:100px;"align="right" >Category Name <span class="error">*</span></td>
                                       <td><asp:TextBox ID="txtcategoryname"  runat="server" class="textbox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvl" runat="server" ControlToValidate="txtcategoryname" ErrorMessage="Required Category"
                                         Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                       </td>
                                   </tr>
                                   <tr>
                                        <td></td>
                                        <td><asp:Button ID="btnSubmit" runat="server"  Text="Save" CssClass="save button" OnClick="btnSubmit_Click" ValidationGroup="vg" /></td>
                                   </tr>
                            </table>
                       </td>
                  </tr>
           </table>
           
           </td>
      </tr>
 </table>
  </asp:PlaceHolder>
</asp:Content>

