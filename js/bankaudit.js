$(document).ready(function(){
    setInterval(function(){
        var issaved = saveBankAudit(true);
    },60000);
    $(".jq-bankaudit-tab").click(function(){
        if($(".jq-id").val()=="0")
        {
            return false;
        }
    });
    $(document).live("change","input,textarea,select", function(){
        if($(".jq-issaveenabled").val()=="0")
        {
            //alert("You can not edit!");
            return false;
        }
        $(".jq-ispagemodified").val("1");
    });
    $(document).live("keypress","input,textarea,select", function(){
        if($(".jq-issaveenabled").val()=="0")
        {
            showTopMessage("You can not edit!",false);
            return false;
        }
    });
    $(".jq-chkqueryresolved").click(function(){
        if($(this).is(":checked"))
        {
            if(!confirm("Are you sure that you resolved advisor's query?"))return false;
            var td = $(this).closest("td");
            $(".jq-ispagemodified").val("1");
            td.find(".jq-hdnisqueryresolved").val("1");
            $(this).parent().remove();
            td.find(".bankaudit-remarks").removeClass("bankaudit-remarks");
        }
        else
        {
            $(this).closest("td").find(".jq-hdnisqueryresolved").val("0");
        }
    });
    $(".jq-questinnarie-remarks-advisor").change(function(){
        $(this).closest("td").find(".jq-hdnisqueryresolved").val("0");
    });
    $(".jq-savebankaudit").click(function(){  
        if($(".jq-issaveenabled").val()=="0") return false;  
        $(".jq-lblmessage").text("");
        var issaved = saveBankAudit(false);
        if(issaved)return true;
        return true;
    });
    $(".jq-btnsavebankaudit").click(function(){
        $(".jq-lblmessage").text("");
        $(".jq-lblmessage").hide();
        var issaved = saveBankAudit(false);
        return false;
    });
    $(".jq-btnsendforreview,.jq-btnsubmitreview").click(function(){
        return confirm("Are you sure you want to submit?");
    });
    $(".jq-btncompleteaudit").click(function(){
        return confirm("Are you sure you want to close this audit?");
    });
    $(".jq-addcustomlabel-header").click(function(){
        var tbl = $(this).closest(".jq-tbl-annualturnover-currencywise");
        var header = tbl.find(".grid-ui-header");
        if(header.find(".jq-customlabel-header").length>=5)
        {
            alert("You can not add any more columns!");
            return;
        }
        showDialog($("#divaddcustomlabel-annualturnover"), "Add Custom Field");
    });
    $("#jq-btnsavecustomlebal").click(function(){
        if($(".jq-txtcustomlabel-annualturnover").val().trim()=="")
        {
            $(".jq-txtcustomlabel-annualturnover").focus();
            return;
        }
        saveCustomLabel();
    });
    $(".jq-addbankingcostlabel").click(function(){
        showDialog($("#divaddbankcost-currencylabel"), "Add Banking Cost");
    });
     $("#jq-btnsavecurrencywiselabelbankcost").click(function(){
        if($(".jq-txtcurrencywiselabel-bankcostdetails").val().trim()=="")
        {
            $(".jq-txtcurrencywiselabel-bankcostdetails").focus();
            return;
        }
        saveBankingCostLabel();
    });
});
function saveBankAudit(isautosave)
{
    if(isautosave && $(".jq-ispagemodified").val()=="0")return true;
    if($(".jq-issaveenabled").val()=="0")return true;
    if($(".jq-savebankaudit").length==0)return true;
    $(".jq-ispagemodified").val("0");
    var urlprefix = "../";
    if(__LOGIN_USERID == "0") urlprefix = "";
    var url = urlprefix + "utilities.ashx?m=save-bankaudit&id="+$(".jq-id").val();
    var data = $("input[type=text],select,textarea").not("#__VIEWSTATE").serialize();
    var issaved = false;
    $.ajax({
        url:url,
        type: "POST",
        data:data,
        async:false,
        success:function(response)
        {
            try
            {
               var json = jQuery.parseJSON(response);
               if(json.status == "ok")
               {
                  issaved = true; 
//                  $(".jq-lblmessage").text("Data saved successfully!");
//                  $(".jq-lblmessage").show("slow");
//                  setTimeout(function(){
//                    $(".jq-lblmessage").hide("slow");
//                  },3000);
                  showTopMessage("Data saved successfully!",false);
               }
               else if(json.status == "error")
               {
                  $(".jq-ispagemodified").val("1");
                  alert(json.msg);
               }
            }
            catch(ex)
            {
                $(".jq-ispagemodified").val("1");
//                $(".jq-lblmessage").text("Unable to save the details, please try again.");
//                $(".jq-lblmessage").show("slow");
                showTopMessage("Unable to save the details, please try again.", true);
            }
        }
    });
    return issaved;
}
function saveCustomLabel()
{
    var urlprefix = "../";
    if(__LOGIN_USERID == "0") urlprefix = "";
    var url = urlprefix + "utilities.ashx?m=bankaudit&cm=save-customlabel&type=annualcurrencyturnover&bankauditid="+$(".jq-id").val();
    var data = $("#divaddcustomlabel-annualturnover").find("input[type=text],select,textarea").not("#__VIEWSTATE").serialize();
    $.ajax({
        url:url,
        type: "POST",
        data:data,
        async:false,
        success:function(response)
        {
            try
            {
               var json = jQuery.parseJSON(response);
               if(json.status == "ok")
               {
                  $(".dialog").hide();
                  addCustomLabel($(".jq-txtcustomlabel-annualturnover").val(),json.index);
                  $(".jq-txtcustomlabel-annualturnover").val("");
               }
               else if(json.status == "validation")
               {
                 alert(json.msg);
               }
               else
               {    
                 alert("Error occurred!");
               }
            }catch(ex)
            {
                alert("Unable to save the details, please try again.");
            }
        }
    });
}
function saveBankingCostLabel()
{
    var urlprefix = "../";
    if(__LOGIN_USERID == "0") urlprefix = "";
    var url = urlprefix + "utilities.ashx?m=bankaudit&cm=save-bankcostlabel&type=totalbankingcost&bankauditid="+$(".jq-id").val();
    var data = $("#divaddbankcost-currencylabel").find("input[type=text],select,textarea").not("#__VIEWSTATE").serialize();
    $.ajax({
        url:url,
        type: "POST",
        data:data,
        async:false,
        success:function(response)
        {
            try
            {
               var json = jQuery.parseJSON(response);
               if(json.status == "ok")
               {
                  $(".dialog").hide();
                  addBankCostingLabelRecord($(".jq-txtcurrencywiselabel-bankcostdetails").val(),json.id);
                  $(".jq-txtcurrencywiselabel-bankcostdetails").val("");
               }
               else if(json.status == "validation")
               {
                 alert(json.msg);
               }
               else
               {    
                 alert("Error occurred!");
               }
            }catch(ex)
            {
                alert("Unable to save the details, please try again.");
            }
        }
    });
}
function addCustomLabel(label, index)
{
    var tbl = $(".jq-tbl-annualturnover-currencywise");
    var header = tbl.find(".grid-ui-header");
    var headertd = "<td class='jq-customlabel-header'>"+label+"</td>";
    $(headertd).insertBefore(header.find("td:last"));
    tbl.find("tr").each(function(){
        if($(this).hasClass("grid-ui-header"))return;
        var id = $(this).find(".jq-rowid").val();
        $(this).append("<td><input type='text' class='mbox val-dbl' name='txtbankauditannualcurrencyturnover_custom_"+index+"_"+id+"'/></td>");
    });
}
function addBankCostingLabelRecord(label, id)
{
    var grid = $(".jq-totalbankingcostgrid");
    var newrow = "<tr><td>"+label+"<input type='text' class='hidden' name='txtbankaudityearlybankingcost_id_"+id+"' value='"+id+"'/></td>";
    newrow += "<td><input type='text' class='mbox val-dbl' name='txtbankaudityearlybankingcost_yearlybankcost1_"+id+"'/>";
    newrow += "<td><input type='text' class='mbox val-dbl' name='txtbankaudityearlybankingcost_yearlybankcost2_"+id+"'/>";
    newrow += "<td><input type='text' class='mbox val-dbl' name='txtbankaudityearlybankingcost_yearlybankcost3_"+id+"'/>";
    $(newrow).insertBefore(grid.find("tr:last"));
}
function showTopMessage(msg, iserror)
{
    if($(".top-msg").length==0)
    {
        $("body").append("<div class='top-msg'></div>");
    }
    var divmsg = null;
    if ($(".top-msg").length == 0) {
        $("body").append("<div class='top-msg'></div>");
    }
    divmsg = $(".top-msg");
    divmsg.show();
    if(iserror)
    {
        divmsg.removeClass("top-msg-success").removeClass("top-msg-err").addClass("top-msg-err");
        divmsg.text(msg);
    }
    else
    {
        divmsg.removeClass("top-msg-success").removeClass("top-msg-err").addClass("top-msg-success");
        divmsg.text(msg);
    }
    setTimeout(function(){
        divmsg.hide("slow");
    },5000);
}