<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="apiconfig.aspx.cs" 
Inherits="client_apiconfig" Title="Api Config" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server">
  <table width='100%'>
         <tr>
              <td class='page-inner2'>
                  <table width='100%' cellpadding='0' cellspacing='0'>
                         <tr>
                             <td class='page-title2'>Api Configuration</td>
                         </tr>
                         <tr>
                             <td><asp:Label ID="lblmessage" runat="server" CssClass="error"></asp:Label></td>
                         </tr>
                         <tr>
                             <td>
                                 <table cellpadding='5' cellspacing='5' >
                                    <tr>
                                        <td style="width:600px;">
                                            <table>
                                                <tr>
                                                   <td  style="padding:15px;width:110px;"align="right">API User Name<span class="error">*</span></td>
                                                   <td><asp:TextBox ID="txtapiusername" runat="server" class="textbox"></asp:TextBox>
                                                     <asp:RequiredFieldValidator ID="rfvl" runat="server" ControlToValidate="txtapiusername" ErrorMessage="Required Api User Name"
                                                    Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                                   </td>
                                               </tr>
                                               <tr>
                                                   <td>Max API Calls Per Day</td>
                                                   <td><asp:TextBox ID="txtmaxapicallsperday" runat="server" class="textbox val-i"></asp:TextBox></td>
                                               </tr>
                                               <tr>
                                                    <td>API Password</td>
                                                    <td><asp:Label ID="lblapipassword" runat="server"></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td></td>
                                                   <td><asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="save button"  OnClick="btnSubmit_Click" ValidationGroup="vg"  /></td>
                                               </tr>
                                            </table>            
                                        </td>
                                        <td  style="vertical-align:top;">
                                            <table cellpadding='5' cellspacing='5'>
                                               <%-- <tr>
                                                    <td></td>
                                                    <td><input type="checkbox" id="Checkbox1" name="checkbox1" />
                                                        <label for="checkbox1">Contact Email 1</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                     <td></td>
                                                     <td><input type="checkbox" id="Checkbox2" name="checkbox2" />
                                                        <label for="checkbox2">Contact Email 2</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                     <td></td>
                                                     <td><input type="checkbox" id="Checkbox3" name="checkbox3" />
                                                        <label for="checkbox3">Contact Email 3</label>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                     <td></td>
                                                     <td><asp:Literal ID="ltlcontact" runat="server"></asp:Literal></td>
                                                </tr>
                                                <tr>
                                                     <td></td>
                                                     <td><asp:Button ID="btnSend" runat="server" Text="Send Email" CssClass="save button" /></td>
                                                </tr>
                                                               
                                            </table>
                                            </td>
                                        </td>
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

