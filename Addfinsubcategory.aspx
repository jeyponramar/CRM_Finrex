<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Addfinsubcategory.aspx.cs" Inherits="Addfinsubcategory" Title="Addfinsubcategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server">
 <table width='100%' cellpadding='0' cellspacing='0' >
      <tr>
           <td class='page-inner2'>
           <table width='100%'>
                  <tr>
                      <td class="page-title2">Add Findoc SubCategory </td>
                           
                  </tr>
                  <tr>
                       <td>
                            <asp:Label ID="lblmessage" runat="server"></asp:Label>
                       </td>
                  </tr>
                  <tr>
                       <td>
                            <table>
                                   <tr>
                                       <td style="padding:15px;width:150px;"align="right" >Sub Category Name</td>
                                       <td><asp:TextBox ID="txtsubcategoryname"  runat="server" class="textbox"></asp:TextBox></td>
                                   </tr>
                                   <tr>
                                        <td></td>
                                        <td><asp:Button ID="btnSubmit" runat="server"  Text="Save" CssClass="save button" OnClick="btnSubmit_Click" /></td>
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

