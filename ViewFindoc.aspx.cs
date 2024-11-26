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
        query = @"select * from tbl_findocdocument
                 join tbl_findocdepartment on findocdepartment_findocdepartmentid=findocdocument_findocdepartmentid
                 join tbl_findoccategory On findoccategory_findoccategoryid=findocdocument_findoccategoryid
                 join tbl_findocsubcategory on findocsubcategory_findocsubcategoryid=findocdocument_findocsubcategoryid
                 join tbl_findocdocumenttype on findocdocumenttype_findocdocumenttypeid=findocdocument_findocdocumenttypeid
                 join tbl_clientuser on clientuser_clientuserid=findocdocument_clientuserid";
                
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();

        html.Append("<table width='100%' cellpadding=7  border=1 class='grid-ui'>");
        html.Append(@"<tr class='grid-ui-header'><td>Department Name</td><td>Category Name</td><td>Sub Category Name</td>
                      <td>Document Type</td><td>Upload By</td><td>Subject</td><td>Remarks</td></tr>");
        
       
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
           
          
            html.Append("<tr>");
            html.Append("<td>" + dttbl.Rows[i]["findocdepartment_departmentname"].ToString()+"</td>");
            html.Append("<td>" + dttbl.Rows[i]["findoccategory_categoryname"].ToString() + "</td>");
            html.Append("<td>" + dttbl.Rows[i]["findocsubcategory_subcategoryname"].ToString() + "</td>");
            html.Append("<td>" + dttbl.Rows[i]["findocdocumenttype_documenttype"].ToString() + "</td>");
            html.Append("<td>" + dttbl.Rows[i]["clientuser_name"].ToString() + "</td>");
            html.Append("<td>" + dttbl.Rows[i]["findocdocument_subject"].ToString() + "</td>");
            html.Append("<td>" + dttbl.Rows[i]["findocdocument_remarks"].ToString() + "</td>");
            html.Append("</tr>");
            
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
