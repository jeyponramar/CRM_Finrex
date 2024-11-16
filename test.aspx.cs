using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.IO;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //RPlusEmailUtility.SendBulkEmail("", "", "jeyponramar@gmail.com", "", "", "test", "test", "");
        var request = (HttpWebRequest)WebRequest.Create("https://finstation.in/test2.aspx");
        //request.Headers.Add("Authorization", _authKey);
        request.ContentType = "application/json";
        request.Method = "POST";
        string body = "";
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(body);
        }
        var httpResponse = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Response.Write("ok");
        }
    }
}
