 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="EmailIntegration_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
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
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Source Email <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtsourceemail"  IsUnique="true" MaxLength="100" runat="server"  dcn="emailintegration_sourceemail" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Source Email" ValidationGroup="vg" ControlToValidate="txtsourceemail"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Destination Module <span class="error">*</span></td>
											<td ti='1'><asp:DropDownList ID="ddlemailintegrationmoduleid"  dcn="emailintegration_emailintegrationmoduleid" runat="server" m="emailintegrationmodule" cn="modulename" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Destination Module" ValidationGroup="vg" ControlToValidate="ddlemailintegrationmoduleid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='2'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="emailintegration_remarks" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="tremailkeywordmapping" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="2"><tr><td class='subtitle'>Email Keyword Mapping</td></tr><tr><td style='padding:10px;'><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~keyword,priority,tagid~dc~keyword,priority,tag_tagname~smprefix~@sm1_~jt~tag~m~emailkeywordmapping' id='emailkeywordmapping_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='emailkeywordmapping_param' runat='server'/>
							<tr class='srepeater-header'>
								<td cn="keyword">Keyword</td><td cn="priority">Priority</td><td cn="tagname">Tag</td><td></td></tr>
							<asp:Literal ID="ltemailkeywordmapping" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="keyword" class="textbox first" ir='1'  maxlength='200'  ti='0'/></td>
								<td><input type="text" id="priority" class="sbox val-i right" maxlength='10'  ti='1'/></td>
								<td><input type="text" id="tagid" class='hdnqa hdnac' maxlength='100'/><input type="text" id="tagname" class="textbox txtqa ac "  m="tag" cn="tagname" include="0"   ti='2'/><img src="../images/down-arr1.jpg" class="quick-menu"/></td></tr>
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
