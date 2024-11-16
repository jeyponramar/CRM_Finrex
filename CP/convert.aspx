<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="convert.aspx.cs" Inherits="CP_convert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".help").click(function() {
            if ($(".trhelp").css("display") == "none") {
                $(".trhelp").show();
            }
            else {
                $(".trhelp").hide();
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr>
        <td class="title"><asp:Label ID="lblTitle" Text="Convert Module" runat="server"></asp:Label></td>
    </tr>
    <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
    <tr><td><img src="../images/help.png" title="Help" class="help hand"/></td></tr>
    <tr class="trhelp hidden">
    <td style="background-color:#ffffff;padding:10px;color:#000000">
        <table border="1" cellspacing="0" cellpadding="5">
            <tr><td colspan="2">Value can be a column name or text or dbo.GetDate() or Code=Q- or Session or Query String value</td></tr>
            <tr>
                <td>For Code</td>
                <td>Code=Q-</td>
            </tr>
            <tr>
                <td>For Custum Session</td>
                <td>CustomSession("UserId")</td>
            </tr>
            <tr>
                <td>For QueryString</td>
                <td>QueryString("id")</td>
            </tr>
        </table>
    </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td class="label">Name</td>
                    <td class="val"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="label">From Module</td>
                    <td class="val"><asp:Label ID="lblModuleName" runat="server"></asp:Label></td>
                    <td class="label">To Module</td>
                    <td class="val"><asp:DropDownList ID="ddlToModule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlToModule_Changed"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="label">From Child Module</td>
                    <td class="val"><asp:DropDownList ID="ddlFromChildModule" runat="server"></asp:DropDownList></td>
                    <td class="label">To Child Module</td>
                    <td class="val"><asp:DropDownList ID="ddlToChildModule" runat="server"></asp:DropDownList></td>
                </tr>
                <tr><td></td>
                    <td><asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" CssClass="button"/></td>
                </tr>
                <tr>
                    <td colspan="2" class="label">Column Mapping</td>
                    <td colspan="2" class="label">Child Column Mapping</td>
               </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <asp:Literal ID="ltColumnMapping" runat="server"></asp:Literal>
                    </td>
                    <td colspan="2" valign="top">
                        <asp:Literal ID="ltChildColumnMapping" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>Status Column</td>
                    <td><asp:TextBox ID="txtStatusColumn" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Status Value</td>
                    <td><asp:TextBox ID="txtStatusValue" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" CssClass="button"/>
                    </td>
                </tr>    
            </table>
        </td>
    </tr>
</table>        
</asp:Content>

