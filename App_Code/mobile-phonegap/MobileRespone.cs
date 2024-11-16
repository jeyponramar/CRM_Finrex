using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for MobileRespone
/// </summary>
public class MobileRespone
{
    public string msg { get; set; }
    public string error { get; set; }
    public string data { get; set; }
    public string script { get; set; }
    public string redirect { get; set; }
}
