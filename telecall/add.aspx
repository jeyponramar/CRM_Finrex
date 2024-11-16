 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="TeleCall_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        //SetDetailPage('<%=Request.QueryString["id"]%>');
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
				<asp:Button ID="btncompanyprofile" Text="Company Profile" OnClick="btncompanyprofile_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnnotinterested" Text="Not Interested" OnClick="btnnotinterested_Click"  Visible="false" runat="server" CssClass="button btnaction btnred "></asp:Button>
				<asp:Button ID="btnconverttoenquiry" Text="Convert To Enquiry" OnClick="btnconverttoenquiry_Click"  Visible="false" runat="server" CssClass="button btnaction btngreen "></asp:Button>
				<asp:Button ID="btneditlastcall" Text="Edit Last Call" OnClick="btneditlastcall_Click"  Visible="false" runat="server" CssClass="button btnaction btnorange "></asp:Button>
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
											<td class="label">Customer Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtcustomername" MaxLength="100" runat="server"  dcn="telecall_customername" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer Name" ValidationGroup="vg" ControlToValidate="txtcustomername"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Contact Person <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtcontactperson" MaxLength="100" runat="server"  dcn="telecall_contactperson" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Contact Person" ValidationGroup="vg" ControlToValidate="txtcontactperson"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td ti='2'><asp:TextBox ID="txtmobileno"  dcn="telecall_mobileno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Landline No</td>
											<td ti='3'><asp:TextBox ID="txtlandlineno"  dcn="telecall_landlineno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='4'><asp:TextBox ID="txtemailid"  dcn="telecall_emailid" MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Assigned Date</td>
											<td ti='5'><asp:TextBox ID="txtassigneddate"  dcn="telecall_assigneddate"  Enabled="false" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Assigned To</td>
											<td ti='6'><asp:TextBox ID="employee"  Enabled="false"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="telecall_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='7'><asp:TextBox ID="telecallstatus"  Enabled="false"  dcn="telecallstatus_status" MaxLength="100" runat="server" m="telecallstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txttelecallstatusid"  dcn="telecall_telecallstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Subject</td>
											<td ti='0'><asp:TextBox ID="txtsubject" MaxLength="100" runat="server"  dcn="telecall_subject" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Address</td>
											<td ti='1'><asp:TextBox TextMode="MultiLine" ID="txtaddress"  dcn="telecall_address" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">City</td>
											<td ti='2'><asp:TextBox ID="city"  dcn="city_cityname" MaxLength="100" runat="server" m="city" cn="cityname" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtcityid"  dcn="telecall_cityid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/></td>
										</tr>
										<tr>
											<td class="label">Area</td>
											<td ti='3'><asp:TextBox ID="area"  dcn="area_areaname" MaxLength="100" runat="server" m="area" cn="areaname" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtareaid"  dcn="telecall_areaid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='4'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="telecall_remarks" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Remind me</td>
											<td ti='5'><asp:CheckBox ID="chkremindme"  IsSave="false"  dcn="telecall_remindme" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Next Followup</td>
											<td ti='6'><asp:TextBox ID="txtnextfollowup"  dcn="telecall_nextfollowup" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr><td class='label'>Telecalling History</td></tr>
					<tr>
						<td><asp:Literal ID="lttelecallinghistory" runat="server"></asp:Literal></td>
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
				<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" ValidationGroup="vg" Visible="false"/>
				<input type="button" class="close-page cancel" value="Cancel"/>
					<!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
