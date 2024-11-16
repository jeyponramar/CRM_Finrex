 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="ScheduleReminderConfig_add" EnableEventValidation="false" ValidateRequest="false"%>
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
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Module <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="module"  search='true'  dcn="module_modulename" MaxLength="100" runat="server" m="module" cn="modulename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtmoduleid"  dcn="schedulereminderconfig_moduleid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Module" ValidationGroup="vg" ControlToValidate="module"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Query</td>
											<td ti='1'><asp:TextBox TextMode="MultiLine" ID="txtquery"  dcn="schedulereminderconfig_query"  runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Subject <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtsubject"  MaxLength="100" runat="server"  dcn="schedulereminderconfig_subject" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Subject" ValidationGroup="vg" ControlToValidate="txtsubject"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">To Email Id <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txttoemailid"  MaxLength="100" runat="server"  dcn="schedulereminderconfig_toemailid" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required To Email Id" ValidationGroup="vg" ControlToValidate="txttoemailid"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">CC Email Id</td>
											<td ti='4'><asp:TextBox ID="txtccemailid"  MaxLength="100" runat="server"  dcn="schedulereminderconfig_ccemailid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">BCC Email Id</td>
											<td ti='5'><asp:TextBox ID="txtbccemailid"  MaxLength="100" runat="server"  dcn="schedulereminderconfig_bccemailid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Template</td>
											<td ti='6'><asp:TextBox ID="setting"  cm="emailtemplatesetting"  search='true'  dcn="setting_settingname" MaxLength="100" runat="server" m="setting" cn="settingname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtsettingid"  dcn="schedulereminderconfig_settingid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Is Active</td>
											<td ti='7'><asp:CheckBox ID="chkisactive"  dcn="schedulereminderconfig_isactive" runat="server"></asp:CheckBox></td>
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
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trschedulereminderdetail" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="2"><tr><td class='subtitle'>Schedule Reminder Detail</td></tr><tr runat="server" ID="trschedulereminderdetail_grid"><td><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~reminderdays~dc~reminderdays~smprefix~@sm1_~jt~~m~schedulereminderdetail' id='schedulereminderdetail_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='schedulereminderdetail_param' runat='server'/>
							<tr class='srepeater-header'><td cn="reminderdays">Reminder Days</td><td></td></tr>
							<asp:Literal ID="ltschedulereminderdetail" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="reminderdays" class="sbox val-i right first" maxlength='10'  ti='0'/></td></tr>
							<tr class='end'><td></td></tr>
							<tr><td><asp:TextBox ID="hdnTotalAmount" runat="server" CssClass="hidden gridtotal" Text="0"></asp:TextBox></td>
								<td class='right'><b>Total : </b></td><td class='gridtotal right bold'></td></tr>
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
				<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" Visible="false"/>
				<input type="button" class="close-page cancel" value="Cancel"/>
				<asp:Button ID="btnSubmitAndView" runat="server" Visible="false"/>
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
