 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="BulkWhatsAppMessage_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        if($(".jq-txtwhatsappvariables").length > 0)
        {
            var templatemsg = $(".jq-txtwhatsapptemplatemessage").val();
            var count = templatemsg.match(/{{/g).length;
            var html = "<table>";
            var arrval = $(".jq-txtwhatsappvariables").val().split('|');
            var startindex1 = 0; var endindex = 0;
            for(var i=0;i<count;i++)
            {
                var val = ""; var label="";
                if(arrval.length > i) val = arrval[i];
                startindex1 = templatemsg.indexOf("{{", startindex1);
                if(startindex1 > 0)
                {
                    var endindex = templatemsg.indexOf("}}", startindex1);
                    label = templatemsg.substring(startindex1+2,endindex);
                    startindex1 = endindex;
                }
                html+="<tr><td>"+label+"</td><td><input type='textbox' class='mbox jq-whatsappvariable' value='"+val+"'/></td></tr>";
            }
            html+="</table>";
            $(".jq-txtwhatsappvariables").hide();
            $(".jq-txtwhatsappvariables").parent().append(html);
        }
        $(".jq-whatsappvariable").live("blur",function(){
            var variables = "";
            $(".jq-whatsappvariable").each(function(){
                if(variables=="")
                {
                    variables=$(this).val();
                }
                else
                {
                    variables+="|"+$(this).val();
                }
                $(".jq-txtwhatsappvariables").val(variables);
            });    
        });
        $("form").submit(function() {
            var isvalid = true;
            $(".jq-whatsappvariable").each(function(){
                if(isvalid && $(this).val() == "")
                {
                    $(this).focus();
                    isvalid=false;
                }
            });
            if(isvalid)
            {
                var isgroupselected=false;
                $(".mchk-clientgroup_groupname").find("input[type=checkbox]").each(function(){
                    if($(this).is(":checked"))
                    {
                        isgroupselected = true;
                    }
                });
                if(!isgroupselected)
                {
                    isvalid = false;
                    alert("Please select groups.");
                }
            }
            if(!isvalid)
            {
                $(".save").val("Send");
            }
            return isvalid;
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
											<td class="label">Date <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtdate"  dcn="bulkwhatsappmessage_date" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Template Name</td>
											<td ti='1'><asp:DropDownList ID="ddlwhatsapptemplateid"  dcn="bulkwhatsappmessage_whatsapptemplateid"  OnSelectedIndexChanged="ddlwhatsapptemplate_changed" AutoPostback="true"  search='true' runat="server" m="whatsapptemplate" cn="templatename" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Client Groups</td>
											<td ti='2'><uc:MultiCheckbox ID="mcbulkwhatsappmessageclientgroups"  Module="clientgroup" Column="groupname" TargetModule="bulkwhatsappclientgroups"  runat="server" ></uc:MultiCheckbox></td>
										</tr>
										<tr>
											<td class="label">WhatsApp Template Message <span class="error">*</span></td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtwhatsapptemplatemessage"  Enabled="false" Height="200" Width="300"  dcn="bulkwhatsappmessage_whatsapptemplatemessage"  search='true'  runat="server" CssClass="textarea jq-txtwhatsapptemplatemessage "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required WhatsApp Template Message" ValidationGroup="vg" ControlToValidate="txtwhatsapptemplatemessage"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">WhatsApp Variables</td>
											<td ti='4'><asp:TextBox ID="txtwhatsappvariables"  Width="300"  MaxLength="100" runat="server"  dcn="bulkwhatsappmessage_whatsappvariables" CssClass="textbox jq-txtwhatsappvariables hidden "></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Total Messages</td>
											<td ti='0'><asp:TextBox ID="txttotalmessages"  dcn="bulkwhatsappmessage_totalmessages"  Enabled="false" runat="server" MaxLength="10" CssClass="mbox val-i jq-totalmessages " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Total Sent</td>
											<td ti='1'><asp:TextBox ID="txttotalsent"  dcn="bulkwhatsappmessage_totalsent"  Enabled="false" runat="server" MaxLength="10" CssClass="mbox val-i jq-totalmessagesent " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Total Failed</td>
											<td ti='2'><asp:TextBox ID="txttotalfailed"  dcn="bulkwhatsappmessage_totalfailed"  Enabled="false" runat="server" MaxLength="10" CssClass="mbox val-i jq-totalfailed " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='3'><asp:TextBox ID="emailsmsstatus"  Enabled="false"  search='true'  dcn="emailsmsstatus_status" MaxLength="100" runat="server" m="emailsmsstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemailsmsstatusid"  dcn="bulkwhatsappmessage_emailsmsstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
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
