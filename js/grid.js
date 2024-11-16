//var _currentRow = 1;
//var _jsGrid_selectedId = 0;

function grid(options) {
    this.detailtable = options.detailtable;
    this.gridtable = options.gridtable;
    this.add = add;
    this.btnadd = options.btnadd;
    this.currentRow;
    this.selectedId;
    function add() {
        var btnadd = this.btnadd;
        var cr = 1;
        var sid = 0;
        var param = this.gridtable.find(".g_param");
        if (getParam(param, "cr") != "") {
            cr = parseInt(getParam(param, "cr"));
        }
        if (getParam(param, "sid") != "") {
            sid = parseInt(getParam(param, "sid"));
        }
        this.currentRow = cr;
        this.selectedId = sid;
        
        if (this.btnadd.val() == "Update") {
            this.currentRow = this.selectedId;
        }
        var c = "repeater-row";
        if(this.currentRow%2==0)
        {
            c="repeater-alt";
        }
        var tr = $("<tr id='grid_" + this.currentRow + "' class='"+c+"'></tr>");
        var delbtn = $("<td class='delete-row'><img src='images/close.png' width='16px' height='16px' /></td>");
        var arrcols = new Array();
        var colname = "";
        tr.append("<td class='hdn'></td>");

        this.gridtable.find("tr:first td").each(function() {
            if ($(this).attr("cn") == undefined) {
                colname = $(this).text();
            }
            else {
                colname = $(this).attr("cn");
            }
            if (colname != "") {
                var val = "";
                var align = "";
                try {
                    if ($("#" + colname)[0].tagName == "SELECT") {
                        val = $("#" + colname).find("option:selected").text();
                    }
                    else {
                        val = $("#" + colname).val();
                    }
                } catch (e) { }
                var dt = $(this).attr("dt");
                if (dt == "amt") {
                    val = parseFloat(val).toFixed(2);
                    align = " style='text-align:right'";
                }
                else if (dt == "i") {
                    align = " style='text-align:right'";
                }
                td = "<td" + align + " class='" + colname + " gridtr'>" + val + "</td>";
                tr.append(td);
            }
        });

        var colIndex = 0;
        var currentRow = this.currentRow;
        this.detailtable.find("input,select").each(function() {
            var type = $(this).attr("type");
            if (type != "button") {
                var id = $(this).attr("id");
                var val = $(this).val();
                var ctrl = $('<input/>').attr({ type: 'hidden', name: '@sm_' + id + "-" + currentRow }).addClass('text');
                ctrl.val(val);
                tr.find(".hdn").append(ctrl);
            }
        });
        if (this.btnadd.val() == "Add") {
            this.gridtable.append(tr);
            this.currentRow = this.currentRow + 1;
            setParam(param,"cr",this.currentRow);
        }
        else {
            this.gridtable.find("#grid_" + this.selectedId).replaceWith(tr);
            btnadd.val("Add");
        }
        this.detailtable.find("input").each(function() {
            if ($(this)[0].tagName == "INPUT" || $(this)[0].tagName == "SELECT") {
                if ($(this).attr("type") != "button") {
                    $(this).val("");
                }
            }
        });
        tr.append(delbtn);


        this.gridtable.find(".delete-row").live("click", function() {
            $(this).parent().remove();
            btnadd.val("Add");
        });
        gridtable = this.gridtable;
        this.gridtable.find(".gridtr").live("click", function() {
            var trid = $(this).parent().attr("id");
            sid = $(this).parent().attr("id").split('_')[1];
            var prm = gridtable.find(".g_param");
            setParam(prm, "sid", sid);
            
            btnadd.val("Update");
            $("#" + trid + " :input").each(function() {
                var nm = $(this).attr("name").split('-')[0].split('_')[1];
                $("#" + nm).val($(this).val());
            });
        });

        return;
    }
}
