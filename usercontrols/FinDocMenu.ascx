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
        var page = pageurl.split('.')[0].replace("view","").replace("add","");
        $(".jq-findocmenu").find("li").each(function(){
            var href = $(this).find("a").attr("href").toString().toLowerCase().replace("view","").replace(".aspx","");
            if(page == href)
            {
                $(this).addClass("findocmenu-active");
            }
        });
    });
</script>
<div>
    <div class="findocmenu" style="margin-right:200px;width:90px;">
        <a href="#" class="add-link " style="background-color:#0b5295;">Settings <i class="icon ion-chevron-down"></i></a>
        <ul class="jq-findocmenu">
            <%--<li><a href="ViewFindoc.aspx">Documents</a></li>--%>
            <li><a href="Viewfindepartment.aspx" class="jq-bankaudit-tab">Department</a></li>
            <li><a href="Viewfincategory.aspx" class="jq-bankaudit-tab">Category</a></li>
            <li><a href="Viewfinsubcategory.aspx" class="jq-bankaudit-tab">Sub Category</a></li>
            <li><a href="Viewfindocumenttype.aspx" class="jq-bankaudit-tab">Type</a></li>
        </ul>
    </div>
</div>
<div>
    
</div>
<%--<table width="100%">
    <tr>
        <td>
            <ul class="bankaudit-leftmenu jq-findocmenu">
                <li><a href="ViewFindoc.aspx">Documents</a></li>
                <li><a href="Viewfindepartment.aspx" class="jq-bankaudit-tab">Document Department</a></li>
                <li><a href="Viewfincategory.aspx" class="jq-bankaudit-tab">Document Category</a></li>
                <li><a href="Viewfinsubcategory.aspx" class="jq-bankaudit-tab">Document Sub Category</a></li>
                <li><a href="Viewfindocumenttype.aspx" class="jq-bankaudit-tab">Document Type</a></li>
            </ul>
        </td>
    </tr>
</table>--%>

