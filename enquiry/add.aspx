 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Enquiry_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<%@ Register Src="~/Followups.ascx" TagName="Followups" TagPrefix="uc" %>
<%@ Register Src="~/Chatter.ascx" TagName="Chatter" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".edit").click(function() {
            $(".qgrid").hide();
            //$(".btnaction").hide();
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
                <asp:TextBox ID="h_telecallingid" runat="server" CssClass="hidden" Text="0"></asp:TextBox>
                
                <!--ACTION_START-->
				<asp:Button ID="btntrial" Text="Trial" OnClick="btntrial_Click"  Visible="false"  runat="server" CssClass="button btnaction btngreen "></asp:Button>
				<asp:Button ID="btncanceled" Text="Canceled" OnClick="btncanceled_Click"  Visible="false"  runat="server" CssClass="button btnaction btnred "></asp:Button>
				<asp:Button ID="btnreject" Text="Reject" OnClick="btnreject_Click"  Visible="false"  runat="server" CssClass="button btnaction btnred btnstatus "></asp:Button>
				<asp:Button ID="btncompanyprofile" Text="Company Profile" OnClick="btncompanyprofile_Click"  Visible="false"  runat="server" CssClass="button btnaction btnemail "></asp:Button>
				<asp:Button ID="btnwelcomeemail" Text="Welcome Email" OnClick="btnwelcomeemail_Click"  Visible="false"  runat="server" CssClass="button btnaction btngreen "></asp:Button>
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
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Enquiry No</td>
											<td ti='0'><asp:TextBox ID="txtenquiryno"  Enabled="false"  IsUnique="true"  MaxLength="100" runat="server"  dcn="enquiry_enquiryno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Enquiry Date <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtenquirydate"  dcn="enquiry_enquirydate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Enquiry Date" ValidationGroup="vg" ControlToValidate="txtenquirydate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Existing Customer</td>
											<td ti='2'><asp:TextBox ID="client"  popsamedata_target='companyname' jt="state,area,designation"  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac pop popsamedata "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="enquiry_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Company Name <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtcompanyname"  search='true'  MaxLength="100" runat="server"  dcn="enquiry_companyname" CssClass="textbox companyname "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Company Name" ValidationGroup="vg" ControlToValidate="txtcompanyname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Contact Person <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtcontactperson"  search='true'  MaxLength="100" runat="server"  dcn="enquiry_contactperson" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Contact Person" ValidationGroup="vg" ControlToValidate="txtcontactperson"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Designation <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="designation"  dcn="designation_designation" MaxLength="100" runat="server" m="designation" cn="designation" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtdesignationid"  dcn="enquiry_designationid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Designation" ValidationGroup="vg" ControlToValidate="designation"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mobile No <span class="error">*</span></td>
											<td ti='6'><asp:TextBox ID="txtmobileno"  search='true'  MaxLength="10" runat="server"  dcn="enquiry_mobileno" CssClass="textbox val-mobile "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv6" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mobile No" ValidationGroup="vg" ControlToValidate="txtmobileno"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Email Id <span class="error">*</span></td>
											<td ti='7'><asp:TextBox ID="txtemailid"  dcn="enquiry_emailid" MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv7" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Email Id" ValidationGroup="vg" ControlToValidate="txtemailid"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Attachment</td>
											<td ti='8'><uc:MultiFileUpload ID="mfuattachment"  IsMultiple="true" FileType="ANy" FolderPath="upload/enquiry" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										<tr>
											<td class="label">Followups Date</td>
											<td ti='9'><asp:TextBox ID="txtfollowupsdate"  dcn="enquiry_followupsdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker followupdate hidden "></asp:TextBox></td>
										</tr>
										<tr ID="trsubject" runat="server" Visible="false">
											<td class="label">Subject</td>
											<td ti='10'><asp:TextBox ID="txtsubject"  MaxLength="100" runat="server"  dcn="enquiry_subject" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trdescription" runat="server" Visible="false">
											<td class="label">Description</td>
											<td ti='11'><asp:TextBox TextMode="MultiLine" ID="txtdescription"  dcn="enquiry_description" ml="500" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Industry <span class="error">*</span></td>
											<td ti='0'><asp:DropDownList ID="ddlindustrytypesid"  dcn="enquiry_industrytypesid"  search='true' runat="server" m="industrytypes" cn="industrytypes" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv9" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Industry" ValidationGroup="vg" ControlToValidate="ddlindustrytypesid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">State <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="state"  dcn="state_state" MaxLength="100" runat="server" m="state" cn="state" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtstateid"  dcn="enquiry_stateid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv10" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required State" ValidationGroup="vg" ControlToValidate="state"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Area <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="area"  acparent="stateid" qaparent="stateid"  search='true'  dcn="area_areaname" MaxLength="100" runat="server" m="area" cn="areaname" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtareaid"  dcn="enquiry_areaid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/>
						<asp:RequiredFieldValidator ID="rfv11" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Area" ValidationGroup="vg" ControlToValidate="area"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Exposure <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="exposure"  dcn="exposure_exposure" MaxLength="100" runat="server" m="exposure" cn="exposure" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtexposureid"  dcn="enquiry_exposureid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv12" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Exposure" ValidationGroup="vg" ControlToValidate="exposure"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Business <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="business"  search='true'  dcn="business_business" MaxLength="100" runat="server" m="business" cn="business" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtbusinessid"  dcn="enquiry_businessid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv13" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Business" ValidationGroup="vg" ControlToValidate="business"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Assigned To <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="employee"  search='true'  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="enquiry_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv14" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Assigned To" ValidationGroup="vg" ControlToValidate="employee"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Assigned Date <span class="error">*</span></td>
											<td ti='6'><asp:TextBox ID="txtassigneddate"  dcn="enquiry_assigneddate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv15" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Assigned Date" ValidationGroup="vg" ControlToValidate="txtassigneddate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Enquiry Status</td>
											<td ti='7'><asp:TextBox ID="enquirystatus"  Enabled="false"  dcn="enquirystatus_status" MaxLength="100" runat="server" m="enquirystatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtenquirystatusid"  dcn="enquiry_enquirystatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='8'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="enquiry_remarks" ml="800" runat="server" CssClass="textarea"></asp:TextBox></td>
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
        <tr>
            <td align="center" colspan="2" style="padding-top:10px;"><asp:Button ID="btnUpdateMobile" Visible="false" runat="server"
             OnClick="btnUpdateMobile_Click" Text="Update Mobile No" CssClass="save button" ValidationGroup="vg"  OnClientClick="return confirm('Are you sure you want to update the mobile no.?')"/></td>
        </tr>
    <%--<tr><td class="title qgrid">Quotations</td></tr>
    <tr>
        <td class="qgrid">
             <uc:Grid id="grid" runat="server" Module="quotation_enquiry" Visible="false"  />       
    </td>
    </tr>--%>
    <tr>
	        <td colspan="2">
		        <uc:Followups id="Followups" runat="server" Module="followups_enquiry"/>
	        </td>
	    </tr>
	    <tr>
	<td colspan="2">
		<uc:Chatter id="chatter" runat="server" Module="enquiry"/>
	</td>
	</tr>
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
