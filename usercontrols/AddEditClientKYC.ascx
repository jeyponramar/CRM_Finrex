<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditClientKYC.ascx.cs" Inherits="usercontrols_AddEditClientKYC" %>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>
<script>
    $(document).ready(function(){
        if($(".jq-iseefcaccount").find("input").is(":checked"))
        {
            $(".jq-tdeefcbankcurrencies").show();
        }
        $(".jq-iseefcaccount").click(function(){
            if($(this).find("input").is(":checked"))
            {
                $(".jq-tdeefcbankcurrencies").show();
            }
            else
            {
                $(".jq-tdeefcbankcurrencies").hide();
            }
        });
    });
</script>
<table width="100%">
    <tr><td class="jq-message" align="center"><asp:Label ID="lblmessage" runat="server" CssClass="error"></asp:Label>
        <asp:TextBox ID="hdnmaintabid" runat="server" CssClass="hidden jq-hdnmaintabid"></asp:TextBox>
        <asp:TextBox ID="hdnclientid" runat="server" CssClass="hidden jq-hdnclientid"></asp:TextBox>
    </td></tr>
    <tr>
        <td>
            <asp:PlaceHolder ID="form" runat="server">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lttab" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr class="jq-trbacisinfo hidden">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="subtitle">Basic Business Information</td>
                                </tr>
                                <tr>
                                    <td style="border:solid 1px #eee;">
                                        <table width="100%">
                                            <tr>
                                                <td width="50%" style="vertical-align:top;">
                                                    <table width="100%" cellpadding="5">
                                                        <tr>
                                                            <td class="label">Company Name</td>
                                                            <td><asp:Label ID="lblcustomername" runat="server" CssClass="val"></asp:Label></td>
                                                        </tr>
                                                        <tr>
		                                                    <td class="label">Address</td>
		                                                    <td ti='9'><asp:TextBox TextMode="MultiLine" ID="txtaddress"  dcn="client_address" ml="500" runat="server" CssClass="textarea"></asp:TextBox></td>
	                                                    </tr>
	                                                    <tr>
		                                                    <td class="label">State</td>
		                                                    <td ti='7'><asp:TextBox ID="state"  dcn="state_state" MaxLength="100" runat="server" m="state" cn="state" CssClass="textbox txtqa ac txtac"></asp:TextBox>
		                                                    <asp:TextBox id="txtstateid"  dcn="client_stateid"  Text="0" runat="server" class=" hdnac"/><span class="ac-arrow"></span></td>
	                                                    </tr>
	                                                    <tr>
		                                                    <td class="label">City</td>
		                                                    <td ti='8'><asp:TextBox ID="city"  acparent="stateid" qaparent="stateid"  search='true'  dcn="city_cityname" MaxLength="100" runat="server" m="city" cn="cityname" CssClass="textbox txtqa ac txtac"></asp:TextBox>
		                                                    <asp:TextBox id="txtcityid"  dcn="client_cityid"  Text="0" runat="server" class=" hdnac"/>
		                                                    <span class="ac-arrow"></span></td>
	                                                    </tr>
	                                                    <tr>
					                                        <td class="label">Pin Code</td>
					                                        <td ti='11'><asp:TextBox ID="txtpincode"  MaxLength="100" runat="server"  dcn="client_pincode" CssClass="textbox"></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Office Landline No</td>
					                                        <td ti='11'><asp:TextBox ID="txtlandlineno"  MaxLength="100" runat="server"  dcn="client_landlineno" CssClass="textbox"></asp:TextBox></td>
				                                        </tr>
	                                                    <tr>
					                                        <td class="label">Office Accounts Email ID</td>
					                                        <td ti='10'><asp:TextBox ID="txtofficeaccountsemailid"  dcn="client_officeaccountsemailid" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">GST Number</td>
					                                        <td ti='11'><asp:TextBox ID="txtgstin"  MaxLength="100" runat="server"  dcn="client_gstin" CssClass="textbox"></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">LEI Code</td>
					                                        <td ti='12'><asp:TextBox ID="txtleicode"  MaxLength="100" runat="server"  dcn="client_leicode" CssClass="textbox"></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">LEI Renewal Date</td>
					                                        <td ti='13'><asp:TextBox ID="txtleirenewaldate"  dcn="client_leirenewaldate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
											                <td class="label">Industry</td>
											                <td ti='2'><asp:DropDownList ID="ddlindustrytypesid"  dcn="client_industrytypesid" runat="server" m="industrytypes" cn="industrytypes" CssClass="ddl"></asp:DropDownList></td>
										                </tr>
				                                        <tr>
					                                        <td class="label">Business Into</td>
					                                        <td ti='14'><uc:MultiCheckbox ID="mcbusinessinto"  Module="business" Column="business"  TargetModule="businessinto" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
                				                        
                                                    </table>
                                                </td>
                                                <td style="vertical-align:top;">
                                                    <table width="100%" cellpadding="5">
                                                        <tr>
					                                        <td class="label">Business Type</td>
					                                        <td ti='15'><uc:MultiCheckbox ID="mcbusinesstype"  Module="businesstype" Column="businesstype"  TargetModule="businesstype" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Export Annual Turnover in Rs.</td>
					                                        <td ti='16'><asp:DropDownList ID="ddlexportannualturnoverid"  dcn="client_exportannualturnoverid" runat="server" m="exportannualturnover" cn="turnover" CssClass="ddl"></asp:DropDownList></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Import Annual Turnover in Rs.</td>
					                                        <td ti='17'><asp:DropDownList ID="ddlimportannualturnoverid"  dcn="client_importannualturnoverid" runat="server" m="importannualturnover" cn="turnover" CssClass="ddl"></asp:DropDownList></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Enterprise Type</td>
					                                        <td ti='18'><uc:MultiCheckbox ID="mcenterprisetype"  Module="enterprisetype" Column="enterprisetype"  TargetModule="enterprisetype" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Importing/Exporting countries</td>
					                                        <td ti='19'><asp:DropDownList ID="importingexportingcountry" runat="server" m="importingexportingcountry" cn="country" CssClass="ddl ddlmultiselect"></asp:DropDownList>
					                                        <asp:TextBox ID="hdnimportingexportingcountryids" runat="server" CssClass="hdn"></asp:TextBox>
					                                        </td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Product Details</td>
					                                        <td ti='20'><asp:TextBox ID="txtproductdetails"  MaxLength="100" runat="server"  dcn="client_productdetails" CssClass="textbox"></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
											                <td class="label">Business/Payment Cycle</td>
											                <td ti='21'><uc:MultiCheckbox ID="mcclientbusinesspaymentcycle"  Module="kycbusinesspaymentcycle" Column="paymentcycle" IsCommaSeperated="true" TargetModule="businesspaymentcycle"  runat="server" ></uc:MultiCheckbox></td>
										                </tr>
				                                        <tr>
					                                        <td class="label">No. of Invoice Per Month</td>
					                                        <td ti='21'><asp:TextBox ID="txtnoofinvoicepermonth"  dcn="client_noofinvoicepermonth" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Value of Invoice approx range $</td>
					                                        <td ti='22'><asp:TextBox ID="txtvalueofinvoiceapproxrange"  dcn="client_valueofinvoiceapproxrange" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Benchmark/Costing Method</td>
					                                        <td ti='23'><asp:TextBox ID="txtbenchmarkcostingmethod"  MaxLength="100" runat="server"  dcn="client_benchmarkcostingmethod" CssClass="textbox"></asp:TextBox></td>
				                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Button ID="btnSubmit1" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="button"/>
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </td>
                                </tr>
                                
                            </table>
                        </td>
                    </tr>
                    <tr class="jq-trexposure hidden">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr><td class="subtitle">Exposure Details</td></tr>
                                <tr>
                                    <td style="border:solid 1px #eee;">
                                        <table width="100%">
                                            <tr>
                                                <td width="50%" style="vertical-align:top;">
                                                    <table width="100%" cellpadding="5">
                                                        <tr>
					                                        <td class="label">Currency Dealing in</td>
					                                        <td ti='19'><asp:DropDownList ID="currencydealingin" runat="server" m="bankauditcurrency" cn="currency" CssClass="ddl ddlmultiselect"></asp:DropDownList>
					                                        <asp:TextBox ID="hdncurrencydealingids" runat="server" CssClass="hdn"></asp:TextBox>
					                                        </td>
				                                        </tr>
                                                        <tr>
					                                        <td ti='24'><asp:CheckBox ID="chkiseefcaccount"  dcn="client_iseefcaccount" runat="server" Text="EEFC Account?" CssClass="jq-iseefcaccount"></asp:CheckBox></td>
					                                        <td class="jq-tdeefcbankcurrencies hidden">
					                                            <asp:DropDownList ID="eefcaccountcurrencies" runat="server" m="bankauditcurrency" cn="currency" CssClass="ddl ddlmultiselect"></asp:DropDownList>
					                                            <asp:TextBox ID="hdneefcaccountcurrencyids" runat="server" CssClass="hdn"></asp:TextBox>
					                                        </td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Hedging Policy</td>
					                                        <td ti='25'><uc:MultiCheckbox ID="mchedgingpolicy"  Module="hedgingpolicymaster" Column="hedgingpolicy"  TargetModule="hedgingpolicy" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Exposure Sheet maintained</td>
					                                        <td ti='26'><uc:MultiCheckbox ID="mcexposuresheetmaintained"  Module="exposuresoftwaremaster" Column="exposuresoftware"  TargetModule="exposuresheetmaintained" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Forward Limits</td>
					                                        <td ti='27'><uc:MultiCheckbox ID="mcforwardlimits"  Module="forwardlimitmaster" Column="forwardlimit"  TargetModule="forwardlimits" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Forward Contract Booking</td>
					                                        <td ti='28'><uc:MultiCheckbox ID="mcforwardcontractbooking"  Module="forwardcontractbookingmaster" Column="forwardcontractbooking"  TargetModule="forwardcontractbooking" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Hedging Period Months</td>
					                                        <td ti='29'><uc:MultiCheckbox ID="mchedgingperiod"  Module="hedgingperiodmaster" Column="hedgingperiod"  TargetModule="hedgingperiod" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align:top;">
                                                    <table width="100%" cellpadding="5">
                                                        
				                                        <tr>
					                                        <td class="label">Type of Booking</td>
					                                        <td ti='30'><uc:MultiCheckbox ID="mctypeofbooking"  Module="bookingtypemaster" Column="bookingtype"  TargetModule="typeofbooking" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Frequency of Advisory</td>
					                                        <td ti='31'><uc:MultiCheckbox ID="mcfrequencyofadvisory"  Module="advisoryfrequencymaster" Column="advisoryfrequency"  TargetModule="frequencyofadvisory" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
				                                        <tr>
					                                        <td class="label">Type of Funding</td>
					                                        <td ti='32'><uc:MultiCheckbox ID="mctypeoffunding"  Module="fundingtypemaster" Column="fundingtype"  TargetModule="typeoffunding" runat="server" IsCommaSeperated="true"></uc:MultiCheckbox></td>
				                                        </tr>
                                                    </table>
                                                </td>
                                             </tr>
                                             <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Button ID="btnSubmit2" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="button"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                				
                            </table>
                        </td>
                    </tr>
                    <tr class="jq-trownercontact hidden">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="subtitle">Owner's Details</td>
                                </tr>
                                <tr>
                                    <td style="border:solid 1px #eee;padding:20px;">
                                        <table width="100%">
                                            <tr><td align="right">
                                                <div class="linktext jq-common-btnaddmodal" mn="kycclientownercontact" modaltitle="Add Owner Contact">Add New Contact</div>
                                            </td></tr>
                                            <tr>
                                                <td class="jq-common-ajax-content-onload jq-kycownercontactgrid" m="kycownercontactgrid"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="jq-trfinancecontact hidden">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="subtitle">Finance Person's Details</td>
                                </tr>
                                <tr>
                                    <td style="border:solid 1px #eee;padding:20px;">
                                        <table width="100%">
                                            <tr><td align="right">
                                                <div class="linktext jq-common-btnaddmodal" mn="kycclientfinancecontact" modaltitle="Add Finance Person Contact">Add New Contact</div>
                                            </td></tr>
                                            <tr>
                                                <td class="jq-common-ajax-content-onload jq-kycfinancecontactgrid" m="kycfinancecontactgrid"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="jq-trbankdetail hidden">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="subtitle">Bank Details</td>
                                </tr>
                                <tr>
                                    <td style="border:solid 1px #eee;padding:20px;">
                                        <table width="100%">
                                            <tr><td align="right">
                                                <div class="linktext jq-common-btnaddmodal" mn="kycbankdetail" modaltitle="Add Bank Details" modalwidth='900px' modalheight='500px'>Add Bank Details</div>
                                            </td></tr>
                                            <tr>
                                                <td class="jq-common-ajax-content-onload jq-kycbankdetailgrid" m="kycbankdetailgrid"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </td>
    </tr>
</table>

