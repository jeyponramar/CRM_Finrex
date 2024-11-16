<%@ Page Title="" Language="C#" MasterPageFile="~/exe/ExeMasterPage.master" AutoEventWireup="true" CodeFile="setliveratealert.aspx.cs" 
Inherits="exe_setliveratealert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".save").click(function() {
            var target = ConvertToDouble($(".txttarget").val());
            var stoploss = ConvertToDouble($(".txtstoploss").val());
            if (target == 0 && stoploss == 0) {
                alert("Please enter target or stop loss!");
                return false;
            }
            var emailids = ""; var mobilenos = "";
            $(".tblemailids").find(".chktwoselect").each(function() {
                if ($(this).is(":checked")) {
                    if (emailids == "") {
                        emailids = $(this).val();
                    }
                    else {
                        emailids += "," + $(this).val();
                    }
                }
            });
            $(".tblmobilenos").find(".chktwoselect").each(function() {
                if ($(this).is(":checked")) {
                    if (mobilenos == "") {
                        mobilenos = $(this).val();
                    }
                    else {
                        mobilenos += "," + $(this).val();
                    }
                }
            });
            $(".txtemailid").val(emailids);
            $(".txtmobileno").val(mobilenos);
        });
        $(".chktwoselect").click(function() {
            var count = 0;
            var emailids = "";
            $(this).closest("table").find(".chktwoselect").each(function() {
                if ($(this).is(":checked")) {
                    count++;
                }
            });
            if (count > 2) {
                alert("You can not select more than one contact!");
                $(this).removeAttr("checked");
                return false;
            }
        });
        $("#lnkviewalerts").click(function() {
            window.parent.location = "viewalerts.aspx";
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">
                    <table width="100%">
                        <tr>
                            <td width="30"><asp:HyperLink ID="lnkback" runat="server"><i class='icon ion-android-arrow-back jq-page-back'></i></asp:HyperLink></td>
                            <td>Set Alert</td>
                        </tr>
                    </table>
                
                </td>
            </tr>
        </table>
        </td>
     </tr>
</table>
<asp:PlaceHolder ID="form" runat="server">
    <div style="width:100%;max-height:400px;overflow-y:scroll;">
    <table width="100%">
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <%--<tr><td align="right"><a href="#" id="lnkviewalerts" style="color:#fff">View all alerts</a></td></tr>--%>
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
				<!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='100%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Currency <span class="error">*</span></td>
											<td ti='1'><asp:DropDownList ID="ddlcurrencymasterid"  dcn="liveratealert_currencymasterid"  search='true' runat="server" m="currencymaster" cn="currency" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Currency" ValidationGroup="vg" ControlToValidate="ddlcurrencymasterid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Cover Type <span class="error">*</span></td>
											<td ti='2'><asp:DropDownList ID="ddlcovertypeid"  dcn="liveratealert_covertypeid"  search='true' runat="server" m="covertype" cn="covertype" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Cover Type" ValidationGroup="vg" ControlToValidate="ddlcovertypeid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Target</td>
											<td ti='3'><asp:TextBox ID="txttarget"  dcn="liveratealert_target" runat="server" MaxLength="15" CssClass="mbox val-dbl txttarget" Text=""></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Stop Loss</td>
											<td ti='4'><asp:TextBox ID="txtstoploss"  dcn="liveratealert_stoploss" runat="server" MaxLength="15" CssClass="mbox val-dbl txtstoploss" Text=""></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Expiry Date <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="txtexpirydate"  dcn="liveratealert_expirydate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Expiry Date" ValidationGroup="vg" ControlToValidate="txtexpirydate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label" colspan="2">Email Id</td>
										</tr>
										<tr>	
											<td ti='6' colspan="2">
											    <asp:Literal ID="ltemailids" runat="server"></asp:Literal>
											    <asp:TextBox ID="txtemailid"  dcn="liveratealert_emailid"  search='true' MaxLength="300" runat="server" CssClass="txtemailid hdn"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td class="label" colspan="2">Mobile No</td>
										</tr>
										<tr>
											<td ti='7' colspan="2">
											<asp:Literal ID="ltmobilenos" runat="server"></asp:Literal>
											<asp:TextBox ID="txtmobileno"  search='true'  MaxLength="300" runat="server"  dcn="liveratealert_mobileno" CssClass="txtmobileno hdn"></asp:TextBox></td>
										</tr>
										 <tr>
										    <td></td>
                                            <td>
				                                <!--SAVEBUTTON_START-->
					                                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="button" ValidationGroup="vg"/>
					                                <!--SAVEBUTTON_END-->
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
		

    </table>
    </div>
</asp:PlaceHolder>
</asp:Content>

