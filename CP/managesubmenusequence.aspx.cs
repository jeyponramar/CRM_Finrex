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

public partial class CP_managesubmenu : System.Web.UI.Page
{
    GlobalData globalData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            globalData.FillDropdown(ddlMenuId, "tbl_menu", "menu_menuname", "menu_menuid", "", "menu_menuname");            
            //globalData.FillListbox(lstSubmenus, "tbl_submenu", "submenuNAME");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblmessage.Visible = false;
        UpdateSubMenus();

        //lstSubmenus.DataSource =
    }
    private void UpdateSubMenus()
    {
        string Quey = "";
        string strtempitem = "";
        string system = "";
        int intSequence = 0;
        Array arrSubmenuId = hdnGridColumns.Text.Split(',');
        for (int i = 0; i < arrSubmenuId.Length; i++)
        {
            intSequence = i + 1;
            int SubmenuId = GlobalUtilities.ConvertToInt(arrSubmenuId.GetValue(i));
            Quey += "UPDATE tbl_submenu SET submenu_sequence=" + intSequence + " WHERE submenu_submenuid= " + SubmenuId + ";";
        }
        InsertUpdate obj = new InsertUpdate();
        bool ans = obj.ExecuteQuery(Quey);

        BindSubMenus(GlobalUtilities.ConvertToInt(hdnMenuId.Text));

        lblmessage.Text = "Submenu Sequence changed sucessfully";
        lblmessage.Visible = true;

    }
    protected void ddlMenuId_Changed(object sender, EventArgs e)
    {
        int MenuId = GlobalUtilities.ConvertToInt(ddlMenuId.SelectedValue);
        if (MenuId > 0)
        {
            BindSubMenus(MenuId);
            hdnMenuId.Text = MenuId.ToString();
        }
        else
        {
            lblmessage.Text = "Please select Menu";
            lblmessage.Visible = true;
            hdnMenuId.Text = "";
        }
    }
    private void BindSubMenus(int MenuId)
    {
        string Qurey = "SELECT * FROM tbl_submenu WHERE submenu_menuid=" + MenuId + " ORDER BY submenu_sequence";
        InsertUpdate obj = new InsertUpdate();
        DataTable dtSubMenus = new DataTable();
        dtSubMenus = obj.ExecuteSelect(Qurey);
        if (dtSubMenus.Rows.Count > 0)
        {
            lstSubmenus.DataValueField = "submenu_submenuid";
            lstSubmenus.DataTextField = "submenu_submenuname";
            lstSubmenus.DataSource = dtSubMenus;
            
            lstSubmenus.DataBind();
        }

    }
}
