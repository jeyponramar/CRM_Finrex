 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Sales_add" EnableEventValidation="false" ValidateRequest="false"%>
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
                <asp:TextBox ID="quotationid" runat="server" CssClass=" hidden" Text="0"></asp:TextBox>
                <!--ACTION_START-->
				<asp:Button ID="btnconverttoinvoice" Text="Convert To Invoice" OnClick="btnconverttoinvoice_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnaddexpenses" Text="Add Expenses" OnClick="btnaddexpenses_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
				<!--ACTION_END-->
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
											<td class="label">Bill No</td>
											<td ti='0'><asp:TextBox ID="txtbillno"  Enabled="false"  IsUnique="true" MaxLength="100" runat="server"  dcn="sales_billno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Customer Name <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="client"  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac pop "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="sales_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer Name" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtdate"  dcn="sales_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="sales_remarks" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Status</td>
											<td ti='0'><asp:TextBox ID="salesstatus"  Enabled="false"  dcn="salesstatus_status" MaxLength="100" runat="server" m="salesstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtsalesstatusid"  dcn="sales_salesstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Billing Address</td>
											<td ti='1'><asp:TextBox TextMode="MultiLine" ID="txtbillingaddress"  dcn="sales_billingaddress" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Contact Person</td>
											<td ti='2'><asp:TextBox ID="txtcontactperson" MaxLength="100" runat="server"  dcn="sales_contactperson" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td ti='3'><asp:TextBox ID="txtmobileno"  dcn="sales_mobileno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='4'><asp:TextBox ID="txtemailid"  dcn="sales_emailid" MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trsalesdetail" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="2"><tr><td class='subtitle'>Sales Detail</td></tr><tr><td><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~productid,productmodelid,serialno,quantity,rate,amount~dc~product_productname,productmodel_modelno,serialno,quantity,rate,amount~smprefix~@sm1_~jt~product,productmodel~m~salesdetail' id='salesdetail_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='salesdetail_param' runat='server'/>
							<tr class='srepeater-header'><td cn="productname">Product</td><td cn="modelno">Model</td>
								<td cn="serialno">Serial No</td><td cn="quantity">Quantity</td><td cn="rate">Rate</td><td cn="amount">Amount</td><td></td></tr>
							<asp:Literal ID="ltsalesdetail" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="productid" class='hdnqa hdnac' maxlength='100'/><input type="text" id="productname" class="textbox first txtqa ac "  m="product" cn="productname" include="0"  ir='1'  cm="taxproduct" ir="1" ti='0'/><img src="../images/down-arr1.jpg" class="quick-menu"/></td>
								<td><input type="text" id="productmodelid" class=' hdnac' maxlength='100'/><input type="text" id="modelno" class="textbox ac "  m="productmodel" cn="modelno" include="0"  acparent="productid" ti='1'/><img src="../images/down-arrow.png" class="epage"/></td>
								<td><input type="text" id="serialno" class="textbox" maxlength='100'  ti='2'/></td>
								<td><input type="text" id="quantity" class="sbox val-i right" maxlength='10'  ti='3'/></td>
								<td><input type="text" id="rate" class="sbox val-dbl right" maxlength='15'  ti='4'/></td>
								<td><input type="text" id="amount" class="sbox val-dbl right" ir='1'  maxlength='15'  ti='5'/></td></tr>
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
                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" ValidationGroup="vg" Visible="false"/>
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
