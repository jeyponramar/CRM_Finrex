using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.IO;

public partial class replace : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string qry = "select * from tbl_module";
        //DataTable dttbl = DbTable.ExecuteSelect(qry);
        //for(int i=0;i<dttbl.Rows.Count;i++)
        //{
        //    CP objCp = new CP();
        //    int moduleid = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["module_moduleid"]);
        //    string modulename = GlobalUtilities.ConvertToString(dttbl.Rows[i]["module_modulename"]);
        //    DataRow dr = dttbl.Rows[i];
        //    objCp.GenerateAddDesignPageV2_0(dr, moduleid, true);
        //    objCp.GenerateViewPage(dr, moduleid, true);
        //}

        //Replace(Server.MapPath("~/quotationfor/add.aspx.cs"));
        //return;

        DirectoryInfo dir1 = new DirectoryInfo(Server.MapPath("~/"));
        foreach (DirectoryInfo dir in dir1.GetDirectories())
        {
            string filePath = dir.FullName + "/add.aspx.cs";
            if (File.Exists(filePath))
            {
                Replace(filePath);
            }
        }
    }
    private void Replace(string filePath)
    {
        string code = GlobalUtilities.ReadFile(filePath);
        int startIndex = code.IndexOf("btnSaveAndView_Click");
        if (startIndex > 0)
        {
            startIndex = code.IndexOf("SaveData(false);", startIndex);
            if (startIndex > 0)
            {
                code = code.Remove(startIndex, 16);
                code = code.Insert(startIndex, "if (SaveData(false) > 0)\n\t\t{");
                startIndex = code.IndexOf("Response.Redirect", startIndex);
                code = code.Insert(startIndex, "\t");
                startIndex = code.IndexOf(";", startIndex);
                code = code.Insert(startIndex + 1, "\n\t\t}");

                GlobalUtilities.WriteFile(filePath, code);
            }
        }
    }
}
