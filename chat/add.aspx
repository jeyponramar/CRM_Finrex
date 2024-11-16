 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Chat_add" EnableEventValidation="false" ValidateRequest="false"%>
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
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Chat Client <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="chatclient"  dcn="chatclient_name" MaxLength="100" runat="server" m="invoicepending" cn="name" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtchatclientid"  dcn="chat_chatclientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Chat Client" ValidationGroup="vg" ControlToValidate="chatclient"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date</td>
											<td ti='1'><asp:TextBox ID="txtdate"  dcn="chat_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">City</td>
											<td ti='2'><asp:TextBox ID="txtcity"  search='true'  MaxLength="50" runat="server"  dcn="chat_city" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Country</td>
											<td ti='3'><asp:TextBox ID="txtcountry"  MaxLength="100" runat="server"  dcn="chat_country" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">IP</td>
											<td ti='4'><asp:TextBox ID="txtip"  search='true'  MaxLength="20" runat="server"  dcn="chat_ip" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is Active</td>
											<td ti='5'><asp:CheckBox ID="chkisactive"  dcn="chat_isactive" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Chat Status</td>
											<td ti='6'><asp:TextBox ID="chatstatus"  search='true'  dcn="chatstatus_status" MaxLength="100" runat="server" m="sharecomment" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtchatstatusid"  dcn="chat_chatstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Agent</td>
											<td ti='7'><asp:TextBox ID="agent"  dcn="agent_agentname" MaxLength="100" runat="server" m="" cn="agentname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtagentid"  dcn="chat_agentid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Agent Name</td>
											<td ti='8'><asp:TextBox ID="txtagentname"  MaxLength="100" runat="server"  dcn="chat_agentname" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Company Name</td>
											<td ti='9'><asp:TextBox ID="txtcompanyname"  search='true'  MaxLength="100" runat="server"  dcn="chat_companyname" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Client Id <span class="error">*</span></td>
											<td ti='10'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="chat_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv10" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Client Id" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr ID="trclientuserid" runat="server" Visible="false">
											<td class="label">Client User Id</td>
											<td ti='11'><asp:TextBox ID="clientuser"  dcn="clientuser_name" MaxLength="100" runat="server" m="clientuser" cn="name" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientuserid"  dcn="chat_clientuserid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Feedback</td>
											<td ti='12'><asp:TextBox TextMode="MultiLine" ID="txtfeedback"  dcn="chat_feedback" ml="500" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Rating</td>
											<td ti='13'><asp:TextBox ID="txtrating"  dcn="chat_rating" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
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
