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

public partial class HistoricalData_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_historicaldata", "historicaldataid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlcurrencyid, "tbl_currency", "currency_currency", "currency_currencyid", "", "currency_currency");
			EnableControlsOnEdit();
			//FillDropDown_END
            gblData.FillDropdown(ddlcurrencyid, "tbl_currency", "currency_currency", "currency_currencyid", "currency_currencyid>1", "currency_currency");

            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Historical Data";
        }
        else
        {
            lblPageTitle.Text = "Edit Historical Data";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_END
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
        double avg = 0; double change = 0;
        avg = (GlobalUtilities.ConvertToDouble(txtopen.Text) + GlobalUtilities.ConvertToDouble(txthigh.Text) +
                GlobalUtilities.ConvertToDouble(txtlow.Text) + GlobalUtilities.ConvertToDouble(txtclose.Text)) / 4;
        change = (GlobalUtilities.ConvertToDouble(txtopen.Text) - GlobalUtilities.ConvertToDouble(txtclose.Text)) / 
            GlobalUtilities.ConvertToDouble(txtopen.Text) * 100;
        txtaverage.Text = avg.ToString();
        txtchange.Text = change.ToString();
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
    
	protected void btnautocapture_Click(object sender, EventArgs e)
	{
        //insert usd inr data
        Hashtable hstbl = new Hashtable();
        InsertUpdate obj;
        string query = "select * from tbl_historicaldata where cast(historicaldata_date as date)=cast(getdate() as date)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        lblMessage.Visible = false;
        if (dttbl.Rows.Count == 4)
        {
            lblMessage.Text = "Already added all historical data for today!";
            lblMessage.Visible = true;
            return;
        }
        if (!IsAlreadyAdded(2))
        {
            hstbl.Add("date", "getdate()");
            AddHighLowData(hstbl, 4, 2);
            hstbl.Add("rbirefrate", Custom.GetLiveRate(1098));
            hstbl.Add("currencyid", 2);
            obj = new InsertUpdate();
            obj.InsertData(hstbl, "tbl_historicaldata");
        }
        if (!IsAlreadyAdded(3))
        {
            hstbl = new Hashtable();
            hstbl.Add("date", "getdate()");
            AddHighLowData(hstbl, 13, 3);
            hstbl.Add("rbirefrate", Custom.GetLiveRate(1100));
            hstbl.Add("currencyid", 3);
            obj = new InsertUpdate();
            obj.InsertData(hstbl, "tbl_historicaldata");
        }
        if (!IsAlreadyAdded(4))
        {
            hstbl = new Hashtable();
            hstbl.Add("date", "getdate()");
            AddHighLowData(hstbl, 22,4 );
            hstbl.Add("rbirefrate", Custom.GetLiveRate(1102));
            hstbl.Add("currencyid", 4);
            obj = new InsertUpdate();
            obj.InsertData(hstbl, "tbl_historicaldata");
        }
        if (!IsAlreadyAdded(5))
        {
            hstbl = new Hashtable();
            hstbl.Add("date", "getdate()");
            AddHighLowData(hstbl, 31, 5);
            hstbl.Add("rbirefrate", Custom.GetLiveRate(1104));
            hstbl.Add("currencyid", 5);
            obj = new InsertUpdate();
            obj.InsertData(hstbl, "tbl_historicaldata");
        }
        lblMessage.Text = "All historical data have been added for today!";
        lblMessage.Visible = true;
        return;
	}
    private void AddHighLowData(Hashtable hstbl, int startId, int currencyId)
    {
        double high = Custom.GetLiveRate(startId);
        hstbl.Add("high", high);
        double low = Custom.GetLiveRate(startId+1);
        hstbl.Add("low", low);
        double open = Custom.GetLiveRate(startId+2);
        hstbl.Add("open", open);
        double close = Custom.GetLiveRate(startId-3);
        hstbl.Add("close", close);

        double avg = 0; double change = 0;
        avg = (open + high + low + close ) / 4;
        change = (open - close) / open * 100;
        hstbl.Add("average", avg);
        hstbl.Add("change", change);
    }
    private bool IsAlreadyAdded(int currenyId)
    {
        string query = "select * from tbl_historicaldata where cast(historicaldata_date as date)=cast(getdate() as date) and historicaldata_currencyid="+currenyId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return false;
        return true;
    }
}
