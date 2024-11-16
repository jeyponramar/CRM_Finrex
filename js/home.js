$(document).ready(function() {
    bindScrollNews();
    setInterval(function(){bindScrollNews();},60000);
});
function bindScrollNews()
{
    ajaxCall("finstationhandler.ashx?m=getscrollnews",function(response){
        if(response == "")
        {
            return;
        }
        else if(response.indexOf("Error:")>=0)
        {
            return;
        }
        else
        {
            if($(".marque-news1").find("marquee").html()!=response)
            {
                $(".marque-news1").html("<marquee>"+response+"</marquee>");
            }
        }
    });
}