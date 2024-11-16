 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="FutureForwardrateHistory_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Date</td>
											<td ti='0'><asp:TextBox ID="txtdate"  dcn="futureforwardratehistory_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Upload Date <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtuploaddate"  dcn="futureforwardratehistory_uploaddate"  Enabled="false" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Upload Date" ValidationGroup="vg" ControlToValidate="txtuploaddate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Month End Start Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtmonthendstartdate"  dcn="futureforwardratehistory_monthendstartdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Month End Start Date" ValidationGroup="vg" ControlToValidate="txtmonthendstartdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Monthend End Date <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtmonthendenddate"  dcn="futureforwardratehistory_monthendenddate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Monthend End Date" ValidationGroup="vg" ControlToValidate="txtmonthendenddate"></asp:RequiredFieldValidator></td>
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
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trfutureforwardrate" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="2"><tr><td class='subtitle'>Future Forward Rate</td></tr><tr runat="server" ID="trfutureforwardrate_grid"><td><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~monthenddate,usdinrbid,usdinrask,eurinrbid,eurinrask,gbpinrbid,gbpinrask~dc~monthenddate,usdinrbid,usdinrask,eurinrbid,eurinrask,gbpinrbid,gbpinrask~smprefix~@sm1_~jt~~m~futureforwardrate' id='futureforwardrate_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='futureforwardrate_param' runat='server'/>
							<tr class='srepeater-header'><td cn="monthenddate">monthenddate</td>
								<td cn="usdinrbid">usdinrbid</td>
								<td cn="usdinrask">usdinrask</td>
								<td cn="eurinrbid">eurinrbid</td>
								<td cn="eurinrask">eurinrask</td>
								<td cn="gbpinrbid">gbpinrbid</td>
								<td cn="gbpinrask">gbpinrask</td><td></td></tr>
							<asp:Literal ID="ltfutureforwardrate" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="monthenddate" class="sbox datepicker first" maxlength='10'  ti='0'/></td>
								<td><input type="text" id="usdinrbid" class="textbox" maxlength='100'  ti='1'/></td>
								<td><input type="text" id="usdinrask" class="textbox" maxlength='100'  ti='2'/></td>
								<td><input type="text" id="eurinrbid" class="textbox" maxlength='100'  ti='3'/></td>
								<td><input type="text" id="eurinrask" class="textbox" maxlength='100'  ti='4'/></td>
								<td><input type="text" id="gbpinrbid" class="textbox" maxlength='100'  ti='5'/></td>
								<td><input type="text" id="gbpinrask" class="textbox" maxlength='100'  ti='6'/></td></tr>
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
