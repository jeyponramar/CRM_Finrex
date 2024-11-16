using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;

public partial class viewmodule : System.Web.UI.Page
{
    GlobalData gbl = new GlobalData("tbl_submenu");
    protected void Page_Load(object sender, EventArgs e)
    {
        lblTitle.Text = CustomSession.Session("S_ProjectName") + " - Main Menu";
        gridData._DataProvider = (DataProvider)new SubMenuBL();
        if (!IsPostBack)
        {
            gbl.FillDropdown(ddlMenuId, "tbl_menu", "menu_menuname", "menu_menuid", "", "menu_menuname");
            gridData.SearchBy("Search By", "");
            gridData.SearchBy("Sub Menu", "submenu_submenuname");
            gridData.SearchBy("Main Menu", "menu_menuname");
            gridData.BindData();
        }
    }
    protected void ddlMenuId_Changed(object s, EventArgs e)
    {
        //if (ddlMenuId.SelectedIndex == 0) return;
        //string query = "select top 1 * from tbl_submenu where submenu_menuid=" + ddlMenuId.SelectedValue + " order by submenu_sequence desc";
        //InsertUpdate obj = new InsertUpdate();
        //DataRow dr = obj.ExecuteSelectRow(query);
        //if (dr == null)
        //{
        //    txtSequence.Text = "1";
        //}
        //else
        //{
        //    txtSequence.Text = Convert.ToString(GlobalUtilities.ConvertToInt(dr["submenu_sequence"]) + 1);
        //}
    }
    protected void btnSave_Click(object s, EventArgs e)
    {
        Common.CheckDemoVersion();

        if (txtSubMenuName.Text.Trim() == "")
        {
            lblMessage.Text = "Please enter sub menu name";
            return;
        }
        if (GlobalUtilities.ConvertToInt(ddlMenuId.SelectedValue) == 0)
        {
            lblMessage.Text = "Please select main menu.";
            return;
        }
        string query = "update tbl_submenu set submenu_sequence=submenu_sequence+1 where " +
                     "submenu_menuid=" + ddlMenuId.SelectedValue + " and submenu_sequence>=" + GlobalUtilities.ConvertToInt(txtSequence.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);

        gbl.SaveForm(plmenu,Convert.ToInt32(h_SubMenuId.Text));
        gridData.BindData();
        lblMessage.Text = "Menu saved successfully";
        txtSubMenuName.Text = "";
        txtSequence.Text = "";
        txtURL.Text = "";
        h_SubMenuId.Text = "0";
        ddlMenuId.SelectedIndex = 0;
        
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int id = GlobalUtilities.ConvertToInt(h_SubMenuId.Text);
        string query = "delete from tbl_submenu where submenu_submenuid=" + id;
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);
        lblMessage.Text = "Sub menu deleted successfully";
        gridData.BindData();
        txtSubMenuName.Text = "";
        txtSequence.Text = "";
        txtURL.Text = "";
        h_SubMenuId.Text = "0";
        ddlMenuId.SelectedIndex = 0;
    }
}
