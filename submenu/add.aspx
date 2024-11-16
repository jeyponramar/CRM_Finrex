 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="SubMenu_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".deletesubmenu").click(function() {
            return confirm("Are you sure you want to delete this Sub Menu?");
        });
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
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
            </td>
            <td align="right"> 
                <uc:NextPrevDetail ID="NextPrevDetail" runat="server" />
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
				<!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Sub Menu Name <span class="error">*</span></td>
											<td><asp:TextBox ID="txtsubmenuname"  IsUnique="true" MaxLength="100" runat="server"  dcn="submenu_submenuname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Sub Menu Name" ValidationGroup="vg" ControlToValidate="txtsubmenuname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Menu <span class="error">*</span></td>
											<td><asp:TextBox ID="menu"  dcn="menu_menuname" MaxLength="100" runat="server" m="menu" cn="menuname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtmenuid"  dcn="submenu_menuid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Menu" ValidationGroup="vg" ControlToValidate="menu"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">URL</td>
											<td><asp:TextBox ID="txturl" MaxLength="200" runat="server"  dcn="submenu_url" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Sequence</td>
											<td><asp:TextBox ID="txtsequence"  dcn="submenu_sequence" runat="server" MaxLength="10" CssClass="textbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is New Window</td>
											<td><asp:CheckBox ID="chkisnewwindow"  dcn="submenu_isnewwindow" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Menu Type</td>
											<td><asp:TextBox ID="txtmenutype" MaxLength="100" runat="server"  dcn="submenu_menutype" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is Visible</td>
											<td><asp:CheckBox ID="chkisvisible"  Checked="true"  dcn="submenu_isvisible" runat="server"></asp:CheckBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					</table>
					<!--CONTROLS_END-->
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="save button" ValidationGroup="vg"/>
                <asp:Button ID="btnSubmitAndView" runat="server" OnClick="btnSaveAndView_Click" Text="Save & View" CssClass="save button" ValidationGroup="vg"/>
                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="save redbutton deletesubmenu" ValidationGroup="vg"/>
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
