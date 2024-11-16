<%@ Page Language="C#" AutoEventWireup="true" CodeFile="allocatetask.aspx.cs" Inherits="task_allocatetask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>R PLUS CRM - Allocate Task</title>
    <script src="../js/constant.js" type="text/javascript"></script>
    <link href="../css/common.css" rel="stylesheet" type="text/css" />
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />    
    <script src="../js/jquery.min.js" type="text/javascript"></script>    
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/global.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />    
    <link rel="stylesheet" type="text/css" href="../css/jquery.autocomplete.css" />    
	<script type="text/javascript" src="../js/jquery.autocomplete.js"></script>
	<script type="text/javascript" src="../js/jquery.datetimepicker.js"></script>
	
    <script>
        var trDrag;
        var tblSource;
        var dropCtrl;
        var selectedmodule;
        var selectedmid = 0;
        var selectedTaskId = 0;
        $(document).ready(function() {
            var h = $(window).height();
            var nh = parseInt(h) - 50;

            $(".opentasks-panel").css("height", nh);
            $(".task-right-arr").css("height", nh);
            $(".task-eng-list").css("height", nh);
            $(".divLeftPanel").css("height", h);
            drag();
            drop();
            function drag() {
                $(".trtask").draggable({
                    appendTo: "body",
                    helper: "clone"
                });
            }
            $(".trtask").click(function() {
                selectedmid = $(this).attr("mid");
                selectedTaskId = $(this).attr("tid");
                selectedmodule = $(this).attr("m");
                if (selectedmodule == "enquiry") {
                    $(".trbottom").hide();
                }
                else {
                    $(".trbottom").show();
                }
                $("#assigndate").datepicker("disable");
                $("#saveDialog").dialog({
                    autoOpen: true,
                    width: 500,
                    height: 500,
                    modal: true,
                    draggable: false,
                    show: {
                        effect: "blind",
                        duration: 500
                    },
                    hide: {
                        effect: "explode",
                        duration: 500
                    }
                });
                dropCtrl = $(this);
                trDrag = $(this);
                PopulateTaskDetail($(this).attr("tid"), true);
            });

            function drop() {
                $(".task-box-outer").droppable({
                    hoverClass: "task-box-outer-hover",
                    drop: function(event, ui) {
                        trDrag = ui.draggable;
                        tblSource = trDrag.closest(".tbltask");
                        dropCtrl = $(this);
                        if (trDrag.attr("class").indexOf("trnotask") >= 0) {
                            return;
                        }

                        $("#assigndate").datepicker("disable");
                        $("#closeddate").datepicker("disable");
                        $("#saveDialog").dialog({
                            autoOpen: true,
                            width: 500,
                            modal: true,
                            draggable: false,
                            show: {
                                effect: "blind",
                                duration: 500
                            },
                            hide: {
                                effect: "explode",
                                duration: 500
                            }
                        });
                        //PopulateOpenTaskDetail($(this), trDrag);
                        var taskId = trDrag.attr("tid");
                        selectedmid = trDrag.attr("mid");
                        selectedmid = taskId;
                        selectedmodule = trDrag.attr("m");
                        alert(selectedmodule);
                        if (selectedmodule == "enquiry") {
                            $(".trbottom").hide();
                        }
                        else {
                            $(".trbottom").show();
                        }
                        PopulateTaskDetail(taskId, false);
                    }
                });

            }
        });
        function PopulateTaskDetail(taskId, isClick) {
            var url = "../detail.ashx?m=taskdetail&tid=" + taskId;
            var data = RequestData(url);
            var engName = GetColumnDataFromRow(data, "employee_employeename");
            var engId = ConvertToInt(GetColumnDataFromRow(data, "employee_employeeid"));
            var statusId = GetColumnDataFromRow(data, "task_statusid");
            var assigndate = ConvertToDateTime(GetColumnDataFromRow(data, "task_assigneddate"));
            var closeddate = ConvertToDateTime(GetColumnDataFromRow(data, "task_closeddate"));
            var ttype = GetColumnDataFromRow(data, "tasktype_tasktype");
            var cdate = GetColumnDataFromRow(data, "task_createddate");
            var customerName = GetColumnDataFromRow(data, "client_customername");
            var remarks = GetColumnDataFromRow(data, "task_remarks");
            var status = GetColumnDataFromRow(data, "status_status");
            var subject = GetColumnDataFromRow(data, "task_subject");
            var description = GetColumnDataFromRow(data, "task_description");

            if (engId == 0 || isClick == false) {
                engId = dropCtrl.closest(".maintr").attr("eid");
                engName = dropCtrl.closest(".maintr").attr("nm");
            }
            if (isClick == false) {
                //if (assigndate.indexOf("-00") > 0) {
                    assigndate = $(".d" + dropCtrl.attr("colid")).text();
                //}
            }
            if (closeddate.indexOf("-00") > 0) {
                closeddate = "";
            }
            $(".taskstatus").val(statusId);
            $(".assignedtoeng").val(engName);
            $(".assignedtoengid").val(engId);
            $("#assigndate").val(assigndate);
            $("#ttype").text(ttype);
            $("#cdate").text(cdate);
            $("#customer").text(customerName);
            $("#assigndate").datepicker("enable");
            $("#taskid").val(taskId);
            $("#engid").val(engId);
            $("#remarks").val(remarks);
            $(".taskstatus").text(status);
            $("#subject").text(subject);
            $("#description").text(description);
            $("#closeddate").val(closeddate);
            $("#closeddate").datepicker("enable");
        }
        $(function() {
            $("#dialog").dialog({
                autoOpen: false,
                width: 1000,
                show: {
                    effect: "blind",
                    duration: 500
                },
                hide: {
                    effect: "explode",
                    duration: 500
                }
            });

            $(".savetask").click(function() {
                var statusId = ConvertToInt($(this).attr("sid"));
                if (ConvertToInt($(".assignedtoengid").val()) == 0 && $("#assigndate").val() == "") {
                    statusId = 1; //open
                }
                if ($(this).val() != "Save") {
                    if (confirm("Are you sure you want to update task status to " + $(this).val()) == false) return false;
                }
                $("#statusid").val(statusId);
                if (ConvertToInt($(".assignedtoengid").val()) > 0 && $("#assigndate").val() == "") {
                    alert("Please enter assign date");
                    $("#assigndate").focus();
                    return false;
                }
                if (ConvertToInt($(".assignedtoengid").val()) == 0 && $("#assigndate").val().trim() != "") {
                    alert("Please select assigned to!");
                    $(".assignedtoeng").focus();
                    return false;
                }
                if (statusId == 4) {
                    if ($("#closeddate").val().trim() == "") {
                        alert("Please enter closed date");
                        $("#closeddate").focus();
                        return false;
                    }
                }
                var URL = "../savetask.ashx?sid=" + statusId;
                var Data = GetJsonInputData("stask");
                var newId = 0;
                $.ajax({
                    url: URL,
                    type: "POST",
                    async: false,
                    data: Data,
                    success: function(msg) {

                        newId = ConvertToInt(msg);
                        if (newId <= 0) {
                            alert("Error occurred while saving data, please try again!");
                        }
                        else {
                            location.reload();
                            if ($(".taskstatus").val() == "4") {
                                trDrag.remove();
                            }
                            else {
                                dropCtrl.find(".tbltask").append(trDrag);
                                dropCtrl.find(".tbltask").find(".trnotask").remove();
                                if (tblSource != undefined) {
                                    if (tblSource.find(".trtask").length == 0) {
                                        tblSource.html("<tr class='trnotask trtask'><td>No Task</td></tr>");
                                    }
                                }
                            }
                            $(".ui-dialog-titlebar-close").trigger("click");
                            $("#remarks").val("");
                        }
                    },
                    error: function(msg) {
                        alert("Error occurred.");
                    }
                });
            });
            $(".task-left-arr").click(function() {
                $(".hdnisleft").val("1");
                $("form").submit();
            });
            $(".task-right-arr").click(function() {
                $(".hdnisleft").val("2");
                $("form").submit();
            });
            $(".add-eng").click(function() {
                $(".tdadd-eng").toggle();
                $(".tdadd-eng").find(".ac").focus();
            });
            $("#lnkdetail").click(function() {
                if (selectedmodule == "") {
                    var url = "../task/add.aspx?id=" + selectedTaskId;
                    window.open(url);
                }
                else {
                    if (ConvertToInt(selectedmid) > 0) {
                        var url = "../" + selectedmodule + "/add.aspx?id=" + selectedmid;
                        window.open(url);
                    }
                }
                return false;
            });
            //            $(window).scroll(function() {
            //                var scrollTop = $(window).scrollTop();
            //                $(".opentasks-panel").css("top", scrollTop + "px");
            //            });

        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="saveDialog" title="Task Allocation" class="hidden">
        <input type="text" name="taskid" id="taskid" class=" stask hidden"/>
        <input type="text" name="statusid" id="statusid" class=" stask hidden"/>
        <input type="text" name="engid" id="engid" class=" stask hidden"/>
        <table width="100%">
            <tr>
                <td style="vertical-align:top;">
                   <table>
                    <tr><td>Client</td><td class="val" id="customer"></td></tr>
                    <tr><td>Type</td><td class="val ttype" id="ttype"></td></tr>
                    <tr><td>Status</td><td class="taskstatus val"></td></tr>
                    <tr><td>Subject</td><td class="val" id="subject"></td></tr>
                    <tr><td>Description</td><td class="val" id="description"></td></tr>
                    <tr>
                        <td width="150">Assigned To</td>
                        <td>
                            <asp:TextBox ID="engineer" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac assignedtoeng"></asp:TextBox>
                            <asp:TextBox id="engineerid"  Text="0" runat="server" class=" hdnac assignedtoengid stask"/><img src="../images/down-arr.jpg" class="quick-menu epage"/>            
                        </td>
                    </tr>   
                    <tr><td>Assign Date <span class="error">*</span></td><td><input type="text" class="datetimepicker textbox stask" id="assigndate" name="assigndate"/></td></tr>
                    <tr><td>Remarks</td><td><textarea name="remarks" id="remarks" class="textarea stask" maxlength="300"></textarea></td></tr>
                    <tr><td></td><td><input type="button" id="btnsave" value="Save" sid="3" class="button savetask" /></td></tr>
                    <tr><td></td><td>
                    <a href="#" id="lnkdetail"  style='color:#0000ff' target="_blank">Detail</a>
                    </td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr class="trbottom">
                        <td>Close Date <span class="error">*</span></td>
                        <td><input type="text" class="datetimepicker textbox stask" id="closeddate" name="closeddate"/></td>
                    </tr>
                    <tr class="trbottom"><td colspan="2" align="center"><input type="button" id="btnCloseTask" sid="4" value="Close Task" class="button btngreen savetask" />
                        <input type="button" id="btnOpen" value="ReOpen Task" sid="1" class="button btnred savetask" />
                        <input type="button" id="btnHold" value="Hold" sid="2" class="button btnorange savetask" /></td>
                    </tr>
                </table>
                </td>
                
            </tr>
         </table>   
    </div>
        
    <table width="100%">
    <tr>
        <td width="250" class="valign">
            <div style="position:fixed;top:0px;overflow:scroll;" class="divLeftPanel">
            <table width="100%">
                <tr>
                    <td>Task&nbsp;Type</td>
                    <td><asp:DropDownList ID="ddlTaskType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTaskType_Changed"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Literal ID="ltOpenTask" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            </div>
        </td>
        <td>
            <table width="100%">
                <tr>
                    <td colspan="3">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>Employee Type</td>
                                            <td><asp:DropDownList ID="ddlEmployeeType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployeeType_Changed"></asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="task-left-arr"><asp:TextBox ID="hdnIsLeft" runat="server" CssClass="hidden hdnisleft"/></td>
                    <td valign="top"><asp:Literal ID="ltTaskByDate" runat="server"></asp:Literal></td>
                    <td class="task-right-arr">&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
    </form>
</body>
</html>
