<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionManager.aspx.cs" Inherits="CP_ActionManager" EnableEventValidation="false" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<style type="text/css">
    .btnred
    {
        width:150px;
        height:35px;
        background-color:Red;
        color:White;
    }
    .btngreen
    {
        width:150px;
        height:35px;
        background-color:Green;
        color:White;
    }
    .error
    {
        color:Red;
    }
</style>
    <title></title>
</head>

<body style="background-color:#fffffe">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="1" width="50%">    
        <tr><td  colspan="2" align="center"><asp:Label runat="server" ID="lblMessage" Visible="false" CssClass="error"></asp:Label> </td></tr>            
        <tr>
            <td>
                Module Name
            </td>
            <td>
                <asp:DropDownList ID="ddlModule" runat="server"></asp:DropDownList>
            </td>
        </tr>
        
        <tr>
            <td colspan="2" align="center">
                <asp:Button runat="server" Text="Action Manager" CssClass="btnred"  OnClick="btn_generateAction" ID="Button1" />
            </td>    
         </tr>
         <tr>
            <td colspan="2" align="center">
                <asp:Button runat="server" Text="Generate Code" CssClass="btngreen"  OnClick="btn_GenareteCode" ID="btnGenerateOpp" />
            </td>             
        </tr> 
        
    </table>
    <table width="100%" style="padding-top:100px">
    <tr>
        <td></td>
    </tr>
      <tr>
            <td>
                DESIGN
            </td>
            <td style="font-size:19px">
                    <asp:TextBox TextMode="MultiLine" ID="txtDesignCode" runat="server" Width="700" Height="150"></asp:TextBox>         
            </td>
        </tr>
        <tr>
            <td>CODE</td>
            <td style="font-size:19px">
                <asp:TextBox TextMode="MultiLine" ID="txtCharpCode" runat="server" Width="700" Height="150"></asp:TextBox>         
            </td>
        </tr>
    </table>
    
    
    </form>
</body>
</html>
