 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Document_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Document Type <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="finstationdocumenttype"  search='true'  dcn="finstationdocumenttype_documenttype" MaxLength="100" runat="server" m="finstationdocumenttype" cn="documenttype" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtfinstationdocumenttypeid"  dcn="document_finstationdocumenttypeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Document Type" ValidationGroup="vg" ControlToValidate="finstationdocumenttype"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtdate"  dcn="document_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Particular</td>
											<td ti='2'><asp:TextBox ID="txtparticular"  search='true'  MaxLength="300" runat="server"  dcn="document_particular" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Url</td>
											<td ti='3'><asp:TextBox ID="txturl"  MaxLength="0" runat="server"  dcn="document_url" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Remark</td>
											<td ti='4'><asp:TextBox ID="txtremark"  search='true'  MaxLength="300" runat="server"  dcn="document_remark" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Document</td>
											<td ti='5'><uc:MultiFileUpload ID="mfudocument"  IsMultiple="false" FileType="Any" FolderPath="upload/documents" SaveExt="true" runat="server" CssClass="textbox "></uc:MultiFileUpload>
							<asp:TextBox ID="txtdocument" runat="server" CssClass="hidden"></asp:TextBox></td>
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
