<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="bulkreassigned.aspx.cs" Inherits="bulk_reassigned" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $(".allocateassigneddata").click(function() {
            var TotalAssigned = 0;
            var TotalNewAssigned = "";
            var IsValidData = true;
            $(".chkreassigndata").each(function() {
                if ($(this).is(':checked')) {
                    isValidAllocation = true;
                    var statusname = $(this).attr("statusname");
                    TotalAssigned = ConvertToInt($(this).attr("totalstatuscount"));
                    TotalNewAssigned = ConvertToInt($(".txt_" + statusname).val());
                    if (TotalNewAssigned > TotalAssigned) {
                        IsValidData = false;
                    }
                }
            });
            if (!IsValidData) {
                alert("Total new allocation data should not greater than the Current allocation Count");
            }
            else if (TotalNewAssigned == 0) {
                alert("Allocation count should be greater than zero");
                return false;
            }
            return IsValidData;
        });
        $(".chkreassigndata").click(function() {
            var statusname = $(this).attr("statusname");
            if ($(this).is(':checked')) {
                TotalAssigned = ConvertToInt($(this).attr("totalstatuscount"));
                TotalNewAssigned = ConvertToInt($(".txt_" + statusname).val());
                $(".txt_" + statusname).removeAttr("disabled");
            }
            else {
                $(".txt_" + statusname).attr("disabled", "disabled");
                $(".txt_" + statusname).val(0);

            }
        });
        $(".getopendata").click(function() {
            if (ConvertToInt($(".frompersonid").val()) == ConvertToInt($(".topersonid").val())) {
                alert("From Person and To Person should be different");
                return false;
            }
        });
        $(".allocateassigneddata").click(function() {
            var isValidAllocation = false;
            $(".chkreassigndata").each(function() {
                if ($(this).is(':checked')) {
                    isValidAllocation = true;
                }
            });
            if (!isValidAllocation) {
                alert("Please select any one status to allocate data");
                return false;
            }
        });

    });
</script>
 <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" Text="Bulk Reassigned" runat="server"/>
                <asp:TextBox CssClass="hidden TotalOpenCall" Text="0" runat="server" ID="hdn_TotalOpenCall"></asp:TextBox>
                <asp:TextBox CssClass="hidden hdn_TeleCallerId" Text="0" runat="server" ID="hdn_TeleCallerId"></asp:TextBox>
            </td>
         </tr>
         <tr>
            <td align="center" class="error"><asp:Label runat="server" ID="lblmessage" runat="server"></asp:Label> </td>
         </tr>
         <tr>
            <td align="center" class="error"><asp:Label Font-Size="19px" Font-Bold="true" CssClass="error" runat="server" ID="lblWarn" Text="You Can't RollBack Once the data is Re Allocated" runat="server"></asp:Label> </td>
         </tr>
         <tr>
            <td class="form">
                <table>
                    <tr>
                        <td>
                            <table width="500">
                                <tr>
                                    <td class="label">From <span class="error" >*</span></td>
									<td><asp:TextBox ID="Fromperson"  MaxLength="100" runat="server" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtfrmpersonid" Text="0" runat="server" class="frompersonid hdnac "/><img src="../images/down-arrow.png" class="epage"/>
									<asp:RequiredFieldValidator runat="server" ValidationGroup="vg" ControlToValidate="Fromperson" ErrorMessage="Please select from value" SetFocusOnError="true" ></asp:RequiredFieldValidator>
									</td>                                   
									<td class="label">To <span class="error" >*</span></td>
									<td><asp:TextBox ID="ToPerson"  MaxLength="100" runat="server" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txttopersonid" Text="0" runat="server" class="topersonid hdnac "/><img src="../images/down-arrow.png" class="epage"/>
									<asp:RequiredFieldValidator runat="server" ValidationGroup="vg" ControlToValidate="ToPerson" ErrorMessage="Please select To value" SetFocusOnError="true" ></asp:RequiredFieldValidator>
									</td>  									                          
		                        </tr>  	
		                        <tr>
		                            <td colspan="4">
		                                <table>
		                                    <tr>
		                                         <td>From Date </td><td> <asp:TextBox ID="from_date" CssClass="datepicker from" runat="server" Format="Date"/></td>
						                        <td> To Date </td> <td><asp:TextBox ID="to_date" CssClass="datepicker to" runat="server" Format="Date"/></td> 
		                                    </tr>
		                                </table>
		                            </td>
		                            
		                        </tr>	                        		                       
		                       
                                 <tr>
                                    <td colspan="2" align="center"><asp:Button runat="server" ValidationGroup="vg" ID="btnGetAllAssignedData" Text="Go" CssClass="button getopendata" OnClick="btnGetAllAssignedData_Click" /> </td>
                                 </tr> 
                            </table>
                        </td>
                    </tr>                                          
                    <tr>
                        <td align="center" runat="server" id="tdReAssignedData" visible="false">
                            <table width="900" border="1" cellpadding="1" cellspacing="1">
                                <tr><td colspan="2" align="center" style="font-size:19px"><b>Transfer Allocation</b></td></tr>
                                <tr>
                                    <asp:Literal runat="server" Visible="false" ID="ltReAssignedData"></asp:Literal>
                                </tr>
                                
                            </table>                
                        </td>
                    </tr>    
                </table>
            </td> 
         </tr>         
        <tr>
            <td align="center">
                <asp:Button ID="btnAllocate" Text="Allocate Data" OnClick="btnAllocateTask_Click" ValidationGroup="vg" msg="Are you sure want to transfer Data?"  runat="server" CssClass="button btngreen confirmstatus allocateassigneddata"></asp:Button>&nbsp;
            </td>
        </tr>                             
    </table>
</asp:PlaceHolder>
</asp:Content>



