using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;

public partial class viewmodule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblTitle.Text = CustomSession.Session("S_ProjectName") + " - Main Menu";
        gridData._DataProvider = (DataProvider)new MenuBL();
        if (!IsPostBack)
        {
            gridData.SearchBy("Search By", "");
            gridData.SearchBy("Menu", "menu_menuname");
            gridData.BindData();
        }
    }
    protected void btnSave_Click(object s, EventArgs e)
    {
        Common.CheckDemoVersion();
        
        if (txtMenuName.Text.Trim() == "")
        {
            lblMessage.Text = "Please enter menu name";
            return;
        }
        string query = "if exists(select * from tbl_menu where menu_sequence=" + GlobalUtilities.ConvertToInt(txtSequence.Text) + ")" +
                                  "update tbl_menu set menu_sequence=menu_sequence+1 where menu_sequence>=" + GlobalUtilities.ConvertToInt(txtSequence.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);

        GlobalData gbl = new GlobalData("tbl_menu");
        int id = gbl.SaveForm(plmenu,Convert.ToInt32(h_MenuId.Text));
        gridData.BindData();
        lblMessage.Text = "Menu saved successfully";
        txtMenuName.Text = "";
        txtSequence.Text = "";
        txtURL.Text = "";
        h_MenuId.Text = "0";
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int id = GlobalUtilities.ConvertToInt(h_MenuId.Text);
        string query = "delete from tbl_menu where menu_menuid=" + id;
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);
        lblMessage.Text = "Menu deleted successfully";
        gridData.BindData();
        txtMenuName.Text = "";
        txtSequence.Text = "";
        txtURL.Text = "";
        h_MenuId.Text = "0";
    }
}
