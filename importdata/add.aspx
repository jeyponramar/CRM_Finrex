 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="ImportData_add" EnableEventValidation="false" ValidateRequest="false"%>
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
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr visible="false" runat="server" id="trModule">
											<td class="label">Module Name <span class="error">*</span></td>
											<td><asp:DropDownList ID="ddlimportdatamoduleid"  dcn="importdata_importdatamoduleid" runat="server" m="importdatamodule" cn="modulename" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Module Name" ValidationGroup="vg" ControlToValidate="ddlimportdatamoduleid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Upload Excel File (.xlsx)</td>
											<td><uc:MultiFileUpload ID="mfuuploadexcelfilexlsxxls"  IsMutiple="false" FileType="Doc" SaveExt="false"  FolderPath="upload/importdata" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Download Template</td>
											<td><asp:LinkButton Text="Download Template" PostBackUrl="~/importdata/add.aspx?isdownloadtemplate=1" OnClick="btndownloadtemplate_Click" runat="server" CssClass="button greenbutton "></asp:LinkButton>											
											</td>
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
         <tr><td colspan="2"><asp:Literal ID="ltColumns" runat="server"></asp:Literal></td></tr>
         <tr>
            <td colspan="2">
                <table border="1" width="20%">
                    <tr>
                        <td class="repeater"><b><b>Total Existing Data (Before Import data from ExcelData)</b> </td>
                        <td class="repeater"><b>Total ExcelData</b> </td>
                        <td class="repeater"><b>Total ImportedData</b> </td>
                        <td class="repeater"><b>Total Data (After Import Data From Excel Data)</b> </td>
                    </tr>                            
                    <tr>
                        <asp:Literal ID="ltDetails" runat="server"></asp:Literal>
                    </tr>
                </table>
            </td>
        </tr>
        
		 <tr>
            <td align="center" colspan="2">
				<!--SAVEBUTTON_START-->
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Import Data" CssClass="save button" ValidationGroup="vg"/>
                <asp:Button ID="btnSubmitAndView" Visible="false" runat="server" OnClick="btnSaveAndView_Click" Text="Save & View" CssClass="save button" ValidationGroup="vg"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Literal ID="ltImportStatus" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
