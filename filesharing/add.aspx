 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="FileSharing_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
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
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
                <!--ACTION_START--><!--ACTION_END-->
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
											<td class="label">Title <span class="error">*</span></td>
											<td><asp:TextBox ID="txttitle"  IsUnique="true" MaxLength="100" runat="server"  dcn="filesharing_title" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Title" ValidationGroup="vg" ControlToValidate="txttitle"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Keyword</td>
											<td><asp:TextBox ID="txtkeyword" MaxLength="100" runat="server"  dcn="filesharing_keyword" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Description</td>
											<td><asp:TextBox TextMode="MultiLine" ID="txtdescription"  Width="600" Height="300"  dcn="filesharing_description" runat="server" CssClass="htmleditor htmleditor "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">File Sharing Document</td>
											<td><uc:MultiFileUpload ID="mfufilesharingdocument"  IsMutiple="true" FileType="Any" FolderPath="upload/filesharing" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
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
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trfilesharingrole" ><td class='subgridbox'><table width='100%' cellspacing='0'><tr><td class='subtitle'>File Sharing Role</td></tr><tr><td style='padding:10px;'><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~roleid~dc~role_rolename~smprefix~@sm1_~jt~role~m~filesharingrole' id='filesharingrole_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='filesharingrole_param' runat='server'/>
							<tr class='srepeater-header'><td cn="rolename">Role</td><td></td></tr>
							<asp:Literal ID="ltfilesharingrole" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="roleid" class=' hdnac' maxlength='100'/><input type="text" id="rolename" class="textbox first ac "  m="role" cn="rolename" include="0"  ir='1'  /><img src="../images/down-arrow.png" class="epage"/></td></tr>
							<tr class='end'><td></td></tr>
						</table></td></tr>
					</table></td></tr><tr><td>&nbsp;</td></tr>
					</table>
					<!--CONTROLS_END-->
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center" colspan="2">
				<!--SAVEBUTTON_START-->
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="save button" ValidationGroup="vg"/>
                <asp:Button ID="btnSubmitAndView" runat="server" OnClick="btnSaveAndView_Click" Text="Save & View" CssClass="save button" ValidationGroup="vg"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					<script>
$(document).ready(function() {
	//add or update
	setTotal($(".grid"));
	$(".newrow").find("input").live("keypress", function(event) {
		if (event.which == 13) {
			CalculateTaxAmount($(this));
			addGridRow($(this));
			setTotal($(this).closest(".grid"));
			return false;
		}
	});
	$(".newrow").find("input").live("change", function(event) {
		findAmount($(this));
	});
	//delete
	$(".delete-row").live("click", function(event) {
		var grid = $(this).closest(".grid");
		removeGridRow($(this));
		setTotal(grid);
	});
	//edit
	$(".gridtr").live("click", function(event) {
		editGridRow($(this));
	});
	$("#productname").live("blur", function(event){
	    CalculateTaxAmount($(this));
	});
	
});
</script>
					<!--JSCODE_END-->

</asp:Content>
