<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="contact-us.aspx.cs" Inherits="contact_us" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".i-mainmenu").removeClass("i-mainmenu-active");
        $(".menu-contactus").addClass("i-mainmenu-active");
    });
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr><td><h1>Finrex Treasury Advisors</h1></td></tr>
    <tr>
        <td>
            B-209, Shyam Kamal, Agarwal Market, Tejpal Road <br />
            Vile Parle (East), Mumbai - 400 057.<br />
            info@finrex.in <br />
            Board line : 022-26122369
        </td>
    </tr>
</table>
</asp:Content>

