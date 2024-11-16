 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="LiveRateSection_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $("#rtdcode").live("blur", function() {
            var code = $(this).val();
            if (code.indexOf("tickerplantrtdserver") > 0) {
                code = code.replace('=RTD("tickerplantrtdserver",,"', "").replace('")', "");
                $(this).val(code);
            }
            else if (code.indexOf("GetQuote") > 0) {
                var arr = code.split('"');
                code = arr[1] + '_' + arr[5];
                $(this).val(code);
            }
        });
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
				
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtname"  IsUnique="true"  search='true'  MaxLength="100" runat="server"  dcn="liveratesection_name" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Name" ValidationGroup="vg" ControlToValidate="txtname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Code</td>
											<td ti='1'><asp:TextBox ID="txtcode"  MaxLength="50" runat="server"  dcn="liveratesection_code" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Rows</td>
											<td ti='2'><asp:TextBox ID="txtrows"  MaxLength="200" runat="server"  dcn="liveratesection_rows" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Columns</td>
											<td ti='3'><asp:TextBox ID="txtcolumns"  MaxLength="200" runat="server"  dcn="liveratesection_columns" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Description</td>
											<td ti='4'><asp:TextBox ID="txtdescription"  MaxLength="100" runat="server"  dcn="liveratesection_description" CssClass="textbox"></asp:TextBox></td>
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
					</tr><tr><td>&nbsp;</td></tr><tr runat="server" ID="trliverate" ><td class='subgridbox'><table width='100%' cellspacing='0' class='tblform' tblindex="2"><tr><td class='subtitle'>Live Rate</td></tr><tr><td><table cellpadding='3' cellspacing='0' class='grid'>
							<input type='hidden' class='g_setting' value='hc~rtdcode,row,column,calculation,istick,issavehistory~dc~rtdcode,row,column,calculation,istick,issavehistory~smprefix~@sm1_~jt~~m~liverate' id='liverate_setting' runat='server'/>
							<input type='hidden' class='g_param' value='' id='liverate_param' runat='server'/>
							<tr class='srepeater-header'>
								<td cn="rtdcode">RTD Code</td><td cn="row">Row</td><td cn="column">Column</td>
								<td cn="calculation">Calculation</td><td cn="istick">Is Tick</td><td cn="issavehistory">Is Save History</td><td></td></tr>
							<asp:Literal ID="ltliverate" runat="server"></asp:Literal>
							<tr class='newrow'>
								<td><input type="hidden" id="id" class="id"/><input type="text" id="rtdcode" class="textbox first" maxlength='100'  ti='0'/></td>
								<td><input type="text" id="row" class="sbox val-i right" maxlength='10'  ti='1'/></td>
								<td><input type="text" id="column" class="sbox val-i right" maxlength='10'  ti='2'/></td>
								<td><input type="text" id="calculation" class="textbox" maxlength='500' Width="300" ti='3'/></td>
								<td><input type="text" id="istick" class="sbox val-i right" maxlength='10'  ti='4'/></td>
								<td><input type="text" id="issavehistory" class="sbox val-i right" maxlength='10'  ti='5'/></td>
						    </tr>
							<tr class='end'><td></td></tr>
						</table></td></tr>
					</table></td></tr><tr><td>&nbsp;</td></tr>
					</table>
					
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center" colspan="2">
			
					<asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="save button" ValidationGroup="vg"/>
				<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" Visible="false"/>
				<input type="button" class="close-page cancel" value="Cancel"/>
				<asp:Button ID="btnSubmitAndView" runat="server" Visible="false"/>
				
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
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
	    CalculateTaxAmount($(this));
	});
	
});
</script>

</asp:Content>
