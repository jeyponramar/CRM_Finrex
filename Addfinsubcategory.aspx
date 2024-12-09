<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Addfinsubcategory.aspx.cs" Inherits="Addfinsubcategory" Title="Addfinsubcategory" %>
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
           <td class='page-inner2'>
           <table width='100%'>
                  
                  <tr>
                      <td class="page-title2">Add SubCategory </td>
                           
                  </tr>
                  <tr>
                        <td align="right" style="padding-right:10px;">
                            <uc:findocmenu id="findocmenu1" runat="server" IsAdminPage="false"></uc:findocmenu>
                        </td>
                    </tr>
                  <tr>
                       <td>
                            <asp:Label ID="lblmessage" runat="server" CssClass="error"></asp:Label>
                       </td>
                  </tr>
                  <tr>
                       <td>
                            <table cellpadding="10">
                                  <tr>
                                     <td>Category <span class="error">*</span></td>
                                     <td>
                                        <asp:TextBox ID="findoccategory" MaxLength="100" runat="server" m="findoccategory" cn="categoryname" 
                                          CssClass="textbox ac txtac"></asp:TextBox>  
                                         <asp:TextBox id="txtfindoccategoryid" Text="0" runat="server" class="hdnac" />
                                         <img src="images/down-arrow.png" class="quick-new epage" />
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="findoccategory" 
                                         ErrorMessage="Required Category"
                                         Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                     </td>
                                 </tr>
                                   <tr>
                                       <td>Sub Category Name <span class="error">*</span></td>
                                       <td><asp:TextBox ID="txtsubcategoryname"  runat="server" class="textbox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvl" runat="server" ControlToValidate="txtsubcategoryname" ErrorMessage="Required Sub Category"
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

