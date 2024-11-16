$(document).ready(function() {
    $(".note-priority").click(function() {
        $(".h_notepriority").val($(this).attr("priority"));
        $(".note-priority").css("border", "solid 1px #4c4e4e");
        $(this).css("border", "solid 2px #4c4e4e");
    });
    $(".createnote").click(function() {
        $(".tdaddnote").hide();
        $(".tdnote").show();
        
        $(".txtnote").focus();
    });
    $(".save-note").click(function() {
        if ($(".txtnote").val().trim() != null && $(".txtnote").val().trim() != "") {

            var id = SaveFormData($(this));
        }
        else {
            alert('Please Enter Note');
            return false;
        }
        //var id = SaveFormData($(this));
        if (id <= 0) {
            alert("Unable to save the note");
            return false;
        }
        var note = $(".txtnote").val();
        var priority = parseInt($(".h_notepriority").val());
        $(".h_notepriority").val("1");
        $(".note-priority").css("border", "solid 1px #4c4e4e");
        $(".note-priority:first").css("border", "solid 2px #4c4e4e");
        $(".txtnote").val("");
        var color = "";
        if (priority == 1) {
            color = "blue";
        }
        else if (priority == 2) {
            color = "green";
        }
        else {
            color = "red";
        }
        var html = "<tr class='noterow'><td><table width='100%'><tr><td class='note_" + color + "'><table width='100%'><tr><td width='90%' class='note'>" + note + "</td><td rowspan='2'><img src='../images/icon/pin_" + color + ".png' class='delete-note hand' inner='1' nid='" + id + "' title='Delete this note'/></td></tr><tr><td class='right'>Now</td></tr></table></td></tr></table></td></tr>";
        var tr;
        if ($(".tblnotelist").find("tr").length == 0) {
            $(".tblnotelist").append(html);
        }
        else {
            $(html).insertBefore($(".tblnotelist tr:first"));
        }
        $(".tdaddnote").show();
        $(".tdnote").hide();
    });
    $(".cancel-note").click(function() {
        $(".tdnote").hide();
        $(".tdaddnote").show();
    });
    $(".delete-note").live("click", function() {
        if (confirm("Are you sure you want to delete this note?")) {
            if ($(this).attr("inner") == "1") {
                RequestData("../utilities.ashx?m=deletenote&id=" + $(this).attr("nid"));
            }
            else {
                RequestData("utilities.ashx?m=deletenote&id=" + $(this).attr("nid"));
            }
            $(this).closest(".noterow").remove();
            $(".tdnote").hide();
            $(".tdaddnote").show();
        }
    });
});