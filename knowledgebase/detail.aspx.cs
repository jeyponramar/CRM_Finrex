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
using System.Text;
using System.IO;

public partial class knowledgebase_detail : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_knowledgebase", "knowledgebaseid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
             lblPageTitle.Text = "Knowledge Base Detail";
            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");

            //KnowledgeBaseDAO dao = new KnowledgeBaseDAO();
            int knowledgebaseid = Convert.ToInt32(Request.QueryString["id"]);
            //DataRow dr = dao.GetKnowledgeBaseDetail(knowledgebaseid);
            string query = "select * from tbl_knowledgebase where knowledgebase_knowledgebaseid="+knowledgebaseid;
            InsertUpdate obj = new InsertUpdate();
            DataRow dr = obj.ExecuteSelectRow(query);

            gblData.PopulateForm(dr,form);
            if (dr != null)
            {
                DateTime dt = Convert.ToDateTime(dr["knowledgebase_createddate"]);
                string createddate = dt.ToString("dd MMMM yyyy");
                lblcreateddate.Text = createddate;
                lbldescription.Text = Convert.ToString(dr["knowledgebase_description"]);
            }

            //files to display

            string folderpath = "../upload/knowledgebase/" + knowledgebaseid;
            StringBuilder htmlfiles = new StringBuilder();
            if (Directory.Exists(Server.MapPath(folderpath)))
            {
                string[] filePaths = Directory.GetFiles(Server.MapPath(folderpath));
                htmlfiles.Append("<table>");
                for (int j = 0; j < filePaths.Length; j++)
                {
                    string path = filePaths[j];
                    string filename = Path.GetFileName(path);

                    string fullfilename = folderpath + "/" + filename;
                    string href = "<a href='" + fullfilename + "' target='_blank'>" + filename + "</a>";
                    htmlfiles.Append("<tr><td>" + href + "</td></tr>");
                }
                htmlfiles.Append("</table>");
            }
            ltfiles.Text = htmlfiles.ToString();
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        Response.Redirect("add.aspx?id=" + Convert.ToInt32(Request.QueryString["id"]));
    }
}
