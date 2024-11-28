<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Addfindocumenttype.aspx.cs" Inherits="Addfindocumenttype" Title="Addfindocumenttype" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server" >
  <table width='100%'>
         <tr>
             <td class='page-inner2'>
                <table width='100%'>
                       <tr>
                          <td class="page-title2">Add Findoc Document Type</td>
                       </tr>
                       <tr>
                           <td><asp:Label ID="lblmessage" runat="server" CssClass="error"></asp:Label></td>
                       </tr>
                       <tr>
                           <td>
                               <table>
                                      <tr>
                                           <td style="padding:15px;width:100px;"align="right">Document Type<span class="error">*</span></td>
                                           <td><asp:TextBox ID="txtdocumenttype" runat="server" class="textbox"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="rfvl" runat="server" ControlToValidate="txtdocumenttype" ErrorMessage="Required Document"
                                              Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                           </td>
                                      </tr>
                                      <tr>
                                           <td></td>
                                           <td><asp:Button ID="btnSubmit" runat="server" Text="save" CssClass="save button" OnClick="btnSubmit_Click" ValidationGroup="vg"/></td>
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

