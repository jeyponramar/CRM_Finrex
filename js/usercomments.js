$(document).ready(function(){
    LoadUserComment();
    $(".user-comment").live("click",function(){
        var css = "user-comm-green";
        if($(this).parent().find(".txtuser-comm").length % 2==0)css = "user-comm-red";
        var txt = $("<textarea class='txtuser-comm "+css+"'/>");
        $(this).parent().append(txt);
        txt.focus();
        $(this).hide();
    });
    $("form").submit(function(){
        $(".txtuser-comm").each(function(){
            var dcn = "";
            var txt;
            if($(this).closest("tr").find(".grid").length==1)
            {
                //if it is a sub grid comment
                dcn = $(this).closest("tr").find(".grid").find("input:first").attr("id");
            }
            else
            {
                txt = $(this).parent().find("input:first");
                if(txt == undefined || txt.length==0)txt = $(this).parent().find("textarea:first");
                if(txt == undefined || txt.length==0)return;
                dcn = txt.attr("dcn");
            }
            if(dcn=="" || dcn==null|| dcn==undefined)return;
            if($(this).val()!="")
            {
                var currentComment = dcn + "#@" + _USERID + "#@DATE_TIME#@" + _USERNAME + "#@" + $(this).val();
                var usercomments = $(".usercomments").val();
                var arrComments = usercomments.split('|$|');
                var ctrFound = false;
                for(i=0;i<arrComments.length;i++)
                {
                    var arr = arrComments[i].split('#@');
                    var dcn_tmp = arr[0];
                    
                    if(dcn_tmp==dcn)
                    {
                        arrComments[i] = arrComments[i] + "||" + currentComment;
                        ctrFound = true;
                    }
                }
                
                if(ctrFound)
                {
                    usercomments = "";
                    for(i=0;i<arrComments.length;i++)
                    {
                        if(usercomments=="")
                        {
                            usercomments = arrComments[i]; 
                        }
                        else
                        {
                           usercomments = usercomments + "|$|" + arrComments[i]; 
                        }
                    }  
                }
                else
                { 
                    if(usercomments=="")
                    {
                        usercomments = currentComment;
                    }
                    else
                    {
                        usercomments = usercomments + "|$|" + currentComment;
                    }
                }
            }    
            $(".usercomments").val(usercomments);
        });
    });
});    
function LoadUserComment()
{
    $(".form").find(".val").each(function(){
        var txt = $(this).parent().find("input:first");
        if(txt.length==0)txt = $(this).parent().find("textarea:first");
        if(txt.attr("disabled")!="disabled")
        {
            var img = $("<img src='../images/user-comment.png' class='user-comment hand'/>");
            if($(this).closest(".grid").length==0)
            {
                $(this).parent().append(img);
            }
        }    
    });
    var img2 = $("<img src='../images/user-comment.png' class='user-comment hand'/>");
    $(".form").find(".grid").parent().append(img2);
    var comments = $(".usercomments").val();
    if(comments=="") return;
    var arrComments = comments.split('|$|');
    for(i=0;i<arrComments.length;i++)
    {
        var arr = arrComments[i].split('#@');
        var dcn = arr[0];
        var txt;
        if(dcn.indexOf("ctl00_")==0)
        {
            txt = $("#"+dcn).parent();
        }
        else
        {
            txt = GetDataControl(dcn);
        }
        var html = "";
        var arrComment = arrComments[i].split('||');
        for(j=0;j<arrComment.length;j++)
        {
            var arrComm = arrComment[j].split('#@');
            var comm = arrComm[4];
            var dt = arrComm[2];
            var uname = arrComm[3];
            if(j%2==0)
            {
                html+="<div class='user-comm-redbox' title='"+dt+"'><b>"+uname+" said : </b>"+comm+"</div>";
            }
            else
            {
                html+="<div class='user-comm-greenbox' title='"+dt+"'><b>"+uname+" said : </b>"+comm+"</div>";
            }
            
        } 
        txt.parent().append(html);   
    }
}
