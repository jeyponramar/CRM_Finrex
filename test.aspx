<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
     <%--<tr>      <td class="label">FinDoc Department</td>      <td ti='0'>      <asp:TextBox ID="findocdepartment" dcn="findocdepartment_departmentname" MaxLength="100" runat="server" m="findocdepartment" cn="departmentname"       CssClass="textbox ac txtac"></asp:TextBox>      <asp:TextBox id="txtfindocdepartmentid"  dcn="findocdocument_findocdepartmentid"  Text="0" runat="server" class=" hdnac"/>      <img src="../images/down-arrow.png" class="quick-new epage"/></td>    </tr>--%>
    
    <tr>
        <td>FinDoc Department</td>
        <td>
            <asp:TextBox ID="findocdepartment" MaxLength="100" runat="server" m="findocdepartment" cn="departmentname"                 CssClass="textbox ac txtac"></asp:TextBox>
            <asp:TextBox id="txtfindocdepartmentid"  Text="0" runat="server" class=" hdnac"/>
            <img src="images/down-arrow.png" class="quick-new epage"/>
        </td>
    </tr>
    <tr>
        <td>FinDoc Department</td>
        <td>
            <asp:TextBox ID="cate" MaxLength="100" runat="server" m="findocdepartment" cn="departmentname"                 CssClass="textbox ac txtac"></asp:TextBox>
            <asp:TextBox id="ttcatyeg"  Text="0" runat="server" class=" hdnac"/>
            <img src="images/down-arrow.png" class="quick-new epage"/>
        </td>
    </tr>
    
</table>
</asp:Content>

