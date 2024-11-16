 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="SerialNo_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Serial No <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtserialno" MaxLength="100" runat="server"  dcn="serialno_serialno" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Serial No" ValidationGroup="vg" ControlToValidate="txtserialno"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtdate"  dcn="serialno_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Product <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="product"  dcn="product_productname" MaxLength="100" runat="server" m="product" cn="productname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtproductid"  dcn="serialno_productid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Product" ValidationGroup="vg" ControlToValidate="product"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Client Name</td>
											<td ti='3'><asp:TextBox ID="txtclientname" MaxLength="100" runat="server"  dcn="serialno_clientname" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Engineer</td>
											<td ti='4'><asp:TextBox ID="employee"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="serialno_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Module <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="txtmodule" MaxLength="20" runat="server"  dcn="serialno_module" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Module" ValidationGroup="vg" ControlToValidate="txtmodule"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">In Out <span class="error">*</span></td>
											<td ti='6'><asp:TextBox ID="txtinout" MaxLength="3" runat="server"  dcn="serialno_inout" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv6" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required In Out" ValidationGroup="vg" ControlToValidate="txtinout"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Reference No <span class="error">*</span></td>
											<td ti='7'><asp:TextBox ID="txtreferenceno" MaxLength="100" runat="server"  dcn="serialno_referenceno" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv7" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Reference No" ValidationGroup="vg" ControlToValidate="txtreferenceno"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='8'><asp:TextBox ID="txtremarks" MaxLength="100" runat="server"  dcn="serialno_remarks" CssClass="textbox"></asp:TextBox></td>
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
                <asp:Button ID="btnSubmitAndView" runat="server" OnClick="btnSaveAndView_Click" Text="Save & View" CssClass="save button" ValidationGroup="vg"/>
		  <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" ValidationGroup="vg" Visible="false"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
