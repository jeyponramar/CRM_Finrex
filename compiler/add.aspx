 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Compiler_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"> 
<script>
    $(document).ready(function() {
        //SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".fixerror").click(function() {
            var status = ($(this).html());
            if (status.indexOf("Success") >= 0 || status.indexOf("Error") >= 0) {
                return false;
            }
            var val = $(this).closest('tr').text();
            var ColVar = $(this).attr("name");
            var tr = $(this).closest('tr');
            var ColName = tr.find('textarea').val();
            if (ColName == "") {
                alert("Please enter column name");
                tr.find('textarea').focus();
                return false;
            }
            //$(this).closest('tr').css('text-color', 'White').css('background-color', 'Green');
            //$(this).closest('tr').addCss('fixed_error');
            $('.fixed_error td').css({ color: '#789090' });
            var URL = "../" + $(this).attr("url");
            URL += "&ColName=" + ColName;
            //alert(URL);
            //$(this).removeCss();
            //var URL = "../utilities.ashx?mm=compiler";
            $(this).removeAttr("url");
            var isAsc = true;
            var param = $(this);
            param.html("Processing.........");
            $.ajax({
                url: URL,
                type: 'GET',
                async: isAsc,
                success: function(jsonObj) {
                    if ((jsonObj + "").indexOf("session expired") > 0) {
                        window.location = "../login.aspx";
                        return;
                    }
                    else if ((jsonObj + "").indexOf("Error") >= 0) {
                        //alert("Error : \n\n" + jsonObj);
                        jsonObj = "Error occurred!";
                        param.html("Error occurred!");
                        param.css('background-color', 'Red').css('color', 'White');
                    }
                    else {
                        param.html("Success!!!");
                        param.css('background-color', 'Green').css('color', 'White');
                    }
                }
            });

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
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr runat="server" id="trModule">
											<td class="label">Module Name <span class="error">*</span></td>
											<td><asp:DropDownList ID="ddlimportdatamoduleid"  dcn="importdata_importdatamoduleid" runat="server" m="importdatamodule" cn="modulename" CssClass="ddl"></asp:DropDownList>
						                    <asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Module Name" ValidationGroup="vg" ControlToValidate="ddlimportdatamoduleid"  InitialValue="0"></asp:RequiredFieldValidator></td>
						                    <td>&nbsp;</td>
						                    <td><asp:Button runat="server" Visible="false" ID="btn_DeleteModule" OnClick="btnDeleteModule_Click" CssClass="redbutton button" Text="Delete Module" /></td>
						                    <td>Last Update Date <span class="error">*</span> </td>
						                    <td>
						                        <asp:TextBox ID="txtLastUpdateDate"  dcn="invoice_invoicedate" runat="server" MaxLength="10" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						                        <asp:RequiredFieldValidator runat="server" ValidationGroup="vgdbscript" ControlToValidate="txtLastUpdateDate" ErrorMessage="Enter Last Updated Date" ></asp:RequiredFieldValidator>
						                    </td>
										</tr>										
										</table>
									</td>									
								</tr>
							</table>
						</td>
					<!--CONTROLS_END-->
                </table>
            </td>   
         </tr>
		 <tr>
            <td align="center" colspan="2">
				<!--SAVEBUTTON_START-->
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Compile Module" CssClass="save btngreen" ValidationGroup="vg"/>
                <asp:Button ID="Button1" runat="server" OnClick="btnSubmit_Click" Text="Compile All Module" errormsg="Are you sure you want to compile all the modules?" CssClass="save btngreen confirm"/>
                <asp:Button ID="Button2" runat="server" OnClick="btnSubmit_Click" Text="Get Upgradable Objects" CssClass="save btngreen" ValidationGroup="vgdbscript" />
                <asp:Button ID="Button3" runat="server" OnClick="btnSubmit_Click" Text="Upgrade DataBase" CssClass="save btngreen"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Literal ID="ltWarnings" runat="server" ></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                  <table class="repeater" border="1" cellspacing="0">
                    <tr class="repeater-header">
                        <td>Module</td><td>Message</td><td>Debugging Status</td><td>Suggestion</td><td>Status</td>
                    </tr>
                    <asp:Literal ID="ltCompilerStatus" runat="server"></asp:Literal>
                </table>
                
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                  <table class="repeater" border="1" cellspacing="0">
                    <tr class="repeater-header">
                        <td>OBJECT_NAME</td><td>CREATED_DATE</td><td>LAST_MODIFY_DATE</td><td>Suggestion</td><td>Status</td><td>OBJ_Definition</td>
                    </tr>
                    <asp:Literal ID="ltDbData" runat="server"></asp:Literal>
                </table>
                
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
