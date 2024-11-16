 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Invoice_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>
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
                <!--ACTION_START-->
				<asp:Button ID="btngotoreceipt" Text="Go to Receipt" OnClick="btngotoreceipt_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnsendinvoice" Text="Send Invoice" OnClick="btnsendinvoice_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<!--ACTION_END-->
            </td>
            <td align="right"> 
                <uc:NextPrevDetail ID="NextPrevDetail" runat="server" />
                <asp:HyperLink ID="lnkPrint" runat="server" ToolTip="Print" ImageUrl="~/images/icon/print.png" Target="_blank"/>
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
											<td class="label">Invoice No</td>
											<td ti='0'><asp:TextBox ID="txtinvoiceno"  Enabled="false"  IsUnique="true"  search='true'  MaxLength="100" runat="server"  dcn="invoice_invoiceno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Invoice Date <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtinvoicedate"  dcn="invoice_invoicedate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Invoice Date" ValidationGroup="vg" ControlToValidate="txtinvoicedate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Customer Name <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="client"  jt="state,city"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac pop "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="invoice_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer Name" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">GSTIN</td>
											<td ti='3'><asp:TextBox ID="txtgstin"  MaxLength="100" runat="server"  dcn="invoice_gstin" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Director Name</td>
											<td ti='4'><asp:TextBox ID="txtdirectorname"  MaxLength="100" runat="server"  dcn="invoice_directorname" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Director Mobile</td>
											<td ti='5'><asp:TextBox ID="txtdirectormobile"  MaxLength="100" runat="server"  dcn="invoice_directormobile" CssClass="textbox val-mobile "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Director Email Id</td>
											<td ti='6'><asp:TextBox ID="txtdirectoremailid"  dcn="invoice_directoremailid" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Finance Name</td>
											<td ti='7'><asp:TextBox ID="txtfinancename"  MaxLength="100" runat="server"  dcn="invoice_financename" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Finance Mobile</td>
											<td ti='8'><asp:TextBox ID="txtfinancemobile"  MaxLength="100" runat="server"  dcn="invoice_financemobile" CssClass="textbox val-mobile "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Finance Email Id</td>
											<td ti='9'><asp:TextBox ID="txtfinanceemailid"  dcn="invoice_financeemailid" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Office Tel no</td>
											<td ti='10'><asp:TextBox ID="txtofficetelno"  dcn="invoice_officetelno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Office Email Id</td>
											<td ti='11'><asp:TextBox ID="txtofficeemailid"  dcn="invoice_officeemailid" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Subject</td>
											<td ti='12'><asp:TextBox ID="txtsubject"  MaxLength="300" runat="server"  dcn="invoice_subject" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Period From <span class="error">*</span></td>
											<td ti='13'><asp:TextBox ID="txtperiodfrom"  dcn="invoice_periodfrom" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv13" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Period From" ValidationGroup="vg" ControlToValidate="txtperiodfrom"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Period To <span class="error">*</span></td>
											<td ti='14'><asp:TextBox ID="txtperiodto"  dcn="invoice_periodto" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv14" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Period To" ValidationGroup="vg" ControlToValidate="txtperiodto"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Terms And Condition</td>
											<td ti='15'><asp:DropDownList ID="ddlsetupfortermsandconditionid"  dcn="invoice_setupfortermsandconditionid" runat="server" m="setupfortermsandcondition" cn="name" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Subscription Code</td>
											<td ti='16'><asp:TextBox ID="subscription"  dcn="subscription_subscriptioncode" MaxLength="100" runat="server" m="subscription" cn="subscriptioncode" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtsubscriptionid"  dcn="invoice_subscriptionid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Taxable Amount</td>
											<td ti='17'><asp:TextBox ID="txttaxableamount"  dcn="invoice_taxableamount" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">CGST</td>
											<td ti='18'><asp:TextBox ID="txtcgst"  dcn="invoice_cgst"  Enabled="false" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">SGST</td>
											<td ti='19'><asp:TextBox ID="txtsgst"  dcn="invoice_sgst"  Enabled="false" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">IGST</td>
											<td ti='20'><asp:TextBox ID="txtigst"  dcn="invoice_igst"  Enabled="false" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">GST</td>
											<td ti='21'><asp:TextBox ID="txtgst"  dcn="invoice_gst"  Enabled="false" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Total Amount</td>
											<td ti='22'><asp:TextBox ID="txttotalamount"  dcn="invoice_totalamount"  Enabled="false" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Proforma Invoice No</td>
											<td ti='0'><asp:TextBox ID="proformainvoice"  search='true'  dcn="proformainvoice_proformainvoiceno" MaxLength="100" runat="server" m="proformainvoice" cn="proformainvoiceno" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtproformainvoiceid"  dcn="invoice_proformainvoiceid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr ID="trreferenceno" runat="server" Visible="false">
											<td class="label">Reference No</td>
											<td ti='1'><asp:TextBox ID="txtreferenceno"  MaxLength="100" runat="server"  dcn="invoice_referenceno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Contact Person</td>
											<td ti='2'><asp:TextBox ID="txtcontactperson"  MaxLength="100" runat="server"  dcn="invoice_contactperson" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='3'><asp:TextBox ID="txtemailid"  dcn="invoice_emailid" MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td ti='4'><asp:TextBox ID="txtmobileno"  dcn="invoice_mobileno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Billing Address</td>
											<td ti='5'><asp:TextBox TextMode="MultiLine" ID="txtbillingaddress"  dcn="invoice_billingaddress" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">State</td>
											<td ti='6'><asp:TextBox ID="state"  dcn="state_state" MaxLength="100" runat="server" m="state" cn="state" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtstateid"  dcn="invoice_stateid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">City</td>
											<td ti='7'><asp:TextBox ID="city"  acparent="stateid"  dcn="city_cityname" MaxLength="100" runat="server" m="city" cn="cityname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtcityid"  dcn="invoice_cityid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Pin Code</td>
											<td ti='8'><asp:TextBox ID="txtpincode"  MaxLength="100" runat="server"  dcn="invoice_pincode" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Courier Address</td>
											<td ti='9'><asp:TextBox TextMode="MultiLine" ID="txtcourieraddress"  dcn="invoice_courieraddress" ml="800" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Lead By</td>
											<td ti='10'><asp:TextBox ID="leadby"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtleadbyid"  dcn="invoice_leadbyid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Advisory by</td>
											<td ti='11'><asp:TextBox ID="advisoryby"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtadvisorybyid"  dcn="invoice_advisorybyid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Meeting1</td>
											<td ti='12'><asp:TextBox ID="meeting1"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtmeeting1id"  dcn="invoice_meeting1id"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Meeting2</td>
											<td ti='13'><asp:TextBox ID="meeting2"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtmeeting2id"  dcn="invoice_meeting2id"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Closed By</td>
											<td ti='14'><asp:TextBox ID="closedby"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclosedbyid"  dcn="invoice_closedbyid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='15'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="invoice_remarks" ml="800" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Assigned To</td>
											<td ti='16'><asp:TextBox ID="employee"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="invoice_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='17'><asp:TextBox ID="invoicestatus"  IsSave="false" Enabled="false"  search='true'  dcn="invoicestatus_status" MaxLength="100" runat="server" m="invoicestatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtinvoicestatusid"  dcn="invoice_invoicestatusid"  issave="false" Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trinvoicedetail" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="2"><tr><td class='subtitle'>Invoice Detail</td></tr><tr runat="server" ID="trinvoicedetail_grid"><td><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~servicetypeid,amount~dc~servicetype_servicetype,amount~smprefix~@sm1_~jt~servicetype~m~invoicedetail' id='invoicedetail_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='invoicedetail_param' runat='server'/>
							<tr class='srepeater-header'><td cn="servicetype">Service Type</td><td cn="amount">Amount</td><td></td></tr>
							<asp:Literal ID="ltinvoicedetail" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="servicetypeid" class=' hdnac' maxlength='100'/><input type="text" id="servicetype" class="textbox first ac "  m="servicetype" cn="servicetype" include="0"   ti='0'/><img src="../images/down-arrow.png" class="epage"/></td>
								<td><input type="text" id="amount" class="sbox val-dbl right" ir='1'  maxlength='15'  ti='1'/></td></tr>
							<tr class='end'><td></td></tr>
						</table></td></tr>
					</table></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trinvoiceprofitsharingdetail" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="3"><tr><td class='subtitle'>Invoice Profit Sharing Detail</td></tr><tr runat="server" ID="trinvoiceprofitsharingdetail_grid"><td><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~employeeid,profitpercentage,profitamount~dc~employee_employeename,profitpercentage,profitamount~smprefix~@sm2_~jt~employee~m~invoiceprofitsharingdetail' id='invoiceprofitsharingdetail_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='invoiceprofitsharingdetail_param' runat='server'/>
							<tr class='srepeater-header'><td cn="employeename">Employee</td><td cn="profitpercentage">Profit Percentage</td><td cn="profitamount">Profit Amount</td><td></td></tr>
							<asp:Literal ID="ltinvoiceprofitsharingdetail" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="employeeid" class=' hdnac' maxlength='100'/><input type="text" id="employeename" class="textbox first ac "  m="employee" cn="employeename" include="0"   ti='0'/><img src="../images/down-arrow.png" class="epage"/></td>
								<td><input type="text" id="profitpercentage" class="sbox val-dbl right" maxlength='15'  ti='1'/></td>
								<td><input type="text" id="profitamount" class="sbox val-dbl right" maxlength='15'  ti='2'/></td></tr>
							<tr class='end'><td></td></tr>
						</table></td></tr>
					</table></td></tr><tr><td>&nbsp;</td></tr>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="4">
										<tr ID="trmodule" runat="server" Visible="false">
											<td class="label">Module</td>
											<td ti='0'><asp:TextBox ID="txtmodule"  search='true'  MaxLength="20" runat="server"  dcn="invoice_module" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trmoduleid" runat="server" Visible="false">
											<td class="label">Module Id</td>
											<td ti='1'><asp:TextBox ID="txtmoduleid"  dcn="invoice_moduleid" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Services</td>
											<td ti='2'><uc:MultiCheckbox ID="mcinvoiceservices"  Module="service" Column="service_service" TargetModule="invoiceservices"  runat="server" ></uc:MultiCheckbox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="5">
										<tr>
											<td class="label">Service Plan</td>
											<td ti='0'><asp:TextBox ID="serviceplan"  search='true'  dcn="serviceplan_planname" MaxLength="100" runat="server" m="serviceplan" cn="planname" CssClass="textbox ac txtac txtserviceplan populateserviceplandetail "></asp:TextBox><asp:TextBox id="txtserviceplanid"  dcn="invoice_serviceplanid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Softwares</td>
											<td ti='1'><uc:MultiCheckbox ID="mcinvoicesoftwares"  Module="prospect" Column="prospect_prospect" TargetModule="invoiceprospects"  runat="server" ></uc:MultiCheckbox></td>
										</tr>
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
