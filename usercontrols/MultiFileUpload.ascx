<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultiFileUpload.ascx.cs" Inherits="MultiFileUpload" %>
<table class="tblmiltiupload">
    <tr id="trUpload" runat="server">
         <td>
            <div id="uploadfilediv" class="filediv" title="Click here to upload file" runat="server">            
                <input type="file" name="file">
                <button >Upload</button>
                <div>Upload File</div>                     
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <table class="tblfiles" cellspacing="5" cellpadding="0">
                <asp:TextBox ID="h_FileName" runat="server" CssClass="hdnfiles hidden"/>
                <asp:Literal ID="ltFiles" runat="server"></asp:Literal>    
            </table>
    </td></tr>
</table>

