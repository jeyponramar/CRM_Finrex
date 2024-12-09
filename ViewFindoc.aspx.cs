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
using System.Data;
using System.Text;
using System.IO;

public partial class ViewFindoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }

       
    }
   
    private void BindData()
    {
        string query = "";

        int clientId = Common.ClientId;
        StringBuilder viewQuery = new StringBuilder();
        viewQuery.Append(@"select * from tbl_findocdocument 
                left join tbl_findocdepartment on findocdepartment_findocdepartmentid=findocdocument_findocdepartmentid
                left join tbl_findoccategory On findoccategory_findoccategoryid=findocdocument_findoccategoryid
                left join tbl_findocsubcategory on findocsubcategory_findocsubcategoryid=findocdocument_findocsubcategoryid
                left join tbl_findocdocumenttype on findocdocumenttype_findocdocumenttypeid=findocdocument_findocdocumenttypeid
                left join tbl_clientuser on clientuser_clientuserid=findocdocument_clientuserid
                where findocdocument_clientid=" + clientId);
        Hashtable hstblp = new Hashtable();
        int findocdepartmentid = GlobalUtilities.ConvertToInt(txtfindocdepartmentid.Text);
        int findocsubcategoryid = GlobalUtilities.ConvertToInt(txtfindocsubcategoryid.Text);
        int findoccategoryid = GlobalUtilities.ConvertToInt(txtfindoccategoryid.Text);

        if (txtfromdate.Text != "")
        {
            viewQuery.Append(" AND cast(findocdocument_uploaddate as date)>=cast(@fromdate as date)");
            hstblp.Add("fromdate", GlobalUtilities.ConvertMMDateToDD(txtfromdate.Text));
        }
        if (txtfromdate.Text != "")
        {
            viewQuery.Append(" AND cast(findocdocument_uploaddate as date)<=cast(@todate as date)");
            hstblp.Add("todate", GlobalUtilities.ConvertMMDateToDD(txttodate.Text));
        }
        if (findocdepartmentid != 0) viewQuery.Append(" and findocdocument_findocdepartmentid = " + findocdepartmentid);
        if (findocsubcategoryid != 0) viewQuery.Append(" and findocdocument_findocsubcategoryid = " + findocsubcategoryid);
        if (findoccategoryid != 0) viewQuery.Append(" and findocdocument_findoccategoryid = " + findoccategoryid);
        
        viewQuery.Append(" order by findocdocument_findocdocumentid desc");
        
        DataTable dttbl = DbTable.ExecuteSelect(viewQuery.ToString());
        StringBuilder html = new StringBuilder();

        html.Append("<table width='100%' cellpadding=7  border=1 class='grid-ui'>");
        html.Append(@"<tr class='grid-ui-header'><td>Uploaded Date</td><td>Subject</td><td>Uploaded Document</td><td>Department Name</td><td>Category Name</td>
                      <td>Sub Category Name</td><td>Document Type</td><td>Upload By</td><td>Remarks</td><td>Edit</td></tr>");
        
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["findocdocument_findocdocumentid"]);
            string uploadDate = GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["findocdocument_uploaddate"]);
            string attachment = GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdocument_attachment"]);
            StringBuilder documentHtml = new StringBuilder();
            Array arrfiles = attachment.Split(',');
            if (attachment != "")
            {
                documentHtml.Append("<ul class='file-list-ul'>");
                for (int j = 0; j < arrfiles.Length; j++)
                {
                    string folderPath = Server.MapPath("~/upload/client/" + clientId + "/findoc/" + id);
                    string fileName = arrfiles.GetValue(j).ToString();
                    if (File.Exists(folderPath + "/" + fileName))
                    {
                        string acualFileName = Common.GetFileName(fileName);
                        documentHtml.Append("<li><a href='upload/client/" + clientId + "/findoc/" + id + "/" + fileName + "' target='_blank'>" + acualFileName + "</a></li>");
                    }
                }
                documentHtml.Append("</ul>");
            }
            html.Append("<tr>");

            html.Append("<td>" + uploadDate.ToString() + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdocument_subject"]) + "</td>");
            html.Append("<td>" + documentHtml.ToString() + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdepartment_departmentname"])+"</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findoccategory_categoryname"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocsubcategory_subcategoryname"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdocumenttype_documenttype"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["clientuser_name"]) + "</td>");
            //html.Append("<td>" + uploadDate + "</td>");
            
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdocument_remarks"]) + "</td>");
            html.Append("<td><a href='addfindoc.aspx?id=" + id + "'>Detail</a></td>");
            html.Append("</tr>");
            
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    
}
