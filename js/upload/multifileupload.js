$(document).ready(function() {
    if (!_enabledFileUpload) return;
    $(".filediv,.imgfilediv").click(function() {
        _selectedFileDiv = $(this);
    });
});
$(function() {
    if (!_enabledFileUpload) return;

    $('form').fileUploadUI({
        method: 'POST',
        buildUploadRow: function(files, index, handler) {

            var fileType = _selectedFileDiv.attr("filetype");
            var tbl = _selectedFileDiv.closest("table").find(".tblfiles");
            if (_selectedFileDiv.attr("heditor-imgupload") == "true") {
                return "";
            }

            var fileName = files[index].name;
            //check ext
            var arr = fileName.split('.');
            var ext = arr[arr.length - 1];
            var valid = false;
            if (fileType == "image") {
                if (ext == "jpg" || ext == "jpeg" || ext == "png" || ext == "bmp") {
                    valid = true;
                }
            }
            else if (fileType == "audio" || fileType == "song") {
                if (ext == "mp3" || ext == "wav") {
                    valid = true;
                }
            }
            else if (fileType == "video") {
                if (ext == "avi" || ext == "wmv" || ext == "mov" || ext == "mpg" || ext == "vob" || ext == "3g2") {
                    valid = true;
                }
            }
            else if (fileType == "text") {
                if (ext == "txt") {
                    valid = true;
                }
            }
            else if (fileType == "word") {
                if (ext == "doc" || ext == "docx") {
                    valid = true;
                }
            }
            else if (fileType == "ppt") {
                if (ext == "ppt" || ext == "pptx") {
                    valid = true;
                }
            }
            else if (fileType == "excel") {
                if (ext == "xls" || ext == "xlsx") {
                    valid = true;
                }
            }
            else if (fileType == "doc") {
                if (ext == "xml" || ext == "xmlx" || ext == "txt" || ext == "ppt" || ext == "pptx" || ext == "xls" || ext == "xlsx") {
                    valid = true;
                }
            }
            else {
                valid = true;
            }
            if (!valid) {
                alert("Invalid file format!");
                return "stop";
            }
            //check already exists
            var exists = false;
            tbl.find("a").each(function() {
                if ($(this).text() == fileName) {
                    exists = true;
                }
            });
            if (exists) {
                alert("File already exists!");
                return "stop";
            }
            return $('<tr><td>' + files[index].name + '<\/td>' +
                    '<td class="file_upload_progress"><div><\/div><\/td>' +
                    '<td class="file_upload_cancel">' +
                    '<button class="ui-state-default ui-corner-all" title="Cancel">' +
                    '<span class="ui-icon ui-icon-cancel">Cancel<\/span>' +
                    '<\/button><\/td><\/tr>');
        },
        buildDownloadRow: function(file) {
            if (file == "") return;
            var imgsrc;
            var deleteimgsrc;
            var ismulti = ConvertToBool(_selectedFileDiv.attr("ismultiple"));
            var isguid = ConvertToBool(_selectedFileDiv.attr("isguid"));
            var FolderPath = _selectedFileDiv.attr("folder");
            var tbl = _selectedFileDiv.closest("table").find(".tblfiles");
            var hdn = _selectedFileDiv.closest(".tblmiltiupload").find(".hdnfiles");
            //var filenames = ConvertToString(_selectedFileDiv.attr("filenames"));
            var filenames = hdn.val();

            if (_selectedFileDiv.attr("heditor-imgupload") == "true") {
                _htmlEditor.focus();
                document.execCommand('insertHTML', false, "<img class='htmleditor-img' src='../upload/htmleditor/" + file.guidfilename + "'/>");
                _htmlEditor.focus();
                return "";
            }

            if (isguid == true) {
                deleteimgsrc = FolderPath + file.guidfilename;
            }
            else {
                deleteimgsrc = FolderPath + file.name;
            }
            if (ismulti === false) {
                tbl.find("tr").remove();
                filenames = "";
            }
            var fullPath = deleteimgsrc.replace("~", "..");
            deleteimgsrc = deleteimgsrc.replace("..", "~");
            if (file.filetype == "image") {
                if (isguid == true) {
                    imgsrc = _fileUploadPrefix + FolderPath.replace("~", "..") + file.guidfilename;
                }
                else {
                    imgsrc = _fileUploadPrefix + FolderPath.replace("~", "..") + file.name;
                }
            }
            else if (file.filetype == "song") {
                imgsrc = _fileUploadPrefix + "images/song-icon.png";
            }
            else if (file.filetype == "video") {
                imgsrc = _fileUploadPrefix + "images/video-icon.png";
            }
            else if (file.filetype == "doc") {
                imgsrc = _fileUploadPrefix + "images/doc-icon.png";
            }
            else if (file.filetype == "txt") {
                imgsrc = _fileUploadPrefix + "images/txt-icon.png";
            }
            else if (file.filetype == "pdf") {
                imgsrc = _fileUploadPrefix + "images/pdf-icon.png";
            }
            else if (file.filetype == "zip") {
                imgsrc = _fileUploadPrefix + "images/icon/zip.png";
            }
            else if (file.filetype == "excel") {
                imgsrc = _fileUploadPrefix + "images/xl-icon.png";
            }
            else if (file.filetype == "ppt") {
                imgsrc = _fileUploadPrefix + "images/ppt-icon.png";
            }
            else if (file.filetype == "unknown") {
                imgsrc = _fileUploadPrefix + "images/unknown.png";
            }

            if (isguid == true) {
                if (filenames == "") {
                    filenames = file.name + "," + file.guidfilename;
                }
                else {
                    filenames = filenames + "|" + file.name + "," + file.guidfilename;
                }
            }
            else {
                if (filenames == "") {
                    filenames = file.name;
                }
                else {
                    filenames = filenames + "|" + file.name;
                }
            }
            hdn.val(filenames);
            var html = $("<tr><td align='center'><img src=\'" + imgsrc + "' width='25px'/>" +
                "<\/td><td><a href='" + _fileUploadPrefix + fullPath + "' target='_blank'>" + file.name + "</a><\/td>" +
                "<td style='padding-left:20px;'><img src='" + _fileUploadPrefix + "images/delete.png' class='hand deletefile' val='" + deleteimgsrc + "' fn='" + file.name + "' title='Delete'></td></tr>");
            //if (ismulti == false) {
            //  filediv.hide();
            //}

            return html;
        }
    });
});
function deletefile(imgsrc) {
    $.ajax({
        type: "POST",
        url: _fileUploadPrefix + "deletefile.ashx?imgsrc=" + imgsrc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function(jsonObj) {
        },
        error: function(ex) {
            alert("Error occurred while processing your request");
        }
    });
    return false;
}
function setFileInHidden(obj) {
    
}
$(document).ready(function() {
    $(".deletefile").live("click", function() {
        var r = confirm("Are you sure you want to delete this file?")
        if (!r) return;
        var tbl = $(this).closest(".tblmiltiupload");
        var filediv = tbl.find(".filediv");
        var imgsrc = $(this).attr("val");
        var fn = $(this).attr("fn");
        var URL = _fileUploadPrefix + "deletefile.ashx?imgsrc=" + imgsrc+"&fn="+fn+"&m="+filediv.attr("m")+"&mid="+filediv.attr("mid")+"&cm="+filediv.attr("cm")+"&cmid="+filediv.attr("cmid");
        var response = RequestData(URL);
        if (response != "1") {
            if (response == "Session Expired") {
                alert("Your session expired, please logout and login again!");
                return false;
            }
            else {
                alert("Error occurred while processing your request!");
                return false;
            }
        }
        var hdn = tbl.find(".hdnfiles");
        var files = "";
        var tbl = $(this).closest(".tblmiltiupload");
        $(this).closest("tr").remove();
        tbl.closest(".tblmiltiupload").find(".deletefile").each(function() {
            var fn = $(this).attr("fn");
            if (files == "") {
                files = fn;
            }
            else {
                files += "|" + fn;
            }
        });
        hdn.val(files);

        //var trhtml = $(this).closest("tr").remove();
        //var filetodelete = $(this).attr("fn");
        //var hdnfiles = hdn.val();
        //alert(hdnfiles);
        /*if (hdnfiles.indexOf(filetodelete) >= 0) {
        if (hdnfiles.indexOf('|') >= 0)//multiple files
        {
        var arr1 = hdn.val().split('|');
        var arr2 = "";
        for (i = 0; i < arr1.length; i++) {
        if (arr1[i].indexOf(filetodelete) < 0) {
        if (arr2 == "") {
        arr2 = arr1[i];
        }
        else {
        arr2 = "|" + arr1[i];
        }
        }
        }
        hdn.val(arr2);
        }
        else {//one file only
        hdn.val("");

            }
        }*/

    });

});