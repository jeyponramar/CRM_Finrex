<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="viewcustomerquery.aspx.cs" Inherits="viewcustomerquery" %>
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
                        <table width="100%">
                            <tr>
                                <td>
                                    <div class='report-field' id='reportfield_metalid' runat='server'>
						                <div class='report-label'>Query Topic</div>
					                <div class='report-control'><asp:DropDownList ID="ddlquerytopicid" runat="server" CssClass="ddl"></asp:DropDownList></div>
					                </div>
					                <div class='report-field' id='Div1' runat='server'>
						                <div class='report-label'>Status</div>
					                <div class='report-control'><asp:DropDownList ID="ddlstatusid" runat="server" CssClass="ddl"></asp:DropDownList></div>
					                </div>
					                <div class='report-field' id='reportfield_date' runat='server'>
						                <div class='report-label'>Date</div>
					                <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="query_date_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						                <div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="query_date_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					                </div>
					                <div class='report-field'><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                 </tr>
                 <tr>
                    <td colspan="2">
                        <uc:Grid id="grid" runat="server" Module="advancedreportcustomerquery" EnableAddLink="false"/>
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