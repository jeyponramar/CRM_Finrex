<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FinDocMenu.ascx.cs" Inherits="usercontrols_FinDocMenu" %>
<script>
    $(document).ready(function(){
        var url = window.location.href.toString().toLowerCase();
        var arr = url.split('/');
        var pageurl = "";
        if(url.indexOf("localhost") > 0)
        {
            pageurl = arr[4];
        }
        else
        {
            pageurl = arr[3];
        }
        var page = pageurl.split('.')[0].replace("view","");
        $(".jq-findocmenu").find("li").each(function(){
            var href = $(this).find("a").attr("href").toString().toLowerCase().replace("view","").replace(".aspx","");
            if(page == href)
            {
                $(this).addClass("bankaudit-left-current");
            }
        });
    });
</script>
<table width="100%">
    <tr>
        <td>
            <ul class="bankaudit-leftmenu jq-findocmenu">
                <li class=""><a href="ViewFindoc.aspx" class="">Documents</a></li>
                <li class=""><a href="Viewfindepartment.aspx" class="jq-bankaudit-tab">Document Department</a></li>
                <li class=""><a href="Viewfincategory.aspx" class="jq-bankaudit-tab">Document Category</a></li>
                <li class=""><a href="Viewfinsubcategory.aspx" class="jq-bankaudit-tab">Document Sub Category</a></li>
                <li class=""><a href="Viewfindocumenttype.aspx" class="jq-bankaudit-tab">Document Type</a></li>
            </ul>
        </td>
    </tr>
</table>

