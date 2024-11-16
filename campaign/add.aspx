 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Campaign_add" EnableEventValidation="false" ValidateRequest="false"%>
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
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Campaign Name <span class="error">*</span></td>
											<td><asp:TextBox ID="txtcampaignname"  IsUnique="true" MaxLength="100" runat="server"  dcn="campaign_campaignname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Campaign Name" ValidationGroup="vg" ControlToValidate="txtcampaignname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Type <span class="error">*</span></td>
											<td><asp:DropDownList ID="ddlcampaigntypeid"  dcn="campaign_campaigntypeid" runat="server" m="campaigntype" cn="campaigntype" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Type" ValidationGroup="vg" ControlToValidate="ddlcampaigntypeid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Start Date</td>
											<td><asp:TextBox ID="txtstartdate"  dcn="campaign_startdate" runat="server" MaxLength="10" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">End Date</td>
											<td><asp:TextBox ID="txtenddate"  dcn="campaign_enddate" runat="server" MaxLength="10" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Budjet Cost</td>
											<td><asp:TextBox ID="txtbudjetcost"  dcn="campaign_budjetcost" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Actual Expense</td>
											<td><asp:TextBox ID="txtactualexpense"  dcn="campaign_actualexpense" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Description</td>
											<td><asp:TextBox TextMode="MultiLine" ID="txtdescription"  dcn="campaign_description" MaxLength="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Expected Revenue</td>
											<td><asp:TextBox ID="txtexpectedrevenue"  dcn="campaign_expectedrevenue" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Expected Response</td>
											<td><asp:TextBox ID="txtexpectedresponse"  dcn="campaign_expectedresponse" runat="server" MaxLength="10" CssClass="textbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Participants</td>
											<td><asp:TextBox TextMode="MultiLine" ID="txtparticipants"  dcn="campaign_participants" MaxLength="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Conducted By</td>
											<td><asp:TextBox TextMode="MultiLine" ID="txtconductedby"  dcn="campaign_conductedby" MaxLength="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Venue</td>
											<td><asp:TextBox TextMode="MultiLine" ID="txtvenue"  dcn="campaign_venue" MaxLength="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is Active</td>
											<td><asp:CheckBox ID="chkisactive"  dcn="campaign_isactive" runat="server"></asp:CheckBox></td>
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
                <asp:Button ID="btnSubmitAndView" runat="server" OnClick="btnSaveAndView_Click" Text="Save & View" CssClass="save button" ValidationGroup="vg"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
