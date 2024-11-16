 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Contacts_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $("#ctl00_ContentPlaceHolder1_chkisactive").click(function(){
            if($(this).is(":checked"))return;
            $("#ctl00_ContentPlaceHolder1_chkisemailcommunication").prop("checked",false);
            $("#ctl00_ContentPlaceHolder1_chkissmscommunication").prop("checked",false);
            $("#ctl00_ContentPlaceHolder1_chkiswhatsappcommunication").prop("checked",false);
            $("#ctl00_ContentPlaceHolder1_chkiswebuser").prop("checked",false);
            $("#ctl00_ContentPlaceHolder1_chkisexeuser").prop("checked",false);
            $("#ctl00_ContentPlaceHolder1_chkismobileuser").prop("checked",false);
            $("#ctl00_ContentPlaceHolder1_chkisfinmessenger").prop("checked",false);
            $("#ctl00_ContentPlaceHolder1_chkisemailcommunication").prop("checked",false);
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
                <!--ACTION_START-->
				<asp:Button ID="btnsendpassword" Text="Send Password" OnClick="btnsendpassword_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
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
											<td class="label">Customer Name</td>
											<td ti='0'><asp:TextBox ID="client"  popsamedata_target='companyname'  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac popsamedata "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="contacts_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Contact Person <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtcontactperson"  search='true'  MaxLength="100" runat="server"  dcn="contacts_contactperson" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Contact Person" ValidationGroup="vg" ControlToValidate="txtcontactperson"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Contact Type</td>
											<td ti='2'><asp:TextBox ID="contactpersontype"  search='true'  dcn="contactpersontype_contacttype" MaxLength="100" runat="server" m="contactpersontype" cn="contacttype" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtcontactpersontypeid"  dcn="contacts_contactpersontypeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Designation</td>
											<td ti='3'><asp:TextBox ID="designation"  search='true'  dcn="designation_designation" MaxLength="100" runat="server" m="designation" cn="designation" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtdesignationid"  dcn="contacts_designationid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td ti='4'><asp:TextBox ID="txtmobileno"  dcn="contacts_mobileno"  search='true' MaxLength="10" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Landline No</td>
											<td ti='5'><asp:TextBox ID="txtlandlineno"  MaxLength="100" runat="server"  dcn="contacts_landlineno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='6'><asp:TextBox ID="txtemailid"  dcn="contacts_emailid"  search='true' MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Date of Birth</td>
											<td ti='7'><asp:TextBox ID="txtdateofbirth"  dcn="contacts_dateofbirth" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Anniversary Date</td>
											<td ti='8'><asp:TextBox ID="txtanniversarydate"  dcn="contacts_anniversarydate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Is Email Communication</td>
											<td ti='0'><asp:CheckBox ID="chkisemailcommunication"  dcn="contacts_isemailcommunication" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is SMS Communication</td>
											<td ti='1'><asp:CheckBox ID="chkissmscommunication"  dcn="contacts_issmscommunication" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is Whatsapp Communication</td>
											<td ti='2'><asp:CheckBox ID="chkiswhatsappcommunication"  dcn="contacts_iswhatsappcommunication" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Finstation</td>
											<td ti='3'><asp:CheckBox ID="chkiswebuser"  dcn="contacts_iswebuser" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Finwatch</td>
											<td ti='4'><asp:CheckBox ID="chkisexeuser"  dcn="contacts_isexeuser" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">FinIcon</td>
											<td ti='5'><asp:CheckBox ID="chkismobileuser"  dcn="contacts_ismobileuser" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">FinFulse</td>
											<td ti='6'><asp:CheckBox ID="chkisfinmessenger"  dcn="contacts_isfinmessenger" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is Active</td>
											<td ti='7'><asp:CheckBox ID="chkisactive"  dcn="contacts_isactive" runat="server"></asp:CheckBox></td>
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
