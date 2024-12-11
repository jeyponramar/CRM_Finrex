using System;
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
using System.IO;

public partial class AddFindoc : System.Web.UI.Page
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
        int id = Common.GetQueryStringValue("id");
        if (id == 0) return;
        int clientId = Common.ClientId;
        tbluploadby.Visible = true;
        GlobalData objGlobalData = new GlobalData("tbl_findocdocument", "findocdocumentid");
        string query = "";
        query = @"select * from tbl_findocdocument 
                  left join tbl_findocdepartment on findocdepartment_findocdepartmentid=findocdocument_findocdepartmentid
                  left join tbl_findoccategory On findoccategory_findoccategoryid=findocdocument_findoccategoryid
                  left join tbl_findocsubcategory on findocsubcategory_findocsubcategoryid=findocdocument_findocsubcategoryid
                  left join tbl_findocdocumenttype on findocdocumenttype_findocdocumenttypeid=findocdocument_findocdocumenttypeid
                  left join tbl_clientuser on clientuser_clientuserid=findocdocument_clientuserid
                  where findocdocument_findocdocumentid=" + id +
                  " and findocdocument_clientid=" + clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            Response.End();
        }
        objGlobalData.PopulateForm(dr, form);
        string attachment = GlobalUtilities.ConvertToString(dr["findocdocument_attachment"]);
        string folderPath = "upload/client/" + clientId + "/findoc/" + id;
        mfuattachment.FolderPath = folderPath;
        mfuattachment.SetTempFolderPath(folderPath);
        Array arr = attachment.Split(',');
        ArrayList arrfiles = new ArrayList();
        for (int i = 0; i < arr.Length; i++)
        {
            string filePath = arr.GetValue(i).ToString();
            arrfiles.Add(filePath);
        }
        mfuattachment.BindMultiFiles(arrfiles, folderPath);
        btnDelete.Visible = true;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string attachment = mfuattachment.fileNames;
        if (attachment == "")
        {
            lblmessage.Text = "Please choose attachment.";
            lblmessage.Visible = true;
            return;
        }
        int clientId = Common.ClientId;
        int clientUserId = Common.ClientUserId;
        int id = Common.GetQueryStringValue("id");
        
        Hashtable hstbl = new Hashtable();
        hstbl.Add("findocdepartmentid", txtfindocdepartmentid.Text);
        hstbl.Add("findoccategoryid", txtfindoccategoryid.Text);
        hstbl.Add("findocsubcategoryid", txtfindocsubcategoryid.Text);
        hstbl.Add("findocdocumenttypeid", txtfindocdocumenttypeid.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid",clientUserId);
        hstbl.Add("subject", txtsubject.Text);
        hstbl.Add("remarks", txtremarks.Text);
        hstbl.Add("uploaddate", "getdate()");
        hstbl.Add("attachment", attachment);
        InsertUpdate obj = new InsertUpdate();
        int documentId = 0;
        if (id == 0)
        {
            documentId = obj.InsertData(hstbl, "tbl_findocdocument");
        }
        else
        {
            documentId = obj.UpdateData(hstbl, "tbl_findocdocument", id);
        }
        if (id == 0)
        {
            string folderPath = Server.MapPath("~/upload/client/" + clientId + "/findoc/" + documentId);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            Array arrfiles = attachment.Split(',');
            for (int i = 0; i < arrfiles.Length; i++)
            {
                string fileName = arrfiles.GetValue(i).ToString();
                string tempFilePath = Server.MapPath("~/upload/client/findoc/temp/" + fileName);
                string newFilePath = folderPath + "/" + fileName;
                File.Move(tempFilePath, newFilePath);
            }
        }
        if (documentId > 0)
        {
            Response.Redirect("~/viewfindoc.aspx");
        }
        else
        {
            lblmessage.Text = "Error occurred!";
            lblmessage.Visible = true;
        }

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string query = "";
        int id = Common.GetQueryStringValue("id");
        query = "delete from tbl_findocdocument where findocdocument_findocdocumentid=" + id + 
                " and findocdocument_clientid=" + Common.ClientId;
        DbTable.ExecuteQuery(query);
        Response.Redirect("~/viewfindoc.aspx");
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
