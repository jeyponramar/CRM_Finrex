 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Role_add" EnableEventValidation="false" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".chkmenu").click(function() {
            if ($(this).is(":checked")) {
                $(this).closest("tr").next().find("input").attr("checked", true);
            }
            else {
                $(this).closest("tr").next().find("input").removeAttr("checked");
            }
        });
//        $(".subtitle").click(function() {
//            if ($(this).closest("tr").next().css("display") == "none") {
//                $(this).closest("tr").next().show();
//            }
//            else {
//                $(this).closest("tr").next().show();
//            }
        //        });
        $(".subtitle").live("click", function() {
            var div = $(this).parent().next();
            if (div.css("display") == "none") {
                div.show("fast");
                $(this).css("background-image", "url(../images/up-arrow-white.png)");
            }
            else {
                $(this).css("background-image", "url(../images/down-arrow-white.png)");
                div.hide("fast");
            }
        });
        $(".chksubmenu").click(function() {
            $(this).closest(".trsubmenu").prev().find("input").removeAttr("checked");
        });
        $(".btnselectall").click(function() {
            if ($(this).val() == "Select All") {
                $(this).val("Deselect All");
                $(":checkbox").attr("checked", true);
            }
            else {
                $(this).val("Select All");
                $(":checkbox").removeAttr("checked");
            }
        });
        var id = ConvertToInt('<%=Request.QueryString["id"]%>');
        if (id > 0) {

            $(".visibleinedit").show();
        }
        else {
            $(".visibleinedit").hide();
        }
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
            <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center" colspan="4"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                <!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Role Name <span class="error">*</span></td>
											<td><asp:TextBox ID="txtrolename"  IsUnique="true" MaxLength="100" runat="server"  dcn="role_rolename" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Role Name" ValidationGroup="vg" ControlToValidate="txtrolename"></asp:RequiredFieldValidator></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					</table>
					<!--CONTROLS_END-->
                    <tr><td colspan='2' class='bold visibleinedit' align="center">Menu Rights</td></tr>
                    <tr><td colspan='2'><input type="button" class="button btnselectall visibleinedit" value="Select All"/></td></tr>
                    <tr><td colspan="2">
                        <asp:Literal ID="ltRights" runat="server"></asp:Literal>
                    </td></tr>
                    
                    <tr><td colspan='2' class='bold visibleinedit' align="center">Action Rights</td></tr>
                    <%--<tr><td colspan='2'><input type="button" class="button btnselectall-action visibleinedit" value="Select All"/></td></tr>--%>
                    <tr><td colspan="2">
                        <asp:Literal ID="ltActionRights" runat="server"></asp:Literal>
                    </td></tr>
                    
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" ValidationGroup="vg"/>
                <input type="button" class="close-page cancel" value="Cancel"/>
            </td>
        </tr>
        <!--FOLLOWUP_START-->
        <!--FOLLOWUP_END-->
        <!--COMMENTS_START-->
        <!--COMMENTS_END-->
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
