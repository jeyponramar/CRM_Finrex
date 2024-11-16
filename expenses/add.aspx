 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Expenses_add" EnableEventValidation="false" ValidateRequest="false"%>
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
                <!--ACTION_START-->
				<asp:Button ID="btnaddpayment" Text="Add Payment" OnClick="btnaddpayment_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
				<!--ACTION_END-->
            </td>
            <td align="right"> 
                <uc:NextPrevDetail ID="NextPrevDetail" runat="server" />
                <asp:HyperLink ID="lnkPrint" runat="server" ToolTip="Print" ImageUrl="~/images/icon/print.png" CssClass="hand" Target="_blank"/>
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
											<td class="label">Voucher No</td>
											<td ti='0'><asp:TextBox ID="txtvoucherno"  Enabled="false"  IsUnique="true" MaxLength="100" runat="server"  dcn="expenses_voucherno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Expense Date <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtexpensedate"  dcn="expenses_expensedate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Expense Date" ValidationGroup="vg" ControlToValidate="txtexpensedate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Referance No</td>
											<td ti='2'><asp:TextBox ID="txtreferanceno" MaxLength="100" runat="server"  dcn="expenses_referanceno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Customer Name</td>
											<td ti='3'><asp:TextBox ID="client"  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="expenses_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Employee Name <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="employee"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="expenses_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Employee Name" ValidationGroup="vg" ControlToValidate="employee"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Expenses For</td>
											<td ti='5'><asp:TextBox ID="txtexpensesfor" MaxLength="100" runat="server"  dcn="expenses_expensesfor" CssClass="textbox"></asp:TextBox></td>
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
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trexpensesdetail" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="2"><tr><td class='subtitle'>Expenses Detail</td></tr><tr><td><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~expensetypeid,amount~dc~expensetype_expensetype,amount~smprefix~@sm1_~jt~expensetype~m~expensesdetail' id='expensesdetail_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='expensesdetail_param' runat='server'/>
							<tr class='srepeater-header'><td cn="expensetype">Expenses Type</td><td cn="amount">Amount</td><td></td></tr>
							<asp:Literal ID="ltexpensesdetail" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="expensetypeid" class=' hdnac' maxlength='100'/><input type="text" id="expensetype" class="textbox first ac "  m="expensetype" cn="expensetype" include="0"  ir='1'   ti='0'/><img src="../images/down-arrow.png" class="epage"/></td>
								<td><input type="text" id="amount" class="sbox val-dbl right" maxlength='15'  ti='1'/></td></tr>
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
