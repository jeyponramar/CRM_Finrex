using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
 
public partial class Compiler_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_importdata", "importdataid");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            //FillDropDown_START

            gblData.FillDropdown(ddlimportdatamoduleid, "tbl_module", "module_modulename", "module_moduleid", "", "module_modulename");
            //FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //PopulateAcOnAdd_START

                //PopulateAcOnAdd_END
                //PopulateOnAdd_START
                //PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                int ModuleId = Common.GetQueryStringValue("mid");
                if (ModuleId > 0)
                {
                    trModule.Visible = false;
                    DataRow dr = DbTable.ExecuteSelectRow(" SELECT * FROM tbl_module WHERE module_moduleid=" + ModuleId);
                    if (dr != null)
                    {
                        ViewState["ModuleName"] = GlobalUtilities.ConvertToString(dr["module_modulename"]).ToLower().Trim().Replace(" ", "");
                        ViewState["ModuleId"] = ModuleId;

                        lblPageTitle.Text = "Import " + GlobalUtilities.ConvertToString(dr["module_modulename"]);
                    }
                }                
            }
            else
            {
                
            }
            //CallPopulateSubGrid_START

            //CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Refux Compiler";
        }
        else
        {
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START

    //PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Button btn = new Button();
        btn = (Button)sender;
        if (btn.Text == "Upgrade DataBase")
        {
            UpdateDb();
        }
        if (btn.Text == "Get Upgradable Objects")
        {
            Check_DbObjects();
        }
        if (btn.Text == "Compile All Module")
        {
            CompileModule(true);
            return;
        }
        if (GlobalUtilities.ConvertToInt(ddlimportdatamoduleid.SelectedValue) > 0)
        {

            CompileModule();
        }
        else
        {
            lblMessage.Text = "Please select any one module to Compile";
            lblMessage.Visible = true;
        }
        
    }
    private DataTable GetFilteredColuns()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Columns");
        dt.Columns.Add("Msg");
        dt.Columns.Add("Status");
        dt.Columns.Add("Error");
        string status = Convert.ToString((AppConstants.IsLive) ? "Warning" : "Error");
        DataRow dr = dt.NewRow();
        dr["Columns"] = "columns_isrequired";
        dr["Error"] = "RequiredField";
        dr["Msg"] = "Required Field is Missing";
        dr["Status"] = status;
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["Columns"] = "columns_isunique";
        dr["Error"] = "UniqueField";
        dr["Msg"] = "Unique Field is Missing";
        dr["Status"] = status;
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["Columns"] = "columns_isviewpage";
        dr["Error"] = "ViewColumn";
        dr["Msg"] = "View Column is  Missing";
        dr["Status"] = status;
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["Columns"] = "columns_issearchfield";
        dr["Error"] = "Searchcolumn";
        dr["Msg"] = "Search column is  Missing";
        dr["Status"] = status;
        dt.Rows.Add(dr);
        return dt;
    }

    private bool isValidModule(DataTable dt)
    {
        return false;
    }
    private void CompileModule()
    {
        CompileModule(false);
    }
    private string getCss(ref int rowid)
    {
        rowid = rowid + 1;
        string rcls = "repeater-alt";
        if (rowid % 2 == 0)
        {

            rcls = "repeater-row";
        }
        return rcls;
    }
    private void CheckSmsPackageEnabled(ref string errorMsg)
    {        
        string strSettings = GlobalUtilities.ReadFile(Server.MapPath("~/App_Code/Custom/CustomSettings.cs"));
        string html = " <div style='color:red;font-size:20px;text-align:center;padding:20px'>";
        if (strSettings.IndexOf("IsSMSPackageEnable")>0)
        {
            if (strSettings.Contains("public static bool IsSMSPackageEnable = false;"))
            {
                errorMsg = "<b> SMSPackageEnable = False </b>";
            }
            else if(strSettings.Contains("public static bool IsSMSPackageEnable = true;"))
            {
                errorMsg = "<b> SMSPackageEnable = True </b>";
            }
            //public static bool IsSMSPackageEnable = false;
        }
        errorMsg =(errorMsg=="")?"SMSPackageEnable Line is Missing!! Please Check whether this project need this settings or not":errorMsg;
        errorMsg = html + errorMsg + "</div>";
    }
    private void CompileModule(bool isAll)
    {
        int Counter = 0;
        string strWarn = "";
        Thread t = new Thread(() =>
            {
                CheckSmsPackageEnabled(ref strWarn);
            });
        t.Start();

        int moduleid = GlobalUtilities.ConvertToInt(ddlimportdatamoduleid.SelectedValue);
        string query = @"SELECT * FROM tbl_module
                    JOIN tbl_columns ON columns_moduleid = module_moduleid" + Convert.ToString((isAll) ? "" : " WHERE columns_moduleid=" + moduleid);
        DataTable dtColumns = new DataTable(); //DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<tr>");
        string module = ddlimportdatamoduleid.SelectedItem.Text;
        string msg = "";
        string debuggingStatus = "";
        string strtablename = "";
        query = @"SELECT * FROM tbl_module
                    " + Convert.ToString((isAll) ? "" : " WHERE module_moduleid= " + moduleid);
        DataTable dtModule = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dtModule))
        {
            for (int j = 0; j < dtModule.Rows.Count; j++)
            {
                module = GlobalUtilities.ConvertToString(dtModule.Rows[j]["module_modulename"]).ToLower().Replace(" ", "");
                strtablename = GlobalUtilities.ConvertToString(dtModule.Rows[j]["module_tablename"]);
                moduleid = GlobalUtilities.ConvertToInt(dtModule.Rows[j]["module_moduleid"]);
                bool isSubModuleCheckOver = false;
                query = @"SELECT * FROM tbl_module
                    JOIN tbl_columns ON columns_moduleid = module_moduleid WHERE columns_moduleid=" + moduleid;
                dtColumns = DbTable.ExecuteSelect(query);

                DataTable dt = GetFilteredColuns();
                string columnsName = "";
                string AllColumns = getTop10CoulmnsCommaSep(dtColumns, "columns_columnname");
                for (int i = 0; i < dt.Rows.Count; i++)//check filtered column and sequence
                {
                    columnsName = GlobalUtilities.ConvertToString(dt.Rows[i]["Columns"]);
                    msg = GlobalUtilities.ConvertToString(dt.Rows[i]["Msg"]);
                    string error = GlobalUtilities.ConvertToString(dt.Rows[i]["Error"]);
                    debuggingStatus = GlobalUtilities.ConvertToString(dt.Rows[i]["Status"]);
                    DataRow[] arrDr = dtColumns.Select(columnsName + "=1");
                    
                    if (arrDr.Length == 0)
                    {
                        if (columnsName == "columns_isunique")
                        {
                            string strCheckUniqueContraint = @"SELECT COUNT(*)AS Cnt FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                                                    WHERE CONSTRAINT_TYPE='UNIQUE' AND TABLE_NAME='" + strtablename + "'";
                            if (GlobalUtilities.ConvertToInt(DbTable.ExecuteSelectRow(strCheckUniqueContraint)["Cnt"]) == 0)
                            {
                                html.Append(getHtml(AllColumns, "", error, getCss(ref Counter), module, moduleid, msg, true));
                            }
                        }
                        else
                        {
                            html.Append(getHtml(AllColumns, "", error, getCss(ref Counter), module, moduleid, msg, true));
                        }
                    }
                }
                for (int i = 0; i < dtColumns.Rows.Count; i++)
                {
                    columnsName = GlobalUtilities.ConvertToString(dtColumns.Rows[i]["columns_columnname"]);
                    int submoduleId = GlobalUtilities.ConvertToInt(dtColumns.Rows[i]["columns_submoduleid"]);
                    //Check status Module contains status ,then check Color Columns
                    if (columnsName.EndsWith("_status") || columnsName.EndsWith("_" + module + "status"))
                    {
                        DataRow[] arrDr = dtColumns.Select("columns_moduleid=" + moduleid + " AND columns_columnname='" + module + "_backgroundcolor'");
                        if (arrDr != null && arrDr.Length > 0)
                        {
                        }
                        else
                        {
                            html.Append(getHtml(columnsName, "", "CCMissing", getCss(ref Counter), module, moduleid, "BackGround Color Columns is missing",false));
                        }
                    }
                    //end
                    //Is SubGrid Contains Min One Required Columns
                    if (submoduleId > 0 && !isSubModuleCheckOver)
                    {
                        string subModuleName = "";
                        if (!IsValidSubModule_Required(submoduleId, ref columnsName, ref subModuleName))
                        {
                            html.Append(getHtml(columnsName, "", "RequiredField", getCss(ref Counter), subModuleName, submoduleId, "SubModule (Reqired Field is Missing)", true));
                        }

                    }
                    //End
                    //Suggested View Columns

                    //End
                }
            }

            ltCompilerStatus.Text = html.ToString();
        }
        ltWarnings.Text = strWarn;
    }
    private string getTop10CoulmnsCommaSep(DataTable dt, string columnname)
    {
        string strColumns = "";
        for (int i = 0; i < dt.Rows.Count&&i<3; i++)
        {
            string col = GlobalUtilities.ConvertToString(dt.Rows[i][columnname]);
            strColumns += (strColumns == "") ? col : "," + col;
        }
        return strColumns;

    }
    private string getHtml(string AllColumns, string url, string errortype, string css, string module, int moduleid, string error, bool isError)
    {
        StringBuilder html = new StringBuilder();
        Array arrColumn = AllColumns.Split(',');
        string columnName = GlobalUtilities.ConvertToString(arrColumn.GetValue(0));
        html.Append("<tr class='" +css + "'><td>" + module + "</td>");
        html.Append("<td>" + Convert.ToString((isError) ? "Warning" : "Warning") + "</td>");
        html.Append("<td style='color:blue;'>" + errortype + "</td><td><textarea rows='15' cols='30' class='"+columnName+" status' name='"+columnName+"'>"+AllColumns+"</textarea></td>");
        html.Append("<td><div style='text-decoration:underline;color:#0D05F5;' name='" + columnName + "' class='fixerror' url=utilities.ashx?mm=compiler&" + Convert.ToString((url == "") ? "error=" + errortype + "&m=" + module + "&mid=" + moduleid + "&cn=" + columnName + "" : url) + ">Fix this </div></td></tr>");
        return html.ToString();
    }
    private bool IsValidSubModule_Required(int submoduleId,ref string strSubModuleColumns,ref string SubModulename)
    {
        bool IsUniqueKeyExists = true;
       DataTable dt = DbTable.ExecuteSelect("SELECT * FROM tbl_columns WHERE columns_submoduleid=" + submoduleId);
       if (GlobalUtilities.IsValidaTable(dt))
       {           
           DataRow[] arrDr = dt.Select("columns_isrequired=1");
           if (arrDr.Length == 0)
           {
               string strSubmodule = Common.GetOneColumnData("tbl_columns", submoduleId, "columns_gridcolumnname").ToLower().Replace(" ", "").Trim();
               SubModulename = strSubmodule;
               IsUniqueKeyExists = false;
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   string strColumnName = GlobalUtilities.ConvertToString(dt.Rows[i]["columns_columnname"]);
                   if (strColumnName.EndsWith("id"))//like productid or others
                   {
                       strSubModuleColumns = strColumnName;
                       //break;
                   }
                   else
                   {
                       if (strSubModuleColumns.EndsWith("amount"))
                       {
                           strSubModuleColumns += (strSubModuleColumns == "") ? strColumnName : "," + strColumnName;
                       }
                   }
               }
           }
       }
       return IsUniqueKeyExists;
    }
    private Array getFirstLevel_table_Data(DataTable dt, string tablename)
    {
        string strTable_Name = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strColumnName = dt.Rows[i]["COLUMN_NAME"].ToString();
            string strInnerTableName = (strColumnName.IndexOf("id") > 0) ? strColumnName.Substring(strColumnName.IndexOf('_') + 1, strColumnName.Length - strColumnName.IndexOf('_') - 1).Replace("id", "") : "";
            if (strInnerTableName != "" && strInnerTableName != tablename)
            {
                strTable_Name += (strTable_Name == "") ? strInnerTableName : "," + strInnerTableName;
            }
        }
        return strTable_Name.Split(',');
    }
    protected void btnDeleteModule_Click(object sender, EventArgs e)
    {

    }
    private DataTable GetLocalDbData()
    {
        DataTable dtLocalDb = new DataTable();
       // return dtLocalDb;
        string connection = "Data Source=RSERVER\\SQLEXPRESS; Initial Catalog=CRM_V4; Integrated Security=false; User Id=sa; Password=refux;";
        SqlConnection conn = new SqlConnection(connection);
        SqlCommand cmd = new SqlCommand(@"SELECT * FROM tbl_localdbscript t1 WHERE localdbscript_objtype IN('U','P','FN','TR','CN') AND localdbscript_projectid = " + GlobalUtilities.ConvertToInt(Common.GetQueryStringValue("pid")),conn);        

        //string query = "SELECT '' AS localdbscript_datatype,10 AS localdbscript_objid,1 As localdbscript_localdbscriptid,name AS localdbscript_objname,create_date AS localdbscript_createddate,modify_date AS localdbscript_modifieddate,*,OBJECT_DEFINITION(t1.object_id) AS localdbscript_objdefinition FROM sys.objects t1 WHERE type IN('U','P','FN','TR')";
        //SqlCommand cmd = new SqlCommand(query,conn);

        conn.Open();
        cmd.CommandType = CommandType.Text;        
        SqlDataAdapter adp = new SqlDataAdapter();
        adp.SelectCommand = cmd;
        adp.Fill(dtLocalDb);
      return dtLocalDb;
    }
    private DataTable getObj_Data()
    {
        DataTable dt = new DataTable();
        string query = "SELECT name AS Obj_Name,create_date AS CreatedDate,modify_date AS ModifiedDate,OBJECT_DEFINITION(t1.object_id) AS Obj_Definition,* FROM sys.objects t1 WHERE type IN('U','P','FN','TR')";
        string connection = "Data Source=; Initial Catalog=; Integrated Security=false; User Id=sa; Password=refux;";
        SqlConnection conn = new SqlConnection(connection);
        SqlCommand cmd = new SqlCommand(query, conn);
        conn.Open();
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter adp = new SqlDataAdapter();
        adp.SelectCommand = cmd;
        adp.Fill(dt);
        conn.Close();
        adp.Dispose();
        cmd.Dispose();
        int cnt = dt.Rows.Count;
        return dt;
    }
    private void Check_DbObjects()
    {
        DataTable dtLocalDb = GetLocalDbData();
        if (!GlobalUtilities.IsValidaTable(dtLocalDb)) return;
        DataTable dtClientDb = new DataTable();
        //dtClientDb = DbTable.ExecuteSelect(@"SELECT name AS Obj_Name,create_date AS CreatedDate,modify_date AS ModifiedDate,*,OBJECT_DEFINITION(t1.object_id) AS Obj_Definition FROM sys.objects t1 WHERE type IN('U','P','FN','TR')");

        dtClientDb = DbTable.ExecuteSelect(@" SELECT type,name AS Obj_Name,create_date AS CreatedDate,modify_date AS ModifiedDate,OBJECT_DEFINITION(t1.object_id) AS Obj_Definition,''AS datatype,'' AS maxlen   FROM sys.objects t1 WHERE type IN('U','P','FN','TR') 
                                            UNION 
                                             SELECT 'CN' AS type,COLUMN_NAME AS Obj_Name,GETDATE() AS CreatedDate,GETDATE() AS ModifiedDate,'' AS Obj_Definition,CHARACTER_MAXIMUM_LENGTH AS datatype,CHARACTER_MAXIMUM_LENGTH AS maxlen FROM INFORMATION_SCHEMA.COLUMNS
                                            ");


        DataTable dtLocal_Procedure = ((dtLocalDb.Select("localdbscript_objtype='P'")).Length > 0) ? (dtLocalDb.Select("localdbscript_objtype='P'")).CopyToDataTable() : null;
        DataTable dtLocal_Function = ((dtLocalDb.Select("localdbscript_objtype='FN'")).Length > 0) ? (dtLocalDb.Select("localdbscript_objtype='FN'")).CopyToDataTable() : null;
        DataTable dtLocal_Trigger = ((dtLocalDb.Select("localdbscript_objtype='TR'")).Length > 0) ? (dtLocalDb.Select("localdbscript_objtype='TR'")).CopyToDataTable() : null;
        DataTable dtLocal_Table = ((dtLocalDb.Select("localdbscript_objtype='U'")).Length > 0) ? (dtLocalDb.Select("localdbscript_objtype='U'")).CopyToDataTable() : null;
        DataTable dtLocal_CName = ((dtLocalDb.Select("localdbscript_objtype='CN'")).Length > 0) ? (dtLocalDb.Select("localdbscript_objtype='CN'")).CopyToDataTable() : null;
        StringBuilder html_Table = new StringBuilder();
        StringBuilder html_Procedure = new StringBuilder();
        StringBuilder html_Function = new StringBuilder();
        StringBuilder html_Trigger = new StringBuilder();
        StringBuilder html_Column = new StringBuilder();

        DataTable dtClient_Procedure = ((dtClientDb.Select("type='P'")).Length > 0) ? (dtClientDb.Select("type='P'")).CopyToDataTable() : null;
        DataTable dtClient_Function = ((dtClientDb.Select("type='FN'")).Length > 0) ? (dtClientDb.Select("type='FN'")).CopyToDataTable() : null;
        DataTable dtClient_Trigger = ((dtClientDb.Select("type='TR'")).Length > 0) ? (dtClientDb.Select("type='TR'")).CopyToDataTable() : null;
        DataTable dtClient_Table = ((dtClientDb.Select("type='U'")).Length > 0) ? (dtClientDb.Select("type='U'")).CopyToDataTable() : null;
        DataTable dtClient_CName = ((dtClientDb.Select("type='CN'")).Length > 0) ? (dtClientDb.Select("type='CN'")).CopyToDataTable() : null;


        html_Table.Append(getSuggestion(dtLocal_Table, dtClient_Table,"Table"));
        html_Procedure.Append(getSuggestion(dtLocal_Procedure, dtClient_Procedure,"Procedure"));
        html_Function.Append(getSuggestion(dtLocal_Function, dtClient_Function,"Function"));
        html_Trigger.Append(getSuggestion(dtLocal_Trigger, dtClient_Trigger,"Trigger"));
        html_Column.Append(getSuggestion(dtLocal_CName, dtClient_CName, "Columns"));

        //Thread t_Table = new Thread(() =>
        //{
        //    html_Table.Append(getSuggestion(dtLocal_Table, dtClient_Table));
        //});
        //t_Table.Start();

        //Thread t_procedure = new Thread(() =>
        //    {
        //        html_Table.Append(getSuggestion(dtLocal_Procedure, dtClient_Procedure));
        //    });
        //t_procedure.Start();

        //Thread t_Function = new Thread(() =>
        //{
        //    html_Table.Append(getSuggestion(dtLocal_Function, dtClient_Function));
        //});
        //t_Function.Start();

        //Thread t_Trigger = new Thread(() =>
        //{
        //    html_Table.Append(getSuggestion(dtLocal_Trigger, dtClient_Trigger));
        //});
        //t_Trigger.Start();

        //while(t_Trigger.IsAlive||t_Table.IsAlive||t_procedure.IsAlive||t_Function.IsAlive)
        //{
        //    Thread.Sleep(500);
        //}
        string str_Function = html_Function.ToString();
        string str_Procedure = html_Procedure.ToString();
        string str_Table = html_Table.ToString();
        string str_Trigger = html_Trigger.ToString();
        string str_Column = html_Column.ToString();
        ltDbData.Text = str_Table + str_Column + str_Function + str_Procedure + str_Trigger;
    }
    private string getSuggestion(DataTable dtLocal,DataTable dtClient,string type)
    {        
        if(!GlobalUtilities.IsValidaTable(dtLocal)||!GlobalUtilities.IsValidaTable(dtClient))return " <div class='error'>--No Updation Or New Creation Found--</div>";
        DataTable dtUpdatableData = new DataTable();
        dtUpdatableData.Columns.Add("Type");
        dtUpdatableData.Columns.Add("ObjName");
        dtUpdatableData.Columns.Add("Objdef");
        dtUpdatableData.Columns.Add("Datatype");
        int ProjectId = GlobalUtilities.ConvertToInt(Common.GetQueryStringValue("pid"));
        //DateTime dtTempDate = DateTime.Now.AddDays(-20);
        string[] arrDate = txtLastUpdateDate.Text.Split('-');
        DateTime dtTempDate = new DateTime(GlobalUtilities.ConvertToInt(arrDate.GetValue(2)), GlobalUtilities.ConvertToInt(arrDate.GetValue(1)), GlobalUtilities.ConvertToInt(arrDate.GetValue(0)));
        StringBuilder html = new StringBuilder();
        html.Append("<tr><td colspan='6' class='repeater-header'>"+type+"</td></tr>");
        bool _isDataExists = false;
        for (int i = 0; i < dtLocal.Rows.Count; i++)
        {
            bool isObjExists = false;
            bool isObjModified = false;
            DataRow dr_Local = dtLocal.Rows[i];
            string str_LObjName = GlobalUtilities.ConvertToString(dr_Local["localdbscript_objname"]);
            int int_LScriptId = GlobalUtilities.ConvertToInt(dr_Local["localdbscript_localdbscriptid"]);
            string str_LCreatedDate = GlobalUtilities.ConvertToDate(dr_Local["localdbscript_createddate"]);
            string str_LModifiedDate = GlobalUtilities.ConvertToDate(dr_Local["localdbscript_modifieddate"]);
            string str_LObjDefinition = GlobalUtilities.ConvertToString(dr_Local["localdbscript_objdefinition"]);
            DateTime dt_LcreatedDate = Convert.ToDateTime(dr_Local["localdbscript_createddate"]);
            DateTime dt_LmodifiedDate = Convert.ToDateTime(dr_Local["localdbscript_modifieddate"]);
            int objId = GlobalUtilities.ConvertToInt(dr_Local["localdbscript_objid"]);
            string datatype = GlobalUtilities.ConvertToString(dr_Local["localdbscript_datatype"]);
            int maxlen = GlobalUtilities.ConvertToInt(dr_Local["localdbscript_maxlen"]);
            string objtype= GlobalUtilities.ConvertToString(dr_Local["localdbscript_objtype"]);



            string str_CCreatedDate = "";
            string str_CModifiedDate = "";
            string strStatus = "";
            string css = (i % 2 == 0) ? "repeater-row" : "repeater-alt";
            html.Append("<tr class='" + css + "'>");
            for (int j = 0; j < dtClient.Rows.Count; j++)
            {
                DataRow dr_Client = dtClient.Rows[j];
                string str_CObjName = GlobalUtilities.ConvertToString(dr_Client["Obj_Name"]);
                string str_CObjDefinition = GlobalUtilities.ConvertToString(dr_Client["Obj_Definition"]);
                str_CCreatedDate = GlobalUtilities.ConvertToDate(dr_Client["CreatedDate"]);
                str_CModifiedDate = GlobalUtilities.ConvertToDate(dr_Client["ModifiedDate"]);
                DateTime dt_CcreatedDate = Convert.ToDateTime(dr_Client["CreatedDate"]);
                DateTime dt_CmodifiedDate = Convert.ToDateTime(dr_Client["ModifiedDate"]);
                if (str_CObjName == str_LObjName)
                {
                    isObjExists = true;  
                    if (dt_LmodifiedDate >= dtTempDate)
                    {
                        isObjModified= true;                        
                        break;
                    }                                      
                    break;
                }                
            }
            if (!isObjExists||isObjModified)//Not exists ON Client Side
            {                
                _isDataExists = true;
                if(!isObjExists)strStatus =" New " + type;
                if(isObjModified)
                    strStatus = " Modified " + type;
                if ((type == "Columns" && !isObjExists) || type != "Columns")
                {
                    html.Append("<td>" + str_LObjName + "</td>");
                    html.Append("<td>Local(" + str_LCreatedDate + ") Client(" + str_CCreatedDate + ")</td>");
                    html.Append("<td>Local(" + str_LModifiedDate + ") Client(" + str_CModifiedDate + ")</td>");

                    html.Append("<td style='color:Red;'>" + strStatus + "</td>");
                    html.Append("<td class='hidden'>" + objId + "</td>");
                    

                    DataRow dr = dtUpdatableData.NewRow();
                    dr["Type"] = objtype;
                    dr["Objname"] = str_LObjName;
                    dr["Objdef"] = str_LObjDefinition;
                    string strDtType = "";
                    if (maxlen > 0)
                    {
                        strDtType = datatype + "(" + maxlen + ")";
                    }
                    dr["Datatype"] = strDtType;
                    dtUpdatableData.Rows.Add(dr);
                    html.Append("<td><div style='color:Blue' url='utilities.ashx?mm=dbscript&type=" + type + "&obj_name=" + str_LObjName + "&datatype=" + strDtType + "&objtype=" + objtype + "&isNewObj=" + Convert.ToString((strStatus.IndexOf("New") >= 0) ? "true" : "false") + "&id=" + int_LScriptId + "&projectid=" + ProjectId + "&objid=" + objId + "&obj_def=" + str_LObjDefinition + "' class='fixerror' >Execute</div></td>");
                    html.Append("<td><textarea >" + str_LObjDefinition.Replace("&nbsp;", "\t").Replace("~", "'").Replace("<br/>", "\n") + "</textarea></td>");
                }
            }
            html.Append("</tr>");

        }
        ViewState["UpdatableData"] = dtUpdatableData;
        if (!_isDataExists) html.Append("<tr ><td colspan='6' class='error' align='center'>--No Updation Or New Creation Found--</td></tr>");
        return html.ToString();
    }
    private void UpdateDb()
    {
        if (ViewState["UpdatableData"] != null)
        {
            DataTable dt = (DataTable)ViewState["UpdatableData"];
            if (GlobalUtilities.IsValidaTable(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string objtype = GlobalUtilities.ConvertToString(dt.Rows[i]["Type"]);
                    string Objname = GlobalUtilities.ConvertToString(dt.Rows[i]["Objname"]);
                    string Objdef = GlobalUtilities.ConvertToString(dt.Rows[i]["Objdef"]);
                    string Datatype = GlobalUtilities.ConvertToString(dt.Rows[i]["Datatype"]);

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
                        DbTable.ExecuteQuery(Objdef.Replace("&nbsp;", "/t").Replace("~", "'").Replace("<br/>", "/n"));
                    }
                }
            }
        }
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
