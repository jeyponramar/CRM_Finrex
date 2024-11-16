<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="ledgeraccount.aspx.cs" Inherits="ledgeraccountreport" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%" cellpadding="0" cellspacing="5">
    <tr>
        <td width="80%" style="vertical-align:top" class="tddatapanel">
            <table width="100%">
                <tr>
                    <td class="title">
                        <asp:Label ID="lblPageTitle" runat="server"/>
                    </td>
                 </tr>
                 <tr>
                    <td>
                        <table>
                            <tr>
                                <td><asp:Label ID="lblLedgerType" runat="server"></asp:Label> <span class="error">*</span>
                                </td>
                                <td><asp:TextBox ID="ledger"  dcn="ledger_ledgername" MaxLength="100" runat="server" m="ledger" cn="ledgername" CssClass="textbox ac txtac"></asp:TextBox>
                                <asp:TextBox id="txtledgerid"  dcn="ledgervoucher_ledgerid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ledger" ValidationGroup="vg" ErrorMessage="Required Ledger" Display="Dynamic"></asp:RequiredFieldValidator>
                                <td>Voucher Date From</td>
                                <td><asp:TextBox ID="txtVoucherDate_From" runat="server" CssClass="datepicker"></asp:TextBox></td>
                                <td>To</td>
                                <td><asp:TextBox ID="txtVoucherDate_To" runat="server" CssClass="datepicker"></asp:TextBox></td>
                                <td><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" ValidationGroup="vg" OnClick="btnReport_Click"/></td>
                            </tr>
                        </table>
                    </td>
                 </tr>
                 <tr>
                    <td align="right"><asp:Label ID="lblOpeningBal" runat="server"></asp:Label></td>
                 </tr>   
                 <tr>
                    <td>
                        <uc:Grid id="grid" runat="server" />
                    </td>
                 </tr>
            </table>
        </td>
        <td width="20%" class="tdrightpanel hidden valign">
            <table width="100%">
                <tr><td align="right"><img class="quickedit-close hand" title="Close this panel" src="../images/close.png" /></td></tr>
                <tr><td class="tdrightpanel-inner">
                    <iframe src="" id="ifrRightPanel" class="summary-right-panel-iframe"></iframe>
                </td></tr>
            </table>
        </td>
    </tr>
     
</table>
<!--DESIGN_END-->
</asp:Content>