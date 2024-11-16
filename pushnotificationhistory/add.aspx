 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="PushNotificationHistory_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Client <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="pushnotificationhistory_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Client" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Client User <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="clientuser"  dcn="clientuser_name" MaxLength="100" runat="server" m="clientuser" cn="name" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientuserid"  dcn="pushnotificationhistory_clientuserid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Client User" ValidationGroup="vg" ControlToValidate="clientuser"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">User <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="user"  search='true'  dcn="user_fullname" MaxLength="100" runat="server" m="user" cn="fullname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtuserid"  dcn="pushnotificationhistory_userid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required User" ValidationGroup="vg" ControlToValidate="user"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">OneSignal Id <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtonesignalid"  search='true'  MaxLength="100" runat="server"  dcn="pushnotificationhistory_onesignalid" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required OneSignal Id" ValidationGroup="vg" ControlToValidate="txtonesignalid"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Android OneSignal Id</td>
											<td ti='4'><asp:TextBox ID="txtandroidonesignalid"  MaxLength="100" runat="server"  dcn="pushnotificationhistory_androidonesignalid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">iOS OneSignal Id</td>
											<td ti='5'><asp:TextBox ID="txtiosonesignalid"  MaxLength="100" runat="server"  dcn="pushnotificationhistory_iosonesignalid" CssClass="textbox"></asp:TextBox></td>
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
