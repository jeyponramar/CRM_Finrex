<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="admin_dashboard" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td>
        <table width="100%">
            <tr><td class="title">Dashboard</td>
                <td align="right"><img src="../images/refresh.png" class="refresh"/></td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td>
        <table width="100%" cellpadding="0" cellspacing="0">
             <tr>
                <td style="width:70%;vertical-align:top;">
                    <table width="100%">
                        <tr>
                            <td class="home-title tcorner">AMC Reminder</td>
                        </tr>
                        <tr>
    <td style="padding-top:20px;padding-bottom:20px;">
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td class="reminder-box">
                    <div class="left">
                    <table width="100%">
                        <tr><td align="center"><div class="left" style="padding-left:25px"><img src="../images/amc-reminder.png" /></div>
                        </td></tr>
                        <tr><td>AMC</td></tr>
                    </table>
                    </div>
                </td>
                <td><div class="left notification-circle" id="Div5">8</div></td>
                <td width="30">&nbsp;</td>
                <td class="reminder-box">
                    <table width="100%">
                        <tr><td align="center"><div class="left" style="padding-left:25px"><img src="../images/amc-service-reminder.jpg" /></div>
                        </td></tr>
                        <tr><td>AMC Service</td></tr>
                    </table>
                </td>
                <td><div class="left notification-circle" id="Div1">8</div></td>
                <td width="30">&nbsp;</td>
                <td class="reminder-box">
                    <table width="100%">
                        <tr><td align="center"><div class="left" style="padding-left:25px"><img src="../images/amc-reminder.png" /></div>
                        </td></tr>
                        <tr><td>AMC</td></tr>
                    </table>
                </td>
                <td><div class="left notification-circle" id="Div6">8</div></td>
                <td width="30">&nbsp;</td>
                <td class="reminder-box">
                    <table width="100%">
                        <tr><td align="center"><div class="left" style="padding-left:25px"><img src="../images/amc-service-reminder.jpg" /></div>
                        </td></tr>
                        <tr><td>AMC Service</td></tr>
                    </table>
                </td>
                <td><div class="left notification-circle" id="Div7">8</div></td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </td>
    
</tr>
                        
                    </table>
                </td>
                <td style="width:30%;vertical-align:top;">
                    <table width="100%">
                        <tr>
                            <td class="home-title tcorner">Notes</td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height:300px;overflow:scroll;font-family:Tahoma;">
                                    <table>
                                        <tr>
                                            <td class="tdnote" style="display:none;">
                                                <table width="100%">
                                                    <tr>
                                                        <td><textarea class="txtnote" name="txtnote" rows="100" style="height:50px;" cols="30"></textarea></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left:20px;">
                                                            <div class="left">Priority : </div>
                                                            <div class="left">
                                                                <div class="bg-blue note-priority hand" priority="1" style="border:solid 2px #4c4e4e"></div>
                                                                <div class="bg-green note-priority hand" priority="2"></div>
                                                                <div class="bg-red note-priority hand" priority="3"><input type="hidden" class="h_notepriority" value="1" name="hdnpriorityid"/></div>
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <input type="button" value="save" class="button save-note" m="note" target="tblnote"/>
                                                            <input type="button" value="cancel" class="cancel cancel-note"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="spage" href="#/note/a/noteview">view all</td>
                                                        <td align="right" valign="top" class="tdaddnote"><img src="../images/note-add.png" height="15" title="Create new note" class="hand createnote"/></td>
                                                    </tr>
                                                </table>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tblnotelist">
                                                    <asp:Literal ID="ltNotes" runat="server"></asp:Literal>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                            </td>
                        </tr>
                    </table>
                </td>
             </tr>
        </table>            
    </td>
</tr>   
<tr>
    <td class="home-title tcorner">Complaint Summary</td>
</tr>
<tr>
    
</tr>
<tr>
    <td>
        <table width="100%" cellpadding="0" cellspacing="0">
             <tr>
                <td style="width:50%;vertical-align:top;">
                    <table width="100%">
                        <tr>
                            <td class="home-title tcorner">AMC Reminder</td>
                        </tr>
                        <tr>
                            <td>
                                <uc:Grid id="gridAmcReminder" runat="server" Module="amcreminder"/>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:50%;vertical-align:top;">
                    <table width="100%">
                        <tr>
                            <td class="home-title tcorner">Sales Reminder</td>
                        </tr>
                        <tr>
                            <td>
                                <uc:Grid id="gridsalesreminder" runat="server" Module="salesreminder"/>
                            </td>
                        </tr>
                    </table>
                </td>
             </tr>
        </table>            
    </td>
</tr>   
</table>
</asp:Content>

