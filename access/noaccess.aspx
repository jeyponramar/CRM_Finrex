<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="noaccess.aspx.cs" Inherits="noaccess" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function(){
        parent.setTitle();
        $("#back").click(function(){
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title" colspan="2">
            Access Rights
        </td>
    </tr>
    <tr>
        <td class="error" align="center" style="padding-top:50px;"><asp:Label ID="lblmessage" runat="server" Text="You do not have enough rights to perform this operation, Please contact administrator."></asp:Label></td>
    </tr>
</table>    
</asp:Content>

