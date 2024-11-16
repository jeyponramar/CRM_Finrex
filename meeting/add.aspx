 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Meeting_add" EnableEventValidation="false" ValidateRequest="false"%>
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
                <!--ACTION_START-->
				<asp:Button ID="btnsendmeetingrequest" Text="Send Meeting Request" OnClick="btnsendmeetingrequest_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnsendmom" Text="Send MOM" OnClick="btnsendmom_Click"  Visible="false" runat="server" CssClass="button btnaction btnorange "></asp:Button>
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
											<td class="label">Subject <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtsubject"  IsUnique="true"  MaxLength="100" runat="server"  dcn="meeting_subject" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Subject" ValidationGroup="vg" ControlToValidate="txtsubject"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Client <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac search='true' "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="meeting_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Client" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtdate"  dcn="meeting_date" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Agenda</td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtagenda"  dcn="meeting_agenda" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Minutes of Meeting</td>
											<td ti='4'><asp:TextBox TextMode="MultiLine" ID="txtminutesofmeeting"  dcn="meeting_minutesofmeeting"  search='true' ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is Thanks Mail Sent</td>
											<td ti='5'><asp:CheckBox ID="chkisthanksmailsent"  Enabled="false"  dcn="meeting_isthanksmailsent" runat="server"></asp:CheckBox></td>
										</tr>
										<tr ID="trmodule" runat="server" Visible="false">
											<td class="label">Module</td>
											<td ti='6'><asp:TextBox ID="txtmodule"  MaxLength="100" runat="server"  dcn="meeting_module" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trmoduleid" runat="server" Visible="false">
											<td class="label">Module Id</td>
											<td ti='7'><asp:TextBox ID="txtmoduleid"  dcn="meeting_moduleid" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Is Meeting Request Sent</td>
											<td ti='0'><asp:CheckBox ID="chkismeetingrequestsent"  Enabled="false"  dcn="meeting_ismeetingrequestsent" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is MOM Sent</td>
											<td ti='1'><asp:CheckBox ID="chkismomsent"  Enabled="false"  dcn="meeting_ismomsent" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='2'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="meeting_remarks"  search='true' ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
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
					<!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
