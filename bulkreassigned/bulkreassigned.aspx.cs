using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;

public partial class bulk_reassigned : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_telecalling", "telecallingid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "Transfer Allocation";
            AssignAc();
        }
    }
    protected void btnAllocateTask_Click(object sender, EventArgs e)
    {
        lblmessage.Visible = false;
        if(GlobalUtilities.ConvertToInt(txttopersonid.Text)==GlobalUtilities.ConvertToInt(txtfrmpersonid.Text))
        {
            lblmessage.Text="From person and to person should be different";
            lblmessage.Visible =true;
            return;
        }
        if (GlobalUtilities.ConvertToInt(txttopersonid.Text) > 0 && GlobalUtilities.ConvertToInt(txtfrmpersonid.Text) > 0)
        {
            int ToAssignedId = GlobalUtilities.ConvertToInt(txttopersonid.Text);
            string ModuleName = GlobalUtilities.ConvertToString(Request.QueryString["mn"]);
            string StatusModule = GlobalUtilities.ConvertToString(Request.QueryString["sm"]);
            string StatusId = GlobalUtilities.ConvertToString(Request.QueryString["sid"]);
            string ModuleStatusIdentityColumn = ModuleName + "_" + StatusModule + "id";
            string StatusIdentityColumn = StatusModule + "_" + StatusModule + "id "; 
            string AcModuleName = GlobalUtilities.ConvertToString(ViewState["AcModuleName"]);
            string datecol = GlobalUtilities.ConvertToString(Request.QueryString["datecol"]);
            string ExtraWhere="";
            string strFromdate = GlobalUtilities.ConvertToString(ViewState["FromDate"]);
            string strTodate = GlobalUtilities.ConvertToString(ViewState["ToDate"]);
            if (strFromdate != "" && strTodate != "")
            {
                ExtraWhere = " AND " + ModuleName + "_" + datecol + " BETWEEN '" + GlobalUtilities.ConvertToSqlMinDateFormat(strFromdate) + "' AND '" + GlobalUtilities.ConvertToSqlMaxDateFormat(strTodate) + "'";
            }
            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                string KeyName = Request.Form.Keys[i];
                if (KeyName.StartsWith("chk_"))
                {
                    int keyValue = GlobalUtilities.ConvertToInt(Request.Form[KeyName.Replace("chk_","txt_")]);
                    int statusid = GlobalUtilities.ConvertToInt(KeyName.Replace("chk_", ""));
                    //bind All Data
                    string strQuery = "SELECT TOP (" + keyValue + ")* FROM tbl_" + ModuleName +
                     " WHERE " + ModuleStatusIdentityColumn + "=" + statusid + " AND " + ModuleName + "_" + AcModuleName + "id=" + txtfrmpersonid.Text+ExtraWhere;
                    DataTable dt = DbTable.ExecuteSelect(strQuery);
                    if (GlobalUtilities.IsValidaTable(dt))
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            int id = GlobalUtilities.ConvertToInt(dt.Rows[j][ModuleName + "_" + ModuleName + "id"]);
                            string strquery = "UPDATE tbl_" + ModuleName + " SET " + ModuleName + "_" + AcModuleName + "id=" + txttopersonid.Text + " WHERE " + ModuleName + "_" + ModuleName + "id=" + id;
                            if (!DbTable.ExecuteQuery(strquery))
                            {
                                lblmessage.Text = "Error Occured";
                                lblmessage.Visible = true;
                                break;
                            }
                        }
                    }
                    //End
                }
            }
            lblmessage.Text = "data allocated sucessfully!!";
            lblmessage.Visible = true;
            ltReAssignedData.Text = "";
            ltReAssignedData.Visible = false;
            tdReAssignedData.Visible = false;
        }
        else
        {            
            lblmessage.Text = "Please select from to value";
            lblmessage.Visible = true;
        }

    }
    private void AssignAc()
    {        
        Array arrAcModuleName = GlobalUtilities.ConvertToString(Request.QueryString["acm"]).Split(';');
        string AcColumnName = "";
        string AcDcnName = "";
        string AcModuleName = "";
        for (int i = 0; i < arrAcModuleName.Length; i++)
        {
            //dcn="marketingperson_maketingpersonname"  m="marketingperson" cn="maketingpersonname" 
            string val = GlobalUtilities.ConvertToString(arrAcModuleName.GetValue(i));
            if (val != "")
            {
                if (val.StartsWith("m="))
                {
                    AcModuleName = val.Replace("m=", "");
                    Fromperson.Attributes.Add("m", AcModuleName);
                    ToPerson.Attributes.Add("m", AcModuleName);
                }
                else if (val.StartsWith("cn="))
                {
                    AcColumnName = val.Replace("cn=", "");
                    Fromperson.Attributes.Add("cn", AcColumnName);
                    ToPerson.Attributes.Add("cn", AcColumnName);
                }
            }
        }
        ViewState["accolumn"] = arrAcModuleName;
        AcDcnName = AcModuleName + "_" + AcColumnName;
        Fromperson.Attributes.Add("dcn", AcDcnName);
        ToPerson.Attributes.Add("dcn", AcDcnName);
        ViewState["AcModuleName"] = AcModuleName;
    }
    protected void btnGetAllAssignedData_Click(object sender, EventArgs e)
    {
        //in Query String AcColumn='m=abc;statusid=1,2,23;dcn=status'
        //Go
        lblmessage.Visible = false;
        string ModuleName = GlobalUtilities.ConvertToString(Request.QueryString["mn"]);
        string StatusModule = GlobalUtilities.ConvertToString(Request.QueryString["sm"]);
        string StatusId = GlobalUtilities.ConvertToString(Request.QueryString["sid"]);
        string AcModuleName = GlobalUtilities.ConvertToString(ViewState["AcModuleName"]);
        string datecol = GlobalUtilities.ConvertToString(Request.QueryString["datecol"]);  
        string ModuleStatusIdentityColumn= ModuleName + "_" + StatusModule + "id";
        string StatusIdentityColumn =  StatusModule + "_" + StatusModule + "id ";
        string ExtraWhere=(StatusId!="")? " AND " + ModuleStatusIdentityColumn + " IN (" + StatusId + ")":"";
        ViewState["FromDate"] = from_date.Text;
        ViewState["ToDate"] = to_date.Text;
        if (from_date.Text != "" && to_date.Text != "")
        {
            ExtraWhere += " AND " + ModuleName + "_" + datecol + " BETWEEN '" + GlobalUtilities.ConvertToSqlMinDateFormat(from_date.Text) + "' AND '" + GlobalUtilities.ConvertToSqlMaxDateFormat(to_date.Text) + "'";
        }
        string Query="";
        Query = "SELECT COUNT(*)AS Total,"+ModuleStatusIdentityColumn+",MIN("+StatusModule+"_status)AS Status FROM tbl_" + ModuleName + " JOIN tbl_" + StatusModule + " ON " + ModuleStatusIdentityColumn + "=" + StatusIdentityColumn +
                " WHERE "+ModuleName+"_"+AcModuleName+"id="+txtfrmpersonid.Text+ExtraWhere+
                " GROUP BY "+ModuleStatusIdentityColumn;

        DataTable dtAssignedData = DbTable.ExecuteSelect(Query);
        if (GlobalUtilities.IsValidaTable(dtAssignedData))
        {
            StringBuilder htmlheader = new StringBuilder();
            StringBuilder htmldata = new StringBuilder();
            int DivCount = 3;

            for (int j = 0; j < dtAssignedData.Rows.Count; j++)
            {
                string rcls = "repeater-alt";
                if (j % 2 == 0)
                {
                    rcls = "repeater-row";
                    //ISreportExists = true;
                }
                string status = GlobalUtilities.ConvertToString(dtAssignedData.Rows[j]["Status"]);
                int Count = GlobalUtilities.ConvertToInt(dtAssignedData.Rows[j]["Total"]);
                int statusid = GlobalUtilities.ConvertToInt(dtAssignedData.Rows[j][ModuleStatusIdentityColumn]);//<label class='label'>Total Count:"+Count+"</label>
                htmlheader.Append("<tr class='" + rcls + "'><td style='width:300px'><span style='font-weight:bold;font-size:16px;'> " + status + " </span>&nbsp;&nbsp;<span class='error' style='font-weight:bold;font-size:20px;'> (" + Count + ")</span><input statusname=" + status + " totalstatuscount='" + Count + "' type='checkbox' checked='checked' id='" + statusid + "' class='chkreassigndata' name='chk_" + statusid + "' /></td><td style='width:100px'><input type='text' class='textbox txt_" + status + "' style='width:50px;' value='" + Count + "' totalstatus_count='" + Count + "' name='txt_" + statusid + "' </td></tr>");
                //bind All Data
                //string strQuery = "SELECT * FROM tbl_" + ModuleName + " JOIN tbl_" + StatusModule + " ON " + ModuleStatusIdentityColumn + "=" + StatusIdentityColumn +
                // " WHERE " + ModuleStatusIdentityColumn + "=" + statusid + " AND " + ModuleName + "_" + AcModuleName + "id=" + txtfrmpersonid.Text;
                //DataTable dt = DbTable.ExecuteSelect(strQuery);
                //if (GlobalUtilities.IsValidaTable(dt))
                //{
                //    htmldata.Append("<tr>");
                //    for (int k = 0; k < dt.Rows.Count; k++)
                //    {
                //        htmldata.Append("<td><input type='checkbox' id='' name='chk_' /><a href='../enquiry/add.aspx' target='_blank'>ghgdgf</a></td>");
                //    }
                //    htmldata.Append("</tr>");
                //}
                //End
            }
            ltReAssignedData.Text = htmlheader.ToString();
            ltReAssignedData.Visible = true;
            tdReAssignedData.Visible = true;
        }
        else
        {
            lblmessage.Text = "No data found!!"; ;
            lblmessage.Visible = true;
            ltReAssignedData.Text = "<div class='error'>--No Data Found--</div>";
            ltReAssignedData.Visible = true;
            tdReAssignedData.Visible = true;
        }
    }
    
}
