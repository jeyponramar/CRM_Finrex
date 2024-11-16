using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;
using System.Collections;

public partial class MultiCheckBox : System.Web.UI.UserControl
{
    private string _module = "";
    private string _targetmodule = "";
    private string _column = "";
    private int _columns = 1;
    private int _height = 0;
    private bool _isAlreadyBinded = false;
    private string _ids = "";
    private string _where = "";
    private string _isCommaSeperated = "";
    private string _isAll = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!_isAlreadyBinded)
            {
                Bind();
            }
        }
    }
    public string Module
    {
        get { return _module; }
        set { _module = value; }
    }
    public string TargetModule
    {
        get { return _targetmodule; }
        set { _targetmodule = value; }
    }
    public string Column
    {
        get { return _column; }
        set { _column = value; }
    }
    public int Columns
    {
        get { return _columns; }
        set { _columns = value; }
    }
    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }
    public string Ids
    {
        get { return _ids; }
    }
    public string Where
    {
        get { return _where; }
        set { _where = value; }
    }
    public string IsCommaSeperated
    {
        get { return _isCommaSeperated; }
        set { _isCommaSeperated = value; }
    }
    public string IsAll
    {
        get { return _isAll; }
        set { _isAll = value; }
    }
    public string GetSelectedIds()
    {
        StringBuilder ids = new StringBuilder();
        string prefix = "ch" + this.ID + "-" + Module + "_";
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith(prefix))
            {
                Array arr = key.Split('_');
                int newid = GlobalUtilities.ConvertToInt(arr.GetValue(1));
                if (ids.ToString() == "")
                {
                    ids.Append(newid);
                }
                else
                {
                    ids.Append("," + newid);
                }
            }
        }
        return ids.ToString();
    }
    public void AddSelectedIds_CommaSep(GlobalData objgbldata)
    {
        string ids = GetSelectedIds();
        objgbldata.AddExtraValues(TargetModule + "ids", ids);
    }
    public void Save(int id)
    {
        string masterModule = Common.GetModuleName();
        string query = "";
        string prefix = "ch" + this.ID + "-" + Module + "_";
        StringBuilder ids = new StringBuilder();
        if (IsSaveAdCommaSep)
        {
            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                string key = Request.Form.Keys[i];
                if (key.StartsWith(prefix))
                {
                    Array arr = key.Split('_');
                    int newid = GlobalUtilities.ConvertToInt(arr.GetValue(1));
                    if (ids.ToString() == "")
                    {
                        ids.Append(newid);
                    }
                    else
                    {
                        ids.Append("," + newid);
                    }
                }
            }
            _ids = ids.ToString();
            StringBuilder values = new StringBuilder();
            if (ids.ToString() != "")
            {
                query = "select * from tbl_" + Module + " where " + Module + "_" + Module + "id in(" + ids.ToString() + ")";
                DataTable dttbl = DbTable.ExecuteSelect(query);
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        values.Append(GlobalUtilities.ConvertToString(dttbl.Rows[i][Module + "_" + Column])).Replace("'","");
                    }
                    else
                    {
                        values.Append("," + GlobalUtilities.ConvertToString(dttbl.Rows[i][Module + "_" + Column])).Replace("'", "");
                    }
                }
            }
            query = "update tbl_" + masterModule + " set " + masterModule + "_" + TargetModule + "ids='" + ids.ToString() + "'," + masterModule + "_" + TargetModule + "values='" + values.ToString() + "' where " + masterModule + "_" + masterModule + "id=" + id;
            DbTable.ExecuteQuery(query);
        }
        else
        {
            string mainColumn = TargetModule + "_" + masterModule + "id";
            query = "delete from tbl_" + TargetModule + " WHERE " + mainColumn + "=" + id;
            DbTable.ExecuteQuery(query);
            string idcolumn = TargetModule + "_" + Module + "id";
            
            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                string key = Request.Form.Keys[i];
                if (key.StartsWith(prefix))
                {
                    Array arr = key.Split('_');
                    int newid = GlobalUtilities.ConvertToInt(arr.GetValue(1));

                    Hashtable hstbl = new Hashtable();
                    hstbl.Add(Module + "id", newid);
                    hstbl.Add(masterModule + "id", id);
                    InsertUpdate obj = new InsertUpdate();
                    obj.InsertData(hstbl, "tbl_" + TargetModule, true);

                    if (ids.ToString() == "")
                    {
                        ids.Append(newid);
                    }
                    else
                    {
                        ids.Append("," + newid);
                    }
                }
            }
            _ids = ids.ToString();
        }
        Bind();
    }
    public void Bind()
    {
        int masterId = Common.GetQueryStringValue("id");
        Bind(masterId, "","", "","");
    }
    public void Bind(int masterId)
    {
        Bind(masterId, "","","","");
    }
    public bool IsSaveAdCommaSep
    {
        get
        {
            if (TargetModule == "clientgroups" || _isCommaSeperated == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void Bind(int masterId, string checkboxModule, string checkboxColumn, string targetmodule, string currentModule)
    {
        _isAlreadyBinded = true;
        if (checkboxModule == "") checkboxModule = Module;
        if (checkboxColumn == "") checkboxColumn = Column;
        if (targetmodule == "") targetmodule = TargetModule;
        if (currentModule == "") currentModule = Common.GetModuleName();
        string query = "select * from tbl_" + checkboxModule;
        if (Where != "") query += " where " + Where;

        string[] arrIds = { };
        DataTable dttbl = DbTable.ExecuteSelect(query);
        //Response.Write(query + ";" + IsSaveAdCommaSep.ToString() + ";" + masterId+";");
        if (masterId > 0 && IsSaveAdCommaSep)
        {
            DataRow dr = DbTable.GetOneRow("tbl_" + currentModule, masterId);
            string ids = GlobalUtilities.ConvertToString(dr[currentModule + "_" + TargetModule + "ids"]);
            //Response.Write(ids);
            arrIds = ids.Split(',');
        }
        //Response.Write("|");
        string columnName = checkboxColumn;
        if (!columnName.Contains("_")) columnName = checkboxModule + "_" + columnName;
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            DataTable dttblSelected = new DataTable();
            if (masterId > 0 && IsSaveAdCommaSep == false)
            {
                query = "select * from tbl_" + targetmodule + " WHERE " + targetmodule + "_" + currentModule + "id=" + masterId;
                dttblSelected = DbTable.ExecuteSelect(query);
            }
            StringBuilder html = new StringBuilder();
            string strHeight = "";
            if (Height > 0)
            {
                strHeight = "style='height:" + Height + "px;' ";
            }
            html.Append("<table><tr><td><div " + strHeight + "class='mchk mchk-" + columnName + "'><table>");
            string idcolumn = targetmodule + "_" + checkboxModule + "id";
            //if (IsSaveAdCommaSep)
            //{
            //    html.Append("<td><div class='left'><input type='checkbox' id='chselectall_" + columnName + "' class='jq-mchkselectall'/></div>" +
            //                    "<label class='left' style='margin-top:5px;' for='chselectall_" + columnName + "'>All</label></td>");
            //}
            string controlId = "";
            if (IsAll == "true")
            {
                controlId = "ch" + this.ID + "-" + checkboxModule + "_all";
                html.Append("<td><div class='left'><input type='checkbox' id='" + controlId + "' class='jq-mchkall'/></div>" +
                            "<label class='left' style='margin-top:5px;' for='" + controlId + "'>All</label></td>");
            }
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i][0]);
                string label = GlobalUtilities.ConvertToString(dttbl.Rows[i][columnName]);
                controlId = "ch" + this.ID + "-" + checkboxModule + "_" + id;
                if (i % Columns == 0) html.Append("<tr>");
                string selected = "";
                if (IsSaveAdCommaSep)
                {
                    if (arrIds.Contains(id.ToString()))
                    {
                        selected = "checked='checked' ";
                    }
                }
                else
                {
                    if (dttblSelected != null)
                    {
                        for (int j = 0; j < dttblSelected.Rows.Count; j++)
                        {
                            int selectedId = GlobalUtilities.ConvertToInt(dttblSelected.Rows[j][idcolumn]);
                            if (selectedId == id)
                            {
                                selected = "checked='checked' ";
                            }
                        }
                    }
                }
                html.Append("<td><div class='left'><input " + selected + "type='checkbox' name='" + controlId + "' id='" + controlId + "' class='chk' dval='" + id + "'/></div>" +
                            "<label class='left' style='margin-top:5px;' for='" + controlId + "'>" + label + "</label></td>");
                if ((i + 1) % Columns == 0) html.Append("</tr>");
            }
            if (!html.ToString().EndsWith("</tr>")) html.Append("</tr>");
            html.Append("</table></div></td></tr></table>");
            ltCheckbox.Text = html.ToString();
        }
    }
}
