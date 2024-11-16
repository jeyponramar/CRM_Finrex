function deletedetail(frm) { 
            var setting = frm.find(".g_deletesetting");                
            var m = getParam(setting, "m");           
            var sm = getParam(setting, "sm");
            var ic = getParam(setting, "ic");
            var ashxurl = "delete.php?m=" + m + "&sm="+sm+"&ic=" + ic + "&id=" + QueryString("id");// + "&param=" + param;
            var prejsndata = RequestData(ashxurl);
            alert("Data deleted"); 
}

