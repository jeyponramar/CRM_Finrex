 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Note_add" EnableEventValidation="false" %>
<%--CONTROLREGISTER_START--%>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
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
            <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center" colspan="4"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                <!--CONTROLS_START-->
					
					<tr>
						<td class="label">Note <span class="error">*</span></td>
						<td><asp:TextBox TextMode="MultiLine" ID="txtnote"  dcn="note_note" runat="server" CssClass="textarea"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" Display="Dynamic" runat="server" ErrorMessage="Required Note" ValidationGroup="vg" ControlToValidate="txtnote"></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
						<td class="label">Priority <span class="error">*</span></td>
						<td><asp:DropDownList ID="ddlpriorityid"  dcn="note_priorityid" runat="server" m="priority" cn="priority" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv2" Display="Dynamic" runat="server" ErrorMessage="Required Priority" ValidationGroup="vg" ControlToValidate="ddlpriorityid"  InitialValue="0"></asp:RequiredFieldValidator></td>
					</tr>
					<!--CONTROLS_END-->
                <!--SUBGRID_START-->
					
					<!--SUBGRID_END-->
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" ValidationGroup="vg"/>
                <input type="button" class="close-page cancel" value="Cancel"/>
            </td>
        </tr>
        <!--FOLLOWUP_START-->
        <!--FOLLOWUP_END-->
        <!--COMMENTS_START-->
        <!--COMMENTS_END-->
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
