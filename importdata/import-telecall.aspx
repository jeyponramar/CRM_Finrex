<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="import-telecall.aspx.cs" Inherits="importdata_importtelecall" Title="Excel Import" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
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
            <asp:Label ID="lblPageTitle" runat="server" Text="Telecalling Data Import"/>
        </td>
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
                    <td>Select Excel File (.xls,.xlsx) <span class="error">*</span></td>
                    <td><asp:FileUpload CssClass="fileupload" ID="flExcel" runat="server" />&nbsp;
                    <asp:HyperLink ID="lnkDownloadTemplate" runat="server" Text="Download Template" Target="_blank"/></td>
                </tr>
                <tr><td>&nbsp;</td><td><asp:CheckBox ID="chkIsUpdate" Text="Update Existing Data" runat="server" /></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnImportData" runat="server" OnClick="btnImportData_Click" Text="Import Data" CssClass="button import" ValidationGroup="vg"/>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr id="trResult" runat="server" visible="false"><td colspan="2">
                    <table width="100%">
                        <tr>
                            <td width="120">Total Data</td>
                            <td><asp:Label ID="lblTotalData" runat="server" CssClass="bold"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Valid Datas</td>
                            <td><asp:Label ID="lblValiddatas" runat="server" CssClass="bold"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Invalid Datas</td>
                            <td><asp:Label ID="lblInvaliddatas" runat="server" CssClass="bold"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Data Imported</td>
                            <td><asp:Label ID="lblDataImported" runat="server" CssClass="bold"></asp:Label></td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr><td colspan="2" class="error">Invalid Datas : </td></tr>
                        <tr>
                            <td colspan="2">
                                <asp:Literal ID="ltResult" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                    
                </td></tr>
            </table>
        </td>
     </tr>
     
</table>         
</asp:Content>

