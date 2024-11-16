 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="CallLog_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        //SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".edit").hide();
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
										<tr ID="trmobileno" runat="server" Visible="false">
											<td class="label">Mobile No</td>
											<td ti='0'><asp:TextBox ID="txtmobileno"  search='true'  MaxLength="0" runat="server"  dcn="calllog_mobileno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="tremailid" runat="server" Visible="false">
											<td class="label">Email Id</td>
											<td ti='1'><asp:TextBox ID="txtemailid"  search='true'  MaxLength="0" runat="server"  dcn="calllog_emailid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trccemailid" runat="server" Visible="false">
											<td class="label">CC Email Id</td>
											<td ti='2'><asp:TextBox ID="txtccemailid"  MaxLength="0" runat="server"  dcn="calllog_ccemailid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trbccemailid" runat="server" Visible="false">
											<td class="label">BCC Email Id</td>
											<td ti='3'><asp:TextBox ID="txtbccemailid"  MaxLength="0" runat="server"  dcn="calllog_bccemailid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trsubscriptionid" runat="server" Visible="false">
											<td class="label">Subscription</td>
											<td ti='4'><asp:TextBox ID="subscription"  Enabled="false"  search='true'  dcn="subscription_subscriptioncode" MaxLength="100" runat="server" m="subscription" cn="subscriptioncode" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtsubscriptionid"  dcn="calllog_subscriptionid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr ID="trbulksmstemplateid" runat="server" Visible="false">
											<td class="label">SMS Template</td>
											<td ti='5'><asp:DropDownList ID="ddlbulksmstemplateid"  dcn="calllog_bulksmstemplateid"  OnSelectedIndexChanged="ddlsmstemplate_changed" AutoPostback="true" runat="server" m="bulksmstemplate" cn="templatename" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr ID="trbulkemailtemplateid" runat="server" Visible="false">
											<td class="label">Email Template</td>
											<td ti='6'><asp:DropDownList ID="ddlbulkemailtemplateid"  dcn="calllog_bulkemailtemplateid"  OnSelectedIndexChanged="ddlemailtemplate_changed" AutoPostback="true" runat="server" m="bulkemailtemplate" cn="templatename" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr id="trwhatsapptemplate" runat="server" visible="false">
											<td class="label">WhatsApp Template</td>
											<td ti='7'><asp:DropDownList ID="ddlwhatsapptemplateid"  dcn="calllog_whatsapptemplate"  OnSelectedIndexChanged="ddlwhatsapptemplate_changed" AutoPostback="true" runat="server" m="whatsapptemplate" cn="" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr ID="trsubject" runat="server" Visible="false">
											<td class="label">Subject</td>
											<td ti='8'><asp:TextBox ID="txtsubject"  Width="400"  MaxLength="400" runat="server"  dcn="calllog_subject" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trmessage" runat="server" Visible="false">
											<td class="label">Message</td>
											<td ti='9'><asp:TextBox TextMode="MultiLine" ID="txtmessage"  dcn="calllog_message"  runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr ID="trwhatsapptemplatemessage" runat="server" Visible="false">
											<td class="label">WhatsApp Template Message</td>
											<td ti='10'><asp:TextBox TextMode="MultiLine" ID="txtwhatsapptemplatemessage"  Enabled="false" Height="200" Width="300"  dcn="calllog_whatsapptemplatemessage" ml="300" runat="server" CssClass="textarea jq-txtwhatsapptemplatemessage "></asp:TextBox></td>
										</tr>
										<tr ID="trwhatsappvariables" runat="server" Visible="false">
											<td class="label">WhatsApp Variables</td>
											<td ti='11'><asp:TextBox ID="txtwhatsappvariables"  Width="300"  MaxLength="100" runat="server"  dcn="calllog_whatsappvariables" CssClass="textbox jq-txtwhatsappvariables "></asp:TextBox></td>
										</tr>
										<tr ID="trtrialid" runat="server" Visible="false">
											<td class="label">Trial</td>
											<td ti='12'><asp:TextBox ID="trial"  search='true'  dcn="trial_trialcode" MaxLength="100" runat="server" m="trial" cn="trialcode" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txttrialid"  dcn="calllog_trialid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Client <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="client"  Enabled="false"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac pop "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="calllog_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Client" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Notification Type</td>
											<td ti='1'><asp:TextBox ID="notificationtype"  Enabled="false"  search='true'  dcn="notificationtype_notificationtype" MaxLength="100" runat="server" m="notificationtype" cn="notificationtype" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtnotificationtypeid"  dcn="calllog_notificationtypeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Sent By <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="employee"  Enabled="false"  search='true'  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="calllog_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv14" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Sent By" ValidationGroup="vg" ControlToValidate="employee"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Sent Date</td>
											<td ti='3'><asp:TextBox ID="txtsentdate"  dcn="calllog_sentdate"  Enabled="false" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='4'><asp:TextBox ID="emailsmssentstatus"  Enabled="false"  search='true'  dcn="emailsmssentstatus_status" MaxLength="100" runat="server" m="emailsmssentstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemailsmssentstatusid"  dcn="calllog_emailsmssentstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr ID="trattachment" runat="server" a="b">
											<td class="label">Attachment</td>
											<td ti='5'><uc:MultiFileUpload ID="mfuattachment"  IsMultiple="true" FileType="Any" FolderPath="upload/calllog" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
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
        <tr>
            <td>
                <asp:Button ID="btnGetJSON" runat="server" Text="Get API Call" OnClick="btnGetJSON_Click" />
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
