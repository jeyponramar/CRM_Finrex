<%@ Control Language="C#" AutoEventWireup="true" CodeFile="chatter.ascx.cs" Inherits="chatter" %>
<script>
    var _CUR_ATTACHMENT;
    $(document).ready(function() {
        
        $(".file").click(function(){
            _CUR_ATTACHMENT = $(this).parent().parent().find(".attachment");
        });
        $(".post").click(function() {
            var dtarget = $(this).attr('dtarget');
            var s = saveJson($(this));
            if(!s) return;
            var message = $(".sharemessage").val();
            var photoPath = "../images/user/" + __LOGIN_USERID + ".jpg";
            var date = "Just now";
            var name = __LOGIN_FIRSTNAME;
            var row = "<tr><td width='70' class='valign'><img src='" + photoPath + "' width='25'/></td>" +
                    "<td class='valign'>" +
                        "<table width='100%' cellpadding=0 cellspacing=0>" +
                            "<tr><td class='valign'><table width='100%' cellpadding=0 cellspacing=0><tr><td><b>" + name + "</b></td>" +
                                    "<td class='right date'>" + date + "</td></tr>" +
                                    "</table></td></tr>" +
                            "<tr><td>" + message + "</td></tr>" +
                        "</table>" +
                    "</td>" +
                    "</tr>";
            row += "<tr><td>&nbsp;</td><td>"+GetAttachment()+"</td></tr>";
            if($(".tblshare").find("tr").length==0)
            {
                $(".tblshare").append(row);
            }
            else
            {
                $(row).insertBefore(".tblshare tr:first");
            }
            $("." + dtarget).each(function(){
                if($(this).attr("class").indexOf("noclear")<0)
                {
                    $(this).val("");
                }
            });
            $(".uploaded-files").html("");
        });
        
        $(".savesharecomment").keypress(function(event) {
            if (event.which == 13) {
                var dtarget = $(this).attr('dtarget');
                var s = saveJson($(this));
                if(!s) return false;
                var message = $(this).val();

                var photoPath = "../images/user/" + __LOGIN_USERID + ".jpg";
                var date = "Just now";
                var name = __LOGIN_FIRSTNAME;
                var row = "<tr><td>&nbsp;</td><td style='background-color:#f0f2f6;'><table width='100%'>" +
                            "<tr>"+
                            "<td width='70' class='valign'><img src='" + photoPath + "' width='50'/></td>" +
                            "<td class='valign'>" +
                                "<table width='100%' cellpadding=0 cellspacing=0>" +
                                    "<tr><td class='valign'><table width='100%' cellpadding=0 cellspacing=0><tr><td><b>" + name + "</b></td>" +
                                            "<td class='right date'>" + date + "</td></tr>" +
                                            "</table></td></tr>" +
                                    "<tr><td>" + message + "</td></tr>" +
                                "</table>" +
                            "</td>" +
                            "</tr>" +
                        "</table></td></tr>";
                                    
                $(row).insertBefore($(this).parent().parent());
                $("." + dtarget).val("");
                return false;
            }
        });
        function GetAttachment()
        {
            if(_CUR_ATTACHMENT == undefined) return "";
            var files = _CUR_ATTACHMENT.val();
            if(files == "") return "";
            var arr = files.split('|');
            var html = "";
            for(i=0;i<arr.length;i++)
            {
                var arr2 = arr[i].split('=');
                var newfileName = arr2[1];
                var fileName = arr2[0];
                var width = 25;
                
                var ext = "." + getFileExt(newfileName);
                
                if (ext == ".doc" || ext == ".docx") {
                    icon = "../images/icon/doc.png";
                }
                else if (ext == ".bmp" || ext == ".jpg" || ext == ".gif"
                        || ext == ".png" || ext == ".tif") {
                    icon = "../upload/share/"+newfileName;
                    width = 50;
                }
                else if (ext == ".pdf") {
                    icon = "../images/icon/pdf.png";
                }
                else if (ext == ".zip") {
                    icon = "../images/icon/zip.png";
                }
                else if (ext == ".txt") {
                    icon = "../images/icon/text.png";
                }
                else {
                    icon = "../images/icon/unknown.png";
                }
                html += "<a href='../download.aspx?f=upload/share/"+newfileName+"' target='_blank'>"+
                                "<img width='" + width + "' src='" + icon + "'/>"+
                         "</a>&nbsp;" +
                        fileName+"&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;";
            }
            return html;
        }
        function getJsonSaveUrl(btn) {
            var url = "../savejson.ashx?m=" + btn.attr('m');
            return url;
        }
        function saveJson(btn) {
            var dtarget = btn.attr('dtarget');
            var Data = getJsonInputData(dtarget);
            var issuccess = false;
            $.ajax({
                   url: getJsonSaveUrl(btn),
                   type: "POST",
                   data: Data,
                   dataType: 'json',
                   async: false,
                   success: function(msg) {
                       issuccess = true;
                   },
                   error: function(msg) {
                       alert("Error occurred.");
                       issuccess = false;
                   }
            });
            return issuccess;
        }
        function getJsonInputData(dtarget) {
            var data = "";
            var arr = [];
            $('.' + dtarget).each(function() {
                var $this = $(this);
                arr.push({ name: $this.attr('name'), value: $this.val() });
            });
            data = $.param(arr); 
            return data;
        }
    });
