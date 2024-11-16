<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Notes.ascx.cs" Inherits="usercontrols_Dashboard_Notes" %>
<table id="tblnotes" width="100%" cellpadding="0" cellspacing="0">
    <tr><td class="dashboard-title">Notes</td></tr>
    <tr>
    <td align="center">
        <div style="font-family:Tahoma;">
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
                                <td class="spage" href="#/note/a/noteview" title="View Note">view all</td>
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
