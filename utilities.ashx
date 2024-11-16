<%@ WebHandler Language="C#" Class="utilities" %>

using System;
using System.Web;
using System.Data;
using WebComponent;
using System.Xml;
using System.Text;

public class utilities : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        Common.ValidateAjaxRequest();    
        context.Response.ContentType = "text/plain";
        string m = context.Request.QueryString["m"];
        if (m == "deletenote")
        {
            int id = GlobalUtilities.ConvertToInt(context.Request.QueryString["id"]);
            InsertUpdate obj = new InsertUpdate();
            obj.ExecuteQuery("delete from tbl_note where note_noteid=" + id);
            context.Response.Write("1");
        }
        else if (m == "removeallreminder")
        {
            int userid = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            string query = @"update tbl_followups set followups_isremoved=1 where followups_date <= getdate() and followups_isreminder=1 and followups_followupstatusid=1
                            and followups_userid=" + userid + " and isnull(followups_isremoved,0)=0";
            InsertUpdate obj = new InsertUpdate();
            obj.ExecuteQuery(query);
        }
        else if (m == "enable-tab")
        {
            int userId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            string action = context.Request.QueryString["a"];
            if (action == "e")
            {
                action = "1";
            }
            else
            {
                action = "0";
            }
            string query = "update tbl_user set user_ismultitab=" + action + " WHERE user_userid=" + userId;
            if (DbTable.ExecuteQuery(query))
            {
                context.Response.Write("1");
            }
            return;
        }
        else if (m == "outstanding-bills")
        {
            int lid = GlobalUtilities.ConvertToInt(context.Request.QueryString["lid"]);
            StringBuilder html = new StringBuilder();
            html.Append("<table class='repeater' cellspacing=0><tr class='repeater-header'><td>Ref No</td><td>Date</td><td>Total Amount</td><td>Amount Paid</td><td>Balance</td><td>Narration</td></tr>");
            string query = "select * from tbl_ledgervoucher WHERE ISNULL(ledgervoucher_balanceamount,0) > 0 AND ledgervoucher_ledgerid=" + lid;
            DataTable dttblBills = DbTable.ExecuteSelect(query);
            for (int i = 0; i < dttblBills.Rows.Count; i++)
            {
                DataRow dr = dttblBills.Rows[i];
                string css = "repeater-alt";
                if (i % 2 == 0) css = "repeater-row";
                html.Append("<tr class='" + css + "'>" +
                            "<td>" + GlobalUtilities.ConvertToString(dr["ledgervoucher_referenceno"]) + "</td>" +
                            "<td>" + GlobalUtilities.ConvertToDate(dr["ledgervoucher_voucherdate"]) + "</td>" +
                            "<td>" + GlobalUtilities.ConvertToDouble(dr["ledgervoucher_totalamount"]) + "</td>" +
                            "<td>" + GlobalUtilities.ConvertToDouble(dr["ledgervoucher_amountpaid"]) + "</td>" +
                            "<td>" + GlobalUtilities.ConvertToDouble(dr["ledgervoucher_balanceamount"]) + "</td>" +
                            "<td>" + GlobalUtilities.ConvertToString(dr["ledgervoucher_narration"]) + "</td></tr>");
            }
            html.Append("</table>");
            context.Response.Write(html.ToString());
        }
        else if (m == "save-bankaudit")
        {
            BankAudit obj = new BankAudit();
            obj.Save();
        }
        else if (m == "bankaudit")
        {
            BankAudit obj = new BankAudit();
            obj.Process();
        }
        else if (m == "onesignal-suscribe")
        {
            OneSignalPushNotification obj = new OneSignalPushNotification();
            obj.SaveOnesignalSubscription_ClientUser(1);
        }
        else if (m == "pushnotification")
        {
            FinrexPushNotification obj = new FinrexPushNotification();
            obj.ProcessRequest();
        }
        string mm = context.Request.QueryString["mm"];
        string execError = "";
        if (mm == "compiler")
        {
            //html.Append("<td><a href='#' class='fixerror' url=getdata.ashx?mm=compiler&" + Convert.ToString((url == "") ? " error=CCMissing&m=" + module + "&mid=" + moduleid + "cn=" + columnName + "" : url) + ">Fix this Error</a></td></tr>");
            string error = getQueryString("error");
            string strColumnName = getQueryString("cn");
            strColumnName = getQueryString("ColName");
            string Module = getQueryString("m");
            int ModuleId = GlobalUtilities.ConvertToInt(getQueryString("mid"));
            Array arrColumnName = strColumnName.Split(',');
            try
            {
                for (int i = 0; i < arrColumnName.Length; i++)
                {
                    strColumnName = GlobalUtilities.ConvertToString(arrColumnName.GetValue(i));
                    string query = "";
                    if (error == "RequiredField")
                    {
                        DbTable.ExecuteQuery("UPDATE tbl_columns SET columns_isrequired=1 WHERE columns_columnname='" + strColumnName + "'");
                    }
                    else if (error == "UniqueField")
                    {
                        if (arrColumnName.Length > 0)
                        {
                            query = @"ALTER TABLE tbl_" + Module + " ADD CONSTRAINT Unique_" + strColumnName + " UNIQUE (" + strColumnName + ")";
                            if (DbTable.ExecuteQuery(query))
                            {
                                DbTable.ExecuteQuery("UPDATE tbl_columns SET columns_isunique=1 WHERE columns_columnname='" + strColumnName + "'");
                                execError = "Success!!!!!";
                            }
                            else
                            {
                                execError = "Error Occured!!!!!!!";
                            }

                        }
                    }
                    else if (error == "ViewColumn" || error == "Searchcolumn")
                    {
                        BindXmlNode(error, Module, strColumnName);
                    }
                    else if (error == "CCMissing")//ColorColumn Missing
                    {
                        query = "ALTER TABLE tbl_" + Module + " ADD " + strColumnName + " VARCHAR(100)";
                        if (DbTable.ExecuteQuery(query))
                        {
                            execError = "Success!!!!!";
                        }
                        else
                        {
                            execError = "Error Occured!!!!!!!";
                        }
                    }                    
                }
            }
            catch(Exception ex)
            {
                execError = "Error:" + ex.ToString();
            }

            context.Response.Write(execError);
        }
        else if (mm == "dbscript")
        {
            try
            {
                bool isNewObj = GlobalUtilities.ConvertToBool(getQueryString("isNewObj"));
                int ScriptId = GlobalUtilities.ConvertToInt(getQueryString("id"));
                string Objname = getQueryString("Objname");                
                string strObjDefinition = getQueryString("obj_def");
                string Objdef = strObjDefinition;
                string Datatype = getQueryString("Datatype");
                string strObj_Definition = getQueryString("ColName");
                string objtype = getQueryString("objtype");
                
                //DbTable.ExecuteQuery(strObjDefinition);
                if (objtype.ToLower() == "cn")
                {
                    string modulenamne = objtype.Substring(0, objtype.IndexOf('_') - 1);
                    if (isObjExists(objtype, Objname))
                    {
                        DbTable.ExecuteQuery(" ALTER TABLE tbl_" + modulenamne + " ALTER COLUMN " + Objname + " " + Datatype);
                    }
                    else
                    {
                        if (objtype.IndexOf('_') > 0)
                        {
                            DbTable.ExecuteQuery(" ALTER TABLE tbl_" + modulenamne + " ADD COLUMN " + Objname + " " + Datatype);
                        }
                    }
                }
                //else if (objtype.ToLower() != "u")
                else if (Objdef.Trim() != "")
                {
                    string def = Objdef.Replace("&nbsp;", "\t").Replace("~", "'").Replace("<br/>", "\n");
                    string type = getQueryString("type").ToUpper() + " ";
                    if (isNewObj)
                    {
                        def = def.Replace("ALTER " + type, "CREATE " + type);
                    }
                    else
                    {
                        def = def.Replace("CREATE " + type, "ALTER " + type);
                    }
                    DbTable.ExecuteQuery(def);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.ToString());
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    private string getQueryString(string name)
    {
        return GlobalUtilities.ConvertToString(HttpContext.Current.Request.QueryString[name]);
    }
    private void BindXmlNode(string strType, string strModuleName, string columnName)
    {
        string strRootNode = "/setting/";
        if (strType == "ViewColumn")
        {
            string strFileName = XMLNodeBinder.getXMLFilePath(strModuleName,"view/"+strModuleName.ToLower().Trim());
            System.Xml.XmlDocument doc = XMLNodeBinder.getXMLDocument("view", strModuleName);
            string str_columns = "<column>" +
                         "<name>" + columnName + "</name>" +
                         "<headertext>" + DbTable.GetOneColumnData("tbl_columns", "columns_gridcolumnname", "columns_columnname='" + columnName + "'") + "</headertext>" +
                         "<row>1</row>" +
                         "<width>10%</width>" +
                         "<format></format>" +
                     "</column>";
            bindXmlNodeText(doc.SelectSingleNode("/setting/gridcolumn"), str_columns, true);
            doc.Save(strFileName);
            DbTable.ExecuteQuery("UPDATE tbl_columns SET columns_isviewpage=1 WHERE columns_columnname='" + columnName + "'");
        }
        if (strType == "Searchcolumn")
        {
            string strFileName = XMLNodeBinder.getXMLFilePath(strModuleName, "view/" + strModuleName.ToLower().Trim());
            System.Xml.XmlDocument doc = XMLNodeBinder.getXMLDocument("view", strModuleName);
            string strSearchColumn = XMLNodeBinder.getSingleNodeText("searchbycolumns", doc);
            string strSearchColumnHeader = XMLNodeBinder.getSingleNodeText("searchbylabels", doc);
            strSearchColumn += "," + columnName;
            strSearchColumnHeader += "," + DbTable.GetOneColumnData("tbl_columns", "columns_gridcolumnname", "columns_columnname='" + columnName + "'");
            bindXmlNodeText(doc.SelectSingleNode("/setting/searchbycolumns"), strSearchColumn, false);
            bindXmlNodeText(doc.SelectSingleNode("/setting/searchbylabels"), strSearchColumnHeader, false);
            doc.Save(strFileName);
            DbTable.ExecuteQuery("UPDATE tbl_columns SET columns_issearchfield=1 WHERE columns_columnname='" + columnName + "'");
        }
    }
    private XmlNode bindXmlNodeText(XmlNode xmlNode, string strNodevalue, bool isXML)
    {
        if (xmlNode != null)
        {
            if (isXML)
            {
                xmlNode.InnerXml = strNodevalue;
            }
            else
            {
                xmlNode.InnerText = strNodevalue;
            }
        }
        return xmlNode;
    }
    private bool isObjExists(string type, string objName)
    {
        DataRow dr = null;
        bool isexists = false;
        if (type.ToLower() == "cn")
        {
            dr = DbTable.ExecuteSelectRow("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME ='" + objName + "'");
        }
        else
        {
            dr = DbTable.ExecuteSelectRow("SELECT * FROM sys.objects WHERE name='" + objName + "' AND type='" + type + "'");
        }
        if (dr != null)
        {
            isexists = true;
        }
        return isexists;
    }       
}