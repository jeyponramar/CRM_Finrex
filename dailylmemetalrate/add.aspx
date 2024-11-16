 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="DailyLMEMetalRate_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Metal <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="metal"  search='true'  dcn="metal_metalname" MaxLength="100" runat="server" m="metal" cn="metalname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtmetalid"  dcn="dailylmemetalrate_metalid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Metal" ValidationGroup="vg" ControlToValidate="metal"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date</td>
											<td ti='1'><asp:TextBox ID="txtdate"  dcn="dailylmemetalrate_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Open</td>
											<td ti='2'><asp:TextBox ID="txtopen"  dcn="dailylmemetalrate_open" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">High</td>
											<td ti='3'><asp:TextBox ID="txthigh"  dcn="dailylmemetalrate_high" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Low</td>
											<td ti='4'><asp:TextBox ID="txtlow"  dcn="dailylmemetalrate_low" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Close</td>
											<td ti='5'><asp:TextBox ID="txtclose"  dcn="dailylmemetalrate_close" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Cash</td>
											<td ti='6'><asp:TextBox ID="txtcash"  dcn="dailylmemetalrate_cash" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">3months</td>
											<td ti='7'><asp:TextBox ID="txtthreemonths"  dcn="dailylmemetalrate_threemonths" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">1year</td>
											<td ti='8'><asp:TextBox ID="txtoneyear"  dcn="dailylmemetalrate_oneyear" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">CashOffer</td>
											<td ti='9'><asp:TextBox ID="txtcashoffer"  dcn="dailylmemetalrate_cashoffer" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">3monthsoffer</td>
											<td ti='10'><asp:TextBox ID="txtthreemonthsoffer"  dcn="dailylmemetalrate_threemonthsoffer" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">1yearoffer</td>
											<td ti='11'><asp:TextBox ID="txtoneyearoffer"  dcn="dailylmemetalrate_oneyearoffer" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">USD INR Close</td>
											<td ti='12'><asp:TextBox ID="txtusdinrclose"  dcn="dailylmemetalrate_usdinrclose" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">USD INR RBI refrate</td>
											<td ti='13'><asp:TextBox ID="txtusdinrrbirefrate"  dcn="dailylmemetalrate_usdinrrbirefrate" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Opening stock</td>
											<td ti='14'><asp:TextBox ID="txtopeningstock"  dcn="dailylmemetalrate_openingstock" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Live Warrants</td>
											<td ti='15'><asp:TextBox ID="txtlivewarrants"  dcn="dailylmemetalrate_livewarrants" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Cancelled Warrants</td>
											<td ti='16'><asp:TextBox ID="txtcancelledwarrants"  dcn="dailylmemetalrate_cancelledwarrants" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Prevday Opening Stock</td>
											<td ti='17'><asp:TextBox ID="txtprevdayopeningstock"  dcn="dailylmemetalrate_prevdayopeningstock" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Asian rate</td>
											<td ti='18'><asp:TextBox ID="txtasianrate"  dcn="dailylmemetalrate_asianrate" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Asian contract</td>
											<td ti='19'><asp:TextBox ID="txtasiancontract"  MaxLength="100" runat="server"  dcn="dailylmemetalrate_asiancontract" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">bid</td>
											<td ti='20'><asp:TextBox ID="txtbid"  dcn="dailylmemetalrate_bid" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">ask</td>
											<td ti='21'><asp:TextBox ID="txtask"  dcn="dailylmemetalrate_ask" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Prev Close</td>
											<td ti='22'><asp:TextBox ID="txtprevclose"  dcn="dailylmemetalrate_prevclose" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Change</td>
											<td ti='23'><asp:TextBox ID="txtchange"  dcn="dailylmemetalrate_change" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Change Per</td>
											<td ti='24'><asp:TextBox ID="txtchangeper"  dcn="dailylmemetalrate_changeper" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">ct week high</td>
											<td ti='25'><asp:TextBox ID="txtctweekhigh"  dcn="dailylmemetalrate_ctweekhigh" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">ct month high</td>
											<td ti='26'><asp:TextBox ID="txtctmonthhigh"  dcn="dailylmemetalrate_ctmonthhigh" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">ct month low</td>
											<td ti='27'><asp:TextBox ID="txtctmonthlow"  dcn="dailylmemetalrate_ctmonthlow" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">ct week low</td>
											<td ti='28'><asp:TextBox ID="txtctweeklow"  dcn="dailylmemetalrate_ctweeklow" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">1 week change per</td>
											<td ti='29'><asp:TextBox ID="txtoneweekchangeper"  dcn="dailylmemetalrate_oneweekchangeper" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">1 month change per</td>
											<td ti='30'><asp:TextBox ID="txtonemonthchangeper"  dcn="dailylmemetalrate_onemonthchangeper" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">3 month change per</td>
											<td ti='31'><asp:TextBox ID="txtthreemonthchangeper"  dcn="dailylmemetalrate_threemonthchangeper" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">1 year change per</td>
											<td ti='32'><asp:TextBox ID="txtoneyearchangeper"  dcn="dailylmemetalrate_oneyearchangeper" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">52 week high</td>
											<td ti='33'><asp:TextBox ID="txtfiftytwoweekhigh"  dcn="dailylmemetalrate_fiftytwoweekhigh" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">52 week low</td>
											<td ti='34'><asp:TextBox ID="txtfiftytwoweeklow"  dcn="dailylmemetalrate_fiftytwoweeklow" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Sterling Equivalent Cash</td>
											<td ti='35'><asp:TextBox ID="txtsterlingequivalentcash"  dcn="dailylmemetalrate_sterlingequivalentcash" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Sterling Equivalent 3Months</td>
											<td ti='36'><asp:TextBox ID="txtsterlingequivalentthreemonths"  dcn="dailylmemetalrate_sterlingequivalentthreemonths" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
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
