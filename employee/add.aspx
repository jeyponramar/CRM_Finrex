 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Employee_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Employee Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtemployeename"  IsUnique="true"  search='true'  MaxLength="100" runat="server"  dcn="employee_employeename" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Employee Name" ValidationGroup="vg" ControlToValidate="txtemployeename"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Employee Type <span class="error">*</span></td>
											<td ti='1'><asp:DropDownList ID="ddlemployeetypeid"  dcn="employee_employeetypeid"  search='true' runat="server" m="employeetype" cn="employeetype" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Employee Type" ValidationGroup="vg" ControlToValidate="ddlemployeetypeid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mobile No <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtmobileno"  dcn="employee_mobileno"  search='true' MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mobile No" ValidationGroup="vg" ControlToValidate="txtmobileno"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Email Id <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtemailid"  search='true'  MaxLength="100" runat="server"  dcn="employee_emailid" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Email Id" ValidationGroup="vg" ControlToValidate="txtemailid"></asp:RequiredFieldValidator></td>
										</tr>
										<tr ID="trbasicsalary" runat="server" Visible="false">
											<td class="label">Basic Salary <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtbasicsalary"  dcn="employee_basicsalary" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Basic Salary" ValidationGroup="vg" ControlToValidate="txtbasicsalary"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Address</td>
											<td ti='5'><asp:TextBox TextMode="MultiLine" ID="txtaddress"  dcn="employee_address" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Back Up Person</td>
											<td ti='6'><asp:TextBox ID="backupperson"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtbackuppersonid"  dcn="employee_backuppersonid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Manager</td>
											<td ti='7'><asp:TextBox ID="manager"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtmanagerid"  dcn="employee_managerid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
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
					<!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