</script>
        
        <script>
            var FolderPath = "../upload/" + $("form").attr("folder") + "/";
            $(function() {
                function getUploadUrl() {
                    var folder = '../FileUpload.ashx?folder=' + $("form").attr("folder");
                    return folder;
                }
                $('form').fileUploadUI({
                    url: getUploadUrl(),
                    method: 'POST',
                    uploadTable: $('.upload-files'),
                    downloadTable: $('.uploaded-files'),
                    buildUploadRow: function(files, index) {
                        return $('<tr><td>' + files[index].name + '<\/td>' +
                            '<td class="file_upload_progress"><div><\/div><\/td>' +
                            '<td class="file_upload_cancel">' +
                            '<button class="ui-state-default ui-corner-all" title="Cancel">' +
                            '<span class="ui-icon ui-icon-cancel">Cancel<\/span>' +
                            '<\/button><\/td><\/tr>');
                    },
                    buildDownloadRow: function(file) {
                        var fileName = FolderPath + file.name;
                        var ext = file.ext;
                        var thumbnail = FolderPath + file.thumbnail;
                        var newfileName = file.newfile;
                        var icon = "";
                        var arr = fileName.split('/');
                        var shortName = arr[arr.length - 1];
                        var tooltip = shortName + " (" + file.size + ")";
                        var width = 25;
                        if (ext == ".doc" || ext == ".docx") {
                            icon = "../images/icon/doc.png";
                        }
                        else if (ext == ".bmp" || ext == ".jpg" || ext == ".gif"
                                || ext == ".png" || ext == ".tif") {
                            icon = thumbnail;
                            width = 25;
                        }
                        else if (ext == ".pdf") {
                            icon = "../images/icon/pdf.png";
                        }
                        else if (ext == ".zip") {
                            icon = "../images/icon/zip.png";
                        }
                        else if (ext == ".txt") {
                            icon = "../images/icon/text.png";
                        }
                        else {
                            icon = "../images/icon/unknown.png";
                        }
                        var files = _CUR_ATTACHMENT.val();
                        if(files == "")
                        {
                            files = file.name + "=" + newfileName;
                        }
                        else
                        {
                            files = files + "|" + file.name + "=" + newfileName;
                        }
                        _CUR_ATTACHMENT.val(files);
                        return $("<tr>"+
                                     "<td>"+
                                         "<a href='download.aspx?m=followup&f=upload/share/"+newfileName+"' target='_blank'>"+
                                            "<img width='" + width + "' src='" + icon + "' alt='" + tooltip + "' title='" + tooltip + "'/>"+
                                         "</a>" +
                                      "</td>"+
                                      "<td>"+shortName+"</td>"+
                                      "<td>"+file.size+"</td>"+
                                      //"<td><img src='../images/delete.png'</td>"+
                                  "</tr>");

                    }
                });
            });
        </script> 
<table width="100%">
    <tr><td class="title openclose hand">Post / Comments</td></tr>
    <tr>
        <td>
            <table class="share">
            <tr>
                <td>
                    <table>
                        <tr><td><textarea id="txtMessage" name="txtMessage" rows="2" style="height:30px;" cols="70" class="d_post saveenter sharemessage" savetarget="post"></textarea>
                                <asp:TextBox ID="txtTargetModule" runat="server" CssClass="d_post noclear hidden"></asp:TextBox>
                                <asp:TextBox ID="txtTargetId" runat="server" CssClass="d_post noclear hidden"></asp:TextBox>
                        </td>
                        <td><input type="button" id="btnPost" class="post button" value="Post" dtarget="d_post" m="share"/></td></tr>
                    </table>
                </td>
            </tr>
            <%--<tr>
                <td>
                <table>
                    <tr>
                        <td align="left">
                            <input type="hidden" name="attachment" class="share_attachment attachment d_post"/>
                            <div id="filediv" style="width:100px;" title="You can select and upload one or more files">
                                Attachment : <img src="../images/attach.jpg" id="attach" class="attach" title="Attach file"/>
                                         <input type="file" multiple name="file" class="file" folder="followup">
                                         
                            </div>                                 
                        </td>
                    </tr>
                    <tr><td>
                        <table class="uploaded-files"></table>
                    </td></tr>
                    <tr><td class="upload-files">
                        <table></table>
                    </td></tr>
                </table>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <table width='100%' class="tblshare">
                        <asp:Literal ID="ltshare" runat="server"></asp:Literal>
                    </table>
                </td>
            </tr>
        </table>
        </td>
    </tr>
</table>        

