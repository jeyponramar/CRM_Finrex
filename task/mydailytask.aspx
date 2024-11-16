<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="mydailytask.aspx.cs" Inherits="task_mydailytask" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".dailytaskbox1").click(function() {
            if (selectedmodule == "") {
                var url = "../task/add.aspx?id=" + selectedTaskId;
                window.open(url);
            }
            else {
                if (ConvertToInt(selectedmid) > 0) {
                    var url = "../" + selectedmodule + "/add.aspx?id=" + selectedmid;
                    window.open(url);
                }
            }
            return false;
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" style="background-color:#efefef;" cellspacing="0" cellpadding="10">
     <tr>
        <td class="title" style="background-color:#fff;">
            <asp:Label ID="lblPageTitle" runat="server" Text="My Daily Task"/>
        </td>
     </tr>
     <tr>
        <td style="background-color:#fff;">
            <table>
                <tr>
                    <td>From Date</td>
                    <td><asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker textbox"></asp:TextBox></td>
                    <td>To Date</td>
                    <td><asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker textbox"></asp:TextBox></td>
                    <td>Status</td>
                    <td><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList></td>
                    <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnSubmit_Click" /></td>
                </tr>
            </table>
        </td>
     </tr>
     <tr>
        <td style="height:500px;vertical-align:top;"><asp:Literal ID="ltTask" runat="server"></asp:Literal></td>
     </tr>
</table>         
</asp:Content>

