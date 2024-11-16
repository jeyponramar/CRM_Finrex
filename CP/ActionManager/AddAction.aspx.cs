using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Collections;
using System.Data;

public partial class CP_ActionManager_AddAction : System.Web.UI.Page
{
    GlobalData gbldata = new GlobalData("tbl_actionmanager", "actionmanager_actionmanagerid");
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void btnSaveAction_Click(object sender, EventArgs e)
    {
        int moduleid = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        string strmodulename = (Session["modulename"] != null) ? GlobalUtilities.ConvertToString(Session["modulename"]) : "";
        string strActionvalue = "";
       // string query = " SELECT * FROM tbl_actionmanager WHERE actionmanager_moduleid="+moduleid;
       // DataRow dr = DbTable.ExecuteSelectRow(query);
        GlobalUtilities.ConvertToInt(Request.QueryString["actionmanagerid"]);
        int actionManagerId = 0;
        if (actionManagerId == 0)
        {
            Hashtable hstblActionManager = new Hashtable();
            hstblActionManager.Add("moduleid", moduleid);
            hstblActionManager.Add("modulename", strmodulename);
            InsertUpdate objactionmanager = new InsertUpdate();
            actionManagerId = objactionmanager.InsertData(hstblActionManager, "tbl_actionmanager", false);
        }
        if (actionManagerId > 0)
        {
            Hashtable hstbl = new Hashtable();
            hstbl.Add("actionmanagerid", actionManagerId);
            hstbl.Add("actionmanageractiontypeid", ddlAction.SelectedValue);
            hstbl.Add("actionmanageractiontype", ddlAction.SelectedItem.Text);
            string strActionManagerKeyValue = "";
            for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
            {
                string key = HttpContext.Current.Request.Form.Keys[i].ToLower();
                int startindex = key.IndexOf("txt");
                if (startindex >= 0)
                {
                    startindex += 3;
                    string strActionKey = key.Substring(startindex, key.Length - startindex);
                    string strColumnName = "";
                    string strTempColumnname = (key.Contains("txt")) ? strColumnName : "txt" + strColumnName;

                    if (key.ToLower().EndsWith(strTempColumnname.ToLower()))
                    {
                        string value = HttpContext.Current.Request.Form[key];
                        //hstbl.Add(strActionKey, value);
                        strActionManagerKeyValue += (strActionManagerKeyValue == "") ? strActionKey + "=" + value : "|"+strActionKey + "=" + value;
                        //break;
                    }
                }
            }
            hstbl.Add("actionvalue", strActionManagerKeyValue);

            InsertUpdate obj = new InsertUpdate();
            obj.InsertData(hstbl, "tbl_actionmanagerdetail", false);
            Context.Response.Redirect("actionmanager.aspx?actionmanagerid="+actionManagerId);
        }
        else
        {
            Context.Response.Write("Error occured");
        }
    }
}
