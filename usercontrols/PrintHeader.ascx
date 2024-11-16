<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrintHeader.ascx.cs" Inherits="usercontrols_PrintHeader" %>
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="font-size:30px;color:#015e7c;font-weight:bold;" align = "center">
                        <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                    </td>
                 </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="padding-left:25px; padding-right:25px;">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style=" width:90%;color:#052a47;border-bottom:solid 0px #015e7c;">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="font-size:15px;text-align:center;padding-top:5px;padding-left:10px;"><b>Admin. Off. : </b><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="font-size:15px;text-align:center;padding-top:5px;padding-left:10px;"><img src="../images/dot2.png" width="7px"/>&nbsp; <b>Telefax : </b><asp:Label ID="lblphone" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;<img src="../images/dot2.png" width="7px"/>&nbsp; <b>Cells : </b><asp:Label ID="lblmobile" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td  style="font-size:15px;text-align:center;padding-top:5px;padding-left:10px;padding-bottom:5px;"><b>E-mails : </b><asp:Label ID="lblemail" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;<img src="../images/dot2.png" width="7px"/>&nbsp; <b>Web Site : </b><asp:Label ID="lblwebsite" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td  style="font-size:15px;text-align:center;padding-top:5px;padding-left:10px;padding-bottom:5px;"><b>GST No. : </b><asp:Label ID="lblgstno" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;<img src="../images/dot2.png" width="7px"/>&nbsp; <b><asp:Label ID="lblstate" runat="server"></asp:Label></b></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>