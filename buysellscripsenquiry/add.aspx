 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="BuySellScripsEnquiry_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Company Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="buysellscripsenquiry_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Company Name" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Person Name <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtpersonname"  search='true'  MaxLength="100" runat="server"  dcn="buysellscripsenquiry_personname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Person Name" ValidationGroup="vg" ControlToValidate="txtpersonname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='2'><asp:TextBox ID="txtemailid"  dcn="buysellscripsenquiry_emailid" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Mobile No <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtmobileno"  search='true'  MaxLength="100" runat="server"  dcn="buysellscripsenquiry_mobileno" CssClass="textbox val-mobile "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mobile No" ValidationGroup="vg" ControlToValidate="txtmobileno"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">IEC Code</td>
											<td ti='4'><asp:TextBox ID="txtieccode"  search='true'  MaxLength="100" runat="server"  dcn="buysellscripsenquiry_ieccode" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">City</td>
											<td ti='5'><asp:TextBox ID="txtcity"  search='true'  MaxLength="100" runat="server"  dcn="buysellscripsenquiry_city" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is Exporter</td>
											<td ti='6'><asp:CheckBox ID="chkisexporter"  dcn="buysellscripsenquiry_isexporter" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is Buy</td>
											<td ti='7'><asp:CheckBox ID="chkisbuy"  dcn="buysellscripsenquiry_isbuy" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Script Type</td>
											<td ti='8'><asp:TextBox ID="buysellscripttype"  search='true'  dcn="buysellscripttype_scriptname" MaxLength="100" runat="server" m="buysellscripttype" cn="scriptname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtbuysellscripttypeid"  dcn="buysellscripsenquiry_buysellscripttypeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Script  Amount</td>
											<td ti='9'><asp:TextBox ID="txtscriptamount"  dcn="buysellscripsenquiry_scriptamount" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Script Number</td>
											<td ti='10'><asp:TextBox ID="txtscriptnumber"  MaxLength="100" runat="server"  dcn="buysellscripsenquiry_scriptnumber" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Script Date</td>
											<td ti='11'><asp:TextBox ID="txtscriptdate"  dcn="buysellscripsenquiry_scriptdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Port Code</td>
											<td ti='12'><asp:TextBox ID="txtportcode"  MaxLength="100" runat="server"  dcn="buysellscripsenquiry_portcode" CssClass="textbox"></asp:TextBox></td>
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
