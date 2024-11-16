<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="import.aspx.cs" Inherits="importdata_import" Title="Excel Import" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function(){
        $(".import").click(function(){
            if($(".deleteprevdata").find("input").is(":checked"))
            {
                if(!confirm("You will loss the previous data and can not roll back, Are you sure you want to continue?"))
                {
                    return false;
                }
            }
            return confirm("Are you sure you want to import data?");
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Excel Import"/>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2" align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        </td>
     </tr>
     <tr>
        <td colspan="2">
            <table width="100%" class="form">
                <tr>    
                    <td class="label">Module <span class="error">*</span></td>
                    <td>
                        <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlModule_Click"></asp:DropDownList>
                        <asp:Button ID="btnDownloadTemplate" runat="server" Text="Download Template" OnClick="btnDownloadTemplate_Click" CssClass="button" ValidationGroup="vg"/>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddlModule" InitialValue="0" ErrorMessage="Please select Module"
                        Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2"><asp:Literal ID="ltColumns" runat="server"></asp:Literal></td></tr>
                <tr>
                    <td>Select Excel File (.xls,.xlsx) <span class="error">*</span></td>
                    <td><asp:FileUpload ID="flExcel" runat="server" /></td>
                </tr>
                <tr id="tr" runat="server" visible="false">
                    <td></td>
                    <td><asp:CheckBox ID="chkDeletePrevData" runat="server" Text="Delete all previous data" CssClass="deleteprevdata"/></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table border="1" width="100%">
                            <tr>
                                <td class="repeater">Total Existing Data (Before Import data from ExcelData)</td>
                                <td class="repeater">Total ExcelData</td>
                                <td class="repeater">Total ImportedData</td>
                                <td class="repeater">Total Data (After Import Data From Excel Data)</td>
                            </tr>                            
                            <tr>
                                <asp:Literal ID="ltDetails" runat="server"></asp:Literal>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button import" ValidationGroup="vg"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Literal ID="ltImportStatus" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </td>
     </tr>
 <tr>
</table>         
</asp:Content>

