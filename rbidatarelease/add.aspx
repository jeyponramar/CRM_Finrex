 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="RBIDataRelease_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
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
											<td class="label">Reference Date <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtreferencedate"  dcn="rbidatarelease_referencedate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Reference Date" ValidationGroup="vg" ControlToValidate="txtreferencedate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Particular</td>
											<td ti='1'><asp:TextBox ID="txtparticular"  search='true'  MaxLength="0" runat="server"  dcn="rbidatarelease_particular" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Title <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txttitle"  search='true'  MaxLength="0" runat="server"  dcn="rbidatarelease_title" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Title" ValidationGroup="vg" ControlToValidate="txttitle"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Document</td>
											<td ti='3'><uc:MultiFileUpload ID="mfudocument"  IsMultiple="true" FileType="Any" FolderPath="upload/rbidatarelease" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										<tr>
											<td class="label">Remark</td>
											<td ti='4'><asp:TextBox TextMode="MultiLine" ID="txtremark"  dcn="rbidatarelease_remark"  runat="server" CssClass="textarea"></asp:TextBox></td>
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
