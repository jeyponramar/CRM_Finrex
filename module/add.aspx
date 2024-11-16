 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Module_add" EnableEventValidation="false" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".modulename").change(function() {
            $(".tablename").val("tbl_" + $(this).val().trim().toLowerCase().replace(/\W/g, ""));
            $(".addtitle").val("Add " + $(this).val());
            $(".edittitle").val("Edit " + $(this).val());
            $(".viewtitle").val("View " + $(this).val());
        });
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
                <asp:Button ID="btnConfig" runat="server" OnClick="btnConfig_Click" Text="Config" CssClass="button"/>
                <asp:Button ID="btnConfigBulkUpdate" runat="server" OnClick="btnConfigBulkUpdate_Click" Text="Config - Bulk" CssClass="button"/>
                <asp:Button ID="btnActionManager" runat="server" OnClick="btnActionManager_Click" Text="Action Manager" CssClass="button"/>
                Go to 
                <asp:DropDownList runat="server" ID="GotoConfigPageSetting" OnSelectedIndexChanged="GotoConfigPageSetting_changed" AutoPostBack="true">
                <asp:ListItem Value="0">Select</asp:ListItem>
                <asp:ListItem Value="configure-viewpage-view">Go To View Page Settings</asp:ListItem>
                <asp:ListItem Value="controlpanelview-setting">Go To Quick Page Alignment</asp:ListItem>
                <asp:ListItem Value="configure-viewpage">Go To Auto Bind Sub Grid</asp:ListItem>
                <asp:ListItem Value="Configure-UpdatableGrid">Go To updatable Sub Grid</asp:ListItem>
                </asp:DropDownList>
                <!--ACTIONBUTTON_START-->
                
                <!--ACTIONBUTTON_END-->
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center" colspan="4"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                <tr>
					<td>
					<!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Module Name <span class="error">*</span></td>
											<td><asp:TextBox ID="txtmodulename" MaxLength="50" runat="server"  dcn="module_modulename" CssClass="textbox modulename "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Module Name" ValidationGroup="vg" ControlToValidate="txtmodulename"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Table Name <span class="error">*</span></td>
											<td><asp:TextBox ID="txttablename" MaxLength="50" runat="server"  dcn="module_tablename" CssClass="textbox tablename "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Table Name" ValidationGroup="vg" ControlToValidate="txttablename"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Menu</td>
											<td><asp:TextBox ID="menu"  dcn="menu_menuname" MaxLength="100" runat="server" m="menu" cn="menuname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtmenuid"  dcn="module_menuid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Enable Grid Edit</td>
											<td><asp:CheckBox ID="chkenablegridedit"  dcn="module_enablegridedit" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is Mobile</td>
											<td><asp:CheckBox ID="chkismobile"  dcn="module_ismobile" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is Right Report</td>
											<td><asp:CheckBox ID="chkisrightreport"  dcn="module_isrightreport" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Apply View Rights</td>
											<td><asp:CheckBox ID="chkapplyviewrights"  dcn="module_applyviewrights" runat="server"></asp:CheckBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Add Title</td>
											<td><asp:TextBox ID="txtaddtitle" MaxLength="50" runat="server"  dcn="module_addtitle" CssClass="textbox addtitle "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">View Title</td>
											<td><asp:TextBox ID="txtviewtitle" MaxLength="50" runat="server"  dcn="module_viewtitle" CssClass="textbox viewtitle "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Edit Title</td>
											<td><asp:TextBox ID="txtedittitle" MaxLength="50" runat="server"  dcn="module_edittitle" CssClass="textbox edittitle "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is Editable</td>
											<td><asp:CheckBox ID="chkiseditable"  Checked="true"  dcn="module_iseditable" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Description</td>
											<td><asp:TextBox TextMode="MultiLine" ID="txtdescription"  dcn="module_description" MaxLength="100" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trextravalues" ><td class='subgridbox'><table width='100%' cellspacing='0'><tr><td class='subtitle'>Extra Values</td></tr><tr><td style='padding:10px;'><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~extracolumn,extravaluetypeid,extravalue,extravaluesetting~dc~extracolumn,extravaluetype_extravaluetype,extravalue,extravaluesetting~smprefix~@sm1_~jt~extravaluetype~m~extravalues' id='extravalues_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='extravalues_param' runat='server'/>
							<tr class='srepeater-header'>
								<td cn="extracolumn">Extra Column</td><td cn="extravaluetypeid">Extra Value Type</td>
								<td cn="extravalue">Extra Value</td>
								<td cn="extravaluesetting">Extra Value Setting</td><td></td></tr>
							<asp:Literal ID="ltextravalues" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="extracolumn" class="textbox first" maxlength='100'/></td>
								<td><asp:DropDownList ID="extravaluetypeid" runat="server" m="extravaluetypeid" cn="extravaluetype" CssClass="ddl extravaluetypeid"></asp:DropDownList><img src="../images/down-arrow.png" class="epage"/></td>
								<td><input type="text" id="extravalue" class="textbox" maxlength='100'/></td>
								<td><input type="text" id="extravaluesetting" class="textbox" maxlength='100'/></td></tr>
							<tr class='end'><td></td></tr>
						</table></td></tr>
					</table></td></tr><tr><td>&nbsp;</td></tr>
					<tr><td class='label'>Settings</td></tr>
					<tr>
						<td><asp:TextBox TextMode="MultiLine" ID="txtsettings"  Width="100%" Height="300"  dcn="module_settings" MaxLength="1000" runat="server" CssClass="textarea"></asp:TextBox></td>
				</tr>
					</table>
					<!--CONTROLS_END-->
					</td>
                </tr>
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center">
                <!--SAVEBUTTON_START-->
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="save button" ValidationGroup="vg"/>
                <!--SAVEBUTTON_END-->
                <asp:Button ID="btnSaveAndConfig" runat="server" OnClick="btnSaveAndConfig_Click" Text="Save and Config" CssClass="save button" ValidationGroup="vg"/>
                <asp:Button ID="btnSaveAndBulkConfig" runat="server" OnClick="btnSaveAndBulkConfig_Click" Text="Save and Bulk Config" CssClass="save button" ValidationGroup="vg"/>
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					<script>
	$(document).ready(function() {
		//add or update
		setTotal($(".grid"));
		$(".newrow").find("input").live("keypress", function(event) {
			if (event.which == 13) {
				CalculateTaxAmount($(this));
				addGridRow($(this));
				setTotal($(this).closest(".grid"));
				return false;
			}
		});
		$(".newrow").find("input").live("change", function(event) {
			findAmount($(this));
		});
		//delete
		$(".delete-row").live("click", function(event) {
			var grid = $(this).closest(".grid");
			removeGridRow($(this));
			setTotal(grid);
		});
		//edit
		$(".gridtr").live("click", function(event) {
			editGridRow($(this));
		});
		$("#productname").live("blur", function(event){
		    CalculateTaxAmount(this);
		});
		
	});
	</script>
					<!--JSCODE_END-->

</asp:Content>
