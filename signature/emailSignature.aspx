<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InnerMaster.master" CodeFile="emailSignature.aspx.cs" Inherits="emailSignature" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:PlaceHolder ID="form1" runat="server">
   
    <table width="100%">
        <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
            <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
         </tr>      
         <tr><td colspan="2" align="center">
            <asp:Label ID="message" CssClass="error" runat="server" Text="" Visible="false"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0"> 
                               
                        <tr id="mailtypeText" runat="server">        
                             <td style="vertical-align:top;" class="label">
                              Text Signature
                            </td>
                            
                            <td>
                                <asp:TextBox ID="tmessage" runat="server" CssClass="textarea" Rows="10" Height="100" Width="820" TextMode="MultiLine">                               
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID ="rfv1" runat ="server" ValidationGroup="vg" ErrorMessage="Please Enter Your Signature" ControlToValidate ="tmessage"></asp:RequiredFieldValidator>                                                  
                            </td>
                        </tr>
                        
                          <tr id="mailtypehtml" runat="server">
                            <td style="vertical-align:top;"  class="label">Html Signature</td><td>
                            <asp:TextBox CssClass="htmleditor" ID="Email_Body" runat="server" TextMode="MultiLine" Height="300" Width="750">
                            
                            </asp:TextBox>            
                            </td>            
                            <td></td>
                        </tr>                       
                         
                </table>
             </td>
         </tr>
          <tr>                              
            <td align="center">
                <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="button" OnClick="btn_SubmitClick" ValidationGroup="vg"/>                
                <asp:Button ID="btn_cancel" class="close-page cancel"  runat="server" Text="Cancel"/>                
            </td>                           
         </tr>              
    </table>
    <table>
     
    </table>      
   </asp:PlaceHolder>
</asp:Content>
 