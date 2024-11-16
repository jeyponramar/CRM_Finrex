 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Opportunity_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<%@ Register Src="~/Followups.ascx" TagName="Followups" TagPrefix="uc" %>
<%@ Register Src="~/Chatter.ascx" TagName="Chatter" TagPrefix="uc" %>
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
				<asp:Button ID="btncancel" Text="Cancel" OnClick="btncancel_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnreject" Text="Reject" OnClick="btnreject_Click"  Visible="false" runat="server" CssClass="button btnaction btnred "></asp:Button>
				<asp:Button ID="btnsubscription" Text="Subscription" OnClick="btnsubscription_Click" runat="server" CssClass="button btnaction btngreen "></asp:Button>
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
											<td ti='0'><asp:TextBox ID="client"  jt="state,area"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac pop "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="opportunity_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer Name" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Subject</td>
											<td ti='1'><asp:TextBox ID="txtsubject"  MaxLength="100" runat="server"  dcn="opportunity_subject" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Contact Person</td>
											<td ti='2'><asp:TextBox ID="txtcontactperson"  search='true'  MaxLength="100" runat="server"  dcn="opportunity_contactperson" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Description</td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtdescription"  dcn="opportunity_description" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr ID="trpriorityid" runat="server" Visible="false">
											<td class="label">Priority</td>
											<td ti='4'><asp:DropDownList ID="ddlpriorityid"  dcn="opportunity_priorityid"  search='true' runat="server" m="priority" cn="priority" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Status <span class="error">*</span></td>
											<td ti='5'><asp:DropDownList ID="ddlopportunitystatusid"  dcn="opportunity_opportunitystatusid"  search='true' runat="server" m="opportunitystatus" cn="status" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Status" ValidationGroup="vg" ControlToValidate="ddlopportunitystatusid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Stage</td>
											<td ti='6'><asp:DropDownList ID="ddlopportunitystageid"  dcn="opportunity_opportunitystageid"  search='true' runat="server" m="opportunitystage" cn="stage" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='7'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="opportunity_remarks" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Opportunity Date <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtopportunitydate"  dcn="opportunity_opportunitydate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv8" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Opportunity Date" ValidationGroup="vg" ControlToValidate="txtopportunitydate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='1'><asp:TextBox ID="txtemailid"  dcn="opportunity_emailid" MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td ti='2'><asp:TextBox ID="txtmobileno"  dcn="opportunity_mobileno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Landline No</td>
											<td ti='3'><asp:TextBox ID="txtlandlineno"  dcn="opportunity_landlineno" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">State</td>
											<td ti='4'><asp:TextBox ID="state"  dcn="state_state" MaxLength="100" runat="server" m="state" cn="state" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtstateid"  dcn="opportunity_stateid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Area Name</td>
											<td ti='5'><asp:TextBox ID="area"  acparent="stateid"  search='true'  dcn="area_areaname" MaxLength="100" runat="server" m="area" cn="areaname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtareaid"  dcn="opportunity_areaid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Enquiry No <span class="error">*</span></td>
											<td ti='6'><asp:TextBox ID="enquiry"  Enabled="false"  search='true'  dcn="enquiry_enquiryno" MaxLength="100" runat="server" m="enquiry" cn="enquiryno" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtenquiryid"  dcn="opportunity_enquiryid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv14" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Enquiry No" ValidationGroup="vg" ControlToValidate="enquiry"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Address</td>
											<td ti='7'><asp:TextBox TextMode="MultiLine" ID="txtaddress"  dcn="opportunity_address" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Assign To</td>
											<td ti='8'><asp:TextBox ID="employee"  cm='marketingexecutive'  search='true'  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="opportunity_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
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
        <tr><td class="title qgrid">Quotations</td></tr>
        <tr>
            <td class="qgrid">
                 <uc:Grid id="grid" runat="server" Module="quotation_opportunity"  />       
        </td>
        </tr>
        <tr>
	            <td colspan="2">
		            <uc:Followups id="Followups" runat="server" Module="followups_opportunity"/>
	            </td>
	        </tr>
	        <tr>
	    <td colspan="2">
		    <uc:Chatter id="chatter" runat="server" Module="opportunity"/>
	    </td>
	    </tr>
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
