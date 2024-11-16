<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="populate-on-add.aspx.cs" Inherits="CP_populate_on_add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Populate on Add"/>
        </td>
     </tr>
     <tr>
        <td align="center"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td>
    </tr>
     <tr>
        <td>
            <table>
                <tr>
                    <td class="label">Query String Column</td>
                    <td><asp:DropDownList ID="ddlQueryStringColumn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQueryStringColumn_Changed"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="label">Query String Column <span class="error">*</span></td>
                    <td><asp:TextBox ID="txtQueryStringColumn" runat="server" CssClass="textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="vg" ID="rfv1" runat="server" ControlToValidate="txtQueryStringColumn" ErrorMessage="Required Query String Column" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="label">Join Modules Comma Sep (eg, client,amc)</td>
                    <td><asp:TextBox ID="txtJoinModules" runat="server" CssClass="textbox"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="GO" CssClass="button" /></td>
                </tr>
                <tr>
                    <td class="label">Select Query <span class="error">*</span></td>
                    <td><asp:TextBox ID="txtQuery" runat="server" CssClass="textarea" TextMode="MultiLine" Width="600" Height="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="vg" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtQuery" ErrorMessage="Required Query" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnSubmit" ValidationGroup="vg" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" /></td>
                </tr>
            </table>
        </td>
     </tr>
</table>         
</asp:Content>

