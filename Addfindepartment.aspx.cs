﻿using System;
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
public partial class Addfindepartment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateData();
        }
    }
    private void PopulateData()
    {
        GlobalData objGlobalData = new GlobalData("tbl_findocdepartment", "findocdepartmentid");
        objGlobalData.PopulateForm(form);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
       
      
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        Hashtable hstbl = new Hashtable();
        hstbl.Add("departmentname", departmentname.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        InsertUpdate obj = new InsertUpdate();
        int findocdepartmentId = obj.InsertData(hstbl, "tbl_findocdepartment");
        if (findocdepartmentId > 0)
        {
            lblmessage.Text = "Data saved successfully";
        }


    }

}
