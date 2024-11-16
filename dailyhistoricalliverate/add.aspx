 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="DailyHistoricalLiveRate_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        //SetDetailPage('<%=Request.QueryString["id"]%>');
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
         </tr>
         <tr>
            <td>
                <%--<input type="button" value="Edit" class="edit button dpage"/>--%>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
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
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Currency <span class="error">*</span></td>
											<td ti='0'><asp:DropDownList ID="ddlcurrencyid" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlcurrencyid_Changed" ValidationGroup="vg"></asp:DropDownList>
											<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddlcurrencyid" InitialValue="0" ErrorMessage="Required Currency"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td class="label">Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtdate"  dcn="dailyhistoricalliverate_date" runat="server" autocomplete="off" MaxLength="11" 
											Format="Date" CssClass="textbox datepicker" AutoPostBack="true" OnTextChanged="ddlcurrencyid_Changed"></asp:TextBox></td>
										</tr>
										</table>
									</td>
								</tr>
								<tr>
								    <td><asp:Literal ID="ltrate" runat="server"></asp:Literal></td>
								</tr>
							</table>
						</td>
					</tr>
					</table>
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center" colspan="2">
					<asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="save button" ValidationGroup="vg"/>
				<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" Visible="false"/>
				<input type="button" class="close-page cancel" value="Cancel"/>
				<asp:Button ID="btnSubmitAndView" runat="server" Visible="false"/>
            </td>
        </tr>

    </table>
</asp:PlaceHolder>

</asp:Content>
