<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="home_dashboard" %>
<%@ Register Src="~/usercontrols/dashboard.ascx" TagName="dashboard" TagPrefix="uc" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $(".repeater-row,.repeater-alt").find(".spage").removeClass("spage");
        $(".repeater-row,.repeater-alt").click(function() {
          
            var module = $(this).closest(".gridtd").attr("m");
            var moduleid = $(this).find(".idval").attr("idval");
            var Url = module + "/add.aspx?id=" + moduleid;
            loadPage(Url, "Edit " + module);
        });
    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<uc:dashboard ID="dashboard" runat="server" />
<table>
<tr>
    <td class="home-title tcorner">Graphical Summary</td>
</tr>
<tr>
    <td>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <iframe src="graph/orderstatus.aspx" width="280" height="260" style="border:0px" scrolling="no"></iframe>
                </td>
                <td>
                    <iframe src="graph/monthwiseorderummary.aspx" width="320" height="260" style="border:0px" scrolling="no"></iframe>
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr id="trminstock" runat="server">
  <td class="home-title ">Minimum Stock Report</td>
 </tr>
 <tr id="trminstockreport" runat="server"><td>
<uc:Grid ID="productminimumstock" runat="server" Module="productminimumquantity" />
</td></tr>
<tr>
  <td class="home-title ">New Order</td>
 </tr>
 <tr><td>
<uc:Grid ID="neworder" runat="server" Module="openorder" />
</td></tr>
<tr>
  <td class="home-title ">Pending Order</td>
 </tr>
 <tr><td>
<uc:Grid ID="pendingorder" runat="server" Module="pendingorder" />
</td></tr>
</table>
</asp:Content>

