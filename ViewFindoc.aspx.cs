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
        query = @"select * from tbl_findocdocument 
                left join tbl_findocdepartment on findocdepartment_findocdepartmentid=findocdocument_findocdepartmentid
              left join tbl_findoccategory On findoccategory_findoccategoryid=findocdocument_findoccategoryid
                left join tbl_findocsubcategory on findocsubcategory_findocsubcategoryid=findocdocument_findocsubcategoryid
                left join tbl_findocdocumenttype on findocdocumenttype_findocdocumenttypeid=findocdocument_findocdocumenttypeid
                left join tbl_clientuser on clientuser_clientuserid=findocdocument_clientuserid
                  where findocdocument_clientid="+clientId;
                
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();

        html.Append("<table width='100%' cellpadding=7  border=1 class='grid-ui'>");
        html.Append(@"<tr class='grid-ui-header'><td>Department Name</td><td>Category Name</td><td>Sub Category Name</td>
                      <td>Document Type</td><td>Upload By</td><td>Subject</td><td>Remarks</td><td>Edit</td></tr>");
        
       
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {

            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["findocdocument_findocdocumentid"]);

            html.Append("<tr>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdepartment_departmentname"])+"</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findoccategory_categoryname"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocsubcategory_subcategoryname"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdocumenttype_documenttype"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["clientuser_name"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdocument_subject"]) + "</td>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findocdocument_remarks"]) + "</td>");
            html.Append("<td><a href='addfindoc.aspx?id=" + id + "'>Detail</a></td>");
            html.Append("</tr>");
            
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
