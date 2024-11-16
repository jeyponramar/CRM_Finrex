<%@ Page Title="" Language="C#" MasterPageFile="~/CP/ConfigureMaster.master" AutoEventWireup="true" CodeFile="XMLBuilder.aspx.cs" Inherits="CP_XMLBuilder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
         <td class="label">Select Module</td><td><asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_Changed"></asp:DropDownList></td>
         <td><asp:CheckBox runat="server" ID="chkUnder" />IsViewXML</td>                  
    </tr>
    <tr>
        <td>
            <asp:TextBox runat="server" ID="txtXML" TextMode="MultiLine" Width="600" Height="400"></asp:TextBox>
        </td>                
    </tr>
    <tr>
        <td>
            <asp:Button runat="server" Text="Add XML Node" ID="btnAddXMLNode" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtNewNode"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            
        </td>
    </tr>
</table>
</asp:Content>

