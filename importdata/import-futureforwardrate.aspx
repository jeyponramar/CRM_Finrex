<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
CodeFile="import-futureforwardrate.aspx.cs" Inherits="importdata_import_futureforwardrate" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    _enabledFileUpload = false;
    $(document).ready(function(){
        $(".import").click(function(){
            return confirm("Are you sure you want to import data?");
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Data Import - Future Forward Rate"/>
        </td>
     </tr>
     <tr>
        <td colspan="2" align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        </td>
     </tr>
     <tr>
        <td colspan="2">
            <table width="100%" class="form" cellpadding="5">
                <tr>
                    <td>As on date <span class="error">*</span></td>
                    <td>
                        <asp:TextBox ID="txtdate" runat="server" CssClass="datepicker cdate" ValidationGroup="vg"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtdate" ErrorMessage="Required" Display="Static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Select Excel File (.xlsx) <span class="error">*</span></td>
                    <td><asp:FileUpload CssClass="fileupload" ID="flExcel" runat="server" />&nbsp;
                    <asp:HyperLink ID="lnkDownloadTemplate" runat="server" Text="Download Template" Target="_blank"/></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnImportData" runat="server" OnClick="btnImportData_Click" Text="Import Data" 
                         CssClass="button import" ValidationGroup="vg"/>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                
            </table>
        </td>
     </tr>
     
</table> 
</asp:Content>

