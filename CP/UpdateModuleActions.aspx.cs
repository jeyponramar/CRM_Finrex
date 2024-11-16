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
using WebComponent;
using System.IO;
public partial class CP_UpdateModuleActions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void btn_moduleUpdateClick(object sender, EventArgs e)
    {
        ClearData();
        //Common.CheckRights(sender);
        InsertUpdate obj = new InsertUpdate();
        DataTable dttblModules = new DataTable();
        string Query = "";

        Query = "select * from tbl_module";
        dttblModules = obj.ExecuteSelect(Query);
        for (int i = 0; i < dttblModules.Rows.Count; i++)
        {
            int intModuleId = GlobalUtilities.ConvertToInt(dttblModules.Rows[i]["module_moduleid"]);
            string modulename =Convert.ToString(dttblModules.Rows[i]["module_modulename"]);
            modulename = modulename.Replace(" ", "");
            if (Directory.Exists(Server.MapPath("~/" + modulename)))
            {
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/" + modulename));
                string moduleActions = "";
                string action = "";

                foreach (FileInfo file in dir.GetFiles())
                {
                    string fileName = file.FullName;
                    if (file.Name.ToLower().EndsWith("view.aspx"))
                    {
                        action = "View";
                        if (moduleActions == "")
                        {
                            moduleActions = action;
                        }
                        else
                        {
                            moduleActions += "," + action;
                        }
                    }
                    else if (file.Name.ToLower().EndsWith("add.aspx"))
                    {
                        string code = GlobalUtilities.ReadFile(fileName);
                        int startIndex = 0;
                        int endIndex = 0;
                        bool isexists = true;
                        while (isexists)
                        {
                            startIndex = code.IndexOf("<asp:Button", startIndex, StringComparison.CurrentCultureIgnoreCase);
                            if (startIndex > 0)
                            {
                                startIndex = code.IndexOf("Text", startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
                                if (startIndex > 0)
                                {
                                    startIndex = code.IndexOf("\"", startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
                                    if (startIndex > 0)
                                    {
                                        endIndex = code.IndexOf("\"", startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
                                        if (endIndex > 0)
                                        {
                                            action = code.Substring(startIndex + 1, endIndex - startIndex - 1);
                                            if ((action.ToLower() == "submit") || (action.ToLower() == "save"))
                                            {
                                                action = "Create,Update";
                                            }
                                            if (moduleActions == "")
                                            {
                                                moduleActions = action;
                                            }
                                            else
                                            {
                                                moduleActions += "," + action;
                                            }
                                        }
                                        else
                                        {
                                            isexists = false;
                                        }
                                    }
                                    else
                                    {
                                        isexists = false;
                                    }
                                }
                                else
                                {
                                    isexists = false;
                                }
                            }
                            else
                            {
                                isexists = false;
                            }
                            startIndex = endIndex + 1;
                        }
                    }
                }
                //moduleActions += ",Verify";
                UpDateModule(intModuleId, moduleActions, modulename);
            }
        }
    }
    private void UpDateModule(int ModuleId,string ModuleActions,string ModuleName)
    {
        InsertUpdate obj = new InsertUpdate();
        Hashtable hstbl = new Hashtable();
        hstbl.Add("modulename", ModuleName);
        hstbl.Add("rightsaction", ModuleActions);
       int id = obj.InsertData(hstbl, "tbl_moduleaction",true);
    }
    private void ClearData()
    {
        InsertUpdate obj = new InsertUpdate();
        string query = "";
        query = "TRUNCATE TABLE tbl_moduleaction";
        obj.ExecuteQuery(query);
        query = @"INSERT INTO tbl_moduleaction
                    (	
	                    moduleaction_modulename,moduleaction_isreport,moduleaction_rightsaction
                    )
                    SELECT report_reportname,1,'View' FROM tbl_report";
        obj = new InsertUpdate();
        obj.ExecuteQuery(query);
    }
}
