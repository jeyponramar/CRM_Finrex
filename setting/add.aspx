 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Setting_add" EnableEventValidation="false" ValidateRequest="false"%>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 

<script> 
    $(document).ready(function() {
        //SetDetailPage('<%=Request.QueryString["id"]%>');
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
    <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
            <%--<td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>--%>
         </tr>
        
         <tr>
            <td>
                <%--<input type="button" value="Edit" class="edit button dpage"/>--%>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="10">
                <tr>
                    <td align="center" colspan="4"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
					<tr>
						<td class="label">Setting Name <span class="error">*</span></td>
						<td><asp:TextBox ID="txtsettingname" runat="server"  dcn="setting_settingname" style="width:350px;" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" Display="Dynamic" runat="server" ErrorMessage="Required Setting Name" ValidationGroup="vg" ControlToValidate="txtsettingname"></asp:RequiredFieldValidator></td>
					</tr>
					<tr id="trtype" runat="server"><td>Type</td>
					    <td><asp:DropDownList ID="ddlishtml" runat="server" AutoPostBack="true" dcn="setting_ishtml" style="width:350px;" OnSelectedIndexChanged="ddlishtml_Changed">
					            <asp:ListItem Selected=True Text="Plain Text" Value="False"></asp:ListItem>
					            <asp:ListItem Text="HTML" Value="True"></asp:ListItem>
					        </asp:DropDownList></td>
					</tr>
					<tr>
						<td class="label" style="vertical-align:top;">Setting Value</td>
						<td><asp:TextBox TextMode="MultiLine" ID="settingvalue_plain"  dcn="setting_settingvalue" runat="server" CssClass="textarea" Width="700" Height="400"></asp:TextBox>
						    <asp:TextBox CssClass="htmleditor" ID="settingvalue_html" runat="server" TextMode="MultiLine" Height="400" Width="750"></asp:TextBox>
						</td>
					</tr>
                    
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center">
            <!--SAVEBUTTON_START-->
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" ValidationGroup="vg"/>
                <input type="button" class="close-page cancel" value="Cancel"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>
        <!--FOLLOWUP_START-->
        <!--FOLLOWUP_END-->
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
