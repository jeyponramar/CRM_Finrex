using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Collections;
using System.Text;

public partial class utilities_delete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId")) != 1)
            {
                if (!Common.IsDeleteRights())
                {
                    lblMessage.Text = "You do not have rights to perform this operation, please contact administrator.";
                    btnDelete.Visible = false;
                    return;
                }
            }
            BindRelatedModules();
        }
    }
    
    private void BindRelatedModules()
    {
        bool isrelatedDataFound = false;
        ltRelatedModules.Text = DeleteModule.BindRelatedModules(ref isrelatedDataFound);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        bool isrelatedDataFound = false;
        ltRelatedModules.Text = DeleteModule.BindRelatedModules(ref isrelatedDataFound);
        if (isrelatedDataFound) return;

        string error = "";
        bool isdeleted = DeleteModule.Delete(ref error);
        if (error == "")
        {
            btnDelete.Visible = false;
            btnCancel.Visible = false;
            btnOK.Visible = true;
            ltRelatedModules.Text = "";
            lblMessage.Text = "Data has been deleted successfully!";
        }
        else
        {
            lblMessage.Text = error;
        }

        //string m = Request.QueryString["m"];
        //m = m.Replace(" ", "").Trim().ToLower();
        //string query = "";
        //query = "select * from tbl_module WHERE REPLACE(module_modulename,' ','')='" + Request.QueryString["m"] + "'";
        //DataRow drModule = DbTable.ExecuteSelectRow(query);
        //if (drModule == null)
        //{
        //    lblMessage.Text = "Error occurred while processing your request! Error Code(180).";
        //    btnDelete.Visible = false;
        //    return;
        //}
        //int id = Common.GetQueryStringValue("id");
        //DataTable dttblRelatedModules = GetRelatedModules(m);

        //for (int i = 0; i < dttblRelatedModules.Rows.Count; i++)
        //{
        //    string subModule = GlobalUtilities.ConvertToString(dttblRelatedModules.Rows[i]["module_modulename"]).Replace(" ", "").ToLower().Trim();
        //    int subModuleId = GlobalUtilities.ConvertToInt(dttblRelatedModules.Rows[i]["module_moduleid"]);
        //    //delete sub grid modules
        //    query = "select * from tbl_columns WHERE columns_moduleid=" + subModuleId + " AND columns_control='Sub Grid'";
        //    DataTable dttblSubGrid = DbTable.ExecuteSelect(query);
        //    if (GlobalUtilities.IsValidaTable(dttblSubGrid))
        //    {
        //        for (int j = 0; j < dttblSubGrid.Rows.Count; j++)
        //        {
        //            string subSubModule = GlobalUtilities.ConvertToString(dttblSubGrid.Rows[j]["columns_columnname"]).Replace(" ", "").ToLower().Trim();
        //            Array arr = subSubModule.Split('_');
        //            subSubModule = Convert.ToString(arr.GetValue(1));
        //            query = "delete from tbl_" + subSubModule + " WHERE " + subSubModule + "_" + subModule + "id " +
        //                    "IN(select " + subModule + "_" + subModule + "id from tbl_" + subModule +
        //                        " JOIN tbl_" + m + " ON " + m + "_" + m + "id=" + subModule + "_" + m + "id WHERE " + m + "_" + m + "id" + "=" + id + ")";
        //            DbTable.ExecuteQuery(query);

        //        }
        //    }
        //    DeleteFromCommonModules(m, subModule, id);

        //    query = "delete from tbl_" + subModule + " WHERE " + subModule + "_" + m + "id=" + id;
        //    DbTable.ExecuteQuery(query);

        //}

        //int mainModuleId = GlobalUtilities.ConvertToInt(drModule["module_moduleid"]);
        //query = "select * from tbl_columns WHERE columns_moduleid=" + mainModuleId + " AND columns_control='Sub Grid'";
        //DataTable dttblSubGrid1 = DbTable.ExecuteSelect(query);
        //if (GlobalUtilities.IsValidaTable(dttblSubGrid1))
        //{
        //    for (int j = 0; j < dttblSubGrid1.Rows.Count; j++)
        //    {
        //        string subSubModule = GlobalUtilities.ConvertToString(dttblSubGrid1.Rows[j]["columns_columnname"]).Replace(" ", "").ToLower().Trim();
        //        Array arr = subSubModule.Split('_');
        //        subSubModule = Convert.ToString(arr.GetValue(1));
        //        query = "delete from tbl_" + subSubModule + " WHERE " + subSubModule + "_" + m + "id=" + id;
        //        DbTable.ExecuteQuery(query);
        //    }
        //}
        ////delete from accounts
        //Accounts.DeleteVoucher();

        ////delete stock
        //Stock.Delete();

        ////delete serial nos
        //Stock.DeleteSerialNos();

        ////custom delete here
        //CustomUpdate(m, id);

        ////delete main table
        //query = "delete from tbl_" + m + " WHERE " + m + "_" + m + "id=" + id;
        //DbTable.ExecuteQuery(query);

        //btnDelete.Visible = false;
        //btnCancel.Visible = false;
        //btnOK.Visible = true;
        //ltRelatedModules.Text = "";
        //lblMessage.Text = "Data has been deleted successfully!";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/" + Request.QueryString["url"]);
    }
    //custom delete
    private void CustomUpdate(string m, int mid)
    {
        if (m == "stockin")
        {
            DataRow dr = DbTable.GetOneRow("tbl_stockin", mid);
            int purchaseOrderId = GlobalUtilities.ConvertToInt(dr["stockin_purchaseorderid"]);
            Common.UpdateParentQuantityByProduct("stockin", "stockindetail", "purchaseorder", "purchaseorderdetail", "receivedquantity",
                            "quantity", purchaseOrderId);
            Common.UpdateParentQuantity("purchaseorder", "purchaseorderdetail", "receivedquantity", "receivedquantity", purchaseOrderId);
        }
    }
    //private void DeleteFromCommonModules(string module, string submodule, int mid)
    //{
    //    string query = "select " + submodule + "_" + submodule + "id from tbl_" + submodule + " where " + submodule + "_" + module + "id=" + mid;
    //    DataTable dtttbl = DbTable.ExecuteSelect(query);
    //    if (!GlobalUtilities.IsValidaTable(dtttbl)) return;

    //    StringBuilder ids = new StringBuilder();

    //    for (int i = 0; i < dtttbl.Rows.Count; i++)
    //    {
    //        if (i == 0)
    //        {
    //            ids.Append(dtttbl.Rows[i][0]);
    //        }
    //        else
    //        {
    //            ids.Append("," + dtttbl.Rows[i][0]);
    //        }
    //    }
    //    //delete from accounts
    //    Accounts.DeleteVoucher(submodule, ids.ToString());

    //    //delete stock
    //    Stock.Delete(submodule, ids.ToString());

    //    //delete serial nos
    //    Stock.DeleteSerialNos(submodule, ids.ToString());
    //}
}
