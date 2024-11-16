using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class AMC_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
        grid.setGridData += new SetGridData(grid_setGridData);
        grid.GridQuery = "select * from tbl_amc join tbl_city on city_cityid=amc_cityid left join tbl_client on client_clientid=amc_clientid left join tbl_amctype on amctype_amctypeid=amc_amctypeid left join tbl_area on area_areaid=amc_areaid left join tbl_amcrenewtype on amcrenewtype_amcrenewtypeid=amc_amcrenewtypeid";
    }
    public void grid_setGridData(int start, int end, string SortBy, bool IsSearch)
    {
        string query = "select * from tbl_amc join tbl_city on city_cityid=amc_cityid left join tbl_client on client_clientid=amc_clientid left join tbl_amctype on amctype_amctypeid=amc_amctypeid left join tbl_area on area_areaid=amc_areaid left join tbl_amcrenewtype on amcrenewtype_amcrenewtypeid=amc_amcrenewtypeid";
        InsertUpdate obj = new InsertUpdate();
        grid.Data = obj.ExecuteSelect(query);
        grid.gridSettings.TotalRecords = 10;
    }
    
}