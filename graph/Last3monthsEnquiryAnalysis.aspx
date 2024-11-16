<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Last3monthsEnquiryAnalysis.aspx.cs" Inherits="graph_lastthreemonthsenqanalysis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../js/jsapi.js"></script>
    <%--<script type="text/javascript">
        google.load("visualization", "1", { packages: ["columnchart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {
            var data = google.visualization.arrayToDataTable([
          ['Year', 'Sales', 'Expenses'],
          ['2004', 1000, 400],
          ['2005', 1170, 460],
          ['2006', 660, 1120],
          ['2007', 1030, 540]
        ]);

            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
            chart.draw(data, { width: 300, height: 230, is3D: true, title: 'Company Performance' });
        }
    </script>--%>
</head>
<body style="margin:0px">
    <form id="form1" runat="server">
    <!--Div that will hold the pie chart-->
    <div id="chart_div" style="margin:0px"></div>
    </form>
</body>
</html>
