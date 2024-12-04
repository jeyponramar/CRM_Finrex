 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="~/client/add.aspx.cs" Inherits="Client_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".btnhistory").click(function() {
            window.open('client-history.aspx?id=<%=Request.QueryString["id"]%>');
            return false;
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<ol id="tour">
      <li data-id="ctl00_ContentPlaceHolder1_txtcustomername" data-button="Next" class="custom" data-options="tipLocation:right">
        <h4>Enter Customer Name</h4>
        <p>Billing name will be automatically populated!</p>
      </li>
      <li data-id="ctl00_ContentPlaceHolder1_txtcontactperson" data-button="Next" data-options="tipLocation:right">
        <h4>Enter Contact Person</h4>
        <p>Specify the contact person name!</p>
      </li>
      <li data-id="ctl00_ContentPlaceHolder1_txtmobileno" data-button="Next" data-options="tipLocation:right">
        <h4>Enter Mobile No</h4>
        <p>Mention the mobile number of the contact person!</p>
      </li>
      <li data-id="ctl00_ContentPlaceHolder1_txtemailid" data-button="Next" data-options="tipLocation:right">
        <h4>Enter Email Id</h4>
        <p>Mention the email id of the contact person!</p>
      </li>
      <li data-id="ctl00_ContentPlaceHolder1_city" data-button="Next" data-options="tipLocation:right">
        <h4>City (Autofill Data)</h4>
        <p>Click and type few letters to find the data. Down arrow indicates that there is a auto populate facility available in this field. 
        Also if you mouseover on the icon you will find "Quick Add" option which will help you to add any master data without going to Master!</p>
      </li>
       <li data-id="name" data-button="Next" data-options="tipLocation:right">
        <h4 last="true">How to add multiple entries in Sub Grid</h4>
        <p>Enter required detail in the grid row. Then Press ENTER to add multiple entries.!</p>
        <p><div class="enterkey" /></p>
      </li>
    </ol>--%>
    <%--<div class="tour btntour"></div>--%>
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
				<asp:Button ID="btnaddcontact" Text="Add Contact" OnClick="btnaddcontact_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnviewcontacts" Text="View Contacts" OnClick="btnviewcontacts_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btntrial" Text="Trial" OnClick="btntrial_Click"  Visible="false"  runat="server" CssClass="button btnaction btnred "></asp:Button>
				<asp:Button ID="btnsubscription" Text="Subscription" OnClick="btnsubscription_Click"  Visible="false"  runat="server" CssClass="button btnaction btnorange "></asp:Button>
				<asp:Button ID="btnaddcompetitor" Text="Add Competitor" OnClick="btnaddcompetitor_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnviewcompetitor" Text="View Competitor" OnClick="btnviewcompetitor_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnupdatekycdetails" Text="Update KYC Details" OnClick="btnupdatekycdetails_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<!--ACTION_END-->
            </td>
            <td align="right"> 
                <uc:NextPrevDetail ID="NextPrevDetail" runat="server" />
            </td>
         </tr>
         <tr>
            <td class="form font-family"  colspan="2">
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
											<td class="label">Customer Code</td>
											<td ti='0'><asp:TextBox ID="txtcustomercode"  Enabled="false"  search='true'  MaxLength="100" runat="server"  dcn="client_customercode" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Customer Name <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtcustomername"  IsUnique="true"  search='true'  MaxLength="150" runat="server"  dcn="client_customername" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer Name" ValidationGroup="vg" ControlToValidate="txtcustomername"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Contact Person</td>
											<td ti='2'><asp:TextBox ID="txtcontactperson"  search='true'  MaxLength="100" runat="server"  dcn="client_contactperson" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Designation</td>
											<td ti='3'><asp:TextBox ID="designation"  dcn="designation_designation" MaxLength="100" runat="server" m="designation" cn="designation" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtdesignationid"  dcn="client_designationid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td ti='4'><asp:TextBox ID="txtmobileno"  dcn="client_mobileno"  search='true' MaxLength="10" runat="server" CssClass="textbox val-ph val-mobile "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Landline No</td>
											<td ti='5'><asp:TextBox ID="txtlandlineno"  dcn="client_landlineno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='6'><asp:TextBox ID="txtemailid"  dcn="client_emailid" MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">State</td>
											<td ti='7'><asp:TextBox ID="state"  dcn="state_state" MaxLength="100" runat="server" m="state" cn="state" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtstateid"  dcn="client_stateid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">City</td>
											<td ti='8'><asp:TextBox ID="city"  acparent="stateid"  dcn="city_cityname" MaxLength="100" runat="server" m="city" cn="cityname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtcityid"  dcn="client_cityid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Area</td>
											<td ti='9'><asp:TextBox ID="area"  acparent="stateid" qaparent="stateid"  search='true'  dcn="area_areaname" MaxLength="100" runat="server" m="area" cn="areaname" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtareaid"  dcn="client_areaid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/></td>
										</tr>
										<tr>
											<td class="label">Address</td>
											<td ti='10'><asp:TextBox TextMode="MultiLine" ID="txtaddress"  dcn="client_address" ml="500" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">GST Number</td>
											<td ti='11'><asp:TextBox ID="txtgstin"  MaxLength="100" runat="server"  dcn="client_gstin" CssClass="textbox"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Pan No</td>
											<td ti='0'><asp:TextBox ID="txtpanno"  MaxLength="100" runat="server"  dcn="client_panno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Website</td>
											<td ti='1'><asp:TextBox ID="txtwebsite"  MaxLength="100" runat="server"  dcn="client_website" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Industry</td>
											<td ti='2'><asp:DropDownList ID="ddlindustrytypesid"  dcn="client_industrytypesid" runat="server" m="industrytypes" cn="industrytypes" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Campaign</td>
											<td ti='3'><asp:TextBox ID="campaign"  dcn="campaign_campaignname" MaxLength="100" runat="server" m="campaign" cn="campaignname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtcampaignid"  dcn="client_campaignid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Exposure</td>
											<td ti='4'><asp:TextBox ID="exposure"  dcn="exposure_exposure" MaxLength="100" runat="server" m="exposure" cn="exposure" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtexposureid"  dcn="client_exposureid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Business</td>
											<td ti='5'><asp:TextBox ID="business"  dcn="business_business" MaxLength="100" runat="server" m="business" cn="business" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtbusinessid"  dcn="client_businessid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Client Group</td>
											<td ti='6'><asp:TextBox ID="clientgroup"  Enabled="false"  search='true'  dcn="clientgroup_groupname" MaxLength="100" runat="server" m="clientgroup" cn="groupname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientgroupid"  dcn="client_clientgroupid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Assign To</td>
											<td ti='7'><asp:TextBox ID="employee"  Enabled="false"  search='true'  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="client_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Subscription Status</td>
											<td ti='8'><asp:TextBox ID="subscriptionstatus"  Enabled="false"  search='true'  dcn="subscriptionstatus_status" MaxLength="100" runat="server" m="subscriptionstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtsubscriptionstatusid"  dcn="client_subscriptionstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Start Date</td>
											<td ti='9'><asp:TextBox ID="txtstartdate"  dcn="client_startdate"  Enabled="false" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Software End Date</td>
											<td ti='10'><asp:TextBox ID="txtenddate"  dcn="client_enddate"  Enabled="false" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Whatsapp End Date</td>
											<td ti='11'><asp:TextBox ID="txtwhatsappenddate"  dcn="client_whatsappenddate"  Enabled="false" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Contact Type</td>
											<td ti='12'><asp:DropDownList ID="ddlcontacttypeid"  dcn="client_contacttypeid" runat="server" m="contacttype" cn="contacttype" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Unique Id</td>
											<td ti='13'><asp:TextBox ID="txtuniqueid"  Enabled="false"  MaxLength="100" runat="server"  dcn="client_uniqueid" CssClass="textbox"></asp:TextBox></td>
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
					
					<!--JSCODE_END-->

</asp:Content>
