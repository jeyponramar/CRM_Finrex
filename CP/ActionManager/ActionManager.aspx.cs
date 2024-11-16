using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections;
using System.Web.UI.HtmlControls;
using WebComponent;
using System.Data;

public partial class CP_ActionManager : System.Web.UI.Page
{
    GlobalData gbldata = new GlobalData("tbl_module", "module_moduleid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {           
            gbldata.FillDropdown(ddlModule, "module_modulename");            
        }
    }
    protected void btn_generateAction(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToInt(ddlModule.SelectedValue) > 0)
        {
            Session["modulename"] = ddlModule.SelectedItem.Text;
            Response.Redirect("AddAction.aspx?id=" + ddlModule.SelectedValue);
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Please Select Module";
        }
    }
    protected void btn_GenareteCode(object sender, EventArgs e)
    {
        int actionmanagerid = GlobalUtilities.ConvertToInt(Request.QueryString["actionmanagerid"]);
        string query = " SELECT * FROM tbl_actionmanager JOIN tbl_actionmanagerdetail ON actionmanagerdetail_actionmanagerid=actionmanager_actionmanagerid WHERE actionmanager_actionmanagerid="+actionmanagerid;
        DataTable dt = new DataTable();
        dt = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dt))
        {
            string strdesignCode = "";
            string strCSharpCode = "";
            for(int i =0; i<dt.Rows.Count;i++)
            {                
                string strInnerCharpCode = "";
                int actiontypeid = GlobalUtilities.ConvertToInt(dt.Rows[i]["actionmanagerdetail_actionmanageractiontypeid"]);
                string strActionKey = GlobalUtilities.ConvertToString(dt.Rows[i]["actionmanagerdetail_actionvalue"]);
                Array arrActionKey = strActionKey.Split('|');
                strdesignCode = "<asp:Button runat='server' ";
                strCSharpCode = @"  protected void $actionnameclick$(object sender, EventArgs e)" + Environment.NewLine+"" + "  {" + Environment.NewLine;
                for (int j = 0; j < arrActionKey.Length; j++)
                {
                    string strKey = GlobalUtilities.ConvertToString(arrActionKey.GetValue(j)).Trim();
                    if (strKey != "")
                    {
                        string strKeyValue = strKey.Substring(strKey.IndexOf('='), strKey.Length - strKey.IndexOf('=')).Substring(1).Trim();
                        string strtempKey = strKey.Substring(0,strKey.IndexOf('='));
                        if (strtempKey == "buttoncolor")
                        {
                            strdesignCode += " style='color:" + strKeyValue + "'";
                        }
                        else if (strtempKey == "actionname")
                        {
                            strdesignCode += " Text='" + strKeyValue + "' OnClick='btn" + strKeyValue + "_Click' id='btn"+strKeyValue+"'";
                            strCSharpCode = strCSharpCode.Replace("$actionnameclick$", "btn" + strKeyValue + "_Click");
                        }                       
                        else if (strtempKey == "redirecturl")
                        {
                            strKeyValue = (strKeyValue.Contains(".aspx")) ? strKeyValue : strKeyValue + ".aspx";
                            strInnerCharpCode = "       Response.Redirect('"+strKeyValue+"');";
                        }
                        else if (strtempKey == "createcontacts")
                        {
                            string strval = (strKeyValue == "") ? "Form.Controls" : "Form.Controls,'" + strKeyValue + "'";
                            strInnerCharpCode = Environment.NewLine + "         Contacts.createContacts(" + strval + ");";
                        }                        
                    }
                    //Generate Button
                   // strdesignCode = "<asp:Button runat='server' style='color:"++"' ID='btn' OnClick='btn_Click' />";
                    //
                    
                }
                strdesignCode += " />";
                strCSharpCode += Environment.NewLine + strInnerCharpCode;
                strCSharpCode += "" + Environment.NewLine + "   }";
            }
            //strdesignCode+=" />";
            txtDesignCode.Text = strdesignCode.Replace("'","\"");
            txtCharpCode.Text = strCSharpCode.Replace("'", "\""); 
        }
        
    }  
  
    private void generateCode()
    {
        string query = " SELECT * FROM tbl_";

        //ArrayList controlList = new ArrayList();
        //Hashtable hstbl = new Hashtable();
        //GridSettings obj = new GridSettings();
        //obj.Module = "user";
        //obj.BindXml(obj.Module);
        //Array arr = obj.arrColumns;
        ////Contacts Insertion
        //for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        //{
        //    string key = HttpContext.Current.Request.Form.Keys[i].ToLower();
        //    for (int j = 0; j < controlList.Count; j++)
        //    {
        //        string strColumnName = GlobalUtilities.ConvertToString(controlList[j]);
        //        string strTempColumnname = (key.Contains("txt")) ? strColumnName : "txt" + strColumnName;
        //        if (key.ToLower().EndsWith(strTempColumnname.ToLower()))
        //        {
        //            string value = HttpContext.Current.Request.Form[key];
        //            if (!hstbl.Contains(strColumnName))
        //            {
        //                hstbl.Add(strColumnName, value);
        //            }
        //            break;
        //        }
        //    }
        //}
        ////End Contacts Insertion
    }    
}
