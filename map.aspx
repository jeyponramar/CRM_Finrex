<%@ Page Language="C#" AutoEventWireup="true" CodeFile="map.aspx.cs" Inherits="map" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jcanvas.min.js" type="text/javascript"></script>
    <script>
    var map;
        $(document).ready(function(){
            map = $("#mycanvas");
//            map.drawQuadratic({
//              strokeStyle: "#444444",
//              strokeWidth: 5,
//              x1: 100, y1: 100, // Start point
//              cx1: 170, cy1: 70, // Control point
//              x2: 300, y2: 150 // End point
//            });
//            map.drawQuadratic({
//              strokeStyle: "#444444",
//              strokeWidth: 5,
//              x1: 300, y1: 150, // Start point
//              cx1: 400, cy1: 300, // Control point
//              x2: 500, y2: 400 // End point
//            });
//            map.drawQuadratic({
//              strokeStyle: "#444444",
//              strokeWidth: 5,
//              x1: 500, y1: 400, // Start point
//              cx1: 400, cy1: 300, // Control point
//              x2: 650, y2: 100 // End point
//            });
//               drawMarker(100,100);
//               drawMarker(300,150);
//               drawMarker(500,400);
//               drawMarker(650,100);
//               displayText(100,100,"Dadar Godown");
//               displayText(300,150,"Sion");
//               displayText(500,400,"Malad");
//               displayText(650,100,"Mulund");
            
        });
        function drawMarker(x,y)
            {
                x = x - 5;
                y = y - 25;
                $("body").append("<div style='width:20px;height:37px;left:"+x+"px;top:"+y+"px;position:absolute;'>"+
                                  "<img src='images/marker.png' title='Godown'/></div>");
            }
            function displayText(x,y,msg)
            {
                map.drawText({
                  fillStyle: "#000000",
                  strokeWidth: 1,
                  x: x+25, y: y-10,
                  fontSize: "10pt",
                  maxWidth: 10,
                  fontFamily: "Verdana, sans-serif",
                  text: msg
                });
            }
    </script>
</head>
<body style="background-image:url(images/grid-fg.png);">
    <form id="form1" runat="server">
    <div>
        <canvas id="mycanvas" width=1000 height=1000 style="border:1px solid;"></canvas>
    </div>
    </form>
</body>
</html>
