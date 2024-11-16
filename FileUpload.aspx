<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="FileUpload.aspx.cs" Inherits="FileUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk File Upload</title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" id="theme">
    <link rel="stylesheet" href="js/upload/jquery.fileupload-ui.css" />
    <link rel="stylesheet" href="css/common.css" />
</head>
<body>
    <form id="file_upload" runat="server">
        <div id = "filediv">
            <input type="file" name="file" multiple>
            <button>Upload</button>
            <div>Upload files</div>
        </div>
        <div>Click on above button Upload files to upload <b>one or more </b>files.</div>
        <div style="height:20px">&nbsp;</div>
        <table id="files" style="background-color:#ffffff;border:1px solid #9999ff" cellspacing="0" cellpadding="5">
            <tr style="background-color:#9999ff;color:White;font-weight:bold;">
                <td style="width:300px">Icon</td>
                <td style="width:300px">File Name</td>
                <td style="width:100px">Size</td>
            </tr>
            <tr>
                <td style="height:1px"></td>
            </tr>
        </table>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js"></script>
        <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/jquery-ui.min.js"></script>
        <script src="js/upload/jquery.fileupload.js"></script>
        <script src="js/upload/jquery.fileupload-ui.js"></script>
        <script>
            /*global $ */
            var FolderPath = "images/product/";
            var FileType = "image";

            $(function() {
                $('#file_upload').fileUploadUI({
                    url: 'FileUpload.ashx?type=image',
                    method: 'POST',
                    uploadTable: $('#files'),
                    downloadTable: $('#files'),
                    buildUploadRow: function(files, index) {
                        return $('<tr><td>' + files[index].name + '<\/td>' +
                            '<td class="file_upload_progress"><div><\/div><\/td>' +
                            '<td class="file_upload_cancel">' +
                            '<button class="ui-state-default ui-corner-all" title="Cancel">' +
                            '<span class="ui-icon ui-icon-cancel">Cancel<\/span>' +
                            '<\/button><\/td><\/tr>');
                    },
                    buildDownloadRow: function(file) {
                        var imgsrc;
                        if (FileType == "image") {
                            imgsrc = FolderPath + file.name;
                        }
                        else if (FileType == "song") {
                            imgsrc = "images/songicon.gif";
                        }
                        else if (FileType == "song") {
                            imgsrc = "images/videoicon.gif";
                        }
                        return $("<tr><td style='height:5px'></td></tr><tr><td align='center'><img src=\'" + imgsrc + "' width='100' border=1/>" +
                        "<\/td><td>" + file.name + "<\/td><td>" + file.size + "<\/td><\/tr><tr><td colspan='3' style='border-bottom:1px solid #9999ff;height:5px'></td></tr>");
                    }
                });
            });
        </script> 
    </form>
</body>
</html>
