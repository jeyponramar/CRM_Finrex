<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="bagridviewadmin.aspx.cs" Inherits="bagridviewadmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../css/bagrid-common.css?v=1.0" rel="stylesheet" type="text/css" />
    <script>
        var _txtba = null;
        $(document).ready(function() {
            adjustDatePickerAlways();
            $("#gridpanel").css("width", $(window).width());
            $("#gridpanel").css("height", $(window).height());
            $("#dtcommon").datepicker({
                dateFormat: 'dd-mm-yy',
                onSelect: function(dateText, inst) {
                    _txtba.val(dateText);
                    _txtba.closest("tr").attr("changed", "true");
                    _txtba.focus();
                    $("#dtcommon").hide();
                }
            });
            $(".tdlnkadvsearch").click(function() {
                $(".tradvsearch").show();
            });
            $(".ba-btnaddnew").click(function() {
                var tr = $(".bagrid-template").clone();
                tr.removeClass("bagrid-template");
                tr.insertAfter($(".bagrid-header"));
                tr.show();
                tr.removeClass("bagrid-row").addClass("bagrid-row");
                tr.find("td:first").find(".txtba").focus();
                tr.find(".ac").each(function() {
                    setAutoComplete($(this));
                });
            });
            $(".bagrid-footer").click(function() {
                var tr = $(".bagrid-template"); //$("<tr class='bagrid-row'>" + $(".bagrid-template").html() + "</tr>");
                tr.insertBefore($(this));
                //tr.find(".datepicker").datepicker({ dateFormat: 'dd-mm-yy' });
                tr.show();
                tr.removeClass("bagrid-row").addClass("bagrid-row");
                tr.find("td:first").find(".txtba").focus();
            });
            $(".bagrid-row").find("td").live("click", function() {
                $(this).find(".txtba").show();
                $(this).find(".hidden").hide();
                if ($(this).find(".lblba").length > 0) {
                    $(this).closest("tr").find(".txtba:first").focus();
                }
            });
            $(".txtba").live("keypress", function(e) {
                if (e.keyCode != 13) return;
                /*if ($(this).attr("ir") == "1") {
                if ($(this).val().trim() == "") {
                $(this).focus();
                }
                }*/
                var index = $(this).parent().index();
                var ctrow = $(this).closest("tr");
                var isvalid = true;
                if (ConvertToInt(ctrow.find("#txtba_leadsheetstatusid").val()) == 0) {
                    ctrow.find("#txtba_leadsheetstatusid").val("1");
                    ctrow.find("#ba_leadsheetstatusid").val("Active");
                }
                ctrow.find(".txtba").each(function() {
                    if (isvalid) {
                        if ($(this).attr("ir") != undefined) {
                            if ($(this).val().trim() == "") {
                                isvalid = false;
                                $(this).focus();
                                return false;
                            }
                        }
                    }
                });
                if (!isvalid) return false;
                /*
                if (ctrow.next().attr("class").indexOf("bagrid-footer") >= 0) {//last row
                if (ctrow.attr("changed") == "true") {
                saveba(ctrow, true);
                }
                else {
                var trnew = $("<tr class='bagrid-row'>" + $(".bagrid-template").html() + "</tr>");
                trnew.insertAfter(ctrow);
                trnew.find(".txtba:first").focus();
                trnew.find(".ac").each(function() {
                setAutoComplete($(this));
                });
                }
                }
                else {*/
                if (ctrow.attr("changed") == "true") {
                    saveba(ctrow, false);
                }
                else {
                    if (ctrow.next().length > 0) {
                        ctrow.next().find("td:eq(" + index + ")").find(".txtba").focus();
                    }
                }
                ctrow.attr("changed", "false");
                //}
                return false;

            });
            $(".txtba").live("keydown", function(e) {
                var index = $(this).parent().index();
                var ctrow = $(this).closest("tr");
                if (e.keyCode == 36)//HOME
                {
                    ctrow.find(".txtba:first").focus();
                }
                if (e.keyCode == 35)//END
                {
                    ctrow.find(".txtba:last").focus();
                }
                if (e.keyCode == 38) {//UP
                    if (ctrow.prev().attr("class").indexOf("bagrid-header") < 0) {//last row
                        ctrow.prev().find("td:eq(" + index + ")").find(".txtba").focus();
                    }
                }
                else if (e.keyCode == 40) {//UP
                    if (ctrow.next().attr("class").indexOf("bagrid-footer") < 0) {//last row
                        ctrow.next().find("td:eq(" + index + ")").find(".txtba").focus();
                    }
                }
                else if (e.keyCode == 13) {//ENTER
                }
                else {
                    ctrow.attr("changed", "true");
                }
            });

            $(".txtba").live("focus", function() {
                $(this).closest("td").css("border", "solid 2px #0867B1");
                $("#dtcommon").hide();
                _txtba = $(this);
                if ($(this).attr("class").indexOf("val-dt") >= 0) {
                    $("#dtcommon").css("left", $(this).position().left - 10);
                    $("#dtcommon").css("top", $(this).position().top + 30);
                    var dt = $(this).val();
                    if (dt != "") {
                        var arr = dt.split('-');
                        if (arr.length == 3) {
                            var m = ConvertToInt(arr[1]) - 1;
                            var cdt = new Date(arr[2], m, arr[0]);
                            $("#dtcommon").datepicker("setDate", cdt);
                        }
                    }
                    $("#dtcommon").show();
                }
            });

            $(".txtba").live("blur", function() {
                $(this).closest("td").css("border", "solid 1px #aaa");
                $(this).closest("td").css("border-top", "0px");
                $(this).closest("td").css("border-left", "0px");
                /*if ($(this).attr("ir") == "1") {
                if ($(this).val().trim() == "") {
                $(this).focus();
                }
                }*/
            });
            $(".ac").live("blur", function() {
                if ($(this).attr("ir") == undefined) return;
                if ($(this).parent().find(".hdnac").val() == "") {
                    $(this).focus();
                    return false;
                }
            });
            $(".ba-delete").live("click", function() {
                if (confirm("Are you sure you want to delete this data?")) {
                    var m = $(this).closest(".bagrid").attr("m");
                    var tr = $(this).closest("tr");
                    var id = ConvertToInt(tr.find(".hdnbaid").val());
                    if (id == 0) {
                        tr.remove();
                        alert("Data deleted successfully!");
                    }
                    else {
                        var URL = "../bagridadmin.ashx?m=" + m + "&id=" + id + "&a=d";
                        $.ajax({
                            isAsync: false,
                            url: URL,
                            success: function(response) {
                                if (response == "Session Expired") {
                                    alert("Your session has expired, please login now!");
                                    window.location = "../adminlogin.aspx";
                                }
                                else if (response.indexOf("Error") == 0) {
                                    alert("Error occurred!");
                                }
                                else {
                                    tr.remove();
                                    alert("Data deleted successfully!");
                                }
                            }
                        });
                    }
                }
            });
            /*$("#txtba_pcfcdate-dt,#txtba_days-i").live("blur", function() {
            var tr = $(this).closest("tr");
            var pcfcdate = tr.find("#txtba_pcfcdate-dt").val();
            if (pcfcdate == "") return;
            var days = ConvertToInt(tr.find("#txtba_days-i").val());
            var arr = pcfcdate.split('-');
            var y = ConvertToInt(arr[2]);
            var m = ConvertToInt(arr[1]);
            var d = ConvertToInt(arr[0]);
            var pcduedate = new Date(y, m, d);
            pcduedate.setDate(pcduedate.getDate() + parseInt(days));
            d = pcduedate.getDay();
            m = pcduedate.getMonth();
            y = pcduedate.getFullYear();
            if (d.length == 1) d = "0" + d;
            if (m.length == 1) m = "0" + m;
            tr.find("#txtba_pcduedate-dt").val(d + "-" + m + "-" + y);
            });*/
        });
        function exportorder_calc() {
            $(".txtba").live("blur", function(e) {
                var tr = $(this).closest("tr");
                var exportorderamount = ConvertToDouble(tr.find("#txtba_exportorderamount-dbl").val());
                var amountreceived = ConvertToDouble(tr.find("#txtba_amountreceived-dbl").val());
                var costing = ConvertToDouble(tr.find("#txtba_costing-dbl").val());
                var forwardbookingamount = ConvertToDouble(tr.find("#txtba_forwardbookingamount-dbl").val());
                var forwardbookingrate = ConvertToDouble(tr.find("#txtba_forwardbookingrate-dbl").val());
                var pcfcamount = ConvertToDouble(tr.find("#txtba_pcfcamount-dbl").val());
                var pcfcrate = ConvertToDouble($("#txtba_pcfcrate-dbl").val());

                var netamount = 0;
                if (amountreceived <= exportorderamount) {
                    netamount = exportorderamount - amountreceived;
                }
                tr.find("#txtba_netamount-dbl").val(netamount);
                var value = netamount * costing;
                value = value.toFixed(2);
                tr.find("#txtba_value-dbl").val(value);

                var unhedgedamount = netamount - forwardbookingamount - pcfcamount;
                unhedgedamount = unhedgedamount.toFixed(2);
                tr.find("#txtba_unhedgedamount-dbl").val(unhedgedamount);

                var effectiverate = (forwardbookingamount * forwardbookingrate + pcfcamount * pcfcrate) / (forwardbookingamount + pcfcamount);
                effectiverate = effectiverate.toFixed(2);
                tr.find("#txtba_effectiverate-dbl").val(effectiverate);

            });

        }
        function forwardcontract_calc() {
            $(".txtba").live("blur", function(e) {
                var tr = $(this).closest("tr");
                var sold = ConvertToDouble(tr.find("#txtba_sold-dbl").val());
                var utilized = ConvertToDouble(tr.find("#txtba_utilised-dbl").val());
                var balancesold = 0;
                if (utilized <= sold) {
                    balancesold = sold - utilized;
                }
                balancesold = balancesold.toFixed(2);
                tr.find("#txtba_balancesold-dbl").val(balancesold);
                var rate = ConvertToDouble(tr.find("#txtba_rate-dbl").val());
                var soldamountinRs = balancesold * rate;
                soldamountinRs = soldamountinRs.toFixed(2);
                tr.find("#txtba_soldamountinrs-dbl").val(soldamountinRs);

                var mtmrate = 0;
                
            });

        }
        function pcfc_calc() {
            $(".txtba").live("blur", function(e) {
                var tr = $(this).closest("tr");
                var fcamount = ConvertToDouble(tr.find("#txtba_fcamount-dbl").val());
                var repayment = ConvertToDouble(tr.find("#txtba_repayment-dbl").val());
                var fcamountbalance = fcamount - repayment;
                fcamountbalance = fcamountbalance.toFixed(2);
                tr.find("#txtba_fcamountbalance-dbl").val(fcamountbalance);
                var repayment = ConvertToDouble(tr.find("#txtba_repayment-dbl").val());
                var spotrate = ConvertToDouble(tr.find("#txtba_sellingspotrate-dbl").val());
                var product = spotrate * fcamountbalance;
                product = product.toFixed(2);
                tr.find("#txtba_product-dbl").val(product);

            });
        }
        function saveba(tr, isLastRow) {
            $(".txtba").each(function() {
                $(this).removeAttr("name");
            });
            $(".hdnbaid").removeAttr("name");
            var txthdn = tr.find(".hdnbaid");
            txthdn.attr("name", txthdn.attr("id"));
            
            tr.find(".txtba").each(function() {
                $(this).attr("name", $(this).attr("id"));
            });
            var m = $(".bagrid").attr("m");
            var URL = "../bagridadmin.ashx?m=" + m + "&id=" + tr.find(".hdnbaid").val();
            $.ajax({
                type: "POST",
                url: URL,
                isAsync: false,
                data: $("form").serialize(),
                success: function(response) {
                    $("#dtcommon").hide();
                    if (response == "Session Expired") {
                        alert("Your session has expired, please login now!");
                        window.location = "../customerlogin.aspx";
                    }
                    var divmsg = null;
                    if ($(".bagrid-msg").length == 0) {
                        $("body").append("<div class='bagrid-msg'></div>");
                    }
                    divmsg = $(".bagrid-msg");
                    if (response.indexOf("Error :") == 0) {
                        divmsg.removeClass("bagrid-msg-success").removeClass("bagrid-msg-err").addClass("bagrid-msg-err");
                        divmsg.text(response);
                    }
                    else if (response.indexOf("Error") == 0 || response == "-1" || response == "0") {
                        //divmsg.attr("background-color", "#ff0000");
                        if (response == "-1") {
                            divmsg.text("Duplicate entry not allowed!");
                            alert("Duplicate entry not allowed!");
                        }
                        else {
                            divmsg.text("Error occurred!");
                            alert("Error occurred!");
                        }
                        divmsg.removeClass("bagrid-msg-success").removeClass("bagrid-msg-err").addClass("bagrid-msg-err");
                    }
                    else {
                        var data = jQuery.parseJSON(response);
                        var newid = data["id"];
                        tr.find(".hdnbaid").val(newid);
                        tr.find("label").each(function() {
                            var columnName = m + $(this).attr("id").replace("lblba", "");
                            $(this).text(data[columnName]);
                        });

                        divmsg.removeClass("bagrid-msg-success").removeClass("bagrid-msg-err").addClass("bagrid-msg-success");
                        divmsg.text("Data saved successfully");
                        alert("Data saved successfully!");
                        if (isLastRow) {
                            var trnew = $("<tr class='bagrid-row'>" + $(".bagrid-template").html() + "</tr>");
                            trnew.insertAfter(tr);
                            trnew.find(".txtba:first").focus();
                            trnew.find(".ac").each(function() {
                                setAutoComplete($(this));
                            });
                        }
                    }
                    divmsg.css("left", $(window).width() / 2 - divmsg.width() / 2);
                    divmsg.show();
                    setTimeout(function() { $(".bagrid-msg").hide(); }, 2000);
                }
            });
        }
        function adjustDatePickerAlways() {
            setTimeout(function() {
                adjustDatePickerAlways();
            }, 500);
            if ($("#dtcommon").css("display") == "none") return;
            $("#dtcommon").css("left", _txtba.position().left - 10);
            $("#dtcommon").css("top", _txtba.position().top + 30);
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="dtcommon" style="display:none;position:absolute;"></div>
    <div style="width:500px;height:350px;overflow:scroll;" id="gridpanel">
    <table width="100%">
        <tr id="tradvsearch" class="tradvsearch">
            <td>
                <table>
                    <tr>
                        <td>Date</td>
                        <td>
                            <asp:TextBox ID="search_txtfromdate" runat="server" CssClass="datepicker"></asp:TextBox>&nbsp;
                            <asp:TextBox ID="search_txttodate" runat="server" CssClass="datepicker"></asp:TextBox>&nbsp;
                        </td>
                        <td>Company</td>
                        <td><asp:TextBox ID="search_txtcompany" runat="server" CssClass="mbox"></asp:TextBox></td>
                        <td>Assign To</td>
                        <td>
                            <asp:TextBox ID="search_employee" runat="server" CssClass="mbox ac" m="employee" cn="employeename"></asp:TextBox>
                            <asp:TextBox ID="search_txtemployeeid" runat="server" CssClass="hdnac"></asp:TextBox>
                        </td>
                        <td>Status</td>
                        <td><asp:DropDownList ID="search_ddlstatus" runat="server" Width="70">
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Dead" Value="2"></asp:ListItem>
                        </asp:DropDownList></td>
                        <td>
                            Keyword : <asp:TextBox ID="search_txtkeyword" runat="server" Width="100"></asp:TextBox>
                        </td>
                        <td><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnsearch_click" style="background-color: #eee;padding: 10px;height: 30px;line-height: 10px;"/></td>
                        <td>&nbsp;</td>
                        <td><asp:ImageButton ID="imgExportExcel" OnClick="imgExportExcel_Click" runat="server" ImageUrl="~/images/excel.png" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td><table><tr><td style='color:#035081;height:20px;cursor:pointer;' class="ba-btnaddnew">Add New</td>
                <%--<td style="padding-left:30px;" id="tdlnkadvsearch" runat="server" class="tdlnkadvsearch">
                <a href="#" style="text-decoration:none;color:#035081;">Advanced search</a>
                </td>--%>
            </tr></table></td>
        </tr>
        <tr><td><table width="100%"><tr><td><asp:Literal id="ltbagrid" runat="server"></asp:Literal></td></tr></table></td></tr>
        <tr><td style="padding-left:450px;">
            <table>
                <tr>    
                    <td><asp:Label ID="lblTotalRecords" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td>
                    <ul class="paging">
                    <li class="prev-page">
                        <asp:LinkButton ID="lnkPrevPage" Text="<" ToolTip="Previous Page" runat="server" OnClick="lnkPrevPage_Click"></asp:LinkButton>
                    </li>
                    <asp:Repeater ID="rptPaging" runat="server">
                        <ItemTemplate>
                            <li id="page_td" runat="server"><asp:LinkButton ID="lnkPage" runat="server" OnCommand="lnkPage_OnCommand"></asp:LinkButton></li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li class="next-page"><asp:LinkButton ID="lnkNextPage" Text=">" ToolTip="Next Page" runat="server" OnClick="lnkNextPage_Click"></asp:LinkButton></li>
                    </ul>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPageSize" runat="server" Width="45" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_Changed">
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                            <asp:ListItem Text="80" Value="80"></asp:ListItem>
                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td></tr>
    </table>
    
    </div>
</asp:Content>

