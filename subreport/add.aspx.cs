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

public partial class SubReport_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_subreport", "subreportid");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlreportid, "tbl_report", "report_reportname", "report_reportid", "", "report_reportname");
			gblData.FillDropdown(ddlmenuid, "tbl_menu", "menu_menuname", "menu_menuid", "", "menu_menuname");
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
                //PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                ViewState["ReportName"] = txtsubreportname.Text;
                BindSearchFields(GlobalUtilities.ConvertToString(gblData._CurrentRow["subreport_searchfields"]));
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Sub Report";
        }
        else
        {
            lblPageTitle.Text = "Edit Sub Report";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        string searchFields = "";
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith("chkcontrol_"))
            {
                string val = Request.Form[i];
                if (searchFields == "")
                {
                    searchFields = val;
                }
                else
                {
                    searchFields += "," + val;
                }
            }
        }
        gblData.AddExtraValues("searchfields", searchFields);

        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            SaveSubMenu(id);
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            CommonPage.CloseQuickAddEditWindow(Page, form, id);
        }
        else if (id == -1)
        {
            lblMessage.Text = "Data already exists, duplicate entry not allowed!";
            lblMessage.Visible = true;
        }
        else
        {
            lblMessage.Text = "Error occurred while saving data</br>Error : " + gblData._error;
            lblMessage.Visible = true;
        }
        return id;
    }
    //EnableControlsOnEdit_START
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnDelete.Visible = true;
	}//EnableControlsOnEdit_END
    private int GetId()
    {
        if (h_IsCopy.Text == "1")
        {
            return 0;
        }
        else
        {
            return GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common.Delete();
    }
    protected void ddlreport_changed(object sender, EventArgs e)
    {
        BindSearchFields("");
    }
    private void BindSearchFields(string searchFields)
    {
        int reportId = GlobalUtilities.ConvertToInt(ddlreportid.SelectedValue);
        Array arrSearchFields = searchFields.Split(',');
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_reportdetail WHERE reportdetail_reportid=" + reportId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ltsearchfield.Text = "";
        if (!GlobalUtilities.IsValidaTable(dttbl)) return;
        html.Append("<table>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            //string label = GlobalUtilities.ConvertToString(dttbl.Rows[i]["columns_lbl"]);
            string columnName = GlobalUtilities.ConvertToString(dttbl.Rows[i]["reportdetail_columnname"]);
            columnName = Common.GetColumnName(columnName);
            string strchecked = "";
            for (int j = 0; j < arrSearchFields.Length; j++)
            {
                if (columnName == arrSearchFields.GetValue(j).ToString())
                {
                    strchecked = " checked='checked'";
                    break;
                }
            }
            html.Append("<tr><td><input type='checkbox'" + strchecked + " name='chkcontrol_" + columnName + "' id='chkcontrol_" + columnName + "' value='" + columnName + "'/>" +
                                "<label for='chkcontrol_" + columnName + "'>" + columnName + "</label></td></tr>");
        }
        html.Append("</table>");
        ltsearchfield.Text = html.ToString();
    }
    private void SaveSubMenu(int subReportId)
    {
        int menuId = GlobalUtilities.ConvertToInt(ddlmenuid.SelectedValue);
        string subMenuName = txtsubreportname.Text;
        int sequence = 0;
        string submenu = "";
        int submenuId = 0;
        string query = "";
        query = "select * from tbl_report where report_reportid=" + ddlreportid.SelectedValue;
        DataRow drreport = DbTable.ExecuteSelectRow(query);
        string reporturl = "report/" + GlobalUtilities.ConvertToString(drreport["report_reportname"]).Replace(" ", "").ToLower() + ".aspx";
        if (Request.QueryString["id"] != null)
        {
            if (GlobalUtilities.ConvertToString(ViewState["ReportName"]) != txtsubreportname.Text)
            {
                submenu = GlobalUtilities.ConvertToString(ViewState["ReportName"]);
            }
        }
        if (submenu == "")
        {
            submenu = txtsubreportname.Text;
        }
        query = "select * from tbl_submenu where submenu_submenuname='" + submenu + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            submenuId = GlobalUtilities.ConvertToInt(dr["submenu_submenuid"]);
        }

        Hashtable hstbl = new Hashtable();
        hstbl.Add("menuid", menuId);
        hstbl.Add("submenuname", subMenuName);
        hstbl.Add("url", reporturl + "?sr=" + subReportId);
        hstbl.Add("isvisible", 1);
        hstbl.Add("menutype", "report");
        if (dr == null)
        {
            query = "select top 1 * from tbl_submenu where submenu_menuid=" + menuId + " ORDER BY submenu_sequence desc";
            DataRow drSubmenuSeq = DbTable.ExecuteSelectRow(query);
            if (drSubmenuSeq != null)
            {
                sequence = GlobalUtilities.ConvertToInt(drSubmenuSeq["submenu_sequence"]) + 1;

            }
            hstbl.Add("sequence", sequence);
        }
        InsertUpdate obj = new InsertUpdate();
        if (submenuId == 0)
        {
            obj.InsertData(hstbl, "tbl_submenu");
        }
        else
        {
            obj.UpdateData(hstbl, "tbl_submenu", submenuId);
        }
    }
}
