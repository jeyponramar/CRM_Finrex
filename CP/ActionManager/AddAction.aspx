<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddAction.aspx.cs" Inherits="CP_ActionManager_AddAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="../../css/jquery-ui.css" rel="stylesheet" type="text/css" />    
    <script src="../../js/jquery.min.js" type="text/javascript"></script>    
    <script src="../../js/jquery-ui.min.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
   <script type="text/javascript">
       $(document).ready(function() {
           $(".action").change(function() {
               var Actionval = parseInt($(this).find("option:selected").val());
               if (Actionval == 1) {
                   $(".checkconditionbeforeaction").show();
               }
               else if (Actionval == 5) {
                    $(".createcontacts").show();
               }
               if (Actionval > 0) {
                   $(".addAction").show();
               }
           });
       });
</script>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
           <tr>
                <td>
                    Action
                </td>
                <td>                
                    <asp:DropDownList ID="ddlAction" runat="server" CssClass="action">
                        <asp:ListItem Value="0" Selected="True">Select Action</asp:ListItem>
                        <asp:ListItem Value="1">Add Action Button</asp:ListItem>
                        <asp:ListItem Value="2">Check Condition Before Action</asp:ListItem>
                        <asp:ListItem Value="3">Update Status</asp:ListItem>
                        <asp:ListItem Value="4">Copy Data</asp:ListItem>
                        <asp:ListItem Value="5">Create Contacts</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>                         
        </table>
        <table>
            <tr>
                <td>
                    <table>
                        <tr class="addAction"  style="display:none">
                            <td><asp:TextBox Text="" placeholder="ActionName" runat="server" ID="txtActionName"></asp:TextBox><asp:TextBox Text="" placeholder="ActionButton Color" runat="server" ID="txtButtonColor"></asp:TextBox><asp:TextBox Text="" placeholder="RedirectURL" runat="server" ID="txtRedirectURL"></asp:TextBox> </td>
                        </tr>
                        <tr class="checkconditionbeforeaction" style="display:none">
                            <td>
                                <asp:TextBox  placeholder="Write Query" runat="server" ID="txtconditionquery" Text=""></asp:TextBox>                
                            </td>
                        </tr> 
                        <tr class="createcontacts" style="display:none">
                            <td>
                                <asp:TextBox  placeholder="Write Unique Filter  Query .ie(contacts_mobileno=$mobileno$)" Width="600" runat="server" ID="txtcreatecontacts" Text=""></asp:TextBox>                
                            </td>
                        </tr>           
                    </table>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Button runat="server" Text="Save Action" OnClick="btnSaveAction_Click" ID="btnGenerate" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
