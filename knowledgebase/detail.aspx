<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="detail.aspx.cs" Inherits="knowledgebase_detail" Title="Knowledge Base Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table width="100%" id="form" runat="server">
            <tr>
                <td class="title">
                    <asp:Label ID="lblPageTitle" runat="server"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnEdit" Text="Edit" runat="server" OnClick="btnEdit_Click" CssClass="button" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblcreateddate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight:bold;color:#1177ea;font-size:18px;">
                    <asp:Label ID="lbltitle" runat="server"></asp:Label>
                </td>
            </tr>
           
            <%--<tr>
                <td style="font-weight:bold;color:#1177ea;">
                    <asp:Label ID="lblsubject" runat="server"></asp:Label>
                    </td>
            </tr>--%>
            <tr>
                <td style="text-align:justify;">
                    <asp:Label ID="lbldescription" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltfiles" runat="server"></asp:Literal>
                </td>
            </tr>
       </table>
</asp:Content>

