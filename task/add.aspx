 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Task_add" EnableEventValidation="false" ValidateRequest="false"%>
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
                <!--ACTION_START-->
				<asp:Button ID="btnhold" Text="Hold" OnClick="btnhold_Click"  Visible="false" runat="server" CssClass="button btnaction btnred btnstatus "></asp:Button>&nbsp;
				<asp:Button ID="btnclose" Text="Close" OnClick="btnclose_Click"  Visible="false" runat="server" CssClass="button btnaction btngreen btnstatus "></asp:Button>&nbsp;
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
											<td class="label">Client <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="client"  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="task_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Client" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Subject</td>
											<td ti='1'><asp:TextBox ID="txtsubject" MaxLength="100" runat="server"  dcn="task_subject" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Task Type <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="tasktype"  dcn="tasktype_tasktype" MaxLength="100" runat="server" m="tasktype" cn="tasktype" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txttasktypeid"  dcn="task_tasktypeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv7" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Task Type" ValidationGroup="vg" ControlToValidate="tasktype"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Description</td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtdescription"  dcn="task_description" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Attachment</td>
											<td ti='4'><uc:MultiFileUpload ID="mfuattachment"  IsMutiple="true"   FileType="Any" FolderPath="upload/task" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Assigned To</td>
											<td ti='0'><asp:TextBox ID="employee"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="task_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Assigned Date</td>
											<td ti='1'><asp:TextBox ID="txtassigneddate"  dcn="task_assigneddate" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='2'><asp:TextBox ID="txtremarks" MaxLength="100" runat="server"  dcn="task_remarks" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Closed Date</td>
											<td ti='3'><asp:TextBox ID="txtcloseddate"  dcn="task_closeddate" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='4'><asp:TextBox ID="status"  Enabled="false"  dcn="status_status" MaxLength="100" runat="server" m="status" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtstatusid"  dcn="task_statusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
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
                <asp:Button ID="btnSubmitAndView" runat="server" OnClick="btnSaveAndView_Click" Text="Save & View" CssClass="save button" ValidationGroup="vg"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
