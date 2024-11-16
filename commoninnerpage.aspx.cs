using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.IO;

public partial class commoninnerpage_page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        string m = Common.GetQueryString("m");
        if (m == "othersrules" || m == "fedairules")
        {
        }
        else
        {
            if (!Finstation.IsFinstationEnabled())
            {
                HttpContext.Current.Response.Redirect("~/noaccessfortrial.aspx");
            }
        }
        if (!IsPostBack)
        {
            BindGridData();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGridData();
    }
    private void BindGridData()
    {
        int typeId = Common.GetQueryStringValue("tid");
        //Array arr = Request.Url.ToString().ToLower().Split('/');
        string moduleName = Common.GetQueryString("m");
        //for (int i = 0; i < arr.Length; i++)
        //{
        //    if (arr.GetValue(i).ToString().Contains(".aspx"))
        //    {
        //        moduleName = arr.GetValue(i).ToString().Replace(".aspx", "").Replace("-", "");
        //    }
        //}
        //string[] arr2 = moduleName.Split('?');
        //moduleName = arr2.GetValue(0).ToString();
        string mname = moduleName.Replace(" ", "").ToLower();
        string query = @"select * from tbl_columns 
                      join tbl_module on module_moduleid=columns_moduleid
                      where columns_isviewpage=1 AND columns_submoduleid=0 
                      AND replace(module_modulename,' ','') = '" + moduleName + "'";
        if (mname == "notificationrules")
        {
            query += " and columns_columnname<>'notificationrules_finstationnotificationtypeid'";
        }
        else if (mname == "essentialreading")
        {
            query += " and columns_columnname<>'essentialreading_essentialreadingtypeid'";
        }
        query += " order by columns_sequence";
        DataTable dttblcol = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttblcol))
        {
            return;
        }
        if (mname == "knowledgeseries")
        {
            //WebControl trsearch = (WebControl)this.FindControl("trsearch");
            trsearch.Visible = false;
        }
        StringBuilder viewQuery = new StringBuilder();
        int mid = GlobalUtilities.ConvertToInt(dttblcol.Rows[0]["module_moduleid"]);
        string mfullname = GlobalUtilities.ConvertToString(dttblcol.Rows[0]["module_viewtitle"]).Replace("View ","");
        if (moduleName == "essentialreading")
        {
            DataRow drt = DbTable.GetOneRow("tbl_essentialreadingtype", typeId);
            mfullname = GlobalUtilities.ConvertToString(drt["essentialreadingtype_type"]);
        }
        //Label lbltitle = (Label)this.FindControl("lbltitle");
        lbltitle.Text = mfullname;
        viewQuery.Append("select * from tbl_" + mname);
        //Literal ltpage = new Literal();
        //this.Master.FindControl("ContentPlaceHolder1").Controls.Add(ltpage);
        for (int i = 0; i < dttblcol.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttblcol.Rows[i]["columns_dropdownmoduleid"]) > 0)
            {
                string ddlcol = GlobalUtilities.ConvertToString(dttblcol.Rows[i]["columns_dropdowncolumn"]);
                Array arr1 = ddlcol.Split('_');
                string ddlm = arr1.GetValue(0).ToString();
                if (ddlm != "")
                {
                    viewQuery.Append(" LEFT JOIN tbl_" + ddlm + " ON " + ddlm + "_" + ddlm + "id=" + mname + "_" + ddlm + "id");
                }
            }
        }
        viewQuery.Append(" where 1=1");
        if (mname == "notificationrules")
        {
            viewQuery.Append(" and notificationrules_finstationnotificationtypeid=" + typeId);
        }
        else if (mname == "essentialreading")
        {
            viewQuery.Append(" and essentialreading_essentialreadingtypeid=" + typeId);
        }
        //TextBox txtfromdate = (TextBox)this.FindControl("txtfromdate");
        //TextBox txttodate = (TextBox)this.FindControl("txttodate");
        if (txtfromdate.Text != "") viewQuery.Append(" AND cast(" + mname + "_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txtfromdate.Text)) + "' as date)");
        if (txttodate.Text != "") viewQuery.Append(" AND cast(" + mname + "_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txttodate.Text)) + "' as date)");

        if (mname == "knowledgeseries" || mname=="indianecoindicators")
        {
            viewQuery.Append(" order by " + mname + "_" + mname + "id desc");
        }
        else
        {
            viewQuery.Append(" order by " + mname + "_date desc");
        }
        DataTable dttbl = DbTable.ExecuteSelect(viewQuery.ToString());
        if (mname == "notificationrules")
        {
            mfullname = "Notification - " + DbTable.GetOneColumnData("tbl_finstationnotificationtype", "finstationnotificationtype_type", typeId);
        }
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append("<tr class='grid-ui-header'>");
        for (int i = 0; i < dttblcol.Rows.Count; i++)
        {
            string lbl = GlobalUtilities.ConvertToString(dttblcol.Rows[i]["columns_lbl"]);
            html.Append("<td>" + lbl + "</td>");
        }
        html.Append("</tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            html.Append("<tr class='" + css + "'>");
            for (int j = 0; j < dttblcol.Rows.Count; j++)
            {
                int id=GlobalUtilities.ConvertToInt(dttbl.Rows[i][moduleName+"_"+moduleName+"id"]);
                string colName = GlobalUtilities.ConvertToString(dttblcol.Rows[j]["columns_columnname"]);
                string lbl = GlobalUtilities.ConvertToString(dttblcol.Rows[j]["columns_lbl"]);
                string ddlcolumn = GlobalUtilities.ConvertToString(dttblcol.Rows[j]["columns_dropdowncolumn"]);
                string control = GlobalUtilities.ConvertToString(dttblcol.Rows[j]["columns_control"]);
                if (ddlcolumn != "") colName = ddlcolumn;
                string val = GlobalUtilities.ConvertToString(dttbl.Rows[i][colName]).Trim();
                if (control == "Date")
                {
                    val = GlobalUtilities.ConvertToDate(dttbl.Rows[i][colName]);
                }
                else if (control == "File Upload")
                {
                    if (val != "")
                    {
                        string filePath = "upload/" + moduleName + "/" + id + "." + val;
                        if (File.Exists(Server.MapPath("~/" + filePath)))
                        {
                            val = "<a href='" + filePath + "' target='_blank'>Download</a>";
                        }
                        else
                        {
                            val = "&nbsp;";
                        }
                    }
                }
                if (lbl == "Particular" && dttbl.Columns.Contains(moduleName + "_url"))
                {
                    string url = GlobalUtilities.ConvertToString(dttbl.Rows[i][moduleName+"_url"]);
                    if (url == "")
                    {
                        html.Append("<td>" + val + "</td>");
                    }
                    else
                    {
                        if (url.StartsWith("http://") || url.StartsWith("https://"))
                        {
                        }
                        else
                        {
                            if (!url.StartsWith("http://")) url = "http://" + url;
                        }
                        html.Append("<td><a href='"+url+"' target='_blank'>" + val + "</a></td>");
                    }
                }
                else if (colName == moduleName + "_url")
                {
                    if (val == "")
                    {
                        html.Append("<td>&nbsp;</td>");
                    }
                    else
                    {
                        string url = val;
                        if (val.StartsWith("http://") || val.StartsWith("https://"))
                        {
                        }
                        else
                        {
                            if (!val.StartsWith("http://")) url = "http://" + val;
                        }
                        html.Append("<td><a href='" + url + "' target='_blank'>" + val + "</a></td>");
                    }
                }
                else
                {
                    html.Append("<td>" + val + "</td>");
                }
            }
            html.Append("</tr>");
        }
        html.Append("</table>");
        //Literal ltdata = (Literal)this.FindControl("lbltitle");
        ltdata.Text = html.ToString();
    }
}
