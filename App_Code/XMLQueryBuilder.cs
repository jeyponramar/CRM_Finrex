using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

/// <summary>
/// Summary description for XMLQueryBuilder
/// </summary>
public static class XMLQueryBuilder
{
    public static string bulidXMLQuery(string query)
    {
        bool isSessionVarExists = true;
        int stindex = -1;
        int enindex;
        while (isSessionVarExists)
        {
            stindex = query.IndexOf("$SESSION_");
            if (stindex > 0)
            {
                enindex = query.IndexOf("$", stindex + 1);
                string variable = query.Substring(stindex + 9, enindex - stindex - 9);
                string value =(CustomSession.Session(variable));
                value = (value=="")?"0":value;
                query = query.Replace("$SESSION_" + variable + "$", value);
            }
            else
            {
                isSessionVarExists = false;
            }
        }
        bool isConstantExists = true;
        stindex = 0;
        enindex = 0;
        while (isConstantExists)
        {
            stindex = query.IndexOf("$CONSTANT_");
            if (stindex > 0)
            {
                enindex = query.IndexOf("$", stindex + 1);
                string variable = query.Substring(stindex + 10, enindex - stindex - 10);
                string value = CustomSession.Session(variable);
                value = (value=="")?"0":value;
                query = query.Replace("$SESSION_" + variable + "$",value );
            }
            else
            {
                isConstantExists = false;
            }
        }
        bool isQueryStringExists = true;
        stindex = 0;
        enindex = 0;
        while (isQueryStringExists)
        {
            stindex = query.IndexOf("$QUERYSTRING_");
            if (stindex > 0)
            {
                enindex = query.IndexOf("$", stindex + 1);
                string variable = query.Substring(stindex + 13, enindex - stindex - 13);
                string value =GlobalUtilities.ConvertToString(HttpContext.Current.Request.QueryString[variable]);
                value = (value=="")?"0":value;
                query = query.Replace("$QUERYSTRING_" + variable + "$",value);
            }
            else
            {
                isQueryStringExists = false;
            }
        }
        return query;
    }
    private static string getFormControlKeyValue(Control form, string Key)
    {
        string strValue = "";
        if (form != null)
        {
            if (Key.IndexOf("txt") >= 0)
            {
                TextBox txtControl = (TextBox)form.Parent.FindControl(Key);
                strValue = txtControl.Text;
                return strValue;
            }
            else if (Key.IndexOf("h_") >= 0)
            {
                TextBox txtControl = (TextBox)form.Parent.FindControl(Key);
                strValue = txtControl.Text;
                return strValue;
            }
            else if (Key.IndexOf("ddl") >= 0)
            {
                DropDownList ddl = (DropDownList)form.Parent.FindControl(Key);
                strValue = ddl.SelectedValue;
                return strValue;
            }
            else
            {
                Key = "h_" + Key;
            }
            if (Key.IndexOf(Key) >= 0)
            {
                TextBox txtControl = (TextBox)form.Parent.FindControl(Key);
                strValue = txtControl.Text;
            }
        }
        return strValue;
    }
    public static string bulidXMLQuery(Control form, string query)
    {
        bool isPageConstantsExists =true;
        int startindex = -1;
        int endindex;
        if (GlobalUtilities.ConvertToString(HttpContext.Current.Request.QueryString["id"])!="")
        {
            while (isPageConstantsExists)
            {
                startindex = query.IndexOf("$FRM_", StringComparison.CurrentCultureIgnoreCase);
                if (startindex > 0)
                {

                    endindex = query.IndexOf('$', (startindex + 1));
                    string strKey = query.Substring(startindex + 5, (endindex - (startindex + 5)));
                    query = query.Replace("$FRM_" + strKey + "$", getFormControlKeyValue(form, strKey));
                }
                else
                {
                    isPageConstantsExists = false;
                }
            }
        }
        return bulidXMLQuery(query);
    }     
}
