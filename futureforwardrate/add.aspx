 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Futureforwardrate_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">futureforwardratehistoryid</td>
											<td ti='0'><asp:TextBox ID="txtfutureforwardratehistoryid"  dcn="futureforwardrate_futureforwardratehistoryid" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">monthenddate</td>
											<td ti='1'><asp:TextBox ID="txtmonthenddate"  dcn="futureforwardrate_monthenddate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">usdinrbid</td>
											<td ti='2'><asp:TextBox ID="txtusdinrbid"  MaxLength="100" runat="server"  dcn="futureforwardrate_usdinrbid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">usdinrask</td>
											<td ti='3'><asp:TextBox ID="txtusdinrask"  MaxLength="100" runat="server"  dcn="futureforwardrate_usdinrask" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">eurinrbid</td>
											<td ti='4'><asp:TextBox ID="txteurinrbid"  MaxLength="100" runat="server"  dcn="futureforwardrate_eurinrbid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">eurinrask</td>
											<td ti='5'><asp:TextBox ID="txteurinrask"  MaxLength="100" runat="server"  dcn="futureforwardrate_eurinrask" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">gbpinrbid</td>
											<td ti='6'><asp:TextBox ID="txtgbpinrbid"  MaxLength="100" runat="server"  dcn="futureforwardrate_gbpinrbid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">gbpinrask</td>
											<td ti='7'><asp:TextBox ID="txtgbpinrask"  MaxLength="100" runat="server"  dcn="futureforwardrate_gbpinrask" CssClass="textbox"></asp:TextBox></td>
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
