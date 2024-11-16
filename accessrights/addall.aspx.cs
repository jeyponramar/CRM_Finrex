using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using WebComponent;
using System.Collections;

public partial class accessrights_addall : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_rights","rightsid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlRole);
            //gblData.FillDropdown(ddlModule);
        }
    }
    protected void ddlRole_Changed(object sender, EventArgs e)
    {
        BindAccessRights();
        
    }
    private void BindAccessRights()
    {
        lblMessage.Visible = false;
        if (GlobalUtilities.ConvertToInt(ddlRole.SelectedValue) == 1)
        {
            lblMessage.Text = "Administrator has all the rights.";
            lblMessage.Visible = true;
            ltRights.Text = "";
            return;
        }

        if (ddlRole.SelectedIndex == 0)
        {
            ltRights.Text = "";
            return;
        }

        StringBuilder html = new StringBuilder();
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        string query = "select * from tbl_module where module_rightsrequired=1";
        dttbl = obj.ExecuteSelect(query);
        html.Append("<table width='80%' cellpadding=0 cellspacing=0>");
        for (int m = 0; m < dttbl.Rows.Count; m++)
        {
            string extraActions = Convert.ToString(dttbl.Rows[m]["module_rightsaction"]);
            string actions = "Create,Update,Delete,View,Export";
            DataTable dttblRights = gblData.GetData("tbl_rights", "rights_module='" + Convert.ToString(dttbl.Rows[m]["module_modulename"]) +
                                        "' and rights_roleid=" + GlobalUtilities.ConvertToInt(ddlRole.SelectedValue));
            if (extraActions != "")
            {
                actions += "," + extraActions;
            }
            Array arrActions = actions.Split(',');

            html.Append("<tr><td class='subtitle'>" + Convert.ToString(dttbl.Rows[m]["module_modulename"]) + "</td></tr><tr><td><table><tr>");
            for (int i = 0; i < arrActions.Length; i++)
            {
                string chked = "";
                string action = arrActions.GetValue(i).ToString();
                for (int j = 0; j < dttblRights.Rows.Count; j++)
                {
                    if (Convert.ToString(dttblRights.Rows[j]["rights_action"]).ToLower() == action.ToLower())
                    {
                        chked = "checked";
                        break;
                    }
                }
                html.Append("<td style='padding-left:10px;'><input " + chked + " type='checkbox' class='chk' name='ar_" +
                    Convert.ToString(dttbl.Rows[m]["module_modulename"]) + "_" + action + "'/></td>" +
                            "<td>" + action + "</td>");

            }
            html.Append("</tr></table></td></tr>");
        }
        html.Append("</table>");
        ltRights.Text = html.ToString();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlRole.SelectedValue == "1")
        {
            lblMessage.Text = "Administrator rights can not be changed.";
            lblMessage.Visible = true;
            return;
        }
        if (ddlRole.SelectedIndex == 0)
        {
            lblMessage.Text = "Please select the role.";
            lblMessage.Visible = true;
            return;
        }
        string query = "delete from tbl_rights where rights_roleid=" + GlobalUtilities.ConvertToInt(ddlRole.SelectedValue);
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i].ToLower();
            if (key.StartsWith("ar_"))
            {
                Array arr = key.Split('_');
                Hashtable hstbl = new Hashtable();
                hstbl.Add("roleid", ddlRole.SelectedValue);
                hstbl.Add("module", arr.GetValue(1));
                hstbl.Add("action", arr.GetValue(2));
                InsertUpdate obj2 = new InsertUpdate();
                obj2.InsertData(hstbl, "tbl_rights");
            }
        }
        BindAccessRights();
    }
}
