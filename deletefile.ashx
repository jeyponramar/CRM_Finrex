<%@ WebHandler Language="C#" Class="deletefile" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using WebComponent;
public class deletefile : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        Common.ValidateAjaxRequest();    
        string imgsrc=Convert.ToString(context.Request.QueryString["imgsrc"]);
        File.Delete(System.Web.HttpContext.Current.Server.MapPath( imgsrc));
        
        context.Response.Write(1);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}