function load_mobile(){
    bindLiveRate();
    setInterval(function(){bindLiveRate();},3000);
    initMobile();
    
    document.addEventListener('init', function(event) {
        var page = event.target;
        //alert(page.id);
        if (page.id == 'homepage') {
            //document.querySelector('#myNavigator').pushPage('li.html', { data: { title: 'Page 2'} });
        }
    });
}
function initMobile()
{
    $(document).on("click", "#btnlogin", function(){
        var URL = _RPLUS_API + "/mobile/mobile.ashx?m=login&source=phonegap"; 
        var Data = {"username":$("#username").val(),"password":$("#password").val()};
	    $.ajax({
		    url:URL,    
		    type:"POST",
		    data:Data,
//		     crossDomain: true,
//    	    dataType: 'JSONp',  
		    success:function(json){
			    if(json.indexOf("Error:")==0)
			    {
			        alert(json.replace("Error:",""));
			        return;
			    }
			    $("#menu").show();
			    var url = "liverate.html";
			    document.querySelector('#myNavigator').pushPage(url, { data: {} });
				document.querySelector('ons-splitter-side').close();
		    },
		    error: function (xhr, ajaxOptions, thrownError) {
                alert("Error occurred!");
              }
	    });  
    });
}
function bindLiveRate(){          
	var URL = _RPLUS_API + "/liverate.ashx?a=mobile-liverate&source=phonegap"; 
	$.ajax({
		url:URL,    
		type:"GET",
        crossDomain: true,
	    dataType: 'JSONp',  
		success:function(json){
			json = json.replace(/__NEWLINE__/g,"\n").replace(/__NEWLINER__/g,"\n");
			if (json != "") {
                var data = jQuery.parseJSON(json);
                for (i = 0; i < data.length; i++) {
                    var rid = data[i].rid;
                    var rate = data[i].cr;
                    setUpDownStatus($("#lr" + rid), rate);
                }
            }
		},
		error: function (xhr, ajaxOptions, thrownError) {
            //alert("Error occurred!");
          }
	});  
} 
function setUpDownStatus(obj, rate) {
    if (!isNaN(rate)) {
        rate = parseFloat(rate);
        var prevrate = parseFloat(obj.text());
        if (rate < prevrate) {
            obj.removeClass("rate-down").addClass("rate-mid");
            setTimeout(function() {
                obj.removeClass("rate-mid").addClass("rate-up");
            }, 500);
        }
        else if (rate > prevrate) {
            obj.removeClass("rate-up").addClass("rate-mid");
            setTimeout(function() {
            obj.removeClass("rate-mid").addClass("rate-down");
            }, 500);
        }
    }
    obj.text(rate);
}