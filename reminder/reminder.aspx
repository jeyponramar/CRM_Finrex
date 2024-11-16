<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="reminder.aspx.cs" Inherits="reminder_reminder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Reminder"/>
        </td>
     </tr>
     <tr>
        <td>
            <asp:PlaceHolder ID="form" runat="server">
            <table width="100%">
            <tr id="trcustomerName" runat="server">
				<td class="label">Customer Name </td>
				<td>
				<asp:TextBox ID="client" Enabled="false" dcn="client_clientname" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="followups_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
				</td>
			</tr>
                <tr>
                    <td class="label">Subject</td>
                    <td class="val"><asp:Label ID="lblSubject" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="label">Date</td>
                    <td class="val"><asp:Label ID="lblDate" Format="DateTime" runat="server"></asp:Label>&nbsp;
                        <asp:Label ID="lblTime" runat="server"></asp:Label>&nbsp;
                        <asp:Label ID="lblAmPm" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">Remarks</td>
                    <td class="val"><asp:Label ID="lblRemarks" runat="server"></asp:Label></td>
                </tr>
            </table>
            </asp:PlaceHolder>
        </td>
     </tr>
     <tr>
        <td class="center">
            <asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove" CssClass="button"/>&nbsp;
            <asp:Button ID="btnGoto" runat="server" OnClick="btnGoto_Click" Text="Go to" CssClass="button"/>
        </td>
     </tr>
</table>     
</asp:Content>

