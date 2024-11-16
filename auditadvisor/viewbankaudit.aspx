<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="viewbankaudit.aspx.cs" Inherits="auditadvisor_viewbankaudit"%>
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
                        <asp:PlaceHolder ID="plSearch" runat="server">
                        <table width="100%">
                            <tr>
                                <td>
                                    <div class='report-field' id='Div1' runat='server'>
						                <div class='report-label'>Client</div>
					                    <div class='report-control'>
					                        <asp:TextBox ID="customer" runat="server" CssClass="textbox ac" m="client" cn="client_customername"></asp:TextBox>
					                        <asp:TextBox ID="bankaudit_clientid" runat="server" CssClass="hdn hdnac" Text="0"></asp:TextBox>
					                    </div>
					                </div>
					                <div class='report-field' id='Div2' runat='server'>
						                <div class='report-label'>Bank</div>
					                    <div class='report-control'>
					                        <asp:TextBox ID="bankauditbank" runat="server" CssClass="textbox ac" m="bankauditbank" cn="bankauditbank_bankname"></asp:TextBox>
					                        <asp:TextBox ID="bankaudit_bankauditbankid" runat="server" CssClass="hdn hdnac" Text="0"></asp:TextBox>
					                    </div>
					                </div>
					                
					                <div class='report-field' id='reportfield_bankauditstatusid' runat='server'>
						                <div class='report-label'>Status</div>
					                <div class='report-control'><asp:DropDownList ID="bankaudit_bankauditstatusid" runat="server" CssClass="ddl"></asp:DropDownList></div>
					                </div>
					                <div class='report-field' id='reportfield_date' runat='server'>
						                <div class='report-label'>Date</div>
					                <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="bankaudit_date_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						                <div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="bankaudit_date_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					                </div>
					                <div class='report-field'><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
                                </td>
                            </tr>
                        </table>
                        </asp:PlaceHolder>
                    </td>
                 </tr>
                 <tr>
                    <td colspan="2">
                        <uc:Grid id="grid" runat="server" Module="advancedsearchbankaudit" EnableAddLink="false"/>
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